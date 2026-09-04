using Audisoft.Application.Common;
using Audisoft.Application.RequestFeatures;
using Audisoft.Application.Requests;

namespace Audisoft.Application.Contracts.Services;

public interface IEstudianteService
{
    Task<PagedList<EstudianteRequest>> GetAllAsync(EstudianteParameters parameters, 
        CancellationToken cancellationToken = default);
    Task<EstudianteRequest?> GetByIdAsync(int id, 
        CancellationToken cancellationToken = default);
    Task<EstudianteRequest> CreateAsync(CreateEstudianteRequest dto, 
        CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, UpdateEstudianteRequest dto, 
        CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}
