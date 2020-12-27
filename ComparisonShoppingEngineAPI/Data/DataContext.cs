﻿using ComparisonShoppingEngineAPI.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace ComparisonShoppingEngineAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) :
            base(options)
        {

        }

        public virtual DbSet<Product> Products { get; set; }
        public virtual DbSet<Shop> Shops { get; set; }
        public virtual DbSet<ShopProduct> ShopProducts { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Receipt> Receipts { get; set; }
        public virtual DbSet<ReceiptProduct> ReceiptProducts { get; set; }
        public virtual DbSet<Report> Reports { get; set; }
        public virtual DbSet<Image> Images { get; set; }
    }
}
