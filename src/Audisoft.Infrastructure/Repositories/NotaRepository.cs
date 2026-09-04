using Audisoft.Application.Contracts.Repositories;
using Audisoft.Application.RequestFeatures;
using Audisoft.Domain;
using Audisoft.Infrastructure.ApplicationContext;
using Microsoft.EntityFrameworkCore;

namespace Audisoft.Infrastructure.Repositories;

public class NotaRepository : RepositoryBase<Nota>, INotaRepository
{
    public NotaRepository(AppDbContext context) : base(context) { }

    public async Task<IEnumerable<Nota>> GetNotasAsync(
        NotaParameters parameters,
        bool trackChanges,
        CancellationToken cancellationToken = default)
    {
        return await FindByCondition(n => n.Status == Status.Activo &&
                (!parameters.EstudianteId.HasValue || n.EstudianteId == parameters.EstudianteId) &&
                (!parameters.ProfesorId.HasValue || n.ProfesorId == parameters.ProfesorId) &&
                (!parameters.Materia.HasValue || n.Materia == parameters.Materia) &&
                (!parameters.ValorMinimo.HasValue || n.Valor == parameters.ValorMinimo), trackChanges)
            .Include(n => n.Estudiante)
            .Include(n => n.Profesor)
            .OrderByDescending(n => n.Fecha)
            .Skip((parameters.PageNumber - 1) * parameters.PageSize)
            .Take(parameters.PageSize)
            .ToListAsync();
    }

    public async Task<Nota?> GetNotaAsync(
        int id, 
        bool trackChanges,
        CancellationToken cancellationToken = default)
    {
        return await FindByCondition(n => n.Id == id && n.Status == Status.Activo, trackChanges)
            .Include(n => n.Estudiante)
            .Include(n => n.Profesor)
            .SingleOrDefaultAsync();
    }

    public async Task<int> GetNotasCountAsync(NotaParameters parameters, 
        CancellationToken cancellationToken = default)
    {
        return await FindByCondition(n => n.Status == Status.Activo &&
                (!parameters.EstudianteId.HasValue || n.EstudianteId == parameters.EstudianteId) &&
                (!parameters.ProfesorId.HasValue || n.ProfesorId == parameters.ProfesorId) &&
                (!parameters.Materia.HasValue || n.Materia == parameters.Materia) &&
                (!parameters.ValorMinimo.HasValue || n.Valor == parameters.ValorMinimo), trackChanges: false)
            .CountAsync(cancellationToken);
    }

    public void CreateNota(Nota nota) => Create(nota);

    public void UpdateNota(Nota nota) => Update(nota);

    public void DeleteNota(Nota nota)
    {
        nota.Status = Status.Inactivo;
        Update(nota);
    }

    public async Task<bool> HasActiveNotasByEstudianteAsync(
        int estudianteId, 
        CancellationToken cancellationToken = default)
    {
        return await FindByCondition(n => n.Status == Status.Activo && 
            n.EstudianteId == estudianteId, trackChanges: false)
            .AnyAsync(cancellationToken);
    }

    public async Task<bool> HasActiveNotasByProfesorAsync(
        int profesorId, 
        CancellationToken cancellationToken = default)
    {
        return await FindByCondition(n => n.Status == Status.Activo && 
            n.ProfesorId == profesorId, trackChanges: false)
            .AnyAsync(cancellationToken);
    }
}
