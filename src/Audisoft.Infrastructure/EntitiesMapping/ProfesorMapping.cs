using Audisoft.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Audisoft.Infrastructure.EntitiesMapping;

public class ProfesorMapping : IEntityTypeConfiguration<Profesor>
{
    public void Configure(EntityTypeBuilder<Profesor> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Nombre)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnType("nvarchar");

        builder
            .Property(x => x.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasDefaultValue(Status.Activo);

        builder.Property<DateTime>("CreatedAt")
            .IsRequired().HasColumnName("CreatedAt");
        builder.Property<DateTime>("UpdatedAt")
            .IsRequired().HasColumnName("UpdatedAt");
    }
}
