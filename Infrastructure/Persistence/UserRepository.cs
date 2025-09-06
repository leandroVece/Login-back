using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Infrastructure.Identity;
using Application.Dto;
using Application.outgoing;
using Microsoft.Extensions.Configuration;
using Infrastructure.Configurations;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;


namespace Infrastructure.Persistence;

public class UserRepository : IUserRepository
{

    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<ApplicationRole> _roleManager;
    private readonly IConfiguration _configuration;
    private readonly IMapper _mapper;
    private readonly ApplicationDbContext _context;

    public UserRepository(
        UserManager<ApplicationUser> userManager,
        RoleManager<ApplicationRole> roleManager,
        SignInManager<ApplicationUser> signInManager,
        IConfiguration configuration,

        IMapper mapper,
        ApplicationDbContext context
        )
    {
        _userManager = userManager;
        _roleManager = roleManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _mapper = mapper;
        _context = context;
    }

    public async Task<bool> Verify2FA(IUser user, string token)
    {
        var appUser = CastToApplicationUser(user);
        return await _userManager.VerifyTwoFactorTokenAsync(appUser, "Email", token);
    }


    public async Task<IdentityResult> ConfirmEmailAsync(IUser user, string token)
    {
        var appUser = CastToApplicationUser(user);

        return await _userManager.ConfirmEmailAsync(appUser, token);
    }

    public async Task<bool> UserEmailExist(string Email)
    {
        var UserExist = await _userManager.FindByEmailAsync(Email);
        return UserExist != null;
    }
    public async Task<IUser> GetUserByEmail(string Email)
    {
        var UserExist = await _userManager.FindByEmailAsync(Email);
        return UserExist;
    }

    public async Task<List<Claim>> AddClaimForUser(IUser dto)
    {
        var appUser = CastToApplicationUser(dto);
        var roles = await GetUserRol(appUser);
        var authClaims = new List<Claim>{
                new Claim(ClaimTypes.Name,appUser.UserName!),
                new Claim(ClaimTypes.Email, appUser.Email!),
                new Claim(ClaimTypes.NameIdentifier, appUser.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

        foreach (var role in roles)
        {
            authClaims.Add(new Claim(ClaimTypes.Role, role));
        }

        // var author = await _perfilRepository.GetAuthorByIdUser(user.Id);
        // if (author != null && author.Id_UserFk != null)
        // {
        //     authClaims.Add(new Claim("userPerfilId", author.Id_author.ToString()));
        // }

        return authClaims;
    }

    public async Task AssingRoleToUserAsyn(IUser dto, List<string> roles)
    {
        var appUser = CastToApplicationUser(dto);
        var AssingRoles = new List<string>();
        foreach (var item in roles)
        {
            if (await _roleManager.RoleExistsAsync(item) && !await _userManager.IsInRoleAsync(appUser, item))
            {
                await _userManager.AddToRoleAsync(appUser, item);
                AssingRoles.Add(item);
            }
        }
    }

    public async Task<CreateUserResponse> CreateUserWithTokenAsyn(RegisterUser data)
    {
        //Add the User en the DB
        ApplicationUser user = new ApplicationUser()
        {
            Email = data.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = data.UserName,
            IdLocation = data.IdLocation,
        };
        try
        {
            var result = await _userManager.CreateAsync(user, data.Password);
            if (result.Succeeded)
            {
                // //Add token verify the email
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                var Response = new CreateUserResponse { Token = token, User = user };

                return Response;
            }
            else
            {
                return null;
            }
        }
        catch (System.Exception ex)
        {
            Console.Write("error" + ex);
            return null;
        }
    }

    public async Task<(IUser, bool)> VerifyUser(string userName, string Password)
    {
        var user = await GetUserByNameAsync(userName);
        if (user != null && await _userManager.CheckPasswordAsync(user, Password))
        {
            return (user, user.TwoFactorEnabled);
        }
        else
            return (null, false);
    }

    public async Task<List<string>> GetUserRol(IUser user)
    {
        var appUser = CastToApplicationUser(user);
        var roles = await _userManager.GetRolesAsync(appUser);
        return roles.ToList();
    }

    public async Task<ApplicationUser> GetUserByNameAsync(string userName)
    {
        return await _userManager.FindByNameAsync(userName);
    }
    public async Task<IUser?> GetUserdtoByNameAsync(string userName)
    {
        var res = await _userManager.FindByNameAsync(userName);
        return res;
    }
    public async Task<IUser> GetUserInterfaceByNameAsync(string userName)
    {
        var res = await _userManager.FindByNameAsync(userName);
        return res;
    }

    public async Task<string?> GetOtpLoginByAsyn(IUser dto)
    {
        // Solo generas el token si se requiere 2FA
        var user = CastToApplicationUser(dto);
        var token = await _userManager.GenerateTwoFactorTokenAsync(user, "Email");
        return token;

    }


    public async Task<UserDto> GetUserById(Guid id)
    {
        var res = await _context.Users.Where(x => x.Id == id)
          .ProjectTo<UserDto>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
        return res;
    }

    public async Task<string> GeneratePasswordReset(IUser user)
    {
        var appUser = CastToApplicationUser(user);
        var token = await _userManager.GeneratePasswordResetTokenAsync(appUser);
        return token;
    }
    public async Task<IdentityResult> ResetPassword(IUser user, string token, string password)
    {
        var appUser = CastToApplicationUser(user);
        var res = await _userManager.ResetPasswordAsync(appUser, token, password);
        return res;
    }




    // public async Task<UserPerfil> GetUserPerfil(string id)
    // {
    //     var res = await _context.Users.Where(x => x.Id == id)
    //     .ProjectTo<UserPerfil>(_mapper.ConfigurationProvider).FirstOrDefaultAsync();
    //     return res;
    // }


    // public async Task<List<UserListDto>> GetUserList(PaginacionUser pag)
    // {
    //     var query = await GetQueryFilter(pag.userName, pag.email);

    //     var totalRecords = await query.CountAsync();

    //     var res = await query
    //         .Skip((pag.PageNumber - 1) * 30)
    //         .Take(pag.pageZise)
    //         .ToListAsync();

    //     return res;
    // }

    #region 


    // private void updateTokenRefresh(ApplicationUser user)
    // {
    //     user.TokenRefresh = _tokenService.GenerateRefreshToken();
    //     var daysExpiry = int.Parse(_configuration["JWT:TokenRefreshInDays"]);
    //     user.TokenRefreshExpiry = DateTime.Now.AddDays(daysExpiry);

    //     _userManager.UpdateAsync(user);
    // }

    private async Task<IQueryable<UserListDto>> GetQueryFilter(string userName, string email)
    {
        var query = _context.Users.AsQueryable();
        if (!string.IsNullOrEmpty(userName))
        {
            query.Where(x => x.UserName == userName);
        }
        if (!string.IsNullOrEmpty(email))
        {
            query.Where(x => x.Email == email);
        }

        var projectedQuery = query.ProjectTo<UserListDto>(_mapper.ConfigurationProvider);

        return projectedQuery;
    }

    private ApplicationUser CastToApplicationUser(IUser user)
    {
        var appUser = user as ApplicationUser;
        if (appUser == null)
            throw new InvalidCastException("El usuario no es del tipo ApplicationUser.");
        return appUser;
    }

    #endregion


}