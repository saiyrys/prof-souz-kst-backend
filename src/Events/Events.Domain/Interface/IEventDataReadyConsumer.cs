using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Domain.Interface
{
    public interface IEventDataReadyConsumer
    {
        Task<bool> WaitEventConfirmation(CancellationToken cancellation);

        Task<bool> WaitNotificationForEventWasDeleted(CancellationToken cancellation);
    }
}
