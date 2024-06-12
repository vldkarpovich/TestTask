using DAL.Models;

namespace ReceiptService.Interfaces
{
    public interface IReceiptGenerateService
    {
        Task<byte[]> GenerateReceiptAsync(Order order);
    }
}
