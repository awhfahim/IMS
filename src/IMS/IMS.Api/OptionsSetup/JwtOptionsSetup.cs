using IMS.Infrastructure.Membership.Tokens;
using Microsoft.Extensions.Options;

namespace IMS.Api.OptionsSetup;

public class JwtOptionsSetup(IConfiguration configuration) : IConfigureOptions<JwtOptions>
{
    private const string SectionName = "JwtTokenSettings";
    public void Configure(JwtOptions options)
    {
        configuration.GetSection(SectionName).Bind(options);
    }
}