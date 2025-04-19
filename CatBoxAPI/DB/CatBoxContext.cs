using CatBoxAPI.DB.Entities;
using Microsoft.EntityFrameworkCore;

namespace CatBoxAPI.DB;

public class CatBoxContext(DbContextOptions<CatBoxContext> options) : DbContext(options)
{
    public DbSet<CatProfileEntity> CatProfiles { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        builder.Entity<CatProfileEntity>().Property(e => e.Weight)
            .HasColumnType("Decimal")
            .HasPrecision(4, 2);

        builder.Entity<CatProfileEntity>().Property(e => e.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Entity<CatProfileEntity>();

        builder.Entity<BoxRegistration>().Property(e => e.CreatedAt)
            .HasDefaultValueSql("GETUTCDATE()");

        builder.Entity<BoxRegistration>();
    }
}
