using Application.Models.HelperModels;
using Application.Models.MomoModels;

namespace Application.Interfaces.IExtensionServices
{
    public interface IPaymentService
    {
        Task<Response<MomoResponse>> PaymentWithMomo(Guid OrderId, MomoRequestModel model);
        Task<Response<QueryTransactionResponse>> QueryTransaction(Guid OrderId);
        Task<Response<RefundResponse>> Refund(Guid OrderId, RefundRequestModel model);
    }
}