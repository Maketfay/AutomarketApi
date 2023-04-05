namespace AutomarketApi
{
    public interface IUnitOfWork
    {
        Task CommitAsync();
    }
}
