using AutoMapper;
using BreweryAPI.Extensions;
using BreweryAPI.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System;
using System.Linq.Expressions;

namespace BreweryAPI.Data
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly DataContext _DbContext;
        private readonly IDistributedCache _cache;

        public DbSet<T> DbSet { get; set; }

        public Repository(DataContext dataContext, IDistributedCache cache)
        {
            _DbContext = dataContext;
            _cache = cache;
            DbSet = dataContext.Set<T>();
        }

        public async Task<T> GetCached(Guid Id)
        {
            var cachedRecord = await _cache.GetRecordAsync<T>(Id.ToString());
            if (cachedRecord is not null)
                return cachedRecord;

            var entity = await DbSet.FindAsync(Id);
            if (entity is null)
                return null;

            await _cache.SetRecordAsync<T>(Id.ToString(), entity);
            return entity;
        }

        public async Task<bool> Add(T entity)
        {
            await _DbContext.AddAsync(entity);
            var saved = await _DbContext.SaveChangesAsync();
            return saved > 0;
        }

        public async Task<bool> Update(T entity)
        {
            var entityToUpdate = await DbSet.FirstOrDefaultAsync(e => e.Id == entity.Id);
            if (entityToUpdate == null)
                return false;

            foreach (var prop in typeof(T).GetProperties())
            {
                if (!prop.CanWrite || prop.Name == "Id")
                    continue;

                var newValue = prop.GetValue(entity);
                if (newValue != null)
                {
                    prop.SetValue(entityToUpdate, newValue);
                    _DbContext.Entry(entityToUpdate).Property(prop.Name).IsModified = true;
                }
            }

            var updated = await _DbContext.SaveChangesAsync();
            return updated > 0;

        }

        public async Task<bool> UpdateWithCache(T entity)
        {
            var entityToUpdate = await DbSet.FirstOrDefaultAsync(e => e.Id == entity.Id);
            if (entityToUpdate == null)
                return false;

            foreach (var prop in typeof(T).GetProperties())
            {
                if (!prop.CanWrite || prop.Name == "Id")
                    continue;

                var newValue = prop.GetValue(entity);
                if (newValue != null)
                {
                    prop.SetValue(entityToUpdate, newValue);
                    _DbContext.Entry(entityToUpdate).Property(prop.Name).IsModified = true;
                }
            }

            await _cache.SetRecordAsync<T>(entity.Id.ToString(), entityToUpdate);

            var updated = await _DbContext.SaveChangesAsync();
            return updated > 0;
        }

        public async Task<bool> Delete(Guid Id)
        {
            var entity = await DbSet.FindAsync(Id);
            if (entity is null) return false;

            DbSet.Remove(entity);
            var deleted = await _DbContext.SaveChangesAsync();
            return deleted > 0;
        }

        public async Task<bool> DeleteWithCache(Guid Id)
        {
            var entity = await DbSet.FindAsync(Id);
            if (entity is null) return false;

            await _cache.RemoveAsync(entity.Id.ToString());

            DbSet.Remove(entity);
            var deleted = await _DbContext.SaveChangesAsync();
            return deleted > 0;
        }
    }
}
