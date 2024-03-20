using IncomeTaxCalculator.API.Profiles;
using IncomeTaxCalculator.API.ViewModels.Requests;
using IncomeTaxCalculator.API.ViewModels.Responses;
using IncomeTaxCalculator.Domain.DomainModels;

namespace IncomeTaxCalculator.API.Tests.Profiles;

public class TaxBandProfileTests
{
    private readonly IMapper _mapper;
    private readonly Fixture _fixture;

    public TaxBandProfileTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<TaxBandProfile>();
        });

        _mapper = new Mapper(config);
        _fixture = new Fixture();
    }

    [Fact]
    public void Should_Map_From_AddTaxBandRequestViewModel_To_TaxBandDomainModel()
    {
        // Arrange
        var given = _fixture.Create<AddTaxBandRequestViewModel>();

        // Act
        var actual = _mapper.Map<TaxBandDomainModel>(given);

        // Assert
        actual.Should().BeEquivalentTo(given);
    }

    [Fact]
    public void Should_Map_From_TaxBandDomainModel_To_TaxBandResponseViewModel()
    {
        // Arrange
        var given = _fixture.Create<TaxBandDomainModel>();

        // Act
        var actual = _mapper.Map<TaxBandResponseViewModel>(given);

        // Assert
        actual.Should().BeEquivalentTo(given);
    }
}
