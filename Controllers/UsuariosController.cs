using InmobiliariaGrosso.Models;
using InmobiliariaGrosso.ModelsAux;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace InmobiliariaGrosso.Controllers
{
 //   [Authorize]
    public class UsuariosController : Controller
    {
        private RepoUsuario repo;
        private IConfiguration configuration;
        private IWebHostEnvironment environment;

        public UsuariosController(IConfiguration config, IWebHostEnvironment environment)
        {
            this.repo = new RepoUsuario();
            this.configuration = config;
            this.environment = environment;

        }

        // GET: Usuarios
    //    [Authorize(Policy = "Administrador")]
        public ActionResult Index()
        {
            var usuarios = repo.ObtenerTodos();
            return View(usuarios);
        }

        // GET: Usuarios/Details/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Details(int id)
        {
            try
            {
                var u = repo.ObtenerPorId(id);
                if (u != null)
                {
                    return View(u);
                }
                else
                {
                    TempData["msg"] = "No se encontró el usuario";
                    return RedirectToAction(nameof(Index), new { id = id });
                }
            }
            
                catch (Exception ex)
                {
                    throw;
                }
            
        }

        // GET: Usuarios/Create
        [Authorize(Policy = "Administrador")]
        public ActionResult Create()
        {
            try
            {
                ViewBag.Roles = Usuario.ObtenerRoles();
                return View();
            }
            catch (Exception ex)
            { 
                throw;
            }

        }

        // POST: Usuarios/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Create(Usuario usu)
        {
            try
            {
                string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: usu.Clave,
                        salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));
                usu.Clave = hashed;               
                int res = repo.Alta(usu);
                if (usu.AvatarFile != null && usu.Id > 0)
                {
                    string wwwPath = environment.WebRootPath;
                    string path = Path.Combine(wwwPath, "Uploads");
                    if (!Directory.Exists(path))
                    {
                        Directory.CreateDirectory(path);
                    }
            
                    string fileName = "avatar_" + usu.Id + Path.GetExtension(usu.AvatarFile.FileName);
                    string pathCompleto = Path.Combine(path, fileName);
                    usu.Avatar = Path.Combine("/Uploads", fileName);
                    using (FileStream stream = new FileStream(pathCompleto, FileMode.Create))
                    {
                        usu.AvatarFile.CopyTo(stream);
                    }
                    repo.Modificacion(usu);
                }
                else
                {
                    usu.Avatar = Path.Combine("/Uploads", "usuario.png");
                    repo.Modificacion(usu);
                }
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ViewBag.Roles = Usuario.ObtenerRoles();
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: Usuarios/Edit/5
        [Authorize]
        public ActionResult Perfil()
        {
            try
            {
                ViewData["Title"] = "Mi perfil";
                TempData["returnUrl"] = Request.Headers["referer"].FirstOrDefault();
                var usu = repo.ObtenerPorEmail(User.Identity.Name);
                if (usu != null)
                {
                    ViewBag.Roles = Usuario.ObtenerRoles();
                    return View("Edit", usu);
                }
                else
                {
                    TempData["msg"] = "No se encontró usuario. Intente nuevamente.";
                    return RedirectToAction("Index", "Home");
                }

            }
            catch (Exception ex)
                { 
                    throw;  
                }

        }

        // GET: Usuarios/Edit/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Edit(int id)
        {
            try
            {
                ViewData["Title"] = "Editar usuario";
                TempData["returnUrl"] = Request.Headers["referer"].FirstOrDefault();
                var usu = repo.ObtenerPorId(id);
                if (usu != null)
                {
                    ViewBag.Roles = Usuario.ObtenerRoles();
                    return View(usu);
                }
                else
                {
                    TempData["msg"] = "No se encontró el usuario. Intente nuevamente.";
                    return RedirectToAction(nameof(Index));
                }
            }
            catch (Exception ex)
                {  
                throw;
                }

        }

        // POST: Usuarios/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public ActionResult Edit(int id, Usuario usu)
        {
            var returnUrl = Request.Headers["referer"].FirstOrDefault();
            bool editAvatar = false;
            bool editRol = false;

            try
            {
                var sessionUser = repo.ObtenerPorEmail(User.Identity.Name);

                if (usu.Id == 0) 
                {
                    usu.Id = sessionUser.Id; 
                }

                if (usu.Rol > 0)
                {
                    editRol = true; 
                }

                if (usu.AvatarFile != null)
                {
                    string wwwPath = environment.WebRootPath; 
                    string pathUploads = Path.Combine(wwwPath, "Uploads");

                    if (!Directory.Exists(pathUploads))
                    {
                        Directory.CreateDirectory(pathUploads);
                    }

                    string fileName = "avatar_" + usu.Id + Path.GetExtension(usu.AvatarFile.FileName);
                    string avatarFullPath = Path.Combine(pathUploads, fileName);

                    using (FileStream stream = new FileStream(avatarFullPath, FileMode.Create))
                    {
                        usu.AvatarFile.CopyTo(stream);
                        usu.Avatar = Path.Combine("Uploads", fileName);
                        editAvatar = true;
                    }
                }

                var res = repo.Update(usu, editRol, editAvatar);

                if (res > 0)
                {
                    TempData["msg"] = "¡Actualizacion Realizada!";
                    return Redirect(returnUrl);
                }
                else
                {
                    TempData["msg"] = "No se han actualizado datos.";
                    return Redirect(returnUrl);
                }


            }
            catch (Exception ex)
            {    
                    throw;
            }
        }


        // GET: Usuarios/EditPass/{id}
        [Authorize]
        public ActionResult EditPass(int id)
        {
            try
            {
                var sessionUser = repo.ObtenerPorEmail(User.Identity.Name);

                var userToEdit = repo.ObtenerPorId(id);
                if (userToEdit != null)
                {
                    ViewBag.Usuario = userToEdit;
                    return View();
                }
                else
                {
                    TempData["msg"] = "No se ha encontrado el usuario. Intente Nuevamente";
                    return Redirect(Request.Headers["referer"].FirstOrDefault());
                }
            }
            catch (Exception ex)
            {
                 throw;
            }
        }

        // POST: Usuarios/EditPass/{id}
        [HttpPost]
        [Authorize]
        public ActionResult EditPass(int id, UsuarioPassEdit pass)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var sessionUser = repo.ObtenerPorEmail(User.Identity.Name);
                    var userToEdit = repo.ObtenerPorId(id);

                    if (userToEdit == null)
                    {
                        TempData["msg"] = "No se encontró el usuario. Intente Nuevamente.";
                        return Redirect(Request.Headers["referer"].FirstOrDefault());
                    }

                    string oldPassHashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                           password: pass.OldPass,
                           salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                           prf: KeyDerivationPrf.HMACSHA1,
                           iterationCount: 1000,
                           numBytesRequested: 256 / 8));

                    if (oldPassHashed != userToEdit.Clave)
                    {
                        TempData["msg"] = "La contraseña antigua no es válida";
                        return RedirectToAction(nameof(EditPass), new { id = id});
                    }

                    string newPassHashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                           password: pass.NewPass,
                           salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                           prf: KeyDerivationPrf.HMACSHA1,
                           iterationCount: 1000,
                           numBytesRequested: 256 / 8));

                    var res = repo.UpdatePass(id, newPassHashed);

                    if (res > 0)
                    {
                        TempData["msg"] = "¡Contraseña actualizada!";
                        return Redirect(Request.Headers["referer"].FirstOrDefault());
                    }
                    else
                    {
                        TempData["msg"] = "No se pudo cambiar contraseña. Intente nuevamente.";
                        return RedirectToAction(nameof(EditPass), new { id = id});
                    }

                }
                else
                {
                    TempData["msg"] = "Los datos ingresados no son válidos. Intente nuevamente.";
                    return RedirectToAction(nameof(EditPass), new { id = id });
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        // GET: Usuarios/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            try
            {
                var usu = repo.ObtenerPorId(id);
                if (usu != null)
                {
                    return View(usu);
                }
                else
                {
                    TempData["msg"] = "No se encontró el usuario";
                    return RedirectToAction(nameof(Delete), new { id = id });
                }
            }
            catch (Exception ex)
            {
               throw;   
            }

        }

        // POST: Usuarios/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                // TODO: Add delete logic here
                var sessionUser = repo.ObtenerPorEmail(User.Identity.Name);
                if(id == sessionUser.Id){
                    TempData["msg"] = "No se eliminó al usuario. Esta intentando borrar el usuario con el que se encuentra logueado.";
                    return RedirectToAction(nameof(Delete));
                }
                var res = repo.Baja(id);
                if (res > 0)
                {
                    TempData["msg"] = "¡Usuario eliminado!";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["msg"] = "No se eliminó al usuario. Intnte nuevamente.";
                    return RedirectToAction(nameof(Delete));
                }

            }
            catch (Exception ex)
            {
                throw;  
            }
        }



        [AllowAnonymous]
        // GET: Usuarios/Login/
        public ActionResult Login(string returnUrl)
        {
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }

            TempData["returnUrl"] = returnUrl;
            return View();
        }

        // POST: Usuarios/Login/
        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginView login)
        {
            
            try
            {
                var returnUrl = String.IsNullOrEmpty(TempData["returnUrl"] as string) ? "/Home" : TempData["returnUrl"].ToString();
                if (ModelState.IsValid)
                {
                    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                        password: login.Clave,
                        salt: System.Text.Encoding.ASCII.GetBytes(configuration["Salt"]),
                        prf: KeyDerivationPrf.HMACSHA1,
                        iterationCount: 1000,
                        numBytesRequested: 256 / 8));

                    var e = repo.ObtenerPorEmail(login.Usuario);

                    if (e == null || e.Clave != hashed)
                    {
                        ModelState.AddModelError("", "El email o la clave no son correctos");
                        TempData["returnUrl"] = returnUrl;
                        return View();
                    }

                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, e.Email),
                        new Claim("FullName", e.Nombre + " " + e.Apellido),
                        new Claim(ClaimTypes.Role, e.RolNombre),
                    };

                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    await HttpContext.SignInAsync(
                        CookieAuthenticationDefaults.AuthenticationScheme,
                        new ClaimsPrincipal(claimsIdentity));

                    TempData.Remove("returnUrl");
                
                    return Redirect(returnUrl);
                }
                ModelState.AddModelError("", "El email o la clave no son correctos");
                TempData["returnUrl"] = returnUrl;
                return View();
            }
            catch (Exception ex)
            {
               throw;  
            }
        }

        // GET: /salir
        [Route("salir", Name = "logout")]
        public async Task<ActionResult> Logout()
        {
            try
            {
                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Index", "Home");
            }
            catch (Exception e)
            {
                try
                {
                    string user = "Anónimo";
                    if (!String.IsNullOrEmpty(User.Identity.Name))
                    {
                        user = User.Identity.Name;
                    }
                    
                    TempData["msg"] = "Ocurrió un error. Intente nuevamente. ID_ERROR: ";
                    return Redirect(Request.Headers["referer"].FirstOrDefault());
                }
                catch (Exception)
                {
                    throw;
                }
            }

        }
    }
}
