using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebCIIPMaestrosERP.Models;
using WebCIIPMaestrosERP.Seguridad;

namespace WebCIIPMaestrosERP.Controllers
{
    [Acceder]
    public class MaeTutorController : Controller
    {
        // GET: MaeTutor
        public ActionResult Index()
        {

            llenarCurso();

            ViewBag.ListaEstado = ListaEstado;

            
            List<MaeTutorCLS> ListaTutores = null;

            using (var db = new DB_WebCIIPEntitiesERP())
            {
                ListaTutores = (from tutor in db.MAE_TUTOR
                                join tablas in db.MAE_TABLAS
                                on tutor.TUT_ACTIVO equals tablas.ID.ToString()
                                where tablas.COD_TABLA == "ACT"
                                orderby tutor.TUT_ID

                                select new MaeTutorCLS
                                {
                                    TUT_ID = tutor.TUT_ID,
                                    TUT_NOMBRES = tutor.TUT_NOMBRES,
                                    TUT_APELLIDOS = tutor.TUT_APELLIDOS,
                                    TUT_EMAIL = tutor.TUT_EMAIL,
                                    TUT_FOTO = tutor.TUT_FOTO,
                                    TUT_ACTIVO = tutor.TUT_ACTIVO,
                                    TUT_ESTADO = tablas.DESCRIPCION

                                }).ToList();
            }

            return View(ListaTutores);
        }

        public int ActivarDesactivar(int id, string estado)
        {
            int nregistrosAfectados = 0;
            try
            {
                using (var db = new DB_WebCIIPEntitiesERP())
                {
                    MAE_TUTOR oMAE_TUTOR = db.MAE_TUTOR.Where(p => p.TUT_ID == id).First();
                    oMAE_TUTOR.TUT_ACTIVO = estado;
                    nregistrosAfectados = db.SaveChanges();
                }

            }
            catch (Exception ex)
            {

                nregistrosAfectados = 0;
            }

            return nregistrosAfectados;
        }

        public JsonResult recuperarDatos(int IdTutor)
        {
            MaeTutorCLS oMaeTutorCLS = new MaeTutorCLS();
            using (var db = new DB_WebCIIPEntitiesERP())
            {
                MAE_TUTOR oMAE_TUTOR = db.MAE_TUTOR.Where(p => p.TUT_ID == IdTutor).First();

                oMaeTutorCLS.TUT_NOMBRES = oMAE_TUTOR.TUT_NOMBRES;
                oMaeTutorCLS.TUT_APELLIDOS = oMAE_TUTOR.TUT_APELLIDOS;
                oMaeTutorCLS.TUT_EMAIL = oMAE_TUTOR.TUT_EMAIL;
                oMaeTutorCLS.TUT_TELEFONO = oMAE_TUTOR.TUT_TELEFONO;
                //oMaeTutorCLS.TUT_ACTIVO = oMAE_TUTOR.TUT_ACTIVO;


                oMaeTutorCLS.extension = Path.GetExtension(oMAE_TUTOR.TUT_FOTO);
                oMaeTutorCLS.fotoCadena = Convert.ToBase64String(oMAE_TUTOR.FOTO);

            }

            return Json(oMaeTutorCLS, JsonRequestBehavior.AllowGet);
        }


        List<SelectListItem> ListaEstado;
        private void llenarCurso()
        {
            using (var db = new DB_WebCIIPEntitiesERP())
            {

                ListaEstado = (from tablas in db.MAE_TABLAS
                               where tablas.COD_TABLA == "ACT"
                               select new SelectListItem
                               {
                                   Text = tablas.DESCRIPCION,
                                   Value = tablas.ID.ToString()

                               }).ToList();
            }

        }

