using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OutFitMaker.DataAccess.DbContext;
using OutFitMaker.DataAccess.Repositories.Main;
using OutFitMaker.Domain.Constants.Enums;
using OutFitMaker.Domain.Constants.Statics;
using OutFitMaker.Domain.DTOs.Operation;
using OutFitMaker.Domain.Helper;
using OutFitMaker.Domain.Interfaces.Base;
using OutFitMaker.Domain.Interfaces.Operation;
using OutFitMaker.Domain.Models.Main;
using OutFitMaker.Domain.Models.Operation;
using OutFitMaker.Domain.Models.Security;
using OutFitMaker.Services.IServices.Security;
using OutFitMaker.Services.Services.Generic;
using System.Linq.Expressions;

namespace OutFitMaker.DataAccess.Repositories.Operation
{
    public class OrderServices : GenricRepo<OrderSet>, IOrderServices
    {
        private IHttpContextAccessor _httpContextAccessor { get; }
        private IUserRepo _userRepo { get; }
        private UserManager<UserSet> _userManager { get; }
        private SignInManager<UserSet> _signInManager { get; }
        private IBaseServices _baseServices { get; }
        public OrderServices(OutFitMakerDbContext context
            , UserManager<UserSet> userManager,
            IUserRepo userRepo,
            SignInManager<UserSet> signInManager,
            IBaseServices baseServices,
            IHttpContextAccessor httpContextAccessor) : base(context)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _baseServices = baseServices;
            _userRepo = userRepo;
            _httpContextAccessor = httpContextAccessor;
        }

        private IEnumerable<OrderItemsSet> FillOrderItems(IEnumerable<OrderItemDto> orderItems, Guid orderId)
        {
            var orderItemsList = new List<OrderItemsSet>();



            var products = _context.Products
                .AsNoTracking()
                .Where(e => !e.IsDeleted)
                .ToList();
            foreach (var item in orderItems)
            {
                orderItemsList.Add(new OrderItemsSet
                {
                 
                    Quentity = item.Quantity,
                    OrderId = orderId,
                    ProductId = item.ProductId,
                    Price = products.Where(p => p.Id == item.ProductId).Select(e => e.Price).FirstOrDefault(),
                });
            }
            return orderItemsList;
        }

        private async Task<double> GetOrderTotalAsync(IEnumerable<OrderItemsSet> products)
        {
            var total = 0.0;

            foreach (var item in products)
            {
                total += (item.Price * item.Quentity);
            }
            return total;
        }
        public async Task<APIResponse<object>> AddOrderAsync(AddOrderDto dto)
        {
            var response = new APIResponse<object>();
            try
            {
                var userId = _userRepo.GetCurrentUserId();
                var productsIds = dto.OrderItems.Select(e => e.ProductId).ToList();

                bool someProductNotExist = false;
                var existingProducts = _context.Products.Where(e => !e.IsDeleted
               && e.IsAvailable).ToList();
                var existProductsIds = existingProducts.Select(e => e.Id).ToList();
                foreach (var item in productsIds)
                {
                    if (!existProductsIds.Contains(item))
                    {
                        someProductNotExist = true;
                        break;
                    }
                }

                if (someProductNotExist)
                {
                    response.Status = StatusCodes.Status400BadRequest;
                    response.Message = GlobalStatices.ProductUnAvailable;
                    return response;
                }

                var order = new OrderSet
                {
                    OrderStatus = OrderStatusEnum.PreConfirmed,
                    UserId = userId,
                    OrderNumber = _baseServices.GenerateRandomCode(6)
                };

                await Add(order);
                await SaveChangesAsync();

                var orderItems = FillOrderItems(dto.OrderItems, order.Id);
                var total = GetOrderTotalAsync(orderItems).Result;

                await _context.OrderItems.AddRangeAsync(orderItems);
                order.Total = total;
                Update(order);
                await SaveChangesAsync();

                response.Status = StatusCodes.Status200OK;
                response.Message = GlobalStatices.Success;
                response.Data = new
                {
                    Total = total,
                    OrderId = order.Id,
                };
            }
            catch (Exception ex)
            {
                response.Message = GlobalStatices.Fail + ex.Message;
                response.Status = StatusCodes.Status500InternalServerError;
            }
            return response;
        }

