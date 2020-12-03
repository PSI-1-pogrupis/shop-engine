using ComparisonShoppingEngineAPI.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComparisonShoppingEngineAPI.Data.Services.OCRService
{
    public interface IProductComparerService
    {
        public Task<List<ScannedItemDto>> ChangeIncorrectNamesAsync(List<ScannedItemDto> scannedList);

        public List<ScannedItemDto> ChangeIncorrectNames(List<ScannedItemDto> scannedList);

        public Task<List<ScannedItemDto>> GetItemsWithBetterPricesAsync(List<ScannedItemDto> products);
    }
}
