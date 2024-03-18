using AutoMapper;
using IncomeTaxCalculator.API.ViewModels.Responses;

namespace Print3dMarketplace.AuthAPI.Profiles;

public class IncomeTaxProfile : Profile
{
	public IncomeTaxProfile()
	{
		CreateMap<CalculateTaxResultDomainModel, CalculateTaxResponseViewModel>();
	}
}
