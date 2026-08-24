namespace OnlineShopWebApp.Interfaces
{
    public interface IUserContextService
    {
        string? GetCurrentUserID();
        string? GetAnonymousID();
    }
}
