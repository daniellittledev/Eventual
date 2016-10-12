using System.Reflection;

namespace Eventual.IntegrationTests
{
    public static class EventualIntegrationTestsAssembly
    {
        public static Assembly Assembly => typeof(EventualIntegrationTestsAssembly).GetTypeInfo().Assembly;
    }
}
