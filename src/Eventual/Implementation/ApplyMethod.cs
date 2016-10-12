using Eventual.Domain;
using Eventual.MessageContracts;
using System;
using System.Reflection;

namespace Eventual.Implementation
{
    public class MethodInfoApplyMethod : IApplyMethod
    {
        private readonly MethodInfo methodInfo;

        public MethodInfoApplyMethod(MethodInfo methodInfo)
        {
            this.methodInfo = methodInfo;
        }

        public Type AggregateRootType => methodInfo.GetParameters()[0].ParameterType;

        public Type EventType => methodInfo.GetParameters()[1].ParameterType;

        public IAggregateRoot Invoke(IAggregateRoot aggregateRoot, IPersistedDomainEvent domainEvent)
        {
            return (IAggregateRoot)methodInfo.Invoke(null, new object[] { aggregateRoot, domainEvent });
        }
    }

    public class DelegateApplyMethod : IApplyMethod
    {
        private readonly Delegate @delegate;

        public DelegateApplyMethod(Delegate @delegate)
        {
            this.@delegate = @delegate;
        }

        public Type AggregateRootType => @delegate.GetMethodInfo().GetParameters()[0].ParameterType;

        public Type EventType => @delegate.GetMethodInfo().GetParameters()[1].ParameterType;

        public IAggregateRoot Invoke(IAggregateRoot aggregateRoot, IPersistedDomainEvent domainEvent)
        {
            return (IAggregateRoot)@delegate.DynamicInvoke(aggregateRoot, domainEvent);
        }
    }
}