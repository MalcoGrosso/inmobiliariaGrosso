using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using InmobiliariaGrosso.Data;
using InmobiliariaGrosso.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Authorization;


namespace InmobiliariaGrosso.Controllers
{
        [Authorize]
    public class InmueblesController : Controller
    {
        private RepoInmueble repo;
        private RepoPropietario repoPropietario;

        public InmueblesController()
        {
            repo = new RepoInmueble();
            repoPropietario = new RepoPropietario();
        }

        // GET: Inmueble
        public ActionResult Index()
        {

            IList<Inmueble> lista = repo.All();
            return View(lista);
          
          
        }

        // GET: Inmueble/Details/5
    
        public ActionResult Details(int id)
        {
            
                var i = repo.Details(id);
                if (i.Id > 0)
                {
                    return View(i);
                }
                else
                {
                    return RedirectToAction(nameof(Index));
                }

    
        }

        // GET: Inmueble/Create
        public ActionResult Create()
        {
                ViewBag.Propietarios = repoPropietario.All();
                return View();
        }

        // POST: Inmueble/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inmueble i)
        {
          {
            try
            {

                repo.Put(i);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                
                return RedirectToAction(nameof(Index));
            }
        }
        }

        // GET: Inmueble/Edit/5
        public ActionResult Edit(int id)
        {
          
            var entidad = repo.ObtenerPorId(id);
            ViewBag.Propietarios = repoPropietario.All();
            return View(entidad);
        }

        // POST: Inmueble/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Inmueble i)
        {
            

            try
            {
              
                var res = repo.Edit(i);
                if (res > 0)
                {
                       TempData["msg"] = "Cambios guardados.";
                        return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["msg"] = "No se guardaron los cambios. Intente nuevamente.";
                        return RedirectToAction(nameof(Edit), new { id = id });
                }
            }
            catch
            {
                return View(i);
            }
                
                

            
          
        }

        // GET: Inmueble/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            
           	try
			{
                var entidad = repo.ObtenerPorId(id);
                return View(entidad);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // POST: Inmueble/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, IFormCollection collection)
        {
           try
            {
                repo.Delete(id);
                TempData["Mensaje"] = "Eliminación realizada correctamente";
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
      
    }
}