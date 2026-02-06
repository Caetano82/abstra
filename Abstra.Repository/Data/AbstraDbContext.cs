using Abstra.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Abstra.Repository.Data;

public class AbstraDbContext : DbContext
{
    public AbstraDbContext(DbContextOptions<AbstraDbContext> options) : base(options)
    {
    }

    public DbSet<Country> Countries { get; set; }
    public DbSet<State> States { get; set; }
    public DbSet<City> Cities { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Country>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Code).IsRequired().HasMaxLength(3);
            entity.HasIndex(e => e.Code).IsUnique();
            entity.Property(e => e.CreatedAt).IsRequired();
        });

        modelBuilder.Entity<State>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Code).IsRequired().HasMaxLength(10);
            entity.Property(e => e.CountryId).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
            entity.HasIndex(e => new { e.Code, e.CountryId }).IsUnique();
            
            entity.HasOne(e => e.Country)
                .WithMany(c => c.States)
                .HasForeignKey(e => e.CountryId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<City>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.StateId).IsRequired();
            entity.Property(e => e.CreatedAt).IsRequired();
            
            entity.HasOne(e => e.State)
                .WithMany(s => s.Cities)
                .HasForeignKey(e => e.StateId)
                .OnDelete(DeleteBehavior.Cascade);
        });
    }
}
