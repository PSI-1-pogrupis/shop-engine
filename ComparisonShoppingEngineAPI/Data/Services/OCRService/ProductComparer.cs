using ComparisonShoppingEngineAPI.Data.Models;
using ComparisonShoppingEngineAPI.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComparisonShoppingEngineAPI.Data.Services.OCRService
{
    public class ProductComparer : IProductComparerService
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


        public async Task<List<ScannedItemDto>> GetItemsWithBetterPricesAsync(List<ScannedItemDto> products)
        {
            // this method is a bit different to one used in wpf as this one returns only products that have better price

            var newProductList = new List<ScannedItemDto>();
            List<ProductDto> dataList;

            var response = await _productService.GetAll();
            if (response.Success)
            {
                dataList = response.Data;
            }
            else
            {
                return newProductList;
            }

            // we need to compare price per quantity if the product is measured in some type of units:
            MovePricePerQToPrice(products);

            decimal lowest;
            string lowestShop;

            foreach (var scannedProduct in products)
            {
                foreach (var dataProduct in dataList)
                {
                    if (scannedProduct.Name == dataProduct.Name)
                    {
                        lowest = scannedProduct.Price;
                        lowestShop = null;
                        foreach (KeyValuePair<string, decimal> shopPrice in dataProduct.ShopPrices)
                        {

                            if (shopPrice.Key != scannedProduct.Shop && shopPrice.Value < lowest)
                            {
                                lowestShop = shopPrice.Key;
                                lowest = shopPrice.Value;
                            }
                        }
                        if(lowestShop != null)
                        {
                            newProductList.Add(new ScannedItemDto
                            {
                                Name = scannedProduct.Name,
                                Shop = lowestShop,
                                Price = lowest
                            });
                        }
                    }
                }
            }

            return newProductList;

        }

        private void MovePricePerQToPrice(List<ScannedItemDto> products)
        {
            foreach(ScannedItemDto product in products)
            {
                if (product.PricePerQuantity != 0)
                    product.Price = product.PricePerQuantity;
            }
        }
    }
}
