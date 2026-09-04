using Audisoft.Domain;
using Microsoft.EntityFrameworkCore;

namespace Audisoft.Infrastructure.ApplicationContext;

public static class DbSeeder
{
    private static readonly string[] NombresProfesores =
    [
        "Carlos Pérez", "María González", "Luis Ramírez", "Sofía Torres",
        "Andrés Gómez", "Valentina Díaz", "Jorge Herrera", "Camila Rojas",
        "Miguel Castro", "Daniela Morales", "Fernando Ruiz", "Paola Vargas"
    ];

    private static readonly string[] NombresEstudiantes =
    [
        "Juan López", "Ana Martínez", "Pedro Sánchez", "Laura Fernández",
        "Diego Ramos", "Isabella Mendoza", "Sebastián Guzmán", "Valeria Cruz",
        "Mateo Ortiz", "Camila Reyes", "Nicolás Vega", "Gabriela Silva"
    ];

    private static readonly string[] NombresEvaluaciones =
    [
        "Primera evaluación", "Segunda evaluación", "Tercera evaluación"
    ];

    private static readonly int[] Valores = [8, 10, 11, 12, 13, 14, 15, 16, 17, 18, 19, 20];

    public static async Task SeedAsync(AppDbContext context)
    {
        if (!await context.Profesores.AnyAsync())
        {
            context.Profesores.AddRange(NombresProfesores.Select(nombre => new Profesor { Nombre = nombre }));
            await context.SaveChangesAsync();
        }

        if (!await context.Estudiantes.AnyAsync())
        {
            context.Estudiantes.AddRange(NombresEstudiantes.Select(nombre => new Estudiante { Nombre = nombre }));
            await context.SaveChangesAsync();
        }

        if (!await context.Notas.AnyAsync())
        {
            var profesores = await context.Profesores.OrderBy(p => p.Id).ToListAsync();
            var estudiantes = await context.Estudiantes.OrderBy(e => e.Id).ToListAsync();
            var materias = Enum.GetValues<Materias>();

            var notas = new List<Nota>();

            for (var i = 0; i < 30; i++)
            {
                var mes = (i % 12) + 1;
                var dia = 3 + (i % 25);

                notas.Add(new Nota
                {
                    Fecha = new DateOnly(2026, mes, dia),
                    Nombre = NombresEvaluaciones[(i / estudiantes.Count) % NombresEvaluaciones.Length],
                    Materia = materias[i % materias.Length],
                    EstudianteId = estudiantes[i % estudiantes.Count].Id,
                    ProfesorId = profesores[i % profesores.Count].Id,
                    Valor = Valores[i % Valores.Length]
                });
            }

            context.Notas.AddRange(notas);
            await context.SaveChangesAsync();
        }
    }
}
