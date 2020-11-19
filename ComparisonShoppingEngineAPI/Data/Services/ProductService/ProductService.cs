using AutoMapper;
using ComparisonShoppingEngineAPI.Data.Models;
using ComparisonShoppingEngineAPI.Dtos.Product;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ComparisonShoppingEngineAPI.Data
{
    public partial class ProductService : IProductService
    {

        private readonly IMapper _mapper;
        private readonly DataContext _context;

        public ProductService(IMapper mapper, DataContext context)
        {
            _mapper = mapper;
            _context = context;
        }

        async Task<ServiceResponse<List<GetProductDto>>> IProductService.GetAll()
        {
            ServiceResponse<List<GetProductDto>> serviceResponse = new ServiceResponse<List<GetProductDto>>();
            List<Product> dbProducts = await _context.Products.Include(a => a.ShopProduct).ThenInclude(a => a.Shop).ToListAsync();
            serviceResponse.Data = dbProducts.Select(p => _mapper.Map<GetProductDto>(p)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<GetProductDto>> GetProductByName(string name)
        {
            ServiceResponse<GetProductDto> serviceResponse = new ServiceResponse<GetProductDto>();
            Product dbProduct = await _context.Products.FirstOrDefaultAsync(x => x.ProductName == name);
            serviceResponse.Data = _mapper.Map<GetProductDto>(dbProduct);
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetProductDto>>> Insert(AddProductDto product)
        {
            ServiceResponse<List<GetProductDto>> serviceResponse = new ServiceResponse<List<GetProductDto>>();
            if (await _context.Products.AnyAsync(a => a.ProductName == product.Name)) return await Update(product);

            Product newProduct = new Product()
            {
                ProductName = product.Name,
                ProductUnit = product.Unit,
            };

            await _context.Products.AddAsync(newProduct);
            await _context.SaveChangesAsync();
            
            foreach (KeyValuePair<string, decimal> price in product.ShopPrices)
            {
                var shop = await _context.Shops.Where(a => a.ShopName == price.Key).SingleOrDefaultAsync();

                if (shop == null) continue;

                await _context.ShopProducts.AddAsync(new ShopProduct()
                {
                    ProductId = newProduct.ProductId,
                    ShopId = shop.ShopId,
                    Price = price.Value
                });
            }
            await _context.SaveChangesAsync();
            serviceResponse.Data = _context.Products.Select(p => _mapper.Map<GetProductDto>(p)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<GetProductDto>>> Delete(string name)
        {
            ServiceResponse<List<GetProductDto>> serviceResponse = new ServiceResponse<List<GetProductDto>>();
            var foundProduct = await _context.Products.FirstOrDefaultAsync(x => x.ProductName == name);
            if (foundProduct == null) return null;

            _context.Products.Remove(foundProduct);
            await _context.SaveChangesAsync();
            serviceResponse.Data = _context.Products.Select(p => _mapper.Map<GetProductDto>(p)).ToList();
            return serviceResponse;
        }

        // Updates the information of a product
        public async Task<ServiceResponse<List<GetProductDto>>> Update(AddProductDto product)
        {
            ServiceResponse<List<GetProductDto>> serviceResponse = new ServiceResponse<List<GetProductDto>>();
            serviceResponse.Data = null;
            Product foundProduct = await _context.Products.Include(x => x.ShopProduct).ThenInclude(x => x.Shop).FirstOrDefaultAsync();

            if (foundProduct == null) return serviceResponse;

            foundProduct.ProductUnit = product.Unit;
            
            foreach(KeyValuePair<string, decimal> price in product.ShopPrices)
            {
                var shopProduct = foundProduct.ShopProduct.Where(a => a.Shop.ShopName == price.Key).SingleOrDefault();
                var shop = await _context.Shops.Where(a => a.ShopName == price.Key).SingleOrDefaultAsync();

                if (shop == null) continue;

                if (shopProduct != null) {
                    shopProduct.Price = price.Value;
                    _context.ShopProducts.Update(shopProduct);
                }
                else await _context.ShopProducts.AddAsync(new ShopProduct()
                {
                    ProductId = foundProduct.ProductId,
                    ShopId = shop.ShopId,
                    Price = price.Value
                });
            }
            await _context.SaveChangesAsync();
            serviceResponse.Data = _context.Products.Select(p => _mapper.Map<GetProductDto>(p)).ToList();
            return serviceResponse;
        }

        // finds all prices for a particular item.
        private Dictionary<string, decimal> FindDictionary(string name)
        {
            Dictionary<string, decimal> dictionary = new Dictionary<string, decimal>();

            try
            {
                dictionary = _context.ShopProducts.Include(x => x.Shop).Where(x => x.Product.ProductName == name).ToDictionary(x => x.Shop.ShopName, x => x.Price);
            }
            catch (Exception e)
            {
                Debug.Print(e.StackTrace);
            }

            return dictionary;
        }
    }
}
