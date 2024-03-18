using FluentValidation;
using IncomeTaxCalculator.API.ViewModels.Requests;

namespace IncomeTaxCalculator.API.Validators
{
    public class AddTaxBandRequestViewModelValidator : AbstractValidator<AddTaxBandRequestViewModel>
    {
        public AddTaxBandRequestViewModelValidator()
        {
            RuleFor(x => x.AnnualSalaryLowerLimit)
                .GreaterThan(0)
                .WithMessage(x => $"'{nameof(x.AnnualSalaryLowerLimit)}': Value {x.AnnualSalaryLowerLimit} is incorrect. It should be greater than zero");

            RuleFor(x => x.AnnualSalaryUpperLimit)
                .GreaterThan(0)
                .WithMessage(x => $"'{nameof(x.AnnualSalaryUpperLimit)}': Value {x.AnnualSalaryUpperLimit} is incorrect. It should be greater than zero")
                .GreaterThan(x => x.AnnualSalaryLowerLimit)
                .WithMessage(x => $"'{nameof(x.AnnualSalaryUpperLimit)}': Value {x.AnnualSalaryUpperLimit} is incorrect. It should be greater than {nameof(x.AnnualSalaryLowerLimit)}");

            RuleFor(x => x.TaxRate)
                .GreaterThanOrEqualTo(0)
                .WithMessage(x => $"'{nameof(x.TaxRate)}': Value {x.TaxRate} is incorrect. It should be greater than zero")
                .LessThanOrEqualTo(100)
                .WithMessage(x => $"'{nameof(x.TaxRate)}': Value {x.TaxRate} is incorrect. It should be less or equal to 100");
        }
    }
}
