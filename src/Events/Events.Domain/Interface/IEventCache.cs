using Events.Shared.Dto;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Domain.Interfaces
{
    public interface IEventCache
    {
        ConcurrentDictionary<string, IEnumerable<string>> GetDataCache();

        IEnumerable<string>? GetCategories(string eventId);

        Task WaitForCacheUpdateAsync(CancellationToken cancellationToken);
    }
}
