using Microsoft.Extensions.Caching.Memory;

namespace EDUBackEnd.Services.Auth
{
    public class BruteForceProtectionService
    {
        private readonly IMemoryCache _cache;
        public BruteForceProtectionService(IMemoryCache cache)
        {
            _cache = cache;
        }
        public bool IsBlocked(string ip)
            => _cache.TryGetValue($"BLOCK_{ip}", out _);

        public void RegisterFailedAttempt(string ip)
        {
            var key = $"FAIL_{ip}";
            var count = _cache.Get<int?>(key) ?? 0;
            count++;

            if(count >= 5)
            {
                _cache.Set($"BLOCK_{ip}", true, TimeSpan.FromMinutes(10)); 
                _cache.Remove(key);
            }
            else
            {
                 _cache.Set(key, count, TimeSpan.FromMinutes(10));
            }
        }

        public void Reset(string ip)
        {
            _cache.Remove($"FAIL_{ip}");
            _cache.Remove($"BLOCK_{ip}");
        }
    }
}
