using AutomarketApi.Models;

namespace AutomarketApi.Filters
{
    public class BaseFilter<T> where T : BaseEntity
    {
        public int CurrentPage { get; set; } = 1;
        public int PageSize { get; set; } = 10;

        public virtual IQueryable<T> EnrichQuery(IQueryable<T> query) { return query; }
    }
}
