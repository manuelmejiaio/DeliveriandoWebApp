using DeliveriandoWebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace DeliveriandoWebApp.Helper
{
    public static class SessionManager
    {
        private const string CurrentUserKey = "CurrentUser";
        public static string Username
        {
            get { return HttpContext.Current.User.Identity.Name; }
        }

        public static UserProfile CurrentMember
        {
            get
            {
                var context = HttpContext.Current;

                if (context.Session == null)
                    return null;

                if (context.Session[CurrentUserKey] == null)
                {
                    var db = new UsersContext();
                    context.Session[CurrentUserKey] = db.UserProfiles.FirstOrDefault(m => m.UserName == Username);
                    db.Dispose();
                }
                return context.Session[CurrentUserKey] as UserProfile;
            }
            set
            {
                HttpContext.Current.Session[CurrentUserKey] = value;
            }
        }
    }
}