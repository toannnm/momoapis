using Application.Extensions;
using Application.Models.HelperModels;
using Application.Models.OrderModels;

namespace Application.Interfaces.IServices
{
    public interface IOrderService
    {
        Task<Response<OrderModel>> AddOrderAsync(CreateOrderModel model);
        Task<Response<OrderModel>> GetOrderByIdAsync(Guid id);
        Task<Response<Pagination<OrderModel>>> GetOrdersAsync(int pageIndex, int pageSize = 10);
    }
}