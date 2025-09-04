using Application.Dto;
using Domain.Entities;


namespace Application.ingoing;

public interface IUserUseCases
{
    Task<string?> LoginAsync(LoginUser data);
    Task<string?> Login2FAAsync(Login2FARequest request);
    Task<string?> RefreshTokenAsync(string tokens);
    Task RegisterAsync(RegisterUserWithConsents data);
    Task<(bool Success, string Message)> ConfirmEmailAsync(string token, string email);
    Task ForgotPasswordAsync(string email);
    Task ResetPasswordAsync(PaswordReset data);

    Task<UserDto> GetUser(Guid id);

    // Task<UserPerfil> getPerfil(string id);
    // Task<List<UserListDto>> getUsers(PaginacionUser pag);

}
