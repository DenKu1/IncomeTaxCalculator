using AutoMapper;
using IncomeTaxCalculator.API.ViewModels.Responses;

namespace IncomeTaxCalculator.API.Profiles;

public class IncomeTaxProfile : Profile
{
    public IncomeTaxProfile()
    {
        CreateMap<CalculateTaxResultDomainModel, CalculateTaxResponseViewModel>();
    }
}
