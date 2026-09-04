namespace Audisoft.Application.Requests;

public record CreateEstudianteRequest(string Nombre);
public record UpdateEstudianteRequest(string Nombre);
public record EstudianteRequest(int Id, string Nombre);