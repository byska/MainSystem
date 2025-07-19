using Xunit;
using FluentAssertions;
using MainSystem.Tests.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using System.Net;
using System.Text.Json;

namespace MainSystem.Tests.Acceptance.Features;

/// <summary>
/// Feature: Flight Roster Creation
/// As a flight operations manager
/// I want to create flight rosters automatically
/// So that I can efficiently manage crew and passenger assignments
/// </summary>
public class RosterCreationFeatureTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public RosterCreationFeatureTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Scenario_CreateCompleteRosterForDomesticFlight()
    {
        // Given I have a domestic flight TK1234
        var flightNumber = "TK1234";
        
        // When I create a roster for this flight
        var createResponse = await _client.PostAsync($"/api/rosters/{flightNumber}", null);
        
        // Then the roster should be created successfully
        createResponse.StatusCode.Should().Be(HttpStatusCode.Created);
        
        // And I should get a roster ID
        var location = createResponse.Headers.Location?.ToString();
        location.Should().NotBeNullOrEmpty();
        var rosterId = ExtractIdFromLocation(location!);
        
        // When I retrieve the created roster
        var getResponse = await _client.GetAsync($"/api/rosters/{rosterId}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var rosterJson = await getResponse.Content.ReadAsStringAsync();
        var roster = JsonSerializer.Deserialize<JsonElement>(rosterJson);
        
        // Then the roster should contain all required crew members
        roster.GetProperty("pilots").GetArrayLength().Should().BeGreaterOrEqualTo(2);
        roster.GetProperty("attendants").GetArrayLength().Should().BeGreaterOrEqualTo(5);
        roster.GetProperty("passengers").GetArrayLength().Should().BeGreaterThan(0);
        
        // And all passengers should have assigned seats
        var passengers = roster.GetProperty("passengers").EnumerateArray();
        passengers.Should().OnlyContain(p => 
            p.TryGetProperty("seatNumber", out var seat) && !seat.ValueEquals("null"));
    }

    [Fact]
    public async Task Scenario_ExportRosterToJsonFile()
    {
        // Given I have a created roster
        var rosterId = await CreateTestRoster("TK5678");
        
        // When I export the roster
        var exportResponse = await _client.GetAsync($"/api/rosters/{rosterId}/export");
        
        // Then I should get a JSON file
        exportResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        exportResponse.Content.Headers.ContentType?.MediaType.Should().Be("application/json");
        
        // And the file should contain roster data
        var content = await exportResponse.Content.ReadAsStringAsync();
        content.Should().NotBeNullOrEmpty();
        content.Should().Contain("flightInfo");
        content.Should().Contain("pilots");
        content.Should().Contain("attendants");
        content.Should().Contain("passengers");
    }

    [Fact]
    public async Task Scenario_ViewRosterInPlaneLayout()
    {
        // Given I have a created roster
        var rosterId = await CreateTestRoster("TK9012");
        
        // When I request the plane view
        var planeViewResponse = await _client.GetAsync($"/api/rosters/{rosterId}/plane-view");
        
        // Then I should get seat layout information
        planeViewResponse.StatusCode.Should().Be(HttpStatusCode.OK);
        
        var planeViewJson = await planeViewResponse.Content.ReadAsStringAsync();
        var seats = JsonSerializer.Deserialize<JsonElement[]>(planeViewJson);
        
        // And the seats should be properly organized
        seats.Should().NotBeEmpty();
        seats.Should().OnlyContain(seat => 
            seat.TryGetProperty("row", out _) && 
            seat.TryGetProperty("column", out _) &&
            seat.TryGetProperty("occupied", out _));
    }

    [Fact]
    public async Task Scenario_HandleInvalidFlightNumber()
    {
        // Given I have an invalid flight number
        var invalidFlightNumber = "INVALID123";
        
        // When I try to create a roster
        var response = await _client.PostAsync($"/api/rosters/{invalidFlightNumber}", null);
        
        // Then I should get an error response
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Scenario_AccessRosterWithoutPermission()
    {
        // Given I am not authenticated
        // (No authentication headers set)
        
        // When I try to create a roster
        var response = await _client.PostAsync("/api/rosters/TK1234", null);
        
        // Then I should be denied access
        response.StatusCode.Should().BeOneOf(HttpStatusCode.Unauthorized, HttpStatusCode.Forbidden);
    }

    private async Task<Guid> CreateTestRoster(string flightNumber)
    {
        var response = await _client.PostAsync($"/api/rosters/{flightNumber}", null);
        var location = response.Headers.Location?.ToString();
        return ExtractIdFromLocation(location!);
    }

    private static Guid ExtractIdFromLocation(string location)
    {
        var id = location.Split('/').Last();
        return Guid.Parse(id);
    }
}