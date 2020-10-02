namespace SampleProject.Domain.ForeignExchange
{
    using System.Collections.Generic;

    public interface IForeignExchange
    {
        List<ConversionRate> GetConversionRates();
    }
}