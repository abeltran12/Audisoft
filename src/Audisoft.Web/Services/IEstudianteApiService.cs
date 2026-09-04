using Audisoft.Web.Models;
using Audisoft.Web.Models.Abstractions;
using Audisoft.Web.Models.Parameters;

namespace Audisoft.Web.Services;

public interface IEstudianteApiService
{
    Task<PagedList<EstudianteRequest>> GetEstudiantesAsync(
        EstudianteParameters parameters, CancellationToken cancellationToken = default);
    Task<EstudianteRequest?> GetEstudianteAsync(
        int id, CancellationToken cancellationToken = default);
    Task<EstudianteRequest> CreateEstudianteAsync(
        CreateEstudianteRequest request, CancellationToken cancellationToken = default);
    Task UpdateEstudianteAsync(
        int id, UpdateEstudianteRequest request, CancellationToken cancellationToken = default);
    Task DeleteEstudianteAsync(int id, CancellationToken cancellationToken = default);
}
