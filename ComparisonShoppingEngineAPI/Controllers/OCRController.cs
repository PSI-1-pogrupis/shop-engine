using ComparisonShoppingEngineAPI.Data;
using ComparisonShoppingEngineAPI.Data.Models;
using ComparisonShoppingEngineAPI.Data.Services.OCRService;
using ComparisonShoppingEngineAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComparisonShoppingEngineAPI.Controllers
{
    [Authorize]
    [ApiController]
    [Route("ocr")]
    public class OCRController : ControllerBase
    {
        private readonly ILogger<OCRController> _logger;
        private readonly IOCRService _ocrService;
        private readonly IProductComparerService _comparerService;
        private readonly IItemScannerService _scannerService;

        public OCRController(ILogger<OCRController> logger, IOCRService ocrService, IProductComparerService comparerService, IItemScannerService scannerService)
        {
            _logger = logger;
            _ocrService = ocrService;
            _comparerService = comparerService;
            _scannerService = scannerService;
        }
        [AllowAnonymous]
        [HttpPost("read")]
        public async Task<IActionResult> ReadImage()
        {
            var file = Request.Form.Files.FirstOrDefault();

            if (file == null || file.Length <= 0) return BadRequest();

            ServiceResponse<string> ocrResponse = await _ocrService.ReadImage(file);

            if (!ocrResponse.Success) return BadRequest(ocrResponse);

            List<ScannedItemDto> scannedItems = await _scannerService.ScanProductsAsync(ocrResponse.Data);
            scannedItems = await _comparerService.ChangeIncorrectNamesAsync(scannedItems);

            return Ok(scannedItems);
        }

        [AllowAnonymous]
        [HttpPost("scan")]
        public async Task<IActionResult> ScanImage()
        {
            var file = Request.Form.Files.FirstOrDefault();

            if (file == null || file.Length <= 0) return BadRequest();

            ServiceResponse<string> ocrResponse = await _ocrService.ReadImage(file);

            if (!ocrResponse.Success) return BadRequest(ocrResponse);

            List<ScannedItemDto> scannedItems = await _scannerService.ScanProductsAsync(ocrResponse.Data);

            return Ok(scannedItems);
        }

        [AllowAnonymous]
        [HttpPost("compare")]
        public async Task<IActionResult> CompareProducts([FromBody] ScannedItemDto[] products)
        {
            if (products.Length == 0) return BadRequest();

            List<ScannedItemDto> betterPricedItems = await _comparerService.GetItemsWithBetterPricesAsync(products.ToList());

            return Ok(betterPricedItems);
        }
    }
}
