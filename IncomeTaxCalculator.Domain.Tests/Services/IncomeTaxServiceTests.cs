using IncomeTaxCalculator.API.ViewModels.Responses;
using IncomeTaxCalculator.Domain.Services;

namespace IncomeTaxCalculator.Domain.Tests.Services
{
    public class TaxBandServiceTests
    {
        private readonly IMapper _mapper;
        private readonly Fixture _fixture;

        private readonly ITaxBandService _taxBandService;

        public TaxBandServiceTests()
        {
            _mapper = Substitute.For<IMapper>();
            _fixture = new Fixture();

            _taxBandService = Substitute.For<ITaxBandService>();
        }

        [Fact]
        public async Task GetAllTaxBandsAsync_Should_Calculate_Tax_Correctly()
        {
            // Arrange
            var sut = CreateSut();
            var grossAnnualSalary = 40_000m;

            var expected = new CalculateTaxResultDomainModel
            {
                GrossAnnualSalary = 40_000m,
                GrossMonthlySalary = 40_000m / 12,
                NetAnnualSalary = 29_000m,
                NetMonthlySalary = 29_000m / 12,
                AnnualTaxPaid = 11_000m,
                MonthlyTaxPaid = 11_000m / 12
            };

            _taxBandService.CalculateTotalBandTaxAsync(grossAnnualSalary).Returns(11_000m);

            // Act
            var actual = await sut.CalculateIncomeTaxAsync(grossAnnualSalary);

            // Assert
            actual.Should().BeEquivalentTo(expected);
        }

        private IncomeTaxService CreateSut()
        {
            return new IncomeTaxService(_taxBandService);
        }
    }
}
