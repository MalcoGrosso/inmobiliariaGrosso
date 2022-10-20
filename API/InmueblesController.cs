﻿using System;
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
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var usuario = User.Identity.Name;
                var inmuebles = contexto.Inmuebles.Include(e =>  e.Duenio).Where(e => e.Duenio.Email == usuario);
                return Ok(inmuebles);
            }
            catch (Exception ex)
            {
                return BadRequest(ex);
            }
        }



        [HttpPost("Nuevo")] // nuevo inmueble
        public async Task<ActionResult<Inmueble>> Post([FromBody] Inmueble inmueble)
        {

            try
            {

                 var usuario = User.Identity.Name;
                Propietario propietario = await contexto.Propietarios.FirstOrDefaultAsync(x => x.Email == usuario);
                inmueble.IdPropietario = propietario.Id;
               if (inmueble.Imagen != null )
               {
                   
                  //  string stream1 = Convert.ToBase64String(System.Text.Encoding.Default.GetBytes(inmueble.ImagenGuardar));
                    
        //            var st = Convert.FromBase64String(inmueble.ImagenGuardar);
                    
                    MemoryStream stream1 = new MemoryStream(Convert.FromBase64String(inmueble.Imagen));
                    IFormFile ImagenInmo = new FormFile(stream1, 0, stream1.Length, "inmueble", ".jpg");
                    string wwwPath = environment.WebRootPath;
                    string path = Path.Combine(wwwPath, "Uploads");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
                    Random r = new Random();
                    string fileName = "inmueble_" + inmueble.IdPropietario + r.Next(0,100000)+Path.GetExtension(ImagenInmo.FileName);
                    string pathCompleto = Path.Combine(path, fileName);
                    
                    inmueble.Imagen = Path.Combine("http://192.168.1.100:5000/" ,"Uploads/", fileName);
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

           [HttpPut("{id}")]//Cambiar Estado
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