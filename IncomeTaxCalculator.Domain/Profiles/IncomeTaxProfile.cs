using AutoMapper;
using IncomeTaxCalculator.Domain.DomainModels;
using IncomeTaxCalculator.Persistence.Entities;

namespace IncomeTaxCalculator.Domain.Profiles;

public class TaxBandProfile : Profile
{
    public TaxBandProfile()
    {
        CreateMap<TaxBand, TaxBandDomainModel>();
    }
}
