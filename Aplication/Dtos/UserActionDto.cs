using Domain.Entities;

namespace Application.Dto;

public class CreateUserResponse
{
    public string Token { get; set; }
    public IUser User { get; set; }
}


public class RegisterUser
{
    public string UserName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }

    public int IdLocation { get; set; }
    public List<string>? Roles { get; set; } = new List<string> { "User" };

}
public class RegisterUserWithConsents
{
    public RegisterUser registerUser { get; set; }
    // public ConsentsDto userConsents { get; set; }

}