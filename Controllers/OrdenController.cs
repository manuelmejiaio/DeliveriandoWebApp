﻿using System;
﻿using System.Collections.Generic;
using System.Data;
﻿using System.Data.Entity;
﻿using System.Linq;
﻿using System.Net.Http.Headers;
﻿using System.Web.Mvc;
using System.Web.Security;
﻿using System.Web.UI.WebControls;
﻿using System.Web.UI.WebControls.Expressions;
﻿using DeliveriandoWebApp.Mailers;
﻿using DeliveriandoWebApp.Models;
﻿using Microsoft.Ajax.Utilities;
﻿using WebGrease.Css.Extensions;
using System.Web.Script.Serialization;
using DeliveriandoWebApp.Helper;

namespace DeliveriandoWebApp.Controllers
{

    public class OrdenController : Controller
    {
        private readonly DBModels.DefaultConnection db = new DBModels.DefaultConnection();

        private DateTime tiempoLimite = DateTime.Now.AddMinutes(-265); // -242 son 4 horas y dos minutos (Esto es por el desfase del servidor)
        //
        // GET: /Orden/

        public ActionResult Index()
        {
            Response.AddHeader("Refresh", "12");
            return View(db.Ordenes.ToList());

        }

        [Authorize]
        public ActionResult CheckOut() //Da opcion de pagar con tarjeta o efectivo.
        {
            ViewBag.DireccionUsuario = SessionManager.CurrentMember.UserAddress;
            return View();
        }



        [HttpPost]
        public JsonResult SeleccionDelCheckOut(string formaDeEntrega, string formaDePago, string numeroDeTarjeta, string fechaDeExpiracionDeTarjeta, string direccionUsuario)
        {

            if (formaDeEntrega == "1") //Si es takeout
            {
                Session["FormaDeEntrega"] = "Takeout";
                Session["FormaDePago"] = null;
                Session["NumeroDeTarjeta"] = null;
                Session["FechaDeExpiracionDeTarjeta"] = null;
                Session["DireccionUsuario"] = direccionUsuario;
            }

            if (formaDeEntrega == "2" && formaDePago == "1") //Si es Delivery y tarjeta
            {
                Session["FormaDeEntrega"] = "Delivery";
                Session["FormaDePago"] = "Tarjeta";
                Session["NumeroDeTarjeta"] = numeroDeTarjeta;
                Session["FechaDeExpiracionDeTarjeta"] = fechaDeExpiracionDeTarjeta;
                Session["DireccionUsuario"] = direccionUsuario;
            }

            if (formaDeEntrega == "2" && formaDePago == "2") //Si es Delivery y efectivo
            {
                Session["FormaDeEntrega"] = "Delivery";
                Session["FormaDePago"] = "Efectivo";
                Session["NumeroDeTarjeta"] = null;
                Session["FechaDeExpiracionDeTarjeta"] = null;
                Session["DireccionUsuario"] = direccionUsuario;
            }


            Create();//ve al metodo Create()
            string result = "ok";
            return Json(result);// para dar respuesta al ajax post
        }


        public void OrderConfirmationMail()
        {
            //Para obtener el usuario loggeado
            var usuarioSelecionado = SessionManager.CurrentMember;

            //Buscame las ordenes-productos del usuario loggeado que esten falsas  en OrdenNotificadaPorCorreo
            var products = (from i in db.Ordenes.ToList()
                            where i.product.IdUsuario == usuarioSelecionado.UserId && i.OrdenNotificadaPorCorreo == false
                            select i).ToList();

            //----------------------------------------------------------------------------------Para enviar correo.
            IUserMailer mailer = new UserMailer();
            var message = mailer.ConfirmationOrder(usuarioSelecionado.UserName, products);
            message.Send();


            // a cada orden se le pondra como enviada por correo.
            foreach (var models in products)
            {
                models.OrdenNotificadaPorCorreo = true;
            }
            db.SaveChanges();
        }


