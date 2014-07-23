using Mvc.Mailer;
﻿using DeliveriandoWebApp.Models;
﻿using System.Collections.Generic;
using System.Linq;

namespace DeliveriandoWebApp.Mailers
{ 
    public class UserMailer : MailerBase, IUserMailer 	
	{
		public UserMailer()
		{
			MasterName="_Layout";
		}

        public virtual MvcMailMessage OnTheWay(string email) // si quieres cambiar un parametro, tambien debes es IUserMailer.cs
		{
			//ViewBag.Data = someObject;
			return Populate(x =>
			{
				x.Subject = "En camino !";
				x.ViewName = "OnTheWay";
				x.To.Add( email);
			});
		}

        public virtual MvcMailMessage ComeAndGetIt(string email) // si quieres cambiar un parametro, tambien debes es IUserMailer.cs
        {
            //ViewBag.Data = someObject;
            return Populate(x =>
            {
                x.Subject = "Orden lista !";
                x.ViewName = "ComeAndGetIt";
                x.To.Add(email);
            });
        }

        public virtual MvcMailMessage ConfirmationOrder(string email, List<DeliveriandoWebApp.Models.OrdenModels> products) // si quieres cambiar un parametro, tambien debes es IUserMailer.cs
        {
            ViewBag.Products = products.ToList();

            return Populate(x =>
            {
                x.Subject = "Deliveriando, confirmación de orden !";
                x.ViewName = "ConfirmationOrder";
                x.To.Add(email);
            });
        }

        public virtual MvcMailMessage CommentOrder(string email, string orderGuid, string comment) // si quieres cambiar un parametro, tambien debes es IUserMailer.cs
        {
            ViewBag.orderGuid = orderGuid;
            ViewBag.comment = comment;   

            return Populate(x =>
            {
                x.Subject = "Ha recibido un nuevo comentario !";
                x.ViewName = "CommentOrder";
                x.To.Add(email);
            });
        }

        public virtual MvcMailMessage CancelOrder(string email, string restaurantName, string orderGuid, string reason) // si quieres cambiar un parametro, tambien debes es IUserMailer.cs
        {
            ViewBag.restaurantName = restaurantName;
            ViewBag.orderGuid = orderGuid;
            ViewBag.reason = reason;


            return Populate(x =>
            {
                x.Subject = "Deliveriando, orden cancelada !";
                x.ViewName = "CancelOrder";
                x.To.Add(email);
            });
        }


        public virtual MvcMailMessage Register(string email, string userRegister ) // si quieres cambiar un parametro, tambien debes es IUserMailer.cs
        {
            ViewBag.userReg = userRegister;
           
            return Populate(x =>
            {
                x.Subject = "Gracias por registrarte !";
                x.ViewName = "Register";
                x.To.Add(email);
            });
        }
 
		public virtual MvcMailMessage PasswordReset()
		{
			//ViewBag.Data = someObject;
			return Populate(x =>
			{
				x.Subject = "PasswordReset";
				x.ViewName = "PasswordReset";
				x.To.Add("some-email@example.com");
			});
		}
 	}
}