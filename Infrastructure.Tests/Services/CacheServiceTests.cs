using Core.Entities;
using Infrastructure.Services;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;

namespace Infrastructure.Tests.Services
{
    public sealed class CacheServiceTests
    {
        private readonly IDistributedCache _cache;
        private readonly CacheService _cacheService;

        public CacheServiceTests()
        {
            _cache = new MemoryDistributedCache(Options.Create(new MemoryDistributedCacheOptions()));
            _cacheService = new(_cache);
        }

        [Fact]
        public async Task SetAsync_WhenCalled_NotThrowExceptionAsync()
        {
            //Arrange
            Category category = new()
            {
                CategoryId = 1,
                Name = "Test",
            };

            //Act
            Func<Task> action = async () => await _cacheService.SetAsync("test", category);

            //Assert
            await action.Should().NotThrowAsync();
        }

        [Fact]
        public async Task GetAsync_WhenCache_ReturnObject()
        {
            //Arrange
            Category category = new()
            {
                CategoryId = 1,
                Name = "Test",
            };

            //Act
            await _cacheService.SetAsync("test", category);
            var result = await _cacheService.GetAsync<Category>("test");

            //Assert
            result.Should().BeOfType<Category>();
            result.Should().BeEquivalentTo(category);
        }

        [Fact]
        public async Task GetAsync_WhenNoCache_ReturnNull()
        {
            //Arrange

            //Act
            var result = await _cacheService.GetAsync<Category>("test");

            //Assert
            result.Should().BeNull();
        }

        [Fact]
        public async Task RemoveAsync_WhenCalled_RemoveObject()
        {
            //Arrange
            Category category = new()
            {
                CategoryId = 1,
                Name = "Test",
            };

            //Act
            await _cacheService.SetAsync("test", category);
            var result = await _cacheService.GetAsync<Category>("test");
            await _cacheService.RemoveAsync("test");
            var result2 = await _cacheService.GetAsync<Category>("test");

            //Assert
            result.Should().BeOfType<Category>();
            result.Should().BeEquivalentTo(category);
            result2.Should().BeNull();
        }
    }
}
