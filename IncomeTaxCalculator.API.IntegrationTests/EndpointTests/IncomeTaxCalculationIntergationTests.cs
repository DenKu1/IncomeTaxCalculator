using IncomeTaxCalculator.API.IntegrationTests.Utilities;
using IncomeTaxCalculator.API.ViewModels.Requests;
using IncomeTaxCalculator.API.ViewModels.Responses;
using System.Net;
using System.Net.Http.Json;

namespace IncomeTaxCalculator.API.IntegrationTests.EndpointTests;

public class IncomeTaxCalculationIntergationTests
{
    [Fact]
    public async Task CalculateIncomeTax_Should_Return_Valid_CalculateIncomeTaxResponseViewModel()
    {
        var application = new IncomeTaxCalculatorWebApplicationFactory();
        var client = application.CreateClient();

        // Arrange
        var request = new CalculateIncomeTaxRequestViewModel
        {
            GrossAnnualSalary = 40_000m
        };

        var expected = new CalculateIncomeTaxResponseViewModel
        {
            GrossAnnualSalary = 40_000m,
            GrossMonthlySalary = 40_000m / 12,
            NetAnnualSalary = 29_000m,
            NetMonthlySalary = 29_000m / 12,
            AnnualTaxPaid = 11_000m,
            MonthlyTaxPaid = 11_000m / 12
        };

        var requestUri = $"api/taxes/income-tax/calculate";

        // Act
        var response = await client.PostAsJsonAsync(requestUri, request);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var resultViewModel = await response.Content.ReadFromJsonAsync<ResponseViewModel<CalculateIncomeTaxResponseViewModel>>();
        resultViewModel.Result.Should().BeEquivalentTo(expected);
    }
}
