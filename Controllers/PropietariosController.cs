using InmobiliariaGrosso.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Inmobiliaria_.Net_Core.Models;

namespace InmobiliariaGrosso.Controllers
{
    [Authorize]
    public class PropietariosController : Controller
    {
        RepoPropietario repo;


        RepoInmueble repoInmueble;


 /*       public PropietariosController(IRepositorioPropietario repo, IConfiguration config)
        {
            // Sin inyección de dependencias y sin usar el config (quitar el parámetro repo del ctor)
            this.repo = new RepoPropietario();
        
            this.repo = new RepoPropietario(config);
            
        }*/
        public PropietariosController()
        {
            repo = new RepoPropietario();
        }

        // GET: PropietariosController
        public ActionResult Index()
        {
            IList<Propietario> lista = repo.All();
            return View(lista);
        }

        // GET: PropietariosController/Details/5
        public ActionResult Details(int id)
        {
            Propietario p = repo.Details(id);

            if (p.Id != 0)
            {
                return View(p);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: PropietariosController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: PropietariosController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Propietario p)
        {
            try
            {
                repo.Put(p);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: PropietariosController/Edit/5
        public ActionResult Edit(int id)
        {
             var  entidad = repo.ObtenerPorId(id);
            return View(entidad);
        }

        // POST: PropietariosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            Propietario p = new Propietario();
            p.Id = id;
            p.Nombre = collection["Nombre"].ToString();
            p.Dni = collection["Dni"].ToString();
            p.Apellido = collection["Apellido"].ToString();
            p.Email = collection["Email"].ToString();
            p.Telefono = collection["Telefono"].ToString();

            try
            {
                int res = repo.Edit(p);
                if (res > 0)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }
            }
            catch
            {
                return View();
            }
        }

        // GET: PropietariosController/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            	try
			{
                var entidad = repo.ObtenerPorId(id);
                return View(entidad);
            }
            catch (Exception ex)
            {//poner breakpoints para detectar errores
                throw;
            }
        }

        // POST: PropietariosController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}