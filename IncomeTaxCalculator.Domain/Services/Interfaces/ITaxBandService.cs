using IncomeTaxCalculator.Domain.DomainModels;

namespace IncomeTaxCalculator.Domain.Services;

public interface ITaxBandService
{
    Task<decimal> CalculateTotalBandTaxAsync(decimal grossAnnualSalary);
    Task AddTaxBandAsync(TaxBandDomainModel taxBandDomainModel);
    void DeleteTaxBand(Guid id);
}
