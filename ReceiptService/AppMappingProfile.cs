using AutoMapper;
using DAL.Models;
using DAL.Views;

namespace ReceiptService
{ 
    public class AppMappingProfile : Profile
    {
        public AppMappingProfile()
        {
            CreateMap<CreateOrderRequestView, Order>().ReverseMap();
        }
    }
}
