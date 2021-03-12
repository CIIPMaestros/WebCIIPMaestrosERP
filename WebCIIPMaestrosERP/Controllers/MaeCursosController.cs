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
    public class MaeCursosController : Controller
    {

        List<SelectListItem> ListaHoras;
        List<SelectListItem> ListaDias;
        List<SelectListItem> ListaCategorias;
        // GET: MaeCursos
        public ActionResult Index(MaeCursosCLS oMaeCursosCLS, int? page)
        {
            MaeCursosCLS GetCursos = new MaeCursosCLS();


            LlenarCategorias();
            LlenarDias();
            LlenarHoras();
            

            ViewBag.ListaCategorias = ListaCategorias;
            ViewBag.ListaDias = ListaDias;
            ViewBag.ListaHoras = ListaHoras;
            


            string nombreCurso = oMaeCursosCLS.CUR_NOMBRE;
            page = page == null ? 1 : page;
            int cantidadRegistrosPorPagina = 10;

            using (var db = new DB_WebCIIPEntitiesERP())
            {
                if (nombreCurso == null)
                {

                    GetCursos.ListadoCursos = (from cursos in db.MAE_CURSOS
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
                                   }).Skip(((int)page - 1) * cantidadRegistrosPorPagina)
                                   .Take(cantidadRegistrosPorPagina)
                                   .ToList();

                    GetCursos.RegPerPage = cantidadRegistrosPorPagina;
                    GetCursos.Page = (int)page;
                    GetCursos.TotalReg = (from cursos in db.MAE_CURSOS
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
                                          }).ToList().Count();

                }
                else if (nombreCurso != null)
                {

                    GetCursos.ListadoCursos = (from cursos in db.MAE_CURSOS
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
                                   }).Skip(((int)page - 1) * cantidadRegistrosPorPagina)
                                   .Take(cantidadRegistrosPorPagina)
                                   .ToList();

                    GetCursos.RegPerPage = cantidadRegistrosPorPagina;
                    GetCursos.Page = (int)page;
                    GetCursos.TotalReg = (from cursos in db.MAE_CURSOS
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
                                          }).ToList().Count();

                }



            }


            return View(GetCursos);
        }


        public ActionResult Registrar()
        {
            return View();
        }

        

        public void LlenarHoras()
        {

            using (var db = new DB_WebCIIPEntitiesERP())
            {

                ListaHoras = (from Dias in db.MAE_TABLAS
                             where Dias.COD_TABLA == "HORA"
                             select new SelectListItem
                             {
                                 Text = Dias.DESCRIPCION,
                                 Value = Dias.CODIGO,
                             }).ToList();

            }

        }

        public void LlenarDias()
        {

            using (var db = new DB_WebCIIPEntitiesERP())
            {

                ListaDias = (from Dias in db.MAE_TABLAS
                             where Dias.COD_TABLA == "DIAS"
                             select new SelectListItem
                             {
                                 Text = Dias.DESCRIPCION,
                                 Value = Dias.CODIGO,
                             }).ToList();

            }

        }

        public void LlenarCategorias()
        {

            using (var db = new DB_WebCIIPEntitiesERP())
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
                using (var db = new DB_WebCIIPEntitiesERP())
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

        public ActionResult Filtro(string nombreCurso, int? page)
        {
            MaeCursosCLS GetCursos = new MaeCursosCLS();
            page = page == null ? 1 : page;
            int cantidadRegistrosPorPagina = 10;
            using (var db = new DB_WebCIIPEntitiesERP())
            {
                if (nombreCurso == null)
                {
                    GetCursos.ListadoCursos = (from cursos in db.MAE_CURSOS
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
                                                   CAT_DESCRIPCION = categorias.CAT_NOMBRE,
                                                   CUR_ESTADO = tablas.DESCRIPCION,
                                                   CAT_ID = (int)cursos.CAT_ID
                                               }).Skip(((int)page-1) * cantidadRegistrosPorPagina)
                                               .Take(cantidadRegistrosPorPagina)
                                               .ToList();

                    GetCursos.RegPerPage = cantidadRegistrosPorPagina;
                    GetCursos.Page = (int)page;
                    GetCursos.TotalReg = (from cursos in db.MAE_CURSOS
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
                                          }).ToList().Count();
                }
                else {
                    GetCursos.ListadoCursos = (from cursos in db.MAE_CURSOS
                                               join categorias in db.MAE_CATEGORIAS
                                               on cursos.CAT_ID equals categorias.CAT_ID
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
                                       CAT_DESCRIPCION = categorias.CAT_NOMBRE,
                                       CAT_ID = (int)cursos.CAT_ID
                                   }).Skip(((int)page - 1) * cantidadRegistrosPorPagina)
                                   .Take(cantidadRegistrosPorPagina)
                                   .ToList();

                    GetCursos.RegPerPage = cantidadRegistrosPorPagina;
                    GetCursos.Page = (int)page;
                    GetCursos.TotalReg = (from cursos in db.MAE_CURSOS
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
                                          }).ToList().Count();
                }
            }
            return PartialView("_Index", GetCursos);
        }

        public string GuardarNuevoControladorParcial(MaeCursosCLS oMaeCursosCLS, int ? accion)
        {

            int idCurso = 0;
            string rpta="";
            
            string cadenaContraCifrada = "";

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

                using (var db = new DB_WebCIIPEntitiesERP())
                {
                    if (oMaeCursosCLS.CUR_ID == 0)
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

                        //debemos recuperar el numero del id para encriptarlo
                        idCurso = oMaeCursos.CUR_ID; // recuperar

                        //iniciamos la encriptacion

                        using (var transacction = new TransactionScope())
                        {

                            SHA256Managed sha = new SHA256Managed();
                            byte[] byteContra = Encoding.Default.GetBytes(idCurso.ToString());
                            byte[] byteContraCifrado = sha.ComputeHash(byteContra);
                            cadenaContraCifrada = "";
                            cadenaContraCifrada = BitConverter.ToString(byteContraCifrado).Replace("-", "");


                            MAE_CURSOS ooMAE_CURSOS = db.MAE_CURSOS.Where(p => p.CUR_ID == idCurso).First();
                            ooMAE_CURSOS.CUR_ID_ENCRIPTADO = cadenaContraCifrada;
                            db.SaveChanges().ToString();
                            transacction.Complete();

                        }

                        // recorremos la lsita de horarios

                        foreach (var Horario in oMaeCursosCLS.horarios) {

                            MAE_CURSOS_HORARIOS oMAE_CURSOS_HORARIOS = new MAE_CURSOS_HORARIOS();
                            oMAE_CURSOS_HORARIOS.CUR_ID = oMaeCursos.CUR_ID;
                            oMAE_CURSOS_HORARIOS.SCH_DIA = Horario.SCH_DIA;
                            oMAE_CURSOS_HORARIOS.SCH_HORA = Horario.SCH_HORA;
                            db.MAE_CURSOS_HORARIOS.Add(oMAE_CURSOS_HORARIOS);

                        }

                        db.SaveChanges();


                    }
                    else//modificar
                    {

                        MAE_CURSOS oMaeCursos = db.MAE_CURSOS.Where(p => p.CUR_ID == oMaeCursosCLS.CUR_ID).First();

                        oMaeCursos.CUR_NOMBRE = oMaeCursosCLS.CUR_NOMBRE;
                        oMaeCursos.CUR_DESCRIPCION = oMaeCursosCLS.CUR_DESCRIPCION;
                        oMaeCursos.CUR_RESULTADOS = oMaeCursosCLS.CUR_RESULTADOS;
                        oMaeCursos.CUR_CERTIFICACION = oMaeCursosCLS.CUR_CERTIFICACION;
                        oMaeCursos.CUR_PRECIO = oMaeCursosCLS.CUR_PRECIO;
                        oMaeCursos.CAT_ID = oMaeCursosCLS.CAT_ID;
                        //oSEG_USUARIOS.USU_CONTRASENA = cadenaContraCifrada;
                        //oSEG_USUARIOS.USU_ACTIVO = "1";

                        using (var transacction = new TransactionScope())
                        {

                            if (oMaeCursosCLS.CUR_ID_ENCRIPTADO =="" || oMaeCursosCLS.CUR_ID_ENCRIPTADO is null)
                            { 

                            SHA256Managed sha = new SHA256Managed();
                            byte[] byteContra = Encoding.Default.GetBytes(oMaeCursosCLS.CUR_ID.ToString());
                            byte[] byteContraCifrado = sha.ComputeHash(byteContra);
                            cadenaContraCifrada = "";
                            cadenaContraCifrada = BitConverter.ToString(byteContraCifrado).Replace("-", "");


                            MAE_CURSOS ooMAE_CURSOS = db.MAE_CURSOS.Where(p => p.CUR_ID == oMaeCursosCLS.CUR_ID).First();
                            ooMAE_CURSOS.CUR_ID_ENCRIPTADO = cadenaContraCifrada;
                            transacction.Complete();

                            }

                        }

                        rpta = db.SaveChanges().ToString();
                        // recorremos la lsita de horarios
                        // solo cuando es nuevo insertamos cuando es editar no mandamos nada por q se cae // corregir luego

                        if (oMaeCursosCLS.horarios != null) { 

                            foreach (var Horario in oMaeCursosCLS.horarios)
                            {

                                    MAE_CURSOS_HORARIOS oMAE_CURSOS_HORARIOS = new MAE_CURSOS_HORARIOS();
                                    oMAE_CURSOS_HORARIOS.CUR_ID = oMaeCursos.CUR_ID;
                                    oMAE_CURSOS_HORARIOS.SCH_DIA = Horario.SCH_DIA;
                                    oMAE_CURSOS_HORARIOS.SCH_HORA = Horario.SCH_HORA;
                                    db.MAE_CURSOS_HORARIOS.Add(oMAE_CURSOS_HORARIOS);

                            }

                            rpta = db.SaveChanges().ToString();

                        }

                    }
                }

                
            }
            return rpta;
        }

        public JsonResult SP_Execute()
        {
            using (var db = new DB_WebCIIPEntitiesERP())
            {
                db.Sp_Crear_Lanzamientos_Masivo(0,0);
                db.SaveChanges();
            }
                return Json(new { res = true }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult recuperarDatos(int IdCurso)
        {
            var db = new DB_WebCIIPEntitiesERP();

            db.Configuration.ProxyCreationEnabled = false;
            

            MaeCursosCLS oMaeCursosCLS = new MaeCursosCLS();
            MAE_CURSOS oMAE_CURSOS = db.MAE_CURSOS.Where(p => p.CUR_ID == IdCurso).First();
            oMaeCursosCLS.CUR_NOMBRE = oMAE_CURSOS.CUR_NOMBRE;
            oMaeCursosCLS.CUR_DESCRIPCION = oMAE_CURSOS.CUR_DESCRIPCION;
            oMaeCursosCLS.CUR_CERTIFICACION = oMAE_CURSOS.CUR_CERTIFICACION;
            oMaeCursosCLS.CUR_RESULTADOS = oMAE_CURSOS.CUR_RESULTADOS;
            oMaeCursosCLS.CAT_ID = (int)oMAE_CURSOS.CAT_ID;
            oMaeCursosCLS.CUR_PRECIO = (decimal)oMAE_CURSOS.CUR_PRECIO;

            var listadoHorarios = db.MAE_CURSOS_HORARIOS.Where(x => x.CUR_ID == IdCurso).ToList();



            listadoHorarios.ForEach(x => x.MAE_CURSOS = null);

            oMaeCursosCLS.GetHorarios = listadoHorarios;
            return Json(oMaeCursosCLS, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult SP_Lanzamiento(int idCurso)
        {
            using (var db = new DB_WebCIIPEntitiesERP())
            {
                db.Sp_Crear_Lanzamientos_By_Curso(idCurso);
                db.SaveChanges();
            }
            return Json(new { res = true }, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult EliminarHorario(int id)
        {
            using (var db = new DB_WebCIIPEntitiesERP())
            {
                MAE_CURSOS_HORARIOS getHoratio = db.MAE_CURSOS_HORARIOS.Where(x => x.SCH_ID == id).Single();
                db.MAE_CURSOS_HORARIOS.Remove(getHoratio);
                db.SaveChanges();
            }

            return Json(new { Exito = true }, JsonRequestBehavior.AllowGet);
        }
    }
}