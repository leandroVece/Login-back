using Microsoft.AspNetCore.Identity;
using Domain.Entities;

namespace Infrastructure.Identity;

public class ApplicationUser : IdentityUser<Guid>, IUser
{
    public DateTime FechaRegistro { get; set; }
    public Guid? IdUserPerfile { get; set; }
    public int IdLocation { get; set; }

    public virtual Location Location { get; set; }
    public virtual UserPerfile? UserPerfile { get; set; }

    public ApplicationUser()
    {
        FechaRegistro = DateTime.UtcNow;
    }
}