        public async Task<APIResponse<object>> ConfirmOrder(Guid id, bool isConfirm)
        {
            var response = new APIResponse<object>();
            try
            {
                var userId = _userRepo.GetCurrentUserId();
                var obj = await _context.Orders.FirstOrDefaultAsync(x => !x.IsDeleted && x.Id == id && x.UserId == userId);
                if (obj == null)
                {
                    response.Message = GlobalStatices.Fail + GlobalStatices.OrderNotFound;
                    response.Status = StatusCodes.Status400BadRequest;
                    return response;
                }
                obj.OrderStatus = OrderStatusEnum.Confirmed;
                _context.Orders.Update(obj);
                await _context.SaveChangesAsync();

                response.Status = StatusCodes.Status200OK;
                response.Message = GlobalStatices.Success;
                response.Data = true;
            }
            catch (Exception ex)
            {
                response.Message = GlobalStatices.Fail + ex.Message;
                response.Status = StatusCodes.Status500InternalServerError;
            }
            return response;
        }

        public async Task<APIResponse<object>> AddFavouriteProduct(Guid productId, bool isDeleted)
        {
            var response = new APIResponse<object>();
            try
            {
                var userId = _userRepo.GetCurrentUserId();
                var check = await _context.Products.AsNoTracking().FirstOrDefaultAsync(e => !e.IsDeleted && e.IsAvailable && e.Id == productId);
                if (check == null)
                {
                    response.Message = GlobalStatices.Fail + GlobalStatices.ProductUnAvailable;
                    response.Status = StatusCodes.Status400BadRequest;
                    return response;
                }
                var ss = await _context.FavouriteProduct
                                  .FirstOrDefaultAsync(e => !e.IsDeleted && e.UserId == userId && e.ProductId == check.Id);
                if (isDeleted)
                {
                   
                    if(ss != null)
                    {
                        ss.IsDeleted = true;
                        _context.FavouriteProduct.Update(ss);
                        await _context.SaveChangesAsync();

                        response.Status = StatusCodes.Status200OK;
                        response.Message = GlobalStatices.Success;
                        response.Data = true;
                    }
                }
                else
                {
                    if (ss != null)
                    {
                        response.Status = StatusCodes.Status200OK;
                        response.Message = "This Product is already Exist";
                        response.Data = true;
                    }
                    else
                    {
                        var obj = new FavouriteProductSet
                        {
                            ProductId = productId,
                            UserId = userId
                        };
                        await _context.FavouriteProduct.AddAsync(obj);
                        await _context.SaveChangesAsync();

                        response.Status = StatusCodes.Status200OK;
                        response.Message = GlobalStatices.Success;
                        response.Data = true;
                    }
                    
                }
            }
            catch (Exception ex)
            {
                response.Message = GlobalStatices.Fail + ex.Message;
                response.Status = StatusCodes.Status500InternalServerError;
            }
            return response;
        }

        public async Task<APIResponse<object>> GetRecommendedProducts(List<string> recommendedImageUrls)
        {


            for (int i = 0; i < recommendedImageUrls.Count; i++)
            {
                recommendedImageUrls[i]= recommendedImageUrls[i].Split("\\")[1];
            }

            var response = new APIResponse<object>();

            try
            {
                var obj = await _context.Products.AsNoTracking().Where(e => !e.IsDeleted&& recommendedImageUrls.Contains(e.ImageUrl))
                    .Select(e => new
                    {
                        e.Name,
                        e.Price,
                        e.ImageUrl,
                        e.Id

                    }).ToListAsync();
                response.Status = StatusCodes.Status200OK;
                response.Message = GlobalStatices.Success;
                response.Data = obj;
                await Console.Out.WriteLineAsync(recommendedImageUrls[0]) ;
            }
            catch (Exception ex)
            {
                response.Message = GlobalStatices.Fail + ex.Message;
                response.Status = StatusCodes.Status500InternalServerError;
            }
            return response;
        }

        public async Task<APIResponse<object>> GetFavouriteProducts()
        {
            var response = new APIResponse<object>();
            try
            {
                var userId = _userRepo.GetCurrentUserId();
                var res = await _context.FavouriteProduct
                    .Include(e => e.Product)
                    .AsNoTracking()
                    .AsSplitQuery()
                    .Where(e => !e.IsDeleted && e.UserId == userId)
                    .Select(e => new
                    {
                        Id = e.ProductId,
                        Name = e.Product!.Name ?? "",
                        ImageUrl = e.Product!.ImageUrl ?? "",
                        e.Product.Rate,
                        e.Product.Price
                    }).Distinct().ToListAsync();

                response.Status = StatusCodes.Status200OK;
                response.Message = GlobalStatices.Success;
                response.Data = res;
            }
            catch (Exception ex)
            {
                response.Message = GlobalStatices.Fail + ex.Message;
                response.Status = StatusCodes.Status500InternalServerError;
            }
            return response;
        }

