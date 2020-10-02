namespace SampleProject.Domain.ForeignExchange
{
    using SharedKernel;

    public class ConversionRate
    {
        public ConversionRate(string sourceCurrency, string targetCurrency, decimal factor)
        {
            SourceCurrency = sourceCurrency;
            TargetCurrency = targetCurrency;
            Factor = factor;
        }

        public string SourceCurrency { get; }

        public string TargetCurrency { get; }

        public decimal Factor { get; }

        internal MoneyValue Convert(MoneyValue value)
        {
            return Factor * value;
        }
    }
}