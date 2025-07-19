using Xunit;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Net.Http.Headers;
using System.Text;

namespace MainSystem.Tests.Security;

public class AuthorizationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public AuthorizationTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateRoster_WithoutAuthentication_ShouldReturnUnauthorized()
    {
        // Act
        var response = await _client.PostAsync("/api/rosters/TK1234", null);

        // Assert
        response.StatusCode.Should().BeOneOf(HttpStatusCode.Unauthorized, HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task CreateRoster_WithViewerRole_ShouldReturnForbidden()
    {
        // Arrange
        var token = await GetTokenForRole("viewer@demo.io", "Passw0rd!");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act
        var response = await _client.PostAsync("/api/rosters/TK1234", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Forbidden);
    }

    [Fact]
    public async Task CreateRoster_WithAdminRole_ShouldSucceed()
    {
        // Arrange
        var token = await GetTokenForRole("admin@demo.io", "Passw0rd!");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act
        var response = await _client.PostAsync("/api/rosters/TK1234", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact]
    public async Task ViewRoster_WithAnyAuthenticatedUser_ShouldSucceed()
    {
        // Arrange
        var token = await GetTokenForRole("viewer@demo.io", "Passw0rd!");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        
        var rosterId = await CreateRosterAsAdmin();

        // Act
        var response = await _client.GetAsync($"/api/rosters/{rosterId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
    }

    [Theory]
    [InlineData("../../../etc/passwd")]           // Path traversal
    [InlineData("'; DROP TABLE Rosters; --")]    // SQL injection attempt
    [InlineData("<script>alert('xss')</script>")] // XSS attempt
    [InlineData("TK1234\0")]                      // Null byte injection
    public async Task CreateRoster_WithMaliciousInput_ShouldHandleSecurely(string maliciousInput)
    {
        // Arrange
        var token = await GetTokenForRole("admin@demo.io", "Passw0rd!");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        // Act
        var response = await _client.PostAsync($"/api/rosters/{maliciousInput}", null);

        // Assert
        response.StatusCode.Should().BeOneOf(
            HttpStatusCode.BadRequest,
            HttpStatusCode.NotFound,
            HttpStatusCode.UnprocessableEntity);
        
        // Should not return any sensitive error information
        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotContain("SQL");
        content.Should().NotContain("Exception");
        content.Should().NotContain("StackTrace");
    }

    private async Task<string> GetTokenForRole(string email, string password)
    {
        var loginData = new
        {
            Email = email,
            Password = password
        };

        var json = System.Text.Json.JsonSerializer.Serialize(loginData);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        
        var response = await _client.PostAsync("/api/auth/login", content);
        response.EnsureSuccessStatusCode();
        
        var responseContent = await response.Content.ReadAsStringAsync();
        var tokenData = System.Text.Json.JsonSerializer.Deserialize<Dictionary<string, object>>(responseContent);
        
        return tokenData["token"].ToString()!;
    }

    private async Task<Guid> CreateRosterAsAdmin()
    {
        var adminToken = await GetTokenForRole("admin@demo.io", "Passw0rd!");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);
        
        var response = await _client.PostAsync("/api/rosters/TK1234", null);
        var location = response.Headers.Location?.ToString();
        var id = location?.Split('/').Last();
        
        return Guid.Parse(id!);
    }
}