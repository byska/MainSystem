// using Xunit;
// using FluentAssertions;

// namespace MainSystem.Tests.Performance;

// public class LoadStressTests
// {
//     [Fact]
//     public void StressTest_ConcurrentRosterCreation()
//     {
//         var scenario = Scenario.Create("stress_create_rosters", async context =>
//         {
//             var flightNumber = $"ST{Random.Shared.Next(1000, 9999)}";
            
//             using var httpClient = new HttpClient();
//             httpClient.BaseAddress = new Uri("http://localhost:5259");
            
//             var response = await httpClient.PostAsync($"/api/rosters/{flightNumber}", null);
            
//             return response.IsSuccessStatusCode ? Response.Ok() : Response.Fail();
//         })
//         .WithLoadSimulations(
//             // Gradually increase load
//             Simulation.InjectPerSec(rate: 5, during: TimeSpan.FromMinutes(1)),
//             Simulation.InjectPerSec(rate: 10, during: TimeSpan.FromMinutes(1)),
//             Simulation.InjectPerSec(rate: 20, during: TimeSpan.FromMinutes(1)),
//             Simulation.InjectPerSec(rate: 50, during: TimeSpan.FromMinutes(2))
//         );

//         var stats = NBomberRunner
//             .RegisterScenarios(scenario)
//             .WithReportFolder("load_test_reports")
//             .Run();

//         // Verify system handles load gracefully
//         Assert.True(stats.AllFailCount < stats.AllOkCount * 0.1); // Less than 10% failure rate
//     }

//     [Fact]
//     public void LoadTest_MixedOperations()
//     {
//         var createScenario = Scenario.Create("create_operations", async context =>
//         {
//             var flightNumber = $"CR{Random.Shared.Next(1000, 9999)}";
//             using var client = new HttpClient { BaseAddress = new Uri("http://localhost:5259") };
//             var response = await client.PostAsync($"/api/rosters/{flightNumber}", null);
//             return response.IsSuccessStatusCode ? Response.Ok() : Response.Fail();
//         })
//         .WithWeight(30)
//         .WithLoadSimulations(
//             Simulation.InjectPerSec(rate: 10, during: TimeSpan.FromMinutes(3))
//         );

//         var readScenario = Scenario.Create("read_operations", async context =>
//         {
//             var rosterId = GetRandomExistingRosterId(); // Implementation needed
//             using var client = new HttpClient { BaseAddress = new Uri("http://localhost:5259") };
//             var response = await client.GetAsync($"/api/rosters/{rosterId}");
//             return response.IsSuccessStatusCode ? Response.Ok() : Response.Fail();
//         })
//         .WithWeight(70)
//         .WithLoadSimulations(
//             Simulation.InjectPerSec(rate: 30, during: TimeSpan.FromMinutes(3))
//         );

//         NBomberRunner
//             .RegisterScenarios(createScenario, readScenario)
//             .WithReportFolder("mixed_load_reports")
//             .Run();
//     }

//     [Fact]
//     public void SpikeTest_SuddenLoadIncrease()
//     {
//         var scenario = Scenario.Create("spike_test", async context =>
//         {
//             using var client = new HttpClient { BaseAddress = new Uri("http://localhost:5259") };
//             var response = await client.GetAsync("/api/rosters");
//             return response.IsSuccessStatusCode ? Response.Ok() : Response.Fail();
//         })
//         .WithLoadSimulations(
//             // Normal load
//             Simulation.InjectPerSec(rate: 10, during: TimeSpan.FromMinutes(2)),
//             // Sudden spike
//             Simulation.InjectPerSec(rate: 100, during: TimeSpan.FromSeconds(30)),
//             // Back to normal
//             Simulation.InjectPerSec(rate: 10, during: TimeSpan.FromMinutes(2))
//         );

//         var stats = NBomberRunner
//             .RegisterScenarios(scenario)
//             .WithReportFolder("spike_test_reports")
//             .Run();

//         // System should recover from spike
//         Assert.True(stats.AllFailCount < stats.AllRequestCount * 0.15); // Less than 15% failure
//     }

//     private Guid GetRandomExistingRosterId()
//     {
//         // In real implementation, this would fetch from a pool of pre-created rosters
//         return Guid.NewGuid();
//     }
// }