using BreweryAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BreweryAPI.Data
{
    public interface IRepository<T> where T : BaseEntity
    {

        public DbSet<T> DbSet { get; set; }
        public Task<T> GetCached(Guid Id);
        public Task<bool> Add(T entity);
        public Task<bool> Update(T entity);
        public Task<bool> UpdateWithCache(T entity);
        public Task<bool> Delete(Guid Id);
        public Task<bool> DeleteWithCache(Guid Id);
    }
    
}