        public async Task<APIResponse<object>> GetMyOrders()
        {
            var response = new APIResponse<object>();
            var userId = _userRepo.GetCurrentUserId();
            try
            {
                var orders = await _context.Orders
                    .AsNoTracking()
                    .Where(e => !e.IsDeleted && e.UserId == userId)
                    .Select(e => new
                    {
                        e.Id,
                        e.OrderNumber,
                        e.Total,
                        Status = e.OrderStatus,
                        Date = e.CreationDate,
                        Product = _context.OrderItems
                            .Where(oi => oi.OrderId == e.Id && !oi.IsDeleted)
                            .Select(oi => new
                            {
                                oi.Product.ImageUrl,
                                oi.Product.Name
                            }).FirstOrDefault()

                    }).ToListAsync();

                var formattedOrders = orders.Select(o => new
                {
                    o.Id,
                    o.OrderNumber,
                    o.Total,
                    o.Status,
                    o.Date,
                    ImageUrl = o.Product != null ? o.Product.ImageUrl : null,
                    Name = o.Product != null ? o.Product.Name : null
                }).ToList();

                response.Status = StatusCodes.Status200OK;
                response.Message = GlobalStatices.Success;
                response.Data = formattedOrders;
            }
            catch (Exception ex)
            {
                response.Message = GlobalStatices.Fail + ex.Message;
                response.Status = StatusCodes.Status500InternalServerError;
            }
            return response;
        }


        //public async Task<APIResponse<object>> GetMyOrders()
        //{
        //    var response = new APIResponse<object>();
        //    var userId = _userRepo.GetCurrentUserId();
        //    try
        //    {
        //        var obj = await _context.Orders
        //            .AsNoTracking()
        //            .Where(e => !e.IsDeleted && e.UserId == userId)
        //            .Select(e => new
        //            {
        //                e.Id,
        //                e.OrderNumber,
        //                e.Total,
        //                Status = e.OrderStatus,
        //                Date = e.CreationDate,
        //                Products = _context.OrderItems
        //            .Where(oi => oi.OrderId == e.Id && !oi.IsDeleted)
        //            .Select(oi => new
        //            {
        //                oi.Product.ImageUrl,
        //                oi.Product.Name
        //            }).ToList()
        //            }).ToListAsync();
        //        response.Status = StatusCodes.Status200OK;
        //        response.Message = GlobalStatices.Success;
        //        response.Data = obj;

        //    }
        //    catch (Exception ex)
        //    {
        //        response.Message = GlobalStatices.Fail + ex.Message;
        //        response.Status = StatusCodes.Status500InternalServerError;
        //    }
        //    return response;
        //}

        public async Task<APIResponse<object>> GetBestSellerProducts()
        {
            var response = new APIResponse<object>();
            try
            {
                var obj = await _context.Products.AsNoTracking().Where(e => !e.IsDeleted && e.Rate > 3)
                    .Select(e => new
                    {
                        e.Name,
                        e.Price,
                        e.ImageUrl,
                        e.Id

                    }).ToListAsync();
                response.Status = StatusCodes.Status200OK;
                response.Message = GlobalStatices.Success;
                response.Data = obj;
            }
            catch (Exception ex)
            {
                response.Message = GlobalStatices.Fail + ex.Message;
                response.Status = StatusCodes.Status500InternalServerError;
            }
            return response;
        }

        public async Task<APIResponse<object>> GetGenderProducts(bool isMen)
        {
            var response = new APIResponse<object>();
            try
            {
                if (isMen)
                {
                    var menObj = await _context.Products
                        .AsNoTracking()
                        .AsSplitQuery()
                        .Include(e => e.Category)
                        .Where(e => !e.IsDeleted && e.Category.Gender == GenderEnum.Male)
                        .Select(e => new
                        {
                            e.Name,
                            e.Price,
                            e.ImageUrl,
                            e.Id
                        }).ToListAsync();
                    response.Status = StatusCodes.Status200OK;
                    response.Message = GlobalStatices.Success;
                    response.Data = menObj;
                }
                else
                {
                    var obj = await _context.Products
                       .AsNoTracking()
                       .AsSplitQuery()
                       .Include(e => e.Category)
                       .Where(e => !e.IsDeleted && e.Category.Gender == GenderEnum.Female)
                       .Select(e => new
                       {
                           e.Name,
                           e.Price,
                           e.ImageUrl,
                           e.Id
                       }).ToListAsync();
                    response.Status = StatusCodes.Status200OK;
                    response.Message = GlobalStatices.Success;
                    response.Data = obj;
                }
            }
            catch (Exception ex)
            {
                response.Message = GlobalStatices.Fail + ex.Message;
                response.Status = StatusCodes.Status500InternalServerError;
            }
            return response;
        }

