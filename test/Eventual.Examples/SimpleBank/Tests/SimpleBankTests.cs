using Autofac;
using Enexure.MicroBus;
using Eventual.Examples.SimpleBank.Domain.Commands;
using System;
using System.Threading.Tasks;

namespace Eventual.Examples.SimpleBank.Tests
{
    public class SimpleBankTests
    {
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
