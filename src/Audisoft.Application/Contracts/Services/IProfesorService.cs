using Audisoft.Application.Common;
using Audisoft.Application.RequestFeatures;
using Audisoft.Application.Requests;

namespace Audisoft.Application.Contracts.Services;

public interface IProfesorService
{
    Task<PagedList<ProfesorRequest>> GetAllAsync(ProfesorParameters parameters, 
        CancellationToken cancellationToken = default);
    Task<ProfesorRequest?> GetByIdAsync(int id, 
        CancellationToken cancellationToken = default);
    Task<ProfesorRequest> CreateAsync(CreateProfesorRequest dto, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, UpdateProfesorRequest dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
