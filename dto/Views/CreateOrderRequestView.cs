using DAL.Models;

namespace DAL.Views;
    public record CreateOrderRequestView(string Email, List<OrderItem> Items);
