using ADP.Backend.Template.Core.Entities;
using ADP.Backend.Template.Core.Exceptions;
using ADP.Backend.Template.Core.Services;
using AutoFixture;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using NSubstitute;

namespace ADP.Backend.Template.Core.Tests.ServicesTests;

[TestFixture]
public class ItemServiceTests
{
    private readonly Fixture _fixture;
    private readonly ILogger<ItemService> _mockLogger;
    private ItemService _sut;

    public ItemServiceTests()
    {
        _fixture = new Fixture();
        _mockLogger = Substitute.For<ILogger<ItemService>>();
    }

    [SetUp]
    public void SetUp()
    {
        _sut = new ItemService(_mockLogger);
    }

    [Test]
    public async Task GetAllItems_ReturnsAllItems()
    {
        // Act
        var result = await _sut.GetAllItems();
        // Assert
        result.Should().BeOfType<List<Item>>();
        result.Should().HaveCountGreaterThan(0);
    }

    [Test]
    public async Task GetById_Returns_Ok()
    {
        //Arrange
        var item = _fixture.Create<Item>();
        var createdItem = await _sut.CreateItem(item);
        // Act
        var result = await _sut.GetItemById(createdItem.Id);
        // Assert
        result.Should().BeOfType<Item>();
        result.Should().NotBeNull();
    }

    [Test]
    public void GetById_Returns_NotFound()
    {
        //Arrange
        var id = 0;
        // Act & Assert
        Assert.ThrowsAsync<ItemNotFoundException>(async () => await _sut.GetItemById(id));
    }

    [Test]
    public async Task Create_Returns_CreatedObject()
    {
        // Arrange
        var item = _fixture.Create<Item>();
        // Act
        var result = await _sut.CreateItem(item);
        // Assert
        result.Should().BeOfType<Item>();
        result.Should().NotBeNull();
    }

    [Test]
    public async Task Update_Returns_UpdatedObject()
    {
        // Arrange
        var item = _fixture.Create<Item>();
        item.Id = 1;
        // Act
        var result = await _sut.UpdateItem(item);
        // Assert
        result.Should().BeOfType<Item>();
        result.Should().NotBeNull();
    }

    [Test]
    public void Update_Returns_NotFound()
    {
        // Arrange
        var item = _fixture.Create<Item>();
        item.Id = 0;
        // Act & Assert
        Assert.ThrowsAsync<ItemNotFoundException>(async () => await _sut.UpdateItem(item));
    }

    [Test]
    public async Task Delete_Returns_DeletedObject()
    {
        // Arrange
        var item = _fixture.Create<Item>();
        var createdItem = await _sut.CreateItem(item);
        // Act
        var result = await _sut.DeleteItem(createdItem.Id);
        // Assert
        result.Should().BeOfType<Item>();
        result.Should().NotBeNull();
    }

    [Test]
    public void Delete_Returns_NotFound()
    {
        // Arrange
        var id = 0;
        // Act & Assert
        Assert.ThrowsAsync<ItemNotFoundException>(async () => await _sut.DeleteItem(id));
    }
}