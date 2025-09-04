using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;


namespace Infrastructure.Configurations;

public class ApplicationDbContext : IdentityDbContext<ApplicationUser, ApplicationRole, Guid>
{


    public DbSet<Location> Locations { get; set; }
    public DbSet<UserPerfile> UserPerfiles { get; set; }
    public DbSet<ApplicationUser> User { get; set; }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    { }
    protected override void OnModelCreating(ModelBuilder builder)
    {

        builder.Entity<Location>(loc =>
        {
            loc.HasKey(l => l.Id);
            loc.Property(l => l.Id).ValueGeneratedOnAdd();
            loc.Property(l => l.Name).HasMaxLength(100).IsRequired();
        });

        builder.Entity<UserPerfile>(up =>
        {
            up.HasKey(u => u.Id);
            up.Property(u => u.Name).HasMaxLength(80).IsRequired();
            up.Property(u => u.LastName).HasMaxLength(80).IsRequired();
            up.Property(u => u.Bio).HasMaxLength(500);
        });

        builder.Entity<ApplicationUser>(au =>
        {
            au.HasKey(a => a.Id);
            au.Property(a => a.UserName).HasMaxLength(80).IsRequired();
            au.Property(a => a.Email).HasMaxLength(100).IsRequired();

            au.HasOne(a => a.Location)
            .WithMany()
            .HasForeignKey(a => a.IdLocation);

            au.HasOne(a => a.UserPerfile)
            .WithOne()
            .HasForeignKey<ApplicationUser>(a => a.IdUserPerfile)
            .OnDelete(DeleteBehavior.Cascade);

        });

        base.OnModelCreating(builder);
        SeedRoles(builder);
    }

    private static void SeedRoles(ModelBuilder builder)
    {
        builder.Entity<ApplicationRole>().HasData(
            new ApplicationRole() { Id = new Guid("4f251d18-3a92-4d1a-8c11-9e7f8e6c4b2a"), Name = "Admin", ConcurrencyStamp = "1", NormalizedName = "ADMIN" },
            new ApplicationRole() { Id = new Guid("a3b4c5d6-e7f8-9012-3456-7890abcdef12"), Name = "User", ConcurrencyStamp = "2", NormalizedName = "USER" },
            new ApplicationRole() { Id = new Guid("b2c3d4e5-f6a7-8b9c-d0e1-234567890abc"), Name = "Porfesor", ConcurrencyStamp = "3", NormalizedName = "PROFESOR" },
            new ApplicationRole() { Id = new Guid("c3d4e5f6-a7b8-c9d0-e1f2-34567890abcd"), Name = "HR", ConcurrencyStamp = "4", NormalizedName = "RRHH" }
        );
    }
}