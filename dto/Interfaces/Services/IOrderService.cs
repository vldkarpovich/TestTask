
using DAL.Models;
using DAL.Views;

namespace DAL.Interfaces.Services
{
    /// <summary>
    /// Order service
    /// </summary>
    public interface IOrderService
    {
        public Task<Order> GetOrderByIdAsync(Guid id);
        public Task<CreateOrderResponseView> CreateOrderAsync(CreateOrderRequestView request);
        public Task UpdateOrderAsync(Order order);
        
    }
}
