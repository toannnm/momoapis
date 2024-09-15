using Application.Interfaces.IExtensionServices;
using Application.Interfaces.IUnitOfWork;
using Application.Models.HelperModels;
using Application.Models.MomoModels;
using Application.Models.Settings;
using AutoMapper;
using Domain.Enums;
using Microsoft.Extensions.Options;

namespace Application.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly MomoSection _momoSection;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IMomoService _momoService;

        public PaymentService(IOptions<MomoSection> momoSection, IUnitOfWork unitOfWork, IMapper mapper, IMomoService momoService)
        {
            (_momoSection, _unitOfWork, _mapper, _momoService) = (momoSection.Value, unitOfWork, mapper, momoService);
        }

        public async Task<Response<MomoResponse>> PaymentWithMomo(Guid OrderId, MomoRequestModel model)
        {

            var order = await _unitOfWork.OrderRepository.GetByIdAsync(OrderId);
            if (order is null)
            {
                return new Response<MomoResponse>("Not found orderId", 404);
            }

            var request = new MomoModel
            {
                partnerCode = _momoSection.PartnerCode,
                partnerName = model.PartnerName,
                requestId = order.UserId.ToString(),
                amount = Math.Round(order.TotalPrice, 0).ToString(),
                orderId = OrderId.ToString(),
                orderInfo = "khachhang",
                redirectUrl = _momoSection.ReturnUrl,
                ipnUrl = _momoSection.IpnUrl,
            };
            request.signature = request.MakeSignature(_momoSection.AccessKey, _momoSection.SecretKey);

            var (createMomoLinkResult, MomoResponse) = await _momoService.IntializePayment(_momoSection.PaymentUrl, request);

            if (createMomoLinkResult)
            {
                order.PaymentStatus = PaymentStatusEnum.Paid;
                order.PaymentMethod = _momoSection.ServiceName;
                var results = _mapper.Map<MomoResponse>(MomoResponse);
                return new Response<MomoResponse>(results);
            }

            return new Response<MomoResponse>("Payment with momo fail!", 500);
        }

        public async Task<Response<QueryTransactionResponse>> QueryTransaction(Guid OrderId)
        {

            var order = await _unitOfWork.OrderRepository.GetByIdAsync(OrderId);
            if (order is null)
            {
                return new Response<QueryTransactionResponse>("Not found orderId", 404);
            }

            var request = new QueryTransactionModel
            {
                partnerCode = _momoSection.PartnerCode,
                requestId = order.UserId.ToString(),
                orderId = OrderId.ToString(),
                lang = _momoSection.Lang,
            };
            request.signature = request.MakeSignature(_momoSection.AccessKey, _momoSection.SecretKey);

            var (createMomoLinkResult, QueryTransactionResponse) = await _momoService.QueryTransaction(_momoSection.QueryTransactionEndpoint, request);

            if (createMomoLinkResult)
            {
                var results = _mapper.Map<QueryTransactionResponse>(QueryTransactionResponse);
                results.payType = "qrcode";
                results.signature = request.signature;
                return new Response<QueryTransactionResponse>(results);
            }

            return new Response<QueryTransactionResponse>("Can not view transaction info!", 500);
        }

        public async Task<Response<RefundResponse>> Refund(Guid OrderId, RefundRequestModel model)
        {

            var order = await _unitOfWork.OrderRepository.GetByIdAsync(OrderId);
            if (order is null)
            {
                return new Response<RefundResponse>("Not found orderId", 404);
            }

            var request = new RefundModel
            {
                partnerCode = _momoSection.PartnerCode,
                orderId = Guid.NewGuid().ToString(),
                requestId = Guid.NewGuid().ToString(),
                amount = Math.Round(model.RefundAmount, 0).ToString() /*model.RefundAmount <= order.TotalPrice ? Math.Round(model.RefundAmount, 0).ToString() : default!*/,
                lang = _momoSection.Lang,
                description = model.RefundReason,
            };

            var a = await QueryTransaction(order.Id);
            if (a.Result is not null)
            {
                request.transId = a.Result.transId;
            }
            request.signature = request.MakeSignature(_momoSection.AccessKey, _momoSection.SecretKey);

            var (createMomoLinkResult, RefundResponse) = await _momoService.Refund(_momoSection.RefundEndpoint, request);

            if (createMomoLinkResult)
            {
                var results = _mapper.Map<RefundResponse>(RefundResponse);
                results.signature = request.signature;
                return new Response<RefundResponse>(results);
            }

            return new Response<RefundResponse>("Can not refund!", 500);
        }
    }
}
