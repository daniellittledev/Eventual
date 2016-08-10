using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Eventual.Domain;
using System;
using Eventual.MessageContracts;
using System.Linq;

namespace Eventual.Implementation
{
    public class TypeDiscoveryService
    {
        public static DiscoveredTypes DiscoverTypes(params Assembly[] assemblies)
        {
            var extentionMethods = new List<MethodInfo>();
            var domainEvents = new List<Type>();

            foreach (var assembly in assemblies) 
            {
                foreach (var type in assembly.GetTypes()) {
                    var typeInfo = type.GetTypeInfo();

                    GetApplyExtensionMethods(extentionMethods, type, typeInfo);
                    GetPersistedDomainEvents(domainEvents, type, typeInfo);
                }
            }

            return new DiscoveredTypes()
            {
                ApplyExtensionMethods = extentionMethods.Select(x => new MethodInfoApplyMethod(x)).ToArray(),
                PersistedDomainEventTypes = domainEvents
            };
        }

        private static void GetPersistedDomainEvents(List<Type> domainEvents, Type type, TypeInfo typeInfo)
        {
            if (typeof(IPersistedDomainEvent).IsAssignableFrom(type)) {
                domainEvents.Add(type);
            }
        }

        private static void GetApplyExtensionMethods(List<MethodInfo> extentionMethods, System.Type type, TypeInfo typeInfo)
        {
            if (typeInfo.IsSealed && !typeInfo.IsGenericType && !type.IsNested) {
                var methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
                foreach (var method in methods) {
                    if (method.IsDefined(typeof(ExtensionAttribute), false)
                        && method.Name == "Apply"
                        && typeof(IAggregateRoot).IsAssignableFrom(method.GetParameters()[0].ParameterType)) {
                        extentionMethods.Add(method);
                    }
                }
            }
        }
    }
}
