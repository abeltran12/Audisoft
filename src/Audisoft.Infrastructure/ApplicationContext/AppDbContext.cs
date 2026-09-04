using Audisoft.Application.Contracts.Repositories;
using Audisoft.Domain;
using Microsoft.EntityFrameworkCore;

namespace Audisoft.Infrastructure.ApplicationContext;

public class AppDbContext(DbContextOptions<AppDbContext> options) 
    : DbContext(options)
{
    public DbSet<Estudiante> Estudiantes { get; set; }
    public DbSet<Profesor> Profesores { get; set; }
    public DbSet<Nota> Notas { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified);

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                entry.Property("CreatedAt").CurrentValue = DateTime.UtcNow;
            }
            else if (entry.State == EntityState.Modified)
            {
                entry.Property("UpdatedAt").CurrentValue = DateTime.UtcNow;
            }
        }

        return await base.SaveChangesAsync(cancellationToken);
    }
}
