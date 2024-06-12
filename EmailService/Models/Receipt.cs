namespace EmailService.Models
{
    public class Receipt
    {
        public Guid OrderId { get; set; }
        public string ReceiptPath { get; set; }
        public string Email { get; set; }
    }
}
