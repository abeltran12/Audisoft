namespace Audisoft.Application.Requests;


public record CreateProfesorRequest(string Nombre);
public record UpdateProfesorRequest(string Nombre);
public record ProfesorRequest(int Id, string Nombre);