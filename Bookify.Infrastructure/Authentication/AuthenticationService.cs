using System.Net.Http.Json;
using Bookify.Application.Abstractions.Authentication;
using Bookify.Domain.Users;
using Bookify.Infrastructure.Authentication.Models;

namespace Bookify.Infrastructure.Authentication;

public sealed class AuthenticationService(HttpClient httpClient) : IAuthenticationService
{
    private const string PasswordCredentialType = "Password";
    public async Task<string> RegisterAsync(User user, string password, CancellationToken cancellationToken = default)
    {
        var userRepresentationModel = UserRepresentationModel.FromUser(user);
        userRepresentationModel.Credentials = new List<CredentialRepresentationModel>
        {
            new()
            {
                Value = password,
                Temporary = false,
                Type = PasswordCredentialType
            }
        };

        HttpResponseMessage response;
        try
        {
            response = await httpClient.PostAsJsonAsync("users", userRepresentationModel, cancellationToken);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return ExtractIdentityIdFromLocationHeader(response);
    }
    private static string ExtractIdentityIdFromLocationHeader(HttpResponseMessage httpResponseHeader)
    {
        const string userSegmentName = "users/";
        var locationHeader = httpResponseHeader.Headers.Location?.PathAndQuery;
        if (locationHeader is null)
            throw new InvalidOperationException("Location header cannot be null.");

        var userSegmentValueIndex =
            locationHeader.IndexOf(userSegmentName, StringComparison.InvariantCultureIgnoreCase);
        var userIdentityId = locationHeader.Substring(userSegmentValueIndex + userSegmentName.Length);

        return userIdentityId;
    }
}