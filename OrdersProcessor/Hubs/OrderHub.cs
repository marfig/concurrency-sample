using Microsoft.AspNetCore.SignalR;

namespace OrdersProcessor.Hubs
{
    public class OrderHub: Hub
    {
        public string GetConnectionId()
        {
            return Context.ConnectionId;
        }
    }
}
