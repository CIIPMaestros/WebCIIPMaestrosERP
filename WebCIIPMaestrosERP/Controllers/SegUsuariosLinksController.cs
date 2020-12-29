using Microsoft.Ajax.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Web.Configuration;
using System.Web.Mvc;
using WebCIIPMaestrosERP.Models;
using WebCIIPMaestrosERP.Seguridad;

namespace WebCIIPMaestrosERP.Controllers
{
    [Acceder]
    public class SegUsuariosLinksController : Controller
    {
        // GET: SegUsuariosLinks

        List<SelectListItem> ListaCursos;
        private void llenarCurso()
        {
            using (var db = new DB_WebCIIPEntities())
            {

                ListaCursos = (from cursos in db.MAE_CURSOS
                               select new SelectListItem
                               {
                                   Text = cursos.CUR_NOMBRE,
                                   Value = cursos.CUR_ID.ToString()

                               }).ToList();
            }

        }

        List<SelectListItem> ListaUsuarios;
        private void LlenarUsuarios()
        {
            string RolMark = WebConfigurationManager.AppSettings["RolMarketing"];

            using (var db = new DB_WebCIIPEntities())
            {
                ListaUsuarios = (from usuarios in db.SEG_USUARIOS
                                 where usuarios.ROL_ID.ToString() == RolMark

                              select new SelectListItem
                              {
                                  Text = usuarios.USU_NOMBRES + " " + usuarios.USU_APELLIDO,
                                  Value = usuarios.USU_ID.ToString()
                              }).ToList();
            }
        }


        public string GuardarDB(SegUsuariosLinksCLS oSegUsuariosLinksCLS, int accion) {

            return oSegUsuariosLinksCLS.IdLanzamiento.ToString();
        }

        public string GuardarNuevoControladorParcial(SegUsuariosLinksCLS oSegUsuariosLinksCLS, int accion)
        {

            string rpta="";
            //-11 es cuando ya tiene un registro.

            int nregistradosEncontrados = 0;

            //oSegUsuariosLinksCLS.LAN_ID = (int)IdLanzamiento;

            if (oSegUsuariosLinksCLS.CUR_ID == 0)
            {
                rpta = "-1";
            }
            else if (oSegUsuariosLinksCLS.USU_ID == 0)
            {
                rpta = "-1";
            }
            else if (oSegUsuariosLinksCLS.LAN_ID == 0)
            {
                rpta = "-1";
            }
            else
            {

                using (var db = new DB_WebCIIPEntities())
                {
                    string LinkDoc = WebConfigurationManager.AppSettings["LinkDocentes"];

                    if (accion.Equals(-1))
                    {

                        nregistradosEncontrados = db.SEG_USUARIOS_LINKS.Where(x => x.CUR_ID.Equals(oSegUsuariosLinksCLS.CUR_ID) &&
                                                                                    x.USU_ID.Equals(oSegUsuariosLinksCLS.USU_ID) &&
                                                                                   x.LAN_ID.Equals(oSegUsuariosLinksCLS.LAN_ID)).Count();

                        if (nregistradosEncontrados > 0)
                        {
                            rpta = "-11";
                        }
                        else
                        {
                            
                            


                            SEG_USUARIOS oSEG_USUARIOS = db.SEG_USUARIOS.Where(p => p.USU_ID.Equals(oSegUsuariosLinksCLS.USU_ID)).First();
                            MAE_CURSOS_LANZAMIENTOS oMAE_CURSOS_LANZAMIENTOS = db.MAE_CURSOS_LANZAMIENTOS.Where(q => q.LAN_ID.Equals(oSegUsuariosLinksCLS.LAN_ID)).First();

                            SEG_USUARIOS_LINKS oSEG_USUARIOS_LINKS = new SEG_USUARIOS_LINKS();
                            oSEG_USUARIOS_LINKS.CUR_ID = oSegUsuariosLinksCLS.CUR_ID;
                            oSEG_USUARIOS_LINKS.USU_ID = oSegUsuariosLinksCLS.USU_ID;
                            oSEG_USUARIOS_LINKS.LAN_ID = oSegUsuariosLinksCLS.LAN_ID;
                            oSEG_USUARIOS_LINKS.USU_LINK_CORTO = oSegUsuariosLinksCLS.USU_LINK_CORTO;
                            oSEG_USUARIOS_LINKS.LNK_ACTIVO = "1";
                            oSEG_USUARIOS_LINKS.USU_LINK = LinkDoc + oMAE_CURSOS_LANZAMIENTOS.LAN_ID_ENCRIPTADO + "&VAR2=" + oSEG_USUARIOS.USU_ID_ENCRIPTADO;

                            db.SEG_USUARIOS_LINKS.Add(oSEG_USUARIOS_LINKS);
                            rpta = db.SaveChanges().ToString();
                        }

                    }
                    else
                    {//editar

                        nregistradosEncontrados = db.SEG_USUARIOS_LINKS.Where(x => x.CUR_ID.Equals(oSegUsuariosLinksCLS.CUR_ID) &&
                                                                                     x.USU_ID.Equals(oSegUsuariosLinksCLS.USU_ID) &&
                                                                                    x.LAN_ID.Equals(oSegUsuariosLinksCLS.LAN_ID) &&
                                                                                    x.USU_LINK_CORTO.Equals(oSegUsuariosLinksCLS.USU_LINK_CORTO)).Count();

                        if (nregistradosEncontrados > 0)
                        {
                            rpta = "-11";
                        }
                        else
                        {
                            SEG_USUARIOS oSEG_USUARIOS = db.SEG_USUARIOS.Where(p => p.USU_ID.Equals(oSegUsuariosLinksCLS.USU_ID)).First();
                            MAE_CURSOS_LANZAMIENTOS oMAE_CURSOS_LANZAMIENTOS = db.MAE_CURSOS_LANZAMIENTOS.Where(q => q.LAN_ID.Equals(oSegUsuariosLinksCLS.IdLanzamiento)).First();

                            SEG_USUARIOS_LINKS oSEG_USUARIOS_LINKS = db.SEG_USUARIOS_LINKS.Where(p => p.LAN_ID.Equals(oSegUsuariosLinksCLS.IdLanzamientoOriginal) &&
                                                                                                      p.CUR_ID.Equals(oSegUsuariosLinksCLS.IdCursoOriginal) &&
                                                                                                      p.USU_ID.Equals(oSegUsuariosLinksCLS.IdUsuarioOriginal)).First();

                            oSEG_USUARIOS_LINKS.LAN_ID = oSegUsuariosLinksCLS.LAN_ID;
                            oSEG_USUARIOS_LINKS.CUR_ID = oSegUsuariosLinksCLS.CUR_ID;
                            oSEG_USUARIOS_LINKS.USU_ID = oSegUsuariosLinksCLS.USU_ID;
                            oSEG_USUARIOS_LINKS.USU_LINK_CORTO = oSegUsuariosLinksCLS.USU_LINK_CORTO;
                            oSEG_USUARIOS_LINKS.USU_LINK = LinkDoc + oMAE_CURSOS_LANZAMIENTOS.LAN_ID_ENCRIPTADO + "&VAR2=" + oSEG_USUARIOS.USU_ID_ENCRIPTADO;

                            rpta = db.SaveChanges().ToString();

                        }
                    }

                }
            }

            return rpta;
        }


