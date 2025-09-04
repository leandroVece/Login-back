namespace Application.Dto;

public class ApplicationUserDto
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string SecurityStamp { get; set; }
    public int IdLocation { get; set; }
    public bool TwoFactorEnabled { get; set; }

}
public class ApplicationUserRolDto
{
    public string Id { get; set; }
    public string UserName { get; set; }
    public string Email { get; set; }
    public string SecurityStamp { get; set; }
    public int IdLocation { get; set; }
    public bool TwoFactorEnabled { get; set; }

    public List<string> Roles { get; set; }

}