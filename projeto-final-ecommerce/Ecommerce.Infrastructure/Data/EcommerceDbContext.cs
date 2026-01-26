namespace Ecommerce.Infrastructure.Data;

using Ecommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

/// <summary>
/// DbContext - Configuração do EF Core
/// </summary>
public class EcommerceDbContext : DbContext
{
    public DbSet<Cliente> Clientes { get; set; }
    public DbSet<Pedido> Pedidos { get; set; }
    public DbSet<ItemPedido> ItensPedido { get; set; }

    public EcommerceDbContext(DbContextOptions<EcommerceDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configurar Cliente
        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.HasKey(c => c.Id);
            entity.Property(c => c.Nome).IsRequired().HasMaxLength(100);
            entity.OwnsOne(c => c.Email, e =>
            {
                e.Property(email => email.Valor).HasColumnName("Email").IsRequired().HasMaxLength(255);
            });
            entity.HasIndex(c => c.Email.Valor).IsUnique();
            entity.HasMany(c => c.Pedidos).WithOne().HasForeignKey("ClienteId");
        });

        // Configurar Pedido
        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.HasKey(p => p.Id);
            entity.Property(p => p.Status).HasConversion<int>();
            entity.HasMany(p => p.Itens).WithOne().HasForeignKey("PedidoId");
        });

        // Configurar ItemPedido
        modelBuilder.Entity<ItemPedido>(entity =>
        {
            entity.HasKey(i => i.Id);
            entity.OwnsOne(i => i.PrecoUnitario, d =>
            {
                d.Property(din => din.Valor).HasColumnName("PrecoUnitario").HasColumnType("decimal(18,2)");
            });
        });
    }
}