        private List<SelectListItem> GetLanzamientosListAll(int IdCurso)
        {
            List<SelectListItem> Lanzamientos;
            using (var db = new DB_WebCIIPEntities())
            {

                /*var Lanzamientos = db.MAE_CURSOS_LANZAMIENTOS.Where(x => x.CUR_ID == IdCurso);
                var resp = Lanzamientos.Select(x => new SelectListItem()
                {
                    Value = x.LAN_ID.ToString(),
                    Text = x.LAN_ID.ToString() + " - " + x.LAN_FEC_CAPACITACION.ToString(),
                }).ToList();*/

                Lanzamientos = (from lanzamiento in db.MAE_CURSOS_LANZAMIENTOS
                                where lanzamiento.CUR_ID == IdCurso

                                select new SelectListItem
                                {
                                    Text = lanzamiento.LAN_ID.ToString() + " - " + lanzamiento.LAN_FEC_CAPACITACION,
                                    Value = lanzamiento.LAN_ID.ToString()

                                }).Distinct().ToList();


                Lanzamientos.Insert(0, new SelectListItem() { Value = "", Text = "Elija una opcion" });

                return Lanzamientos;
            }
        }

        private List<SelectListItem> GetLanzamientosListByUser(int IdCurso, int IdUser)
        {
            List<SelectListItem> Lanzamientos;
            using (var db = new DB_WebCIIPEntities())
            {

                /*var Lanzamientos = db.MAE_CURSOS_LANZAMIENTOS.Where(x => x.CUR_ID == IdCurso);
                var resp = Lanzamientos.Select(x => new SelectListItem()
                {
                    Value = x.LAN_ID.ToString(),
                    Text = x.LAN_ID.ToString() + " - " + x.LAN_FEC_CAPACITACION.ToString(),
                }).ToList();*/

                Lanzamientos = (from lanzamiento in db.MAE_CURSOS_LANZAMIENTOS
                                join usuarioLink in db.SEG_USUARIOS_LINKS
                                on lanzamiento.LAN_ID equals usuarioLink.LAN_ID
                                where   usuarioLink.USU_ID == IdUser &&
                                        usuarioLink.CUR_ID == IdCurso

                                select new SelectListItem
                         {
                             Text = lanzamiento.LAN_ID.ToString() + " - " + lanzamiento.LAN_FEC_CAPACITACION,
                             Value = lanzamiento.LAN_ID.ToString()

                         }).Distinct().ToList();


                Lanzamientos.Insert(0, new SelectListItem() { Value = "", Text = "Elija una opcion" });

                return Lanzamientos;
            }
        }


