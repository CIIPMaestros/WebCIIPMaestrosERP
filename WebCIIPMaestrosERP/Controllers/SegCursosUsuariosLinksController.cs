using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Transactions;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using WebCIIPMaestrosERP.Models;

namespace WebCIIPMaestrosERP.Controllers
{
    public class SegCursosUsuariosLinksController : Controller
    {


        List<SelectListItem> ListaUsuarios;
        private void llenarUsuarios()
        {
            using (var db = new DB_WebCIIPEntitiesERP())
            {

                ListaUsuarios = (from usuarios in db.SEG_USUARIOS
                               where usuarios.USU_ACTIVO == "1" && usuarios.ROL_ID == 1 // solo lista los usuarios de marketing o bots
                                 select new SelectListItem
                               {
                                   Text = usuarios.USU_NOMBRES+" "+ usuarios.USU_APELLIDO,
                                   Value = usuarios.USU_ID.ToString()

                               }).ToList();
            }

        }

        List<SelectListItem> ListaCursos;
        private void llenarCursos()
        {
            using (var db = new DB_WebCIIPEntitiesERP())
            {

                ListaCursos = (from cursos in db.MAE_CURSOS
                               where cursos.CUR_ACTIVO == "1" 
                               select new SelectListItem
                               {
                                   Text = cursos.CUR_NOMBRE,
                                   Value = cursos.CUR_ID.ToString()

                               }).ToList();
            }

        }


        // GET: SegCursosUsuariosLinks
        public ActionResult Index(SegCursoUsuarioLinksCLS oSegCursoUsuarioLinksCLS)
        {
            llenarCursos();
            llenarUsuarios();

            ViewBag.ListaCursos = ListaCursos;
            ViewBag.ListaUsuarios = ListaUsuarios;

            List<SegCursoUsuarioLinksCLS> ListaCursoByUsuario = null;

            string nombreCurso = oSegCursoUsuarioLinksCLS.USU_NOMBRES;

            using (var db = new DB_WebCIIPEntitiesERP())
            {

                if (nombreCurso == null)
                {

                    ListaCursoByUsuario = (from CursosByUsuarios in db.SEG_CURSOS_USUARIOS_LINKS
                                           join cursos in db.MAE_CURSOS
                                            on CursosByUsuarios.CUR_ID equals cursos.CUR_ID
                                           join usuarios in db.SEG_USUARIOS
                                           on CursosByUsuarios.USU_ID equals usuarios.USU_ID
                                          
                                           orderby cursos.CUR_ID

                                           select new SegCursoUsuarioLinksCLS

                                           {
                                               CUS_ID = CursosByUsuarios.CUS_ID,
                                               CUR_ID = cursos.CUR_ID,
                                               CUR_NOMBRE = cursos.CUR_NOMBRE,
                                               CUR_DESCRIPCION = cursos.CUR_DESCRIPCION,
                                               USU_ID = usuarios.USU_ID,
                                               USU_NOMBRES = usuarios.USU_NOMBRES+" "+ usuarios.USU_APELLIDO,
                                               USU_LINK = CursosByUsuarios.USU_LINK,
                                               USU_LINK_CORTO = CursosByUsuarios.USU_LINK_CORTO
                                           }).ToList();

                }
                else {


                    ListaCursoByUsuario = (from CursosByUsuarios in db.SEG_CURSOS_USUARIOS_LINKS
                                           join cursos in db.MAE_CURSOS
                                            on CursosByUsuarios.CUR_ID equals cursos.CUR_ID
                                           join usuarios in db.SEG_USUARIOS
                                           on CursosByUsuarios.USU_ID equals usuarios.USU_ID
                                           where
                                               usuarios.USU_NOMBRES.Contains(nombreCurso)
                                           orderby cursos.CUR_ID

                                           select new SegCursoUsuarioLinksCLS

                                           {
                                               CUS_ID = CursosByUsuarios.CUS_ID,
                                               CUR_ID = cursos.CUR_ID,
                                               CUR_NOMBRE = cursos.CUR_NOMBRE,
                                               CUR_DESCRIPCION = cursos.CUR_DESCRIPCION,
                                               USU_ID = usuarios.USU_ID,
                                               USU_NOMBRES = usuarios.USU_NOMBRES + " " + usuarios.USU_APELLIDO,
                                               USU_LINK = CursosByUsuarios.USU_LINK,
                                               USU_LINK_CORTO = CursosByUsuarios.USU_LINK_CORTO
                                           }).ToList();

                }

            }
            return View(ListaCursoByUsuario);
        }


