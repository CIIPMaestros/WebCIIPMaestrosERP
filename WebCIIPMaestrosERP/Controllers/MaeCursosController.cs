using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebCIIPMaestrosERP.Models;
using WebCIIPMaestrosERP.Seguridad;

namespace WebCIIPMaestrosERP.Controllers
{
    [Acceder]
    public class MaeCursosController : Controller
    {
        // GET: MaeCursos
        public ActionResult Index(MaeCursosCLS oMaeCursosCLS)
        {
            List<MaeCursosCLS> ListaCursos = null;

            LlenarCategorias();
            ViewBag.ListaCategorias = ListaCategorias;
            string nombreCurso = oMaeCursosCLS.CUR_NOMBRE;


            using (var db = new DB_WebCIIPEntities())
            {

                if (nombreCurso == null)
                {

                    ListaCursos = (from cursos in db.MAE_CURSOS
                                   join categorias in db.MAE_CATEGORIAS
                                   on cursos.CAT_ID equals categorias.CAT_ID
                                   join tablas in db.MAE_TABLAS
                                   on cursos.CUR_ACTIVO equals tablas.ID.ToString()
                                   where tablas.COD_TABLA == "ACT"
                                   orderby cursos.CUR_ID

                                   select new MaeCursosCLS

                                   {
                                       CUR_ID = cursos.CUR_ID,
                                       CUR_NOMBRE = cursos.CUR_NOMBRE,
                                       CUR_DESCRIPCION = cursos.CUR_DESCRIPCION,
                                       CUR_CERTIFICACION = cursos.CUR_CERTIFICACION,
                                       CUR_RESULTADOS = cursos.CUR_RESULTADOS,
                                       CAT_DESCRIPCION = categorias.CAT_NOMBRE,
                                       CUR_ACTIVO = cursos.CUR_ACTIVO,
                                       CUR_ESTADO = tablas.DESCRIPCION,
                                       CAT_ID = (int)cursos.CAT_ID
                                   }).ToList();

                }
                else if (nombreCurso != null)
                {

                    ListaCursos = (from cursos in db.MAE_CURSOS
                                   join categorias in db.MAE_CATEGORIAS
                                    on cursos.CAT_ID equals categorias.CAT_ID
                                   join tablas in db.MAE_TABLAS
                                   on cursos.CUR_ACTIVO equals tablas.ID.ToString()
                                   where cursos.CUR_NOMBRE.Contains(nombreCurso) &&
                                         tablas.COD_TABLA == "ACT"
                                   orderby cursos.CUR_ID

                                   select new MaeCursosCLS

                                   {
                                       CUR_ID = cursos.CUR_ID,
                                       CUR_NOMBRE = cursos.CUR_NOMBRE,
                                       CUR_DESCRIPCION = cursos.CUR_DESCRIPCION,
                                       CUR_CERTIFICACION = cursos.CUR_CERTIFICACION,
                                       CUR_RESULTADOS = cursos.CUR_RESULTADOS,
                                       CAT_DESCRIPCION = categorias.CAT_NOMBRE,
                                       CUR_ACTIVO = cursos.CUR_ACTIVO,
                                       CUR_ESTADO = tablas.DESCRIPCION,
                                       CAT_ID = (int)cursos.CAT_ID
                                   }).ToList();

                }



            }


            return View(ListaCursos);
        }


        List<SelectListItem> ListaCategorias;

        public void LlenarCategorias()
        {

            using (var db = new DB_WebCIIPEntities())
            {

                ListaCategorias = (from Categorias in db.MAE_CATEGORIAS
                                   select new SelectListItem
                                   {
                                       Text = Categorias.CAT_NOMBRE,
                                       Value = Categorias.CAT_ID.ToString(),
                                   }).ToList();

            }

        }


        public int ActivarDesactivar(int id, string estado)
        {
            int nregistrosAfectados = 0;
            try
            {
                using (var db = new DB_WebCIIPEntities())
                {
                    MAE_CURSOS oMAE_CURSOS = db.MAE_CURSOS.Where(p => p.CUR_ID == id).First();
                    oMAE_CURSOS.CUR_ACTIVO = estado;
                    nregistrosAfectados = db.SaveChanges();
                }

            }
            catch (Exception ex)
            {

                nregistrosAfectados = 0;
            }

            return nregistrosAfectados;
        }

