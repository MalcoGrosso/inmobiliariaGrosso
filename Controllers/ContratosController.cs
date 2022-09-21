using InmobiliariaGrosso.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace InmobiliariaGrosso.Controllers
{
    [Authorize]
    public class ContratosController : Controller
    {
        private RepoContrato repo;
        private RepoInmueble repoInmueble;
        private RepoInquilino repoInquilino;
        private RepoContrato repoContrato;
        private RepoPago repoPago;
  
        public ContratosController()
        {
            repo = new RepoContrato();
            repoInmueble = new RepoInmueble();
            repoInquilino = new RepoInquilino();
            repoContrato = new RepoContrato();
            repoPago = new RepoPago();
        }

        // GET: ContratosController
        public ActionResult Index()
        {
                var lista = repo.All();
                return View(lista);
        }

        // GET: ContratosController/Details/5
        public ActionResult Details(int id)
        {
                var c = repo.Details(id);
                if (c.Id > 0)
                {
                    return View(c);
                }
                else
                {
                    TempData["msg"] = "No se encontró Contrato. Intente nuevamente.";
                    return RedirectToAction(nameof(Index));
                }
        }

        // GET: ContratosController/Create
       public ActionResult Create()
        {
                ViewBag.Inmuebles = repoInmueble.Validos();
                ViewBag.Inquilinos = repoInquilino.All();
                return View();
        }

        // POST: ContratosController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Contrato c)
        { 
            try
            {
            
                var res = repo.Put(c);
                        if (res > 0)
                        {
                            TempData["msg"] = "Contrato cargado";
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            TempData["msg"] = "No se cargó el contrato. Intente nuevamente.";
                            return RedirectToAction(nameof(Create));
                        }
            }
            catch
            {
                
                return RedirectToAction(nameof(Index));
            }
        
                        
        }

        // GET: ContratosController/Edit/5
        public ActionResult Edit(int id)
        {
                var z = repo.Details(id);
                DateTime Hoy = DateTime.Today;
                z.Desde.AddDays(3);
                
                var c = repo.Details(id);
                if (c.Id <= 0)
                {
                    TempData["msg"] = "No se encontró Contrato. Intente nuevamente.";
                    return RedirectToAction(nameof(Index));
                }

                var inmuebles = repoInmueble.All();
                if (inmuebles.Count == 0)
                {
                    TempData["msg"] = "No se pudo obtener lista de Inmuebles. Intente nuevamente.";
                    return RedirectToAction(nameof(Index));
                }
                
                var inquilinos = repoInquilino.All();
                if (inquilinos.Count == 0)
                {
                    TempData["msg"] = "No se pudo obtener lista de Inquilinos. Intente nuevamente.";
                    return RedirectToAction(nameof(Index));
                }

                if ( z.Desde.AddDays(3) > Hoy){
                    ViewBag.Inmuebles = inmuebles;
                    ViewBag.Inquilinos = inquilinos;
                    return View(z);
                }else {
                     TempData["msg"] = "No se puede editar un contrato luego de 3 dias de su Creacion.";
                    return RedirectToAction(nameof(Index));

                }

        }

        // POST: ContratosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Contrato c)
        {

                var res = repo.Edit(c);
                if (res > 0)
                {
                    TempData["msg"] = "Cambios guardados.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["msg"] = "No se guardaron cambios. Intente nuevamente.";
                    return RedirectToAction(nameof(Edit), new { id = id });
                }
        }


             // GET: ContratosController/Renovacion/5
        public ActionResult Renovacion(int id)
        {
                var z = repo.Details(id);
                DateTime Hoy = DateTime.Today;
                z.Desde.AddDays(3);
                
                var c = repo.Details(id);
                if (c.Id <= 0)
                {
                    TempData["msg"] = "No se encontró Contrato. Intente nuevamente.";
                    return RedirectToAction(nameof(Index));
                }

                var inmuebles = repoInmueble.All();
                if (inmuebles.Count == 0)
                {
                    TempData["msg"] = "No se pudo obtener lista de Inmuebles. Intente nuevamente.";
                    return RedirectToAction(nameof(Index));
                }
                
                var inquilinos = repoInquilino.All();
                if (inquilinos.Count == 0)
                {
                    TempData["msg"] = "No se pudo obtener lista de Inquilinos. Intente nuevamente.";
                    return RedirectToAction(nameof(Index));
                }
                if ( z.Hasta < Hoy){
                    ViewBag.Inmuebles = inmuebles;
                    ViewBag.Inquilinos = inquilinos;
                    return View(z);
                }else {
                     TempData["msg"] = "No se puede Realizar una Renovacion mientras el contrato siga Vigente.";
                    return RedirectToAction(nameof(Index));

                }
        }

        // POST: ContratosController/Renovacion/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Renovacion(Contrato c)
        {

                var res = repo.Renovacion(c);
                if (res > 0)
                {
                    TempData["msg"] = "Cambios guardados.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["msg"] = "No se guardaron cambios. Intente nuevamente.";
                    return RedirectToAction(nameof(Index));
                }
        }




        // GET: ContratosController/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            
                Contrato c = repo.Details(id);
                if (c.Id > 0)
                {
                    return View(c);
                }
                else
                {
                    TempData["msg"] = "No se encontró Contrato. Intente nuevamente";
                    return RedirectToAction(nameof(Index));
                }
            
            

        }

        // POST: ContratosController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            
                var res = repo.Delete(id);
                if (res > 0)
                {
                    TempData["msg"] = "Contrato eliminado de la base de datos.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    Exception e = new Exception("No se eliminó Contrato. Intente nuevamente.");
                    return RedirectToAction(nameof(Delete), new { id = id });
                }
            
        
        }

        public ActionResult Pagos(int id)
        {
            try
            {
                var c = repo.Details(id);
                if (c.Id == 0)
                {
                    TempData["msg"] = "No se encontró Contrato.";
                    return RedirectToAction(nameof(Index));
                }

                IList<Pago> pagos = repoPago.AllByContrato(id);
                ViewBag.Contrato = c;
                return View(pagos);
            }
            
                catch (Exception)
                {
                    throw;
                }
            
        }

    }
}
