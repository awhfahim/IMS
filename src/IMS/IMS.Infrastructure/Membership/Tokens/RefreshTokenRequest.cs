namespace IMS.Infrastructure.Membership.Tokens;

public record RefreshTokenRequest(string Token, string RefreshToken);