namespace IMS.Infrastructure.Membership.Tokens;

public interface ITokenService
{
    Task<TokenResponse> CreateTokenAsync(AppUser user);
    Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest request);
}