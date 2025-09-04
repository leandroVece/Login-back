namespace Domain.Entities;

public interface IUser
{
    Guid Id { get; set; }
    string Email { get; set; }

}