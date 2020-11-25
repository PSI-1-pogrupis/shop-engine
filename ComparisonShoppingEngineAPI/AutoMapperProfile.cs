using AutoMapper;
using ComparisonShoppingEngineAPI.Data.Models;
using ComparisonShoppingEngineAPI.DTOs;
using System.Collections.Generic;
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

            CreateMap<Receipt, GetReceiptDto>().ConstructUsing(x => new GetReceiptDto()
            {
                Date = x.Date,
                Shop = x.Shop.ShopName,
                Total = x.Total
            }).ForMember(x => x.Products, opt => opt.MapFrom(x => x.ReceiptProducts));

            CreateMap<ICollection<ReceiptProduct>, ICollection<GetReceiptProductDto>>();

            CreateMap<ReceiptProduct, GetReceiptProductDto>().ConstructUsing(x => new GetReceiptProductDto() {
                Name = x.Product.ProductName,
                Discount = x.Discount,
                Price = x.Price,
                PricePerQuantity = x.PricePerQuantity
            });
        }
    }
}
