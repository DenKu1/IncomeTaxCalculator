using AutoMapper;
using IncomeTaxCalculator.Domain.DomainModels;
using IncomeTaxCalculator.Persistence.Entities;

namespace Print3dMarketplace.AuthAPI.Profiles;

public class TaxBandProfile : Profile
{
	public TaxBandProfile()
	{
		CreateMap<TaxBand, TaxBandDomainModel>();
	}
}
