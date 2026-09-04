namespace Audisoft.Domain;

public class Nota
{
    public int Id { get; set; }
    public required string Nombre { get; set; }
    public int Valor { get; set; }
    public DateOnly Fecha { get; set; }
    public Materias Materia { get; set; }
    public int EstudianteId { get; set; }
    public Estudiante? Estudiante { get; set; }    
    public int ProfesorId { get; set; }
    public Profesor? Profesor { get; set; }
    public Status Status { get; set; } = Status.Activo;
}
