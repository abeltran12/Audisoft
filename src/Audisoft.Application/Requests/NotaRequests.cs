using Audisoft.Domain;

namespace Audisoft.Application.Requests;

public record CreateNotaRequest(
    string Nombre,
    int Valor,
    DateOnly Fecha,
    Materias Materia,
    int EstudianteId,
    int ProfesorId);

public record UpdateNotaRequest(
    string Nombre,
    int Valor,
    DateOnly Fecha,
    Materias Materia,
    int EstudianteId,
    int ProfesorId);

public record NotaRequest(
    int Id,
    string Nombre,
    int Valor,
    DateOnly Fecha,
    string Materia,
    int EstudianteId,
    string EstudianteNombre,
    int ProfesorId,
    string ProfesorNombre);

public record NotaSmallRequest(
    int Id,
    string Nombre,
    int Valor,
    DateOnly Fecha,
    string Materia);