using IncomeTaxCalculator.API.Middleware;
using IncomeTaxCalculator.API.ViewModels.Responses;
using Microsoft.AspNetCore.Http;
using System.Text.Json;

namespace IncomeTaxCalculator.API.Tests.Middleware;

public class ExceptionHandlingMiddlewareTests
{
    private bool _delegateCalled;

    [Fact]
    public async Task InvokeAsync_Should_Call_Next()
    {
        // Arrange
        var context = CreateContext();
        var sut = CreateSut();

        // Act
        await sut.InvokeAsync(context);

        // Assert
        _delegateCalled.Should().BeTrue();
    }

    [Fact]
    public async Task InvokeAsync_OnNextCallException_Should_Return_Error_ResponseViewModel()
    {
        // Arrange
        var context = CreateContext();
        var sut = CreateSut(typeof(Exception), isErroredDelegate: true);

        // Act
        await sut.InvokeAsync(context);

        // Assert
        var responseModel = GetModel(context);

        responseModel.Should().NotBeNull();
        responseModel.IsSuccess.Should().Be(false);
        responseModel.Message.Should().StartWith("Internal Server Error");
    }

    private ResponseViewModel GetModel(HttpContext context)
    {
        context.Response.Body.Seek(0, SeekOrigin.Begin);
        var responseBody = new StreamReader(context.Response.Body).ReadToEnd();
        return JsonSerializer.Deserialize<ResponseViewModel>(responseBody);
    }

    private HttpContext CreateContext()
    {
        HttpContext context = new DefaultHttpContext();

        context.Response.Body = new MemoryStream();

        return context;
    }

    private ExceptionHandlingMiddleware CreateSut(Type exceptionType = null, bool isErroredDelegate = false)
    {
        _delegateCalled = false;

        RequestDelegate next = (HttpContext) =>
        {
            _delegateCalled = true;

            if (isErroredDelegate)
                throw (Exception)Activator.CreateInstance(exceptionType);

            return Task.CompletedTask;
        };

        return new ExceptionHandlingMiddleware(next);
    }
}
