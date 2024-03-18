using AutoMapper;
using IncomeTaxCalculator.Domain.DomainModels;
using IncomeTaxCalculator.Persistence.Repositories.Interfaces;

namespace IncomeTaxCalculator.Domain.Services;

public class TaxBandService : ITaxBandService
{
    private readonly IMapper _mapper;
    private readonly ITaxBandRepository _taxBandRepository;

    public TaxBandService(
        IMapper mapper, 
        ITaxBandRepository taxBandRepository)
    {
        _mapper = mapper;
        _taxBandRepository = taxBandRepository;
    }

    public async Task<decimal> CalculateTotalBandTaxAsync(decimal grossAnnualSalary)
    {
        var taxBands = await _taxBandRepository.GetAllAsync();

        var taxBandDomainModels = _mapper.Map<IEnumerable<TaxBandDomainModel>>(taxBands);

        return taxBandDomainModels.Sum(taxBand => CalculateBandTax(taxBand, grossAnnualSalary));
    }

    private static decimal CalculateBandTax(TaxBandDomainModel taxBand, decimal grossAnnualSalary)
    {
        return CalculateSalaryWithinTaxBand(taxBand, grossAnnualSalary) * taxBand.TaxRate / 100;
    }

    private static decimal CalculateSalaryWithinTaxBand(TaxBandDomainModel taxBand, decimal grossAnnualSalary)
    {
        if (taxBand.AnnualSalaryUpperLimit.HasValue && taxBand.AnnualSalaryUpperLimit.Value < grossAnnualSalary)
            return taxBand.AnnualSalaryUpperLimit.Value - taxBand.AnnualSalaryLowerLimit;

        return grossAnnualSalary - taxBand.AnnualSalaryLowerLimit;
    }
}
