using Microsoft.AspNetCore.Identity;
using System;

namespace Infrastructure.Identity;
public class ApplicationRole : IdentityRole<Guid>
{
    public ApplicationRole() : base() { }
    public ApplicationRole(string roleName) : base(roleName) { }
}
