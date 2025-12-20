namespace BookStore.Application.Services
{
    public interface IUserContextService
    {
        string? GetCurrentUserId();
        string? GetCurrentUserName();
    }
}
