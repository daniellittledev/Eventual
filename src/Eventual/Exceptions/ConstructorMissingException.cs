using System;
using System.Linq;

namespace Eventual
{
    public class ConstructorMissingException : Exception
    {
        public Type[] ParamTypes { get; }

        public ConstructorMissingException(Type[] paramTypes)
        {
            ParamTypes = paramTypes;
        }

        public override string Message {
            get {
                return "Could not find a constuctor with the following signature. " + string.Join(", ", ParamTypes.Select(x => x.FullName));
            }
        }
    }
}