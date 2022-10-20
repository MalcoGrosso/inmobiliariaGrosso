﻿using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using Inmobiliaria_.Net_Core.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Inmobiliaria_.Net_Core.Api
{
	[Route("api/[controller]")]
	[ApiController]
	public class TestController : ControllerBase
	{
		private readonly DataContext Contexto;

		public TestController(DataContext dataContext)
		{
			this.Contexto = dataContext;
		}
		// GET: api/<controller>
		[HttpGet]
		public async Task<IActionResult> Get()
		{
			try
			{
				return Ok(new
				{
					Mensaje = "Éxito",
					Error = 0,
					Resultado = new
					{
						Clave = "Key",
						Valor = new Random().Next(0, 10000)
					},
				});
			}
			catch (Exception ex)
			{
				return BadRequest(ex);
			}
		}

		// GET api/<controller>/5
		[HttpGet("{id}")]
		public IActionResult Get(int id)
		{
			return Ok(Contexto.Propietarios.Find(id));
			//return Ok(Contexto.Inquilinos.Find(id));

		}



		// GET api/<controller>/5
		[HttpGet("emails/{id=0}")]
		public IActionResult Emails(int id)
		{
			if(id > 0)
				return Ok(Contexto.Propietarios.Where(x => x.Id == id).Select(x => x.Email).Single());
			else
				return Ok(Contexto.Propietarios.Select(x => x.Email).ToList());
		}

		// GET api/<controller>/5
		[HttpGet("anonimo/{id}")]
		public IActionResult GetAnonimo(int id)
		{
			return id > 0 ?
				Ok(Contexto.Propietarios.Where(x =>  x.Id == id)
				.Select(x => new { Id = x.Id, x.Email }).Single()) :
				Ok(Contexto.Propietarios.Select(x => new { Id = x.Id, x.Email }).ToList());
		}

		// POST api/<controller>
		[HttpPost]
		public void Post([FromForm] string value, IFormFile file)
		{

		}
		
	}
}
