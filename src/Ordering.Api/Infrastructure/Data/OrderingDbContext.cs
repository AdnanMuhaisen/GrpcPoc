using Microsoft.EntityFrameworkCore;
using Ordering.Api.Core.Entities;

namespace Ordering.Api.Infrastructure.Data;

public class OrderingDbContext(DbContextOptions dbContextOptions) : DbContext(dbContextOptions)
{
    public DbSet<Order> Orders => Set<Order>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Order>(builder =>
        {
            builder.ToTable("Orders");
            builder.Property(x => x.CustomerName).HasMaxLength(150);
            builder.Property(x => x.ProductId).HasMaxLength(150);
            builder.Property(x => x.TotalPrice).HasPrecision(8, 2);
        });
    }
}