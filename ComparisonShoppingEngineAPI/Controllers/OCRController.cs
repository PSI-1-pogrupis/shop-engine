using ComparisonShoppingEngineAPI.Data;
using ComparisonShoppingEngineAPI.Data.Models;
using ComparisonShoppingEngineAPI.Data.Services.OCRService;
using ComparisonShoppingEngineAPI.DTOs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComparisonShoppingEngineAPI.Controllers
{
    [ApiController]
    [Route("ocr")]
    public class OCRController : ControllerBase
    {
        private readonly ILogger<OCRController> _logger;
        private readonly IOCRService _ocrService;
        private readonly IProductService _productService;

        public OCRController(ILogger<OCRController> logger, IOCRService ocrService, IProductService productService)
        {
            _logger = logger;
            _ocrService = ocrService;
            _productService = productService;
        }

        [HttpPost]
        public async Task<IActionResult> UploadImage()
        {
            var file = Request.Form.Files.FirstOrDefault();

            if (file == null || file.Length <= 0) return BadRequest();

            ServiceResponse<string> ocrResponse = await _ocrService.ReadImage(file);

            if (!ocrResponse.Success) return BadRequest(ocrResponse);

            ItemsScanner scanner = new ItemsScanner();
            ProductComparer comparer = new ProductComparer(_productService);

            List<ScannedItemDto> scannedItems = await scanner.ScanProductsAsync(ocrResponse.Data);
            scannedItems = await comparer.ChangeIncorrectNamesAsync(scannedItems);

            return Ok(scannedItems);
        }
    }
}
