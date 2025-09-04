namespace Application.Dto;

public class UserDto
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string PhoneNumber { get; set; }
    public string email { get; set; }
    public string Avatar { get; set; }

}

public class UserPerfil
{
    public string UserName { get; set; }
    public string Avatar { get; set; }
    // public AuthorDto authorDto { get; set; }

}
public class UserListDto
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }

}
