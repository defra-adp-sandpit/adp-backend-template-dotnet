using ADP.Backend.Template.Api.HealthChecks;
using FluentAssertions;
using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace ADP.Backend.Template.Api.Tests.HealthChecksTests;

[TestFixture]
public class LivenessCheckTests
{
    private readonly LivenessCheck _sut = new();

    [Test]
    public async Task CheckHealthAsync_Returns_Healthy()
    {
        //Arrange
        var mockContext = new HealthCheckContext();
        var cancellationToken = new CancellationToken();

        //Act
        var result = await _sut.CheckHealthAsync(mockContext, cancellationToken);

        //Assert
        result.Should().BeEquivalentTo(HealthCheckResult.Healthy("OK"));
    }
}