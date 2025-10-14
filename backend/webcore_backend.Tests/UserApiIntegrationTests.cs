using System.Net.Http.Json;
using webcore_backend.Features.Users.Core.Dtos;
using webcore_backend.Shared.Users.Entities;

namespace webcore_backend.Tests;

[CollectionDefinition("UserApi")]
public class UserDefinition : ICollectionFixture<UserEntity>;

[Collection("UserApi")]
public class UserApiIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    
    HttpMessageHandler _handler;
    
    private readonly HttpClient _client;

    public UserApiIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Register_And_Get_User_Should_Work()
    {
        var request = new RegisterUserRequestDto(
            Username: "test",
            Email: "test@email.com",
            Password: "password"
        );

        // POST /users/register
        var response = await _client.PostAsJsonAsync("/api/users/register", request);
        response.EnsureSuccessStatusCode();

        var userId = await response.Content.ReadFromJsonAsync<Guid>();

        // GET /users/{id}
        var userResponse = await _client.GetFromJsonAsync<GetUserResponseDto>($"/api/users/{userId}");

        Assert.Equal("test", userResponse!.Username);
    }
}