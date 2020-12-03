using ComparisonShoppingEngineAPI.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComparisonShoppingEngineAPI.Data.Services.OCRService
{
    public interface IItemScannerService
    {
        public Task<List<ScannedItemDto>> ScanProductsAsync(string text);

        public List<ScannedItemDto> ScanProducts(string text);
    }
}
