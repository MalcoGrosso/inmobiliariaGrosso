using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Inmobiliaria_.Net_Core.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using InmobiliariaGrosso.Models;
using Microsoft.AspNetCore.Http.Features;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Inmobiliaria_.Net_Core.Api
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class InmueblesController : Controller
    {
        private readonly DataContext contexto;
        private readonly IWebHostEnvironment environment;

        public InmueblesController(DataContext contexto, IWebHostEnvironment environment)
        {
            this.contexto = contexto;
            this.environment = environment;
        }

        // GET: api/<controller>
        [HttpGet] // obttener todos los inmuebles
        public async Task<IActionResult> Get()
        {
            try
            {
                var usuario1 = int.Parse(User.Claims.First(x => x.Type == "id").Value);
                var inmuebles = contexto.Inmuebles.Where(e => e.IdPropietario == usuario1);
                return Ok(inmuebles);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }



        [HttpPost("Nuevo")] // nuevo inmueble
        public async Task<ActionResult<Inmueble>> Post([FromBody] Inmueble inmueble)
        {
                var feature = HttpContext.Features.Get<IHttpConnectionFeature>();
				var LocalPort = feature?.LocalPort.ToString();
				var ipv4 = HttpContext.Connection.LocalIpAddress.MapToIPv4().ToString();
				var ipConexion = "http://" + ipv4 + ":" + LocalPort + "/";

            try
            {

                 var usuario = User.Identity.Name;
                Propietario propietario = await contexto.Propietarios.FirstOrDefaultAsync(x => x.Email == usuario);
                inmueble.IdPropietario = propietario.Id;
               if (inmueble.Imagen != null )
               {
                   
                    
                    MemoryStream stream1 = new MemoryStream(Convert.FromBase64String(inmueble.Imagen));
                    IFormFile ImagenInmo = new FormFile(stream1, 0, stream1.Length, "inmueble", ".PNG");
                    string wwwPath = environment.WebRootPath;
                    string path = Path.Combine(wwwPath, "Uploads");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    Random r = new Random();
                    string fileName = "inmueble_" + inmueble.IdPropietario + r.Next(0,100000)+Path.GetExtension(ImagenInmo.FileName);
                    string pathCompleto = Path.Combine(path, fileName);
                    
                    inmueble.Imagen = Path.Combine(ipConexion ,"Uploads/", fileName);
                    using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                    {
                        ImagenInmo.CopyTo(stream);
                    }
                    contexto.Add(inmueble);
                    await contexto.SaveChangesAsync();
                    return inmueble;

                }
                else
                {

                    return BadRequest("Hola ");
                }

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

           [HttpPut("{id}")]//Cambiar Estado de disponible a no disponible Checkbox
        public async Task<ActionResult<Inmueble>> Put([FromBody] bool Disponible, int id)
        {

            try
            {

                var usuario = User.Identity.Name;
                var propietario = await contexto.Propietarios.FirstOrDefaultAsync(x => x.Email == usuario);
                Inmueble inmu = await contexto.Inmuebles.FirstOrDefaultAsync(x => x.Id == id && x.IdPropietario == propietario.Id);
                if (inmu == null)
                {
                    return NotFound();
                }

                inmu.Disponible = Disponible;
                contexto.Update(inmu);
                await contexto.SaveChangesAsync();
                return inmu;

            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }

        }


        [HttpGet("contrato")]// Listar contratos por inmuebles
        public async Task<ActionResult<List<Inmueble>>> GetAlquilados()
        {

            try
            {
                var usuario = User.Identity.Name;
                var propietario = await contexto.Propietarios.FirstOrDefaultAsync(x => x.Email == usuario);
                var inmu = contexto.Contratos.Where(x => x.Inmueble.Duenio.Email == usuario).Include(l => l.Inmueble).ToList();
        
                    return Ok(inmu);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);

            }
        }
        
	}
}
