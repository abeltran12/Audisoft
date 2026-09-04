namespace Audisoft.Web.Models;

public record NotaRequest(
    int Id,
    string Nombre,
    int Valor,
    DateOnly Fecha,
    Materias Materia,
    int EstudianteId,
    string EstudianteNombre,
    int ProfesorId,
    string ProfesorNombre);

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
