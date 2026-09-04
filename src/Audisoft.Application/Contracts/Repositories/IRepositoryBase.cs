using Audisoft.Application.Common;
using System.Linq.Expressions;

namespace Audisoft.Application.Contracts.Repositories;

public interface IRepositoryBase<T>
{
    IQueryable<T> FindAll(bool trackChanges);
    IQueryable<T> FindByCondition(Expression<Func<T, bool>> expression, bool trackChanges);

    Task<PagedList<T>> FindAllPagedAsync(int pageNumber, int pageSize, bool trackChanges);
    Task<PagedList<T>> FindByConditionPagedAsync(Expression<Func<T, bool>> expression, int pageNumber, int pageSize, bool trackChanges);

    void Create(T entity);
    void Update(T entity);
    void Delete(T entity);
}
