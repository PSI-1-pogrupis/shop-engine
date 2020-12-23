using AutoMapper;
using ComparisonShoppingEngineAPI.Data.Models;
using ComparisonShoppingEngineAPI.DTOs;
using ComparisonShoppingEngineAPI.DTOs.Report;
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

            CreateMap<Receipt, GetReceiptDto>().ForMember(d => d.Shop, o => o.MapFrom(s => s.Shop.ShopName))
                .ForMember(d => d.Id, o => o.MapFrom(s => s.ReceiptId));

            CreateMap<ReceiptProduct, GetReceiptProductDto>().ConstructUsing(x => new GetReceiptProductDto() {
                Name = x.Product.ProductName,
                Discount = x.Discount,
                Price = x.Price,
                PricePerQuantity = x.PricePerQuantity
            });

            CreateMap<Report, GetReportDto>();
        }
    }
}
