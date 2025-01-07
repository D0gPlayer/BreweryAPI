using Microsoft.EntityFrameworkCore;
using System;
using System.Linq.Expressions;

namespace BreweryAPI.Data
{
     public class Repository<T> : IRepository<T> where T : class
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

            _DbContext.Remove(entity);
            var deleted = await _DbContext.SaveChangesAsync();
            return deleted > 0;
        }

        public async Task<bool> Update(Guid Id)
        {
            var entity = DbSet.Find(Id);
            if(entity is null) return false;

            _DbContext.Update(entity);
            var updated = await _DbContext.SaveChangesAsync();
            return updated > 0;

        }
    }
}
