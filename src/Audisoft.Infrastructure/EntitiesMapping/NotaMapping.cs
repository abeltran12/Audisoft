using Audisoft.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Audisoft.Infrastructure.EntitiesMapping;

public class NotaMapping : IEntityTypeConfiguration<Nota>
{
    public void Configure(EntityTypeBuilder<Nota> builder)
    {
        builder
            .HasKey(x => x.Id);

        builder
            .Property(x => x.Nombre)
            .IsRequired()
            .HasMaxLength(100)
            .HasColumnType("nvarchar");

        builder
            .Property(x => x.Valor)
            .IsRequired();

        builder
            .Property(x => x.Fecha)
            .IsRequired();

        builder
            .Property(x => x.Materia)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(50);

        builder
            .Property(x => x.EstudianteId)
            .IsRequired();

        builder
            .Property(x => x.ProfesorId)
            .IsRequired();

        builder
            .Property(x => x.Status)
            .IsRequired()
            .HasConversion<string>()
            .HasDefaultValue(Status.Activo);

        builder
            .HasOne(x => x.Estudiante)
            .WithMany()
            .HasForeignKey(x => x.EstudianteId)
            .OnDelete(DeleteBehavior.Restrict);

        builder
            .HasOne(x => x.Profesor)
            .WithMany()
            .HasForeignKey(x => x.ProfesorId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property<DateTime>("CreatedAt")
            .IsRequired().HasColumnName("CreatedAt");
        builder.Property<DateTime>("UpdatedAt")
            .IsRequired().HasColumnName("UpdatedAt");
    }
}
