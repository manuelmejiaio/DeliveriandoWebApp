using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using DeliveriandoWebApp.Models;
using PagedList;



namespace DeliveriandoWebApp.Controllers
{
    public class HomeController : Controller
    {
        private DBModels.DefaultConnection db = new DBModels.DefaultConnection();

        public ActionResult Index(string searchString)
        {
            var result = from i in db.Restaurantes select i;

            if (!String.IsNullOrEmpty(searchString))
            {
                result = result.Where(s => s.Nombre.Contains(searchString));
            }

            if (result.Count() == 0)
            {
                ViewBag.MensageDeNoDisponible = "Restaurante aún no disponible en Deliveriando :(";
            }
            return View(result);
        }

        public ActionResult FAQ()
        {
            return View();
        }



    //[Authorize(Users = "mejiamanuel57@gmail.com, administrador")]

        public ActionResult Catalogo(string searchString)
        {
            var result = from i in db.Restaurantes select i;

            if(!String.IsNullOrEmpty(searchString))
            {
                result = result.Where(s => s.Nombre.Contains(searchString));
            }

            return View(result);
        }

        
    }
}
