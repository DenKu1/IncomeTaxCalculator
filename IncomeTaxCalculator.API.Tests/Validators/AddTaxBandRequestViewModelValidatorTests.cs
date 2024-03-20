using FluentValidation.TestHelper;
using IncomeTaxCalculator.API.Validators;
using IncomeTaxCalculator.API.ViewModels.Requests;

namespace IncomeTaxCalculator.API.Tests.Validators;

public class AddTaxBandRequestViewModelValidatorTests
{
    [Fact]
    public void Should_Have_No_Errors_When_Valid_Model()
    {
        // Arrange
        var sut = CreateSut();
        var model = GetValidModel();

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result.Errors.Should().BeEmpty();
    }

    [Fact]
    public void Should_Have_Error_When_AnnualSalaryLowerLimit_Is_Less_Than_Zero()
    {
        // Arrange
        var sut = CreateSut();
        var model = GetValidModel();
        model.AnnualSalaryLowerLimit = -1;

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result.Errors.Count.Should().Be(1);
        result.ShouldHaveValidationErrorFor(model => model.AnnualSalaryLowerLimit);
    }

    [Fact]
    public void Should_Have_Error_When_TaxRate_Is_Less_Than_Zero()
    {
        // Arrange
        var sut = CreateSut();
        var model = GetValidModel();
        model.TaxRate = -1;

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result.Errors.Count.Should().Be(1);
        result.ShouldHaveValidationErrorFor(model => model.TaxRate);
    }

    [Fact]
    public void Should_Have_Error_When_TaxRate_Is_More_Than_Hundred()
    {
        // Arrange
        var sut = CreateSut();
        var model = GetValidModel();
        model.TaxRate = 101;

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result.Errors.Count.Should().Be(1);
        result.ShouldHaveValidationErrorFor(model => model.TaxRate);
    }

    private AddTaxBandRequestViewModel GetValidModel()
    {
        return new AddTaxBandRequestViewModel
        {
            AnnualSalaryLowerLimit = 1_000,
            TaxRate = 25
        };
    }

    private AddTaxBandRequestViewModelValidator CreateSut()
    {
        return new AddTaxBandRequestViewModelValidator();
    }
}
