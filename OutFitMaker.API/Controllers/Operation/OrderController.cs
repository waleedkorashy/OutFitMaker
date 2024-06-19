using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using OutFitMaker.DataAccess.DbContext;
using OutFitMaker.Domain.Constants.Enums;
using OutFitMaker.Domain.DTOs.Operation;
using OutFitMaker.Domain.Interfaces.Operation;
using OutFitMaker.Domain.Models.Security;
using OutFitMaker.Services.IServices.Security;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace OutFitMaker.API.Controllers.Operation
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {

        #region Properties and constructors
        private IUserRepo _userRepo { get; }
        private readonly OutFitMakerDbContext _context;

        private IOrderServices _orderServices { get; }
        private readonly HttpClient _httpClient;


        public OrderController(IUserRepo userRepo,
            IOrderServices orderServices, IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
            
            _userRepo = userRepo;
            _orderServices = orderServices;
            
        }

        #endregion



        [HttpPost("predict")]
        public async Task<IActionResult> predict([FromBody] dynamic inputData)
        {
            try
            {
                if (inputData.ValueKind == JsonValueKind.Undefined)
                {
                    return BadRequest("No input data provided.");
                }

                // Convert the input data to JSON string
                string jsonInput = inputData.ToString();

                // Print the content of the input data
                Console.WriteLine("Received input data:");
                Console.WriteLine(jsonInput);

                // Make a POST request to the Flask API endpoint
                using (var httpClient = new HttpClient())
                {
                    var content = new StringContent(jsonInput, Encoding.UTF8, "application/json");
                    var response = await httpClient.PostAsync("http://localhost:5000/predict", content);

                    if (response.IsSuccessStatusCode)
                    {
                        string jsonResponse = await response.Content.ReadAsStringAsync();
                        return Ok(jsonResponse); // Return the response from Flask API

                    }
                    else
                    {
                        string errorResponse = await response.Content.ReadAsStringAsync();
                        return StatusCode((int)response.StatusCode, errorResponse); // Return error response from Flask API
                    }
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message); // Return internal server error if an exception occurs
            }
        }

        //[HttpPost("recommend")]
        //public async Task<IActionResult> UploadViaProxy([FromForm] IFormFile file)
        //{
        //    if (file == null || file.Length == 0)
        //    {
        //        return BadRequest("No file uploaded.");
        //    }

        //    using (var stream = new MemoryStream())
        //    {
        //        await file.CopyToAsync(stream);
        //        stream.Position = 0; // Reset stream position to beginning

        //        var requestContent = new MultipartFormDataContent();
        //        var fileContent = new ByteArrayContent(stream.ToArray());
        //        fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
        //        {
        //            Name = "file",
        //            FileName = file.FileName
        //        };
        //        fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);

        //        requestContent.Add(fileContent);

        //        var response = await _httpClient.PostAsync("http://127.0.0.1:5000/recommend", requestContent);

        //        if (response.IsSuccessStatusCode)
        //        {
        //            var responseBody = await response.Content.ReadAsStringAsync();
        //            //return Ok(new { message = "File uploaded successfully to Server B.", serverBResponse = responseBody });
        //            return StatusCode((int)response.StatusCode, responseBody);
        //        }

        //        return StatusCode((int)response.StatusCode, new { message = "Failed to upload file to Server B.", serverBResponse = await response.Content.ReadAsStringAsync() });
        //    }
        //}



        [HttpPost("recommend")]
        public async Task<IActionResult> UploadViaProxy([FromForm] IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                return BadRequest("No file uploaded.");
            }
            

            try
            {
                using (var stream = new MemoryStream())
                {
                    await file.CopyToAsync(stream);
                    stream.Position = 0; // Reset stream position to beginning

                    var requestContent = new MultipartFormDataContent();
                    var fileContent = new ByteArrayContent(stream.ToArray());
                    fileContent.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("form-data")
                    {
                        Name = "file",
                        FileName = file.FileName
                    };
                    fileContent.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(file.ContentType);

                    requestContent.Add(fileContent);

                    var response = await _httpClient.PostAsync("http://127.0.0.1:5000/recommend", requestContent);

                    if (response.IsSuccessStatusCode)
                    {
                        var responseBody = await response.Content.ReadAsStringAsync();

                        // Deserialize the response based on its structure
                        JObject jsonResponse = JObject.Parse(responseBody);
                        var recommendedImages = jsonResponse["recommended_images"];

                        if (recommendedImages != null)
                        {
                            // Assuming recommended_images is an array of strings
                            List<string> recommendedImageUrls = recommendedImages.ToObject<List<string>>();

                            if (recommendedImageUrls != null && recommendedImageUrls.Count > 0)
                            {
                                //var recommendedProducts = await _context.Products
                                //    .AsNoTracking()
                                //    .Where(p => recommendedImageUrls.Contains(p.ImageUrl))
                                //    .Select(p => new
                                //    {
                                //        p.Id,
                                //        p.Name,
                                //        p.ImageUrl,
                                //        p.Price,
                                //        p.Description
                                //    }).ToListAsync();

                                //return Ok(new { message = "File uploaded successfully and recommendations fetched.", products = recommendedProducts });
                               return  Ok(await _orderServices.GetRecommendedProducts(recommendedImageUrls));
                            }

                            return Ok(new { message = "File uploaded successfully but no recommendations found.", products = new List<object>() });
                        }

                        // Handle case where recommended_images field is missing or null
                        return BadRequest("Invalid response from recommendation service.");
                    }

                    return StatusCode((int)response.StatusCode, new { message = "Failed to upload file to Server B.", serverBResponse = await response.Content.ReadAsStringAsync() });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "An error occurred while processing your request.", error = ex.Message });
            }
        }






        [HttpPost]
        [Route("[action]")]
        [Authorize(Roles = nameof(RolesEnum.Customer))]
        public async Task<IActionResult> AddOrder(AddOrderDto dto)
        {
            var response = await _orderServices.AddOrderAsync(dto);
            return Ok(response);
        }



        [HttpPost]
        [Route("[action]/{ProductId}")]
        [Authorize(Roles = nameof(RolesEnum.Customer))]
        public async Task<IActionResult> AddFavouriteProduct(Guid ProductId) =>
           Ok(await _orderServices.AddFavouriteProduct(ProductId , false));
        
        [HttpDelete]
        [Route("[action]/{ProductId}")]
        [Authorize(Roles = nameof(RolesEnum.Customer))]
        public async Task<IActionResult> DeleteFavouriteProduct(Guid ProductId) =>
           Ok(await _orderServices.AddFavouriteProduct(ProductId , true));


        [HttpPut]
        [Route("[action]/{OrderId}")]
        [Authorize(Roles = nameof(RolesEnum.Customer))]
        public async Task<IActionResult> ConfirmOrder(Guid OrderId) => 
            Ok(await _orderServices.ConfirmOrder(OrderId, true));

        [HttpGet]
        [Route("[action]")]
        [Authorize(Roles = nameof(RolesEnum.Customer))]
        public async Task<IActionResult> GetFavouriteProducts() =>
            Ok(await _orderServices.GetFavouriteProducts());
        
        [HttpGet]
        [Route("[action]")]
        [Authorize(Roles = nameof(RolesEnum.Customer))]
        public async Task<IActionResult> GetMyOrders() =>
            Ok(await _orderServices.GetMyOrders());
        
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetBestSellerProducts() =>
            Ok(await _orderServices.GetBestSellerProducts());
        
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetMaleProducts() =>
            Ok(await _orderServices.GetGenderProducts(true)); 
        
        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetFemaleProducts() =>
            Ok(await _orderServices.GetGenderProducts(false));
        
        [HttpGet]
        [Route("[action]/{CategoryId}")]
        public async Task<IActionResult> GetMaleProductsWithCategory(Guid CategoryId) =>
           Ok(await _orderServices.GetGenderProductsWithCategory(CategoryId,true));
        
        [HttpGet]
        [Route("[action]/{CategoryId}")]
        public async Task<IActionResult> GetFemaleProductsWithCategory(Guid CategoryId) =>
           Ok(await _orderServices.GetGenderProductsWithCategory(CategoryId,false));

        [HttpGet]
        [Route("[action]")]
        public async Task<IActionResult> GetUniqueProducts([FromQuery]GetUniqueProductsDto dto) =>
           Ok(await _orderServices.GetUniqueProducts(dto));

        [HttpGet]
        [Route("[action]/{OrderId}")]
        public async Task<IActionResult> GetOrderDetails(Guid OrderId) =>
          Ok(await _orderServices.GetOrderDetails(OrderId));
    }
}
