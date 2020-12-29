using Microsoft.Ajax.Utilities;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebCIIPMaestrosERP.Models;
using WebCIIPMaestrosERP.Seguridad;
namespace WebCIIPMaestrosERP.Controllers
{

    [Acceder]
    public class MktDocenteCursoController : Controller
    {
        // GET: MktDocenteCurso


        public FileResult generarExcel()
        {
            byte[] buffer;

            using (MemoryStream ms = new MemoryStream())
            {
                //Todo el documento excel
                ExcelPackage ep = new ExcelPackage();
                //Crear un hoja
                ep.Workbook.Worksheets.Add("Reporte");

                ExcelWorksheet ew = ep.Workbook.Worksheets[1];

                //Ponemos nombre de las columnas
                ew.Cells[1, 1].Value = "Id Docente";
                ew.Cells[1, 2].Value = "Nombres";
                ew.Cells[1, 3].Value = "Apellidos";
                ew.Cells[1, 4].Value = "Email";
                ew.Cells[1, 5].Value = "Celular";


                ew.Column(1).Width = 5;
                ew.Column(2).Width = 60;
                ew.Column(3).Width = 60;
                ew.Column(4).Width = 50;
                ew.Column(5).Width = 10;
                


                using (var range = ew.Cells[1, 1, 1, 5])
                {
                    range.Style.Fill.PatternType = ExcelFillStyle.Solid;
                    range.Style.Font.Color.SetColor(Color.White);
                    range.Style.Fill.BackgroundColor.SetColor(Color.DarkRed);
                }

                List<MktDocenteCursoCLS> lista = (List<MktDocenteCursoCLS>)Session["SessionMktReporteByUser"];
                int nregistros = lista.Count;

                //Pendiente
                for (int i = 0; i < nregistros; i++)
                {
                    ew.Cells[i + 2, 1].Value = lista[i].DOC_ID;
                    ew.Cells[i + 2, 2].Value = lista[i].DOC_NOMBRES;
                    ew.Cells[i + 2, 3].Value = lista[i].DOC_APELLIDOS;
                    ew.Cells[i + 2, 4].Value = lista[i].DOC_EMAIL;
                    ew.Cells[i + 2, 5].Value = lista[i].DOC_CELULAR;

                }
                //Pendiente
                ep.SaveAs(ms);
                buffer = ms.ToArray();

            }

            return File(buffer, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

        }




        public ActionResult MktReporteByUser(MktDocenteCursoCLS oMktDocenteCursoCLS)
        {

            llenarCurso();
            llenarUsuarioMkt();

            ViewBag.CargaUsuMktAll = ListaUsuMkt;
            ViewBag.CargaCursos = ListaCursos;

            int idusuario = oMktDocenteCursoCLS.USU_ID;
            int idCurso = oMktDocenteCursoCLS.CUR_ID;
            int idLan = oMktDocenteCursoCLS.LAN_ID;

            

            List<MktDocenteCursoCLS> ListaDocente = null;

            using (var db = new DB_WebCIIPEntities())
            {
                if (idLan != 0 && idCurso != 0 && idusuario != 0)
                {

                    ListaDocente = (from DocenteCurso in db.MKT_DOCENTE_CURSO
                                    join Docente in db.MKT_DOCENTES
                                    on DocenteCurso.DOC_ID equals Docente.DOC_ID
                                    where DocenteCurso.MKT_ID == idusuario &&
                                            DocenteCurso.LAN_ID == idLan

                                    select new MktDocenteCursoCLS
                                    {
                                        DOC_ID = DocenteCurso.DOC_ID,
                                        LAN_ID = DocenteCurso.LAN_ID,
                                        MKT_ID = (int)DocenteCurso.MKT_ID,
                                        DOC_NOMBRES = Docente.DOC_NOMBRES,
                                        DOC_APELLIDOS = Docente.DOC_APELLIDOS,
                                        DOC_CELULAR = Docente.DOC_CELULAR,
                                        DOC_EMAIL = Docente.DOC_EMAIL
                                    }).ToList();
                    Session["SessionMktReporteByUser"] = ListaDocente;
                }
                else
                {

                    ListaDocente = (from DocenteCurso in db.MKT_DOCENTE_CURSO
                                    join Docente in db.MKT_DOCENTES
                                    on DocenteCurso.DOC_ID equals Docente.DOC_ID
                                    where DocenteCurso.MKT_ID == idusuario &&
                                            DocenteCurso.LAN_ID == idLan

                                    select new MktDocenteCursoCLS
                                    {
                                        DOC_ID = DocenteCurso.DOC_ID,
                                        LAN_ID = DocenteCurso.LAN_ID,
                                        MKT_ID = (int)DocenteCurso.MKT_ID,
                                        DOC_NOMBRES = Docente.DOC_NOMBRES,
                                        DOC_APELLIDOS = Docente.DOC_APELLIDOS,
                                        DOC_CELULAR = Docente.DOC_CELULAR,
                                        DOC_EMAIL = Docente.DOC_EMAIL
                                    }).ToList();
                    Session["SessionMktReporteByUser"] = ListaDocente;


                }

                return View(ListaDocente);
            }
        }


        public ActionResult Index( MktDocenteCursoCLS oMktDocenteCursoCLS)
        {

            llenarCurso();
            
            ViewBag.CargaCursos = ListaCursos;
            

            int idCurso = oMktDocenteCursoCLS.CUR_ID;
            int idLan = oMktDocenteCursoCLS.LAN_ID;

            int idusuario = (int)HttpContext.Session["UsuID"];

            List<MktDocenteCursoCLS> ListaDocente = null;

            using (var db = new DB_WebCIIPEntities())
            {
                if (idLan == 0)
                {
                    ListaDocente = (from DocenteCurso in db.MKT_DOCENTE_CURSO
                                    join Docente in db.MKT_DOCENTES
                                    on DocenteCurso.DOC_ID equals Docente.DOC_ID
                                    where DocenteCurso.MKT_ID == idusuario

                                    select new MktDocenteCursoCLS
                                    {
                                        DOC_ID = DocenteCurso.DOC_ID,
                                        LAN_ID = DocenteCurso.LAN_ID,
                                        MKT_ID = (int)DocenteCurso.MKT_ID,
                                        DOC_NOMBRES = Docente.DOC_NOMBRES,
                                        DOC_APELLIDOS = Docente.DOC_APELLIDOS,
                                        DOC_CELULAR = Docente.DOC_CELULAR,
                                        DOC_EMAIL = Docente.DOC_EMAIL
                                    }).ToList();
                }
                else {

                    ListaDocente = (from DocenteCurso in db.MKT_DOCENTE_CURSO
                                    join Docente in db.MKT_DOCENTES
                                    on DocenteCurso.DOC_ID equals Docente.DOC_ID
                                    where DocenteCurso.MKT_ID == idusuario &&
                                            DocenteCurso.LAN_ID == idLan

                                    select new MktDocenteCursoCLS
                                    {
                                        DOC_ID = DocenteCurso.DOC_ID,
                                        LAN_ID = DocenteCurso.LAN_ID,
                                        MKT_ID = (int)DocenteCurso.MKT_ID,
                                        DOC_NOMBRES = Docente.DOC_NOMBRES,
                                        DOC_APELLIDOS = Docente.DOC_APELLIDOS,
                                        DOC_CELULAR = Docente.DOC_CELULAR,
                                        DOC_EMAIL = Docente.DOC_EMAIL
                                    }).ToList();

                }

            }

            return View(ListaDocente);

        }


        List<SelectListItem> ListaCursos;
        private void llenarCurso()
        {

            int idusuario = (int)HttpContext.Session["UsuID"];

            using (var db = new DB_WebCIIPEntities())
            {

                ListaCursos = (from UsuarioLink in db.SEG_USUARIOS_LINKS
                               join cursos in db.MAE_CURSOS
                               on UsuarioLink.CUR_ID equals cursos.CUR_ID
                               where UsuarioLink.USU_ID == idusuario

                               select new SelectListItem
                               {
                                   Text = cursos.CUR_NOMBRE,
                                   Value = cursos.CUR_ID.ToString()

                               }).Distinct()
                               .ToList();
                ListaCursos.Insert(0, new SelectListItem { Text = "Elija una curso", Value = "" });
            }

        }

        List<SelectListItem> ListaUsuMkt;
        private void llenarUsuarioMkt()
        {

            

            using (var db = new DB_WebCIIPEntities())
            {

                ListaUsuMkt = (from UsuarioMkt in db.SEG_USUARIOS
                               where UsuarioMkt.ROL_ID == 1  //solo aquellos que tengan el perfil de marketing

                               select new SelectListItem
                               {
                                   Text = UsuarioMkt.USU_NOMBRES+" "+ UsuarioMkt.USU_APELLIDO,
                                   Value = UsuarioMkt.USU_ID.ToString()

                               }).ToList();
                ListaUsuMkt.Insert(0, new SelectListItem { Text = "Elija un usuario", Value = "" });
            }

        }

        private List<SelectListItem> GetLanzamientosList(int IdCurso)
        {

            using (var db = new DB_WebCIIPEntities())
            {
                var Lanzamientos = db.MAE_CURSOS_LANZAMIENTOS.Where(x => x.CUR_ID == IdCurso);
                var resp = Lanzamientos.Select(x => new SelectListItem()
                {
                    Value = x.LAN_ID.ToString(),
                    Text = x.LAN_ID.ToString() + " - " + x.LAN_FEC_CAPACITACION.ToString(),
                }).ToList();

                resp.Insert(0, new SelectListItem() { Value = "", Text = "Elija una opcion" });

                return resp;
            }
        }


        List<SelectListItem> ListaLanzamientos;
        public JsonResult GetLanzamientos(int IdCurso)
        {
            ListaLanzamientos = GetLanzamientosList(IdCurso);
            return Json(ListaLanzamientos, JsonRequestBehavior.AllowGet);



        }
    }
}