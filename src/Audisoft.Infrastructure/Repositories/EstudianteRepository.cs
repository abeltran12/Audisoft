using Audisoft.Application.Contracts.Repositories;
using Audisoft.Application.RequestFeatures;
using Audisoft.Domain;
using Audisoft.Infrastructure.ApplicationContext;
using Microsoft.EntityFrameworkCore;

namespace Audisoft.Infrastructure.Repositories;

public class EstudianteRepository : RepositoryBase<Estudiante>, IEstudianteRepository
{
    public EstudianteRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Estudiante>> GetEstudiantesAsync(
        EstudianteParameters parameters,
        bool trackChanges,
        CancellationToken cancellationToken = default)
    {
        return await FindByCondition(e => e.Status == Status.Activo &&
                (string.IsNullOrEmpty(parameters.Nombre) || e.Nombre.Contains(parameters.Nombre)) &&
                (!parameters.Id.HasValue || e.Id == parameters.Id.Value), trackChanges)
            .OrderBy(e => e.Nombre)
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToListAsync();
    }

    public async Task<Estudiante?> GetEstudianteAsync(
        int id, 
        bool trackChanges,
        CancellationToken cancellationToken = default)
    {
        return await FindByCondition(e => e.Id == id && e.Status == Status.Activo, trackChanges)
            .SingleOrDefaultAsync();
    }

    public async Task<int> GetEstudiantesCountAsync(EstudianteParameters parameters, 
        CancellationToken cancellationToken = default)
    {
        return await FindByCondition(e => e.Status == Status.Activo &&
                (string.IsNullOrEmpty(parameters.Nombre)
                || e.Nombre.Contains(parameters.Nombre)) &&
                (!parameters.Id.HasValue || e.Id == parameters.Id.Value),
                trackChanges: false)
            .CountAsync(cancellationToken);
    }

    public void CreateEstudiante(Estudiante estudiante) => Create(estudiante);

    public void UpdateEstudiante(Estudiante estudiante) => Update(estudiante);

    public void DeleteEstudiante(Estudiante estudiante)
    {
        estudiante.Status = Status.Inactivo;
        Update(estudiante);
    }
}
