using DAL.Views;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace OrderService
{
    public class RabbitMQClientService
    {
        private readonly ConnectionFactory _factory;
        private readonly IConnection _connection;
        private readonly IModel _channel;

        public RabbitMQClientService()
        {
            _factory = new ConnectionFactory() { HostName = "localhost" };
            _connection = _factory.CreateConnection();
            _channel = _connection.CreateModel();
        }

        public void PublishOrder(CreateOrderResponseView order)
        {
            var orderJson = JsonConvert.SerializeObject(order);
            var body = Encoding.UTF8.GetBytes(orderJson);

            _channel.BasicPublish(exchange: "",
                                  routingKey: "orderQueue",
                                  basicProperties: null,
                                  body: body);
        }
    }

}
