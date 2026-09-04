using Audisoft.Application.Requests;
using Audisoft.Domain;

namespace Audisoft.Application.Mappers;

public static class ProfesorMapper
{
    public static ProfesorRequest ToDto(this Profesor profesor) =>
        new(profesor.Id, profesor.Nombre);

    public static Profesor ToEntity(this CreateProfesorRequest dto) =>
        new() { Nombre = dto.Nombre };

    public static void ApplyUpdate(this Profesor profesor, UpdateProfesorRequest dto) =>
        profesor.Nombre = dto.Nombre;
}