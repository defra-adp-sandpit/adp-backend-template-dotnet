using ADP.Backend.Template.Core.Exceptions;

namespace ADP.Backend.Template.Core.Tests.ExceptionTests;

public class ItemNotFoundExceptionTests
{
    [Fact]
    public void ItemNotFoundExceptionTests_Creates_Exceptions()
    {
        var exception = new ItemNotFoundException("Error");
        exception.Should().BeAssignableTo<Exception>();
        exception.Message.Should().Be("Error");
    }

    [Fact]
    public void ItemNotFoundExceptionTests_Creates_Exception_With_ParentException()
    {
        var exception = new ItemNotFoundException("Error", new Exception("Parent error"));
        exception.Should().BeAssignableTo<Exception>();
        exception.Message.Should().Be("Error");
    }
}