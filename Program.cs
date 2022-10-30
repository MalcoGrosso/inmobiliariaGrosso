using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Inmobiliaria_.Net_Core.Models;
using InmobiliariaGrosso.Controllers;
using Inmobiliaria_.Net_Core.Api;
using InmobiliariaGrosso.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
var configuration = builder.Configuration;

builder.WebHost.ConfigureKestrel(serverOptions =>
{
	serverOptions.ListenAnyIP(5000);
	serverOptions.ListenAnyIP(5001, listenOptions => listenOptions.UseHttps() );
});

builder.Configuration.AddEnvironmentVariables()
					 .AddUserSecrets(System.Reflection.Assembly.GetExecutingAssembly());


builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)

                .AddCookie(options =>//el sitio web valida con cookie
                {
                    options.LoginPath = "/Usuarios/Login";
                    options.LogoutPath = "/Usuarios/Logout";
                    options.AccessDeniedPath = "/Home/Denied";
                })
                .AddJwtBearer(options =>//la api web valida con token
				{
					options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
					{
						ValidateIssuer = true,
						ValidateAudience = true,
						ValidateLifetime = true,
						ValidateIssuerSigningKey = true,
						ValidIssuer = configuration["TokenAuthentication:Issuer"],
						ValidAudience = configuration["TokenAuthentication:Audience"],
						IssuerSigningKey = new SymmetricSecurityKey(System.Text.Encoding.ASCII.GetBytes(
							configuration["TokenAuthentication:SecretKey"])),
					};

                   options.Events = new JwtBearerEvents
					{
						OnMessageReceived = context =>
						{
							
							var accessToken = context.Request.Query["access_token"];
							var path = context.HttpContext.Request.Path;
							if (!string.IsNullOrEmpty(accessToken) &&
								path.StartsWithSegments("/API/Propietarios/mail"))
							{
								context.Token = accessToken;
							}
							return Task.CompletedTask;
						}
					};


				});     
                
               
builder.Services.AddAuthorization(options =>
            {
                options.AddPolicy("Administrador", policy => policy.RequireRole("Administrador", "SuperAdministrador"));
            });


        
            
builder.Services.AddControllersWithViews();
builder.Services.AddMvc();            
builder.Services.AddDbContext<DataContext>(
				options => options.UseMySql(
					configuration["ConnectionStrings:DefaultConnection"],
                    ServerVersion.AutoDetect(configuration["ConnectionStrings:DefaultConnection"])
				)
			);                

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


// Add services to the container.
builder.Services.AddControllersWithViews();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}




// app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
