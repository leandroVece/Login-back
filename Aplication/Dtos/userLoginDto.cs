namespace Application.Dto;

public class LoginUser
{
    public string UserName { get; set; }
    public string Password { get; set; }
}

public class LoginUserTwoFactoery
{
    public string Email { get; set; }
    public bool TowFactory { get; set; }
}

public class Login2FARequest
{
    public string Token { get; set; }
    public string UserName { get; set; }
}

public class PaswordReset
{
    public string Password { get; set; }
    public string ResetPasword { get; set; }
    public string Token { get; set; }
    public string Email { get; set; }

}