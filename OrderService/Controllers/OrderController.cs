using Microsoft.AspNetCore.Mvc;
using OrderService.Controllers.Base;
using OrderService.Orders.CreateOrder;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrderController : BaseController
    {

        public OrderController(RabbitMQClientService rabbitMQClientService)
        {}

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderCommand command)
        {

            var result = await Mediator.Send(command);

            if (result.Success)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
    }
}