        [HttpPost]
        //En esta funcion se buscan todas los ordenes-productos con el mismo GUID, para luego ser utilizado en el JSParaRestaurantes.js o JSParaClientes.js
        public string filtroDeGuid(int id)
        {
            var orderGuid = db.Ordenes.FirstOrDefault(i => i.id == id).IdOrdenesUnicas; //Para obtener el GUID con ese id
            var orden = db.Ordenes.Where(i => i.IdOrdenesUnicas == orderGuid).ToList(); //Para listar todas las ordenes-productos con ese GUID.

        
            string json = new System.Web.Script.Serialization.JavaScriptSerializer().Serialize(orden);


            return json;
        }

        //Recibe desde la aplicacion de Android para marcar como entregada.
        public string OrdenEntregada(int idMotorista, int idRestaurante)
        {
            var orden = db.Ordenes.Where(i => i.product.IdMotorista == idMotorista && i.product.IdRestaurante == idRestaurante && i.OrdenEstatus == "ENVIADA" && i.product.Fecha > tiempoLimite).ToList();

            foreach (var items in orden)
            {
                items.OrdenEstatus = "ENTREGADA";
            }
            db.SaveChanges();
            return("1");
        }


        [HttpPost]
        //Recibe de RestaurantView, Funcion para marcar la orden como Enviada/Lista, decir con cual motorista se entrega y enviar un email de que la orden fue enviada/lista
        public JsonResult SendOrder(int idOrden, int idMotor)
        {
            //Para guardar la orden como enviada.
            var orderGuid = db.Ordenes.FirstOrDefault(i => i.id == idOrden).IdOrdenesUnicas;//Para obtener el GUID con ese id
            var orden = db.Ordenes.Where(i => i.IdOrdenesUnicas == orderGuid).ToList(); //Para listar todas las ordenes-productos con ese GUID.
            var tipoDeCorreo = "null";

            foreach (var models in orden) // a cada producto de esa orden se le pondra como enviada si tiene le mismo GUID
            {
                //Seguridad de tarjeta
                if (models.product.NumeroDeTarjeta != null)
                {
                    string tarjeta = models.product.NumeroDeTarjeta;
                    models.product.NumeroDeTarjeta = "************" + tarjeta[12] + tarjeta[13] + tarjeta[14] + tarjeta[15];
                    models.product.FechaDeExpiracionDeTarjeta = "****";
                }

                models.product.IdMotorista = idMotor;

                if (models.product.FormaDeEntrega == "Takeout")
                {
                    models.OrdenEstatus = "LISTA";
                    tipoDeCorreo = "takeout";
                }
                else
                {
                    models.OrdenEstatus = "ENVIADA";
                    tipoDeCorreo = "delivery";
                }
            }
            db.SaveChanges();

            //----------------------------------------------------------------------------------Para enviar correo.
            //Para buscar el email del usuario que vamos enviar el pedido
            var usuarioSelecionado = db.Ordenes.FirstOrDefault(i => i.id == idOrden).product.IdUsuario;
            var userContext = new UsersContext();
            var userEmail = userContext.UserProfiles.FirstOrDefault(u => u.UserId == usuarioSelecionado).UserName;


            //Funcion de MvcMailer
            IUserMailer mailer = new UserMailer();
            if (tipoDeCorreo == "takeout")
            {
                var message = mailer.ComeAndGetIt(userEmail);
                message.Send();
            }
            else
            {
                var message = mailer.OnTheWay(userEmail);
                message.Send();
            }

            string result = "ok";
            return Json(result);// para dar respuesta al ajax post
        }

        [HttpPost]
        public JsonResult ComentarOrden(int idOrden, string comentario)
        {
            var orderGuid = db.Ordenes.FirstOrDefault(i => i.id == idOrden).IdOrdenesUnicas;//Para obtener el GUID con ese id
            
            //Para buscar el email al restaurante que vamos a mandar el comentario
            var restaurante = db.Ordenes.FirstOrDefault(i => i.id == idOrden).product.IdRestaurante;
            var userContext = new UsersContext();
            var userEmail = userContext.UserProfiles.FirstOrDefault(u => u.RestauranteID == restaurante).UserName;

            //----------------------------------------------------------------------------------Para enviar correo.
            IUserMailer mailer = new UserMailer();
            var message = mailer.CommentOrder(userEmail, orderGuid.ToString(), comentario);
            message.Send();

            string result = "ok";
            return Json(result);// para dar respuesta al ajax post
        }

