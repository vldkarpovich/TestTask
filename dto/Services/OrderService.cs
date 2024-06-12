using DAL.Interfaces.Repositories;
using DAL.Interfaces.Services;
using DAL.Views;
using DAL.Models;
using AutoMapper;
using DAL.Models.Enums;

namespace DAL.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IMapper _mapper;

        public OrderService(IOrderRepository orderRepository, IMapper mapper, IServiceProvider serviceProvider)
        {
            _orderRepository = orderRepository;
            _mapper = mapper;
        }

        public async Task<Order> GetOrderByIdAsync(Guid id)
        {
            var result = await _orderRepository.GetByIdAsync(id);

            return result;
        }

        public async Task<CreateOrderResponseView> CreateOrderAsync(CreateOrderRequestView request)
        {
            var order = _mapper.Map<Order>(request);
            order.Id = Guid.NewGuid();
            order.OrderStatus = OrderStatus.Created;
            order.CreatedDate = DateTime.Now;

            await _orderRepository.AddAsync(order);
            await _orderRepository.SaveChangesAsync();

            return _mapper.Map<CreateOrderResponseView>(order);
        }

        public async Task UpdateOrderAsync(Order order)
        {
            await _orderRepository.UpdateOrderAsync(order);
        }
    }
}
