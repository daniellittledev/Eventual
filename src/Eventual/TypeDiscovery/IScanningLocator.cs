using System;
using System.Reflection;

namespace Eventual.TypeDiscovery
{
    public interface IScanningLocator
    {
        void Scan(Type type, TypeInfo typeInfo);
    }
}