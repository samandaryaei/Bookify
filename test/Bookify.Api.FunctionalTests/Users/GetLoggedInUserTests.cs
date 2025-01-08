using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using Bookify.Api.FunctionalTests.Infrastructure;
using Bookify.Application.Users.GetLoggedInUser;
using FluentAssertions;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace Bookify.Api.FunctionalTests.Users;

public class GetLoggedInUserTests(FunctionalTestWebAppFactory factory) : BaseFunctionalTest(factory)
{
    [Fact]
    public async Task Get_ShouldReturnUnautorized_WhenAccessTokenIsMissing()
    {
        // Act
        var response = await HttpClient.GetAsync("api/v1/users/me");
        
        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
    
    [Fact]
    public async Task Get_ShouldReturnUser_WhenAccessTokenIsNotMissing()
    {
        // Arrange
        var accessToken = await GetAccessToken();
        HttpClient.DefaultRequestHeaders.Authorization 
            = new AuthenticationHeaderValue(JwtBearerDefaults.AuthenticationScheme, accessToken);
        
        // Act
        var user = await HttpClient.GetFromJsonAsync<UserResponse>("api/v1/users/me");
        
        // Assert
        user.Should().NotBeNull();
    }
}