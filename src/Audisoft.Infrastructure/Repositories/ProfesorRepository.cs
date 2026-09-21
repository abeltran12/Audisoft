using Audisoft.Application.Contracts.Repositories;
using Audisoft.Application.RequestFeatures;
using Audisoft.Domain;
using Audisoft.Infrastructure.ApplicationContext;
using Microsoft.EntityFrameworkCore;

namespace Audisoft.Infrastructure.Repositories;

public class ProfesorRepository : RepositoryBase<Profesor>, IProfesorRepository
{
    public ProfesorRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Profesor>> GetProfesoresAsync(
        ProfesorParameters parameters,
        bool trackChanges,
        CancellationToken cancellationToken = default)
    {
        return await FindByCondition(p =>
                p.Status == (parameters.SoloInactivos ? Status.Inactivo : Status.Activo) &&
                (string.IsNullOrEmpty(parameters.Nombre) || p.Nombre.Contains(parameters.Nombre)) &&
                (!parameters.Id.HasValue || p.Id == parameters.Id.Value), trackChanges)
            .OrderBy(p => p.Id)
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToListAsync();
    }

    public async Task<Profesor?> GetProfesorAsync(
        int id,
        bool trackChanges,
        CancellationToken cancellationToken = default)
    {
        return await FindByCondition(p => p.Id == id && p.Status == Status.Activo, trackChanges)
            .SingleOrDefaultAsync();
    }

    public async Task<int> GetProfesoresCountAsync(
        ProfesorParameters parameters, 
        CancellationToken cancellationToken = default)
    {
        return await FindByCondition(p =>
                p.Status == (parameters.SoloInactivos ? Status.Inactivo : Status.Activo) &&
                (string.IsNullOrEmpty(parameters.Nombre)
                || p.Nombre.Contains(parameters.Nombre)) &&
                (!parameters.Id.HasValue || p.Id == parameters.Id.Value),
                trackChanges: false)
            .CountAsync(cancellationToken);
    }

    public void CreateProfesor(Profesor profesor) => Create(profesor);

    public void UpdateProfesor(Profesor profesor) => Update(profesor);

    public void DeleteProfesor(Profesor profesor)
    {
        profesor.Status = Status.Inactivo;
        Update(profesor);
    }

    public async Task<bool> IsProfesorInactivoAsync(
        int profesorId, 
        CancellationToken cancellationToken = default)
    {
        return await FindByCondition(p => p.Id == profesorId && p.Status == Status.Inactivo, 
            trackChanges: false)
            .AnyAsync(cancellationToken);
    }
}
