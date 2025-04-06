using Microsoft.EntityFrameworkCore;
using StoreBackend.Models;

namespace StoreBackend.Data
{
    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options)
        {
        }

        public DbSet<Stock> Stocks { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Grocer> Grocers { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // הגדרת סטטוס כ-Enum
            modelBuilder.Entity<Order>()
                .Property(o => o.Status)
                .HasConversion<string>();

            // קשר בין הזמנה לסחורה
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Stock)
                .WithMany(s => s.Orders)
                .HasForeignKey(o => o.StockId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict); // מוודא שלא תישרף הקשר עם הסחורה

            // קשר בין סחורה לספק
            modelBuilder.Entity<Stock>()
                .HasOne(s => s.Supplier)
                .WithMany(sup => sup.Stocks) // הוספת WithMany כדי לוודא שהקשר חד-משמעי
                .HasForeignKey(s => s.SupplierId)
                .IsRequired();

            //// קשר בין הזמנה לספק
            //modelBuilder.Entity<Order>()
            //    .HasOne(o => o.Supplier)
            //    .WithMany(sup => sup.Orders) // הוספת WithMany כדי לוודא שהקשר חד-משמעי
            //    .HasForeignKey(o => o.SupplierId)
            //    .IsRequired();

            // קשר בין הזמנה לבעל המכולת
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Grocer)
                .WithMany(g => g.Orders)
                .HasForeignKey(o => o.GrocerId)
                .IsRequired();
        }

    }
}
