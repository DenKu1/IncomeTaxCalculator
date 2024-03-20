using IncomeTaxCalculator.Domain.DomainModels;
using IncomeTaxCalculator.Domain.Exceptions;
using IncomeTaxCalculator.Domain.Services;
using IncomeTaxCalculator.Persistence.Entities;
using IncomeTaxCalculator.Persistence.Repositories.Interfaces;

namespace IncomeTaxCalculator.Domain.Tests.Services;

public class IncomeTaxServiceTests
{
    private readonly IMapper _mapper;
    private readonly Fixture _fixture;

    private readonly IDbUnitOfWork _unitOfWork;
    private readonly ITaxBandRepository _taxBandRepository;

    public IncomeTaxServiceTests()
    {
        _mapper = Substitute.For<IMapper>();
        _fixture = new Fixture();

        _unitOfWork = Substitute.For<IDbUnitOfWork>();
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

    [Fact]
    public async Task PushTaxBandAsync_Should_Throw_TaxBandOperationException_When_First_TaxBand_LowerLimit_NotZero()
    {
        // Arrange
        var sut = CreateSut();
        var taxBandToAdd = _fixture
            .Build<TaxBandDomainModel>()
            .With(x => x.AnnualSalaryLowerLimit, 10_000)
            .Create();

        _taxBandRepository.GetSingleOrDefaultAsync(Arg.Any<Expression<Func<TaxBand, bool>>>()).Returns((TaxBand)null);

        // Act
        var act = async () => await sut.PushTaxBandAsync(taxBandToAdd);

        // Assert
        await act.Should().ThrowAsync<TaxBandOperationException>();
    }

    [Fact]
    public async Task PushTaxBandAsync_Should_Add_TaxBand_When_First_TaxBand_LowerLimit_Is_Zero()
    {
        // Arrange
        var sut = CreateSut();
        var taxBandToAdd = _fixture
            .Build<TaxBandDomainModel>()
            .With(x => x.AnnualSalaryLowerLimit, 0)
            .Create();

        _taxBandRepository.GetSingleOrDefaultAsync(Arg.Any<Expression<Func<TaxBand, bool>>>()).Returns((TaxBand)null);

        // Act
        await sut.PushTaxBandAsync(taxBandToAdd);

        // Assert
        await _taxBandRepository.ReceivedWithAnyArgs(1).AddAsync(Arg.Any<TaxBand>());
        await _unitOfWork.ReceivedWithAnyArgs(1).SaveChangesAsync();
    }

    [Fact]
    public async Task PushTaxBandAsync_Should_Throw_TaxBandOperationException_When_New_Lower_Limit_Lower_Than_Previous()
    {
        // Arrange
        var sut = CreateSut();
        var taxBandToAdd = _fixture
            .Build<TaxBandDomainModel>()
            .With(x => x.AnnualSalaryLowerLimit, 5_000)
            .Create();

        var previousTaxBand = _fixture
            .Build<TaxBand>()
            .With(x => x.AnnualSalaryLowerLimit, 10_000)
            .Create();

        _taxBandRepository.GetSingleOrDefaultAsync(Arg.Any<Expression<Func<TaxBand, bool>>>()).Returns(previousTaxBand);

        // Act
        var act = async () => await sut.PushTaxBandAsync(taxBandToAdd);

        // Assert
        await act.Should().ThrowAsync<TaxBandOperationException>();
    }

    [Fact]
    public async Task PushTaxBandAsync_Should_Add_TaxBand_And_Updated_Previous_Upper_Limit()
    {
        // Arrange
        var sut = CreateSut();
        var taxBandToAdd = _fixture
            .Build<TaxBandDomainModel>()
            .With(x => x.AnnualSalaryLowerLimit, 10_000)
            .Create();

        var previousTaxBand = _fixture
            .Build<TaxBand>()
            .With(x => x.AnnualSalaryLowerLimit, 5_000)
            .Create();

        _taxBandRepository.GetSingleOrDefaultAsync(Arg.Any<Expression<Func<TaxBand, bool>>>()).Returns(previousTaxBand);

        // Act
        await sut.PushTaxBandAsync(taxBandToAdd);

        // Assert
        await _taxBandRepository.ReceivedWithAnyArgs(1).AddAsync(Arg.Any<TaxBand>());
        await _unitOfWork.ReceivedWithAnyArgs(1).SaveChangesAsync();

        previousTaxBand.AnnualSalaryUpperLimit.Should().Be(taxBandToAdd.AnnualSalaryLowerLimit);
    }

    [Fact]
    public async Task PopTaxBandAsync_Should_Not_Remove_If_No_Tax_Bands()
    {
        // Arrange
        var sut = CreateSut();
        _taxBandRepository.GetSingleOrDefaultAsync(Arg.Any<Expression<Func<TaxBand, bool>>>()).Returns((TaxBand)null);

        // Act
        await sut.PopTaxBandAsync();

        // Assert
        _taxBandRepository.DidNotReceiveWithAnyArgs().Remove(Arg.Any<TaxBand>());
        await _unitOfWork.DidNotReceiveWithAnyArgs().SaveChangesAsync();
    }

    [Fact]
    public async Task PopTaxBandAsync_Should_Remove_Latest()
    {
        // Arrange
        var sut = CreateSut();

        var latestTaxBand = _fixture
            .Build<TaxBand>()
            .Without(x => x.AnnualSalaryUpperLimit)
            .Create();

        _taxBandRepository.GetSingleOrDefaultAsync(Arg.Any<Expression<Func<TaxBand, bool>>>())
            .Returns(latestTaxBand);

        // Act
        await sut.PopTaxBandAsync();

        // Assert
        _taxBandRepository.Received(1).Remove(latestTaxBand);
        await _unitOfWork.Received(1).SaveChangesAsync();
    }

    private TaxBandService CreateSut()
    {
        return new TaxBandService(_mapper, _unitOfWork, _taxBandRepository);
    }
}
