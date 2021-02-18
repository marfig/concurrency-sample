using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace OrdersProcessor.Services
{
    public interface IOrderProcessorService
    {
        Task<List<string>> GetOrdersAsync(int countOrders);
        Task<List<string>> ProcessOrdersAsync(List<string> orders, IProgress<int> progress);
    }
}
