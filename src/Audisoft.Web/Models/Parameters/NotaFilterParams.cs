namespace Audisoft.Web.Models.Parameters;

public class NotaFilterParams
{
    public int? EstudianteId { get; set; }
    public int? ProfesorId { get; set; }
    public Materias? Materia { get; set; }
    public int? ValorMinimo { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
