using Inmobiliaria_.Net_Core.Models;
using InmobiliariaGrosso.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Inmobiliaria_.Net_Core.Api
{
	[Route("api/[controller]")]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	[ApiController]
	public class ContratosController : ControllerBase//
	{
		private readonly DataContext contexto;
		private readonly IConfiguration config;

		public ContratosController(DataContext contexto, IConfiguration config)
		{
			this.contexto = contexto;
			this.config = config;
		}

        
        [HttpGet("{id}")]
        public async Task<ActionResult<Contrato>> GetContratoXInmueble(int id)
        {
            
            try
            {
                if (ModelState.IsValid)
                {
                 var usuario = User.Identity.Name;
                 var propietario = await contexto.Propietarios.FirstOrDefaultAsync(x => x.Email == usuario);
                var contrato = await contexto.Contratos.Include(x=> x.Inquilino).Include(x=>x.Inmueble).Where(x =>
                   x.Id == id && propietario.Email == x.Inmueble.Duenio.Email).FirstOrDefaultAsync();
                    return Ok(contrato);

                }
                else
                {
                    return BadRequest();
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message.ToString());

            }
        }
    }
}
