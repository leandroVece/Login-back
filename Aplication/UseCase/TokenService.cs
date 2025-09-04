using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.IdentityModel.Tokens;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Security.Cryptography;
using Application.ingoing;

using Application.outgoing;


namespace Infrastructure.Persistence;


public class TokenService : ITokenService
{
    private readonly IConfiguration _configuration;
    private readonly IUserRepository _user;

    public TokenService(IConfiguration configuration, IUserRepository user)
    {
        _configuration = configuration;
        _user = user;
    }

    public JwtSecurityToken GetToken(List<Claim> authClaims)
    {
        var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
        var expiry = int.Parse(_configuration["JWT:ExpityTokenInMinutes"]);

        var token = new JwtSecurityToken(
            issuer: _configuration["JWT:ValidIssuer"],
            audience: _configuration["JWT:ValidAudience"],
            expires: DateTime.Now.AddMinutes(expiry),
            claims: authClaims,
            signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
        );
        return token;
    }

    public string GenerateRefreshToken()
    {
        var randomNumber = new byte[64];
        using (var rng = RandomNumberGenerator.Create())
        {
            rng.GetBytes(randomNumber);
            return Convert.ToBase64String(randomNumber);
        }
    }

    public ClaimsPrincipal GetClaimsPrincipal(string tokenAccess)
    {
        var tokenValidation = new TokenValidationParameters()
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateIssuerSigningKey = true,

            ValidAudience = _configuration["JWT:ValidAudience"],
            ValidIssuer = _configuration["JWT:ValidIssuer"],
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"])),
            ValidateLifetime = false
        };

        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(tokenAccess, tokenValidation, out SecurityToken securityToken);

        return principal;
    }

    public async Task<string> RenewAccessTokenAsync(string accessToken)
    {
        var principal = GetClaimsPrincipal(accessToken);
        if (principal == null || !principal.Identity.IsAuthenticated)
            return null;

        var user = await _user.GetUserdtoByNameAsync(principal.Identity.Name);
        if (user == null)
            return null;

        var claims = await _user.AddClaimForUser(user);
        return GetToken(claims).ToString();
    }
}