using ComparisonShoppingEngineAPI.Data.Models;
using ComparisonShoppingEngineAPI.DTOs;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ComparisonShoppingEngineAPI.Data.Services.ReceiptService
{
    public interface IReceiptService
    {
        Task<ServiceResponse<List<GetReceiptDto>>> GetAll(int userId);
        Task<ServiceResponse<List<GetReceiptDto>>> Insert(AddReceiptDto receipt, int userId);
        Task<ServiceResponse<GetReceiptDto>> Remove(int receiptId, int userId);
    }
}
