using Microsoft.EntityFrameworkCore;

namespace Inventory.Grpc.Api.Infrastructure.Data;

public class AppDbContext(DbContextOptions dbContextOptions) : DbContext(dbContextOptions)
{
    public DbSet<Core.Entities.Product> Products => Set<Core.Entities.Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.Entity<Core.Entities.Product>(builder =>
        {
            builder.ToTable("Products");
            builder.Property(x => x.Name).HasMaxLength(150);
            builder.Property(x => x.UnitPrice).HasPrecision(8, 2);
        });
    }
}