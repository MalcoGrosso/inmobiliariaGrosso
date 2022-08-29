using InmobiliariaGrosso.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaGrosso.Controllers
{
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
  //  IList<Inquilino> lista = repo.ObtenerLista();
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
                repo.Put(i);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                //return View();
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
        public ActionResult Edit(int id, IFormCollection collection)
        {
            Inquilino i = repo.ObtenerPorId(id);
            i.Id = id;
            i.Nombre = collection["Nombre"].ToString();
            i.Dni = collection["Dni"].ToString();
            i.Apellido = collection["Apellido"].ToString();
            i.Email = collection["Email"].ToString();
            i.Telefono = collection["Telefono"].ToString();
            repo.Edit(i);
            TempData["Mensaje"] = "Datos guardados correctamente";

            try
            {
                int res = repo.Edit(i);
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

        // GET: InquilinosController/Delete/5
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

        // POST: InquilinosController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
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
