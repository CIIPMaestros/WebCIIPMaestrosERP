using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebCIIPMaestrosERP.Seguridad
{
    public class Acceder : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext) 
        {

            var usuario = HttpContext.Current.Session["Usuario"];

            if (usuario == null)
            {
                filterContext.Result = new RedirectResult("~/Login/Index");
            }


            base.OnActionExecuting(filterContext);


        }
    }
}