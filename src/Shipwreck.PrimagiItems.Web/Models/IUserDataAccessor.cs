namespace Shipwreck.PrimagiItems.Web.Models;

public interface IUserDataAccessor
{
    Task<UserData?> ReadAsync(DateTimeOffset? ifModifiedSince);

    Task WriteAsync(UserData userData);
}
