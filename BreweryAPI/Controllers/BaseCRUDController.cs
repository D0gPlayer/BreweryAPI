using BreweryAPI.Models;
using BreweryAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BreweryAPI.Controllers
{
    public class BaseCRUDController<T, TDto> : ControllerBase where T : BaseEntity 
    {
        private readonly BaseCRUDService<T, TDto> _baseService;

        protected BaseCRUDController(BaseCRUDService<T, TDto> baseService)
        {
            _baseService = baseService;
        }

        [HttpGet("{Id}")]
        public virtual async Task<IActionResult> Get(Guid Id)
        {
            var entity = await _baseService.Get(Id);
            return entity is null ? NotFound() : Ok(entity);
        }

        [HttpPost()]
        public virtual async Task<IActionResult> Add(TDto dto)
        {
            var added = await _baseService.Add(dto);
            return added ? Ok() : BadRequest();
        }

        [HttpPut("{Id}")]
        public virtual async Task<IActionResult> Update(Guid Id,TDto dto)
        {
            var added = await _baseService.Update(Id, dto);
            return added ? Ok() : BadRequest();
        }

        [HttpDelete("{Id}")]
        public virtual async Task<IActionResult> Delete(Guid Id)
        {
            var deleted = await _baseService.Delete(Id);
            return deleted ? Ok() : BadRequest();
        }
    }
}
