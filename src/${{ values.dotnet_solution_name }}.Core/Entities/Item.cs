namespace ${{ values.dotnet_solution_name }}.Core.Entities;

public class Item
{
    public int Id { get; set; }
    public required string Name { get; set; }
}

