using IncomeTaxCalculator.API.ViewModels.Responses;

namespace IncomeTaxCalculator.Domain.Services.Interfaces;

public interface IIncomeTaxService
{
    public CalculateTaxResultDomainModel CalculateIncomeTax(decimal grossAnnualSalary);
}
