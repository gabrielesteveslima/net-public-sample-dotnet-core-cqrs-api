namespace SampleProject.Infrastructure.Domain.ForeignExchanges
{
    using Caching;

    public class ConversionRatesCacheKey : ICacheKey<ConversionRatesCache>
    {
        public string CacheKey => "ConversionRatesCache";
    }
}