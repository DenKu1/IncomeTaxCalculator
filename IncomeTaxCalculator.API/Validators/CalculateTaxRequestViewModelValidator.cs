using FluentValidation;
using IncomeTaxCalculator.API.ViewModels.Requests;

namespace IncomeTaxCalculator.API.Validators
{
    public class CalculateTaxRequestViewModelValidator : AbstractValidator<CalculateIncomeTaxRequestViewModel>
    {
        public CalculateTaxRequestViewModelValidator()
        {
            RuleFor(x => x.GrossAnnualSalary)
                .GreaterThan(0)
                .WithMessage(x => $"'{nameof(x.GrossAnnualSalary)}': Value {x.GrossAnnualSalary} is incorrect. It should be greater than zero");
        }
    }
}
