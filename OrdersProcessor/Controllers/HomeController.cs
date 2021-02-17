using System;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading.Tasks;
using OrdersProcessor.Services;

namespace OrdersProcessor.Controllers
{
    public class HomeController : Controller
    {
        private readonly IOrderProcessorService _serviceProcessor;

        public HomeController(IOrderProcessorService serviceProcessor)
        {
            _serviceProcessor = serviceProcessor;
        }

        public IActionResult Index()
        {
            return View();
        }

        public async Task<JsonResult> ProcessOrders()
        {
            int countOrders = 2500;
            var orders = await _serviceProcessor.GetOrdersAsync(countOrders);
            var stopWatch = new Stopwatch();
            stopWatch.Start();

            List<string> rejected;

            try
            {
                rejected = await _serviceProcessor.ProcessOrdersAsync(orders);

                countOrders -= rejected.Count;
            }
            catch (Exception ex)
            {
                return Json(new { Message = $"An error has occured. Message: {ex.Message}" });
            }

            stopWatch.Stop();

            return Json(new { Message = $"{countOrders} orders processed in {stopWatch.ElapsedMilliseconds / 1000} seconds", Rejected = rejected });
        }
    }
}
