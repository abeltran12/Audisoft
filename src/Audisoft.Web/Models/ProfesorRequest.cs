namespace Audisoft.Web.Models;

public record ProfesorRequest(int Id, string Nombre);

public record CreateProfesorRequest(string Nombre);

public record UpdateProfesorRequest(string Nombre);