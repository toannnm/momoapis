using Application.Interfaces.IServices;
using Application.Models.OrderModels;
using Microsoft.AspNetCore.Mvc;

namespace WebAPIs.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService) => _orderService = orderService;

        [HttpPost]
        //[Authorize(Roles = "Customer")]
        public async Task<ActionResult> CreateOrder([FromBody] CreateOrderModel model)
        {
            var result = await _orderService.AddOrderAsync(model);
            return Ok(result);
        }

        [HttpGet]
        public async Task<ActionResult> GetOrders(int pageIndex = 1, int pageSize = 10)
        {
            var result = await _orderService.GetOrdersAsync(pageIndex, pageSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetOrderById([FromRoute] Guid id)
        {
            var result = await _orderService.GetOrderByIdAsync(id);
            return Ok(result);
        }
    }
}
