using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebCIIPMaestrosERP.Models;

namespace WebCIIPMaestrosERP.Controllers
{
    public class DocenteSeguimientoController : Controller
    {
        // GET: DocenteSeguimiento
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult AddDocenteSeguimiento()
        {

            try {

                return View();
            }
            catch(Exception ex)
            {

                return View();


            }
            
        
        }


    }
}