using IncomeTaxCalculator.API.Middleware;
using IncomeTaxCalculator.Domain.Exceptions;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace IncomeTaxCalculator.API.Tests.Middleware;

public class LoggingMiddlewareTests
{
    private bool _delegateCalled;

    private readonly ILogger<LoggingMiddleware> _logger;

    public LoggingMiddlewareTests()
    {
        _logger = Substitute.For<ILogger<LoggingMiddleware>>();
    }

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
    public async Task InvokeAsync_OnNextCall_TaxBandOperationException_Should_Rethrow_Exception_And_Log_Warning()
    {
        // Arrange
        var context = CreateContext();
        var sut = CreateSut(typeof(TaxBandOperationException), isErroredDelegate: true);

        // Act
        var act = async () => await sut.InvokeAsync(context);

        // Assert
        await act.Should().ThrowAsync<TaxBandOperationException>();
        _logger.Received(1).Log(
            Arg.Is<LogLevel>(x => x == LogLevel.Warning),
            Arg.Any<EventId>(),
            Arg.Any<object>(),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception, string>>()
        );
    }

    [Fact]
    public async Task InvokeAsync_OnNextCall_Exception_Should_Rethrow_Exception_And_Log_Error()
    {
        // Arrange
        var context = CreateContext();
        var sut = CreateSut(typeof(Exception), isErroredDelegate: true);

        // Act
        var act = async () => await sut.InvokeAsync(context);

        // Assert
        await act.Should().ThrowAsync<Exception>();
        _logger.Received(1).Log(
            Arg.Is<LogLevel>(x => x == LogLevel.Error),
            Arg.Any<EventId>(),
            Arg.Any<object>(),
            Arg.Any<Exception>(),
            Arg.Any<Func<object, Exception, string>>()
        );
    }

    private HttpContext CreateContext()
    {
        HttpContext context = new DefaultHttpContext();

        context.Response.Body = new MemoryStream();

        return context;
    }

    private LoggingMiddleware CreateSut(Type exceptionType = null, bool isErroredDelegate = false)
    {
        _delegateCalled = false;

        RequestDelegate next = (HttpContext) =>
        {
            _delegateCalled = true;

            if (isErroredDelegate)
                throw (Exception)Activator.CreateInstance(exceptionType);

            return Task.CompletedTask;
        };

        return new LoggingMiddleware(next, _logger);
    }
}
