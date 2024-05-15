using System.ComponentModel.DataAnnotations;

namespace ADP.Backend.Template.Api.Models;

public class ItemRequest
{
    public int? Id { get; set; }
    [Required]
    public required string Name { get; set; }
}