        public int ActivarDesactivar(int id, string estado)
        {
            int nregistrosAfectados = 0;
            try
            {
                using (var db = new DB_WebCIIPEntitiesERP())
                {
                    SEG_CURSOS_USUARIOS_LINKS oSEG_CURSOS_USUARIOS_LINKS = db.SEG_CURSOS_USUARIOS_LINKS.Where(p => p.CUS_ID == id).First();
                    oSEG_CURSOS_USUARIOS_LINKS.LNK_ACTIVO = estado;
                    nregistrosAfectados = db.SaveChanges();
                }

            }
            catch (Exception ex)
            {

                nregistrosAfectados = 0;
            }

            return nregistrosAfectados;
        }


        public JsonResult recuperarDatos(int IdCus)
        {

            SegCursoUsuarioLinksCLS oSegCursoUsuarioLinksCLS = new SegCursoUsuarioLinksCLS();
            using (var db = new DB_WebCIIPEntitiesERP())
            {
                SEG_CURSOS_USUARIOS_LINKS oSEG_CURSOS_USUARIOS_LINKS = db.SEG_CURSOS_USUARIOS_LINKS.Where(p => p.CUS_ID == IdCus).First();
                oSegCursoUsuarioLinksCLS.CUR_ID = oSEG_CURSOS_USUARIOS_LINKS.CUR_ID;
                oSegCursoUsuarioLinksCLS.USU_ID = oSEG_CURSOS_USUARIOS_LINKS.USU_ID;
                oSegCursoUsuarioLinksCLS.USU_LINK = oSEG_CURSOS_USUARIOS_LINKS.USU_LINK;
                oSegCursoUsuarioLinksCLS.USU_LINK_CORTO = oSEG_CURSOS_USUARIOS_LINKS.USU_LINK_CORTO;
                
            }
            return Json(oSegCursoUsuarioLinksCLS, JsonRequestBehavior.AllowGet);


        }



