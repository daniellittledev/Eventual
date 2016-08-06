using System.Reflection;

namespace Eventual.UnitTests
{
    public static class EventualUnitTestsAssembly
    {
        public static Assembly Assembly => typeof(EventualUnitTestsAssembly).GetTypeInfo().Assembly;
    }
}
