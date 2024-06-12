using DAL;
using DAL.Extensions;
using EmailService.Interfaces;
using EmailService.Models;
using EmailService.Services;
using MailKit.Net.Smtp;
using Microsoft.EntityFrameworkCore;

namespace EmailService
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddHostedService<RabbitMQBackgroundService>();
            builder.Services.Configure<EmailSettings>(builder.Configuration.GetSection("EmailSettings"));

            builder.Services.AddDbContext<ApplicationDataContext>(options =>
                options.UseMySql(builder.Configuration.GetConnectionString("DefaultConnection"), new MySqlServerVersion(new Version(8, 0, 25))));

            builder.Services.AddAutoMapper(typeof(AppMappingProfile));
            builder.Services.AddDalService();
            builder.Services.AddScoped<ISmtpClient, SmtpClient>();
            builder.Services.AddScoped<INotificationService, EmailNotificationService>();

            var app = builder.Build();

            app.Run();
        }
    }
}
