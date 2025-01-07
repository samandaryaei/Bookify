using System.Net.Http.Json;
using Bookify.Application.Abstractions;
using Bookify.Domain.Abstractions;
using Bookify.Infrastructure.Authentication.Models;
using Microsoft.Extensions.Options;

namespace Bookify.Infrastructure.Authentication;

public sealed class JwtService(HttpClient httpClient, IOptions<KeyCloakOptions> keyCloakOptions) : IJwtService
{
    private readonly KeyCloakOptions _keyCloakOptions = keyCloakOptions.Value;
    
    private static readonly Error AuthenticationFailed =
        new("Keycloak AuthenticationFailed","Failed to acquire access token do to authentication failure");
    public async Task<Result<string>> GetAccessTokenAsync(string email, string password, CancellationToken cancellationToken)
    {
        try
        {
            var authRequestParameters = new KeyValuePair<string, string>[]
            {
                new("client_id", _keyCloakOptions.AuthClientId),
                new("client_secret", _keyCloakOptions.AuthClientSecret),
                new("scope", "openid email"),
                new("grant_type", "password"),
                new("username", email),
                new("password", password)
            };
            
            var authorizationRequestContent = new FormUrlEncodedContent(authRequestParameters);
            var response = await httpClient.PostAsync("", authorizationRequestContent, cancellationToken);
            response.EnsureSuccessStatusCode();
            var authorizationToken =
                await response.Content.ReadFromJsonAsync<AuthorizationToken>(cancellationToken: cancellationToken);

            if (authorizationToken is null)
                return Result.Failure<string>(AuthenticationFailed);

            return authorizationToken.AccessToken;
        }
        catch (HttpRequestException)
        {
            return Result.Failure<string>(AuthenticationFailed);
        }
    }
}