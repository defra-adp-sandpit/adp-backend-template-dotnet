using ${{ values.dotnet_solution_name }}.Core.Entities;
using ${{ values.dotnet_solution_name }}.Core.Exceptions;
using ${{ values.dotnet_solution_name }}.Core.Interfaces;
using Microsoft.Extensions.Logging;

namespace ${{ values.dotnet_solution_name }}.Core.Services;

public class ItemService : IItemService
{
    private readonly ILogger<ItemService> _logger;
    private static readonly List<Item> items =
            [
                new() { Id = 1, Name= "Item1" },
                new() { Id = 2, Name= "Item2" }
            ];
    public ItemService(ILogger<ItemService> logger)
    {
        _logger = logger;
    }

    public Task<Item> CreateItemAsync(Item item)
    {
        _logger.LogInformation("Creating item");
        var id = items.Max(x => x.Id) + 1;
        item.Id = id;
        items.Add(item);
        return Task.FromResult(item);
    }

    public Task<Item> DeleteItemAsync(int id)
    {
        _logger.LogInformation("Deleting item");
        var item = items.Find(x => x.Id == id);
        if (item != null)
        {
            items.Remove(item);
            return Task.FromResult(item);
        }
        throw new ItemNotFoundException("Item not found");
    }

    public Task<List<Item>> GetAllItemsAsync()
    {
        _logger.LogInformation("Getting all items");
        return Task.FromResult(items);
    }

    public Task<Item> GetItemByIdAsync(int id)
    {
        _logger.LogInformation("Getting item by id {Id}", id);
        var item = items.Find(x => x.Id == id);
        if (item != null)
        {
            return Task.FromResult(item);
        }
        throw new ItemNotFoundException("Item not found");
    }

    public Task<Item> UpdateItemAsync(Item item)
    {
        _logger.LogInformation("Updating item {Id}", item.Id);
        var existingItem = items.Find(x => x.Id == item.Id);
        if (existingItem != null)
        {
            items.Remove(existingItem);
            items.Add(item);
            return Task.FromResult(item);
        }
        throw new ItemNotFoundException("Item not found");
    }
}
