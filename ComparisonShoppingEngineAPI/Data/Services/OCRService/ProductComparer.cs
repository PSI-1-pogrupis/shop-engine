using ComparisonShoppingEngineAPI.Data.Models;
using ComparisonShoppingEngineAPI.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComparisonShoppingEngineAPI.Data.Services.OCRService
{
    public class ProductComparer
    {
        private readonly IProductService _productService;

        public ProductComparer(IProductService productService)
        {
            _productService = productService;
        }

        public async Task<List<ScannedItemDto>> ChangeIncorrectNamesAsync(List<ScannedItemDto> scannedList)
        {
            return await Task.Run(() => ChangeIncorrectNames(scannedList));
        }
        public List<ScannedItemDto> ChangeIncorrectNames(List<ScannedItemDto> scannedList)
        {
            List<ScannedItemDto> updatedList = new List<ScannedItemDto>();

            foreach (var scannedProduct in scannedList)
            {
                var similar = _productService.GetProductBySimilarName(scannedProduct.Name);

                if(similar.Success && similar.Data != null)
                {
                    scannedProduct.Name = similar.Data.Name;
                    updatedList.Add(scannedProduct);
                }
            }

            return updatedList;
        }
    }
}
