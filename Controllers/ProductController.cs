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
    public class ProductController : Controller
    {
        private DBModels.DefaultConnection db = new DBModels.DefaultConnection();

        //
        // GET: /Product/


        public ActionResult Extra()
        {
            return View();
        }



        public ActionResult Index(int id)
        {
            Session["id"] = id; // para pasarle el id del restaurante, que luego usara el GetProduct.


            var nombreRestaurante = (from i in db.Restaurantes
                                     where i.ID == id
                                     select i).FirstOrDefault();
            ViewBag.nombre = nombreRestaurante.Nombre;

            return View();
        }

        public ActionResult List()
        {
            return View(db.Products.ToList());
        }

        public JsonResult GetProducts()
        {
            int id = (int)Session["id"]; 
            //var result = Json(db.Products.ToList() , JsonRequestBehavior.AllowGet);
            var result = Json(from i in db.Products.ToList() where i.IdRestaurante == id select i, JsonRequestBehavior.AllowGet);
            return result;
        }

        //
        // GET: /Product/Details/5

        public ActionResult Details(long id = 0)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        //
        // GET: /Product/Create

        public ActionResult Create()
        {
            return View();
        }

        //
        // POST: /Product/Create

        [HttpPost]
        public ActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Create");
            }

            return View(product);
        }

        //
        // GET: /Product/Edit/5

        public ActionResult Edit(long id = 0)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        //
        // POST: /Product/Edit/5

        [HttpPost]
        public ActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(product);
        }

        //
        // GET: /Product/Delete/5

        public ActionResult Delete(long id = 0)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        //
        // POST: /Product/Delete/5

        [HttpPost, ActionName("Delete")]
        public ActionResult DeleteConfirmed(long id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
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