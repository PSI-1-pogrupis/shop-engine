using ComparisonShoppingEngineAPI.Data.Models;
using ComparisonShoppingEngineAPI.Models;
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
    public partial class MySqlProductRepository : DbContext, IProductRepository
    {
        public virtual DbSet<ProductModel> Products { get; set; }
        public virtual DbSet<ShopModel> Shops { get; set; }
        public virtual DbSet<ShopProductModel> ShopProducts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json")
                .Build();
            optionsBuilder.UseMySql(configuration.GetConnectionString("DefaultConnection"));
        }
        private Dictionary<string, decimal> FindDictionary(string name)
        {
            Dictionary<string, decimal> dictionary = new Dictionary<string, decimal>();

            try
            {
                dictionary = ShopProducts.Include(x => x.Shop).Where(x => x.Product.ProductName == name).ToDictionary(x => x.Shop.ShopName, x => x.Price);
            }
            catch (ArgumentNullException e)
            {
                Debug.Print(e.StackTrace);
            }
            catch (ArgumentException e)
            {
                Debug.Print(e.StackTrace);
            }
            catch (OverflowException e)
            {
                Debug.Print(e.StackTrace);
            }
            catch (Exception e)
            {
                Debug.Print(e.StackTrace);
            }

            return dictionary;
        }
        // Insert and if exists update shop item
        private void InsertDictionary(ProductData shopItem)
        {
            foreach (var newShopPrice in shopItem.ShopPrices)
            {
                try
                {
                    // Update all shop item shops prices if specific product exist in shop
                    var product = ShopProducts.Include(x => x.Shop).Include(x => x.Product).Where(x => x.Product.ProductName == shopItem.Name).Single();
                    product.Price = newShopPrice.Value;
                    product.Date = DateTime.Now;
                    ShopProducts.Update(product);
                }
                catch (ArgumentException e)
                {
                    Debug.Print(e.StackTrace);
                }
                catch (InvalidOperationException e)
                {
                    Debug.Print(e.StackTrace);
                }
                catch (Exception e)
                {
                    Debug.Print(e.StackTrace);
                    // Shop item prices do not exist therefore create new
                    int productId = Products.Where(a => a.ProductName == shopItem.Name).Single().ProductId;
                    int shopId = Shops.Where(a => a.ShopName == newShopPrice.Key.ToString().ToUpper()).Single().ShopId;
                    ShopProducts.Add(new ShopProductModel
                    {
                        ProductId = productId,
                        ShopId = shopId,
                        Price = newShopPrice.Value
                    });
                }
            }
        }
        // Find shop item by name
        public ProductData Find(string name)
        {
            if (name != null && name.Length > 0)
            {
                try
                {
                    var foundItem = Products.Where(a => a.ProductName == name).Single();
                    return new ProductData() { Name = foundItem.ProductName, Unit = foundItem.ProductUnit, ShopPrices = FindDictionary(name) };
                }
                catch (Exception e)
                {
                    Debug.Print(e.StackTrace);
                    return null;
                }
            }

            return null;
        }
        
        // Query made changes into database
        public override int SaveChanges()
        {
            return base.SaveChanges();
        }
        // MySQL Schema modeling for EF
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductModel>(entity =>
            {
                entity.HasKey(x => new { x.ProductId });
                entity.ToTable("product");

                entity.HasIndex(e => e.ProductId)
                    .HasName("product_id_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.ProductId)
                    .HasColumnName("product_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasColumnName("product_name")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.ProductUnit)
                    .IsRequired()
                    .HasColumnName("product_unit")
                    .HasColumnType("enum('kg','g','l','ml','m','cm','piece')")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<ShopModel>(entity =>
            {
                entity.HasKey(x => new { x.ShopId });
                entity.ToTable("shop");

                entity.HasIndex(e => e.ShopId)
                    .HasName("shop_id_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.ShopId)
                    .HasColumnName("shop_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.ShopName)
                    .IsRequired()
                    .HasColumnName("shop_name")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<ShopProductModel>(entity =>
            {
                entity.HasKey(e => new { e.ShopProductId, e.ProductId })
                    .HasName("PRIMARY");

                entity.ToTable("shop_product");

                entity.HasIndex(e => e.ProductId)
                    .HasName("fk_shop_product_product_idx");

                entity.HasIndex(e => e.ShopId)
                    .HasName("fk_shop_product_shop_idx");

                entity.HasIndex(e => e.ShopProductId)
                    .HasName("shop_product_id_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.ShopProductId)
                    .HasColumnName("shop_product_id")
                    .HasColumnType("int(11)")
                    .ValueGeneratedOnAdd();

                entity.Property(e => e.ProductId)
                    .HasColumnName("product_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Price)
                    .HasColumnName("price")
                    .HasColumnType("decimal(4,2)");

                entity.Property(e => e.ShopId)
                    .HasColumnName("shop_id")
                    .HasColumnType("int(11)");

                entity.HasOne(d => d.Product)
                    .WithMany(p => p.ShopProduct)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("fk_shop_product_product");

                entity.HasOne(d => d.Shop)
                    .WithMany(p => p.ShopProduct)
                    .HasForeignKey(d => d.ShopId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("fk_shop_product_shop");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

        IEnumerable<ProductData> IProductRepository.GetAll()
        {
            List<ProductData> productList = new List<ProductData>();
            var allProducts = Products.Include(x => x.ShopProduct).ThenInclude(x => x.Shop).ToList();

            foreach (var singleProduct in allProducts)
            {
                productList.Add(new ProductData() { 
                    Name = singleProduct.ProductName,
                    Unit = singleProduct.ProductUnit,
                    ShopPrices = singleProduct.ShopProduct.ToDictionary(x => x.Shop.ShopName, x => x.Price)
                    });
            }

            return productList;
        }

        public ProductData GetProductByName(string name)
        {
            if (name != null && name.Length > 0)
            {
                try
                {
                    var foundItem = Products.Where(a => a.ProductName == name).Single();
                    return new ProductData() {
                        Name = foundItem.ProductName,
                        Unit = foundItem.ProductUnit,
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

        public void Insert(ProductData product)
        {
            throw new NotImplementedException();
        }

        public void Delete(ProductData product)
        {
            throw new NotImplementedException();
        }

        public void Update(ProductData data)
        {
            throw new NotImplementedException();
        }
    }
}
