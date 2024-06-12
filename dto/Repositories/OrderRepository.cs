using DAL.Interfaces.Repositories;
using DAL.Models;
using DAL.Repositories.Base;
using Microsoft.Extensions.Logging;

namespace DAL.Repositories
{
    public class OrderRepository : BaseKeyRepository<OrderRepository, Order>, IOrderRepository
    {
        public OrderRepository(ILogger<OrderRepository> logger, ApplicationDataContext context)
        : base(logger, context)
        {

        }

        public async Task<Order> UpdateOrderAsync(Order Order)
        {
            try
            {
                if (Order == null)
                    throw new ArgumentNullException("Order cannot be null");

                _context.Orders.Update(Order);
                await _context.SaveChangesAsync();
                return Order;
            }
            catch (Exception e)
            {
                _logger.LogError("Could not update order {type} : {exception}", typeof(Order).ToString(), e.ToString());
                throw;
            }
        }
    }
}
