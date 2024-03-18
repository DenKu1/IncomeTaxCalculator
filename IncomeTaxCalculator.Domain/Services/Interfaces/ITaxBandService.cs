using IncomeTaxCalculator.Domain.DomainModels;

namespace IncomeTaxCalculator.Domain.Services;

public interface ITaxBandService
{
    public decimal CalculateBandTax(TaxBandDomainModel taxBand, decimal grossAnnualSalary);
}
