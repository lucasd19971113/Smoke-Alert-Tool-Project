using System.Threading.Tasks;
using BrunoZell.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Projeto_IOT.Models;
using Projeto_IOT.Services.IServices;
using Projeto_IOT.Dtos.Responses;
using System;

namespace Projeto_IOT.Controllers
{
    public class AlertController : BaseController<IAlertaService, Alerta>
    {
        public AlertController(IAlertaService service, IMemoryCache cache) : base(service, cache)
        {
        }

        [HttpPost]
        public async Task<IActionResult> AddAlerta([FromBody] Alerta alert){
            try
            {
                var result = await service.Add(alert);
                if (result)
                {
                    
                    return Ok(result);
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