        public string GuardarNuevoControladorParcial(SegCursoUsuarioLinksCLS oSegCursoUsuarioLinksCLS, int accion)
        {

            int idCus = 0;
            int nregistradosEncontradosCur = 0;
            int nregistradosEncontradosUsu = 0;
            string rpta = "";

            string LinkDoc = WebConfigurationManager.AppSettings["LinkDocentes"];


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

                    rpta += "<li class='list-group-item'>" + item + "</li>";

                }
                rpta += "</ul>";
            }
            else
            {
                if (oSegCursoUsuarioLinksCLS.CUR_ID == 0)
                {
                    rpta = "-1";
                }
                else if (oSegCursoUsuarioLinksCLS.USU_ID == 0)
                {
                    rpta = "-1";
                }
                else
                {
                    using (var db = new DB_WebCIIPEntitiesERP())
                    {
                        if (accion.Equals(-1))
                        {

                            //iniciamos concatenacion del link

                            nregistradosEncontradosCur = db.MAE_CURSOS.Where(x => x.CUR_ID.Equals(oSegCursoUsuarioLinksCLS.CUR_ID)).Count();

                            nregistradosEncontradosUsu = db.SEG_USUARIOS.Where(x => x.USU_ID.Equals(oSegCursoUsuarioLinksCLS.USU_ID)).Count();

                            if (nregistradosEncontradosCur == 0 && nregistradosEncontradosUsu == 0)
                            {
                                rpta = "-11";
                            }
                            else
                            {

                                SEG_USUARIOS oSEG_USUARIOS = db.SEG_USUARIOS.Where(p => p.USU_ID.Equals(oSegCursoUsuarioLinksCLS.USU_ID)).First();
                                MAE_CURSOS oMAE_CURSOS = db.MAE_CURSOS.Where(q => q.CUR_ID.Equals(oSegCursoUsuarioLinksCLS.CUR_ID)).First();


                                SEG_CURSOS_USUARIOS_LINKS oSEG_CURSOS_USUARIOS_LINKS = new SEG_CURSOS_USUARIOS_LINKS();
                                oSEG_CURSOS_USUARIOS_LINKS.CUR_ID = oSegCursoUsuarioLinksCLS.CUR_ID;
                                oSEG_CURSOS_USUARIOS_LINKS.USU_ID = oSegCursoUsuarioLinksCLS.USU_ID;
                                oSEG_CURSOS_USUARIOS_LINKS.USU_LINK_CORTO = oSegCursoUsuarioLinksCLS.USU_LINK_CORTO;
                                oSEG_CURSOS_USUARIOS_LINKS.USU_LINK = LinkDoc + oMAE_CURSOS.CUR_ID_ENCRIPTADO + "&VAR2=" + oSEG_USUARIOS.USU_ID_ENCRIPTADO;
                                oSEG_CURSOS_USUARIOS_LINKS.LNK_ACTIVO = "1";


                                db.SEG_CURSOS_USUARIOS_LINKS.Add(oSEG_CURSOS_USUARIOS_LINKS);
                                rpta = db.SaveChanges().ToString();

                                //debemos recuperar el numero del id para encriptarlo
                                

                            }

                        }
                        else//modificar
                        {


                            nregistradosEncontradosCur = db.MAE_CURSOS.Where(x => x.CUR_ID.Equals(oSegCursoUsuarioLinksCLS.CUR_ID)).Count();

                            nregistradosEncontradosUsu = db.SEG_USUARIOS.Where(x => x.USU_ID.Equals(oSegCursoUsuarioLinksCLS.USU_ID)).Count();

                            if (nregistradosEncontradosCur == 0 && nregistradosEncontradosUsu == 0)
                            {
                                rpta = "-11";
                            }
                            else
                            {
                                SEG_USUARIOS oSEG_USUARIOS = db.SEG_USUARIOS.Where(p => p.USU_ID.Equals(oSegCursoUsuarioLinksCLS.USU_ID)).First();
                                MAE_CURSOS oMAE_CURSOS = db.MAE_CURSOS.Where(q => q.CUR_ID.Equals(oSegCursoUsuarioLinksCLS.CUR_ID)).First();



                                SEG_CURSOS_USUARIOS_LINKS oSEG_CURSOS_USUARIOS_LINKS = db.SEG_CURSOS_USUARIOS_LINKS.Where(p => p.CUS_ID == accion).First();
                                
                                //oSEG_CURSOS_USUARIOS_LINKS.CUS_ID = oSegCursoUsuarioLinksCLS.CUS_ID;
                                oSEG_CURSOS_USUARIOS_LINKS.CUR_ID = oSegCursoUsuarioLinksCLS.CUR_ID;
                                oSEG_CURSOS_USUARIOS_LINKS.USU_ID = oSegCursoUsuarioLinksCLS.USU_ID;
                                oSEG_CURSOS_USUARIOS_LINKS.USU_LINK_CORTO = oSegCursoUsuarioLinksCLS.USU_LINK_CORTO;
                                oSEG_CURSOS_USUARIOS_LINKS.USU_LINK = LinkDoc + oMAE_CURSOS.CUR_ID_ENCRIPTADO + "&VAR2=" + oSEG_USUARIOS.USU_ID_ENCRIPTADO;


                                rpta = db.SaveChanges().ToString();
                            }

                        }
                    }
                }


            }
            return rpta;
        }


        public ActionResult Filtro(string nombreUsuario)
        {

            List<SegCursoUsuarioLinksCLS> ListaCursoByUsuario = new List<SegCursoUsuarioLinksCLS>();
            using (var db = new DB_WebCIIPEntitiesERP())
            {
                if (nombreUsuario == null)
                    ListaCursoByUsuario = (from CursosByUsuarios in db.SEG_CURSOS_USUARIOS_LINKS
                                           join cursos in db.MAE_CURSOS
                                            on CursosByUsuarios.CUR_ID equals cursos.CUR_ID
                                           join usuarios in db.SEG_USUARIOS
                                           on CursosByUsuarios.USU_ID equals usuarios.USU_ID

                                           orderby cursos.CUR_ID

                                           select new SegCursoUsuarioLinksCLS

                                           {
                                               CUS_ID = CursosByUsuarios.CUS_ID,
                                               CUR_ID = cursos.CUR_ID,
                                               CUR_NOMBRE = cursos.CUR_NOMBRE,
                                               CUR_DESCRIPCION = cursos.CUR_DESCRIPCION,
                                               USU_ID = usuarios.USU_ID,
                                               USU_NOMBRES = usuarios.USU_NOMBRES + " " + usuarios.USU_APELLIDO,
                                               USU_LINK = CursosByUsuarios.USU_LINK,
                                               USU_LINK_CORTO = CursosByUsuarios.USU_LINK_CORTO
                                           }).ToList();
                else
                    ListaCursoByUsuario = (from CursosByUsuarios in db.SEG_CURSOS_USUARIOS_LINKS
                                           join cursos in db.MAE_CURSOS
                                            on CursosByUsuarios.CUR_ID equals cursos.CUR_ID
                                           join usuarios in db.SEG_USUARIOS
                                           on CursosByUsuarios.USU_ID equals usuarios.USU_ID
                                           where
                                               usuarios.USU_NOMBRES.Contains(nombreUsuario)
                                           orderby cursos.CUR_ID

                                           select new SegCursoUsuarioLinksCLS

                                           {
                                               CUS_ID = CursosByUsuarios.CUS_ID,
                                               CUR_ID = cursos.CUR_ID,
                                               CUR_NOMBRE = cursos.CUR_NOMBRE,
                                               CUR_DESCRIPCION = cursos.CUR_DESCRIPCION,
                                               USU_ID = usuarios.USU_ID,
                                               USU_NOMBRES = usuarios.USU_NOMBRES + " " + usuarios.USU_APELLIDO,
                                               USU_LINK = CursosByUsuarios.USU_LINK,
                                               USU_LINK_CORTO = CursosByUsuarios.USU_LINK_CORTO
                                           }).ToList();
            }
            return PartialView("_Index", ListaCursoByUsuario);
        }


    }
}