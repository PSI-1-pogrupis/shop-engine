using ComparisonShoppingEngineAPI.Data.Models;
using ComparisonShoppingEngineAPI.Data.Services.ReceiptService;
using ComparisonShoppingEngineAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ComparisonShoppingEngineAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("receipt")]
    public class ReceiptController : ControllerBase
    {
        private readonly ILogger<ReceiptController> _logger;
        private readonly IReceiptService _receiptService;

        public ReceiptController(ILogger<ReceiptController> logger, IReceiptService receiptService)
        {
            _logger = logger;
            _receiptService = receiptService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllReceipts()
        {
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            ServiceResponse<List<GetReceiptDto>> serviceResponse = await _receiptService.GetAll(userId);

            if (!serviceResponse.Success) return NotFound(serviceResponse);
            else
            {
                return Ok(serviceResponse);
            }
        }

        [HttpPost]
        public async Task<IActionResult> InsertReceipt([FromBody] AddReceiptDto receipt)
        {
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            ServiceResponse<List<GetReceiptDto>> serviceResponse = await _receiptService.Insert(receipt, userId);

            if (!serviceResponse.Success) return NotFound(serviceResponse);
            else
            {
                return Ok(serviceResponse);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteReceipt(int id)
        {
            int userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value);
            ServiceResponse<GetReceiptDto> serviceResponse = await _receiptService.Remove(id, userId);

            if (!serviceResponse.Success) return NotFound(serviceResponse);
            else
            {
                return Ok(serviceResponse);
            }
        }
    }
}
