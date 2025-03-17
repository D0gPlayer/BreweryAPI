using BreweryAPI.Models;
using BreweryAPI.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Linq.Expressions;

namespace BreweryAPI.Controllers
{
    public abstract class BaseCRUDController<T, TDto> : ControllerBase where T : BaseEntity 
    {
        private readonly BaseCRUDService<T, TDto> _baseService;

        protected BaseCRUDController(BaseCRUDService<T, TDto> baseService)
        {
            _baseService = baseService;
        }

        [HttpGet("{id}")]
        public virtual async Task<IActionResult> Get(Guid id)
        {
            var entity = await _baseService.Get(id);
            return entity is null ? NotFound() : Ok(entity);
        }

        [HttpGet("Filter")]
        public virtual async Task<IActionResult> GetFiltered([FromQuery]Dictionary<string, string> queryFilters)
        {
            return Ok(await _baseService.GetFiltered(queryFilters));
        }

        [HttpGet("All")]
        public virtual async Task<IActionResult> GetAll()
        {
            return Ok(await _baseService.GetAll());
        }

        [HttpPost()]
        public virtual async Task<IActionResult> Add(TDto dto)
        {
            var added = await _baseService.Add(dto);
            return added ? Ok() : BadRequest();
        }

        [HttpPut("{id}")]
        public virtual async Task<IActionResult> Update(Guid id,TDto dto)
        {
            var added = await _baseService.Update(id, dto);
            return added ? Ok() : BadRequest();
        }

        [HttpDelete("{id}")]
        public virtual async Task<IActionResult> Delete(Guid id)
        {
            var deleted = await _baseService.Delete(id);
            return deleted ? Ok() : BadRequest();
        }
    }
}
