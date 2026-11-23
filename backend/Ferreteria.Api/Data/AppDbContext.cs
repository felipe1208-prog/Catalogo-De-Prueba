using Ferreteria.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Ferreteria.Api.Data;

public sealed class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Categoria> Categorias => Set<Categoria>();

    public DbSet<Producto> Productos => Set<Producto>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Categoria>(categoria =>
        {
            categoria.ToTable("Categorias");
            categoria.HasKey(c => c.Id);
            categoria.Property(c => c.Nombre).HasMaxLength(80).IsRequired();
            categoria.Property(c => c.Slug).HasMaxLength(80).IsRequired();
            categoria.HasIndex(c => c.Slug).IsUnique();
        });

        modelBuilder.Entity<Producto>(producto =>
        {
            producto.ToTable("Productos");
            producto.HasKey(p => p.Id);
            producto.Property(p => p.Nombre).HasMaxLength(160).IsRequired();
            producto.Property(p => p.Descripcion).HasMaxLength(600).IsRequired();
            producto.Property(p => p.Sku).HasMaxLength(40).IsRequired();
            producto.Property(p => p.ImagenUrl).HasMaxLength(400);
            producto.HasIndex(p => p.Sku).IsUnique();
            producto.HasIndex(p => p.CategoriaId);
            producto.HasOne(p => p.Categoria)
                .WithMany(c => c.Productos)
                .HasForeignKey(p => p.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Categoria>().HasData(SeedData.Categorias);
        modelBuilder.Entity<Producto>().HasData(SeedData.Productos);
    }
}
