namespace Audisoft.Application.RequestFeatures;

public class ProfesorParameters : RequestParameters
{
    public ProfesorParameters()
    {
        OrderBy = "nombre";
    }

    public string? Nombre { get; set; }
    public int? Id { get; set; }
    public bool SoloInactivos { get; set; }
}