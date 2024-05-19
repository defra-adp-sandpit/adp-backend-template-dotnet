namespace ${{ values.dotnet_solution_name }}.Api.Config;

public class AppInsightsConfig
{
    public required string ConnectionString { get; set; }
    public required string CloudRole { get; set; }
}

