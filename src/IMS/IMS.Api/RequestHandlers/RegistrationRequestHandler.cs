using System.ComponentModel.DataAnnotations;
using IMS.Infrastructure.Membership.Enums;

namespace IMS.Api.RequestHandlers;

public class RegistrationRequestHandler
{
    [Required]
    public string? Email { get; set; }
    
    [Required]
    public string? Username { get; set; }
    
    [Required]
    public string? Password { get; set; }
    
    public Role Role { get; set; }
}