using Audisoft.Application.RequestFeatures;
using Audisoft.Domain;

namespace Audisoft.Application.Contracts.Repositories;

public interface INotaRepository : IRepositoryBase<Nota>
{
    Task<IEnumerable<Nota>> GetNotasAsync(NotaParameters parameters, bool trackChanges, 
        CancellationToken cancellationToken = default);
    Task<Nota?> GetNotaAsync(int id, bool trackChanges, 
        CancellationToken cancellationToken = default);
    Task<int> GetNotasCountAsync(NotaParameters parameters, 
        CancellationToken cancellationToken = default);
    void CreateNota(Nota nota);
    void UpdateNota(Nota nota);
    void DeleteNota(Nota nota);
}
