using System.Reflection;

namespace Eventual.UnitTests
{
    public static class EventualIntegrationTestsAssembly
    {
        public static Assembly Assembly => typeof(EventualIntegrationTestsAssembly).GetTypeInfo().Assembly;
    }
}
