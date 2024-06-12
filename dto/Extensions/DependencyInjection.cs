using DAL.Interfaces.Repositories;
using DAL.Interfaces.Services;
using DAL.Repositories;
using DAL.Services;
using Microsoft.Extensions.DependencyInjection;

namespace DAL.Extensions
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddDalService(this IServiceCollection services)
        {
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderRepository, OrderRepository>();

            return services;
        }
    }
}
