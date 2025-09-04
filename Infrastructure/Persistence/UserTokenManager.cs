using Application.Interface;
using Domain.Entities;
using Infrastructure.Identity;
using Microsoft.AspNetCore.Identity;

namespace Infrastructure.Persistence;


public class UserTokenManager : IUserTokenManager
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserTokenManager(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task SetRefreshTokenAsync(IUser user, string token)
    {
        var appUser = await _userManager.FindByIdAsync(user.Id.ToString());
        if (appUser != null)
        {
            await _userManager.SetAuthenticationTokenAsync(appUser, "MyApp", "RefreshToken", token);
        }
    }

    public async Task<string?> GetRefreshTokenAsync(IUser user)
    {
        var appUser = await _userManager.FindByIdAsync(user.Id.ToString());
        return appUser != null
            ? await _userManager.GetAuthenticationTokenAsync(appUser, "MyApp", "RefreshToken")
            : null;
    }

    public async Task SetRefreshTokenExpiryAsync(IUser user, DateTime expiry)
    {
        var appUser = await _userManager.FindByIdAsync(user.Id.ToString());
        if (appUser != null)
        {
            await _userManager.SetAuthenticationTokenAsync(appUser, "MyApp", "RefreshTokenExpiry", expiry.ToString("o"));
        }
    }

    public async Task<DateTime?> GetRefreshTokenExpiryAsync(IUser user)
    {
        var appUser = await _userManager.FindByIdAsync(user.Id.ToString());
        if (appUser != null)
        {
            var expiryString = await _userManager.GetAuthenticationTokenAsync(appUser, "MyApp", "RefreshTokenExpiry");
            return expiryString != null ? DateTime.Parse(expiryString) : null;
        }
        return null;
    }
}

