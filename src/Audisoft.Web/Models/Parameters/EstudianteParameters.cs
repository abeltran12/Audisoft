namespace Audisoft.Web.Models.Parameters;

public class EstudianteParameters
{
    public string? Nombre { get; set; }
    public int? Id { get; set; }
    public int PageNumber { get; set; } = 1;
    public int PageSize { get; set; } = 10;
}
