using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net;

namespace GymMgmt.Api.Tests;

public class WeatherForecastControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public WeatherForecastControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task Get_WeatherForecast_ShouldFail_TestingCIPipeline()
    {
        // This test is intentionally designed to FAIL 
        // to verify that GitHub Actions stops the pipeline on test failures

        // Act
        var response = await _client.GetAsync("/WeatherForecast");

        // Assert - This will fail because we expect 200 but get 500 (due to the exception)
        Assert.Equal(HttpStatusCode.OK, response.StatusCode);

        // Alternative failing assertions you could use:
        // Assert.True(false, "This test should fail to validate CI pipeline");
        // Assert.Equal("Expected", "Actual");
        // Assert.Null("NotNull");
    }

    [Fact]
    public async Task Get_WeatherForecast_AnotherFailingTest()
    {
        // Another way to make a test fail
        var response = await _client.GetAsync("/WeatherForecast");

        // This will fail because the endpoint throws an exception (returns 500)
        response.EnsureSuccessStatusCode(); // Throws if not 2xx status code
    }

    [Fact]
    public void SimpleMathTest_ShouldFail()
    {
        // Simple failing test that doesn't require HTTP calls
        int expected = 5;
        int actual = 2 + 2; // This equals 4, not 5

        Assert.Equal(expected, actual); // This will fail: Expected 5, Actual 4
    }
}