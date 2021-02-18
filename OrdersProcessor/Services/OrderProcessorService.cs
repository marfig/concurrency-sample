using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using OrdersProcessor.Models;

namespace OrdersProcessor.Services
{
    public class OrderProcessorService : IOrderProcessorService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiURL;

        public OrderProcessorService()
        {
            _httpClient = new HttpClient();
            _apiURL = "https://localhost:44370/api";
        }

        public async Task<List<string>> GetOrdersAsync(int countOrders)
        {
            return await Task.Run(() =>
            {
                var orders = new List<string>();

                for (int i = 1; i <= countOrders; i++)
                {
                    orders.Add(i.ToString().PadLeft(12, '0'));
                }

                return orders;
            });
        }

        public async Task<List<string>> ProcessOrdersAsync(List<string> orders, IProgress<int> progress = null)
        {
            using var semaphore = new SemaphoreSlim(1000);

            var listTasks = new List<Task<HttpResponseMessage>>();

            listTasks = orders.Select(async order =>
            {
                var json = JsonSerializer.Serialize(order);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                await semaphore.WaitAsync();

                try
                {
                    return await _httpClient.PostAsync($"{_apiURL}/order/payment", content);
                }
                finally
                {
                    semaphore.Release();
                }
            }).ToList();

            var responsesTasks = Task.WhenAll(listTasks);

            if (progress != null)
            {
                while (await Task.WhenAny(responsesTasks, Task.Delay(1000)) != responsesTasks)
                {
                    var tasksCompleted = listTasks.Count(x => x.IsCompleted);
                    var percent = (double)tasksCompleted / orders.Count;
                    percent *= 100;
                    var percentRound = (int)Math.Round(percent, 0);
                    progress.Report(percentRound);
                }
            }

            var responses = await responsesTasks;

            var rejectedOrders = new List<string>();

            foreach (var response in responses)
            {
                var content = await response.Content.ReadAsStringAsync();
                var responseOrder = JsonSerializer.Deserialize<ResponseOrder>(content, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

                if (!responseOrder.Payed)
                {
                    rejectedOrders.Add(responseOrder.Order);
                }
            }

            return rejectedOrders;
        }
    }
}
