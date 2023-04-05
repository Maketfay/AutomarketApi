namespace AutomarketApi.Services.Interfaces
{
    public interface IHashGenerator
    {
        string HashPassword(string password);
        string GenerateSalt();
    }
}
