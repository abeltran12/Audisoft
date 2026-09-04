using Audisoft.Application.Requests;
using Audisoft.Domain;

namespace Audisoft.Application.Mappers;

public static class EstudianteMapper
{
    public static EstudianteRequest ToDto(this Estudiante estudiante) =>
        new(estudiante.Id, estudiante.Nombre);

    public static Estudiante ToEntity(this CreateEstudianteRequest dto) =>
        new() { Nombre = dto.Nombre };

    public static void ApplyUpdate(this Estudiante estudiante, UpdateEstudianteRequest dto) =>
        estudiante.Nombre = dto.Nombre;
}
