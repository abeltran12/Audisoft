using Audisoft.Application.RequestFeatures;
using Audisoft.Domain;

namespace Audisoft.Application.Contracts.Repositories;

public interface IEstudianteRepository : IRepositoryBase<Estudiante>
{
    Task<IEnumerable<Estudiante>> GetEstudiantesAsync(EstudianteParameters parameters, 
        bool trackChanges, CancellationToken cancellationToken = default);
    Task<Estudiante?> GetEstudianteAsync(int id, bool trackChanges, 
        CancellationToken cancellationToken = default);
    Task<int> GetEstudiantesCountAsync(EstudianteParameters parameters, 
        CancellationToken cancellationToken = default);
    void CreateEstudiante(Estudiante estudiante);
    void UpdateEstudiante(Estudiante estudiante);
    void DeleteEstudiante(Estudiante estudiante);
}
