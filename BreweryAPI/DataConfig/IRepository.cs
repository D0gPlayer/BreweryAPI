using BreweryAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BreweryAPI.Data
{
    public interface IRepository<T> where T : BaseEntity
    {

        public DbSet<T> DbSet { get; set; }
        public Task<bool> Add(T entity);
        public Task<bool> Update(T entity);
        public Task<bool> Delete(Guid Id);
    }
    
}