        public async Task<APIResponse<object>> GetGenderProductsWithCategory(Guid id, bool isMen)
        {
            var response = new APIResponse<object>();
            try
            {
                if (isMen)
                {
                    var menObj = await _context.Products
                        .AsNoTracking()
                        .AsSplitQuery()
                        .Include(e => e.Category)
                        .Where(e => !e.IsDeleted && e.Category.Gender == GenderEnum.Male && e.CategoryId == id)
                        .Select(e => new
                        {
                            e.Name,
                            e.Price,
                            e.ImageUrl,
                            e.Id
                        }).ToListAsync();
                    response.Status = StatusCodes.Status200OK;
                    response.Message = GlobalStatices.Success;
                    response.Data = menObj;
                }
                else
                {
                    var obj = await _context.Products
                       .AsNoTracking()
                       .AsSplitQuery()
                       .Include(e => e.Category)
                       .Where(e => !e.IsDeleted && e.Category.Gender == GenderEnum.Female && e.CategoryId == id)
                       .Select(e => new
                       {
                           e.Name,
                           e.Price,
                           e.ImageUrl,
                           e.Id
                       }).ToListAsync();
                    response.Status = StatusCodes.Status200OK;
                    response.Message = GlobalStatices.Success;
                    response.Data = obj;
                }
            }
            catch (Exception ex)
            {
                response.Message = GlobalStatices.Fail + ex.Message;
                response.Status = StatusCodes.Status500InternalServerError;
            }
            return response;
        }

        public async Task<APIResponse<object>> GetUniqueProducts(GetUniqueProductsDto dto)
        {
            var response = new APIResponse<object>();
            try
            {
                Expression<Func<ProductSet, bool>> filter = e => !e.IsDeleted && e.IsUnique &&
                (!dto.ColorId.HasValue || dto.ColorId.Value == e.ColorId)
                &&
                (!dto.Feature.HasValue || dto.Feature.Value == e.Type)
                &&
                (!dto.CategoryId.HasValue || dto.CategoryId.Value == e.CategoryId)
                &&
                (!dto.Gender.HasValue || dto.Gender.Value == e.Category.Gender);
                var obj = await _context.Products
                      .AsNoTracking()
                      .AsSplitQuery()
                      .Include(e => e.Category)
                      .Where(filter)
                      .Select(e => new
                      {
                          e.Name,
                          e.Price,
                          e.ImageUrl,
                          e.Type,
                          e.Id
                      }).ToListAsync();
                response.Status = StatusCodes.Status200OK;
                response.Message = GlobalStatices.Success;
                response.Data = obj;
            
            }
            catch (Exception ex)
            {
                response.Message = GlobalStatices.Fail + ex.Message;
                response.Status = StatusCodes.Status500InternalServerError;
            }
            return response;
        }

        public async Task<APIResponse<object>> GetOrderDetails(Guid id)
        {
            var response = new APIResponse<object>();
            try
            {
                var total = await _context.Orders.AsNoTracking().FirstOrDefaultAsync(e => !e.IsDeleted && e.Id == id);
                var obj = await _context.OrderItems
                    .AsNoTracking()
                    .AsSplitQuery()
                    .Include (e => e.Order)
                    .Include(e=>e.Product)
                    .Where(e=>!e.IsDeleted && e.OrderId == id)
                    .Select(e=> new
                    {
                        ProductImg = e.Product.ImageUrl,
                        ProductName = e.Product.Name,
                        ProductPrice = e.Price,
                        Quentity = e.Quentity,

                    }).ToListAsync();
                response.Status = StatusCodes.Status200OK;
                response.Message = GlobalStatices.Success;
                response.Data = new
                {
                     total!.Total,
                     Data = obj
                };

            }
            catch (Exception ex)
            {
                response.Message = GlobalStatices.Fail + ex.Message;
                response.Status = StatusCodes.Status500InternalServerError;
            }
            return response;
        }
    }
}