        List<SelectListItem> ListaLanzamientos;
        public JsonResult GetLanzamientosAll(int IdCurso)
        {
            ListaLanzamientos = GetLanzamientosListAll(IdCurso);
            return Json(ListaLanzamientos, JsonRequestBehavior.AllowGet);

        }

        public JsonResult GetLanzamientosByUser(int IdCurso, int IdUser)
        {
            ListaLanzamientos = GetLanzamientosListByUser(IdCurso, IdUser);
            return Json(ListaLanzamientos, JsonRequestBehavior.AllowGet);

        }



        private List<SelectListItem> GetCursoList(int Idusuario)
        {
            List<SelectListItem> Lista;
            using (var db = new DB_WebCIIPEntities())
            {

                Lista = (from cursos in db.MAE_CURSOS
                         join usuarioLink in db.SEG_USUARIOS_LINKS
                         on cursos.CUR_ID equals usuarioLink.CUR_ID
                         where usuarioLink.USU_ID == Idusuario

                         select new SelectListItem
                         {
                             Text = cursos.CUR_ID.ToString() + " - " + cursos.CUR_NOMBRE,
                             Value = cursos.CUR_ID.ToString()

                         }).Distinct().ToList();
                
                //var Lanzamientos = db.MAE_CURSOS_LANZAMIENTOS.Where(x => x.CUR_ID == Idusuario);
                /*
                var resp = Lista.Select(x => new SelectListItem()
                {
                    Value = x.LAN_ID.ToString(),
                    Text = x.LAN_ID.ToString() + " - " + x.LAN_FEC_CAPACITACION.ToString(),
                }).ToList();
                */

                Lista.Insert(0, new SelectListItem() { Value = "", Text = "Elija una opcion" });

                return Lista;
            }
        }

        public JsonResult GetCursosByUser(int Idusuario)
        {
            ListaLanzamientos = GetCursoList(Idusuario);
            return Json(ListaLanzamientos, JsonRequestBehavior.AllowGet);

        }


        public JsonResult recuperarDatos(int idLink)
        {

            SegUsuariosLinksCLS oSegUsuariosLinksCLS = new SegUsuariosLinksCLS();
            using (var db = new DB_WebCIIPEntities())
            {
                SEG_USUARIOS_LINKS oSEG_USUARIOS_LINKS = db.SEG_USUARIOS_LINKS.Where(p => p.LNK_ID.Equals(idLink)).First();

                oSegUsuariosLinksCLS.CUR_ID = oSEG_USUARIOS_LINKS.CUR_ID;
                oSegUsuariosLinksCLS.USU_ID = oSEG_USUARIOS_LINKS.USU_ID;
                oSegUsuariosLinksCLS.LAN_ID = oSEG_USUARIOS_LINKS.LAN_ID;
                oSegUsuariosLinksCLS.USU_LINK = oSEG_USUARIOS_LINKS.USU_LINK;
                oSegUsuariosLinksCLS.USU_LINK_CORTO = oSEG_USUARIOS_LINKS.USU_LINK_CORTO;

            }
            return Json(oSegUsuariosLinksCLS, JsonRequestBehavior.AllowGet);


        }

