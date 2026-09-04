namespace Audisoft.Application.RequestFeatures;

public class EstudianteParameters : RequestParameters
{
    public EstudianteParameters()
    {
        OrderBy = "nombre";
    }

    public string? Nombre { get; set; }
    public int? Id { get; set; }
}