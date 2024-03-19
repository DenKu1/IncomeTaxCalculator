using IncomeTaxCalculator.Domain.DomainModels;
using IncomeTaxCalculator.Domain.Exceptions;
using IncomeTaxCalculator.Domain.Services;
using IncomeTaxCalculator.Persistence.Entities;
using IncomeTaxCalculator.Persistence.Repositories.Interfaces;

namespace IncomeTaxCalculator.Domain.Tests.Services
{
    public class IncomeTaxServiceTests
    {
        private readonly IMapper _mapper;
        private readonly Fixture _fixture;

        private readonly IDbUnitOfWork _uow;
        private readonly ITaxBandRepository _taxBandRepository;

        public IncomeTaxServiceTests()
        {
            _mapper = Substitute.For<IMapper>();
            _fixture = new Fixture();

            _uow = Substitute.For<IDbUnitOfWork>();
            _taxBandRepository = Substitute.For<ITaxBandRepository>();
        }

        [Fact]
        public async Task GetAllTaxBandsAsync_Should_Calculate_Tax_Correctly()
        {
            // Arrange
            var sut = CreateSut();

            var taxBands = _fixture.CreateMany<TaxBand>(2);
            _taxBandRepository.GetAllAsync().Returns(taxBands);

            var mappedTaxBands = _fixture.CreateMany<TaxBandDomainModel>(2);
            _mapper.Map<IEnumerable<TaxBandDomainModel>>(taxBands).Returns(mappedTaxBands);

            // Act
            var actual = await sut.GetAllTaxBandsAsync();

            // Assert
            actual.Should().BeEquivalentTo(mappedTaxBands);
        }

        [Fact]
        public async Task CalculateTotalBandTaxAsync_Should_Throw_TaxBandOperationException_When_No_TaxBands()
        {
            // Arrange
            var sut = CreateSut();
            var grossAnnualSalary = 10_000m;

            _taxBandRepository.GetAllAsync().Returns(Enumerable.Empty<TaxBand>());

            // Act
            var act = async () => await sut.CalculateTotalBandTaxAsync(grossAnnualSalary);

            // Assert
            await act.Should().ThrowAsync<TaxBandOperationException>();
        }

        [Theory]
        [InlineData(10_000, 1_000)]
        [InlineData(40_000, 11_000)]
        public async Task CalculateTotalBandTaxAsync_Should_Calculate_Total_Correctly(
            decimal givenGrossAnnualSalary,
            decimal expectedTotalTax)
        {
            // Arrange
            var sut = CreateSut();

            var taxBands = _fixture.CreateMany<TaxBand>(2);
            _taxBandRepository.GetAllAsync().Returns(taxBands);

            var mappedTaxBands = new List<TaxBandDomainModel>
            {
                new()
                {
                    AnnualSalaryLowerLimit = 0,
                    AnnualSalaryUpperLimit = 5_000,
                    TaxRate = 0
                },
                new()
                {
                    AnnualSalaryLowerLimit = 5_000,
                    AnnualSalaryUpperLimit = 20_000,
                    TaxRate = 20
                },
                new()
                {
                    AnnualSalaryLowerLimit = 20_000,
                    AnnualSalaryUpperLimit = null,
                    TaxRate = 40
                }
            };
            _mapper.Map<IEnumerable<TaxBandDomainModel>>(taxBands).Returns(mappedTaxBands);

            // Act
            var actual = await sut.CalculateTotalBandTaxAsync(givenGrossAnnualSalary);

            // Assert
            actual.Should().Be(expectedTotalTax);
        }

        private TaxBandService CreateSut()
        {
            return new TaxBandService(_mapper, _uow, _taxBandRepository);
        }
    }
}
