using Inventory.Grpc.Api.Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inventory.Grpc.Api.Infrastructure.Data;

public class AppDbContext(DbContextOptions dbContextOptions) : DbContext(dbContextOptions)
{
    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Product>(builder =>
        {
            builder.ToTable("Products");
            builder.Property(x => x.Name).HasMaxLength(150);
            builder.Property(x => x.UnitPrice).HasPrecision(8, 2);
        });
    }
}