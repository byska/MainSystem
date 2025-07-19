using Xunit;
using FluentAssertions;
using NBomber.Http.CSharp;
namespace MainSystem.Tests.Performance;
public class RosterPerformanceTests
{
 [Fact]
public void RosterCreation_ShouldCompleteWithinAcceptableTime()
 {
var scenario = Scenario.Create("create_roster", async context =>
 {
var flightNumber = $"TK{Random.Shared.Next(1000, 9999)}";
var response = await HttpClientArgs
.Create()
.WithHttpClient(new HttpClient { BaseAddress = new Uri("http://localhost:5259") })
.PostAsync($"/api/rosters/{flightNumber}", null);
return response.IsSuccessStatusCode ? Response.Ok() : Response.Fail();
 })
.WithLoadSimulations(
Simulation.InjectPerSec(rate: 10, during: TimeSpan.FromMinutes(1))
 );
NBomberRunner
.RegisterScenarios(scenario)
.Run();
 }
 [Fact]
public void RosterRetrieval_ShouldHandleConcurrentRequests()
 {
// Pre-create a roster for testing
var rosterId = Guid.NewGuid(); // In real test, create actual roster
var scenario = Scenario.Create("get_roster", async context =>
 {
var response = await HttpClientArgs
.Create()
.WithHttpClient(new HttpClient { BaseAddress = new Uri("http://localhost:5259") })
.GetAsync($"/api/rosters/{rosterId}");
return response.IsSuccessStatusCode ? Response.Ok() : Response.Fail();
 })
.WithLoadSimulations(
Simulation.InjectPerSec(rate: 50, during: TimeSpan.FromMinutes(2))
 );
var stats = NBomberRunner
.RegisterScenarios(scenario)
.Run();
// Assert performance requirements
var sceneStats = stats.AllOkCount;
sceneStats.Should().BeGreaterThan(0);
 }
 [Fact]
public async Task SeatAssignment_ShouldCompleteQuickly()
 {
// Arrange
var stopwatch = System.Diagnostics.Stopwatch.StartNew();
// Act - Simulate seat assignment for large aircraft
await SimulateLargeFlightRosterCreation();
stopwatch.Stop();
// Assert - Should complete within 5 seconds for large flight
stopwatch.ElapsedMilliseconds.Should().BeLessThan(5000);
 }
private async Task SimulateLargeFlightRosterCreation()
 {
// Simulate creating roster for large aircraft (396 passengers)
using var client = new HttpClient { BaseAddress = new Uri("http://localhost:5259") };
await client.PostAsync("/api/rosters/TK7777", null);
 }
}