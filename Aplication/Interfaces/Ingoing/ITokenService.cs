using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;


namespace Application.ingoing;

public interface ITokenService
{
    JwtSecurityToken GetToken(List<Claim> authClaims);
    string GenerateRefreshToken();
    ClaimsPrincipal GetClaimsPrincipal(string tokenAccess);
    Task<string> RenewAccessTokenAsync(string accessToken);
}