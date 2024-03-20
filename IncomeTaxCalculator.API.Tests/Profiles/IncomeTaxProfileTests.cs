using IncomeTaxCalculator.API.Profiles;
using IncomeTaxCalculator.API.ViewModels.Responses;

namespace IncomeTaxCalculator.API.Tests.Profiles;

public class IncomeTaxProfileTests
{
    private readonly IMapper _mapper;
    private readonly Fixture _fixture;

    public IncomeTaxProfileTests()
    {
        var config = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<IncomeTaxProfile>();
        });

        _mapper = new Mapper(config);
        _fixture = new Fixture();
    }

    [Fact]
    public void Should_Map_From_CalculateTaxResultDomainModel_To_CalculateIncomeTaxResponseViewModel()
    {
        // Arrange
        var given = _fixture.Create<CalculateTaxResultDomainModel>();

        // Act
        var actual = _mapper.Map<CalculateIncomeTaxResponseViewModel>(given);

        // Assert
        actual.Should().BeEquivalentTo(given);
    }
}
