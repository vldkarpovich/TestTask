using DAL;
using DAL.Extensions;
using Microsoft.EntityFrameworkCore;
using ReceiptService.Interfaces;
using ReceiptService.Services;

namespace ReceiptService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddDbContext<ApplicationDataContext>(options =>
                options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), new MySqlServerVersion(new Version(8, 0, 25))));

            builder.Services.AddHostedService<RabbitMQBackgroundService>();
            builder.Services.AddAutoMapper(typeof(AppMappingProfile));
            builder.Services.AddScoped<IReceiptGenerateService, ReceiptGenerateService>();

            builder.Services.AddControllers();

            builder.Services.AddDalService();

            var app = builder.Build();

            app.Run();
        }
    }
}
