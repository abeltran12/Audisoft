using Audisoft.Application.Common;
using Audisoft.Application.Requests;

namespace Audisoft.Application.Contracts.Services;

public interface INotaService
{
    Task<PagedList<NotaRequest>> GetAllAsync(NotaFilterParams filterParams, 
        CancellationToken cancellationToken = default);
    Task<NotaRequest?> GetByIdAsync(int id, 
        CancellationToken cancellationToken = default);
    Task<NotaSmallRequest> CreateAsync(CreateNotaRequest dto, CancellationToken cancellationToken = default);
    Task UpdateAsync(int id, UpdateNotaRequest dto, CancellationToken cancellationToken = default);
    Task DeleteAsync(int id, CancellationToken cancellationToken = default);
}