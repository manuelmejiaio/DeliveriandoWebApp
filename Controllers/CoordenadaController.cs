using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using DeliveriandoWebApp.Models;

namespace DeliveriandoWebApp.Controllers
{
    public class CoordenadaController : Controller
    {
        private  DBModels.DefaultConnection db = new DBModels.DefaultConnection();

        //
        // GET: /Coordenada/


        public ActionResult Index()
        {
            return View(db.Coordenadas.ToList());
        }

        //
        // GET: /Coordenada/Details/5

        public ActionResult Details(int id = 0)
        {
            CoordenadasModels coordenadasmodels = db.Coordenadas.Find(id);
            if (coordenadasmodels == null)
            {
                return HttpNotFound();
            }
            return View(coordenadasmodels);
        }

        //
        // GET: /Coordenada/Create

        //public ActionResult Create()
        //{
        //    return View();
        //}

        //
        // POST: /Coordenada/Create

        //[HttpPost]
        //public ActionResult Create(CoordenadasModels coordenadasmodels)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Coordenadas.Add(coordenadasmodels);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(coordenadasmodels);
        //}



        [HttpGet]

        public string Create(string longitud, string latitud, int idMotorista, int idRestaurante)
        {
            CoordenadasModels coor = new CoordenadasModels();

            coor.Latitud = latitud;
            coor.Longitud = longitud;
            coor.Fecha = DateTime.Now;
            coor.IdMotorista = idMotorista;
            coor.IdRestaurante = idRestaurante;
            db.Coordenadas.Add(coor);
            db.SaveChanges();
            
            return "1" ;
        }

        //
        // GET: /Coordenada/Edit/5

        public ActionResult Edit(int id = 0)
        {
            CoordenadasModels coordenadasmodels = db.Coordenadas.Find(id);
            if (coordenadasmodels == null)
            {
                return HttpNotFound();
            }
            return View(coordenadasmodels);
        }

        //
        // POST: /Coordenada/Edit/5

        [HttpPost]
        public ActionResult Edit(CoordenadasModels coordenadasmodels)
        {
            if (ModelState.IsValid)
            {
                db.Entry(coordenadasmodels).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(coordenadasmodels);
        }

        //
        // GET: /Coordenada/Delete/5

        public ActionResult Delete(int id = 0)
        {
            CoordenadasModels coordenadasmodels = db.Coordenadas.Find(id);
            if (coordenadasmodels == null)
            {
                return HttpNotFound();
            }
            return View(coordenadasmodels);
        }

        //
        // POST: /Coordenada/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            CoordenadasModels coordenadasmodels = db.Coordenadas.Find(id);
            db.Coordenadas.Remove(coordenadasmodels);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}