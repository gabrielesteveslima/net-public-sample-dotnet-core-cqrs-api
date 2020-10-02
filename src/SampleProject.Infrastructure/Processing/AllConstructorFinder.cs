namespace SampleProject.Infrastructure.Processing
{
    using System;
    using System.Collections.Concurrent;
    using System.Linq;
    using System.Reflection;
    using Autofac.Core.Activators.Reflection;

    internal class AllConstructorFinder : IConstructorFinder
    {
        private static readonly ConcurrentDictionary<Type, ConstructorInfo[]> Cache =
            new ConcurrentDictionary<Type, ConstructorInfo[]>();


        public ConstructorInfo[] FindConstructors(Type targetType)
        {
            ConstructorInfo[] result = Cache.GetOrAdd(targetType,
                t => t.GetTypeInfo().DeclaredConstructors.ToArray());

            return result.Length > 0 ? result : throw new NoConstructorsFoundException(targetType);
        }
    }
}