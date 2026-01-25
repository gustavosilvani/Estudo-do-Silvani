using Microsoft.EntityFrameworkCore;
using TaskManagement.Domain.Entities;

namespace TaskManagement.Infrastructure.Data;

/// <summary>
/// DbContext (EF Core) - Persistência de dados
/// </summary>
public class TaskManagementDbContext : DbContext {
    public DbSet<TarefaTask> Tarefas { get; set; } = null!;
    public DbSet<Comentario> Comentarios { get; set; } = null!;
    
    public TaskManagementDbContext(DbContextOptions<TaskManagementDbContext> options)
        : base(options) {
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder) {
        base.OnModelCreating(modelBuilder);
        
        // Configuração da entidade Tarefa
        modelBuilder.Entity<TarefaTask>(entity => {
            entity.HasKey(t => t.Id);
            
            // Value Object como propriedade complexa
            entity.OwnsOne(t => t.Titulo, titulo => {
                titulo.Property(t => t.Valor)
                    .HasColumnName("Titulo")
                    .HasMaxLength(100)
                    .IsRequired();
            });
            
            entity.Property(t => t.Descricao)
                .HasMaxLength(1000)
                .IsRequired();
            
            entity.Property(t => t.Status)
                .HasConversion<int>()
                .IsRequired();
            
            entity.Property(t => t.Prioridade)
                .HasConversion<int>()
                .IsRequired();
            
            entity.Property(t => t.DataCriacao)
                .IsRequired();
            
            entity.Property(t => t.UsuarioId)
                .IsRequired();
            
            entity.ToTable("Tarefas");
        });
        
        // Configuração da entidade Comentário
        modelBuilder.Entity<Comentario>(entity => {
            entity.HasKey(c => c.Id);
            
            entity.Property(c => c.Texto)
                .HasMaxLength(500)
                .IsRequired();
            
            entity.Property(c => c.UsuarioId)
                .IsRequired();
            
            entity.Property(c => c.DataCriacao)
                .IsRequired();
            
            entity.ToTable("Comentarios");
        });
    }
}
