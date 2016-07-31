using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Eventual.Domain;

namespace Eventual.Implementation
{
    public class TypeDiscoveryService
    {
        public static DiscoveredTypes DiscoverTypes(params Assembly[] assemblies)
        {
            var extentionMethods = new List<MethodInfo>();

            foreach (var assembly in assemblies) 
            {
                foreach (var type in assembly.GetTypes())
                {
                    var typeInfo = type.GetTypeInfo();

                    // Get Extension methods
                    if (typeInfo.IsSealed && !typeInfo.IsGenericType && !type.IsNested)
                    {
                        var methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                        foreach (var method in methods)
                        {
                            if (method.IsDefined(typeof(ExtensionAttribute), false) 
                                && method.Name == "Apply"
                                && typeof(IAggregateRoot).IsAssignableFrom(method.GetParameters()[0].ParameterType))
                            {
                                extentionMethods.Add(method);
                            }
                        }
                    }
                }
            }

            return new DiscoveredTypes()
            {
                ApplyExtensionMethods = extentionMethods
            };
        }
    }
}
