using AutoMapper;
using BreweryAPI.Data;
using BreweryAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace BreweryAPI.Services
{
    public abstract class BaseCRUDService<T, TDto> where T : BaseEntity
    {
        private readonly IRepository<T> _repository;
        private readonly IMapper _mapper;
        //private readonly ILogger _logger;

        protected BaseCRUDService(IRepository<T> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<bool> Add(TDto dto)
        {
            var entity = _mapper.Map<TDto, T>(dto);
            return await _repository.Add(entity);
        }

        public async Task<bool> Delete(Guid id)
        {
            return await _repository.Delete(id);
        }

        public async Task<T?> Get(Guid id)
        {
            return await _repository.DbSet.FindAsync(id);
        }

        public async Task<IList<T>> GetAll()
        {
            return await _repository.DbSet.AsNoTracking().ToListAsync();
        }

        public async Task<IList<T>> GetFiltered(Dictionary<string, string> queryFilters)
        {
            var query = _repository.DbSet.AsNoTracking().AsQueryable();
            try
            {
                foreach (var filter in queryFilters) 
                {
                    
                    var property = typeof(T).GetProperty(filter.Key);
                    if (property == null) throw new InvalidDataException($"{filter.Key} member doesnt exist in {typeof(T)}");

                    object value = null;

                    // Handle specific types like Guid, DateTime, etc.
                    if (property.PropertyType == typeof(Guid))
                    {
                        if (Guid.TryParse(filter.Value, out var guidValue))
                        {
                            value = guidValue;
                        }
                        else
                        {
                            throw new InvalidCastException($"Invalid Guid format for property {filter.Key}");
                        }
                    }
                    else if (property.PropertyType == typeof(DateTime))
                    {
                        if (DateTime.TryParse(filter.Value, out var dateTimeValue))
                        {
                            value = dateTimeValue;
                        }
                        else
                        {
                            throw new InvalidCastException($"Invalid DateTime format for property {filter.Key}");
                        }
                    }
                    else
                    {
                        // For other types, use Convert.ChangeType
                        value = Convert.ChangeType(filter.Value, property.PropertyType);
                    }

                    // Build the expression tree
                    var row = Expression.Parameter(typeof(T), "row");
                    var filterEx = Expression.Constant(value, property.PropertyType); // Ensure constant has correct type
                    var onProperty = Expression.Property(row, property);

                    // Create the equality comparison expression
                    var body = Expression.Equal(onProperty, filterEx);
                    var predicate = Expression.Lambda<Func<T, bool>>(body, row);
                    
                    query = query.Where(predicate);
                }
            }
            catch (Exception ex)
            {
                throw;
            }

            return await query.ToListAsync();
        }

        public async Task<bool> Update(Guid id, TDto dto)
        {
            var entity = _mapper.Map<TDto, T>(dto);
            entity.Id = id;
            return await _repository.Update(entity);
        }
    }
}
