using IncomeTaxCalculator.Domain.DomainModels;
using IncomeTaxCalculator.Domain.Profiles;
using IncomeTaxCalculator.Persistence.Entities;

namespace IncomeTaxCalculator.Domain.Tests.Profiles;

public class TaxBandDomainModelProfileTests
{
    private readonly IMapper _mapper;
    private readonly Fixture _fixture;

    public TaxBandDomainModelProfileTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<TaxBandDomainModelProfile>();
        });

        _mapper = new Mapper(config);
        _fixture = new Fixture();
    }

    [Fact]
    public void Should_Map_From_TaxBand_To_TaxBandDomainModel()
    {
        // Arrange
        var given = _fixture.Create<TaxBand>();

        // Act
        var actual = _mapper.Map<TaxBandDomainModel>(given);

        // Assert
        actual.Should().BeEquivalentTo(given);
    }

    [Fact]
    public void Should_Map_From_TaxBandDomainModel_To_TaxBand()
    {
        // Arrange
        var given = _fixture.Create<TaxBandDomainModel>();

        // Act
        var actual = _mapper.Map<TaxBand>(given);

        // Assert
        actual.Should().BeEquivalentTo(given);
    }
}
