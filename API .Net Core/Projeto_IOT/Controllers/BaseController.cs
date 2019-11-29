using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Projeto_IOT.Dtos.Responses;
using Projeto_IOT.Services.IServices;

namespace Projeto_IOT.Controllers
{
    [Produces("application/json")]
    [Route("api/[controller]/[action]")]
    [ApiController]
    public abstract class BaseController<S, TModel> : ControllerBase
        where S : IService<TModel>
    {
        protected readonly S service;

        protected readonly IMemoryCache cache;

        public BaseController(S service, IMemoryCache cache)
        {
            this.service = service;
            this.cache = cache;
        }

        [HttpPost]
        [NonAction]
        public async Task<IActionResult> Add(TModel entity)
        {
            try
            {
                var result = await service.Add(entity);
                if (result)
                {
                    
                    return Ok(entity);
                }

                return StatusCode(result.StatusCode, ErrorDto.Create(result.StatusCode, result.Error));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ErrorDto.Create(500, "Internal server error"));
            }
        }

        [HttpGet]
        [Route("{id}")]
        public virtual async Task<IActionResult> Get(int id)
        {
            object resultado;

            try
            {
                if (!cache.TryGetValue(id.ToString(), out resultado))
                {
                    var result = await service.GetById(id);
                    if (result)
                    {
                        return Ok(result.Value);
                    }

                    return StatusCode(result.StatusCode, ErrorDto.Create(result.StatusCode, result.Error));
                }
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                return StatusCode(500, ErrorDto.Create(500, "Internal server error"));
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                var result = await service.GetAll();
                if (result)
                {
                    return Ok(result.Value);
                }

                return StatusCode(result.StatusCode, ErrorDto.Create(result.StatusCode, result.Error));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ErrorDto.Create(500, "Internal server error"));
            }
        }

        

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var result = await service.Delete(id);
                if (result)
                {
                    return Ok();
                }

                return StatusCode(result.StatusCode, ErrorDto.Create(result.StatusCode, result.Error));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ErrorDto.Create(500, "Internal server error"));
            }
        }
    }
}