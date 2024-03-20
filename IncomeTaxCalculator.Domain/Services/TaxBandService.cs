using AutoMapper;
using IncomeTaxCalculator.Domain.DomainModels;
using IncomeTaxCalculator.Domain.Exceptions;
using IncomeTaxCalculator.Persistence.Entities;
using IncomeTaxCalculator.Persistence.Repositories.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace IncomeTaxCalculator.Domain.Services;

public class TaxBandService : ITaxBandService
{
    private readonly IMapper _mapper;
    private readonly IDbUnitOfWork _unitOfWork;
    private readonly ITaxBandRepository _taxBandRepository;

    public TaxBandService(
        IMapper mapper,
        IDbUnitOfWork unitOfWork,
        ITaxBandRepository taxBandRepository)
    {
        _mapper = mapper;
        _unitOfWork = unitOfWork;
        _taxBandRepository = taxBandRepository;
    }

    public async Task<IEnumerable<TaxBandDomainModel>> GetAllTaxBandsAsync()
    {
        var taxBands = await _taxBandRepository.GetAllAsync();

        return _mapper.Map<IEnumerable<TaxBandDomainModel>>(taxBands);
    }

    public async Task<decimal> CalculateTotalBandTaxAsync(decimal grossAnnualSalary)
    {
        var taxBands = await _taxBandRepository.GetAllAsync();

        if (taxBands.IsNullOrEmpty())
            throw new TaxBandOperationException($"There is no tax bands added. Add at least one tax band");

        var taxBandDomainModels = _mapper.Map<IEnumerable<TaxBandDomainModel>>(taxBands);

        return taxBandDomainModels.Sum(taxBand => CalculateBandTax(taxBand, grossAnnualSalary));
    }

    public async Task PushTaxBandAsync(TaxBandDomainModel taxBandToAdd)
    {
        var currentTopTaxBand = await _taxBandRepository.GetSingleOrDefaultAsync(x => x.AnnualSalaryUpperLimit == null);

        if (currentTopTaxBand != null)
        {
            if (currentTopTaxBand.AnnualSalaryLowerLimit >= taxBandToAdd.AnnualSalaryLowerLimit)
                throw new TaxBandOperationException($"New lower limit {taxBandToAdd.AnnualSalaryLowerLimit} should be greater than previous {currentTopTaxBand.AnnualSalaryLowerLimit}");

            currentTopTaxBand.AnnualSalaryUpperLimit = taxBandToAdd.AnnualSalaryLowerLimit;
        }
        else
        {
            if (taxBandToAdd.AnnualSalaryLowerLimit != 0)
                throw new TaxBandOperationException($"New lower limit {taxBandToAdd.AnnualSalaryLowerLimit} should be zero for first tax band");
        }

        await _taxBandRepository.AddAsync(_mapper.Map<TaxBand>(taxBandToAdd));

        await _unitOfWork.SaveChangesAsync();
    }

    public async Task PopTaxBandAsync()
    {
        var currentTopTaxBand = await _taxBandRepository
            .GetSingleOrDefaultAsync(x => x.AnnualSalaryUpperLimit == null);

        if (currentTopTaxBand == null)
            return;

        var newTopTaxBand = await _taxBandRepository
            .GetSingleOrDefaultAsync(x => currentTopTaxBand.AnnualSalaryLowerLimit == x.AnnualSalaryUpperLimit);

        if (newTopTaxBand != null)
            newTopTaxBand.AnnualSalaryUpperLimit = null;

        _taxBandRepository.Remove(currentTopTaxBand);
        await _unitOfWork.SaveChangesAsync();
    }

    private static decimal CalculateBandTax(TaxBandDomainModel taxBand, decimal grossAnnualSalary)
    {
        return CalculateSalaryWithinTaxBand(taxBand, grossAnnualSalary) * taxBand.TaxRate / 100;
    }

    private static decimal CalculateSalaryWithinTaxBand(TaxBandDomainModel taxBand, decimal grossAnnualSalary)
    {
        if (grossAnnualSalary < taxBand.AnnualSalaryLowerLimit)
            return 0;

        if (!taxBand.AnnualSalaryUpperLimit.HasValue || grossAnnualSalary < taxBand.AnnualSalaryUpperLimit.Value)
            return grossAnnualSalary - taxBand.AnnualSalaryLowerLimit;

        return taxBand.AnnualSalaryUpperLimit.Value - taxBand.AnnualSalaryLowerLimit;
    }
}
