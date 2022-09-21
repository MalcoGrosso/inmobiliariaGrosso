using InmobiliariaGrosso.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace InmobiliariaGrosso.Controllers
{
    [Authorize]
    public class PropietariosController : Controller
    {
        
        RepoPropietario repo;

        RepoInmueble repoInmueble;

        public PropietariosController()
        {
            repo = new RepoPropietario();
            repoInmueble = new RepoInmueble();
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
            
                var res = repo.Put(p);
                        if (res > 0)
                        {
                            TempData["msg"] = "Propietario cargado";
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            TempData["msg"] = "No se cargó el Inquilino. Intente nuevamente.";
                            return RedirectToAction(nameof(Create));
                        }
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
        public ActionResult Edit(int id, Propietario p)
        {
              try
            {
              
                var res = repo.Edit(p);
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
                return View(p);
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
            {
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
                repo.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Inmuebles(int id)
        {
            try
            {
                var p = repo.ObtenerPorId(id);
                if (p.Id == 0)
                {
                    TempData["msg"] = "No se encontró Propietario.";
                    return RedirectToAction(nameof(Index));
                }
                ViewBag.Propietario = p;
                IList<Inmueble> inmuebles =  repoInmueble.TodosPorInquilino(id);
                return View(inmuebles);
            }
                catch (Exception)
                {
                    throw;
                }
            
        }


    }
}