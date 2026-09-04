namespace Audisoft.Web.Models;

public record EstudianteRequest(int Id, string Nombre);

public record CreateEstudianteRequest(string Nombre);

public record UpdateEstudianteRequest(string Nombre);