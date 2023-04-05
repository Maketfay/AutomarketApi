namespace AutomarketApi.Helpers.Interfaces
{
    public interface IViewHelper<T, U>
    {
        U FromEntityToView(T entity);

        T FromViewToEntity(U view);
    }
}
