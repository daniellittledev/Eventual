using System;
using System.Collections.Generic;
using System.Reflection;
using System.Runtime.CompilerServices;
using Eventual.Domain;
using Eventual.Implementation;

namespace Eventual.TypeDiscovery
{
    public class EventApplyLocator : IScanningLocator, IEventApplyContainer
    {
        private readonly List<IApplyMethod> extentionMethods = new List<IApplyMethod>();

        public IReadOnlyCollection<IApplyMethod> ApplyMethods => extentionMethods;

        public void Scan(Type type, TypeInfo typeInfo)
        {
            if (!typeInfo.IsSealed || typeInfo.IsGenericType || type.IsNested) return;

            var methods = type.GetMethods(BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic);
            foreach (var method in methods) {
                if (method.IsDefined(typeof(ExtensionAttribute), false)
                    && method.Name == "Apply"
                    && typeof(IAggregateRoot).IsAssignableFrom(method.GetParameters()[0].ParameterType)) {
                    extentionMethods.Add(new MethodInfoApplyMethod(method));
                }
            }
        }
    }
}