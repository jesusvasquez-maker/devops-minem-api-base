using DevOpsMinem.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevOpsMinem.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.Property(u => u.Nombre).IsRequired().HasMaxLength(100);
            entity.Property(u => u.Apellido).IsRequired().HasMaxLength(100);
            entity.Property(u => u.Email).IsRequired().HasMaxLength(200);
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.Rol).IsRequired().HasMaxLength(50);
            entity.Property(u => u.FechaRegistro).IsRequired();
            entity.Property(u => u.Activo).IsRequired();
        });
    }
}
