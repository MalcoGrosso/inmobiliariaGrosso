using InmobiliariaGrosso.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace InmobiliariaGrosso.Controllers
{
   // [Authorize(AuthenticationSchemes = CookieAuthenticationDefaults.AuthenticationScheme)]
    [Authorize]
public class InquilinosController : Controller
    {
        RepoInquilino repo;

        public InquilinosController()
        {
            repo = new RepoInquilino();
        }

        // GET: InquilinosController
        public ActionResult Index()
        {
            IList<Inquilino> lista = repo.All();
            return View(lista);
        }
        
        // GET: InquilinosController/Details/5
        public ActionResult Details(int id)
        {
            Inquilino i = repo.Details(id);

            if (i.Id != 0)
            {
                return View(i);
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // GET: InquilinosController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: InquilinosController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Inquilino i)
        {
                try
            {
            
                var res = repo.Put(i);
                        if (res > 0)
                        {
                            TempData["msg"] = "Inquilino cargado";
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

        // GET: InquilinosController/Edit/5
        public ActionResult Edit(int id)
        {
            var  entidad = repo.ObtenerPorId(id);
            return View(entidad);
        }

        // POST: InquilinosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Inquilino i)
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

        // GET: InquilinosController/Delete/5
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

        // POST: InquilinosController/Delete/5
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
