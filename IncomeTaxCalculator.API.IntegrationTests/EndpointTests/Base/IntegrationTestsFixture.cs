using IncomeTaxCalculator.API.IntegrationTests.Utilities;

namespace IncomeTaxCalculator.API.IntegrationTests.EndpointTests.Base;

public class IntegrationTestsFixture
{
    public HttpClient Client;
    public IncomeTaxCalculatorWebApplicationFactory Factory;

    public IntegrationTestsFixture()
    {
        Factory = new IncomeTaxCalculatorWebApplicationFactory();
        Client = Factory.CreateClient();
    }
}
