using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Inmobiliaria_.Net_Core.Models;
using InmobiliariaGrosso.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MailKit;
using MimeKit;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Http.Features;
using Org.BouncyCastle.Asn1.Ocsp;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Inmobiliaria_.Net_Core.Api
{
	[Route("api/[controller]")]
	[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	[ApiController]
	public class PropietariosController : ControllerBase//
	{
		private readonly DataContext contexto;
		private readonly IConfiguration config;
		

		public PropietariosController(DataContext contexto, IConfiguration config)
		{
			this.contexto = contexto;
			this.config = config;
			
		}
		// GET: api/<controller>
		[HttpGet] //obtener todos los propietarios
		public async Task<ActionResult<Propietario>> Get()
		{
			try
			{
				
				var usuario =  User.Identity.Name;
				var prop = await contexto.Propietarios.SingleOrDefaultAsync(x => x.Email == usuario);
				var propView = new PropietarioView(prop);
				return Ok(propView);
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		// POST api/<controller>/login
		[HttpPost("login")] // Login de la aplicacion
		[AllowAnonymous]
		public async Task<IActionResult> Login([FromBody] LoginView loginView)
		{
			try
			{
				string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
					password: loginView.Clave,
					salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
					prf: KeyDerivationPrf.HMACSHA1,
					iterationCount: 1000,
					numBytesRequested: 256 / 8));
				var p = await contexto.Propietarios.FirstOrDefaultAsync(x =>  x.Email == loginView.Usuario);
				if (p == null || p.Clave != hashed)
				{
					return BadRequest("Nombre de usuario o clave incorrecta");
				} 
				else
				{
					var key = new SymmetricSecurityKey(
						System.Text.Encoding.ASCII.GetBytes(config["TokenAuthentication:SecretKey"]));
					var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
					var claims = new List<Claim>
					{
						new Claim(ClaimTypes.Name, p.Email),
						new Claim("FullName", p.Nombre + " " + p.Apellido),
						new Claim("id", p.Id + " " ),
						new Claim(ClaimTypes.Role, "Propietario"),
					};

					var token = new JwtSecurityToken(
						issuer: config["TokenAuthentication:Issuer"],
						audience: config["TokenAuthentication:Audience"],
						claims: claims,
						expires: DateTime.Now.AddMinutes(600),
						signingCredentials: credenciales
					);
					return Ok(new JwtSecurityTokenHandler().WriteToken(token));
				}
			
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

		// PUT api/<controller>
		[HttpPut("Actualizar")] //actualizar la informacion del usuario
		public async Task<IActionResult> Actualizar( [FromBody] Propietario entidad)
		
		{
			try
			{
				
				var usuario = User.Identity.Name;
				if (ModelState.IsValid)
				{
					
					Propietario original = await contexto.Propietarios.AsNoTracking().FirstOrDefaultAsync(x => x.Email == usuario);
					entidad.Id = original.Id;

					if (String.IsNullOrEmpty(entidad.Clave) )
					{
						entidad.Clave = original.Clave;
					}
					else
					{
						entidad.Clave = Convert.ToBase64String(KeyDerivation.Pbkdf2(
							password: entidad.Clave,
							salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
							prf: KeyDerivationPrf.HMACSHA1,
							iterationCount: 1000,
							numBytesRequested: 256 / 8));
					}
					contexto.Propietarios.Update(entidad);	
					await contexto.SaveChangesAsync();
					return Ok(entidad);
					
		
				}
				return BadRequest();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}



		// GET: api/<controller>
		[HttpGet("mail")] // envio del segundo mail con la contraseña sin hashear
		public async Task<ActionResult> mail()
		{
			try
			{
				

				var perfil = new{
				Email =  User.Identity.Name,
				Nombre = User.Claims.First(x=> x.Type == "FullName").Value,
				Rol = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Role).Value
				};

				Random rand        = new Random(Environment.TickCount);
				string randomChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijkmnopqrstuvwxyz0123456789";
				string nuevaClave  = "";
				for (int i = 0; i < 8; i++)
				{
					nuevaClave += randomChars[rand.Next(0, randomChars.Length)];
				}

				String nuevaClaveSin = nuevaClave;

				nuevaClave = Convert.ToBase64String(KeyDerivation.Pbkdf2(
							password: nuevaClave,
							salt: System.Text.Encoding.ASCII.GetBytes(config["Salt"]),
							prf: KeyDerivationPrf.HMACSHA1,
							iterationCount: 1000,
							numBytesRequested: 256 / 8));

				
				Propietario original = await contexto.Propietarios.AsNoTracking().FirstOrDefaultAsync(x => x.Email == perfil.Email);
				original.Clave = nuevaClave;
				contexto.Propietarios.Update(original);	
				await contexto.SaveChangesAsync();	
				
				var message = new MimeKit.MimeMessage();
				message.To.Add(new MailboxAddress(perfil.Nombre, "thecastleofilusion@gmail.com"));
				message.From.Add(new MailboxAddress("Sistema Inmobiliaria", "thecastleofilusion@gmail.com"));
				message.Subject = "Testing";
				message.Body = new TextPart("html")
				{
					Text = @$"<h1>Hola {perfil.Nombre}</h1>
					<p>Bienvenido, esta es su nueva Clave {nuevaClaveSin}</p>",					
				};

				message.Headers.Add("Encabezado", "Valor");
				MailKit.Net.Smtp.SmtpClient client = new MailKit.Net.Smtp.SmtpClient();
				client.ServerCertificateValidationCallback = (object sender,
				System.Security.Cryptography.X509Certificates.X509Certificate certificate,
				System.Security.Cryptography.X509Certificates.X509Chain chain,
				System.Net.Security.SslPolicyErrors sslPolicyErrors) => { return true;};
				client.Connect("smtp.gmail.com", 465, MailKit.Security.SecureSocketOptions.Auto);
				client.Authenticate(config["SMTPUser"], config["SMTPPass"]);

				await client.SendAsync(message);
				return Ok("Su contraseña se ha restablecido, por favor verifique su email para recibir la nueva contraseña");
			
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}


		[HttpPost("emailPedido")] // envio del primer mail con el token para acceder al metodo mail
		[AllowAnonymous]
		public async Task<IActionResult> GetByEmail([FromBody]string email)
		{
				var feature = HttpContext.Features.Get<IHttpConnectionFeature>();
				var LocalPort = feature?.LocalPort.ToString();
				var ipv4 = HttpContext.Connection.LocalIpAddress.MapToIPv4().ToString();
				var ipConexion = "http://" + ipv4 + ":" + LocalPort;
			try
			{	

				var entidad1 = await contexto.Propietarios.FirstOrDefaultAsync(x => x.Email == email);
				var entidad= new PropietarioView(entidad1);
				var key = new SymmetricSecurityKey(
						System.Text.Encoding.ASCII.GetBytes(config["TokenAuthentication:SecretKey"]));
					var credenciales = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
					var claims = new List<Claim>
					{
						new Claim(ClaimTypes.Name, entidad.Email),
						new Claim("FullName", entidad.Nombre + " " + entidad.Apellido),
						new Claim("id", entidad.Id + " " ),
						new Claim(ClaimTypes.Role, "Propietario"),
					};

					var token = new JwtSecurityToken(
						issuer: config["TokenAuthentication:Issuer"],
						audience: config["TokenAuthentication:Audience"],
						claims: claims,
						expires: DateTime.Now.AddMinutes(600),
						signingCredentials: credenciales
					);
					var to = new JwtSecurityTokenHandler().WriteToken(token);
					
					var direccion =  ipConexion + "/API/Propietarios/mail?access_token=" + to;
					try
			{
				
	
				var message = new MimeKit.MimeMessage();
				message.To.Add(new MailboxAddress(entidad.Nombre, "thecastleofilusion@gmail.com"));
				message.From.Add(new MailboxAddress("Sistema Inmobiliaria", "thecastleofilusion@gmail.com"));
				message.Subject = "Testing";
				message.Body = new TextPart("html")

				
				{
					Text = @$"<h1>Hola {entidad.Nombre} {entidad.Apellido} </h1>
					<p>Bienvenido, por favor siga el siguiente link para restablecer su contraseña <a href={direccion} >Restablecer</a> </p>
					<p>En caso de no haber pedido restablecer su contraseña por favor desestime este correo",					
				};
				
				


				message.Headers.Add("Encabezado", "Valor");
				MailKit.Net.Smtp.SmtpClient client = new MailKit.Net.Smtp.SmtpClient();
				client.ServerCertificateValidationCallback = (object sender,
				System.Security.Cryptography.X509Certificates.X509Certificate certificate,
				System.Security.Cryptography.X509Certificates.X509Chain chain,
				System.Net.Security.SslPolicyErrors sslPolicyErrors) => { return true;};
				client.Connect("smtp.gmail.com", 465, MailKit.Security.SecureSocketOptions.Auto);
				client.Authenticate(config["SMTPUser"], config["SMTPPass"]);
				await client.SendAsync(message);
			
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
				return entidad != null ? Ok(entidad) : NotFound();
			}
			catch (Exception ex)
			{
				return BadRequest(ex.Message);
			}
		}

	}
}
