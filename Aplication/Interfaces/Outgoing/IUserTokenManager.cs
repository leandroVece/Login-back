using Domain.Entities;

namespace Application.Interface;

public interface IUserTokenManager
{
    Task SetRefreshTokenAsync(IUser user, string token);
    Task<string?> GetRefreshTokenAsync(IUser user);
    Task SetRefreshTokenExpiryAsync(IUser user, DateTime expiry);
    Task<DateTime?> GetRefreshTokenExpiryAsync(IUser user);
}