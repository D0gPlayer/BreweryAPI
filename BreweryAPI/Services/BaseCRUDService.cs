using AutoMapper;
using BreweryAPI.Data;
using BreweryAPI.Extensions;
using BreweryAPI.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace BreweryAPI.Services
{
    public class BaseCRUDService<T, TDto> where T : BaseEntity
    {
        protected readonly IRepository<T> _repository;
        protected readonly IMapper _mapper;
        //private readonly ILogger _logger;

        public BaseCRUDService(IRepository<T> repository, IMapper mapper)
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
            return await _repository.DeleteWithCache(id);
        }

        public async Task<T?> Get(Guid id)
        {
            return await _repository.GetCached(id);
        }

        public async Task<IList<T>> GetAll()
        {
            return await _repository.DbSet.AsNoTracking().ToListAsync();
        }

        public async Task<IList<T>> GetFiltered(Dictionary<string, string> queryFilters)
        {
            var query = _repository.DbSet.AsNoTracking().AsQueryable();
            query.AddFilters(queryFilters);

            return await query.ToListAsync();
        }


        public async Task<bool> Update(Guid id, TDto dto)
        {
            var entity = _mapper.Map<TDto, T>(dto);
            entity.Id = id;
            return await _repository.UpdateWithCache(entity);
        }
    }
}
