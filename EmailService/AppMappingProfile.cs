using AutoMapper;
using DAL.Models;
using DAL.Views;

namespace EmailService
{
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<CreateOrderRequestView, Order>().ReverseMap();
        }
    }
}
