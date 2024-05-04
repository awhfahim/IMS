using IMS.Infrastructure.Membership.Enums;
using Microsoft.AspNetCore.Identity;

namespace IMS.Infrastructure.Membership;

public class AppUser : IdentityUser<Guid>
{
    public Role Role { get; set; }
    public string? RefreshToken { get; set; }
    public DateTime RefreshTokenExpiryTime { get; set; } = DateTime.Now;
}