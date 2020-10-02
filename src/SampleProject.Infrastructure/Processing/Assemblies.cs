namespace SampleProject.Infrastructure.Processing
{
    using System.Reflection;
    using Application.Orders.PlaceCustomerOrder;

    internal static class Assemblies
    {
        public static readonly Assembly Application = typeof(PlaceCustomerOrderCommand).Assembly;
    }
}