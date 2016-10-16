using System.Collections.Generic;
using System.Reflection;

namespace Eventual.TypeDiscovery
{
    public static class TypeDiscoveryService
    {
        public static void DiscoverTypes(IReadOnlyCollection<IScanningLocator> locators, params Assembly[] assemblies)
        {
            foreach (var assembly in assemblies) 
            {
                foreach (var type in assembly.GetTypes()) {
                    var typeInfo = type.GetTypeInfo();

                    foreach (var scanningLocator in locators)
                    {
                        scanningLocator.Scan(type, typeInfo);
                    }
                }
            }
        }
    }
}
