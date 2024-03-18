namespace IncomeTaxCalculator.API.ViewModels.Requests
{
    public class AddTaxBandRequestViewModel
    {
        public int? AnnualSalaryUpperLimit { get; set; }
        public int AnnualSalaryLowerLimit { get; set; }
        public int TaxRate { get; set; }
    }
}
