namespace SampleProject.Infrastructure.Domain.ForeignExchanges
{
    using System.Collections.Generic;
    using SampleProject.Domain.ForeignExchange;

    public class ConversionRatesCache
    {
        public ConversionRatesCache(List<ConversionRate> rates)
        {
            Rates = rates;
        }

        public List<ConversionRate> Rates { get; }
    }
}