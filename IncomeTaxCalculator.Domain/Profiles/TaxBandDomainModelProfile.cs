using AutoMapper;
using IncomeTaxCalculator.Domain.DomainModels;
using IncomeTaxCalculator.Persistence.Entities;

namespace IncomeTaxCalculator.Domain.Profiles;

public class TaxBandDomainModelProfile : Profile
{
    public TaxBandDomainModelProfile()
    {
        CreateMap<TaxBand, TaxBandDomainModel>().ReverseMap();
    }
}
