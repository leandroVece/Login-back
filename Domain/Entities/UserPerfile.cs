namespace Domain.Entities;

public class UserPerfile
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string LastName { get; set; }
    public string Bio { get; set; }
    public bool IsActive { get; set; }

}