        public ActionResult Filtro(string nombreCurso)
        {

            List<MaeCursosCLS> listaCursos = new List<MaeCursosCLS>();
            using (var db = new DB_WebCIIPEntities())
            {
                if (nombreCurso == null)
                    listaCursos = (from cursos in db.MAE_CURSOS
                                   join tablas in db.MAE_TABLAS
                                   on cursos.CUR_ACTIVO equals tablas.ID.ToString()
                                   where tablas.COD_TABLA == "ACT"
                                   orderby cursos.CUR_ID

                                     select new MaeCursosCLS
                                     {
                                         CUR_ID = cursos.CUR_ID,
                                         CUR_NOMBRE = cursos.CUR_NOMBRE,
                                         CUR_DESCRIPCION = cursos.CUR_DESCRIPCION,
                                         CUR_CERTIFICACION = cursos.CUR_CERTIFICACION,
                                         CUR_ESTADO = tablas.DESCRIPCION,
                                         CAT_ID = (int)cursos.CAT_ID
                                     }).ToList();
                else
                    listaCursos = (from cursos in db.MAE_CURSOS
                                   join tablas in db.MAE_TABLAS
                                   on cursos.CUR_ACTIVO equals tablas.ID.ToString()
                                   where cursos.CUR_NOMBRE.Contains(nombreCurso) && tablas.COD_TABLA == "ACT"
                                   orderby cursos.CUR_ID

                                   select new MaeCursosCLS
                                     {
                                         CUR_ID = cursos.CUR_ID,
                                         CUR_NOMBRE = cursos.CUR_NOMBRE,
                                         CUR_DESCRIPCION = cursos.CUR_DESCRIPCION,
                                         CUR_CERTIFICACION = cursos.CUR_CERTIFICACION,
                                         CUR_ESTADO = tablas.DESCRIPCION,
                                         CAT_ID = (int)cursos.CAT_ID
                                     }).ToList();
            }
            return PartialView("_Index", listaCursos);
        }

        public string GuardarNuevoControladorParcial(MaeCursosCLS oMaeCursosCLS, int accion)
        {

            string rpta="";

            if (!ModelState.IsValid && accion==-1)
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

                using (var db = new DB_WebCIIPEntities())
                {
                    if (accion.Equals(-1))
                    {
                        MAE_CURSOS oMaeCursos = new MAE_CURSOS();
                        oMaeCursos.CUR_NOMBRE = oMaeCursosCLS.CUR_NOMBRE;
                        oMaeCursos.CUR_DESCRIPCION = oMaeCursosCLS.CUR_DESCRIPCION;
                        oMaeCursos.CUR_RESULTADOS = oMaeCursosCLS.CUR_RESULTADOS;
                        oMaeCursos.CUR_CERTIFICACION = oMaeCursosCLS.CUR_CERTIFICACION;
                        oMaeCursos.CUR_PRECIO = oMaeCursosCLS.CUR_PRECIO;
                        oMaeCursos.CUR_ACTIVO = "1";
                        oMaeCursos.CAT_ID = oMaeCursosCLS.CAT_ID;
                        oMaeCursos.CUR_IMAGEN = oMaeCursosCLS.CUR_IMAGEN;
                        db.MAE_CURSOS.Add(oMaeCursos);
                        rpta = db.SaveChanges().ToString();


                    }
                    else//modificar
                    {

                        MAE_CURSOS oMaeCursos = db.MAE_CURSOS.Where(p => p.CUR_ID == accion).First();

                        oMaeCursos.CUR_NOMBRE = oMaeCursosCLS.CUR_NOMBRE;
                        oMaeCursos.CUR_DESCRIPCION = oMaeCursosCLS.CUR_DESCRIPCION;
                        oMaeCursos.CUR_RESULTADOS = oMaeCursosCLS.CUR_RESULTADOS;
                        oMaeCursos.CUR_CERTIFICACION = oMaeCursosCLS.CUR_CERTIFICACION;
                        oMaeCursos.CUR_PRECIO = oMaeCursosCLS.CUR_PRECIO;
                        oMaeCursos.CAT_ID = oMaeCursosCLS.CAT_ID;
                        //oSEG_USUARIOS.USU_CONTRASENA = cadenaContraCifrada;
                        //oSEG_USUARIOS.USU_ACTIVO = "1";

                        rpta = db.SaveChanges().ToString();

                    }
                }

                
            }
            return rpta;
        }

        public JsonResult recuperarDatos(int IdCurso)
        {

            MaeCursosCLS oMaeCursosCLS = new MaeCursosCLS();
            using (var db = new DB_WebCIIPEntities())
            {
                MAE_CURSOS oMAE_CURSOS = db.MAE_CURSOS.Where(p => p.CUR_ID == IdCurso).First();
                oMaeCursosCLS.CUR_NOMBRE = oMAE_CURSOS.CUR_NOMBRE;
                oMaeCursosCLS.CUR_DESCRIPCION = oMAE_CURSOS.CUR_DESCRIPCION;
                oMaeCursosCLS.CUR_CERTIFICACION = oMAE_CURSOS.CUR_CERTIFICACION;
                oMaeCursosCLS.CUR_RESULTADOS = oMAE_CURSOS.CUR_RESULTADOS;
                oMaeCursosCLS.CAT_ID = (int)oMAE_CURSOS.CAT_ID;
                oMaeCursosCLS.CUR_PRECIO = (decimal)oMAE_CURSOS.CUR_PRECIO;

            }
            return Json(oMaeCursosCLS, JsonRequestBehavior.AllowGet);


        }
    }
}