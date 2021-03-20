using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Mvc;
using WebCIIPMaestrosERP.Models;
using WebCIIPMaestrosERP.Seguridad;

namespace WebCIIPMaestrosERP.Controllers
{
    [Acceder]
    public class MaeCursoLanzamientoController : Controller
    {
        // GET: MaeCursoLanzamiento
        public ActionResult Index()
        {

            llenarCurso();
            LlenarTutor();
            llenarTipoCurso();

            ViewBag.lista = ListaCursos;
            ViewBag.listaTutor = ListaTutor;
            ViewBag.listaTipoCurso = ListaTipoCurso;


            List<MaeCursoLanzamientoCLS> ListaLanzamientos = null;

            using (var db = new DB_WebCIIPEntitiesERP())
            {
                ListaLanzamientos = (from lanzamiento in db.MAE_CURSOS_LANZAMIENTOS
                                     join cursos in db.MAE_CURSOS
                                     on lanzamiento.CUR_ID equals cursos.CUR_ID
                                     join tutores in db.MAE_TUTOR
                                     on lanzamiento.TUT_ID equals tutores.TUT_ID
                                     join tablas in db.MAE_TABLAS
                                     on lanzamiento.LAN_ACTIVO equals tablas.ID.ToString()
                                     where tablas.COD_TABLA == "ACT" 

                                     select new MaeCursoLanzamientoCLS
                                     {
                                         LAN_ID = lanzamiento.LAN_ID,
                                         CUR_ID = lanzamiento.CUR_ID,
                                         TUT_ID = lanzamiento.TUT_ID,
                                         LAN_ESTADO = tablas.DESCRIPCION,
                                         CUR_NOMBRE = cursos.CUR_NOMBRE,
                                         TUT_NOMBRES = tutores.TUT_NOMBRES+" "+tutores.TUT_APELLIDOS,
                                         LAN_FEC_CAPACITACION = (DateTime)lanzamiento.LAN_FEC_CAPACITACION
                                     }).ToList();
            }

            return View(ListaLanzamientos);
        }

        public ActionResult Filtro(string nombreLanzamiento)
        {

            List<MaeCursoLanzamientoCLS> listaLanzamientos = new List<MaeCursoLanzamientoCLS>();
            using (var db = new DB_WebCIIPEntitiesERP())
            {
                if (nombreLanzamiento == null)
                    listaLanzamientos = (from lanzamiento in db.MAE_CURSOS_LANZAMIENTOS
                                         join cursos in db.MAE_CURSOS
                                         on lanzamiento.CUR_ID equals cursos.CUR_ID
                                         join tutores in db.MAE_TUTOR
                                         on lanzamiento.TUT_ID equals tutores.TUT_ID
                                         join tablas in db.MAE_TABLAS
                                         on lanzamiento.LAN_ACTIVO equals tablas.ID.ToString()
                                         where tablas.COD_TABLA == "ACT"

                                         select new MaeCursoLanzamientoCLS
                                         {
                                             LAN_ID = lanzamiento.LAN_ID,
                                             CUR_ID = lanzamiento.CUR_ID,
                                             TUT_ID = lanzamiento.TUT_ID,
                                             CUR_NOMBRE = cursos.CUR_NOMBRE,
                                             LAN_ESTADO = tablas.DESCRIPCION,
                                             TUT_NOMBRES = tutores.TUT_NOMBRES + " " + tutores.TUT_APELLIDOS,
                                             LAN_FEC_CAPACITACION = (DateTime)lanzamiento.LAN_FEC_CAPACITACION
                                         }).ToList();
                else
                    listaLanzamientos = (from lanzamiento in db.MAE_CURSOS_LANZAMIENTOS
                                         join cursos in db.MAE_CURSOS
                                         on lanzamiento.CUR_ID equals cursos.CUR_ID
                                         join tutores in db.MAE_TUTOR
                                         on lanzamiento.TUT_ID equals tutores.TUT_ID
                                         join tablas in db.MAE_TABLAS
                                         on lanzamiento.LAN_ACTIVO equals tablas.ID.ToString()
                                         where tablas.COD_TABLA == "ACT" &&
                                                (cursos.CUR_NOMBRE.Contains(nombreLanzamiento)
                                                || tutores.TUT_NOMBRES.Contains(nombreLanzamiento)
                                                || tutores.TUT_APELLIDOS.Contains(nombreLanzamiento))
                                         select new MaeCursoLanzamientoCLS
                                         {
                                             LAN_ID = lanzamiento.LAN_ID,
                                             CUR_ID = lanzamiento.CUR_ID,
                                             TUT_ID = lanzamiento.TUT_ID,
                                             CUR_NOMBRE = cursos.CUR_NOMBRE,
                                             LAN_ESTADO = tablas.DESCRIPCION,
                                             TUT_NOMBRES = tutores.TUT_NOMBRES + " " + tutores.TUT_APELLIDOS,
                                             LAN_FEC_CAPACITACION = (DateTime)lanzamiento.LAN_FEC_CAPACITACION
                                         }).ToList();
            }
            return PartialView("_Index", listaLanzamientos);
        }

