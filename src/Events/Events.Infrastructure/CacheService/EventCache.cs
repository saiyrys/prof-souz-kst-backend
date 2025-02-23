/*using Events.Domain.Interfaces;*/
using Events.Domain.Interfaces;
using Events.Shared.Dto;
using System.Collections.Concurrent;


namespace Events.Infrastructure.CacheService
{
    public class EventCache : IEventCache
    {
        private static readonly ConcurrentDictionary<string, IEnumerable<string>> _dictionary = new();

        public static void UpdateCache(EventDto eventDto)
        {
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
            var awaitTime = DateTime.UtcNow.AddSeconds(10);

            while (!_dictionary.Any())
            {
                if(DateTime.UtcNow > awaitTime)
                {
                    return;
                }
                await Task.Delay(600, cancellationToken);
                Console.WriteLine("Ожидание данных...");
            }
        }

    }
}