        [HttpPost]
        //Recibe de JSParaRestaurante.js, Funcion para cancelar una orden.
        public JsonResult Cancel(int idOrden, string motivo)
        {
            //Para obtener el usuario loggeado
            var usuarioSelecionado = SessionManager.CurrentMember;

            var orderGuid = db.Ordenes.FirstOrDefault(i => i.id == idOrden).IdOrdenesUnicas; //para buscar el GUID de esa orden.
            var orden = db.Ordenes.Where(i => i.IdOrdenesUnicas == orderGuid).ToList(); // para traer todas con el mismo GUID

            foreach (var models in orden) // a cada producto de esa orden se le pondra como enviada si tiene le mismo GUID
            {
                models.MotivoDeCancelacion = motivo;
                models.OrdenBorradaPor = usuarioSelecionado.UserName; //para anotar quien la cancelo.
                models.OrdenEstatus = "CANCELADA";
            }

            db.SaveChanges();

            //----------------------------------------------------------------------------------Para enviar correo.
            //Para buscar el email del usuario que vamos enviar el pedido
            var usuarioSelect = db.Ordenes.FirstOrDefault(i => i.id == idOrden).product.IdUsuario;
            var userContext = new UsersContext();
            var userEmail = userContext.UserProfiles.FirstOrDefault(u => u.UserId == usuarioSelect).UserName;

            //Nombre del restaurante
            var restauranteNombre = db.Ordenes.FirstOrDefault(i => i.IdOrdenesUnicas == orderGuid).product.NombreRestaurante;

            IUserMailer mailer = new UserMailer();
            var message = mailer.CancelOrder(userEmail,restauranteNombre, orderGuid.ToString(), motivo);
            message.Send();


            string result = "ok";
            return Json(result);// para dar respuesta al ajax post
        }




        [Authorize]
        public ActionResult ClienteView() // view de ordernes para los clientes
        {
            // Response.AddHeader("Refresh", "15");
            //Para obtener el usuario loggeado
            var usuarioSelecionado = SessionManager.CurrentMember;

            //Para mostrar las ordernes solo del usuario loggeado y que deje de mostrar despues del tiempoLimite...
            var result = (from i in db.Ordenes.ToList() where i.product.IdUsuario == usuarioSelecionado.UserId && i.product.Fecha > tiempoLimite select i);


            var coordenadas = new List<CoordenadasModels>();

            foreach (var item in result)
            {
                if (item.OrdenEstatus == "ENVIADA") // Para que solo traiga las que estan marcadas como ENVIADA
                {
                    coordenadas.Add(db.Coordenadas.Where(x => x.IdRestaurante == item.product.IdRestaurante && x.IdMotorista == item.product.IdMotorista).ToList().LastOrDefault());
                }
            }


            ViewBag.Coordenadas = coordenadas;

            return View(result);
        }


        [Authorize]
        public ActionResult AllClienteView() // view de todas las ordernes para los clientes
        {
            //Para obtener el usuario loggeado
            var usuarioSelecionado = SessionManager.CurrentMember;

            //Para mostrar todas las ordernes solo del usuario loggeado 
            var result = (from i in db.Ordenes.ToList() where i.product.IdUsuario == usuarioSelecionado.UserId select i);

            return View(result);

        }