        public int ActivarDesactivar(int id, string estado)
        {
            int nregistrosAfectados = 0;
            try
            {
                using (var db = new DB_WebCIIPEntitiesERP())
                {
                    MAE_CURSOS_LANZAMIENTOS oMAE_CURSOS_LANZAMIENTOS = db.MAE_CURSOS_LANZAMIENTOS.Where(p => p.LAN_ID == id).First();
                    oMAE_CURSOS_LANZAMIENTOS.LAN_ACTIVO = estado;
                    nregistrosAfectados = db.SaveChanges();
                }

            }
            catch (Exception ex)
            {

                nregistrosAfectados = 0;
            }

            return nregistrosAfectados;
        }
        public string GuardarNuevoControladorParcial(MaeCursoLanzamientoCLS oMaeCursoLanzamientoCLS, int accion)
        {
            string rpta = "";
            int idLanzamiento = 0;
            //decimal idLanzamientoEncrip = 0;


                using (var db = new DB_WebCIIPEntitiesERP())

                {
                if (accion == -1)
                {
                    MAE_CURSOS_LANZAMIENTOS oMaeCursoLanzamiento = new MAE_CURSOS_LANZAMIENTOS();
                    oMaeCursoLanzamiento.CUR_ID = oMaeCursoLanzamientoCLS.CUR_ID;
                    oMaeCursoLanzamiento.TUT_ID = oMaeCursoLanzamientoCLS.TUT_ID;
                    oMaeCursoLanzamiento.LAN_ACTIVO = "1";
                    oMaeCursoLanzamiento.LAN_FEC_CAPACITACION = oMaeCursoLanzamientoCLS.LAN_FEC_CAPACITACION;
                    oMaeCursoLanzamiento.IND_TIPO_LAN = oMaeCursoLanzamientoCLS.IND_TIPO_LAN;
                    db.MAE_CURSOS_LANZAMIENTOS.Add(oMaeCursoLanzamiento);
                    rpta = db.SaveChanges().ToString();

                    //debemos recuperar el numero del id para encriptarlo
                    idLanzamiento = oMaeCursoLanzamiento.LAN_ID; // recuperar

                    //iniciamos la encriptacion

                    using (var transacction = new TransactionScope())
                    {

                        SHA256Managed sha = new SHA256Managed();
                        byte[] byteContra = Encoding.Default.GetBytes(idLanzamiento.ToString());
                        byte[] byteContraCifrado = sha.ComputeHash(byteContra);
                        string cadenaContraCifrada = BitConverter.ToString(byteContraCifrado).Replace("-", "");


                        MAE_CURSOS_LANZAMIENTOS oMAE_CURSOS_LANZAMIENTOS = db.MAE_CURSOS_LANZAMIENTOS.Where(p => p.LAN_ID == idLanzamiento).First();
                        oMAE_CURSOS_LANZAMIENTOS.LAN_ID_ENCRIPTADO = cadenaContraCifrada;
                        rpta = db.SaveChanges().ToString();
                        transacction.Complete();

                    }

                }
                else {

                    MAE_CURSOS_LANZAMIENTOS oMAE_CURSOS_LANZAMIENTOS = db.MAE_CURSOS_LANZAMIENTOS.Where(p => p.LAN_ID == accion).First();
                    oMAE_CURSOS_LANZAMIENTOS.CUR_ID = oMaeCursoLanzamientoCLS.CUR_ID;
                    oMAE_CURSOS_LANZAMIENTOS.TUT_ID = oMaeCursoLanzamientoCLS.TUT_ID;
                    oMAE_CURSOS_LANZAMIENTOS.IND_TIPO_LAN = oMaeCursoLanzamientoCLS.IND_TIPO_LAN;
                    oMAE_CURSOS_LANZAMIENTOS.LAN_FEC_CAPACITACION = oMaeCursoLanzamientoCLS.LAN_FEC_CAPACITACION;
                    rpta = db.SaveChanges().ToString();


                }


            }

            return rpta;

        }

