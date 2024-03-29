﻿using AutoMapper;
using IncomeTaxCalculator.API.ViewModels.Requests;
using IncomeTaxCalculator.API.ViewModels.Responses;
using IncomeTaxCalculator.Domain.DomainModels;

namespace IncomeTaxCalculator.API.Profiles;

public class TaxBandProfile : Profile
{
    public TaxBandProfile()
    {
        CreateMap<AddTaxBandRequestViewModel, TaxBandDomainModel>();
        CreateMap<TaxBandDomainModel, TaxBandResponseViewModel>();
    }
}
