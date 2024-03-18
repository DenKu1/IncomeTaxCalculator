using IncomeTaxCalculator.API.ViewModels.Responses;
using IncomeTaxCalculator.Domain.DomainModels;
using IncomeTaxCalculator.Domain.Services.Interfaces;

namespace IncomeTaxCalculator.Domain.Services;

public class IncomeTaxService : IIncomeTaxService
{
    private readonly ITaxBandService _taxBandService;

    public IncomeTaxService(
        ITaxBandService taxBandService)
    {
        _taxBandService = taxBandService;
    }

    public CalculateTaxResultDomainModel CalculateIncomeTax(decimal grossAnnualSalary)
    {
        var totalAnnualTaxPaid = CalculateTotalAnnualTaxPaid(taxBands, grossAnnualSalary);
        var netAnnualSalary = CalculateNetAnnualSalary(grossAnnualSalary, totalAnnualTaxPaid);

        return new CalculateTaxResultDomainModel
        {
            GrossAnnualSalary = grossAnnualSalary,
            GrossMonthlySalary = grossAnnualSalary / 12,
            NetAnnualSalary = netAnnualSalary,
            NetMonthlySalary = netAnnualSalary / 12,
            AnnualTaxPaid = totalAnnualTaxPaid,
            MonthlyTaxPaid = totalAnnualTaxPaid / 12
        };
    }

    private decimal CalculateNetAnnualSalary(decimal grossAnnualSalary, decimal totalAnnualTaxPaid)
    {
        return grossAnnualSalary - totalAnnualTaxPaid;
    }

    private decimal CalculateTotalAnnualTaxPaid(IEnumerable<TaxBandDomainModel> taxBands, decimal grossAnnualSalary)
    {
        return taxBands.Sum(taxBand => _taxBandService.CalculateBandTax(taxBand, grossAnnualSalary));
    }
}
