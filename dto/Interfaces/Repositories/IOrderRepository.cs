using DAL.Contracts;
using DAL.Models;

namespace DAL.Interfaces.Repositories
{
    /// <summary>
    /// Repository for work with ef orders object
    /// </summary>
    public interface IOrderRepository : IBaseKeyRepository<Order>
    {
        /// <summary>
        /// Update order method
        /// </summary>
        /// <param name="order"></param>
        /// <returns> Updated Order </returns>
        public Task<Order> UpdateOrderAsync(Order order);
    }
}
