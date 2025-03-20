using BreweryAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BreweryAPI.Data
{
    public interface IRepository<T> where T : BaseEntity
    {

        public DbSet<T> DbSet { get; set; }

        public Task<T?> Get(Guid Id);
        public Task<T?> GetCached(Guid Id);
        public Task<IList<T>> GetAll();
        public Task<IList<T>> GetAllCached(string cacheKey);
        public Task<IList<T>> GetFiltered(Dictionary<string, string> filters);
        public Task<IList<T>> GetFilteredCached(string cacheKey, Dictionary<string, string> filters);
        public Task<bool> Add(T entity);
        public Task<bool> Update(T entity);
        public Task<bool> UpdateWithCache(T entity);
        public Task<bool> Delete(Guid Id);
        public Task<bool> DeleteWithCache(Guid Id);
    }
    
}
