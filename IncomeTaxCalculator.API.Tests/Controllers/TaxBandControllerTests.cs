using IncomeTaxCalculator.API.Controllers;
using IncomeTaxCalculator.API.ViewModels.Requests;
using IncomeTaxCalculator.API.ViewModels.Responses;
using IncomeTaxCalculator.Domain.DomainModels;
using IncomeTaxCalculator.Domain.Services;

namespace IncomeTaxCalculator.API.Tests.Controllers;

public class TaxBandControllerTests
{
    private readonly Fixture _fixture;

    private readonly ITaxBandService _taxBandService;
    private readonly IMapper _mapper;

    public TaxBandControllerTests()
    {
        _fixture = new Fixture();

        _taxBandService = Substitute.For<ITaxBandService>();
        _mapper = Substitute.For<IMapper>();
    }

    [Fact]
    public async Task GetAllTaxBands_Should_Return_OkObjectResult_With_TaxBandResponseViewModels()
    {
        // Arrange
        var sut = CreateSut();
        var mapperResult = _fixture.CreateMany<TaxBandResponseViewModel>(2);

        _mapper.Map<IEnumerable<TaxBandResponseViewModel>>(Arg.Any<IEnumerable<TaxBandDomainModel>>())
            .ReturnsForAnyArgs(mapperResult);

        // Act
        var response = await sut.GetAllTaxBands();

        // Assert
        var result = response.Should().BeOfType<OkObjectResult>().Subject;

        var actualResponseViewModel = (ResponseViewModel<IEnumerable<TaxBandResponseViewModel>>)result.Value;
        actualResponseViewModel.Result.Should().BeEquivalentTo(mapperResult);
    }

    [Fact]
    public async Task PushTaxBand_Invalid_AddTaxBandRequestViewModel_Should_Return_BadRequest()
    {
        // Arrange
        var sut = CreateSut();
        var request = _fixture.Create<AddTaxBandRequestViewModel>();

        sut.ModelState.AddModelError("New error", "Error!");

        // Act
        var response = await sut.PushTaxBand(request);

        // Assert
        response.Should().BeOfType<BadRequestObjectResult>();
    }

    [Fact]
    public async Task PushTaxBand_Should_Return_OkObjectResult_And_Calls_Service()
    {
        // Arrange
        var sut = CreateSut();
        var request = _fixture.Create<AddTaxBandRequestViewModel>();

        // Act
        var response = await sut.PushTaxBand(request);

        // Assert
        await _taxBandService.ReceivedWithAnyArgs(1).PushTaxBandAsync(Arg.Any<TaxBandDomainModel>());
        response.Should().BeOfType<OkObjectResult>();
    }

    [Fact]
    public async Task PopTaxBand_Should_Return_OkObjectResult_And_Calls_Service()
    {
        // Arrange
        var sut = CreateSut();

        // Act
        var response = await sut.PopTaxBand();

        // Assert
        await _taxBandService.ReceivedWithAnyArgs(1).PopTaxBandAsync();
        response.Should().BeOfType<OkObjectResult>();
    }

    private TaxBandController CreateSut()
    {
        return new TaxBandController(_mapper, _taxBandService);
    }
}