        [Authorize]
        public ActionResult RestauranteView() // view de ordernes para los restaurantes
        {
            //Para obtener restaurante loggeado
            var usuarioSelecionado = SessionManager.CurrentMember;

            ViewBag.NombreRest = db.Restaurantes.FirstOrDefault(i => i.ID == usuarioSelecionado.RestauranteID).Nombre; // para enviar el nombre del restaurante al view
            var tiempoLimiteAlerta = DateTime.Now.AddMinutes(-243);// 3 minutos 
  

            if (usuarioSelecionado.RestauranteID > 0) // Los administradores de restaurantes tienen RestauranteID mayores que 0
            {
                //Alerta por si se pasa del tiempo limite a cada orden.
                var alerta = (from i in db.Ordenes.ToList() where i.product.IdRestaurante == usuarioSelecionado.RestauranteID && (i.OrdenEstatus == "NO ENVIADA" || i.OrdenEstatus == "NO LISTA") && i.product.Fecha < tiempoLimiteAlerta && i.AlertaDeTiempo == false select i);
                foreach (var models in alerta)
                {
                    models.AlertaDeTiempo = true;
                }
                db.SaveChanges();


                //Busca los productos que no han sido enviado/listo y que tampoco han sido cancelados de ese restaurante
                var result = (from i in db.Ordenes.ToList() where i.product.IdRestaurante == usuarioSelecionado.RestauranteID && (i.OrdenEstatus == "NO ENVIADA" || i.OrdenEstatus == "NO LISTA") select i);
                return View(result);
            }


            return RedirectToAction("Index", "Home");
        }


        public ActionResult PartialViewCliente() // view de ordernes para los clientes
        {
            // Response.AddHeader("Refresh", "15");
            //Para obtener el usuario loggeado
            var usuarioSelecionado = SessionManager.CurrentMember;

            //Para mostrar las ordernes solo del usuario loggeado y que deje de mostrar despues del tiempoLimite...
            var result = (from i in db.Ordenes.ToList() where i.product.IdUsuario == usuarioSelecionado.UserId && i.product.Fecha > tiempoLimite select i);


            var coordenadas = new List<CoordenadasModels>();

            foreach (var item in result)
            {
                if (item.OrdenEstatus == "ENVIADA") // Para que solo traiga las que estan marcadas como ENVIADA
                {
                    coordenadas.Add(db.Coordenadas.Where(x => x.IdRestaurante == item.product.IdRestaurante && x.IdMotorista == item.product.IdMotorista).ToList().LastOrDefault());
                }
            }


            ViewBag.Coordenadas = coordenadas;

            return View(result);
        }


        public ActionResult PartialViewRestaurante()
        {
            var usuarioSelecionado = SessionManager.CurrentMember;
            ViewBag.NombreRest = db.Restaurantes.FirstOrDefault(i => i.ID == usuarioSelecionado.RestauranteID).Nombre; // para enviar el nombre del restaurante al view
            var tiempoLimiteAlerta = DateTime.Now.AddMinutes(-243);// 3 minutos 
            // Response.AddHeader("Refresh", "25");


            if (usuarioSelecionado.RestauranteID > 0) // Los administradores de restaurantes tienen RestauranteID mayores que 0
            {
                //Alerta por si se pasa del tiempo limite a cada orden.
                var alerta = (from i in db.Ordenes.ToList() where i.product.IdRestaurante == usuarioSelecionado.RestauranteID && (i.OrdenEstatus == "NO ENVIADA" || i.OrdenEstatus == "NO LISTA") && i.product.Fecha < tiempoLimiteAlerta && i.AlertaDeTiempo == false select i);
                foreach (var models in alerta)
                {
                    models.AlertaDeTiempo = true;
                }
                db.SaveChanges();

                var result = (from i in db.Ordenes.ToList() where i.product.IdRestaurante == usuarioSelecionado.RestauranteID && (i.OrdenEstatus == "NO ENVIADA" || i.OrdenEstatus == "NO LISTA") select i);
                return View(result);
            }


            return RedirectToAction("Index", "Home");
        }

