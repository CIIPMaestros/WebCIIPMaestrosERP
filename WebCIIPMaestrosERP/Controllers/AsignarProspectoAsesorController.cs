using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebCIIPMaestrosERP.Controllers
{
    public class AsignarProspectoAsesorController : Controller
    {
        // GET: AsignarProspectoAsesor
        public ActionResult SeguimientoProspecto()
        {
            ViewBag.SituacionLaboral = SituacionLaboral();
            ViewBag.Cargo = Cargo();
            ViewBag.Nivel = Nivel();
            return View();
        }






















        //-------------------------------------------END POINT-------------------------------------------

        public List<SelectListItem> SituacionLaboral()
        {
            List<SelectListItem> SituacionLaboralList = new List<SelectListItem>();
            SituacionLaboralList.Add(new SelectListItem { Text = "StringData", Value = "StringData" });

            return SituacionLaboralList;
        }

        public List<SelectListItem> Cargo()
        {
            List<SelectListItem> CargoList = new List<SelectListItem>();
            CargoList.Add(new SelectListItem { Text = "StringData", Value = "StringData" });

            return CargoList;
        }

        public List<SelectListItem> Nivel()
        {
            List<SelectListItem> NivelList = new List<SelectListItem>();
            NivelList.Add(new SelectListItem { Text = "StringData", Value = "StringData" });

            return NivelList;
        }
    }
}