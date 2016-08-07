using Eventual.Domain;
using Eventual.Implementation;
using Eventual.UnitTests.TestDomain;
using FluentAssertions;
using System;
using System.Collections.Generic;
using Xunit;

namespace Eventual.UnitTests.EventApplicatorTests
{
    public class ApplyEventTests
    {
        [Fact]
        public void ApplyMethodMustBeCalled()
        {
            var calls = new List<string>();

            var action = new Func<IAggregateRoot, SampleEvent, IAggregateRoot>(
                (aggregate, @event) => {
                    calls.Add("Apply");
                    return aggregate;
                });

            var applicator = new EventApplicator(new EventApplyMethods() {
                new DelegateApplyMethod(action)
            });

            var model = new DomainObject(Guid.Empty, 0);
            applicator.ApplyEvent(model, new SampleEvent());

            calls.Count.Should().Be(1);
        }

        [Fact]
        public void ApplyEventMustThrowExceptionIfThereIsNoMatchingApplyMethod()
        {
            var applicator = new EventApplicator(new EventApplyMethods());

            var model = new DomainObject(Guid.Empty, 0);

            applicator
                .Invoking(x => x.ApplyEvent(model, new SampleEvent()))
                .ShouldThrow<ApplyMethodNotFoundException>();
        }
    }
}
