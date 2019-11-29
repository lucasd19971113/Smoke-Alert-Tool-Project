using System.Threading.Tasks;
using BrunoZell.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Projeto_IOT.Models;
using Projeto_IOT.Services.IServices;
using Projeto_IOT.Dtos.Responses;
using System;
using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.Extensions.Configuration;

namespace Projeto_IOT.Controllers
{
    public class UsuarioController : BaseController<IUsuarioService, Usuario>
    {
        private Projeto_IOT.Helper.IEmail _email;
        private readonly IConfiguration config;
        public UsuarioController(IConfiguration config,IUsuarioService service, IMemoryCache cache, Projeto_IOT.Helper.IEmail email) : base(service, cache)
        {
            _email = email;
            this.config = config;
        }

        [HttpPost]
        public async Task<IActionResult> AddUser([FromBody] Usuario user){
            try
            {
                var result = await service.Add(user);
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

        [HttpGet]
        public async Task<IActionResult> Notificar(){
            try
            {
                var result = await service.GetAll();
                if (result)
                {
                    var mailEntity = _email.Notify(result.Value);
                    if(mailEntity != null) 
                    
                    await _email.SendEmail(mailEntity);
                    
                    return Ok(result);
                }

                return StatusCode(result.StatusCode, ErrorDto.Create(result.StatusCode, result.Error));
            }
            catch (Exception ex)
            {
                return StatusCode(500, ErrorDto.Create(500, "Internal server error"));
            }
        }

        public async Task<IActionResult> NotificarUser(){
            try
            {
               
                   
                    
                    return Ok();
            

            }
            catch (Exception ex)
            {
                return StatusCode(500, ErrorDto.Create(500, "Internal server error"));
            }
        }
    }
}