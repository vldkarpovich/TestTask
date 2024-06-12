using DAL.Interfaces.Services;
using DAL.Views;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using ReceiptService.Interfaces;
using System.Text;

namespace ReceiptService
{
    public class RabbitMQBackgroundService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConnection _connection;
        private readonly IModel _channel;


        public RabbitMQBackgroundService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
            var factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        protected override Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _channel.QueueDeclare(queue: "orderQueue",
                                 durable: false,
                                 exclusive: false,
                                 autoDelete: false,
                                 arguments: null);

            var consumer = new EventingBasicConsumer(_channel);
            consumer.Received += async (model, ea) =>
            {
                var body = ea.Body.ToArray();
                var message = Encoding.UTF8.GetString(body);
                var orderResponse = JsonConvert.DeserializeObject<CreateOrderResponseView>(message);


                using (var scope = _serviceProvider.CreateScope())
                {
                    var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();

                    var order = await orderService.GetOrderByIdAsync(orderResponse.Id);

                    var receptService = scope.ServiceProvider.GetRequiredService<IReceiptGenerateService>();

                    var receiptResult = await receptService.GenerateReceiptAsync(order);

                    _channel.BasicPublish(exchange: "",
                                  routingKey: "receiptQueue",
                                  basicProperties: null,
                                  body: receiptResult);
                }
                    _channel.BasicAck(deliveryTag: ea.DeliveryTag, multiple: false);
            };

            _channel.BasicConsume(queue: "orderQueue",
                                 autoAck: false,
                                 consumer: consumer);

            return Task.CompletedTask;
        }
    }
}
