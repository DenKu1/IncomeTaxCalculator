using IncomeTaxCalculator.API.ViewModels.Responses;
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

    public async Task<CalculateTaxResultDomainModel> CalculateIncomeTaxAsync(decimal grossAnnualSalary)
    {
        var totalAnnualTaxPaid = await _taxBandService.CalculateTotalBandTaxAsync(grossAnnualSalary);
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
}
