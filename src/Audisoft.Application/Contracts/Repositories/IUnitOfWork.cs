namespace Audisoft.Application.Contracts.Repositories;

public interface IUnitOfWork
{
    IEstudianteRepository EstudianteRepository { get; }
    IProfesorRepository ProfesorRepository { get; }
    INotaRepository NotaRepository { get; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}
