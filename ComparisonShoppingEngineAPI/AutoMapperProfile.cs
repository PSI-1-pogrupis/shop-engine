using AutoMapper;
using ComparisonShoppingEngineAPI.Data.Models;
using ComparisonShoppingEngineAPI.Dtos.Product;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComparisonShoppingEngineAPI
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Product, GetProductDto>().ConstructUsing(x => new GetProductDto()
            {
                ProductName = x.ProductName,
                ProductUnit = x.ProductUnit,
                ShopPrices = x.ShopProduct.ToDictionary(a => a.Shop.ShopName, a => a.Price)
            });
            CreateMap<AddProductDto, Product>();
            CreateMap<GetProductDto, Product>();
        }
    }
}
