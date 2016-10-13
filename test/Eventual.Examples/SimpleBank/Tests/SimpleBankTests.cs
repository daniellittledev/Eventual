using Autofac;
using Enexure.MicroBus;
using Eventual.Examples.SimpleBank.Domain.Commands;
using System;
using System.Threading.Tasks;
using Xunit;

namespace Eventual.Examples.SimpleBank.Tests
{
    public class SimpleBankTests
    {
        // Concurrency

        // Load Test

        // Validation Test

        // Order of events for a read model

        // The event is already created, then version number can be bumped

        [Fact]
        public async Task EndToEnd()
        {
            using (var scope = TestHelper.GetSystem()) {
                var bus = scope.Resolve<IMicroBus>();

                var accountId = Guid.NewGuid();

                await bus.SendAsync(new OpenAccountCommand(accountId));

                await bus.SendAsync(new DepositCommand(accountId, 1000));

                await bus.SendAsync(new WithdrawCommand(accountId, 1100));

                await bus.SendAsync(new WithdrawCommand(accountId, 900));
            }

        }
    }
}
