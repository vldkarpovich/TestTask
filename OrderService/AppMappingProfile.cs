using AutoMapper;
using DAL.Models;
using DAL.Views;
using OrderService.Orders.CreateOrder;

namespace OrderService
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<CreateOrderCommand, Order>();
            CreateMap<CreateOrderCommand, CreateOrderRequestView>();
            CreateMap<CreateOrderRequestView, Order>();
            CreateMap<Order, CreateOrderResponseView>();
        }
    }
}
