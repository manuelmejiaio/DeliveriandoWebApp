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
    public class RestauranteController : Controller
    {
        private DBModels.DefaultConnection db = new DBModels.DefaultConnection();

        //
        // GET: /Restaurante/

        public ActionResult Index()
        {
            return View(db.Restaurantes.ToList());
        }

        //
        // GET: /Restaurante/Details/5

        public ActionResult Details(int id = 0)
        {
            RestauranteModels restaurantemodels = db.Restaurantes.Find(id);
            if (restaurantemodels == null)
            {
                return HttpNotFound();
            }
            return View(restaurantemodels);
        }

        //
        // GET: /Restaurante/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Restaurante/Create

        [HttpPost]
        public ActionResult Create(RestauranteModels restaurantemodels)
        {
            if (ModelState.IsValid)
            {
                db.Restaurantes.Add(restaurantemodels);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(restaurantemodels);
        }

        //
        // GET: /Restaurante/Edit/5

        public ActionResult Edit(int id = 0)
        {
            RestauranteModels restaurantemodels = db.Restaurantes.Find(id);
            if (restaurantemodels == null)
            {
                return HttpNotFound();
            }
            return View(restaurantemodels);
        }

        //
        // POST: /Restaurante/Edit/5

        [HttpPost]
        public ActionResult Edit(RestauranteModels restaurantemodels)
        {
            if (ModelState.IsValid)
            {
                db.Entry(restaurantemodels).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(restaurantemodels);
        }

        //
        // GET: /Restaurante/Delete/5

        public ActionResult Delete(int id = 0)
        {
            RestauranteModels restaurantemodels = db.Restaurantes.Find(id);
            if (restaurantemodels == null)
            {
                return HttpNotFound();
            }
            return View(restaurantemodels);
        }

        //
        // POST: /Restaurante/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(int id)
        {
            RestauranteModels restaurantemodels = db.Restaurantes.Find(id);
            db.Restaurantes.Remove(restaurantemodels);
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