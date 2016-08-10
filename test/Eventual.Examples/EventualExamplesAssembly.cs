using System.Reflection;

namespace Eventual.Examples
{
    public static class EventualExamplesAssembly
    {
        public static Assembly Assembly => typeof(EventualExamplesAssembly).GetTypeInfo().Assembly;
    }
}
