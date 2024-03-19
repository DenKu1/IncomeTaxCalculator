namespace IncomeTaxCalculator.Domain.Exceptions
{
    public class TaxBandOperationException : Exception
    {
        public TaxBandOperationException()
        {
        }

        public TaxBandOperationException(string message)
            : base(message)
        {
        }
    }
}
