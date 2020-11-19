using ComparisonShoppingEngineAPI.Data;
using ComparisonShoppingEngineAPI.Data.Models;
using ComparisonShoppingEngineAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace ComparisonShoppingEngineAPI.Services
{
    public class ProductService : IProductService
    {
        private readonly DatabaseContext _context;

        public ProductService(DatabaseContext context)
        {
            _context = context;
        }

        public void Delete(ProductDto product)
        {
            throw new NotImplementedException();
        }

        public IEnumerable<ProductDto> GetAll()
        {
            
            List<ProductDto> productList = new List<ProductDto>();
            var allProducts = _context.Products.Include(x => x.ShopProducts).ThenInclude(x => x.Shop).ToList();

            foreach (var singleProduct in allProducts)
            {
                productList.Add(new ProductDto()
                {
                    Name = singleProduct.Name,
                    Unit = singleProduct.Unit,
                    ShopPrices = singleProduct.ShopProducts.ToDictionary(x => x.Shop.Name, x => x.Price)
                });
            }

            return productList;
            
        }

        public ProductDto GetProductByName(string name)
        {
            if (name != null && name.Length > 0)
            {
                try
                {
                    var foundItem = _context.Products.Where(a => a.Name == name).Single();
                    return new ProductDto()
                    {
                        Name = foundItem.Name,
                        Unit = foundItem.Unit,
                        ShopPrices = FindDictionary(name)
                    };
                }
                catch (Exception e)
                {
                    Debug.Print(e.StackTrace);
                    return null;
                }
            }

            return null;
        }

        public ProductDto Insert(ProductDto product)
        {
            if (_context.Products.Any(a => a.Name == product.Name)) return Update(product);

            ProductEntity newProduct = new ProductEntity()
            {
                Name = product.Name,
                Unit = product.Unit,
            };

            _context.Products.Add(newProduct);
            _context.SaveChanges();

            foreach (KeyValuePair<string, decimal> price in product.ShopPrices)
            {
                var shop = _context.Shops.Where(a => a.Name == price.Key).SingleOrDefault();

                if (shop == null) continue;

                _context.ShopProducts.Add(new ShopProductEntity()
                {
                    ProductId = newProduct.Id,
                    ShopId = shop.Id,
                    Price = price.Value
                });
            }

            _context.SaveChanges();

            return new ProductDto()
            {
                Name = newProduct.Name,
                Unit = newProduct.Unit,
                ShopPrices = newProduct.ShopProducts.ToDictionary(x => x.Shop.Name, x => x.Price)
            };
        }

        public ProductDto Update(ProductDto product)
        {
            var allProducts = _context.Products.Include(x => x.ShopProducts).ThenInclude(x => x.Shop).ToList();
            var foundProduct = allProducts.Where(a => a.Name == product.Name.ToUpper()).SingleOrDefault();

            if (foundProduct == null) return null;

            foundProduct.Unit = product.Unit;

            foreach (ShopProductEntity shopProduct in foundProduct.ShopProducts) _context.ShopProducts.Remove(shopProduct);

            foreach (KeyValuePair<string, decimal> price in product.ShopPrices)
            {
                var shop = _context.Shops.Where(a => a.Name == price.Key).SingleOrDefault();

                if (shop == null) continue;

                _context.ShopProducts.Add(new ShopProductEntity()
                {
                    ProductId = foundProduct.Id,
                    ShopId = shop.Id,
                    Price = price.Value
                });
            }

            _context.SaveChanges();

            return new ProductDto()
            {
                Name = foundProduct.Name,
                Unit = foundProduct.Unit,
                ShopPrices = foundProduct.ShopProducts.ToDictionary(x => x.Shop.Name, x => x.Price)
            };
        }

        private Dictionary<string, decimal> FindDictionary(string name)
        {
            Dictionary<string, decimal> dictionary = new Dictionary<string, decimal>();

            try
            {
                dictionary = _context.ShopProducts.Include(x => x.Shop).Where(x => x.Product.Name == name).ToDictionary(x => x.Shop.Name, x => x.Price);
            }
            catch (Exception e)
            {
                Debug.Print(e.StackTrace);
            }

            return dictionary;
        }
    }
}
