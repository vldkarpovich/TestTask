using DAL.Models.Base;
using DAL.Models.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace DAL.Models
{
    [Table("Orders")]
    public class Order : Entity
    {
        public string Email { get; set; }
        public OrderStatus OrderStatus {  get; set; } 
        public List<OrderItem> Items { get; set; }
        public string? ReceiptsAddress {  get; set; } 
    }

    [Table("Items")]
    public class OrderItem : Entity
    {
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}
