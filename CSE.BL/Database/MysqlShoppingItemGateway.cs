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

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<ShopProduct> ShopProducts { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(ConfigurationManager.ConnectionStrings["HerokuMySqlDatabaseConnectionString"].ConnectionString, x => x.ServerVersion("8.0.22-mysql"));
            }
        }
        // Retrieve shop prices for specified shopping item
        private Dictionary<ShopTypes, decimal> FindDictionary(string name)
        {
            Dictionary<ShopTypes, decimal> dictionary = new Dictionary<ShopTypes, decimal>();
            // add or update item
            foreach (ShopProduct i in ShopProducts.Where(a => a.ProductName == name).ToList())
            {
                try
                {
                    dictionary.Add((ShopTypes)Enum.Parse(typeof(ShopTypes), i.ShopName, true), i.Price);
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
            }
            return dictionary;
        }
        // Insert and if exists update shopping item
        private void InsertDictionary(ShoppingItemData item)
        {
            foreach (var i in item.ShopPrices)
            {
                try
                {
                    ShopProduct product = ShopProducts.Where(a => a.ProductName == item.Name && a.ShopName == i.Key.ToString()).ToList().First();
                    product.Price = i.Value;
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
                    ShopProducts.Add(new ShopProduct { ProductName = item.Name, ShopName = i.Key.ToString(), Price = i.Value });
                }
            }
        }
        // Remove shopping item prices from all shops
        private void RemoveDictionary(ShoppingItemData item)
        {
            ShopProducts.RemoveRange(ShopProducts.Where(a => a.ProductName == item.Name));
        }
        // Find shopping item by name
        public ShoppingItemData Find(string name)
        {
            Dictionary<ShopTypes, decimal> dictionary = FindDictionary(name);

            try
            {
                var s = Products.Where(a => a.Name == name).Single();
                try
                {
                    return new ShoppingItemData(s.Name, (UnitTypes)Enum.Parse(typeof(UnitTypes), s.Unit), dictionary);
                }
                catch (ArgumentNullException e)
                {
                    Debug.Print(e.StackTrace);
                    return null;
                }
                catch (ArgumentException e)
                {
                    Debug.Print(e.StackTrace);
                    return null;
                }
                catch (OverflowException e)
                {
                    Debug.Print(e.StackTrace);
                    return null;
                }
                catch (Exception e)
                {
                    Debug.Print(e.StackTrace);
                    return null;
                }
            }
            catch (Exception e)
            {
                Debug.Print(e.StackTrace);
                return null;
            }
        }
        // Retrieve all shopping items
        public List<ShoppingItemData> GetAll()
        {
            List<ShoppingItemData> itemList = new List<ShoppingItemData>();
            var allProducts = Products.ToList();
            foreach (var i in allProducts)
            {
                try
                {
                    itemList.Add(new ShoppingItemData(i.Name, (UnitTypes)Enum.Parse(typeof(UnitTypes), i.Unit), FindDictionary(i.Name)));
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
            }
            return itemList;
        }
        // Insert and if exists update shopping item
        public void Insert(ShoppingItemData item)
        {
            var checkItem = from p in Products
                            where p.Name == item.Name
                            select p;
            try
            {
                if (checkItem.FirstOrDefault() == null)
                {
                    Products.Add(new Product { Name = item.Name, Unit = item.Unit.ToString() });
                }
                else
                {
                    var i = checkItem.First();
                    i.Unit = item.Unit.ToString();
                    Products.Update(i);
                }
                InsertDictionary(item);
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

        public void Remove(ShoppingItemData item)
        {
            if (item != null)
            {
                try
                {
                    Products.Remove(Products.Where(a => a.Name == item.Name).Single());
                }
                finally
                {
                    RemoveDictionary(item);
                }
            }
        }

        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("product");

                entity.HasIndex(e => e.Id)
                    .HasName("id_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");

                entity.Property(e => e.Unit)
                    .IsRequired()
                    .HasColumnName("unit")
                    .HasColumnType("enum('kg','g','l','ml','m','cm','piece')")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_general_ci");
            });

            modelBuilder.Entity<ShopProduct>(entity =>
            {
                entity.ToTable("shop_product");

                entity.Property(e => e.Id)
                    .HasColumnName("id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Price)
                    .HasColumnName("price")
                    .HasColumnType("decimal(4,2)");

                entity.Property(e => e.ProductName)
                    .IsRequired()
                    .HasColumnName("product_name")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.ShopName)
                    .HasColumnName("shop_name")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
