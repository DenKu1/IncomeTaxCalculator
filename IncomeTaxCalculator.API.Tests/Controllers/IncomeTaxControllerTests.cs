using IncomeTaxCalculator.API.Controllers;
using IncomeTaxCalculator.API.ViewModels.Requests;
using IncomeTaxCalculator.API.ViewModels.Responses;
using IncomeTaxCalculator.Domain.Services.Interfaces;

namespace IncomeTaxCalculator.API.Tests.Controllers;

public class IncomeTaxControllerTests
{
    private readonly Fixture _fixture;

    private readonly IIncomeTaxService _incomeTaxService;
    private readonly IMapper _mapper;

    public IncomeTaxControllerTests()
    {
        _fixture = new Fixture();

        _incomeTaxService = Substitute.For<IIncomeTaxService>();
        _mapper = Substitute.For<IMapper>();
    }

    [Fact]
    public async Task CalculateIncomeTax_Invalid_CalculateIncomeTaxRequestViewModel_Should_Return_BadRequest()
    {
        // Arrange
        var sut = CreateSut();
        var request = _fixture.Create<CalculateIncomeTaxRequestViewModel>();

        sut.ModelState.AddModelError("New error", "Error!");

        // Act
        var response = await sut.CalculateIncomeTax(request);

        // Assert
        response.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task CalculateIncomeTax_Should_Return_OkObjectResult_With_CalculateIncomeTaxRequestViewModel()
    {
        // Arrange
        var sut = CreateSut();
        var request = _fixture.Create<CalculateIncomeTaxRequestViewModel>();
        var mapperResult = _fixture.Create<CalculateIncomeTaxResponseViewModel>();

        _mapper.Map<CalculateIncomeTaxResponseViewModel>(Arg.Any<CalculateTaxResultDomainModel>())
            .ReturnsForAnyArgs(mapperResult);

        // Act
        var response = await sut.CalculateIncomeTax(request);

        // Assert
        var result = response.Should().BeOfType<OkObjectResult>().Subject;

        var actualResponseViewModel = (ResponseViewModel<CalculateIncomeTaxResponseViewModel>)result.Value;
        actualResponseViewModel.Result.Should().BeEquivalentTo(mapperResult);
    }

    private IncomeTaxController CreateSut()
    {
        return new IncomeTaxController(_mapper, _incomeTaxService);
    }
}
