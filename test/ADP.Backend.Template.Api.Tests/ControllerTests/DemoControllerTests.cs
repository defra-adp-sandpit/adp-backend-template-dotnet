using ADP.Backend.Template.Api.Controllers;
using ADP.Backend.Template.Api.Models;
using ADP.Backend.Template.Core.Entities;
using ADP.Backend.Template.Core.Exceptions;
using ADP.Backend.Template.Core.Interfaces;
using AutoFixture;
using FluentAssertions;
using Mapster;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using NSubstitute;
using NSubstitute.ExceptionExtensions;

namespace ADP.Backend.Template.Api.Tests.Controllers;

[TestFixture]
public class DemoControllerTests
{
    private readonly Fixture _fixture;
    private readonly ILogger<DemoController> _mockLogger;
    private readonly IItemService _mockItemService;
    private readonly DemoController _sut;

    public DemoControllerTests()
    {
        _fixture = new Fixture();
        _mockLogger = Substitute.For<ILogger<DemoController>>();
        _mockItemService = Substitute.For<IItemService>();
        _sut = new DemoController(_mockItemService, _mockLogger);
    }

    [Test]
    public async Task GetAllItems_ReturnsAllItems()
    {
        // Arrange
        var items = _fixture.CreateMany<Item>().ToList();
        _mockItemService.GetAllItems().Returns(items);
        // Act
        var result = await _sut.Get();
        // Assert
        result.Should().BeOfType<OkObjectResult>();
        result.As<OkObjectResult>().Value.Should().BeEquivalentTo(items);
    }

    [Test]
    public async Task GetById_Returns_Ok()
    {
        // Arrange
        var item = _fixture.Create<Item>();
        _mockItemService.GetItemById(Arg.Any<int>()).Returns(item);
        // Act
        var result = await _sut.Get(1);
        // Assert
        result.Should().BeOfType<OkObjectResult>();
        result.As<OkObjectResult>().Value.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task GetById_Returns_NotFound()
    {
        // Arrange
        _mockItemService.GetItemById(Arg.Any<int>()).Throws<ItemNotFoundException>();
        // Act
        var result = await _sut.Get(1);
        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public async Task Post_Returns_Created()
    {
        // Arrange
        var itemRequest = _fixture.Create<ItemRequest>();
        var item = itemRequest.Adapt<Item>();
        _mockItemService.CreateItem(Arg.Any<Item>()).Returns(item);
        // Act
        var result = await _sut.Post(itemRequest);
        // Assert
        result.Should().BeOfType<ObjectResult>();
        result.As<ObjectResult>().Value.Should().BeEquivalentTo(item);
        result.As<ObjectResult>().StatusCode.Should().Be(StatusCodes.Status201Created);
    }

    [Test]
    public async Task Patch_Returns_Ok()
    {
        // Arrange
        var itemRequest = _fixture.Create<ItemRequest>();
        var item = itemRequest.Adapt<Item>();
        _mockItemService.UpdateItem(Arg.Any<Item>()).Returns(item);
        // Act
        var result = await _sut.Patch(itemRequest);
        // Assert
        result.Should().BeOfType<OkObjectResult>();
        result.As<OkObjectResult>().Value.Should().BeEquivalentTo(item);
    }

    [Test]
    public async Task Patch_Returns_NotFound()
    {
        // Arrange
        var itemRequest = _fixture.Create<ItemRequest>();
        _mockItemService.UpdateItem(Arg.Any<Item>()).Throws<ItemNotFoundException>();
        // Act
        var result = await _sut.Patch(itemRequest);
        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }

    [Test]
    public async Task Delete_Returns_Ok()
    {
        // Arrange
        var item = _fixture.Create<Item>();
        _mockItemService.DeleteItem(Arg.Any<int>()).Returns(item);
        // Act
        var result = await _sut.Delete(1);
        // Assert
        result.Should().BeOfType<NoContentResult>();
    }

    [Test]
    public async Task Delete_Returns_NotFound()
    {
        // Arrange
        _mockItemService.DeleteItem(Arg.Any<int>()).Throws<ItemNotFoundException>();
        // Act
        var result = await _sut.Delete(1);
        // Assert
        result.Should().BeOfType<NotFoundResult>();
    }
}