namespace AutomarketApi.Models
{
    public class PagedList<T>
    {
        public PagedList(List<T> items, int currentPageNumber, int pageSize, int totalItemsCount)
        {
            Items = items;
            CurrentPageNumber = currentPageNumber;
            PageSize = pageSize == 0 ? 1 : pageSize;
            TotalItemsCount = totalItemsCount;
        }


        public List<T> Items { get; set; }
        public int CurrentPageNumber { get; set; }
        public int PageSize { get; set; }
        public int TotalItemsCount { get; set; }
        public int TotalPagesCount() => Convert.ToInt32(Math.Ceiling((decimal)(TotalItemsCount / PageSize)));
    }
}
