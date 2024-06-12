using DAL.Interfaces.Services;
using DAL.Models;
using DAL.Models.Enums;
using Newtonsoft.Json;
using ReceiptService.Interfaces;
using System.Text;

namespace ReceiptService.Services
{
    public class ReceiptGenerateService : IReceiptGenerateService
    {
        private readonly IServiceProvider _serviceProvider;

        public ReceiptGenerateService(IServiceProvider serviceProvider) 
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<byte[]> GenerateReceiptAsync(Order order)
        {
            var receiptContent = $"Order Id: {order.Id}\n" +
                                $"Items:\n" +
                                string.Join("\n", order.Items.Select(i => $"{i.ProductName} x{i.Quantity} @ {i.Price:C}"));

            var filePath = string.Format($"../Receipts/Receipt_{order.Id}.txt", receiptContent);

            if (!File.Exists(filePath))
            {
                File.WriteAllText(filePath, receiptContent);
            }
            var receiptEvent = new { OrderId = order.Id, ReceiptPath = filePath, Email = order.Email };
            var receiptEventJson = JsonConvert.SerializeObject(receiptEvent);
            var body = Encoding.UTF8.GetBytes(receiptEventJson);

            order.OrderStatus = OrderStatus.ReceiptCreated;
            order.ReceiptsAddress = filePath;
            using (var scope = _serviceProvider.CreateScope())
            {
                var orderService = scope.ServiceProvider.GetRequiredService<IOrderService>();
                await orderService.UpdateOrderAsync(order);
            }

            return body;
            //_channel.BasicPublish(exchange: "",
            //                      routingKey: "receiptQueue",
            //                      basicProperties: null,
            //                      body: body);

        }
    }
}
