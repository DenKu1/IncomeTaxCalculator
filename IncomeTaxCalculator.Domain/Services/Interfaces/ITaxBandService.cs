namespace IncomeTaxCalculator.Domain.Services;

public interface ITaxBandService
{
    Task<decimal> CalculateTotalBandTaxAsync(decimal grossAnnualSalary);
}
