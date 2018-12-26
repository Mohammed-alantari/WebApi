using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApi
{
    public class CustomAuthorize: AuthorizeAttribute
    {
     
        public override void OnAuthorization(AuthorizationContext filterContext)
        {

            if(filterContext.HttpContext.User.Identity.IsAuthenticated)
            {
                filterContext.Result = new RedirectResult("/AccountM/Login");
            }
        }

    }
}