        public int ActivarDesactivar(int id, string estado)
        {
            int nregistrosAfectados = 0;
            try
            {
                using (var db = new DB_WebCIIPEntities())
                {
                    SEG_USUARIOS_LINKS oSEG_USUARIOS_LINKS = db.SEG_USUARIOS_LINKS.Where(p => p.LNK_ID == id).First();
                    oSEG_USUARIOS_LINKS.LNK_ACTIVO = estado;
                    nregistrosAfectados = db.SaveChanges();
                }

            }
            catch (Exception ex)
            {

                nregistrosAfectados = 0;
            }

            return nregistrosAfectados;
        }
        public ActionResult Filtro(string nombreVarios)
        {

            List<SegUsuariosLinksCLS> listaUsuariosLink = new List<SegUsuariosLinksCLS>();
            using (var db = new DB_WebCIIPEntities())
            {
                if (nombreVarios == null)
                {
                 listaUsuariosLink = (from links in db.SEG_USUARIOS_LINKS
                                     join cursos in db.MAE_CURSOS
                                     on links.CUR_ID equals cursos.CUR_ID
                                     join usuarios in db.SEG_USUARIOS
                                     on links.USU_ID equals usuarios.USU_ID
                                     join lanzamiento in db.MAE_CURSOS_LANZAMIENTOS
                                     on links.LAN_ID equals lanzamiento.LAN_ID
                                     join tablas in db.MAE_TABLAS
                                     on links.LNK_ACTIVO equals tablas.ID.ToString()
                                     where tablas.COD_TABLA == "ACT"

                                      select new SegUsuariosLinksCLS

                                     {
                                         LNK_ID = (int)links.LNK_ID,
                                         USU_ID = (int)links.USU_ID,
                                         CUR_ID = (int)links.CUR_ID,
                                         CUR_NOMBRE = cursos.CUR_NOMBRE,
                                         LAN_ID = links.LAN_ID,
                                         USU_NOMBRES = usuarios.USU_NOMBRES + " " + usuarios.USU_APELLIDO,
                                         USU_LINK = links.USU_LINK,
                                         LNK_ACTIVO = tablas.DESCRIPCION,
                                         LAN_FEC_CAPACITACION = (DateTime)lanzamiento.LAN_FEC_CAPACITACION

                                     }).ToList();
                }
                else
                {

                 listaUsuariosLink = (from links in db.SEG_USUARIOS_LINKS
                                      join cursos in db.MAE_CURSOS
                                      on links.CUR_ID equals cursos.CUR_ID
                                      join usuarios in db.SEG_USUARIOS
                                      on links.USU_ID equals usuarios.USU_ID
                                      join lanzamiento in db.MAE_CURSOS_LANZAMIENTOS
                                      on links.LAN_ID equals lanzamiento.LAN_ID
                                      join tablas in db.MAE_TABLAS
                                     on links.LNK_ACTIVO equals tablas.ID.ToString()
                                      where (cursos.CUR_NOMBRE.Contains(nombreVarios) ||
                                            usuarios.USU_NOMBRES.Contains(nombreVarios) ||
                                            usuarios.USU_APELLIDO.Contains(nombreVarios) ) && tablas.COD_TABLA == "ACT"

                                      select new SegUsuariosLinksCLS

                                      {
                                          LNK_ID = (int)links.LNK_ID,
                                          USU_ID = (int)links.USU_ID,
                                          CUR_ID = (int)links.CUR_ID,
                                          CUR_NOMBRE = cursos.CUR_NOMBRE,
                                          LAN_ID = links.LAN_ID,
                                          USU_NOMBRES = usuarios.USU_NOMBRES + " " + usuarios.USU_APELLIDO,
                                          USU_LINK = links.USU_LINK,
                                          LNK_ACTIVO = tablas.DESCRIPCION,
                                          LAN_FEC_CAPACITACION = (DateTime)lanzamiento.LAN_FEC_CAPACITACION

                                      }).ToList();
                }
                return PartialView("_Index", listaUsuariosLink);
            }
        }

        public ActionResult ListarLinkDisponiblesByIdUsu() {

            //buscamos el usuario que se a logeado
            int idusuario = (int)HttpContext.Session["UsuID"];


            using (var db = new DB_WebCIIPEntities()) {

                var CursosDisponible = (from links in db.SEG_USUARIOS_LINKS
                                        join cursos in db.MAE_CURSOS
                                        on links.CUR_ID equals cursos.CUR_ID
                                        join usuarios in db.SEG_USUARIOS
                                        on links.USU_ID equals usuarios.USU_ID
                                        join lanzamiento in db.MAE_CURSOS_LANZAMIENTOS
                                        on links.LAN_ID equals lanzamiento.LAN_ID
                                        join tablas in db.MAE_TABLAS
                                       on links.LNK_ACTIVO equals tablas.ID.ToString()
                                        where tablas.COD_TABLA == "ACT" && 
                                                links.USU_ID == idusuario

                                        select new SegUsuariosLinksCLS

                                        {
                                            LNK_ID = (int)links.LNK_ID,
                                            USU_ID = (int)links.USU_ID,
                                            CUR_ID = (int)links.CUR_ID,
                                            CUR_NOMBRE = cursos.CUR_NOMBRE,
                                            LAN_ID = links.LAN_ID,
                                            USU_NOMBRES = usuarios.USU_NOMBRES + " " + usuarios.USU_APELLIDO,
                                            USU_LINK = links.USU_LINK,
                                            LNK_ACTIVO = tablas.DESCRIPCION,
                                            LAN_FEC_CAPACITACION = (DateTime)lanzamiento.LAN_FEC_CAPACITACION

                                        }).ToList();

                return View();
            }
            
        }


