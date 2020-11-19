using ComparisonShoppingEngineAPI.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ComparisonShoppingEngineAPI.Services
{
    public class DatabaseContext : DbContext
    {
        public DbSet<ProductEntity> Products { get; set; }
        public DbSet<ShopEntity> Shops { get; set; }
        public DbSet<ShopProductEntity> ShopProducts { get; set; }

        public DatabaseContext(DbContextOptions<DatabaseContext> options) : base(options)
        {
        }

        // Query made changes into database
        public override int SaveChanges()
        {
            return base.SaveChanges();
        }

        // MySQL Schema modeling for EF
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<ProductEntity>(entity =>
            {
                entity.HasKey(x => new { x.Id });
                entity.ToTable("product");

                entity.HasIndex(e => e.Id)
                    .HasName("product_id_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("product_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("product_name")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");

                entity.Property(e => e.Unit)
                    .IsRequired()
                    .HasColumnName("product_unit")
                    .HasColumnType("enum('kg','g','l','ml','m','cm','piece')")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<ShopEntity>(entity =>
            {
                entity.HasKey(x => new { x.Id });
                entity.ToTable("shop");

                entity.HasIndex(e => e.Id)
                    .HasName("shop_id_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id)
                    .HasColumnName("shop_id")
                    .HasColumnType("int(11)");

                entity.Property(e => e.Name)
                    .IsRequired()
                    .HasColumnName("shop_name")
                    .HasColumnType("varchar(255)")
                    .HasCharSet("utf8")
                    .HasCollation("utf8_general_ci");
            });

            modelBuilder.Entity<ShopProductEntity>(entity =>
            {
                entity.HasKey(e => new { e.Id, e.ProductId })
                    .HasName("PRIMARY");

                entity.ToTable("shop_product");

                entity.HasIndex(e => e.ProductId)
                    .HasName("fk_shop_product_product_idx");

                entity.HasIndex(e => e.ShopId)
                    .HasName("fk_shop_product_shop_idx");

                entity.HasIndex(e => e.Id)
                    .HasName("shop_product_id_UNIQUE")
                    .IsUnique();

                entity.Property(e => e.Id)
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
                    .WithMany(p => p.ShopProducts)
                    .HasForeignKey(d => d.ProductId)
                    .HasConstraintName("fk_shop_product_product");
                
                entity.HasOne(d => d.Shop)
                    .WithMany(p => p.ShopProducts)
                    .HasForeignKey(d => d.ShopId)
                    .OnDelete(DeleteBehavior.SetNull)
                    .HasConstraintName("fk_shop_product_shop");
                
            });

            base.OnModelCreating(modelBuilder);
        }
    }
}
