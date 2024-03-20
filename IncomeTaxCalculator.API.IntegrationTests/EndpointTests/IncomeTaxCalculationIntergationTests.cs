using IncomeTaxCalculator.API.IntegrationTests.EndpointTests.Base;
using IncomeTaxCalculator.API.IntegrationTests.Utilities;
using IncomeTaxCalculator.API.ViewModels.Requests;
using IncomeTaxCalculator.API.ViewModels.Responses;
using System.Net;

namespace IncomeTaxCalculator.API.IntegrationTests.EndpointTests;

public class IncomeTaxCalculationIntergationTests : IClassFixture<IntegrationTestsFixture>
{
    IntegrationTestsFixture _fixture;

    public IncomeTaxCalculationIntergationTests(IntegrationTestsFixture fixture)
    {
        this._fixture = fixture;
    }

    [Fact]
    public async Task CalculateIncomeTax_Should_Return_Valid_CalculateIncomeTaxResponseViewModel()
    {
        // Arrange
        var request = new CalculateIncomeTaxRequestViewModel
        {
            GrossAnnualSalary = 40_000m
        };

        var requestUri = $"api/taxes/income-tax/calculate";

        // Act
        var response = await _fixture.Client.PostAsync(requestUri, request.Serealize());

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var result = await response.DeserializeAsync<ResponseViewModel<CalculateIncomeTaxResponseViewModel>>();

    }
}
