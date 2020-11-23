using AutoMapper;
using ComparisonShoppingEngineAPI.Data.Models;
using ComparisonShoppingEngineAPI.DTOs;
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
                Id = x.ReceiptId,
                Shop = new ShopDto { ShopId = x.ShopId, ShopName = x.Shop.ShopName },
                Date = x.Date,
                Total = x.Total,
                Products = x.ReceiptProducts
            });
            CreateMap<GetReceiptProductDto, Receipt>();

            CreateMap<Receipt, AddReceiptDto>().ConstructUsing(x => new AddReceiptDto()
            {
                ShopId = x.ShopId,
                Total = x.Total
            }).ForMember(dest => dest.Products, opt => opt.MapFrom(x => x.ReceiptProducts));
            CreateMap<AddReceiptDto, Receipt>();

            CreateMap<ReceiptProduct, GetReceiptProductDto>().ConstructUsing(x => new GetReceiptProductDto()
            {
                Id = x.ReceiptId,
                Name = x.Product.ProductName,
                Amount = x.Amount,
                Price = x.ProductPrice
            });
            CreateMap<GetReceiptProductDto, ReceiptProduct>();

            CreateMap<ReceiptProduct, AddReceiptProductDto>().ConstructUsing(x => new AddReceiptProductDto()
            {
                ProductId = x.ProductId,
                Amount = x.Amount,
                Price = x.ProductPrice
            });
            CreateMap<AddReceiptProductDto, ReceiptProduct>();

            CreateMap<Shop, ShopDto>().ConstructUsing(x => new ShopDto()
            {
                ShopId = x.ShopId,
                ShopName = x.ShopName
            });
            CreateMap<ShopDto, Shop>();

            CreateMap<ReceiptProduct, ReceiptProductDto>().ConstructUsing(x => new ReceiptProductDto()
            {
                Id = x.ReceiptProductId,
                Name = x.Product.ProductName,
                Amount = x.Amount,
                Price = x.ProductPrice
            });
            CreateMap<ReceiptProductDto, ReceiptProduct>();
        }
    }
}
