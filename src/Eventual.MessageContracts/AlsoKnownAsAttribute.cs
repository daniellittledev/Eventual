using System;

namespace Eventual.MessageContracts
{
    [AttributeUsage(AttributeTargets.Class)]
    public class AlsoKnownAsAttribute : Attribute
    {
        public string Name { get; }

        public AlsoKnownAsAttribute(string name)
        {
            Name = name;
        }
    }
}
