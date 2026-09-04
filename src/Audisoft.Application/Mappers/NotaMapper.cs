using Audisoft.Application.Requests;
using Audisoft.Domain;

namespace Audisoft.Application.Mappers;

public static class NotaMapper
{
    public static NotaRequest ToDto(this Nota nota) =>
        new(
            nota.Id,
            nota.Nombre,
            nota.Valor,
            nota.Fecha,
            nota.Materia.ToString(),
            nota.EstudianteId,
            nota.Estudiante?.Nombre ?? string.Empty,
            nota.ProfesorId,
            nota.Profesor?.Nombre ?? string.Empty);

    public static NotaSmallRequest ToSmallDto(this Nota nota) =>
        new(
            nota.Id,
            nota.Nombre,
            nota.Valor,
            nota.Fecha,
            nota.Materia.ToString());

    public static Nota ToEntity(this CreateNotaRequest dto) =>
        new()
        {
            Nombre = dto.Nombre,
            Valor = dto.Valor,
            Fecha = dto.Fecha,
            Materia = dto.Materia,
            EstudianteId = dto.EstudianteId,
            ProfesorId = dto.ProfesorId
        };

    public static void ApplyUpdate(this Nota nota, UpdateNotaRequest dto)
    {
        nota.Nombre = dto.Nombre;
        nota.Valor = dto.Valor;
        nota.Fecha = dto.Fecha;
        nota.Materia = dto.Materia;
        nota.EstudianteId = dto.EstudianteId;
        nota.ProfesorId = dto.ProfesorId;
    }
}
