using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BreweryAPI.Data
{
    public interface IRepository<T> where T : class
    {

        public DbSet<T> DbSet { get; set; }
        public Task<bool> Add(T entity);
        public Task<bool> Update(Guid Id);
        public Task<bool> Delete(Guid Id);
    }
    
}
