using FluentAssertions;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;

namespace ADP.Backend.Template.Api.Tests;

[TestFixture]
public class ProgramTests
{
    [Test]
    public void ProgramConfiguresBuilder()
    {
        // Arrange
        var builder = WebApplication.CreateBuilder(options: new WebApplicationOptions()
        {
            EnvironmentName = "Production"
        });
        var inMemorySettings = new List<KeyValuePair<string, string?>>{
                new("AppInsights:ConnectionString", "InstrumentationKey=inskey;IngestionEndpoint=https://uksouth-1.in.applicationinsights.azure.com/;LiveEndpoint=https://uksouth.livediagnostics.monitor.azure.com/;ApplicationId=12345"),
                new("AppInsights:CloudRole", "TestCloudRole")
        };

        builder.Configuration.AddInMemoryCollection(inMemorySettings);

        // Act
        Program.ConfigureServices(builder);

        // Assert
        var app = builder.Build();

        app.Should().NotBeNull();
    }

}
