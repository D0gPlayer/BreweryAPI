using AutoMapper;
using BreweryAPI.Data;
using BreweryAPI.Models;

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

        public async Task<bool> Delete(Guid Id)
        {
            return await _repository.Delete(Id);
        }

        public async Task<T?> Get(Guid Id)
        {
            return await _repository.DbSet.FindAsync(Id);
        }

        public async Task<bool> Update(Guid Id, TDto dto)
        {
            var entity = _mapper.Map<TDto, T>(dto);
            entity.Id = Id;
            return await _repository.Update(entity);
        }
    }
}
