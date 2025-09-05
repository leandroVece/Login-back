
using System.Text;
using Aplication.Common;
using Application.Dto;
using Application.Helpers;
using Application.ingoing;
using Application.Interface;
using Application.outgoing;
using AutoMapper;
using Domain.Entities;
using Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;


namespace Application.UseCases;

public class UserUseCases : IUserUseCases
{
    private readonly IUserTokenManager _userToken;

    private readonly ITokenService _tokenService;

    private readonly IConfiguration _configuration;

    // private readonly IUserConsentsRepository _userConsents;
    private readonly IUserRepository _userRepository;
    private readonly IEmailUseCases _emailServisces;
    private readonly IMapper _mapper;


    public UserUseCases(
        IUserRepository userRepository,
        IMapper mapper,
        IEmailUseCases emailServisces,
        IUserTokenManager userTokenManager,
        ITokenService tokenService,
        IConfiguration configuration
        )
    {
        _userRepository = userRepository;
        _emailServisces = emailServisces;
        _mapper = mapper;
        _userToken = userTokenManager;
        _tokenService = tokenService;
        _configuration = configuration;
    }

    public async Task<string?> LoginAsync(LoginUser data)
    {
        var user = await _userRepository.VerifyUser(data.UserName, data.Password);
        if (user != null)
        {
            if (user.TwoFactorEnabled)
            {
                var token = await _userRepository.GetOtpLoginByAsyn(user);
                var message = new Message(new string[] { user.Email! }, "Confirmar Email por el link", token);
                _emailServisces.SendMail(message);
                // Si requiere 2FA, retorna null y el controlador puede manejar el mensaje
                return null;
            }
            var claims = await _userRepository.AddClaimForUser(user);
            var response = _tokenService.GetToken(claims);
            return response.ToString();
        }
        throw new Exception("Usuario o contraseña incorrectos");
    }

    public async Task<string?> Login2FAAsync(Login2FARequest data)
    {
        var user = await _userRepository.GetUserInterfaceByNameAsync(data.UserName);
        if (user != null)
        {
            var isValid = await _userRepository.Verify2FA(user, data.Token);
            if (isValid)
            {
                var dto = await _userRepository.GetUserRoles(user);
                var claims = await _userRepository.AddClaimForUser(dto);
                var response = _tokenService.GetToken(claims);
                return response.ToString();
            }
        }
        throw new Exception("Token inválido o usuario incorrecto");
    }

    public async Task<string?> RefreshTokenAsync(string tokens)
    {
        var jwt = await _tokenService.RenewAccessTokenAsync(tokens);
        if (jwt != null)
        {
            return jwt;
        }
        throw new Exception("Código inválido");
    }



    public async Task RegisterAsync(RegisterUserWithConsents data)
    {
        var exsit = await _userRepository.UserEmailExist(data.registerUser.Email);
        if (exsit)
            throw new Exception("El usuario Ya existe");


        var createUserResponse = await _userRepository.CreateUserWithTokenAsyn(data.registerUser);
        if (createUserResponse != null)
        {

            // var consents = _mapper.Map<UserConsents>(data.userConsents);
            // consents.Id_userFk = createUserResponse.User.Id;
            // await _userConsents.CreateConsentAsync(consents); //consentimiento
            await _userRepository.AssingRoleToUserAsyn(createUserResponse.User, new List<string> { "user" });

            var encodedToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(createUserResponse.Token));
            var message = bodyEmail("reset-password", encodedToken, data.registerUser.Email);
            _emailServisces.SendMail(message);
            return;

        }
        throw new Exception("Error De usuario o contraseña");
    }

    public async Task<(bool Success, string Message)> ConfirmEmailAsync(string token, string email)
    {
        try
        {
            var user = await _userRepository.GetUserByEmail(email);

            if (user == null)
            {
                return (false, "El usuario no existe");
            }
            // Decodifica el token recibido por URL

            var decodedToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(token));

            var result = await _userRepository.ConfirmEmailAsync(user, decodedToken);
            if (result.Succeeded)
            {
                return (true, "Correo confirmado correctamente");
            }
            else
            {
                var errorMsg = string.Join("; ", result.Errors.Select(e => e.Description));
                return (false, $"No se pudo confirmar el correo: {errorMsg}");
            }
        }
        catch (System.Exception ex)
        {
            return (false, "Error al confirmar el correo: " + ex.Message);
        }
    }

    public async Task ForgotPasswordAsync(string email)
    {
        var user = await _userRepository.GetUserByEmail(email);
        if (user != null)
        {
            var token = await _userRepository.GeneratePasswordReset(user);
            var encodedToken = Uri.EscapeDataString(token);
            var message = bodyEmail("reset-password", encodedToken, email);
            _emailServisces.SendMail(message);
            return;
        }
        throw new Exception("El correo no existe");
    }

    public async Task ResetPasswordAsync(PaswordReset data)
    {
        var user = await _userRepository.GetUserByEmail(data.Email);
        if (user != null)
        {
            var reset = await _userRepository.ResetPassword(user, data.Token, data.Password);
            if (!reset.Succeeded)
                throw new Exception(string.Join("; ", reset.Errors.Select(e => e.Description)));
            return;
        }
        throw new Exception("El usuario no Existe");
    }

    public async Task<UserDto> GetUser(Guid id)
    {
        var res = await _userRepository.GetUserById(id);
        return res;
    }


    // public async Task<UserPerfil> getPerfil(string id)
    // {
    //     var res = await _userRepository.GetUserPerfil(id);
    //     return res;
    // }

    // public async Task<List<UserListDto>> getUsers(PaginacionUser pag)
    // {
    //     var res = await _userRepository.GetUserList(pag);
    //     return res;
    // }
    private Message bodyEmail(string endpoint, string token, string email)
    {
        var confirmationLink = $"{_configuration["Frontend:BaseUrl"]}/{endpoint}?Token={token}&email={email}";
        var htmlBody = EmailTemplateBuilder.GetConfirmationEmail(confirmationLink);

        var message = new Message(new string[] { email! }, "Confirmar Email por el link", htmlBody!) { IsHtml = true };
        return message;
    }

}