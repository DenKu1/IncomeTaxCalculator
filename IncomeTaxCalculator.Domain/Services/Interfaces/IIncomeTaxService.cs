using IncomeTaxCalculator.API.ViewModels.Responses;

namespace IncomeTaxCalculator.Domain.Services.Interfaces;

public interface IIncomeTaxService
{
    Task<CalculateTaxResultDomainModel> CalculateIncomeTaxAsync(decimal grossAnnualSalary);
}
