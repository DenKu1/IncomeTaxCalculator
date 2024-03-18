using IncomeTaxCalculator.Domain.DomainModels;
using IncomeTaxCalculator.Persistence.Repositories.Interfaces;

namespace IncomeTaxCalculator.Domain.Services;

public class TaxBandService : ITaxBandService
{
    public TaxBandService(ITaxBandRepository taxBandRepository)
    {
        TaxBandRepository = taxBandRepository;
    }

    public ITaxBandRepository TaxBandRepository { get; }

    public decimal CalculateTotalBandTax(decimal grossAnnualSalary)
    {
        return CalculateSalaryWithinTaxBand(taxBand, grossAnnualSalary) * taxBand.TaxRate / 100;
    }

    private decimal CalculateBandTax(TaxBandDomainModel taxBand, decimal grossAnnualSalary)
    {
        return CalculateSalaryWithinTaxBand(taxBand, grossAnnualSalary) * taxBand.TaxRate / 100;
    }

    private decimal CalculateSalaryWithinTaxBand(TaxBandDomainModel taxBand, decimal grossAnnualSalary)
    {
        if (!taxBand.AnnualSalaryUpperLimit.HasValue)
            return grossAnnualSalary - taxBand.AnnualSalaryLowerLimit;

        var taxBandRange = taxBand.AnnualSalaryUpperLimit.Value - taxBand.AnnualSalaryLowerLimit;

        return grossAnnualSalary - taxBandRange;
    }
}
