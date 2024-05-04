using IMS.Infrastructure.Membership;
using IMS.Infrastructure.Membership.Tokens;

namespace IMS.Infrastructure.Tokens;

public interface ITokenService
{
    Task<TokenResponse> CreateTokenAsync(AppUser user);
    Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest request);
}