using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using IMS.Domain.Exceptions;
using IMS.Infrastructure.Membership;
using IMS.Infrastructure.Membership.Tokens;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

namespace IMS.Infrastructure.Tokens;

public class TokenService(ILogger<TokenService> logger,
    UserManager<AppUser> userManager, IOptions<JwtOptions> jwtOptions) : ITokenService
{
    private const int ExpirationMinutes = 30;
    private JwtOptions JwtOptionsValue => jwtOptions.Value;

    public async Task<TokenResponse> CreateTokenAsync(AppUser user)
    {
        var expiration = DateTime.UtcNow.AddMinutes(ExpirationMinutes);
        var token = await CreateJwtTokenAsync(
            CreateClaims(user),
            CreateSigningCredentials(),
            expiration
        );
        
        var tokenHandler = new JwtSecurityTokenHandler();
        var jwtToken = tokenHandler.WriteToken(token);

        logger.LogInformation("JWT Token created");

        // Generate refresh token and set its expiry time
        var refreshToken = GenerateRefreshToken();
        var refreshTokenExpiryTime = DateTime.UtcNow.AddDays(1);

        // Update user with refresh token and its expiry time
        user.RefreshToken = refreshToken;
        user.RefreshTokenExpiryTime = refreshTokenExpiryTime;
        await userManager.UpdateAsync(user);

        // Return TokenResponse with JWT token, refresh token, and expiry time
        return new TokenResponse(jwtToken, refreshToken, refreshTokenExpiryTime);
    }

    public async Task<TokenResponse> RefreshTokenAsync(RefreshTokenRequest request)
    {
        var userPrincipal = GetPrincipalFromExpiredToken(request.Token);
        var userEmail = userPrincipal.FindFirstValue(ClaimTypes.Email);
        var user = await userManager.FindByEmailAsync(userEmail!);
        if (user is null)
        {
            throw new UnauthorizedException("Authentication Failed.");
        }

        if (user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
        {
            throw new UnauthorizedException("Invalid Refresh Token.");
        }

        return await GenerateTokensAndUpdateUser(user);
    }
    
    private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
    {
        var tokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOptionsValue.Key)),
            ValidateIssuer = false,
            ValidateAudience = false,
            RoleClaimType = ClaimTypes.Role,
            ClockSkew = TimeSpan.Zero,
            ValidateLifetime = false
        };
        var tokenHandler = new JwtSecurityTokenHandler();
        var principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out var securityToken);
        if (securityToken is not JwtSecurityToken jwtSecurityToken ||
            !jwtSecurityToken.Header.Alg.Equals(
                SecurityAlgorithms.HmacSha256,
                StringComparison.InvariantCultureIgnoreCase))
        {
            throw new UnauthorizedException("Invalid Token.");
        }

        return principal;
    }
    private async Task<TokenResponse> GenerateTokensAndUpdateUser(AppUser user)
    {
        var tokenResponse = await CreateTokenAsync(user);

        user.RefreshToken = GenerateRefreshToken();
        user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(1);

        await userManager.UpdateAsync(user);

        return new TokenResponse(tokenResponse.Token, user.RefreshToken, user.RefreshTokenExpiryTime);
    }
    
    private static string GenerateRefreshToken()
    {
        var randomNumber = new byte[32];
        using var rng = RandomNumberGenerator.Create();
        rng.GetBytes(randomNumber);
        return Convert.ToBase64String(randomNumber);
    }
    private async Task<JwtSecurityToken> CreateJwtTokenAsync(IEnumerable<Claim> claims, SigningCredentials credentials,
        DateTime expiration) =>
        new(
            JwtOptionsValue.ValidIssuer,
            JwtOptionsValue.ValidAudience,
            claims,
            expires: expiration,
            signingCredentials: credentials
        );

    private IEnumerable<Claim> CreateClaims(AppUser user)
        => new List<Claim>
        {
            new (JwtRegisteredClaimNames.Sub, JwtOptionsValue.JwtRegisteredClaimNamesSub),
            new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new (JwtRegisteredClaimNames.Iat, DateTimeOffset.UtcNow.ToUnixTimeSeconds().ToString()),
            new (ClaimTypes.NameIdentifier, user.Id.ToString()),
            new (ClaimTypes.Name, user.UserName),
            new (ClaimTypes.Email, user.Email),
            new (ClaimTypes.Role, user.Role.ToString())
        };

    private SigningCredentials CreateSigningCredentials()
        => new(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(JwtOptionsValue.Key)),
            SecurityAlgorithms.HmacSha256
        );
}