using Events.Shared.Dto;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Events.Infrastructure.CacheService
{
    public class EventCache
    {
        private static readonly ConcurrentDictionary<string, IEnumerable<string>> _dictionary = new();

        public void UpdateCache(EventDto eventDto)
        {
           /* ReloadCache();
*/
            _dictionary.AddOrUpdate(
                eventDto.eventId,
                eventDto.categories,
                (key, categories) => eventDto.categories
            );
        }

        public ConcurrentDictionary<string, IEnumerable<string>> GetDataCache()
        {
            return _dictionary;
        }

        public IEnumerable<string>? GetCategories(string eventId)
        {
            _dictionary.TryGetValue(eventId, out var categories);

            return categories;
        }

        public ConcurrentDictionary<string, IEnumerable<string>> ReloadCache()
        {
            return new ConcurrentDictionary<string, IEnumerable<string>>();
        }

        public async Task WaitForCacheUpdateAsync(CancellationToken cancellationToken)
        {
            while (_dictionary.Any())
            {
                await Task.Delay(100, cancellationToken);
                Console.WriteLine("Ожидание данных...");
            }
        }

    }
}
