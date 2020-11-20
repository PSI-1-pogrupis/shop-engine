﻿using AutoMapper;
using ComparisonShoppingEngineAPI.Data.Models;
using ComparisonShoppingEngineAPI.DTOs.Product;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
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

        async Task<ServiceResponse<List<ProductDto>>> IProductService.GetAll()
        {
            ServiceResponse<List<ProductDto>> serviceResponse = new ServiceResponse<List<ProductDto>>();
            List<Product> dbProducts = await _context.Products.Include(a => a.ShopProduct).ThenInclude(a => a.Shop).ToListAsync();
            serviceResponse.Data = dbProducts.Select(p => _mapper.Map<ProductDto>(p)).ToList();
            return serviceResponse;
        }

        public async Task<ServiceResponse<ProductDto>> GetProductByName(string name)
        {
            ServiceResponse<ProductDto> serviceResponse = new ServiceResponse<ProductDto>();
            Product dbProduct = await _context.Products.FirstOrDefaultAsync(x => x.ProductName == name);
            serviceResponse.Data = _mapper.Map<ProductDto>(dbProduct);
            return serviceResponse;
        }

        public async Task<ServiceResponse<List<ProductDto>>> Insert(ProductDto product)
        {
            ServiceResponse<List<ProductDto>> serviceResponse = new ServiceResponse<List<ProductDto>>();
            if (await _context.Products.AnyAsync(a => a.ProductName == product.ProductName)) return await Update(product);

            Product newProduct = new Product()
            {
                ProductName = product.ProductName,
                ProductUnit = product.ProductUnit,
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
            serviceResponse.Data = _context.Products.Include(x => x.ShopProduct).ThenInclude(x => x.Shop).Where(x => x.ProductName == product.ProductName).Select(p => _mapper.Map<ProductDto>(p)).ToList();
            return serviceResponse;
        }

        /*public async Task<ServiceResponse<List<ProductDto>>> Delete(string name)
        {
            ServiceResponse<List<ProductDto>> serviceResponse = new ServiceResponse<List<ProductDto>>();
            var foundProduct = await _context.Products.FirstOrDefaultAsync(x => x.ProductName == name);
            if (foundProduct == null) return null;

            _context.Products.Remove(foundProduct);
            await _context.SaveChangesAsync();
            serviceResponse.Data = _context.Products.Include(x => x.ShopProduct).ThenInclude(x => x.Shop).Where(x => x.ProductName == name).Select(p => _mapper.Map<ProductDto>(p)).ToList();
            return serviceResponse;
        }*/

        // Updates the information of a product
        public async Task<ServiceResponse<List<ProductDto>>> Update(ProductDto product)
        {
            ServiceResponse<List<ProductDto>> serviceResponse = new ServiceResponse<List<ProductDto>>();
            serviceResponse.Data = null;
            Product foundProduct = await _context.Products.Include(x => x.ShopProduct).ThenInclude(x => x.Shop).Where(x => x.ProductName == product.ProductName).FirstOrDefaultAsync();

            if (foundProduct == null) return serviceResponse;

            foundProduct.ProductUnit = product.ProductUnit;

            foreach (ShopProduct shopProduct in foundProduct.ShopProduct) _context.ShopProducts.Remove(shopProduct);

            foreach (KeyValuePair<string, decimal> price in product.ShopPrices)
            {
                var shop = await _context.Shops.Where(a => a.ShopName == price.Key).SingleOrDefaultAsync();

                if (shop == null) continue;

                _context.ShopProducts.Update(new ShopProduct()
                {
                    ProductId = foundProduct.ProductId,
                    ShopId = shop.ShopId,
                    Price = price.Value
                });
            }
            await _context.SaveChangesAsync();
            serviceResponse.Data = _context.Products.Include(x => x.ShopProduct).ThenInclude(x => x.Shop).Where(x => x.ProductName == product.ProductName).Select(p => _mapper.Map<ProductDto>(p)).ToList();
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