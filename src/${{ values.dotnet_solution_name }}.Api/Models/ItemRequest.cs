using System.ComponentModel.DataAnnotations;

namespace ${{ values.dotnet_solution_name }}.Api.Models;

public class ItemRequest
{
    public int? Id { get; set; }
    [Required]
    public required string Name { get; set; }
}

