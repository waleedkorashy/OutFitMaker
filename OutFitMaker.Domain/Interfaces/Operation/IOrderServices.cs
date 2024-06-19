using OutFitMaker.Domain.DTOs.Operation;
using OutFitMaker.Domain.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OutFitMaker.Domain.Interfaces.Operation
{
    public interface IOrderServices
    {
        Task<APIResponse<object>> GetRecommendedProducts(List<string> recommendedImageUrls);
        Task<APIResponse<object>> AddOrderAsync(AddOrderDto dto);
        Task<APIResponse<object>> ConfirmOrder(Guid id , bool isConfirm);
        Task<APIResponse<object>> AddFavouriteProduct(Guid id , bool isDeleted);
        Task<APIResponse<object>> GetFavouriteProducts();
        Task<APIResponse<object>> GetMyOrders();
        Task<APIResponse<object>> GetBestSellerProducts();
        Task<APIResponse<object>> GetGenderProducts(bool isMen);
        Task<APIResponse<object>> GetGenderProductsWithCategory(Guid id,bool isMen);
        Task<APIResponse<object>> GetUniqueProducts(GetUniqueProductsDto dto);
        Task<APIResponse<object>> GetOrderDetails(Guid id);


    }
}
