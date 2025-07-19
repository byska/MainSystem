using Xunit;
using FluentAssertions;
using MainSystem.Tests.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Text.Json;

namespace MainSystem.Tests.Integration.Api;

public class RosterControllerIntegrationTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;
    private readonly HttpClient _client;

    public RosterControllerIntegrationTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient();
    }

    [Fact]
    public async Task CreateRoster_WithValidFlightNumber_ShouldReturnCreated()
    {
        // Arrange
        var flightNumber = "TK1234";
        
        // Act
        var response = await _client.PostAsync($"/api/rosters/{flightNumber}", null);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        
        var locationHeader = response.Headers.Location?.ToString();
        locationHeader.Should().NotBeNullOrEmpty();
        locationHeader.Should().Contain("/api/rosters/");
    }

    [Fact]
    public async Task GetRoster_WithValidId_ShouldReturnRoster()
    {
        // Arrange
        var rosterId = await CreateTestRoster();

        // Act
        var response = await _client.GetAsync($"/api/rosters/{rosterId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeNullOrEmpty();
        
        var roster = JsonSerializer.Deserialize<dynamic>(content);
        roster.Should().NotBeNull();
    }

    [Fact]
    public async Task GetRoster_WithInvalidId_ShouldReturnNotFound()
    {
        // Arrange
        var invalidId = Guid.NewGuid();

        // Act
        var response = await _client.GetAsync($"/api/rosters/{invalidId}");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task ExportRoster_WithValidId_ShouldReturnJsonFile()
    {
        // Arrange
        var rosterId = await CreateTestRoster();

        // Act
        var response = await _client.GetAsync($"/api/rosters/{rosterId}/export");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        response.Content.Headers.ContentType?.MediaType.Should().Be("application/json");
        
        var content = await response.Content.ReadAsStringAsync();
        content.Should().NotBeNullOrEmpty();
    }

    private async Task<Guid> CreateTestRoster()
    {
        var response = await _client.PostAsync("/api/rosters/TK1234", null);
        var location = response.Headers.Location?.ToString();
        var id = location?.Split('/').Last();
        return Guid.Parse(id!);
    }
}