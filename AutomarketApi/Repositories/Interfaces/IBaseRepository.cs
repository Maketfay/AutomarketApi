using AutomarketApi.Filters;
using AutomarketApi.Models;

namespace AutomarketApi.Repositories.Interfaces
{
    public interface IBaseRepository<T> where T: BaseEntity
    {
        Task<Guid> Create(T entity);
        Task<T?> Read(Guid id);
        Task<IQueryable<T>> ReadAll();
        void Delete(T entity);

        Task<PagedList<T>> GetPageListAsync(BaseFilter<T> filter);

        //Task<T> Update(T entity);

    }
}

    