        public ActionResult PartialViewDeliveriesRestaurante()
        {
            //Para obtener el usuario loggeado
            var usuarioSelecionado = SessionManager.CurrentMember;
            ViewBag.NombreRest = db.Restaurantes.FirstOrDefault(i => i.ID == usuarioSelecionado.RestauranteID).Nombre; // para enviar el nombre del restaurante al view

            var coordenadas = new List<CoordenadasModels>();

            for (int i = 0; i <= 10; i++)
            {
                var coordenadasCorrectas = (db.Coordenadas.Where(x => x.IdRestaurante == usuarioSelecionado.RestauranteID && x.IdMotorista == i).ToList().LastOrDefault());
                if (coordenadasCorrectas != null) // para que solo acepte las que no sean null.
                {
                    coordenadas.Add(coordenadasCorrectas);
                }
                //coordenadas.Add(db.Coordenadas.Where(x => x.IdRestaurante == usuarioSelecionado.RestauranteID && x.IdMotorista == i ).ToList().LastOrDefault());
            }

            ViewBag.Coordenadas = coordenadas;
            return View();
        }


        [Authorize]
        public ActionResult AllRestauranteView() // todas las ordenes desde su inicio.
        {
            var usuarioSelecionado = SessionManager.CurrentMember; //Para obtener restaurante loggeado
            ViewBag.NombreRest = db.Restaurantes.FirstOrDefault(i => i.ID == usuarioSelecionado.RestauranteID).Nombre; // para enviar el nombre del restaurante al view

            if (usuarioSelecionado.RestauranteID > 0) // Los administradores de restaurantes tienen RestauranteID mayores que 0
            {
                var result = (from i in db.Ordenes.ToList() where i.product.IdRestaurante == usuarioSelecionado.RestauranteID && i.OrdenEstatus != "CANCELADA" select i);
                return View(result);
            }


            return RedirectToAction("Index", "Home");

        }

        [Authorize]
        public ActionResult DeliveriesRestauranteView()
        {
            //Para obtener el usuario loggeado
            var usuarioSelecionado = SessionManager.CurrentMember;
            ViewBag.NombreRest = db.Restaurantes.FirstOrDefault(i => i.ID == usuarioSelecionado.RestauranteID).Nombre; // para enviar el nombre del restaurante al view

            var coordenadas = new List<CoordenadasModels>();

            for (int i = 0; i <= 10; i++ )
            {
                var coordenadasCorrectas = (db.Coordenadas.Where(x => x.IdRestaurante == usuarioSelecionado.RestauranteID && x.IdMotorista == i).ToList().LastOrDefault());
                if (coordenadasCorrectas != null) // para que solo acepte las que no sean null.
                {
                    coordenadas.Add(coordenadasCorrectas);
                }
                //coordenadas.Add(db.Coordenadas.Where(x => x.IdRestaurante == usuarioSelecionado.RestauranteID && x.IdMotorista == i ).ToList().LastOrDefault());
            }

            ViewBag.Coordenadas = coordenadas;
            return View();
        }

        [Authorize]
        public ActionResult AllCancelOrdersRestauranteView() // todas las ordenes desde su inicio.
        {
            var usuarioSelecionado = SessionManager.CurrentMember; //Para obtener restaurante loggeado
            ViewBag.NombreRest = db.Restaurantes.FirstOrDefault(i => i.ID == usuarioSelecionado.RestauranteID).Nombre; // para enviar el nombre del restaurante al view

            if (usuarioSelecionado.RestauranteID > 0) // Los administradores de restaurantes tienen RestauranteID mayores que 0
            {
                var result = (from i in db.Ordenes.ToList() where i.product.IdRestaurante == usuarioSelecionado.RestauranteID && i.OrdenEstatus == "CANCELADA" select i);
                return View(result);
            }


            return RedirectToAction("Index", "Home");
        }


        [HttpPost]
        //Aqui llega el Json POST de index.js
        public JsonResult PreOrder(List<OrdenModels> ordenes, double subtotal)
        {
            //// estos session iran para el Create de este controller.
            Session["ordenesListas"] = ordenes;
            Session["subTotalDeLaOrden"] = subtotal;
            Session["ITBIS"] = subtotal * 0.18;
            Session["TotalFinal"] = subtotal + (double)Session["ITBIS"];

            string result = "ok";
            return Json(result);// para dar respuesta al ajax post
        }




