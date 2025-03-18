using BreweryAPI.Data;
using BreweryAPI.Extensions;
using BreweryAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BreweryTests
{
    // A simple test entity for demonstration purposes.
    public class TestEntity : BaseEntity
    {
        public string Name { get; set; }
    }

    // A minimal DbContext for testing.
    public class TestDataContext : DataContext
    {
        public TestDataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        public DbSet<TestEntity> TestEntities { get; set; }
    }

    // A minimal repository implementation for TestEntity.
    public class TestRepository : Repository<TestEntity>
    {
        public TestRepository(TestDataContext context, IDistributedCache cache)
            : base(context, cache) { }
    }

    [TestClass]
    public class RepositoryCacheTests
    {
        private TestDataContext _context;
        private IDistributedCache _cache;
        private TestRepository _repository;

        [TestInitialize]
        public void Setup()
        {
            // Setup in-memory EF Core context.
            var options = new DbContextOptionsBuilder<DataContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;
            _context = new TestDataContext(options);

            // Setup in-memory distributed cache.
            var memoryCacheOptions = Options.Create(new MemoryDistributedCacheOptions());
            _cache = new MemoryDistributedCache(memoryCacheOptions);

            // Initialize repository.
            _repository = new TestRepository(_context, _cache);
        }

        [TestMethod]
        public async Task GetCached_ReturnsEntityFromCacheIfExists()
        {
            // Arrange: create and add an entity.
            var entity = new TestEntity { Id = Guid.NewGuid(), Name = "Test Entity" };
            await _repository.Add(entity);

            // Simulate that the entity is already cached.
            await _cache.SetRecordAsync<TestEntity>(entity.Id.ToString(), entity);

            // Act: get entity via GetCached.
            var result = await _repository.GetCached(entity.Id);

            // Assert.
            Assert.IsNotNull(result);
            Assert.AreEqual(entity.Id, result.Id);
            Assert.AreEqual("Test Entity", result.Name);
        }

        [TestMethod]
        public async Task GetCached_RetrievesEntityFromDbAndCachesIt()
        {
            // Arrange: create and add an entity.
            var entity = new TestEntity { Id = Guid.NewGuid(), Name = "Test Entity 2" };
            await _repository.Add(entity);

            // Ensure cache is empty.
            await _cache.RemoveAsync(entity.Id.ToString());

            // Act: first call will fetch from DB and update cache.
            var result = await _repository.GetCached(entity.Id);

            // Assert: entity is returned and then cached.
            Assert.IsNotNull(result);
            Assert.AreEqual(entity.Id, result.Id);
            var cached = await _cache.GetRecordAsync<TestEntity>(entity.Id.ToString());
            Assert.IsNotNull(cached);
            Assert.AreEqual(entity.Id, cached.Id);
        }

        [TestMethod]
        public async Task UpdateWithCache_UpdatesEntityAndCache()
        {
            // Arrange: add an entity.
            var entity = new TestEntity { Id = Guid.NewGuid(), Name = "Original Name" };
            await _repository.Add(entity);
            await _cache.SetRecordAsync<TestEntity>(entity.Id.ToString(), entity);

            // Act: update only the Name property.
            var updateDto = new TestEntity { Id = entity.Id, Name = "Updated Name" };
            var updateResult = await _repository.UpdateWithCache(updateDto);
            var result = await _repository.GetCached(entity.Id);

            // Assert.
            Assert.IsTrue(updateResult);
            Assert.IsNotNull(result);
            Assert.AreEqual("Updated Name", result.Name);
        }

        [TestMethod]
        public async Task DeleteWithCache_RemovesEntityFromDbAndCache()
        {
            // Arrange: add an entity.
            var entity = new TestEntity { Id = Guid.NewGuid(), Name = "Entity To Delete" };
            await _repository.Add(entity);
            await _cache.SetRecordAsync<TestEntity>(entity.Id.ToString(), entity);

            // Act: delete entity.
            var deleteResult = await _repository.DeleteWithCache(entity.Id);
            var cachedAfterDelete = await _cache.GetRecordAsync<TestEntity>(entity.Id.ToString());
            var dbEntity = await _repository.DbSet.FindAsync(entity.Id);

            // Assert.
            Assert.IsTrue(deleteResult);
            Assert.IsNull(cachedAfterDelete);
            Assert.IsNull(dbEntity);
        }
    }
}
