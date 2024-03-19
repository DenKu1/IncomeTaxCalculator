using IncomeTaxCalculator.Domain.DomainModels;

namespace IncomeTaxCalculator.Domain.Services;

public interface ITaxBandService
{
    Task<IEnumerable<TaxBandDomainModel>> GetAllTaxBandsAsync();
    Task<decimal> CalculateTotalBandTaxAsync(decimal grossAnnualSalary);
    Task PushTaxBandAsync(TaxBandDomainModel taxBandDomainModel);
    Task PopTaxBandAsync();
}
