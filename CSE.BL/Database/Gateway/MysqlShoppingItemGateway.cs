using System;
using System.Collections.Generic;
using System.Configuration;
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

        public virtual DbSet<Iki> Iki { get; set; }
        public virtual DbSet<Lidl> Lidl { get; set; }
        public virtual DbSet<Maxima> Maxima { get; set; }
        public virtual DbSet<Norfa> Norfa { get; set; }
        public virtual DbSet<Rimi> Rimi { get; set; }
        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Unknown> Unknown { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseMySql(ConfigurationManager.ConnectionStrings["MySqlConnectionString"].ConnectionString, x => x.ServerVersion("8.0.22-mysql"));
            }
        }
        // Retrieve shop prices for specified shopping item
        private Dictionary<ShopTypes, double> FindDictionary(string name)
        {
            Dictionary<ShopTypes, double> dictionary = new Dictionary<ShopTypes, double>();
            foreach (Iki i in Iki.Where(a => a.Product == name).ToList())
                dictionary.Add(ShopTypes.IKI, i.Price);
            foreach (Maxima i in Maxima.Where(a => a.Product == name).ToList())
                dictionary.Add(ShopTypes.MAXIMA, i.Price);
            foreach (Lidl i in Lidl.Where(a => a.Product == name).ToList())
                dictionary.Add(ShopTypes.LIDL, i.Price);
            foreach (Norfa i in Norfa.Where(a => a.Product == name).ToList())
                dictionary.Add(ShopTypes.NORFA, i.Price);
            foreach (Rimi i in Rimi.Where(a => a.Product == name).ToList())
                dictionary.Add(ShopTypes.RIMI, i.Price);
            return dictionary;
        }
        // Insert and if exists update shopping item
        private void InsertDictionary(ShoppingItemData item)
        {
            foreach (var i in item.ShopPrices)
            {
                switch (i.Key)
                {
                    case ShopTypes.IKI:
                        try
                        {
                            Iki product = Iki.Where(a => a.Product == item.Name).ToList().First();
                            product.Price = i.Value;
                            product.Date = DateTime.Now;
                            Iki.Update(product);
                        }
                        catch (Exception)
                        {
                            Iki.Add(new Iki { Product = item.Name, Price = i.Value });
                        }
                        break;
                    case ShopTypes.MAXIMA:
                        try
                        {
                            Maxima product = Maxima.Where(a => a.Product == item.Name).ToList().First();
                            product.Price = i.Value;
                            product.Date = DateTime.Now;
                            Maxima.Update(product);
                        }
                        catch (Exception)
                        {
                            Maxima.Add(new Maxima { Product = item.Name, Price = i.Value });
                        }
                        break;
                    case ShopTypes.LIDL:
                        try
                        {
                            Lidl product = Lidl.Where(a => a.Product == item.Name).ToList().First();
                            product.Price = i.Value;
                            product.Date = DateTime.Now;
                            Lidl.Update(product);
                        }
                        catch (Exception)
                        {
                            Lidl.Add(new Lidl { Product = item.Name, Price = i.Value });
                        }
                        break;
                    case ShopTypes.NORFA:
                        try
                        {
                            Norfa product = Norfa.Where(a => a.Product == item.Name).ToList().First();
                            product.Price = i.Value;
                            product.Date = DateTime.Now;
                            Norfa.Update(product);
                        }
                        catch (Exception)
                        {
                            Norfa.Add(new Norfa { Product = item.Name, Price = i.Value });
                        }
                        break;
                    case ShopTypes.RIMI:
                        try
                        {
                            Rimi product = Rimi.Where(a => a.Product == item.Name).ToList().First();
                            product.Price = i.Value;
                            product.Date = DateTime.Now;
                            Rimi.Update(product);
                        }
                        catch (Exception)
                        {
                            Rimi.Add(new Rimi { Product = item.Name, Price = i.Value });
                        }
                        break;
                    default:
                        try
                        {
                            Unknown product = Unknown.Where(a => a.Product == item.Name).ToList().First();
                            product.Price = i.Value;
                            product.Date = DateTime.Now;
                            Unknown.Update(product);
                        }
                        catch (Exception)
                        {
                            Unknown.Add(new Unknown { Product = item.Name, Price = i.Value });
                        }
                        break;
                }
            }
        }
        // Remove shopping item prices from all shops
        private void RemoveDictionary(ShoppingItemData item)
        {
            Iki.RemoveRange(Iki.Where(a => a.Product == item.Name));
            Maxima.RemoveRange(Maxima.Where(a => a.Product == item.Name));
            Lidl.RemoveRange(Lidl.Where(a => a.Product == item.Name));
            Norfa.RemoveRange(Norfa.Where(a => a.Product == item.Name));
            Rimi.RemoveRange(Rimi.Where(a => a.Product == item.Name));
            Unknown.RemoveRange(Unknown.Where(a => a.Product == item.Name));
        }
        // Find shopping item by name
        public ShoppingItemData Find(string name)
        {
            Dictionary<ShopTypes, double> dictionary = FindDictionary(name);

            try
            {
                var s = Products.Where(a => a.Name == name).Single();
                return new ShoppingItemData(s.Name, (UnitTypes)Enum.Parse(typeof(UnitTypes), s.Unit), dictionary);
            }
            catch (Exception)
            {
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
                itemList.Add(new ShoppingItemData(i.Name, (UnitTypes)Enum.Parse(typeof(UnitTypes), i.Unit), FindDictionary(i.Name)));
            }
            return itemList;
        }
        // Insert and if exists update shopping item
        public void Insert(ShoppingItemData item)
        {
            var checkItem = from p in Products
                            where p.Name == item.Name
                            select p;
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
            modelBuilder.Entity<Iki>(entity =>
            {
                entity.ToTable("iki");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.Product)
                    .IsRequired()
                    .HasColumnName("product")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<Lidl>(entity =>
            {
                entity.ToTable("lidl");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.Product)
                    .IsRequired()
                    .HasColumnName("product")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<Maxima>(entity =>
            {
                entity.ToTable("maxima");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.Product)
                    .IsRequired()
                    .HasColumnName("product")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<Norfa>(entity =>
            {
                entity.ToTable("norfa");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.Product)
                    .IsRequired()
                    .HasColumnName("product")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<Product>(entity =>
            {
                entity.ToTable("product");

                entity.HasIndex(e => e.Id)
                    .HasName("id_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("name")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf32")
                    .HasCollation("utf32_general_ci");

                entity.Property(e => e.Unit)
                    .IsRequired()
                    .HasColumnName("unit")
                    .HasColumnType("enum('kg','g','l','ml','m','cm','piece')")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<Rimi>(entity =>
            {
                entity.ToTable("rimi");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.Product)
                    .IsRequired()
                    .HasColumnName("product")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            modelBuilder.Entity<Unknown>(entity =>
            {
                entity.ToTable("unknown");

                entity.Property(e => e.Id).HasColumnName("id");

                entity.Property(e => e.Date)
                    .HasColumnName("date")
                    .HasColumnType("datetime")
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");

                entity.Property(e => e.Price).HasColumnName("price");

                entity.Property(e => e.Product)
                    .IsRequired()
                    .HasColumnName("product")
                    .HasColumnType("varchar(45)")
                    .HasCharSet("utf8mb4")
                    .HasCollation("utf8mb4_0900_ai_ci");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