        [HttpPost]
        //Este el metodo donde se crean y se le agregan valores a los atributos del OrdenModels;
        public void Create(List<OrdenModels> order = null, double subtotal = 0, double itbis = 0, double finalTotal = 0) // estos parametros no reciben nada solo es para declararlos.
        {
            //Para parasarle el objeto que tiene Session de PreOrder
            order = (List<OrdenModels>)Session["ordenesListas"];
            subtotal = (double)Session["subTotalDeLaOrden"];
            itbis = (double)Session["ITBIS"];
            finalTotal = (double)Session["TotalFinal"];

            //Para obtener el usuario loggeado
            var usuarioSelecionado = SessionManager.CurrentMember;

            //Para guardar el id del usuario loggeado cuando crea una orden.
            var userId = usuarioSelecionado.UserId;
            order.ForEach(models => models.product.IdUsuario = userId);

            //Para guardar el nombre completo del usuario cuando crea una orden.
            order.ForEach(models => models.product.NombreUsuario = usuarioSelecionado.UserFullName);

            //Para guardar el numero de telefono del usuario loggeado cuando crea una orden.
            var userPhone = usuarioSelecionado.UserPhone;
            order.ForEach(models => models.product.NumeroUsuario = userPhone);

            // Para guardar el nombre del restaurante al cual pedimos la orden.
            var orderRestauranteId = order.FirstOrDefault().product.IdRestaurante;
            var nombreRestauranteOrden = db.Restaurantes.FirstOrDefault(x => x.ID == orderRestauranteId).Nombre;
            order.ForEach(models => models.product.NombreRestaurante = nombreRestauranteOrden);

            //Para guardar la fecha en que fue creada
            order.ForEach(models => models.product.Fecha = DateTime.Now.AddHours(-4));

            //Para guardar el GUID mismo para varias ordenes.
            Guid id = Guid.NewGuid();
            order.ForEach(i => i.IdOrdenesUnicas = id);

            //Para guardar el subtotal,ITBIS y finalTotal de la orden.
            order.ForEach(model => model.total = subtotal);
            order.ForEach(model => model.ITBIS = itbis);
            order.ForEach(model => model.finalTotal = Math.Round(finalTotal - (double)0.005, 2));

            //Para guardar la forma de pago , entrega del checkout y direccion.
            order.ForEach(models => models.product.FormaDeEntrega = (string)Session["FormaDeEntrega"]);
            order.ForEach(models => models.product.FormaDePago = (string)Session["FormaDePago"]);
            order.ForEach(models => models.product.NumeroDeTarjeta = (string)Session["NumeroDeTarjeta"]);
            order.ForEach(models => models.product.FechaDeExpiracionDeTarjeta = (string)Session["FechaDeExpiracionDeTarjeta"]);
            order.ForEach(models => models.product.DireccionUsuario = (string)Session["DireccionUsuario"]);


            //Para decir que la orden nueva no ha sido enviada o no esta lista (Obvio, pero necesario para el clientView)
            foreach (var models in order)
            {

                if (models.product.FormaDeEntrega == "Takeout")
                {
                    models.OrdenEstatus = "NO LISTA";
                }
                else
                {
                    models.OrdenEstatus = "NO ENVIADA";
                }
            }



            if (ModelState.IsValid)
            {
                foreach (var ordenModel in order)
                {
                    db.Ordenes.Add(ordenModel);
                }

                db.SaveChanges();
            }

            Session["ordenesListas"] = null; //Aqui muere la variable session
            OrderConfirmationMail();
            //return RedirectToAction("OrderConfirmationMail", "Orden"); 
        }

        //
        // GET: /Orden/Edit/5

        public ActionResult Edit(int id = 0)
        {
            OrdenModels ordenmodels = db.Ordenes.Find(id);
            if (ordenmodels == null)
            {
                return HttpNotFound();
            }
            return View(ordenmodels);
        }

        //
        // POST: /Orden/Edit/5

        [HttpPost]
        public ActionResult Edit(OrdenModels ordenmodels)
        {
            if (ModelState.IsValid)
            {
                db.Entry(ordenmodels).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(ordenmodels);
        }



        protected override void Dispose(bool disposing)
        {
            db.Dispose();
            base.Dispose(disposing);
        }
    }
}
