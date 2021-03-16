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
                ew.Cells[1, 6].Value = "Curso";
                ew.Cells[1, 7].Value = "Fecha Registro";
                ew.Cells[1, 8].Value = "Turno";


                ew.Column(1).Width = 5;
                ew.Column(2).Width = 60;
                ew.Column(3).Width = 60;
                ew.Column(4).Width = 50;
                ew.Column(5).Width = 10;
                ew.Column(6).Width = 30;
                ew.Column(7).Width = 30;
                ew.Column(8).Width = 30;



                using (var range = ew.Cells[1, 1, 1, 8])
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
                    ew.Cells[i + 2, 6].Value = lista[i].CUR_NOMBRE;
                    ew.Cells[i + 2, 7].Value = lista[i].DCU_FECCADENA;
                    ew.Cells[i + 2, 8].Value = lista[i].TurnoFechaCadena;

                }
                //Pendiente
                ep.SaveAs(ms);
                buffer = ms.ToArray();

            }

            return File(buffer, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet");

        }


        
        public ActionResult FiltersBySesionUsu(int CursId, int? LanId, int? page)
        {
            List<MktDocenteCursoCLS> ListaDocente = null;
            int UserId = (int)HttpContext.Session["UsuID"];

            MktDocenteCursoCLS GetListaReporte = new MktDocenteCursoCLS();
            page = page == null ? 1 : page;
            int cantidadRegistrosPorPagina = 10;

            if (UserId == 0)
            {
                using (var db = new DB_WebCIIPEntitiesERP())
                {

                    //excel

                    ListaDocente = (from DocenteCurso in db.MKT_DOCENTE_CURSO
                                    join Docente in db.MKT_DOCENTES
                                    on DocenteCurso.DOC_ID equals Docente.DOC_ID
                                    join lanzamiento in db.MAE_CURSOS_LANZAMIENTOS
                                    on DocenteCurso.LAN_ID equals lanzamiento.LAN_ID
                                    join curso in db.MAE_CURSOS
                                    on lanzamiento.CUR_ID equals curso.CUR_ID
                                    orderby DocenteCurso.DCU_FEC descending



                                    select new MktDocenteCursoCLS
                                    {
                                        DOC_ID = DocenteCurso.DOC_ID,
                                        LAN_ID = DocenteCurso.LAN_ID,
                                        MKT_ID = (int)DocenteCurso.MKT_ID,
                                        DOC_NOMBRES = Docente.DOC_NOMBRES,
                                        DOC_NOMBRES_APELLIDOS = Docente.DOC_NOMBRES + ' ' + Docente.DOC_APELLIDOS,
                                        DOC_APELLIDOS = Docente.DOC_APELLIDOS,
                                        DOC_CELULAR = Docente.DOC_CELULAR,
                                        DOC_EMAIL = Docente.DOC_EMAIL,
                                        DCU_FEC = (DateTime)DocenteCurso.DCU_FEC,
                                        DCU_FECCADENA = ((DateTime)DocenteCurso.DCU_FEC).ToString(),
                                        TurnoFecha = (DateTime)lanzamiento.LAN_FEC_CAPACITACION,
                                        TurnoFechaCadena = ((DateTime)lanzamiento.LAN_FEC_CAPACITACION).ToString(),
                                        CUR_NOMBRE = curso.CUR_DESCRIPCION
                                    }).ToList();
                    //fin excel

                    GetListaReporte.Listado = (from DocenteCurso in db.MKT_DOCENTE_CURSO
                                               join Docente in db.MKT_DOCENTES
                                               on DocenteCurso.DOC_ID equals Docente.DOC_ID
                                               join lanzamiento in db.MAE_CURSOS_LANZAMIENTOS
                                               on DocenteCurso.LAN_ID equals lanzamiento.LAN_ID
                                               join curso in db.MAE_CURSOS
                                               on lanzamiento.CUR_ID equals curso.CUR_ID
                                               orderby DocenteCurso.DCU_FEC descending



                                               select new MktDocenteCursoCLS
                                               {
                                                   DOC_ID = DocenteCurso.DOC_ID,
                                                   LAN_ID = DocenteCurso.LAN_ID,
                                                   MKT_ID = (int)DocenteCurso.MKT_ID,
                                                   DOC_NOMBRES = Docente.DOC_NOMBRES,
                                                   //DOC_NOMBRES_APELLIDOS = Docente.DOC_NOMBRES + ' ' + Docente.DOC_APELLIDOS,
                                                   DOC_APELLIDOS = Docente.DOC_APELLIDOS,
                                                   DOC_CELULAR = Docente.DOC_CELULAR,
                                                   DOC_EMAIL = Docente.DOC_EMAIL,
                                                   DCU_FEC = (DateTime)DocenteCurso.DCU_FEC,
                                                   DCU_FECCADENA = ((DateTime)DocenteCurso.DCU_FEC).ToString(),
                                                   CUR_NOMBRE = curso.CUR_DESCRIPCION
                                               }).Skip(((int)page - 1) * cantidadRegistrosPorPagina)
                     .Take(cantidadRegistrosPorPagina)
                     .ToList();

                    GetListaReporte.RegPerPage = cantidadRegistrosPorPagina;
                    GetListaReporte.Page = (int)page;
                    GetListaReporte.TotalReg = (from DocenteCurso in db.MKT_DOCENTE_CURSO
                                                join Docente in db.MKT_DOCENTES
                                                on DocenteCurso.DOC_ID equals Docente.DOC_ID
                                                join lanzamiento in db.MAE_CURSOS_LANZAMIENTOS
                                                on DocenteCurso.LAN_ID equals lanzamiento.LAN_ID
                                                join curso in db.MAE_CURSOS
                                                on lanzamiento.CUR_ID equals curso.CUR_ID
                                                orderby DocenteCurso.DOC_ID

                                                select new MktDocenteCursoCLS
                                                {
                                                    DOC_ID = DocenteCurso.DOC_ID,
                                                    LAN_ID = DocenteCurso.LAN_ID,
                                                    MKT_ID = (int)DocenteCurso.MKT_ID,
                                                    DOC_NOMBRES = Docente.DOC_NOMBRES,
                                                    DOC_APELLIDOS = Docente.DOC_APELLIDOS,
                                                    //DOC_NOMBRES_APELLIDOS = Docente.DOC_NOMBRES + ' ' + Docente.DOC_APELLIDOS,
                                                    DOC_CELULAR = Docente.DOC_CELULAR,
                                                    DOC_EMAIL = Docente.DOC_EMAIL,
                                                    DCU_FEC = (DateTime)DocenteCurso.DCU_FEC,
                                                    DCU_FECCADENA = ((DateTime)DocenteCurso.DCU_FEC).ToString(),
                                                    TurnoFecha = (DateTime)lanzamiento.LAN_FEC_CAPACITACION,
                                                    TurnoFechaCadena = ((DateTime)lanzamiento.LAN_FEC_CAPACITACION).ToString(),
                                                    CUR_NOMBRE = curso.CUR_DESCRIPCION
                                                }).ToList().Count();
                }
            }
            else if (UserId != 0 && CursId == 0)
            {
                using (var db = new DB_WebCIIPEntitiesERP())
                {

                    //excel

                    ListaDocente = (from DocenteCurso in db.MKT_DOCENTE_CURSO
                                    join Docente in db.MKT_DOCENTES
                                    on DocenteCurso.DOC_ID equals Docente.DOC_ID

                                    join lanzamiento in db.MAE_CURSOS_LANZAMIENTOS
                                    on DocenteCurso.LAN_ID equals lanzamiento.LAN_ID

                                    join curso in db.MAE_CURSOS
                                    on lanzamiento.CUR_ID equals curso.CUR_ID

                                    where DocenteCurso.MKT_ID == (int)UserId
                                    orderby DocenteCurso.DCU_FEC descending

                                    select new MktDocenteCursoCLS
                                    {
                                        DOC_ID = DocenteCurso.DOC_ID,
                                        LAN_ID = DocenteCurso.LAN_ID,
                                        MKT_ID = (int)DocenteCurso.MKT_ID,
                                        DOC_NOMBRES = Docente.DOC_NOMBRES,
                                        DOC_APELLIDOS = Docente.DOC_APELLIDOS,
                                        //DOC_NOMBRES_APELLIDOS = (string)Docente.DOC_NOMBRES + ' ' + (string)Docente.DOC_APELLIDOS,
                                        DOC_CELULAR = Docente.DOC_CELULAR,
                                        DOC_EMAIL = Docente.DOC_EMAIL,
                                        DCU_FEC = (DateTime)DocenteCurso.DCU_FEC,
                                        DCU_FECCADENA = ((DateTime)DocenteCurso.DCU_FEC).ToString(),
                                        TurnoFecha = (DateTime)lanzamiento.LAN_FEC_CAPACITACION,
                                        TurnoFechaCadena = ((DateTime)lanzamiento.LAN_FEC_CAPACITACION).ToString(),
                                        CUR_NOMBRE = curso.CUR_DESCRIPCION
                                    }).ToList();

                    //fin excel

                    GetListaReporte.Listado = (from DocenteCurso in db.MKT_DOCENTE_CURSO
                                               join Docente in db.MKT_DOCENTES
                                               on DocenteCurso.DOC_ID equals Docente.DOC_ID

                                               join lanzamiento in db.MAE_CURSOS_LANZAMIENTOS
                                               on DocenteCurso.LAN_ID equals lanzamiento.LAN_ID

                                               join curso in db.MAE_CURSOS
                                               on lanzamiento.CUR_ID equals curso.CUR_ID

                                               where DocenteCurso.MKT_ID == (int)UserId
                                               orderby DocenteCurso.DCU_FEC descending

                                               select new MktDocenteCursoCLS
                                               {
                                                   DOC_ID = DocenteCurso.DOC_ID,
                                                   LAN_ID = DocenteCurso.LAN_ID,
                                                   MKT_ID = (int)DocenteCurso.MKT_ID,
                                                   DOC_NOMBRES = Docente.DOC_NOMBRES,
                                                   DOC_APELLIDOS = Docente.DOC_APELLIDOS,
                                                   //DOC_NOMBRES_APELLIDOS = (string)Docente.DOC_NOMBRES + ' ' + (string)Docente.DOC_APELLIDOS,
                                                   DOC_CELULAR = Docente.DOC_CELULAR,
                                                   DOC_EMAIL = Docente.DOC_EMAIL,
                                                   DCU_FEC = (DateTime)DocenteCurso.DCU_FEC,
                                                   DCU_FECCADENA = ((DateTime)DocenteCurso.DCU_FEC).ToString(),
                                                   TurnoFecha = (DateTime)lanzamiento.LAN_FEC_CAPACITACION,
                                                   TurnoFechaCadena = ((DateTime)lanzamiento.LAN_FEC_CAPACITACION).ToString(),
                                                   CUR_NOMBRE = curso.CUR_DESCRIPCION
                                               }).Skip(((int)page - 1) * cantidadRegistrosPorPagina)
                                     .Take(cantidadRegistrosPorPagina)
                                     .ToList();

                    GetListaReporte.RegPerPage = cantidadRegistrosPorPagina;
                    GetListaReporte.Page = (int)page;
                    GetListaReporte.TotalReg = (from DocenteCurso in db.MKT_DOCENTE_CURSO
                                                join Docente in db.MKT_DOCENTES
                                                on DocenteCurso.DOC_ID equals Docente.DOC_ID
                                                join lanzamiento in db.MAE_CURSOS_LANZAMIENTOS
                                                on DocenteCurso.LAN_ID equals lanzamiento.LAN_ID
                                                join curso in db.MAE_CURSOS
                                                on lanzamiento.CUR_ID equals curso.CUR_ID
                                                where DocenteCurso.MKT_ID == UserId
                                                orderby DocenteCurso.DOC_ID



                                                select new MktDocenteCursoCLS
                                                {
                                                    DOC_ID = DocenteCurso.DOC_ID,
                                                    LAN_ID = DocenteCurso.LAN_ID,
                                                    MKT_ID = (int)DocenteCurso.MKT_ID,
                                                    DOC_NOMBRES = Docente.DOC_NOMBRES,
                                                    DOC_APELLIDOS = Docente.DOC_APELLIDOS,
                                                    //DOC_NOMBRES_APELLIDOS = (string)Docente.DOC_NOMBRES + ' ' + Docente.DOC_APELLIDOS,
                                                    DOC_CELULAR = Docente.DOC_CELULAR,
                                                    DOC_EMAIL = Docente.DOC_EMAIL,
                                                    DCU_FEC = (DateTime)DocenteCurso.DCU_FEC,
                                                    DCU_FECCADENA = ((DateTime)DocenteCurso.DCU_FEC).ToString(),
                                                    TurnoFecha = (DateTime)lanzamiento.LAN_FEC_CAPACITACION,
                                                    TurnoFechaCadena = ((DateTime)lanzamiento.LAN_FEC_CAPACITACION).ToString(),
                                                    CUR_NOMBRE = curso.CUR_DESCRIPCION
                                                }).ToList().Count();
                }
            }
            else if (UserId != 0 && CursId != 0 && LanId == 0)
            {
                using (var db = new DB_WebCIIPEntitiesERP())
                {


                    //excel

                    ListaDocente = (from DocenteCurso in db.MKT_DOCENTE_CURSO
                                    join Docente in db.MKT_DOCENTES
                                    on DocenteCurso.DOC_ID equals Docente.DOC_ID
                                    join lanzamiento in db.MAE_CURSOS_LANZAMIENTOS
                                    on DocenteCurso.LAN_ID equals lanzamiento.LAN_ID
                                    join curso in db.MAE_CURSOS
                                    on lanzamiento.CUR_ID equals curso.CUR_ID
                                    where DocenteCurso.MKT_ID == UserId &&
                                            lanzamiento.CUR_ID == CursId
                                    orderby DocenteCurso.DCU_FEC descending


                                    select new MktDocenteCursoCLS
                                    {
                                        DOC_ID = DocenteCurso.DOC_ID,
                                        LAN_ID = DocenteCurso.LAN_ID,
                                        MKT_ID = (int)DocenteCurso.MKT_ID,
                                        DOC_NOMBRES = Docente.DOC_NOMBRES,
                                        DOC_APELLIDOS = Docente.DOC_APELLIDOS,
                                        //DOC_NOMBRES_APELLIDOS = Docente.DOC_NOMBRES + ' ' + Docente.DOC_APELLIDOS,
                                        DOC_CELULAR = Docente.DOC_CELULAR,
                                        DOC_EMAIL = Docente.DOC_EMAIL,
                                        DCU_FEC = (DateTime)DocenteCurso.DCU_FEC,
                                        DCU_FECCADENA = ((DateTime)DocenteCurso.DCU_FEC).ToString(),
                                        TurnoFecha = (DateTime)lanzamiento.LAN_FEC_CAPACITACION,
                                        TurnoFechaCadena = ((DateTime)lanzamiento.LAN_FEC_CAPACITACION).ToString(),
                                        CUR_NOMBRE = curso.CUR_DESCRIPCION
                                    }).ToList();
                    //fin excel

                    GetListaReporte.Listado = (from DocenteCurso in db.MKT_DOCENTE_CURSO
                                               join Docente in db.MKT_DOCENTES
                                               on DocenteCurso.DOC_ID equals Docente.DOC_ID
                                               join lanzamiento in db.MAE_CURSOS_LANZAMIENTOS
                                               on DocenteCurso.LAN_ID equals lanzamiento.LAN_ID
                                               join curso in db.MAE_CURSOS
                                               on lanzamiento.CUR_ID equals curso.CUR_ID
                                               where DocenteCurso.MKT_ID == UserId &&
                                                       lanzamiento.CUR_ID == CursId
                                               orderby DocenteCurso.DCU_FEC descending


                                               select new MktDocenteCursoCLS
                                               {
                                                   DOC_ID = DocenteCurso.DOC_ID,
                                                   LAN_ID = DocenteCurso.LAN_ID,
                                                   MKT_ID = (int)DocenteCurso.MKT_ID,
                                                   DOC_NOMBRES = Docente.DOC_NOMBRES,
                                                   DOC_APELLIDOS = Docente.DOC_APELLIDOS,
                                                   //DOC_NOMBRES_APELLIDOS = Docente.DOC_NOMBRES + ' ' + Docente.DOC_APELLIDOS,
                                                   DOC_CELULAR = Docente.DOC_CELULAR,
                                                   DOC_EMAIL = Docente.DOC_EMAIL,
                                                   DCU_FEC = (DateTime)DocenteCurso.DCU_FEC,
                                                   DCU_FECCADENA = ((DateTime)DocenteCurso.DCU_FEC).ToString(),
                                                   TurnoFecha = (DateTime)lanzamiento.LAN_FEC_CAPACITACION,
                                                   TurnoFechaCadena = ((DateTime)lanzamiento.LAN_FEC_CAPACITACION).ToString(),
                                                   CUR_NOMBRE = curso.CUR_DESCRIPCION
                                               }).Skip(((int)page - 1) * cantidadRegistrosPorPagina)
                     .Take(cantidadRegistrosPorPagina)
                     .ToList();

                    GetListaReporte.RegPerPage = cantidadRegistrosPorPagina;
                    GetListaReporte.Page = (int)page;
                    GetListaReporte.TotalReg = (from DocenteCurso in db.MKT_DOCENTE_CURSO
                                                join Docente in db.MKT_DOCENTES
                                                on DocenteCurso.DOC_ID equals Docente.DOC_ID
                                                join lanzamiento in db.MAE_CURSOS_LANZAMIENTOS
                                                on DocenteCurso.LAN_ID equals lanzamiento.LAN_ID
                                                join curso in db.MAE_CURSOS
                                                on lanzamiento.CUR_ID equals curso.CUR_ID
                                                where DocenteCurso.MKT_ID == UserId &&
                                                        lanzamiento.CUR_ID == CursId


                                                select new MktDocenteCursoCLS
                                                {
                                                    DOC_ID = DocenteCurso.DOC_ID,
                                                    LAN_ID = DocenteCurso.LAN_ID,
                                                    MKT_ID = (int)DocenteCurso.MKT_ID,
                                                    DOC_NOMBRES = Docente.DOC_NOMBRES,
                                                    DOC_APELLIDOS = Docente.DOC_APELLIDOS,
                                                    //DOC_NOMBRES_APELLIDOS = Docente.DOC_NOMBRES + ' ' + Docente.DOC_APELLIDOS,
                                                    DOC_CELULAR = Docente.DOC_CELULAR,
                                                    DOC_EMAIL = Docente.DOC_EMAIL,
                                                    DCU_FEC = (DateTime)DocenteCurso.DCU_FEC,
                                                    DCU_FECCADENA = ((DateTime)DocenteCurso.DCU_FEC).ToString(),
                                                    TurnoFecha = (DateTime)lanzamiento.LAN_FEC_CAPACITACION,
                                                    TurnoFechaCadena = ((DateTime)lanzamiento.LAN_FEC_CAPACITACION).ToString(),
                                                    CUR_NOMBRE = curso.CUR_DESCRIPCION
                                                }).ToList().Count();
                }
            }
            else
            {
                using (var db = new DB_WebCIIPEntitiesERP())
                {

                    //excel

                    ListaDocente = (from DocenteCurso in db.MKT_DOCENTE_CURSO
                                    join Docente in db.MKT_DOCENTES
                                    on DocenteCurso.DOC_ID equals Docente.DOC_ID
                                    join lanzamiento in db.MAE_CURSOS_LANZAMIENTOS
                                    on DocenteCurso.LAN_ID equals lanzamiento.LAN_ID
                                    join curso in db.MAE_CURSOS
                                    on lanzamiento.CUR_ID equals curso.CUR_ID
                                    where DocenteCurso.MKT_ID == UserId &&
                                          lanzamiento.CUR_ID == CursId &&
                                          lanzamiento.LAN_ID == LanId
                                    orderby DocenteCurso.DCU_FEC descending

                                    select new MktDocenteCursoCLS
                                    {
                                        DOC_ID = DocenteCurso.DOC_ID,
                                        LAN_ID = DocenteCurso.LAN_ID,
                                        MKT_ID = (int)DocenteCurso.MKT_ID,
                                        DOC_NOMBRES = Docente.DOC_NOMBRES,
                                        DOC_APELLIDOS = Docente.DOC_APELLIDOS,
                                        //DOC_NOMBRES_APELLIDOS = Docente.DOC_NOMBRES + ' ' + Docente.DOC_APELLIDOS,
                                        DOC_CELULAR = Docente.DOC_CELULAR,
                                        DOC_EMAIL = Docente.DOC_EMAIL,
                                        DCU_FEC = (DateTime)DocenteCurso.DCU_FEC,
                                        DCU_FECCADENA = ((DateTime)DocenteCurso.DCU_FEC).ToString(),
                                        TurnoFecha = (DateTime)lanzamiento.LAN_FEC_CAPACITACION,
                                        TurnoFechaCadena = ((DateTime)lanzamiento.LAN_FEC_CAPACITACION).ToString(),
                                        CUR_NOMBRE = curso.CUR_DESCRIPCION

                                    }).ToList();

                    //fin excel

                    GetListaReporte.Listado = (from DocenteCurso in db.MKT_DOCENTE_CURSO
                                               join Docente in db.MKT_DOCENTES
                                               on DocenteCurso.DOC_ID equals Docente.DOC_ID
                                               join lanzamiento in db.MAE_CURSOS_LANZAMIENTOS
                                               on DocenteCurso.LAN_ID equals lanzamiento.LAN_ID
                                               join curso in db.MAE_CURSOS
                                               on lanzamiento.CUR_ID equals curso.CUR_ID
                                               where DocenteCurso.MKT_ID == UserId &&
                                                     lanzamiento.CUR_ID == CursId &&
                                                     lanzamiento.LAN_ID == LanId
                                               orderby DocenteCurso.DCU_FEC descending

                                               select new MktDocenteCursoCLS
                                               {
                                                   DOC_ID = DocenteCurso.DOC_ID,
                                                   LAN_ID = DocenteCurso.LAN_ID,
                                                   MKT_ID = (int)DocenteCurso.MKT_ID,
                                                   DOC_NOMBRES = Docente.DOC_NOMBRES,
                                                   DOC_APELLIDOS = Docente.DOC_APELLIDOS,
                                                   //DOC_NOMBRES_APELLIDOS = Docente.DOC_NOMBRES + ' ' + Docente.DOC_APELLIDOS,
                                                   DOC_CELULAR = Docente.DOC_CELULAR,
                                                   DOC_EMAIL = Docente.DOC_EMAIL,
                                                   DCU_FEC = (DateTime)DocenteCurso.DCU_FEC,
                                                   DCU_FECCADENA = ((DateTime)DocenteCurso.DCU_FEC).ToString(),
                                                   TurnoFecha = (DateTime)lanzamiento.LAN_FEC_CAPACITACION,
                                                   TurnoFechaCadena = ((DateTime)lanzamiento.LAN_FEC_CAPACITACION).ToString(),
                                                   CUR_NOMBRE = curso.CUR_DESCRIPCION

                                               }).Skip(((int)page - 1) * cantidadRegistrosPorPagina)
                     .Take(cantidadRegistrosPorPagina)
                     .ToList();

                    GetListaReporte.RegPerPage = cantidadRegistrosPorPagina;
                    GetListaReporte.Page = (int)page;
                    GetListaReporte.TotalReg = (from DocenteCurso in db.MKT_DOCENTE_CURSO
                                                join Docente in db.MKT_DOCENTES
                                                on DocenteCurso.DOC_ID equals Docente.DOC_ID
                                                join lanzamiento in db.MAE_CURSOS_LANZAMIENTOS
                                                on DocenteCurso.LAN_ID equals lanzamiento.LAN_ID
                                                join curso in db.MAE_CURSOS
                                                on lanzamiento.CUR_ID equals curso.CUR_ID
                                                where DocenteCurso.MKT_ID == UserId &&
                                                      lanzamiento.CUR_ID == CursId &&
                                                      lanzamiento.LAN_ID == LanId
                                                orderby DocenteCurso.DOC_ID

                                                select new MktDocenteCursoCLS
                                                {
                                                    DOC_ID = DocenteCurso.DOC_ID,
                                                    LAN_ID = DocenteCurso.LAN_ID,
                                                    MKT_ID = (int)DocenteCurso.MKT_ID,
                                                    DOC_NOMBRES = Docente.DOC_NOMBRES,
                                                    DOC_APELLIDOS = Docente.DOC_APELLIDOS,
                                                    //DOC_NOMBRES_APELLIDOS = Docente.DOC_NOMBRES + ' ' + Docente.DOC_APELLIDOS,
                                                    DOC_CELULAR = Docente.DOC_CELULAR,
                                                    DOC_EMAIL = Docente.DOC_EMAIL,
                                                    DCU_FEC = (DateTime)DocenteCurso.DCU_FEC,
                                                    DCU_FECCADENA = ((DateTime)DocenteCurso.DCU_FEC).ToString(),
                                                    TurnoFecha = (DateTime)lanzamiento.LAN_FEC_CAPACITACION,
                                                    TurnoFechaCadena = ((DateTime)lanzamiento.LAN_FEC_CAPACITACION).ToString(),
                                                    CUR_NOMBRE = curso.CUR_DESCRIPCION

                                                }).ToList().Count();
                }
            }

            ViewBag.ContarDocentesCapturados = GetListaReporte.TotalReg;

            Session["SessionMktReporteByUser"] = ListaDocente;

            return PartialView("_MktReporteByUser", GetListaReporte);
        }

        public ActionResult Filters(int UserId, int? CursId, int? LanId, int? page)
        {
            MktDocenteCursoCLS GetListaReporte = new MktDocenteCursoCLS();
            List<MktDocenteCursoCLS> ListaDocente = null;

            page = page == null ? 1 : page;
            int cantidadRegistrosPorPagina = 10;

            if (UserId == 0)
            {
                using (var db = new DB_WebCIIPEntitiesERP())
                {

                    //excel

                    ListaDocente = (from DocenteCurso in db.MKT_DOCENTE_CURSO
                                               join Docente in db.MKT_DOCENTES
                                               on DocenteCurso.DOC_ID equals Docente.DOC_ID
                                               join lanzamiento in db.MAE_CURSOS_LANZAMIENTOS
                                               on DocenteCurso.LAN_ID equals lanzamiento.LAN_ID
                                               join curso in db.MAE_CURSOS
                                               on lanzamiento.CUR_ID equals curso.CUR_ID
                                               orderby DocenteCurso.DCU_FEC descending



                                               select new MktDocenteCursoCLS
                                               {
                                                   DOC_ID = DocenteCurso.DOC_ID,
                                                   LAN_ID = DocenteCurso.LAN_ID,
                                                   MKT_ID = (int)DocenteCurso.MKT_ID,
                                                   DOC_NOMBRES = Docente.DOC_NOMBRES,
                                                   //DOC_NOMBRES_APELLIDOS = Docente.DOC_NOMBRES + ' ' + Docente.DOC_APELLIDOS,
                                                   DOC_APELLIDOS = Docente.DOC_APELLIDOS,
                                                   DOC_CELULAR = Docente.DOC_CELULAR,
                                                   DOC_EMAIL = Docente.DOC_EMAIL,
                                                   DCU_FEC = (DateTime)DocenteCurso.DCU_FEC,
                                                   DCU_FECCADENA = ((DateTime)DocenteCurso.DCU_FEC).ToString(),
                                                   TurnoFecha = (DateTime)lanzamiento.LAN_FEC_CAPACITACION,
                                                   TurnoFechaCadena = ((DateTime)lanzamiento.LAN_FEC_CAPACITACION).ToString(),
                                                   CUR_NOMBRE = curso.CUR_DESCRIPCION
                                               }).ToList();

                    GetListaReporte.Listado = (from DocenteCurso in db.MKT_DOCENTE_CURSO
                     join Docente in db.MKT_DOCENTES
                     on DocenteCurso.DOC_ID equals Docente.DOC_ID
                     join lanzamiento in db.MAE_CURSOS_LANZAMIENTOS
                     on DocenteCurso.LAN_ID equals lanzamiento.LAN_ID
                     join curso in db.MAE_CURSOS
                     on lanzamiento.CUR_ID equals curso.CUR_ID
                     orderby DocenteCurso.DCU_FEC descending



                     select new MktDocenteCursoCLS
                     {
                         DOC_ID = DocenteCurso.DOC_ID,
                         LAN_ID = DocenteCurso.LAN_ID,
                         MKT_ID = (int)DocenteCurso.MKT_ID,
                         DOC_NOMBRES = Docente.DOC_NOMBRES,
                         //DOC_NOMBRES_APELLIDOS = Docente.DOC_NOMBRES+' '+ Docente.DOC_APELLIDOS,
                         DOC_APELLIDOS = Docente.DOC_APELLIDOS,
                         DOC_CELULAR = Docente.DOC_CELULAR,
                         DOC_EMAIL = Docente.DOC_EMAIL,
                         DCU_FEC = (DateTime)DocenteCurso.DCU_FEC,
                         DCU_FECCADENA = ((DateTime)DocenteCurso.DCU_FEC).ToString(),
                         CUR_NOMBRE = curso.CUR_DESCRIPCION
                     }).Skip(((int)page - 1) * cantidadRegistrosPorPagina)
                     .Take(cantidadRegistrosPorPagina)
                     .ToList();

                    GetListaReporte.RegPerPage = cantidadRegistrosPorPagina;
                    GetListaReporte.Page = (int)page;
                    GetListaReporte.TotalReg = (from DocenteCurso in db.MKT_DOCENTE_CURSO
                                                join Docente in db.MKT_DOCENTES
                                                on DocenteCurso.DOC_ID equals Docente.DOC_ID
                                                join lanzamiento in db.MAE_CURSOS_LANZAMIENTOS
                                                on DocenteCurso.LAN_ID equals lanzamiento.LAN_ID
                                                join curso in db.MAE_CURSOS
                                                on lanzamiento.CUR_ID equals curso.CUR_ID
                                                orderby DocenteCurso.DOC_ID

                                                select new MktDocenteCursoCLS
                                                {
                                                    DOC_ID = DocenteCurso.DOC_ID,
                                                    LAN_ID = DocenteCurso.LAN_ID,
                                                    MKT_ID = (int)DocenteCurso.MKT_ID,
                                                    DOC_NOMBRES = Docente.DOC_NOMBRES,
                                                    DOC_APELLIDOS = Docente.DOC_APELLIDOS,
                                                    //DOC_NOMBRES_APELLIDOS = Docente.DOC_NOMBRES + ' ' + Docente.DOC_APELLIDOS,
                                                    DOC_CELULAR = Docente.DOC_CELULAR,
                                                    DOC_EMAIL = Docente.DOC_EMAIL,
                                                    DCU_FEC = (DateTime)DocenteCurso.DCU_FEC,
                                                    DCU_FECCADENA = ((DateTime)DocenteCurso.DCU_FEC).ToString(),
                                                    TurnoFecha = (DateTime)lanzamiento.LAN_FEC_CAPACITACION,
                                                    TurnoFechaCadena = ((DateTime)lanzamiento.LAN_FEC_CAPACITACION).ToString(),
                                                    CUR_NOMBRE = curso.CUR_DESCRIPCION
                                                }).ToList().Count();
                }
            }else if(UserId != 0 && CursId == 0) //con usuario
            {
                using(var db =  new DB_WebCIIPEntitiesERP())
                {


                    //excel

                    ListaDocente = (from DocenteCurso in db.MKT_DOCENTE_CURSO
                                    join Docente in db.MKT_DOCENTES
                                    on DocenteCurso.DOC_ID equals Docente.DOC_ID

                                    join lanzamiento in db.MAE_CURSOS_LANZAMIENTOS
                                    on DocenteCurso.LAN_ID equals lanzamiento.LAN_ID

                                    join curso in db.MAE_CURSOS
                                    on lanzamiento.CUR_ID equals curso.CUR_ID

                                    where DocenteCurso.MKT_ID == (int)UserId
                                    orderby DocenteCurso.DCU_FEC descending

                                    select new MktDocenteCursoCLS
                                    {
                                        DOC_ID = DocenteCurso.DOC_ID,
                                        LAN_ID = DocenteCurso.LAN_ID,
                                        MKT_ID = (int)DocenteCurso.MKT_ID,
                                        DOC_NOMBRES = Docente.DOC_NOMBRES,
                                        DOC_APELLIDOS = Docente.DOC_APELLIDOS,
                                        //DOC_NOMBRES_APELLIDOS = (string)Docente.DOC_NOMBRES + ' ' + (string)Docente.DOC_APELLIDOS,
                                        DOC_CELULAR = Docente.DOC_CELULAR,
                                        DOC_EMAIL = Docente.DOC_EMAIL,
                                        DCU_FEC = (DateTime)DocenteCurso.DCU_FEC,
                                        DCU_FECCADENA = ((DateTime)DocenteCurso.DCU_FEC).ToString(),
                                        TurnoFecha = (DateTime)lanzamiento.LAN_FEC_CAPACITACION,
                                        TurnoFechaCadena = ((DateTime)lanzamiento.LAN_FEC_CAPACITACION).ToString(),
                                        CUR_NOMBRE = curso.CUR_DESCRIPCION
                                    }).ToList();

                    //fin excel

                    GetListaReporte.Listado = (from DocenteCurso in db.MKT_DOCENTE_CURSO
                                     join Docente in db.MKT_DOCENTES
                                     on DocenteCurso.DOC_ID equals Docente.DOC_ID

                                     join lanzamiento in db.MAE_CURSOS_LANZAMIENTOS
                                     on DocenteCurso.LAN_ID equals lanzamiento.LAN_ID

                                     join curso in db.MAE_CURSOS
                                     on lanzamiento.CUR_ID equals curso.CUR_ID

                                     where DocenteCurso.MKT_ID == (int)UserId
                                     orderby DocenteCurso.DCU_FEC descending

                                               select new MktDocenteCursoCLS
                                     {
                                         DOC_ID = DocenteCurso.DOC_ID,
                                         LAN_ID = DocenteCurso.LAN_ID,
                                         MKT_ID = (int)DocenteCurso.MKT_ID,
                                         DOC_NOMBRES = Docente.DOC_NOMBRES,
                                         DOC_APELLIDOS = Docente.DOC_APELLIDOS,
                                         //DOC_NOMBRES_APELLIDOS = (string)Docente.DOC_NOMBRES + ' ' + (string)Docente.DOC_APELLIDOS,
                                         DOC_CELULAR = Docente.DOC_CELULAR,
                                         DOC_EMAIL = Docente.DOC_EMAIL,
                                         DCU_FEC = (DateTime)DocenteCurso.DCU_FEC,
                                         DCU_FECCADENA = ((DateTime)DocenteCurso.DCU_FEC).ToString(),
                                         TurnoFecha = (DateTime)lanzamiento.LAN_FEC_CAPACITACION,
                                         TurnoFechaCadena = ((DateTime)lanzamiento.LAN_FEC_CAPACITACION).ToString(),
                                         CUR_NOMBRE = curso.CUR_DESCRIPCION
                                               }).Skip(((int)page - 1) * cantidadRegistrosPorPagina)
                                     .Take(cantidadRegistrosPorPagina)
                                     .ToList();

                    GetListaReporte.RegPerPage = cantidadRegistrosPorPagina;
                    GetListaReporte.Page = (int)page;
                    GetListaReporte.TotalReg = (from DocenteCurso in db.MKT_DOCENTE_CURSO
                                                join Docente in db.MKT_DOCENTES
                                                on DocenteCurso.DOC_ID equals Docente.DOC_ID
                                                join lanzamiento in db.MAE_CURSOS_LANZAMIENTOS
                                                on DocenteCurso.LAN_ID equals lanzamiento.LAN_ID
                                                join curso in db.MAE_CURSOS
                                                on lanzamiento.CUR_ID equals curso.CUR_ID
                                                where DocenteCurso.MKT_ID == UserId
                                                orderby DocenteCurso.DOC_ID



                                                select new MktDocenteCursoCLS
                                                {
                                                    DOC_ID = DocenteCurso.DOC_ID,
                                                    LAN_ID = DocenteCurso.LAN_ID,
                                                    MKT_ID = (int)DocenteCurso.MKT_ID,
                                                    DOC_NOMBRES = Docente.DOC_NOMBRES,
                                                    DOC_APELLIDOS = Docente.DOC_APELLIDOS,
                                                    //DOC_NOMBRES_APELLIDOS = (string)Docente.DOC_NOMBRES + ' ' + Docente.DOC_APELLIDOS,
                                                    DOC_CELULAR = Docente.DOC_CELULAR,
                                                    DOC_EMAIL = Docente.DOC_EMAIL,
                                                    DCU_FEC = (DateTime)DocenteCurso.DCU_FEC,
                                                    DCU_FECCADENA = ((DateTime)DocenteCurso.DCU_FEC).ToString(),
                                                    TurnoFecha = (DateTime)lanzamiento.LAN_FEC_CAPACITACION,
                                                    TurnoFechaCadena = ((DateTime)lanzamiento.LAN_FEC_CAPACITACION).ToString(),
                                                    CUR_NOMBRE = curso.CUR_DESCRIPCION
                                                }).ToList().Count();
                }
            }else if(UserId != 0 && CursId != 0 && LanId == 0)
            {
                using (var db = new DB_WebCIIPEntitiesERP())
                {


                    //excel

                    ListaDocente = (from DocenteCurso in db.MKT_DOCENTE_CURSO
                                    join Docente in db.MKT_DOCENTES
                                    on DocenteCurso.DOC_ID equals Docente.DOC_ID
                                    join lanzamiento in db.MAE_CURSOS_LANZAMIENTOS
                                    on DocenteCurso.LAN_ID equals lanzamiento.LAN_ID
                                    join curso in db.MAE_CURSOS
                                    on lanzamiento.CUR_ID equals curso.CUR_ID
                                    where DocenteCurso.MKT_ID == UserId &&
                                            lanzamiento.CUR_ID == CursId
                                    orderby DocenteCurso.DCU_FEC descending


                                    select new MktDocenteCursoCLS
                                    {
                                        DOC_ID = DocenteCurso.DOC_ID,
                                        LAN_ID = DocenteCurso.LAN_ID,
                                        MKT_ID = (int)DocenteCurso.MKT_ID,
                                        DOC_NOMBRES = Docente.DOC_NOMBRES,
                                        DOC_APELLIDOS = Docente.DOC_APELLIDOS,
                                        //DOC_NOMBRES_APELLIDOS = Docente.DOC_NOMBRES + ' ' + Docente.DOC_APELLIDOS,
                                        DOC_CELULAR = Docente.DOC_CELULAR,
                                        DOC_EMAIL = Docente.DOC_EMAIL,
                                        DCU_FEC = (DateTime)DocenteCurso.DCU_FEC,
                                        DCU_FECCADENA = ((DateTime)DocenteCurso.DCU_FEC).ToString(),
                                        TurnoFecha = (DateTime)lanzamiento.LAN_FEC_CAPACITACION,
                                        TurnoFechaCadena = ((DateTime)lanzamiento.LAN_FEC_CAPACITACION).ToString(),
                                        CUR_NOMBRE = curso.CUR_DESCRIPCION
                                    }).ToList();

                    //fin excel


                    GetListaReporte.Listado = (from DocenteCurso in db.MKT_DOCENTE_CURSO
                     join Docente in db.MKT_DOCENTES
                     on DocenteCurso.DOC_ID equals Docente.DOC_ID
                     join lanzamiento in db.MAE_CURSOS_LANZAMIENTOS
                     on DocenteCurso.LAN_ID equals lanzamiento.LAN_ID
                     join curso in db.MAE_CURSOS
                     on lanzamiento.CUR_ID equals curso.CUR_ID
                     where DocenteCurso.MKT_ID == UserId &&
                             lanzamiento.CUR_ID == CursId
                     orderby DocenteCurso.DCU_FEC descending


                                               select new MktDocenteCursoCLS
                     {
                         DOC_ID = DocenteCurso.DOC_ID,
                         LAN_ID = DocenteCurso.LAN_ID,
                         MKT_ID = (int)DocenteCurso.MKT_ID,
                         DOC_NOMBRES = Docente.DOC_NOMBRES,
                         DOC_APELLIDOS = Docente.DOC_APELLIDOS,
                         //DOC_NOMBRES_APELLIDOS = Docente.DOC_NOMBRES + ' ' + Docente.DOC_APELLIDOS,
                         DOC_CELULAR = Docente.DOC_CELULAR,
                         DOC_EMAIL = Docente.DOC_EMAIL,
                         DCU_FEC = (DateTime)DocenteCurso.DCU_FEC,
                         DCU_FECCADENA = ((DateTime)DocenteCurso.DCU_FEC).ToString(),
                         TurnoFecha = (DateTime)lanzamiento.LAN_FEC_CAPACITACION,
                         TurnoFechaCadena = ((DateTime)lanzamiento.LAN_FEC_CAPACITACION).ToString(),
                         CUR_NOMBRE = curso.CUR_DESCRIPCION
                                               }).Skip(((int)page - 1) * cantidadRegistrosPorPagina)
                     .Take(cantidadRegistrosPorPagina)
                     .ToList();

                    GetListaReporte.RegPerPage = cantidadRegistrosPorPagina;
                    GetListaReporte.Page = (int)page;
                    GetListaReporte.TotalReg = (from DocenteCurso in db.MKT_DOCENTE_CURSO
                                                join Docente in db.MKT_DOCENTES
                                                on DocenteCurso.DOC_ID equals Docente.DOC_ID
                                                join lanzamiento in db.MAE_CURSOS_LANZAMIENTOS
                                                on DocenteCurso.LAN_ID equals lanzamiento.LAN_ID
                                                join curso in db.MAE_CURSOS
                                                on lanzamiento.CUR_ID equals curso.CUR_ID
                                                where DocenteCurso.MKT_ID == UserId &&
                                                        lanzamiento.CUR_ID == CursId


                                                select new MktDocenteCursoCLS
                                                {
                                                    DOC_ID = DocenteCurso.DOC_ID,
                                                    LAN_ID = DocenteCurso.LAN_ID,
                                                    MKT_ID = (int)DocenteCurso.MKT_ID,
                                                    DOC_NOMBRES = Docente.DOC_NOMBRES,
                                                    DOC_APELLIDOS = Docente.DOC_APELLIDOS,
                                                    //DOC_NOMBRES_APELLIDOS = Docente.DOC_NOMBRES + ' ' + Docente.DOC_APELLIDOS,
                                                    DOC_CELULAR = Docente.DOC_CELULAR,
                                                    DOC_EMAIL = Docente.DOC_EMAIL,
                                                    DCU_FEC = (DateTime)DocenteCurso.DCU_FEC,
                                                    DCU_FECCADENA = ((DateTime)DocenteCurso.DCU_FEC).ToString(),
                                                    TurnoFecha = (DateTime)lanzamiento.LAN_FEC_CAPACITACION,
                                                    TurnoFechaCadena = ((DateTime)lanzamiento.LAN_FEC_CAPACITACION).ToString(),
                                                    CUR_NOMBRE = curso.CUR_DESCRIPCION
                                                }).ToList().Count();
                }
            }
            else
            {
                using (var db = new DB_WebCIIPEntitiesERP())
                {

                    //excel

                    ListaDocente = (from DocenteCurso in db.MKT_DOCENTE_CURSO
                                    join Docente in db.MKT_DOCENTES
                                    on DocenteCurso.DOC_ID equals Docente.DOC_ID
                                    join lanzamiento in db.MAE_CURSOS_LANZAMIENTOS
                                    on DocenteCurso.LAN_ID equals lanzamiento.LAN_ID
                                    join curso in db.MAE_CURSOS
                                    on lanzamiento.CUR_ID equals curso.CUR_ID
                                    where DocenteCurso.MKT_ID == UserId &&
                                          lanzamiento.CUR_ID == CursId &&
                                          lanzamiento.LAN_ID == LanId
                                    orderby DocenteCurso.DCU_FEC descending

                                    select new MktDocenteCursoCLS
                                    {
                                        DOC_ID = DocenteCurso.DOC_ID,
                                        LAN_ID = DocenteCurso.LAN_ID,
                                        MKT_ID = (int)DocenteCurso.MKT_ID,
                                        DOC_NOMBRES = Docente.DOC_NOMBRES,
                                        DOC_APELLIDOS = Docente.DOC_APELLIDOS,
                                        //DOC_NOMBRES_APELLIDOS = Docente.DOC_NOMBRES + ' ' + Docente.DOC_APELLIDOS,
                                        DOC_CELULAR = Docente.DOC_CELULAR,
                                        DOC_EMAIL = Docente.DOC_EMAIL,
                                        DCU_FEC = (DateTime)DocenteCurso.DCU_FEC,
                                        DCU_FECCADENA = ((DateTime)DocenteCurso.DCU_FEC).ToString(),
                                        TurnoFecha = (DateTime)lanzamiento.LAN_FEC_CAPACITACION,
                                        TurnoFechaCadena = ((DateTime)lanzamiento.LAN_FEC_CAPACITACION).ToString(),
                                        CUR_NOMBRE = curso.CUR_DESCRIPCION

                                    }).ToList();
                    //fin excel

                    GetListaReporte.Listado = (from DocenteCurso in db.MKT_DOCENTE_CURSO
                     join Docente in db.MKT_DOCENTES
                     on DocenteCurso.DOC_ID equals Docente.DOC_ID
                     join lanzamiento in db.MAE_CURSOS_LANZAMIENTOS
                     on DocenteCurso.LAN_ID equals lanzamiento.LAN_ID
                     join curso in db.MAE_CURSOS
                     on lanzamiento.CUR_ID equals curso.CUR_ID
                     where DocenteCurso.MKT_ID == UserId &&
                           lanzamiento.CUR_ID == CursId &&
                           lanzamiento.LAN_ID == LanId
                     orderby DocenteCurso.DCU_FEC descending

                     select new MktDocenteCursoCLS
                     {
                         DOC_ID = DocenteCurso.DOC_ID,
                         LAN_ID = DocenteCurso.LAN_ID,
                         MKT_ID = (int)DocenteCurso.MKT_ID,
                         DOC_NOMBRES = Docente.DOC_NOMBRES,
                         DOC_APELLIDOS = Docente.DOC_APELLIDOS,
                         //DOC_NOMBRES_APELLIDOS = Docente.DOC_NOMBRES + ' ' + Docente.DOC_APELLIDOS,
                         DOC_CELULAR = Docente.DOC_CELULAR,
                         DOC_EMAIL = Docente.DOC_EMAIL,
                         DCU_FEC = (DateTime)DocenteCurso.DCU_FEC,
                         DCU_FECCADENA = ((DateTime)DocenteCurso.DCU_FEC).ToString(),
                         TurnoFecha = (DateTime)lanzamiento.LAN_FEC_CAPACITACION,
                         TurnoFechaCadena = ((DateTime)lanzamiento.LAN_FEC_CAPACITACION).ToString(),
                         CUR_NOMBRE = curso.CUR_DESCRIPCION

                     }).Skip(((int)page - 1) * cantidadRegistrosPorPagina)
                     .Take(cantidadRegistrosPorPagina)
                     .ToList();

                    GetListaReporte.RegPerPage = cantidadRegistrosPorPagina;
                    GetListaReporte.Page = (int)page;
                    GetListaReporte.TotalReg = (from DocenteCurso in db.MKT_DOCENTE_CURSO
                                                join Docente in db.MKT_DOCENTES
                                                on DocenteCurso.DOC_ID equals Docente.DOC_ID
                                                join lanzamiento in db.MAE_CURSOS_LANZAMIENTOS
                                                on DocenteCurso.LAN_ID equals lanzamiento.LAN_ID
                                                join curso in db.MAE_CURSOS
                                                on lanzamiento.CUR_ID equals curso.CUR_ID
                                                where DocenteCurso.MKT_ID == UserId &&
                                                      lanzamiento.CUR_ID == CursId &&
                                                      lanzamiento.LAN_ID == LanId
                                                orderby DocenteCurso.DOC_ID

                                                select new MktDocenteCursoCLS
                                                {
                                                    DOC_ID = DocenteCurso.DOC_ID,
                                                    LAN_ID = DocenteCurso.LAN_ID,
                                                    MKT_ID = (int)DocenteCurso.MKT_ID,
                                                    DOC_NOMBRES = Docente.DOC_NOMBRES,
                                                    DOC_APELLIDOS = Docente.DOC_APELLIDOS,
                                                    //DOC_NOMBRES_APELLIDOS = Docente.DOC_NOMBRES + ' ' + Docente.DOC_APELLIDOS,
                                                    DOC_CELULAR = Docente.DOC_CELULAR,
                                                    DOC_EMAIL = Docente.DOC_EMAIL,
                                                    DCU_FEC = (DateTime)DocenteCurso.DCU_FEC,
                                                    DCU_FECCADENA = ((DateTime)DocenteCurso.DCU_FEC).ToString(),
                                                    TurnoFecha = (DateTime)lanzamiento.LAN_FEC_CAPACITACION,
                                                    TurnoFechaCadena = ((DateTime)lanzamiento.LAN_FEC_CAPACITACION).ToString(),
                                                    CUR_NOMBRE = curso.CUR_DESCRIPCION

                                                }).ToList().Count();
                }
            }

            Session["SessionMktReporteByUser"] = ListaDocente;

            ViewBag.ContarDocentesCapturados = GetListaReporte.TotalReg;

            return PartialView("_MktReporteByUser", GetListaReporte);
        }

        public ActionResult MktReporteByUser(MktDocenteCursoCLS oMktDocenteCursoCLS, int? page)
        {
            page = page == null ? 1 : page;
            int cantidadRegistrosPorPagina = 10;

            llenarCurso();
            llenarUsuarioMkt();

            ViewBag.CargaUsuMktAll = ListaUsuMkt;
            ViewBag.CargaCursos = ListaCursos;

            int idusuario = oMktDocenteCursoCLS.USU_ID;
            int idCurso = oMktDocenteCursoCLS.CUR_ID;
            int idLan = oMktDocenteCursoCLS.LAN_ID;

            //cargar listas


            MktDocenteCursoCLS GetAll = new MktDocenteCursoCLS();

            using (var db = new DB_WebCIIPEntitiesERP())
            {
                if (idusuario != 0 && idCurso != 0 && idLan != 0)
                {

                    GetAll.Listado = (from DocenteCurso in db.MKT_DOCENTE_CURSO
                                    join Docente in db.MKT_DOCENTES
                                    on DocenteCurso.DOC_ID equals Docente.DOC_ID
                                    join lanzamiento in db.MAE_CURSOS_LANZAMIENTOS
                                    on DocenteCurso.LAN_ID equals lanzamiento.LAN_ID
                                    join curso in db.MAE_CURSOS
                                    on lanzamiento.CUR_ID equals curso.CUR_ID
                                    where DocenteCurso.MKT_ID == idusuario &&
                                          lanzamiento.CUR_ID == idCurso &&
                                          lanzamiento.LAN_ID == idLan
                                    orderby DocenteCurso.DOC_ID

                                      select new MktDocenteCursoCLS
                                    {
                                        DOC_ID = DocenteCurso.DOC_ID,
                                        LAN_ID = DocenteCurso.LAN_ID,
                                        MKT_ID = (int)DocenteCurso.MKT_ID,
                                        DOC_NOMBRES = Docente.DOC_NOMBRES,
                                        DOC_APELLIDOS = Docente.DOC_APELLIDOS,
                                        //DOC_NOMBRES_APELLIDOS = Docente.DOC_NOMBRES +" "+ Docente.DOC_APELLIDOS,
                                        TurnoFecha = (DateTime)lanzamiento.LAN_FEC_CAPACITACION,
                                        TurnoFechaCadena = ((DateTime)lanzamiento.LAN_FEC_CAPACITACION).ToString(),
                                        DOC_CELULAR = Docente.DOC_CELULAR,
                                        DOC_EMAIL = Docente.DOC_EMAIL,
                                        DCU_FEC = (DateTime)DocenteCurso.DCU_FEC,
                                        DCU_FECCADENA = ((DateTime)DocenteCurso.DCU_FEC).ToString(),
                                        CUR_NOMBRE = curso.CUR_NOMBRE

                                    }).Skip(((int)page - 1) * cantidadRegistrosPorPagina)
                     .Take(cantidadRegistrosPorPagina)
                     .ToList();

                    GetAll.RegPerPage = cantidadRegistrosPorPagina;
                    GetAll.Page = (int)page;
                    GetAll.TotalReg = (from DocenteCurso in db.MKT_DOCENTE_CURSO
                                       join Docente in db.MKT_DOCENTES
                                       on DocenteCurso.DOC_ID equals Docente.DOC_ID
                                       join lanzamiento in db.MAE_CURSOS_LANZAMIENTOS
                                       on DocenteCurso.LAN_ID equals lanzamiento.LAN_ID
                                       join curso in db.MAE_CURSOS
                                       on lanzamiento.CUR_ID equals curso.CUR_ID
                                       where DocenteCurso.MKT_ID == idusuario &&
                                             lanzamiento.CUR_ID == idCurso &&
                                             lanzamiento.LAN_ID == idLan

                                       select new MktDocenteCursoCLS
                                       {
                                           DOC_ID = DocenteCurso.DOC_ID,
                                           LAN_ID = DocenteCurso.LAN_ID,
                                           MKT_ID = (int)DocenteCurso.MKT_ID,
                                           DOC_NOMBRES = Docente.DOC_NOMBRES,
                                           DOC_APELLIDOS = Docente.DOC_APELLIDOS,
                                           DOC_CELULAR = Docente.DOC_CELULAR,
                                           DOC_EMAIL = Docente.DOC_EMAIL,
                                           DCU_FEC = (DateTime)DocenteCurso.DCU_FEC,
                                           DCU_FECCADENA = ((DateTime)DocenteCurso.DCU_FEC).ToString(),
                                           CUR_NOMBRE = curso.CUR_NOMBRE

                                       }).ToList().Count();
                }
                else if (idusuario != 0 && idCurso != 0 && idLan == 0) //cuando quier ver todo del bot sin importar el curso
                {

                    GetAll.Listado = (from DocenteCurso in db.MKT_DOCENTE_CURSO
                                    join Docente in db.MKT_DOCENTES
                                    on DocenteCurso.DOC_ID equals Docente.DOC_ID
                                    join lanzamiento in db.MAE_CURSOS_LANZAMIENTOS
                                    on DocenteCurso.LAN_ID equals lanzamiento.LAN_ID
                                    join curso in db.MAE_CURSOS
                                    on lanzamiento.CUR_ID equals curso.CUR_ID
                                    where DocenteCurso.MKT_ID == idusuario &&
                                            lanzamiento.CUR_ID == idCurso
                                    orderby DocenteCurso.DOC_ID

                                      select new MktDocenteCursoCLS
                                    {
                                        DOC_ID = DocenteCurso.DOC_ID,
                                        LAN_ID = DocenteCurso.LAN_ID,
                                        MKT_ID = (int)DocenteCurso.MKT_ID,
                                        DOC_NOMBRES = Docente.DOC_NOMBRES,
                                        DOC_APELLIDOS = Docente.DOC_APELLIDOS,
                                        //DOC_NOMBRES_APELLIDOS = Docente.DOC_NOMBRES + " " + Docente.DOC_APELLIDOS,
                                        DOC_CELULAR = Docente.DOC_CELULAR,
                                        DOC_EMAIL = Docente.DOC_EMAIL,
                                        DCU_FEC = (DateTime)DocenteCurso.DCU_FEC,
                                        TurnoFecha = (DateTime)lanzamiento.LAN_FEC_CAPACITACION,
                                        TurnoFechaCadena = ((DateTime)lanzamiento.LAN_FEC_CAPACITACION).ToString(),
                                        DCU_FECCADENA = ((DateTime)DocenteCurso.DCU_FEC).ToString(),
                                        CUR_NOMBRE = curso.CUR_NOMBRE
                                    }).Skip(((int)page - 1) * cantidadRegistrosPorPagina)
                     .Take(cantidadRegistrosPorPagina)
                     .ToList();

                    GetAll.RegPerPage = cantidadRegistrosPorPagina;
                    GetAll.Page = (int)page;
                    GetAll.TotalReg = (from DocenteCurso in db.MKT_DOCENTE_CURSO
                                       join Docente in db.MKT_DOCENTES
                                       on DocenteCurso.DOC_ID equals Docente.DOC_ID
                                       join lanzamiento in db.MAE_CURSOS_LANZAMIENTOS
                                       on DocenteCurso.LAN_ID equals lanzamiento.LAN_ID
                                       join curso in db.MAE_CURSOS
                                       on lanzamiento.CUR_ID equals curso.CUR_ID
                                       where DocenteCurso.MKT_ID == idusuario &&
                                               lanzamiento.CUR_ID == idCurso

                                       select new MktDocenteCursoCLS
                                       {
                                           DOC_ID = DocenteCurso.DOC_ID,
                                           LAN_ID = DocenteCurso.LAN_ID,
                                           MKT_ID = (int)DocenteCurso.MKT_ID,
                                           DOC_NOMBRES = Docente.DOC_NOMBRES,
                                           DOC_APELLIDOS = Docente.DOC_APELLIDOS,
                                           DOC_CELULAR = Docente.DOC_CELULAR,
                                           DOC_EMAIL = Docente.DOC_EMAIL,
                                           DCU_FEC = (DateTime)DocenteCurso.DCU_FEC,
                                           DCU_FECCADENA = ((DateTime)DocenteCurso.DCU_FEC).ToString(),
                                           CUR_NOMBRE = curso.CUR_NOMBRE
                                       }).ToList().Count();
                }
                else if (idusuario != 0 && idCurso == 0 && idLan == 0) //cuando quier ver todo del bot sin importar el curso
                {

                    GetAll.Listado = (from DocenteCurso in db.MKT_DOCENTE_CURSO
                                    join Docente in db.MKT_DOCENTES
                                    on DocenteCurso.DOC_ID equals Docente.DOC_ID
                                    join lanzamiento in db.MAE_CURSOS_LANZAMIENTOS
                                    on DocenteCurso.LAN_ID equals lanzamiento.LAN_ID
                                    join curso in db.MAE_CURSOS
                                    on lanzamiento.CUR_ID equals curso.CUR_ID
                                    where DocenteCurso.MKT_ID == idusuario
                                    orderby DocenteCurso.DOC_ID


                                    select new MktDocenteCursoCLS
                                    {
                                        DOC_ID = DocenteCurso.DOC_ID,
                                        LAN_ID = DocenteCurso.LAN_ID,
                                        MKT_ID = (int)DocenteCurso.MKT_ID,
                                        DOC_NOMBRES = Docente.DOC_NOMBRES,
                                        DOC_APELLIDOS = Docente.DOC_APELLIDOS,
                                        //DOC_NOMBRES_APELLIDOS = Docente.DOC_NOMBRES + " " + Docente.DOC_APELLIDOS,
                                        DOC_CELULAR = Docente.DOC_CELULAR,
                                        DOC_EMAIL = Docente.DOC_EMAIL,
                                        DCU_FEC = (DateTime)DocenteCurso.DCU_FEC,
                                        DCU_FECCADENA = ((DateTime)DocenteCurso.DCU_FEC).ToString(),
                                        TurnoFecha = (DateTime)lanzamiento.LAN_FEC_CAPACITACION,
                                        TurnoFechaCadena = ((DateTime)lanzamiento.LAN_FEC_CAPACITACION).ToString(),
                                        CUR_NOMBRE = curso.CUR_NOMBRE
                                    }).Skip(((int)page - 1) * cantidadRegistrosPorPagina)
                     .Take(cantidadRegistrosPorPagina)
                     .ToList();

                    GetAll.RegPerPage = cantidadRegistrosPorPagina;
                    GetAll.Page = (int)page;
                    GetAll.TotalReg = (from DocenteCurso in db.MKT_DOCENTE_CURSO
                                       join Docente in db.MKT_DOCENTES
                                       on DocenteCurso.DOC_ID equals Docente.DOC_ID
                                       join lanzamiento in db.MAE_CURSOS_LANZAMIENTOS
                                       on DocenteCurso.LAN_ID equals lanzamiento.LAN_ID
                                       join curso in db.MAE_CURSOS
                                       on lanzamiento.CUR_ID equals curso.CUR_ID
                                       where DocenteCurso.MKT_ID == idusuario


                                       select new MktDocenteCursoCLS
                                       {
                                           DOC_ID = DocenteCurso.DOC_ID,
                                           LAN_ID = DocenteCurso.LAN_ID,
                                           MKT_ID = (int)DocenteCurso.MKT_ID,
                                           DOC_NOMBRES = Docente.DOC_NOMBRES,
                                           DOC_APELLIDOS = Docente.DOC_APELLIDOS,
                                           DOC_CELULAR = Docente.DOC_CELULAR,
                                           DOC_EMAIL = Docente.DOC_EMAIL,
                                           DCU_FEC = (DateTime)DocenteCurso.DCU_FEC,
                                           DCU_FECCADENA = ((DateTime)DocenteCurso.DCU_FEC).ToString(),
                                           CUR_NOMBRE = curso.CUR_NOMBRE
                                       }).ToList().Count();
                }
                else
                {

                    GetAll.Listado = (from DocenteCurso in db.MKT_DOCENTE_CURSO
                                    join Docente in db.MKT_DOCENTES
                                    on DocenteCurso.DOC_ID equals Docente.DOC_ID
                                    join lanzamiento in db.MAE_CURSOS_LANZAMIENTOS
                                    on DocenteCurso.LAN_ID equals lanzamiento.LAN_ID
                                    join curso in db.MAE_CURSOS
                                    on lanzamiento.CUR_ID equals curso.CUR_ID
                                    orderby DocenteCurso.DOC_ID


                                      select new MktDocenteCursoCLS
                                    {
                                        DOC_ID = DocenteCurso.DOC_ID,
                                        LAN_ID = DocenteCurso.LAN_ID,
                                        MKT_ID = (int)DocenteCurso.MKT_ID,
                                        DOC_NOMBRES = Docente.DOC_NOMBRES,
                                        DOC_APELLIDOS = Docente.DOC_APELLIDOS,
                                        //DOC_NOMBRES_APELLIDOS = Docente.DOC_NOMBRES + " " + Docente.DOC_APELLIDOS,
                                        DOC_CELULAR = Docente.DOC_CELULAR,
                                        DOC_EMAIL = Docente.DOC_EMAIL,
                                        DCU_FEC = (DateTime)DocenteCurso.DCU_FEC,
                                        DCU_FECCADENA = ((DateTime)DocenteCurso.DCU_FEC).ToString(),
                                        TurnoFecha = (DateTime)lanzamiento.LAN_FEC_CAPACITACION,
                                        TurnoFechaCadena = ((DateTime)lanzamiento.LAN_FEC_CAPACITACION).ToString(),
                                        CUR_NOMBRE = curso.CUR_NOMBRE
                                    }).Skip(((int)page - 1) * cantidadRegistrosPorPagina)
                     .Take(cantidadRegistrosPorPagina)
                     .ToList();

                    GetAll.RegPerPage = cantidadRegistrosPorPagina;
                    GetAll.Page = (int)page;
                    GetAll.TotalReg = (from DocenteCurso in db.MKT_DOCENTE_CURSO
                                          join Docente in db.MKT_DOCENTES
                                          on DocenteCurso.DOC_ID equals Docente.DOC_ID
                                          join lanzamiento in db.MAE_CURSOS_LANZAMIENTOS
                                          on DocenteCurso.LAN_ID equals lanzamiento.LAN_ID
                                          join curso in db.MAE_CURSOS
                                          on lanzamiento.CUR_ID equals curso.CUR_ID

                                             select new MktDocenteCursoCLS
                                             {
                                                DOC_ID = DocenteCurso.DOC_ID,
                                                LAN_ID = DocenteCurso.LAN_ID,
                                                MKT_ID = (int)DocenteCurso.MKT_ID,
                                                DOC_NOMBRES = Docente.DOC_NOMBRES,
                                                DOC_APELLIDOS = Docente.DOC_APELLIDOS,
                                                DOC_CELULAR = Docente.DOC_CELULAR,
                                                DOC_EMAIL = Docente.DOC_EMAIL,
                                                DCU_FEC = (DateTime)DocenteCurso.DCU_FEC,
                                                DCU_FECCADENA = ((DateTime)DocenteCurso.DCU_FEC).ToString(),
                                                CUR_NOMBRE = curso.CUR_NOMBRE
                                             }).ToList().Count();
                }

                ViewBag.ContarDocentesCapturados = GetAll.TotalReg;

                return View(GetAll);
                
            }
        }


        public ActionResult Index(MktDocenteCursoCLS oMktDocenteCursoCLS, int? page)
        {
            page = page == null ? 1 : page;
            int cantidadRegistrosPorPagina = 10;
            int idusuario = (int)HttpContext.Session["UsuID"];
            llenarCurso();
            //llenarUsuarioMkt();

            //ViewBag.CargaUsuMktAll = ListaUsuMkt;
            ViewBag.CargaCursos = ListaCursos;

            //int idusuario = oMktDocenteCursoCLS.USU_ID;
            int idCurso = oMktDocenteCursoCLS.CUR_ID;
            int idLan = oMktDocenteCursoCLS.LAN_ID;

            //cargar listas


            MktDocenteCursoCLS GetAll = new MktDocenteCursoCLS();

            using (var db = new DB_WebCIIPEntitiesERP())
            {
                if ( idCurso != 0 && idLan == 0)
                {

                    GetAll.Listado = (from DocenteCurso in db.MKT_DOCENTE_CURSO
                                      join Docente in db.MKT_DOCENTES
                                      on DocenteCurso.DOC_ID equals Docente.DOC_ID
                                      join lanzamiento in db.MAE_CURSOS_LANZAMIENTOS
                                      on DocenteCurso.LAN_ID equals lanzamiento.LAN_ID
                                      join curso in db.MAE_CURSOS
                                      on lanzamiento.CUR_ID equals curso.CUR_ID
                                      where DocenteCurso.MKT_ID == idusuario &&
                                            DocenteCurso.CUR_ID == idCurso
                                      orderby DocenteCurso.DCU_FEC descending

                                      select new MktDocenteCursoCLS
                                      {
                                          DOC_ID = DocenteCurso.DOC_ID,
                                          LAN_ID = DocenteCurso.LAN_ID,
                                          MKT_ID = (int)DocenteCurso.MKT_ID,
                                          DOC_NOMBRES = Docente.DOC_NOMBRES,
                                          DOC_APELLIDOS = Docente.DOC_APELLIDOS,
                                          //DOC_NOMBRES_APELLIDOS = Docente.DOC_NOMBRES + " " + Docente.DOC_APELLIDOS,
                                          TurnoFecha = (DateTime)lanzamiento.LAN_FEC_CAPACITACION,
                                          TurnoFechaCadena = ((DateTime)lanzamiento.LAN_FEC_CAPACITACION).ToString(),
                                          DOC_CELULAR = Docente.DOC_CELULAR,
                                          DOC_EMAIL = Docente.DOC_EMAIL,
                                          DCU_FEC = (DateTime)DocenteCurso.DCU_FEC,
                                          DCU_FECCADENA = ((DateTime)DocenteCurso.DCU_FEC).ToString(),
                                          CUR_NOMBRE = curso.CUR_DESCRIPCION

                                      }).Skip(((int)page - 1) * cantidadRegistrosPorPagina)
                     .Take(cantidadRegistrosPorPagina)
                     .ToList();

                    GetAll.RegPerPage = cantidadRegistrosPorPagina;
                    GetAll.Page = (int)page;
                    GetAll.TotalReg = (from DocenteCurso in db.MKT_DOCENTE_CURSO
                                       join Docente in db.MKT_DOCENTES
                                       on DocenteCurso.DOC_ID equals Docente.DOC_ID
                                       join lanzamiento in db.MAE_CURSOS_LANZAMIENTOS
                                       on DocenteCurso.LAN_ID equals lanzamiento.LAN_ID
                                       join curso in db.MAE_CURSOS
                                       on lanzamiento.CUR_ID equals curso.CUR_ID
                                       where DocenteCurso.MKT_ID == idusuario &&
                                             DocenteCurso.CUR_ID == idCurso 

                                       select new MktDocenteCursoCLS
                                       {
                                           DOC_ID = DocenteCurso.DOC_ID,
                                           LAN_ID = DocenteCurso.LAN_ID,
                                           MKT_ID = (int)DocenteCurso.MKT_ID,
                                           DOC_NOMBRES = Docente.DOC_NOMBRES,
                                           DOC_APELLIDOS = Docente.DOC_APELLIDOS,
                                           DOC_CELULAR = Docente.DOC_CELULAR,
                                           DOC_EMAIL = Docente.DOC_EMAIL,
                                           DCU_FEC = (DateTime)DocenteCurso.DCU_FEC,
                                           DCU_FECCADENA = ((DateTime)DocenteCurso.DCU_FEC).ToString(),
                                           CUR_NOMBRE = curso.CUR_DESCRIPCION

                                       }).ToList().Count();
                }
                
                else if (idCurso != 0 && idLan != 0) //cuando quier ver todo del bot sin importar el curso
                {

                    GetAll.Listado = (from DocenteCurso in db.MKT_DOCENTE_CURSO
                                      join Docente in db.MKT_DOCENTES
                                      on DocenteCurso.DOC_ID equals Docente.DOC_ID
                                      join lanzamiento in db.MAE_CURSOS_LANZAMIENTOS
                                      on DocenteCurso.LAN_ID equals lanzamiento.LAN_ID
                                      join curso in db.MAE_CURSOS
                                      on lanzamiento.CUR_ID equals curso.CUR_ID
                                      where DocenteCurso.MKT_ID == idusuario &&
                                            DocenteCurso.CUR_ID == idCurso &&
                                            lanzamiento.LAN_ID == idLan
                                      orderby DocenteCurso.DOC_ID


                                      select new MktDocenteCursoCLS
                                      {
                                          DOC_ID = DocenteCurso.DOC_ID,
                                          LAN_ID = DocenteCurso.LAN_ID,
                                          MKT_ID = (int)DocenteCurso.MKT_ID,
                                          DOC_NOMBRES = Docente.DOC_NOMBRES,
                                          DOC_APELLIDOS = Docente.DOC_APELLIDOS,
                                          //DOC_NOMBRES_APELLIDOS = Docente.DOC_NOMBRES + " " + Docente.DOC_APELLIDOS,
                                          DOC_CELULAR = Docente.DOC_CELULAR,
                                          DOC_EMAIL = Docente.DOC_EMAIL,
                                          DCU_FEC = (DateTime)DocenteCurso.DCU_FEC,
                                          DCU_FECCADENA = ((DateTime)DocenteCurso.DCU_FEC).ToString(),
                                          TurnoFecha = (DateTime)lanzamiento.LAN_FEC_CAPACITACION,
                                          TurnoFechaCadena = ((DateTime)lanzamiento.LAN_FEC_CAPACITACION).ToString(),
                                          CUR_NOMBRE = curso.CUR_DESCRIPCION
                                      }).Skip(((int)page - 1) * cantidadRegistrosPorPagina)
                     .Take(cantidadRegistrosPorPagina)
                     .ToList();

                    GetAll.RegPerPage = cantidadRegistrosPorPagina;
                    GetAll.Page = (int)page;
                    GetAll.TotalReg = (from DocenteCurso in db.MKT_DOCENTE_CURSO
                                       join Docente in db.MKT_DOCENTES
                                       on DocenteCurso.DOC_ID equals Docente.DOC_ID
                                       join lanzamiento in db.MAE_CURSOS_LANZAMIENTOS
                                       on DocenteCurso.LAN_ID equals lanzamiento.LAN_ID
                                       join curso in db.MAE_CURSOS
                                       on lanzamiento.CUR_ID equals curso.CUR_ID
                                       where DocenteCurso.MKT_ID == idusuario &&
                                            DocenteCurso.CUR_ID == idCurso &&
                                            lanzamiento.LAN_ID == idLan


                                       select new MktDocenteCursoCLS
                                       {
                                           DOC_ID = DocenteCurso.DOC_ID,
                                           LAN_ID = DocenteCurso.LAN_ID,
                                           MKT_ID = (int)DocenteCurso.MKT_ID,
                                           DOC_NOMBRES = Docente.DOC_NOMBRES,
                                           DOC_APELLIDOS = Docente.DOC_APELLIDOS,
                                           DOC_CELULAR = Docente.DOC_CELULAR,
                                           DOC_EMAIL = Docente.DOC_EMAIL,
                                           DCU_FEC = (DateTime)DocenteCurso.DCU_FEC,
                                           DCU_FECCADENA = ((DateTime)DocenteCurso.DCU_FEC).ToString(),
                                           CUR_NOMBRE = curso.CUR_DESCRIPCION
                                       }).ToList().Count();
                }
                else
                {

                    GetAll.Listado = (from DocenteCurso in db.MKT_DOCENTE_CURSO
                                      join Docente in db.MKT_DOCENTES
                                      on DocenteCurso.DOC_ID equals Docente.DOC_ID
                                      join lanzamiento in db.MAE_CURSOS_LANZAMIENTOS
                                      on DocenteCurso.LAN_ID equals lanzamiento.LAN_ID
                                      join curso in db.MAE_CURSOS
                                      on lanzamiento.CUR_ID equals curso.CUR_ID
                                      where DocenteCurso.MKT_ID == idusuario 
                                      orderby DocenteCurso.DCU_FEC descending


                                      select new MktDocenteCursoCLS
                                      {
                                          DOC_ID = DocenteCurso.DOC_ID,
                                          LAN_ID = DocenteCurso.LAN_ID,
                                          MKT_ID = (int)DocenteCurso.MKT_ID,
                                          DOC_NOMBRES = Docente.DOC_NOMBRES,
                                          DOC_APELLIDOS = Docente.DOC_APELLIDOS,
                                          //DOC_NOMBRES_APELLIDOS = Docente.DOC_NOMBRES + " " + Docente.DOC_APELLIDOS,
                                          DOC_CELULAR = Docente.DOC_CELULAR,
                                          DOC_EMAIL = Docente.DOC_EMAIL,
                                          DCU_FEC = (DateTime)DocenteCurso.DCU_FEC,
                                          DCU_FECCADENA = ((DateTime)DocenteCurso.DCU_FEC).ToString(),
                                          TurnoFecha = (DateTime)lanzamiento.LAN_FEC_CAPACITACION,
                                          TurnoFechaCadena = ((DateTime)lanzamiento.LAN_FEC_CAPACITACION).ToString(),
                                          CUR_NOMBRE = curso.CUR_DESCRIPCION
                                      }).Skip(((int)page - 1) * cantidadRegistrosPorPagina)
                     .Take(cantidadRegistrosPorPagina)
                     .ToList();

                    GetAll.RegPerPage = cantidadRegistrosPorPagina;
                    GetAll.Page = (int)page;
                    GetAll.TotalReg = (from DocenteCurso in db.MKT_DOCENTE_CURSO
                                       join Docente in db.MKT_DOCENTES
                                       on DocenteCurso.DOC_ID equals Docente.DOC_ID
                                       join lanzamiento in db.MAE_CURSOS_LANZAMIENTOS
                                       on DocenteCurso.LAN_ID equals lanzamiento.LAN_ID
                                       join curso in db.MAE_CURSOS
                                       on lanzamiento.CUR_ID equals curso.CUR_ID
                                       where DocenteCurso.MKT_ID == idusuario
                                       orderby DocenteCurso.DCU_FEC descending

                                       select new MktDocenteCursoCLS
                                       {
                                           DOC_ID = DocenteCurso.DOC_ID,
                                           LAN_ID = DocenteCurso.LAN_ID,
                                           MKT_ID = (int)DocenteCurso.MKT_ID,
                                           DOC_NOMBRES = Docente.DOC_NOMBRES,
                                           DOC_APELLIDOS = Docente.DOC_APELLIDOS,
                                           DOC_CELULAR = Docente.DOC_CELULAR,
                                           DOC_EMAIL = Docente.DOC_EMAIL,
                                           DCU_FEC = (DateTime)DocenteCurso.DCU_FEC,
                                           DCU_FECCADENA = ((DateTime)DocenteCurso.DCU_FEC).ToString(),
                                           CUR_NOMBRE = curso.CUR_DESCRIPCION
                                       }).ToList().Count();
                }

                ViewBag.ContarDocentesCapturados = GetAll.TotalReg;

                return View(GetAll);

            }
        }


        List<SelectListItem> ListaCursos;
        private void llenarCurso()
        {

            int idusuario = (int)HttpContext.Session["UsuID"];

            using (var db = new DB_WebCIIPEntitiesERP())
            {

                ListaCursos = (from CursoUsuarioLink in db.SEG_CURSOS_USUARIOS_LINKS
                               join cursos in db.MAE_CURSOS
                               on CursoUsuarioLink.CUR_ID equals cursos.CUR_ID
                               where    CursoUsuarioLink.USU_ID == idusuario &&
                                        CursoUsuarioLink.LNK_ACTIVO == "1"

                               select new SelectListItem
                               {
                                   Text = cursos.CUR_NOMBRE,
                                   Value = cursos.CUR_ID.ToString()

                               }).Distinct()
                               .ToList();
                ListaCursos.Insert(0, new SelectListItem { Text = "Todos", Value = "0" });
            }

        }

        List<SelectListItem> ListaCurso;
        [HttpGet]
        public JsonResult GetCursosBySesionUsu()
        {
            int idusuario = (int)HttpContext.Session["UsuID"];
            using (var db = new DB_WebCIIPEntitiesERP())
            {

                ListaCurso = (from CursoUsuarioLink in db.SEG_CURSOS_USUARIOS_LINKS
                               join cursos in db.MAE_CURSOS
                               on CursoUsuarioLink.CUR_ID equals cursos.CUR_ID
                               where CursoUsuarioLink.USU_ID == idusuario &&
                                        CursoUsuarioLink.LNK_ACTIVO == "1"

                               select new SelectListItem
                               {
                                   Text = cursos.CUR_DESCRIPCION ,
                                   Value = cursos.CUR_ID.ToString()

                               }).ToList();
            }
            return Json(ListaCurso, JsonRequestBehavior.AllowGet);
        }




        List<SelectListItem> ListaUsuMkt;
        private void llenarUsuarioMkt()
        {

            

            using (var db = new DB_WebCIIPEntitiesERP())
            {

                ListaUsuMkt = (from UsuarioMkt in db.SEG_USUARIOS
                               where UsuarioMkt.ROL_ID == 1  //solo aquellos que tengan el perfil de marketing

                               select new SelectListItem
                               {
                                   Text = UsuarioMkt.USU_NOMBRES+" "+ UsuarioMkt.USU_APELLIDO,
                                   Value = UsuarioMkt.USU_ID.ToString()

                               }).ToList();
                ListaUsuMkt.Insert(0, new SelectListItem { Text = "Todos", Value = "0" });
            }

        }

        private List<SelectListItem> GetLanzamientosList(int IdCurso)
        {
            using (var db = new DB_WebCIIPEntitiesERP())
            {
                var Lanzamientos = db.MAE_CURSOS_LANZAMIENTOS.Where(x => x.CUR_ID == IdCurso);
                var resp = Lanzamientos.Select(x => new SelectListItem()
                {
                    Value = x.LAN_ID.ToString(),
                    Text = x.LAN_ID.ToString() + " - " + x.LAN_FEC_CAPACITACION.ToString(),
                }).ToList();

                resp.Insert(0, new SelectListItem() { Value = "0", Text = "Todos" });

                return resp;
            }
        }

        [HttpGet]
        public JsonResult GetDocentes()
        {
            using (var db = new DB_WebCIIPEntitiesERP())
            {

                ListaUsuMkt = (from UsuarioMkt in db.SEG_USUARIOS
                               where UsuarioMkt.ROL_ID == 1  //solo aquellos que tengan el perfil de marketing

                               select new SelectListItem
                               {
                                   Text = UsuarioMkt.USU_NOMBRES + " " + UsuarioMkt.USU_APELLIDO,
                                   Value = UsuarioMkt.USU_ID.ToString()

                               }).ToList();
            }
            return Json(ListaUsuMkt, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetCursosByDoc(int id)
        {
            using (var db = new DB_WebCIIPEntitiesERP())
            {

                ListaCursos = (from CursoUsuarioLink in db.SEG_CURSOS_USUARIOS_LINKS
                               join cursos in db.MAE_CURSOS
                               on CursoUsuarioLink.CUR_ID equals cursos.CUR_ID
                               where CursoUsuarioLink.USU_ID == id &&
                                        CursoUsuarioLink.LNK_ACTIVO == "1"

                               select new SelectListItem
                               {
                                   Text = cursos.CUR_NOMBRE,
                                   Value = cursos.CUR_ID.ToString()

                               }).Distinct()
                               .ToList();
            }

            return Json(ListaCursos, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetLanzamientoByCurso(int id, int IdCurso)
        {
            List<SelectListItem> Lanzamientos;
            using (var db = new DB_WebCIIPEntitiesERP())
            {
                Lanzamientos = (from lanzamiento in db.MAE_CURSOS_LANZAMIENTOS
                                join MktDocenteCurso in db.MKT_DOCENTE_CURSO
                                on lanzamiento.LAN_ID equals MktDocenteCurso.LAN_ID
                                where MktDocenteCurso.MKT_ID == id &&
                                        lanzamiento.CUR_ID == IdCurso

                                select new SelectListItem
                                {
                                    Text = lanzamiento.LAN_ID.ToString() + " - " + lanzamiento.LAN_FEC_CAPACITACION,
                                    Value = lanzamiento.LAN_ID.ToString()

                                }).Distinct().ToList();
            }

            return Json(Lanzamientos, JsonRequestBehavior.AllowGet);
        }


        [HttpGet]
        public JsonResult GetLanzamientoSesionUsuByCurso(int IdCurso)
        {
            int idusuario = (int)HttpContext.Session["UsuID"];
            List<SelectListItem> Lanzamientos;
            using (var db = new DB_WebCIIPEntitiesERP())
            {
                Lanzamientos = (from lanzamiento in db.MAE_CURSOS_LANZAMIENTOS
                                join MktDocenteCurso in db.MKT_DOCENTE_CURSO
                                on lanzamiento.LAN_ID equals MktDocenteCurso.LAN_ID
                                where MktDocenteCurso.MKT_ID == idusuario &&
                                        MktDocenteCurso.CUR_ID == IdCurso

                                select new SelectListItem
                                {
                                    Text = lanzamiento.LAN_ID.ToString() + " - " + lanzamiento.LAN_FEC_CAPACITACION,
                                    Value = lanzamiento.LAN_ID.ToString()

                                }).Distinct().ToList();
            }

            return Json(Lanzamientos, JsonRequestBehavior.AllowGet);
        }


        List<SelectListItem> ListaLanzamientos;
        public JsonResult GetLanzamientos(int IdCurso)
        {
            ListaLanzamientos = GetLanzamientosList(IdCurso);
            return Json(ListaLanzamientos, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLanzamientosByUser(int IdCurso, int IdUser)
        {
            ListaLanzamientos = GetLanzamientosListByUser(IdCurso, IdUser);
            return Json(ListaLanzamientos, JsonRequestBehavior.AllowGet);

        }
        private List<SelectListItem> GetLanzamientosListByUser(int IdCurso, int IdUser)
        {
            List<SelectListItem> Lanzamientos;
            using (var db = new DB_WebCIIPEntitiesERP())
            {
                Lanzamientos = (from lanzamiento in db.MAE_CURSOS_LANZAMIENTOS
                                join MktDocenteCurso in db.MKT_DOCENTE_CURSO
                                on lanzamiento.LAN_ID equals MktDocenteCurso.LAN_ID
                                where MktDocenteCurso.MKT_ID == IdUser &&
                                        lanzamiento.CUR_ID == IdCurso

                                select new SelectListItem
                                {
                                    Text = lanzamiento.LAN_ID.ToString() + " - " + lanzamiento.LAN_FEC_CAPACITACION,
                                    Value = lanzamiento.LAN_ID.ToString()

                                }).Distinct().ToList();


                Lanzamientos.Insert(0, new SelectListItem() { Value = "0", Text = "Todos" });

                return Lanzamientos;
            }
        }

        private List<SelectListItem> GetCursoList(int Idusuario)
        {
            List<SelectListItem> Lista;
            using (var db = new DB_WebCIIPEntitiesERP())
            {

                Lista = (from cursos in db.MAE_CURSOS
                         join CursosUsuarioLink in db.SEG_CURSOS_USUARIOS_LINKS
                         on cursos.CUR_ID equals CursosUsuarioLink.CUR_ID
                         where CursosUsuarioLink.USU_ID == Idusuario 

                         select new SelectListItem
                         {
                             Text = cursos.CUR_ID.ToString() + " - " + cursos.CUR_NOMBRE,
                             Value = cursos.CUR_ID.ToString()

                         }).Distinct().ToList();


                Lista.Insert(0, new SelectListItem() { Value = "0", Text = "Todos" });

                return Lista;
            }
        }

        public JsonResult GetCursosByUser(int Idusuario)
        {
            ListaLanzamientos = GetCursoList(Idusuario);
            return Json(ListaLanzamientos, JsonRequestBehavior.AllowGet);

        }


    }
}