        public ActionResult Filtro(string BuscarVarios)
        {

            List<MaeTutorCLS> listaTutores = new List<MaeTutorCLS>();
            using (var db = new DB_WebCIIPEntitiesERP())
            {
                if (BuscarVarios == null)
                    listaTutores = (from tutores in db.MAE_TUTOR
                                    join tablas in db.MAE_TABLAS
                                    on tutores.TUT_ACTIVO equals tablas.ID.ToString()
                                    where tablas.COD_TABLA == "ACT" orderby tutores.TUT_ID
                                    select new MaeTutorCLS
                                   {
                                       TUT_ID = tutores.TUT_ID,
                                       TUT_NOMBRES = tutores.TUT_NOMBRES,
                                       TUT_APELLIDOS = tutores.TUT_APELLIDOS,
                                       TUT_EMAIL = tutores.TUT_EMAIL,
                                       TUT_TELEFONO = tutores.TUT_TELEFONO,
                                       TUT_ACTIVO = tutores.TUT_ACTIVO,
                                       TUT_ESTADO = tablas.DESCRIPCION
                                   }).ToList();
                else
                    listaTutores = (from tutores in db.MAE_TUTOR
                                    join tablas in db.MAE_TABLAS
                                    on tutores.TUT_ACTIVO equals tablas.ID.ToString()
                                    where tablas.COD_TABLA == "ACT"
                                    where   tutores.TUT_NOMBRES.Contains(BuscarVarios) ||
                                            tutores.TUT_APELLIDOS.Contains(BuscarVarios)
                                    orderby tutores.TUT_ID

                                    select new MaeTutorCLS
                                    {
                                        TUT_ID = tutores.TUT_ID,
                                        TUT_NOMBRES = tutores.TUT_NOMBRES,
                                        TUT_APELLIDOS = tutores.TUT_APELLIDOS,
                                        TUT_EMAIL = tutores.TUT_EMAIL,
                                        TUT_TELEFONO = tutores.TUT_TELEFONO,
                                        TUT_ACTIVO = tutores.TUT_ACTIVO,
                                        TUT_ESTADO = tablas.DESCRIPCION
                                    }).ToList();
            }
            return PartialView("_Index", listaTutores);
        }

        public string GuardarNuevoControladorParcial(MaeTutorCLS oMaeTutorCLS, HttpPostedFileBase foto, int accion)
        {

            string rpta = "";


            if (!ModelState.IsValid && accion == -1)
            {

                string valor;
                var query = (from state in ModelState.Values
                             from error in state.Errors
                             select error.ErrorMessage).ToList();
                rpta += "<ul class='list-group'>";
                foreach (var item in query)
                {
                    valor = item.Contains("Foto").ToString();

                    //if (valor != null && accion != -1)
                    //{
                        
                    //}
                    //else {
                        rpta += "<li class='list-group-item'>" + item + "</li>";
                    ///}
                    

                }
                rpta += "</ul>";
            }
            else
            {

                byte[] fotoBD = null;
                if (foto != null)
                {
                    BinaryReader lector = new BinaryReader(foto.InputStream);
                    fotoBD = lector.ReadBytes((int)foto.ContentLength);
                }

                using (var db = new DB_WebCIIPEntitiesERP())
                {

                    if (accion.Equals(-1))
                    {

                        MAE_TUTOR oMaeTutor = new MAE_TUTOR();
                        oMaeTutor.TUT_NOMBRES = oMaeTutorCLS.TUT_NOMBRES;
                        oMaeTutor.TUT_APELLIDOS = oMaeTutorCLS.TUT_APELLIDOS;
                        oMaeTutor.TUT_EMAIL = oMaeTutorCLS.TUT_EMAIL;
                        oMaeTutor.TUT_TELEFONO = oMaeTutorCLS.TUT_TELEFONO;
                        oMaeTutor.TUT_FOTO = oMaeTutorCLS.TUT_FOTO;
                        oMaeTutor.TUT_ACTIVO = "1";
                        oMaeTutor.FOTO = fotoBD;
                        db.MAE_TUTOR.Add(oMaeTutor);
                        rpta = db.SaveChanges().ToString();


                    }
                    else//modificar
                    {

                        MAE_TUTOR oMAE_TUTOR = db.MAE_TUTOR.Where(p => p.TUT_ID == accion).First();

                        oMAE_TUTOR.TUT_NOMBRES = oMaeTutorCLS.TUT_NOMBRES;
                        oMAE_TUTOR.TUT_APELLIDOS = oMaeTutorCLS.TUT_APELLIDOS;
                        oMAE_TUTOR.TUT_EMAIL = oMaeTutorCLS.TUT_EMAIL;
                        //oMAE_TUTOR.TUT_ACTIVO = oMaeTutorCLS.TUT_ACTIVO;
                        oMAE_TUTOR.TUT_TELEFONO = oMaeTutorCLS.TUT_TELEFONO;
                        //oMAE_TUTOR.FOTO = fotoBD;
                        if (foto != null) oMAE_TUTOR.FOTO = fotoBD;
                        if (foto != null) oMAE_TUTOR.TUT_FOTO = oMaeTutorCLS.TUT_FOTO;
                        rpta = db.SaveChanges().ToString();


                    }
                }

                
            }
            return rpta;
        }
    }
}