        public ActionResult Index(SegUsuariosLinksCLS osegUsuariosLinksCLS)
        {
            llenarCurso();
            LlenarUsuarios();


            ViewBag.ListaCursos = ListaCursos;
            ViewBag.ListaUsuarios = ListaUsuarios;

            using (var db = new DB_WebCIIPEntities()) {

                var CursosDisponible = (from links in db.SEG_USUARIOS_LINKS
                                      join cursos in db.MAE_CURSOS
                                      on links.CUR_ID equals cursos.CUR_ID
                                      join usuarios in db.SEG_USUARIOS
                                      on links.USU_ID equals usuarios.USU_ID
                                      join lanzamiento in db.MAE_CURSOS_LANZAMIENTOS
                                      on links.LAN_ID equals lanzamiento.LAN_ID
                                        join tablas in db.MAE_TABLAS
                                       on links.LNK_ACTIVO equals tablas.ID.ToString()
                                       where tablas.COD_TABLA == "ACT"

                                        select new SegUsuariosLinksCLS

                                         {
                                             LNK_ID = (int)links.LNK_ID,
                                             USU_ID = (int)links.USU_ID,
                                             CUR_ID = (int)links.CUR_ID,
                                             CUR_NOMBRE = cursos.CUR_NOMBRE,
                                             LAN_ID = links.LAN_ID,
                                             USU_NOMBRES = usuarios.USU_NOMBRES+" "+usuarios.USU_APELLIDO,
                                             USU_LINK = links.USU_LINK,
                                            LNK_ACTIVO = tablas.DESCRIPCION,
                                            LAN_FEC_CAPACITACION = (DateTime)lanzamiento.LAN_FEC_CAPACITACION

                                         }).ToList();

                return View(CursosDisponible);
            }

            

        }

        //Reporte de Marketing - Independiente
        public ActionResult LinkDisponible(SegUsuariosLinksCLS osegUsuariosLinksCLS)
        {
            llenarCurso();
            LlenarUsuarios();


            ViewBag.ListaCursos = ListaCursos;
            ViewBag.ListaUsuarios = ListaUsuarios;

            using (var db = new DB_WebCIIPEntities())
            {

                var CursosDisponible = (from links in db.SEG_USUARIOS_LINKS
                                        join cursos in db.MAE_CURSOS
                                        on links.CUR_ID equals cursos.CUR_ID
                                        join usuarios in db.SEG_USUARIOS
                                        on links.USU_ID equals usuarios.USU_ID

                                        join lanzamiento in db.MAE_CURSOS_LANZAMIENTOS
                                        on links.LAN_ID equals lanzamiento.LAN_ID

                                        join tutores in db.MAE_TUTOR
                                        on lanzamiento.TUT_ID equals tutores.TUT_ID

                                        join tablas in db.MAE_TABLAS
                                        on links.LNK_ACTIVO equals tablas.ID.ToString()
                                        where   tablas.COD_TABLA == "ACT" &&
                                                links.LNK_ACTIVO == "1" // solo muestra registros activos

                                        select new SegUsuariosLinksCLS

                                        {
                                            LNK_ID = (int)links.LNK_ID,
                                            USU_ID = (int)links.USU_ID,
                                            CUR_ID = (int)links.CUR_ID,
                                            CUR_NOMBRE = cursos.CUR_NOMBRE,
                                            TUT_NOMBRES = tutores.TUT_NOMBRES+" "+ tutores.TUT_APELLIDOS,
                                            LAN_ID = links.LAN_ID,
                                            USU_NOMBRES = usuarios.USU_NOMBRES + " " + usuarios.USU_APELLIDO,
                                            USU_LINK = links.USU_LINK,
                                            USU_LINK_CORTO = links.USU_LINK_CORTO,
                                            LNK_ACTIVO = tablas.DESCRIPCION,
                                            LAN_FEC_CAPACITACION = (DateTime)lanzamiento.LAN_FEC_CAPACITACION

                                        }).ToList();

                return View(CursosDisponible);
            }



        }
    }
}