using IncomeTaxCalculator.Persistence.Entities.Abstract;

namespace IncomeTaxCalculator.Persistence.Entities
{
    public class TaxBand : BaseEntity
    {
        public int? AnnualSalaryUpperLimit { get; set; }
        public int AnnualSalaryLowerLimit { get; set; }
        public int TaxRate { get; set; }
    }
}
