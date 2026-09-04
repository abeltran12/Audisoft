using Audisoft.Application.Contracts.Repositories;
using Audisoft.Infrastructure.ApplicationContext;

namespace Audisoft.Infrastructure.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly AppDbContext _context;

    private readonly Lazy<IEstudianteRepository> _estudianteRepository;
    private readonly Lazy<IProfesorRepository> _profesorRepository;
    private readonly Lazy<INotaRepository> _notaRepository;

    public UnitOfWork(AppDbContext context)
    {
        _context = context;

        _estudianteRepository = new Lazy<IEstudianteRepository>(() => new EstudianteRepository(_context));
        _profesorRepository = new Lazy<IProfesorRepository>(() => new ProfesorRepository(_context));
        _notaRepository = new Lazy<INotaRepository>(() => new NotaRepository(_context));
    }

    public IEstudianteRepository EstudianteRepository => _estudianteRepository.Value;
    public IProfesorRepository ProfesorRepository => _profesorRepository.Value;
    public INotaRepository NotaRepository => _notaRepository.Value;

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);
}
