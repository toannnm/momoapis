using Application.Interfaces.IExtensionServices;
using Application.Models.MomoModels;
using Microsoft.AspNetCore.Mvc;

namespace WebAPIs.Controllers
{
    [Route("[controller]/[action]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly IPaymentService _paymentService;

        public PaymentController(IPaymentService paymentService) => _paymentService = paymentService;

        [HttpPost]
        //[Authorize(Roles = "Customer")]
        public async Task<IActionResult> MomoPayment(Guid orderId, MomoRequestModel model)
        {
            var result = await _paymentService.PaymentWithMomo(orderId, model);
            return Ok(result);
        }

        [HttpPost]
        //[Authorize(Roles = "Customer,Admin")]
        public async Task<IActionResult> QueryTransaction(Guid orderId)
        {
            var result = await _paymentService.QueryTransaction(orderId);
            return Ok(result);
        }

        [HttpPost]
        //[Authorize(Roles = "Customer")]
        public async Task<IActionResult> Refund(Guid orderId, RefundRequestModel model)
        {
            var result = await _paymentService.Refund(orderId, model);
            return Ok(result);
        }
        //[HttpPost]

    }
}
