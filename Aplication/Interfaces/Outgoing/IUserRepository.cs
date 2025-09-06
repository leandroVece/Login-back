using System.Security.Claims;
using Application.Dto;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;


namespace Application.outgoing;

public interface IUserRepository
{
    public Task<CreateUserResponse> CreateUserWithTokenAsyn(RegisterUser data);

    public Task AssingRoleToUserAsyn(IUser user, List<string> roles);
    public Task<string> GetOtpLoginByAsyn(IUser data);

    // public Task<TokenType> GetJWTTokenAsyn(ApplicationUserDto data);
    Task<(IUser, bool)> VerifyUser(string userName, string Password);
    Task<IUser?> GetUserdtoByNameAsync(string userName);
    Task<IUser> GetUserInterfaceByNameAsync(string userName);
    Task<bool> Verify2FA(IUser user, string token);
    Task<bool> UserEmailExist(string email);
    Task<IUser> GetUserByEmail(string email);
    Task<IdentityResult> ConfirmEmailAsync(IUser user, string token);
    Task<IdentityResult> ResetPassword(IUser user, string token, string password);
    public Task<List<Claim>> AddClaimForUser(IUser user);
    public Task<UserDto> GetUserById(Guid id);
    Task<string> GeneratePasswordReset(IUser user);


    // public Task<UserPerfil> GetUserPerfil(string id);
    // public Task<List<UserListDto>> GetUserList(PaginacionUser pag);

}