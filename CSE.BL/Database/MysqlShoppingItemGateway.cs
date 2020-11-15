using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using CSE.BL.Interfaces;
using CSE.BL.ShoppingList;
using Microsoft.EntityFrameworkCore;

namespace CSE.BL.Database.Models
{
    public partial class MysqlShoppingItemGateway : DbContext, IShoppingItemGateway
    {
        public MysqlShoppingItemGateway()
        {
        }

        public MysqlShoppingItemGateway(DbContextOptions<MysqlShoppingItemGateway> options)
            : base(options)
        {
        }

        public virtual DbSet<ProductModel> Products { get; set; }
        public virtual DbSet<ShopModel> Shops { get; set; }
        public virtual DbSet<ShopProductModel> ShopProducts { get; set; }
        public virtual DbSet<UserQuestionModel> UserQuestions { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(ConfigurationManager.ConnectionStrings["HerokuMySqlDatabaseConnectionString"].ConnectionString, x => x.ServerVersion("8.0.22-mysql"));
            }
        }
        // Retrieve shop prices for specified shop item
        private Dictionary<ShopTypes, decimal> FindDictionary(string name)
        {
            Dictionary<ShopTypes, decimal> dictionary = new Dictionary<ShopTypes, decimal>();

            try
            {
                dictionary = ShopProducts.Include(x => x.Shop).Where(x => x.Product.ProductName == name).ToDictionary(x => (ShopTypes)Enum.Parse(typeof(ShopTypes), x.Shop.ShopName), x => x.Price);
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
        private void InsertDictionary(ShoppingItemData shopItem)
        {
            foreach (var newShopPrice in shopItem.ShopPrices)
            {
                try
                {
                    // Update all shop item shops prices if specific product exist in shop
                    var product = ShopProducts.Include(x=>x.Shop).Include(x=>x.Product).Where(x => x.Product.ProductName == shopItem.Name).Single();
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
                    ShopProducts.Add(new ShopProductModel {
                                        ProductId = productId,
                                        ShopId = shopId,
                                        Price = newShopPrice.Value });
                }
            }
        }
        // Find shop item by name
        public ShoppingItemData Find(string name)
        {
            if (name != null && name.Length > 0)
            {
                try
                {
                    var foundItem = Products.Where(a => a.ProductName == name).Single();
                    return new ShoppingItemData(foundItem.ProductName, (UnitTypes)Enum.Parse(typeof(UnitTypes), foundItem.ProductUnit), FindDictionary(name));
                }
                catch (Exception e)
                {
                    Debug.Print(e.StackTrace);
                    return null;
                }
            }

            return null;
        }
        // Retrieve all shop items
        public List<ShoppingItemData> GetAll()
        {
            List<ShoppingItemData> itemList = new List<ShoppingItemData>();
            var allProducts = Products.Include(x => x.ShopProduct).ThenInclude(x => x.Shop).ToList();
            
            foreach (var singleProduct in allProducts)
            {
                itemList.Add(new ShoppingItemData(singleProduct.ProductName, (UnitTypes)Enum.Parse(typeof(UnitTypes), singleProduct.ProductUnit),
                    singleProduct.ShopProduct.ToDictionary(x => (ShopTypes)Enum.Parse(typeof(ShopTypes), x.Shop.ShopName), x => x.Price)));
            }

            return itemList;
        }
        // Insert or if exists update shop item with its shop prices dictionary
        public void Insert(ShoppingItemData shopItem)
        {
            if (shopItem != null)
            {
                try
                {
                    var product = Products.SingleOrDefault(x => x.ProductName == shopItem.Name);

                    if (product == null)
                    {
                        Products.Add(new ProductModel { ProductName = shopItem.Name, ProductUnit = shopItem.Unit.ToString().ToUpper() });
                    }
                    else
                    {
                        product.ProductUnit = shopItem.Unit.ToString();
                        Products.Update(product);
                    }

                    InsertDictionary(shopItem);
                }
                catch (ArgumentNullException)
                {

                }
                catch (InvalidOperationException)
                {

                }
                catch (Exception e)
                {
                    Debug.Print(e.StackTrace);
                }
            }
        }
        // Remove shop item and its all shop prices
        public void Remove(ShoppingItemData shopItem)
        {
            if (shopItem != null)
            {
                try
                {
                    Products.Remove(Products.Where(a => a.ProductName == shopItem.Name).Single());
                }
                catch (Exception e)
                {
                    Debug.Print(e.StackTrace);
                }
            }
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

            modelBuilder.Entity<UserQuestionModel>(entity =>
            {
                entity.ToTable("user_question");

                entity.HasIndex(e => e.Id)
                    .HasName("id_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasColumnName("email")
                    .HasColumnType("varchar(320)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.Question)
                    .IsRequired()
                    .HasColumnName("question")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
