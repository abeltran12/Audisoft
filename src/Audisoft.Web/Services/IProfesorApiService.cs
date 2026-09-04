using Audisoft.Web.Models;
using Audisoft.Web.Models.Abstractions;
using Audisoft.Web.Models.Parameters;

namespace Audisoft.Web.Services;

public interface IProfesorApiService
{
    Task<PagedList<ProfesorRequest>> GetProfesoresAsync(
        ProfesorParameters parameters, CancellationToken cancellationToken = default);
    Task<ProfesorRequest?> GetProfesorAsync(
        int id, CancellationToken cancellationToken = default);
    Task<ProfesorRequest> CreateProfesorAsync(
        CreateProfesorRequest request, CancellationToken cancellationToken = default);
    Task UpdateProfesorAsync(int id, 
        UpdateProfesorRequest request, CancellationToken cancellationToken = default);
    Task DeleteProfesorAsync(int id, CancellationToken cancellationToken = default);
}
