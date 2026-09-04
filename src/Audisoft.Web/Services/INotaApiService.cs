using Audisoft.Web.Models;
using Audisoft.Web.Models.Abstractions;
using Audisoft.Web.Models.Parameters;

namespace Audisoft.Web.Services;

public interface INotaApiService
{
    Task<PagedList<NotaRequest>> GetNotasAsync(
        NotaFilterParams filterParams, CancellationToken cancellationToken = default);
    Task<NotaRequest?> GetNotaAsync(int id, CancellationToken cancellationToken = default);
    Task<NotaRequest> CreateNotaAsync(CreateNotaRequest request, CancellationToken cancellationToken = default);
    Task UpdateNotaAsync(int id, UpdateNotaRequest request, CancellationToken cancellationToken = default);
    Task DeleteNotaAsync(int id, CancellationToken cancellationToken = default);
}
