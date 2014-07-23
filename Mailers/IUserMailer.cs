using Mvc.Mailer;
﻿using DeliveriandoWebApp.Models;
﻿using System.Collections.Generic;

namespace DeliveriandoWebApp.Mailers
{ 
    public interface IUserMailer
    {
        MvcMailMessage Register(string email, string userRegister);
        MvcMailMessage OnTheWay( string email);
        MvcMailMessage ComeAndGetIt(string email);
        MvcMailMessage ConfirmationOrder(string email, List<DeliveriandoWebApp.Models.OrdenModels> products);
        MvcMailMessage CommentOrder(string email, string orderGuid, string comment);
        MvcMailMessage CancelOrder(string email, string restaurantName, string orderGuid, string reason);
        MvcMailMessage PasswordReset();
	}
}