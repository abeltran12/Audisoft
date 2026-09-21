using Audisoft.Domain;

namespace Audisoft.Application.RequestFeatures;

public class NotaParameters : RequestParameters
{
    public NotaParameters()
    {
        OrderBy = "fecha desc";
    }

    public int? EstudianteId { get; set; }
    public int? ProfesorId { get; set; }
    public Materias? Materia { get; set; }
    public int? ValorMinimo { get; set; }
    public bool SoloInactivos { get; set; }
}
