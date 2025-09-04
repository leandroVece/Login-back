using Microsoft.AspNetCore.Mvc;
using Application.Dto;
using Application.ingoing;
using Presentation.Dtos;
using Serilog;
using System;
using Application.UseCases;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Presentation.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly IUserUseCases _userUseCases;

    public UserController(IUserUseCases userUseCases)
    {
        _userUseCases = userUseCases;
    }

    [Authorize]
    [HttpGet("me")]
    public IActionResult GetCurrentUser()
    {
        var user = new
        {
            username = User.Identity?.Name,
            id_user = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
            roles = User.FindAll(ClaimTypes.Role).Select(c => c.Value).ToList(),
            email = User.FindFirst(ClaimTypes.Email)?.Value,

        };
        return Ok(user);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginUser data)
    {
        try
        {
            var result = await _userUseCases.LoginAsync(data);
            if (result == null)
                return Unauthorized(new ApiResponse<object> { Status = "error", Message = "2FA requerido, revisa tu correo", Data = null });
            return Ok(new ApiResponse<object>
            { Status = "success", Message = "Login exitoso", Data = result });
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error en Login");
            return StatusCode(500, new ApiResponse<object> { Status = "error", Message = ex.Message, Data = null });
        }
    }

    [HttpPost("login-2fa")]
    public async Task<IActionResult> Login2FA([FromBody] Login2FARequest data)
    {
        try
        {
            var result = await _userUseCases.Login2FAAsync(data);
            return Ok(new ApiResponse<object> { Status = "success", Message = "2FA exitoso", Data = result });
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error en Login2FA");
            return StatusCode(500, new ApiResponse<object> { Status = "error", Message = ex.Message, Data = null });
        }
    }

    [HttpPost("refresh-token")]
    public async Task<IActionResult> RefreshToken([FromBody] string token)
    {
        try
        {
            var result = await _userUseCases.RefreshTokenAsync(token);
            return Ok(new ApiResponse<object> { Status = "success", Message = "Token refrescado", Data = result });
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error en RefreshToken");
            return StatusCode(500, new ApiResponse<object> { Status = "error", Message = ex.Message, Data = null });
        }
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterUserWithConsents data)
    {
        try
        {
            await _userUseCases.RegisterAsync(data);
            return Ok(new ApiResponse<object> { Status = "success", Message = "Usuario registrado, revisa tu correo para confirmar", Data = null });
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error en Register");
            return StatusCode(500, new ApiResponse<object> { Status = "error", Message = ex.Message, Data = null });
        }
    }

    [HttpGet("confirm-email")]
    public async Task<IActionResult> ConfirmEmail([FromQuery] string token, [FromQuery] string email)
    {
        try
        {
            var (success, message) = await _userUseCases.ConfirmEmailAsync(token, email);
            if (success)
                return Ok(new ApiResponse<object> { Status = "success", Message = message, Data = null });
            return BadRequest(new ApiResponse<object> { Status = "error", Message = message, Data = null });
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error en ConfirmEmail");
            return StatusCode(500, new ApiResponse<object> { Status = "error", Message = ex.Message, Data = null });
        }
    }

    [HttpPost("forgot-password")]
    public async Task<IActionResult> ForgotPassword([FromBody] string email)
    {
        try
        {
            await _userUseCases.ForgotPasswordAsync(email);
            return Ok(new ApiResponse<object> { Status = "success", Message = "Correo enviado para restablecer contraseña", Data = null });
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error en ForgotPassword");
            return StatusCode(500, new ApiResponse<object> { Status = "error", Message = ex.Message, Data = null });
        }
    }

    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPassword([FromBody] PaswordReset data)
    {
        try
        {
            await _userUseCases.ResetPasswordAsync(data);
            return Ok(new ApiResponse<object> { Status = "success", Message = "Contraseña restablecida correctamente", Data = null });
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error en ResetPassword");
            return StatusCode(500, new ApiResponse<object> { Status = "error", Message = ex.Message, Data = null });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetUser(Guid id)
    {
        try
        {
            var user = await _userUseCases.GetUser(id);
            return Ok(new ApiResponse<object> { Status = "success", Message = "Usuario obtenido", Data = user });
        }
        catch (Exception ex)
        {
            Log.Error(ex, "Error en GetUser");
            return StatusCode(500, new ApiResponse<object> { Status = "error", Message = ex.Message, Data = null });
        }
    }
}
