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
            // ����� ����� �-Enum
            modelBuilder.Entity<Order>()
                .Property(o => o.Status)
                .HasConversion<string>();

            // ��� ��� ����� ������
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Stock)
                .WithMany(s => s.Orders)
                .HasForeignKey(o => o.StockId)
                .IsRequired()
                .OnDelete(DeleteBehavior.Restrict); // ����� ��� ����� ���� �� ������

            // ��� ��� ����� ����
            modelBuilder.Entity<Stock>()
                .HasOne(s => s.Supplier)
                .WithMany(sup => sup.Stocks) // ����� WithMany ��� ����� ����� ��-�����
                .HasForeignKey(s => s.SupplierId)
                .IsRequired();

            //// ��� ��� ����� ����
            //modelBuilder.Entity<Order>()
            //    .HasOne(o => o.Supplier)
            //    .WithMany(sup => sup.Orders) // ����� WithMany ��� ����� ����� ��-�����
            //    .HasForeignKey(o => o.SupplierId)
            //    .IsRequired();

            // ��� ��� ����� ���� ������
            modelBuilder.Entity<Order>()
                .HasOne(o => o.Grocer)
                .WithMany(g => g.Orders)
                .HasForeignKey(o => o.GrocerId)
                .IsRequired();
        }

    }
}
