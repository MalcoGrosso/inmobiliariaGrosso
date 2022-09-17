using InmobiliariaGrosso.Data;
using InmobiliariaGrosso.Models;
using InmobiliariaGrosso.ModelsAux;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.IO.Pipelines;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaGrosso.Controllers
{
    [Authorize]
    public class PagosController : Controller
    {
        private RepoPago repo;
        private RepoContrato repoContrato;
        private RepoInquilino repoInquilino;
    

        public PagosController()
        {
            repo = new RepoPago();
            repoContrato = new RepoContrato();
            repoInquilino = new RepoInquilino();
        }

        // GET: PagosController
        public ActionResult Index()
        {
            try
            {
                var lista = repo.All();
                return View(lista);
            }
            catch (Exception ex)
            {
                try
                {
                    string user = "Anónimo";
                    if (!String.IsNullOrEmpty(User.Identity.Name))
                    {
                        user = User.Identity.Name;
                    }
                    return Redirect(Request.Headers["referer"].FirstOrDefault());
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        // GET: PagosController/Details/5
        public ActionResult Details(int id)
        {
            try
            {
                var p = repo.Details(id);
                return View(p);

            }
            catch (Exception ex)
            {
                try
                {
                    string user = "Anónimo";
                    if (!String.IsNullOrEmpty(User.Identity.Name))
                    {
                        user = User.Identity.Name;
                    }
                    return Redirect(Request.Headers["referer"].FirstOrDefault());
                }
                catch (Exception)
                {
                    throw;
                }
            }
        }

        // GET: PagosController/Create
        public ActionResult Create(int id)
        {
            try
            {
                if (id > 0) 
                {
                    Contrato c = repoContrato.Details(id);
                    if (c.Id == 0)
                    {
                        TempData["msg"] = "No se encontró el Contrato. Intente nuevamente.";
                        return View();
                    }

                    ViewBag.Contrato = c;
                    return View();
                }
                ViewBag.Contratos = repoContrato.All();
                return View(); 
            }
            
                catch (Exception)
                {
                    throw;
                }
            

        }
/*
        // GET: PagosController/Inquilino/{dni}
        public ActionResult Inquilino(int dni)
        {
            PagoCreate pc = new PagoCreate();
            try
            {
                if (dni != null)
                {
                    Inquilino i = repoInquilino.Details(dni);
                    pc.Inquilino = i;
            
                }

                return Ok(pc); 
            }
                catch (Exception)
                {
                    throw;
                }
            

        }*/

        // POST: PagosController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(int id, Pago p)
        {

            try
            {
               
                    if (id > 0 && p.IdContrato == 0)
                    {
                        p.Id = 0;
                        p.IdContrato = id;
                    }

                        var res = repo.Put(p);
                        if (res > 0)
                        {
                            TempData["msg"] = "Pago cargado";
                            return RedirectToAction(nameof(Index));
                        }
                        else
                        {
                            TempData["msg"] = "Pago no cargado. Intente de nuevo.";
                            return RedirectToAction(nameof(Create));
                        }


            }
            
                catch (Exception)
                {
                    throw;
                }

        }

        // GET: PagosController/Edit/5
        public ActionResult Edit(int id)
        {
            try
            {
                Pago p = repo.Details(id);
                if (p.Id == 0)
                {
                    TempData["msg"] = "No se encontró el Pago. Intente nuevamente";
                    return RedirectToAction(nameof(Index));
                }

                return View(p);
            }
            
                catch (Exception)
                {
                    throw;
                }
        }

        // POST: PagosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, Pago p)
        {
            try
            {
                
                    var oldP = repo.Details(id);
                    if (oldP.Id == 0)
                    {
                        TempData["msg"] = "No se encontró el Pago. Intente nuevamente.";
                        return RedirectToAction(nameof(Index));
                    }

                    var res = repo.Edit(p);
                    if (res > 0)
                    {
                        TempData["msg"] = "Pago editado.";
                        return RedirectToAction(nameof(Index));
                    }
                    else
                    {
                        TempData["msg"] = "Cambios no guardados. Intente Nuevamente";
                        return RedirectToAction(nameof(Edit), new { id = id });
                    }
               
            }
            
                catch (Exception)
                {
                    throw;
                }
            
        }

        // GET: PagosController/Delete/5
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id)
        {
            try
            {
                Pago p = repo.Details(id);
                if (p.Id > 0)
                {
                    return View(p);
                }
                else
                {
                    TempData["msg"] = "El Pago " + id + " no existe en la base de datos.";
                    return RedirectToAction(nameof(Index));
                }
            }
                catch (Exception)
                {
                    throw;
                }
            
        }

        // POST: PagosController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Policy = "Administrador")]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                var res = repo.Delete(id);
                if (res > 0)
                {
                    TempData["msg"] = "Pago eliminado.";
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    TempData["msg"] = "Pago no encontrado. Vuelva a Intentar.";
                    return RedirectToAction(nameof(Delete), new { id = id });
                }
            }
            
                catch (Exception)
                {
                    throw;
                }
        }
    }
}
