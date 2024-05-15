using ADP.Backend.Template.Api.Models;
using ADP.Backend.Template.Core.Entities;
using ADP.Backend.Template.Core.Exceptions;
using ADP.Backend.Template.Core.Interfaces;
using Asp.Versioning;
using Mapster;
using Microsoft.AspNetCore.Mvc;

namespace ADP.Backend.Template.Api.Controllers;

[Route("api/[controller]")]
[ApiVersion("1.0")]
[ApiController]
public class DemoController : ControllerBase
{
    private readonly ILogger<DemoController> _logger;
    private readonly IItemService _itemService;
    public DemoController(IItemService itemService, ILogger<DemoController> logger)
    {
        _itemService = itemService;
        _logger = logger;
    }

    /// <summary>
    /// GET method
    /// </summary>
    /// <returns>ActionResult</returns>
    [HttpGet("", Name = "Get")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    public async Task<IActionResult> Get()
    {
        _logger.LogInformation("GET method on Demo controller to getAll");
        var result = await _itemService.GetAllItems();
        return Ok(result);
    }

    /// <summary>
    /// GET by Id method
    /// </summary>
    /// <param name="id">Id of the object to be retrieved</param>
    /// <returns>ActionResult</returns>
    [HttpGet("{id}", Name = "GetById")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Get(int id)
    {
        try
        {
            _logger.LogInformation("GET method on Demo controller to getById");
            var result = await _itemService.GetItemById(id);
            return Ok(result);
        }
        catch (ItemNotFoundException ex)
        {
            _logger.LogError(ex, "Retrieving item threw exception: {Message}", ex.Message);
            return NotFound();
        }
    }

    /// <summary>
    /// POST to create a new item
    /// </summary>
    /// <param name="item">item to be created</param>
    /// <returns>Item created</returns>
    [HttpPost("", Name = "Create")]
    [ProducesResponseType(StatusCodes.Status201Created)]
    public async Task<IActionResult> Post([FromBody] ItemRequest item)
    {
        _logger.LogInformation("POST method on Demo controller to create");
        var itemEntity = item.Adapt<Item>();
        var result = await _itemService.CreateItem(itemEntity);
        return new ObjectResult(result)
        {
            StatusCode = StatusCodes.Status201Created
        };
    }

    /// <summary>
    /// PATCH to update an existing item
    /// </summary>
    /// <param name="item">item to be created</param>
    /// <returns>Item created</returns>
    [HttpPatch("", Name = "Update")]
    public async Task<IActionResult> Patch([FromBody] ItemRequest item)
    {
        try
        {
            _logger.LogInformation("PATCH method on Demo controller to update");
            var itemEntity = item.Adapt<Item>();
            var result = await _itemService.UpdateItem(itemEntity);
            return Ok(result);
        }
        catch (ItemNotFoundException ex)
        {
            _logger.LogError(ex, "Updating item threw exception: {Message}", ex.Message);
            return NotFound();
        }
    }

    /// <summary>
    /// DELETE to delete an existing item
    /// </summary>
    /// <param name="id">Id of the item to be deleted</param>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            _logger.LogInformation("PATCH method on Demo controller to update");
            await _itemService.DeleteItem(id);
            return NoContent();
        }
        catch (ItemNotFoundException ex)
        {
            _logger.LogError(ex, "Deleting item threw exception: {Message}", ex.Message);
            return NotFound();
        }
    }
}

