using AutomarketApi.Context;
using AutomarketApi.Filters;
using AutomarketApi.Models;
using AutomarketApi.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AutomarketApi.Repositories
{
    public class BaseRepository<T> : IBaseRepository<T> where T : BaseEntity
    {
        protected readonly ApplicationDbContext _context;
        private readonly DbSet<T> _dbSet;
        public BaseRepository(ApplicationDbContext context)
        {
            _context = context;
            _dbSet = _context.Set<T>();
        }

        public async Task<Guid> Create(T entity)
        {
            entity.Id = Guid.NewGuid();
            await _context.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity.Id;
        }

        public void Delete(T entity) => _context.Remove(entity);

        public async Task<PagedList<T>> GetPageListAsync(BaseFilter<T> filter)
        {
            IQueryable<T> list = _dbSet.AsQueryable<T>();
            list = filter.EnrichQuery(list);
            int totalItemsCount = list.Count();
            int currentPage = filter.CurrentPage;
            if (totalItemsCount > filter.PageSize)
                list = list.Skip(((filter.CurrentPage - 1) * filter.PageSize)).Take(filter.PageSize);
            else
                currentPage = 1;
            return new PagedList<T>(list.ToList(), currentPage, filter.PageSize, totalItemsCount);
        }

        public async Task<T?> Read(Guid id) => await _dbSet.FirstOrDefaultAsync(o => o.Id == id);

        public async Task<IQueryable<T>> ReadAll() => (await _dbSet.ToListAsync()).AsQueryable();
    }
}
