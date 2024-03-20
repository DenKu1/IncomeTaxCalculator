using IncomeTaxCalculator.API.IntegrationTests.Utilities;
using IncomeTaxCalculator.API.ViewModels.Responses;
using System.Net;
using System.Net.Http.Json;

namespace IncomeTaxCalculator.API.IntegrationTests.EndpointTests;

public class TaxBandsIntergationTests
{
    [Fact]
    public async Task GetAllTaxBands_Should_Return_Three_Tax_Bands()
    {
        var application = new IncomeTaxCalculatorWebApplicationFactory();
        var client = application.CreateClient();

        // Arrange
        var requestUri = $"api/tax-bands";

        // Act
        var response = await client.GetAsync(requestUri);

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var resultViewModel = await response.Content.ReadFromJsonAsync<ResponseViewModel<IEnumerable<TaxBandResponseViewModel>>>();
        resultViewModel.Result.Should().HaveCount(3);
    }
}
