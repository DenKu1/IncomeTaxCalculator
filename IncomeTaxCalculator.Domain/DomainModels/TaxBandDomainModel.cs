namespace IncomeTaxCalculator.Domain.DomainModels;

public class TaxBandDomainModel
{
    public Guid Id { get; set; }
    public int? AnnualSalaryUpperLimit { get; set; }
    public int AnnualSalaryLowerLimit { get; set; }
    public int TaxRate { get; set; }
}
