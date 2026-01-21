using GregCustomers.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GregCustomers.Infrastructure.Persistence;

public class GregCustomersDbContext : DbContext
{
    public GregCustomersDbContext(DbContextOptions<GregCustomersDbContext> options) : base(options) { }

    public DbSet<Client> Clients => Set<Client>();
    public DbSet<Address> Addresses => Set<Address>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>(e =>
        {
            e.ToTable("Clients", "dbo");
            e.HasKey(x => x.Id);

            e.Property(x => x.Name).HasMaxLength(200).IsRequired();
            e.Property(x => x.Email).HasMaxLength(320).IsRequired();

            e.Property(x => x.Logo);
            e.Property(x => x.LogoContentType).HasMaxLength(50);
            e.Property(x => x.LogoFileName).HasMaxLength(255);

            e.Property(x => x.CreatedAt);
            e.Property(x => x.UpdatedAt);

            // lê o relacionamento (mesmo se escrita for por SP)
            e.HasMany<Address>("_addresses")
                .WithOne()
                .HasForeignKey(a => a.ClientId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Address>(e =>
        {
            e.ToTable("Addresses", "dbo");
            e.HasKey(x => x.Id);

            e.Property(x => x.ClientId).IsRequired();
            e.Property(x => x.Street).HasMaxLength(300).IsRequired();

            e.Property(x => x.CreatedAt);
            e.Property(x => x.UpdatedAt);
        });
    }
}