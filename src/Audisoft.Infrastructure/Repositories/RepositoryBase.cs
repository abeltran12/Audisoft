using Audisoft.Application.Common;
using Audisoft.Application.Contracts.Repositories;
using Audisoft.Infrastructure.ApplicationContext;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Audisoft.Infrastructure.Repositories;

public class RepositoryBase<T> : IRepositoryBase<T> where T : class
{
    protected readonly AppDbContext _context;

    public RepositoryBase(AppDbContext context)
    {
        _context = context;
    }

    public IQueryable<T> FindAll(bool trackChanges) =>
        !trackChanges
            ? _context.Set<T>().AsNoTracking()
            : _context.Set<T>();

    public IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges) =>
        !trackChanges
            ? _context.Set<T>().Where(expression).AsNoTracking()
            : _context.Set<T>().Where(expression);

    public async Task<PagedList<T>> FindAllPagedAsync(int pageNumber, int pageSize, bool trackChanges)
    {
        var query = FindAll(trackChanges);

        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedList<T>(items, totalCount, pageNumber, pageSize);
    }

    public async Task<PagedList<T>> FindByConditionPagedAsync(
        Expression<Func<T, bool>> expression, int pageNumber, int pageSize, bool trackChanges)
    {
        var query = FindByCondition(expression, trackChanges);

        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        return new PagedList<T>(items, totalCount, pageNumber, pageSize);
    }

    public void Create(T entity) => _context.Set<T>().Add(entity);
    public void Update(T entity) => _context.Set<T>().Update(entity);
    public void Delete(T entity) => _context.Set<T>().Remove(entity);
}