        public JsonResult recuperarDatos(int IdLanzamiento)
        {

            MaeCursoLanzamientoCLS oMaeCursoLanzamientoCLS = new MaeCursoLanzamientoCLS();
            using (var db = new DB_WebCIIPEntitiesERP())
            {
                MAE_CURSOS_LANZAMIENTOS oMAE_CURSOS_LANZAMIENTOS = db.MAE_CURSOS_LANZAMIENTOS.Where(p => p.LAN_ID.Equals(IdLanzamiento)).First();

                oMaeCursoLanzamientoCLS.CUR_ID = oMAE_CURSOS_LANZAMIENTOS.CUR_ID;
                oMaeCursoLanzamientoCLS.TUT_ID = oMAE_CURSOS_LANZAMIENTOS.TUT_ID;
                oMaeCursoLanzamientoCLS.IND_TIPO_LAN = oMAE_CURSOS_LANZAMIENTOS.IND_TIPO_LAN;
                oMaeCursoLanzamientoCLS.LAN_FEC_CAPACITACIONCADENA = oMAE_CURSOS_LANZAMIENTOS.LAN_FEC_CAPACITACION!= null ?
                                                                ((DateTime)oMAE_CURSOS_LANZAMIENTOS.LAN_FEC_CAPACITACION).ToString("yyyy-MM-ddTHH:mm") : "";


                //(DateTime)oMAE_CURSOS_LANZAMIENTOS.LAN_FEC_CAPACITACION;


            }
            return Json(oMaeCursoLanzamientoCLS, JsonRequestBehavior.AllowGet);


        }

        List<SelectListItem> ListaTipoCurso;
        private void llenarTipoCurso()
        {
            using (var db = new DB_WebCIIPEntitiesERP())
            {

                ListaTipoCurso = (from tablas in db.MAE_TABLAS
                                  where tablas.COD_TABLA == "TCU"
                                  select new SelectListItem
                               {
                                   Text = tablas.DESCRIPCION,
                                   Value = tablas.ID.ToString()

                               }).ToList();
            }

        }

        List<SelectListItem> ListaCursos;
        private void llenarCurso()
        {
            using (var db = new DB_WebCIIPEntitiesERP())
            {

                ListaCursos = (from lanzamiento in db.MAE_CURSOS
                               select new SelectListItem
                               {
                                   Text = lanzamiento.CUR_NOMBRE,
                                   Value = lanzamiento.CUR_ID.ToString()

                               }).ToList();
            }

        }

        List<SelectListItem> ListaTutor;
        private void LlenarTutor()
        {
            using (var db = new DB_WebCIIPEntitiesERP())
            {
                ListaTutor = (from tutor in db.MAE_TUTOR
                              select new SelectListItem
                              {
                                  Text = tutor.TUT_NOMBRES + " " + tutor.TUT_APELLIDOS,
                                  Value = tutor.TUT_ID.ToString()
                              }).ToList();

            }
        }


        public ActionResult AgregarLanzamiento()
        {
            llenarCurso();
            LlenarTutor();

            ViewBag.lista = ListaCursos;
            ViewBag.listaTutor = ListaTutor;

            return View();
        }

        [HttpPost]
        public ActionResult AgregarLanzamiento(MaeCursoLanzamientoCLS oMaeCursoLanzamientoCLS)
        {

            int idLanzamiento = 0;
            //decimal idLanzamientoEncrip = 0;

            if (!ModelState.IsValid)
            {
                llenarCurso();
                LlenarTutor();

                ViewBag.lista = ListaCursos;
                ViewBag.listaTutor = ListaTutor;
                return View(oMaeCursoLanzamientoCLS);
            }
            else
            {

                using (var db = new DB_WebCIIPEntitiesERP())

                {
                    MAE_CURSOS_LANZAMIENTOS oMaeCursoLanzamiento = new MAE_CURSOS_LANZAMIENTOS();
                    oMaeCursoLanzamiento.CUR_ID = oMaeCursoLanzamientoCLS.CUR_ID;
                    oMaeCursoLanzamiento.TUT_ID = oMaeCursoLanzamientoCLS.TUT_ID;
                    oMaeCursoLanzamiento.LAN_FEC_CAPACITACION = oMaeCursoLanzamientoCLS.LAN_FEC_CAPACITACION;
                    db.MAE_CURSOS_LANZAMIENTOS.Add(oMaeCursoLanzamiento);
                    db.SaveChanges();

                    //debemos recuperar el numero del id para encriptarlo
                    idLanzamiento = oMaeCursoLanzamiento.LAN_ID; // recuperar

                    //iniciamos la encriptacion

                    using (var transacction = new TransactionScope())
                    {

                        SHA256Managed sha = new SHA256Managed();
                        byte[] byteContra = Encoding.Default.GetBytes(idLanzamiento.ToString());
                        byte[] byteContraCifrado = sha.ComputeHash(byteContra);
                        string cadenaContraCifrada = BitConverter.ToString(byteContraCifrado).Replace("-", "");


                        MAE_CURSOS_LANZAMIENTOS oMAE_CURSOS_LANZAMIENTOS = db.MAE_CURSOS_LANZAMIENTOS.Where(p => p.LAN_ID == idLanzamiento).First();
                        oMAE_CURSOS_LANZAMIENTOS.LAN_ID_ENCRIPTADO = cadenaContraCifrada;
                        db.SaveChanges().ToString();
                        transacction.Complete();

                    }




                }
            }
            return RedirectToAction("Index");

        }
     }
}