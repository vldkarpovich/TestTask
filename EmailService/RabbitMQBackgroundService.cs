using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using Newtonsoft.Json;
using DAL.Interfaces.Services;
using DAL.Models.Enums;
using EmailService.Interfaces;
using EmailService.Models;

namespace EmailService
{
    public class RabbitMQBackgroundService : BackgroundService
    {

        private readonly IServiceProvider _serviceProvider;
        private readonly IConnection _connection;
        private readonly IModel _channel;
        private readonly EmailSettings _emailSettings;

        public RabbitMQBackgroundService(IOptions<EmailSettings> emailSettings, IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            _emailSettings = emailSettings.Value;
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _channel.QueueDeclare(queue: "receiptQueue",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var receiptEvent = JsonConvert.DeserializeObject<Receipt>(message);

                using (var scope = _serviceProvider.CreateScope())
                {
                    var notificationService = scope.ServiceProvider.GetService<INotificationService>();

                    await notificationService.SendAsync(receiptEvent);

                    var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();

                    var order = await orderService.GetOrderByIdAsync(receiptEvent.OrderId);

                    order.OrderStatus = OrderStatus.EmailSended;
                    await orderService.UpdateOrderAsync(order);
                }
                _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };

            _channel.BasicConsume(queue: "receiptQueue",
                                 autoAck: false,
                                 consumer: consumer);

            return Task.CompletedTask;
        }
    }
}
