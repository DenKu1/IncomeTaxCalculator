using FluentValidation.TestHelper;
using IncomeTaxCalculator.API.Validators;
using IncomeTaxCalculator.API.ViewModels.Requests;

namespace IncomeTaxCalculator.API.Tests.Validators;

public class CalculateTaxRequestViewModelValidatorTests
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

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    [InlineData(int.MinValue)]
    public void Should_Have_Error_When_GrossAnnualSalary_Is_Less_Or_Equeals_To_Zero(int grossAnnualSalary)
    {
        // Arrange
        var sut = CreateSut();
        var model = GetValidModel();
        model.GrossAnnualSalary = grossAnnualSalary;

        // Act
        var result = sut.TestValidate(model);

        // Assert
        result.Errors.Count.Should().Be(1);
        result.ShouldHaveValidationErrorFor(model => model.GrossAnnualSalary);
    }

    private CalculateIncomeTaxRequestViewModel GetValidModel()
    {
        return new CalculateIncomeTaxRequestViewModel
        {
            GrossAnnualSalary = 1_000
        };
    }

    private CalculateTaxRequestViewModelValidator CreateSut()
    {
        return new CalculateTaxRequestViewModelValidator();
    }
}
