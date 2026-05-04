using System;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.DependencyInjection;
using EDUBackEnd.Services.Auth;
using Xunit;

namespace EDUBackEnd.Tests.Services.Auth
{
    public class BruteForceProtectionServiceTests
    {
        private readonly IMemoryCache _cache;
        private readonly BruteForceProtectionService _service;

        public BruteForceProtectionServiceTests()
        {
            // Setup a real, isolated MemoryCache for every test instance
            var services = new ServiceCollection();
            services.AddMemoryCache();
            var serviceProvider = services.AddMemoryCache().BuildServiceProvider();

            _cache = serviceProvider.GetRequiredService<IMemoryCache>();
            _service = new BruteForceProtectionService(_cache);
        }

        [Theory]
        [InlineData("127.0.0.1")]
        [InlineData("192.168.1.1")]
        public void IsBlocked_NewIp_ReturnsFalse(string ip)
        {
            // Act
            var result = _service.IsBlocked(ip);

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void RegisterFailedAttempt_IncrementsCountCorrectly()
        {
            // Arrange
            string ip = "1.1.1.1";
            string cacheKey = $"FAIL_{ip}";

            // Act
            _service.RegisterFailedAttempt(ip);
            _service.RegisterFailedAttempt(ip);

            // Assert
            var count = _cache.Get<int?>(cacheKey);
            Assert.Equal(2, count);
        }

        [Fact]
        public void RegisterFailedAttempt_ThresholdReached_BlocksIpAndClearsFailCount()
        {
            // Arrange
            string ip = "2.2.2.2";
            string failKey = $"FAIL_{ip}";

            // Act: Perform 5 attempts
            for (int i = 0; i < 5; i++)
            {
                _service.RegisterFailedAttempt(ip);
            }

            // Assert
            Assert.True(_service.IsBlocked(ip));
            Assert.Null(_cache.Get(failKey)); // Should be removed from cache after block
        }

        [Fact]
        public void Reset_RemovesAllCacheEntriesForIp()
        {
            // Arrange
            string ip = "3.3.3.3";
            for (int i = 0; i < 5; i++) _service.RegisterFailedAttempt(ip);
            Assert.True(_service.IsBlocked(ip));

            // Act
            _service.Reset(ip);

            // Assert
            Assert.False(_service.IsBlocked(ip));
            Assert.Null(_cache.Get($"FAIL_{ip}"));
            Assert.Null(_cache.Get($"BLOCK_{ip}"));
        }

        [Fact]
        public void MultipleIps_TrackedIndependently()
        {
            // Arrange
            string hackerIp = "99.99.99.99";
            string userIp = "10.10.10.10";

            // Act
            for (int i = 0; i < 5; i++) _service.RegisterFailedAttempt(hackerIp);
            _service.RegisterFailedAttempt(userIp);

            // Assert
            Assert.True(_service.IsBlocked(hackerIp));
            Assert.False(_service.IsBlocked(userIp));

            var userFailCount = _cache.Get<int?>($"FAIL_{userIp}");
            Assert.Equal(1, userFailCount);
        }
    }
}