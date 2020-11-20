using AutoMapper;
using ComparisonShoppingEngineAPI.Data.Models;
using ComparisonShoppingEngineAPI.DTOs.Product;
using System.Linq;

namespace ComparisonShoppingEngineAPI
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Product, ProductDto>().ConstructUsing(x => new ProductDto()
            {
                Name = x.ProductName,
                Unit = x.ProductUnit,
                ShopPrices = x.ShopProduct.ToDictionary(a => a.Shop.ShopName, a => a.Price)
            });
            CreateMap<ProductDto, Product>();
        }
    }
}
