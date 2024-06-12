using AutoMapper;
using DAL.Interfaces.Services;
using DAL.Views;
using MediatR;
using OrderService.Exceptions;
using System.Net;

namespace OrderService.Orders.CreateOrder
{
    public class CreateOrderHandler : IRequestHandler<CreateOrderCommand, CreateOrderResponse>
    {
        private readonly IOrderService _orderService;
        private readonly IMapper _mapper;

        private readonly RabbitMQClientService _rabbitMQClientService;

        public CreateOrderHandler(RabbitMQClientService rabbitMQClientService, IOrderService orderService, IMapper mapper) 
        {
            _rabbitMQClientService = rabbitMQClientService;
            _orderService = orderService;
            _mapper = mapper;
        }

        public async Task<CreateOrderResponse> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
        {
                
            var result = await _orderService.CreateOrderAsync(_mapper.Map<CreateOrderRequestView>(request));

            if (result == null)
            {
                throw new RestException(HttpStatusCode.NotFound);
            }

            _rabbitMQClientService.PublishOrder(result);


            return new CreateOrderResponse(true, "");
        }
    }
}
