using AutoMapper;
using BreweryAPI.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;

namespace BreweryAPI.Data
{
     public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        private readonly DataContext _DbContext;
        public Repository(DataContext dataContext)
        {
            _DbContext = dataContext;
            DbSet = dataContext.Set<T>();
        }

        public DbSet<T> DbSet { get; set; }

        public async Task<bool> Add(T entity)
        {
            await _DbContext.AddAsync(entity);
            var saved = await _DbContext.SaveChangesAsync();
            return saved > 0;
        }

        public async Task<bool> Delete(Guid Id)
        {
            var entity = DbSet.Find(Id);
            if(entity is null) return false;

            DbSet.Remove(entity);
            var deleted = await _DbContext.SaveChangesAsync();
            return deleted > 0;
        }

        public async Task<bool> Update(T entity)
        {
            var entityToUpdate = DbSet.Find(entity.Id);
            if(entityToUpdate is null) return false;

            _DbContext.Entry(entityToUpdate).CurrentValues.SetValues(entity);
            var updated = await _DbContext.SaveChangesAsync();
            return updated > 0;

        }
    }
}
