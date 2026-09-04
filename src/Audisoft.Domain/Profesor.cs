namespace Audisoft.Domain;

public class Profesor
{
    public int Id { get; set; }
    public required string Nombre { get; set; }
    public Status Status { get; set; } = Status.Activo;
}
