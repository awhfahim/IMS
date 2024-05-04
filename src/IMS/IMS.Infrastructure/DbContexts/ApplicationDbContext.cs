using IMS.Infrastructure.Membership;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IMS.Infrastructure.DbContexts;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
    : IdentityDbContext<AppUser,AppRole, Guid>(options), IApplicationDbContext
{
    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
        
        builder.Entity<AppUser>(entity =>
        {
            entity.Property(e => e.RefreshToken).HasMaxLength(2000);
            entity.Property(e => e.RefreshTokenExpiryTime).HasColumnType("datetime");
        });
    }
}