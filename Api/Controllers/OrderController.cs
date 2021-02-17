using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Api.Helpers;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        [HttpPost]
        [Route("payment")]
        public async Task<ActionResult> Payment([FromBody] string orderNumber)
        {
            var randomValue = RandomGenerator.NextDouble();
            var success = randomValue > 0.1;
            // Delay for simulate payment process
            await Task.Delay(2000);
            Console.WriteLine($"Order {orderNumber} paid");
            return Ok(new { Order = orderNumber, Payed = success });
        }
    }
}
