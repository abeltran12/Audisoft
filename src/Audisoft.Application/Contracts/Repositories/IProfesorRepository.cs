using Audisoft.Application.RequestFeatures;
using Audisoft.Domain;

namespace Audisoft.Application.Contracts.Repositories;

public interface IProfesorRepository : IRepositoryBase<Profesor>
{
    Task<IEnumerable<Profesor>> GetProfesoresAsync(ProfesorParameters parameters, bool trackChanges, 
        CancellationToken cancellationToken = default);
    Task<Profesor?> GetProfesorAsync(int id, bool trackChanges, 
        CancellationToken cancellationToken = default);
    Task<int> GetProfesoresCountAsync(ProfesorParameters parameters, 
        CancellationToken cancellationToken = default);
    void CreateProfesor(Profesor profesor);
    void UpdateProfesor(Profesor profesor);
    void DeleteProfesor(Profesor profesor);
    Task<bool> IsProfesorInactivoAsync(int profesorId, CancellationToken cancellationToken = default);
}
