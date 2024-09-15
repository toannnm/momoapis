using Application.Models.MomoModels;

namespace Application.Interfaces.IExtensionServices
{
    public interface IMomoService
    {
        Task<(bool, MomoResponse?)> IntializePayment(string paymentUrl, MomoModel request);
        Task<(bool, QueryTransactionResponse?)> QueryTransaction(string endpoint, QueryTransactionModel request);
        Task<(bool, RefundResponse?)> Refund(string endpoint, RefundModel request);
    }
}