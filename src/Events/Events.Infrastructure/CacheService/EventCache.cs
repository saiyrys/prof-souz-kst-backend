using Events.Shared.Dto;
using System.Collections.Concurrent;


namespace Events.Infrastructure.CacheService
{
    public static class EventCache
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

        public static ConcurrentDictionary<string, IEnumerable<string>> GetDataCache()
        {
            return _dictionary;
        }

        public static IEnumerable<string>? GetCategories(string eventId)
        {
            _dictionary.TryGetValue(eventId, out var categories);

            return categories;
        }

        public static ConcurrentDictionary<string, IEnumerable<string>> ReloadCache()
        {
            return new ConcurrentDictionary<string, IEnumerable<string>>();
        }

        public async static Task WaitForCacheUpdateAsync(CancellationToken cancellationToken)
        {
            while (!_dictionary.Any())
            {
                await Task.Delay(100, cancellationToken);
                Console.WriteLine("Ожидание данных...");
            }
        }

    }
}
