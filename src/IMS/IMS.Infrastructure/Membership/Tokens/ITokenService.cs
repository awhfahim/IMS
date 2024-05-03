namespace IMS.Infrastructure.Membership.Tokens;

public interface ITokenService
{
    Task<string> CreateTokenAsync(AppUser user);
    Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest request);
}