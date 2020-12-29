using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Transactions;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebCIIPMaestrosERP.Models;
using WebCIIPMaestrosERP.Seguridad;

namespace WebCIIPMaestrosERP.Controllers
{
    [Acceder]
    public class SegUsuariosController : Controller
    {
        // GET: SegUsuarios
        public ActionResult Index(SegUsuariosCLS osegUsuariosCLS)
        {


            llenarRol();


            string nombreUsuario = osegUsuariosCLS.USU_NOMBRES;
            List<SegUsuariosCLS> ListaUsuarios = null;

            using (var db = new DB_WebCIIPEntities())
            {

                if (osegUsuariosCLS.USU_NOMBRES == null)
                {
                    ListaUsuarios = (from usuarios in db.SEG_USUARIOS
                                     join rol in db.MAE_ROLES
                                     on usuarios.ROL_ID equals rol.ROL_ID
                                     join tablas in db.MAE_TABLAS
                                     on usuarios.USU_ACTIVO equals tablas.ID.ToString()
                                     where tablas.COD_TABLA == "ACT"
                                     orderby usuarios.USU_ID

                                     select new SegUsuariosCLS

                                     {

                                         USU_ID = usuarios.USU_ID,
                                         USU_NOMBRES = usuarios.USU_NOMBRES,
                                         USU_APELLIDO = usuarios.USU_APELLIDO,
                                         USU_EMAIL = usuarios.USU_EMAIL,
                                         USU_DIRECCION = usuarios.USU_DIRECCION,
                                         USU_CELULAR = usuarios.USU_CELULAR,
                                         ROL_ID = (int)usuarios.ROL_ID,
                                         ROL_DESCRIPCION = rol.ROL_DESCRIPCION,
                                         USU_ACTIVO = tablas.DESCRIPCION,
                                         USU_ID_ENCRIPTADO = usuarios.USU_ID_ENCRIPTADO

                                     }).ToList();
                }
                else
                {
                    ListaUsuarios = (from usuarios in db.SEG_USUARIOS
                                     join rol in db.MAE_ROLES
                                     on usuarios.ROL_ID equals rol.ROL_ID
                                     join tablas in db.MAE_TABLAS
                                     on usuarios.USU_ACTIVO equals tablas.ID.ToString()
                                     where usuarios.USU_NOMBRES.Contains(nombreUsuario) &&
                                           tablas.COD_TABLA == "ACT"
                                     orderby usuarios.USU_ID

                                     select new SegUsuariosCLS

                                     {

                                         USU_ID = usuarios.USU_ID,
                                         USU_NOMBRES = usuarios.USU_NOMBRES,
                                         USU_APELLIDO = usuarios.USU_APELLIDO,
                                         USU_EMAIL = usuarios.USU_EMAIL,
                                         USU_DIRECCION = usuarios.USU_DIRECCION,
                                         USU_CELULAR = usuarios.USU_CELULAR,
                                         ROL_ID = (int)usuarios.ROL_ID,
                                         ROL_DESCRIPCION = rol.ROL_DESCRIPCION,
                                         USU_ACTIVO = tablas.DESCRIPCION,
                                         USU_ID_ENCRIPTADO = usuarios.USU_ID_ENCRIPTADO

                                     }).ToList();


                }
            }

            return View(ListaUsuarios);
        }

        public int ActivarDesactivar(int id, string estado)
        {
            int nregistrosAfectados = 0;
            try
            {
                using (var db = new DB_WebCIIPEntities())
                {
                    SEG_USUARIOS oSEG_USUARIOS = db.SEG_USUARIOS.Where(p => p.USU_ID == id).First();
                    oSEG_USUARIOS.USU_ACTIVO = estado;
                    nregistrosAfectados = db.SaveChanges();
                }

            }
            catch (Exception ex)
            {

                nregistrosAfectados = 0;
            }

            return nregistrosAfectados;
        }

        public ActionResult Filtro(string nombreUsuario)
        {

            List<SegUsuariosCLS> listaUsuarios = new List<SegUsuariosCLS>();
            using (var db = new DB_WebCIIPEntities())
            {
                if (nombreUsuario == null)
                    listaUsuarios = (from usuarios in db.SEG_USUARIOS
                                     join rol in db.MAE_ROLES
                                     on usuarios.ROL_ID equals rol.ROL_ID
                                     join tablas in db.MAE_TABLAS
                                     on usuarios.USU_ACTIVO equals tablas.ID.ToString()
                                     where tablas.COD_TABLA == "ACT"
                                     orderby usuarios.USU_ID

                                     select new SegUsuariosCLS

                                     {

                                         USU_ID = usuarios.USU_ID,
                                         USU_NOMBRES = usuarios.USU_NOMBRES,
                                         USU_APELLIDO = usuarios.USU_APELLIDO,
                                         USU_EMAIL = usuarios.USU_EMAIL,
                                         USU_DIRECCION = usuarios.USU_DIRECCION,
                                         USU_CELULAR = usuarios.USU_CELULAR,
                                         ROL_ID = (int)usuarios.ROL_ID,
                                         ROL_DESCRIPCION = rol.ROL_DESCRIPCION,
                                         USU_ACTIVO = tablas.DESCRIPCION,
                                         USU_ID_ENCRIPTADO = usuarios.USU_ID_ENCRIPTADO
                                     }).ToList();
                else
                    listaUsuarios = (from usuarios in db.SEG_USUARIOS
                                     join rol in db.MAE_ROLES
                                     on usuarios.ROL_ID equals rol.ROL_ID
                                     join tablas in db.MAE_TABLAS
                                     on usuarios.USU_ACTIVO equals tablas.ID.ToString()
                                     where (usuarios.USU_NOMBRES.Contains(nombreUsuario) ||
                                            usuarios.USU_APELLIDO .Contains(nombreUsuario)) &&
                                           tablas.COD_TABLA == "ACT"
                                     orderby usuarios.USU_ID


                                     select new SegUsuariosCLS

                                     {

                                         USU_ID = usuarios.USU_ID,
                                         USU_NOMBRES = usuarios.USU_NOMBRES,
                                         USU_APELLIDO = usuarios.USU_APELLIDO,
                                         USU_EMAIL = usuarios.USU_EMAIL,
                                         USU_DIRECCION = usuarios.USU_DIRECCION,
                                         USU_CELULAR = usuarios.USU_CELULAR,
                                         ROL_ID = (int)usuarios.ROL_ID,
                                         ROL_DESCRIPCION = rol.ROL_DESCRIPCION,
                                         USU_ACTIVO = tablas.DESCRIPCION,
                                         USU_ID_ENCRIPTADO = usuarios.USU_ID_ENCRIPTADO
                                     }).ToList();
            }
            return PartialView("_Index", listaUsuarios);
        }

        public JsonResult recuperarDatos(int IdUsuario) {

            SegUsuariosCLS oSegUsuariosCLS = new SegUsuariosCLS();
            using (var db = new DB_WebCIIPEntities())
            {
                SEG_USUARIOS oSEG_USUARIOS = db.SEG_USUARIOS.Where(p => p.USU_ID == IdUsuario).First();
                oSegUsuariosCLS.USU_NOMBRES = oSEG_USUARIOS.USU_NOMBRES;
                oSegUsuariosCLS.USU_APELLIDO = oSEG_USUARIOS.USU_APELLIDO;
                oSegUsuariosCLS.USU_CELULAR = oSEG_USUARIOS.USU_CELULAR;
                oSegUsuariosCLS.USU_EMAIL = oSEG_USUARIOS.USU_EMAIL;
                oSegUsuariosCLS.ROL_ID = oSEG_USUARIOS.ROL_ID;
                oSegUsuariosCLS.USU_CONTRASENA = oSEG_USUARIOS.USU_CONTRASENA;
                


            }
            return Json(oSegUsuariosCLS, JsonRequestBehavior.AllowGet);

            
        }

      

        private void llenarRol()
        {
            List<SelectListItem> Lista;
            using (var db = new DB_WebCIIPEntities())
            {

                Lista = (from roles in db.MAE_ROLES
                               select new SelectListItem
                               {
                                   Text = roles.ROL_DESCRIPCION,
                                   Value = roles.ROL_ID.ToString()

                               }).ToList();
                ViewBag.listaRol = Lista;
            }

        }

        public string GuardarNuevoControladorParcial(SegUsuariosCLS oSegUsuariosCLS, int accion)
        {

            decimal idUsuario = 0;
            string rpta = "";

            //valida que los campos sean poblados
            if (!ModelState.IsValid)
            {
                var query = (from state in ModelState.Values
                             from error in state.Errors
                             select error.ErrorMessage).ToList();
                rpta += "<ul class='list-group'>";
                foreach (var item in query)
                {
                    rpta += "<li class='list-group-item'>" + item + "</li>";
                }
                rpta += "</ul>";

            }
            else {

              

                    using (var db = new DB_WebCIIPEntities())

                    {

                    if (accion == -1) // cuando es un nuevo registro
                    {
                        //inicia cifrado de contrasena
                        SEG_USUARIOS oSEG_USUARIOS = new SEG_USUARIOS();
                        string cadenaContraCifrada = "";

                        using (var transacction = new TransactionScope())
                        {

                            SHA256Managed sha = new SHA256Managed();
                            byte[] byteContra = Encoding.Default.GetBytes(oSegUsuariosCLS.USU_CONTRASENA.ToString());
                            byte[] byteContraCifrado = sha.ComputeHash(byteContra);
                            cadenaContraCifrada = BitConverter.ToString(byteContraCifrado).Replace("-", "");


                        }
                        //termina cifrado de contrasena



                        oSEG_USUARIOS.USU_NOMBRES = oSegUsuariosCLS.USU_NOMBRES;
                        oSEG_USUARIOS.USU_APELLIDO = oSegUsuariosCLS.USU_APELLIDO;
                        oSEG_USUARIOS.USU_EMAIL = oSegUsuariosCLS.USU_EMAIL;
                        oSEG_USUARIOS.USU_DIRECCION = oSegUsuariosCLS.USU_DIRECCION;
                        oSEG_USUARIOS.USU_CELULAR = oSegUsuariosCLS.USU_CELULAR;
                        oSEG_USUARIOS.ROL_ID = oSegUsuariosCLS.ROL_ID;
                        oSEG_USUARIOS.USU_CONTRASENA = cadenaContraCifrada;
                        oSEG_USUARIOS.USU_ACTIVO = "1";
                        //oSEG_USUARIOS.USU_ID_ENCRIPTADO = oSegUsuariosCLS.

                        db.SEG_USUARIOS.Add(oSEG_USUARIOS);
                        rpta = db.SaveChanges().ToString();

                        //debemos recuperar el numero del id para encriptarlo
                        idUsuario = oSEG_USUARIOS.USU_ID; // recuperar

                        //iniciamos la encriptacion

                        using (var transacction = new TransactionScope())
                        {

                            SHA256Managed sha = new SHA256Managed();
                            byte[] byteContra = Encoding.Default.GetBytes(idUsuario.ToString());
                            byte[] byteContraCifrado = sha.ComputeHash(byteContra);
                            cadenaContraCifrada = "";
                            cadenaContraCifrada = BitConverter.ToString(byteContraCifrado).Replace("-", "");


                            SEG_USUARIOS ooSEG_USUARIOS = db.SEG_USUARIOS.Where(p => p.USU_ID == idUsuario).First();
                            ooSEG_USUARIOS.USU_ID_ENCRIPTADO = cadenaContraCifrada;
                            db.SaveChanges().ToString();
                            transacction.Complete();

                        }

                    }
                    else
                    {

                        SEG_USUARIOS oSEG_USUARIOS = db.SEG_USUARIOS.Where(p => p.USU_ID == accion).First();

                        oSEG_USUARIOS.USU_NOMBRES = oSegUsuariosCLS.USU_NOMBRES;
                        oSEG_USUARIOS.USU_APELLIDO = oSegUsuariosCLS.USU_APELLIDO;
                        oSEG_USUARIOS.USU_EMAIL = oSegUsuariosCLS.USU_EMAIL;
                        oSEG_USUARIOS.USU_DIRECCION = oSegUsuariosCLS.USU_DIRECCION;
                        oSEG_USUARIOS.USU_CELULAR = oSegUsuariosCLS.USU_CELULAR;
                        oSEG_USUARIOS.ROL_ID = oSegUsuariosCLS.ROL_ID;
                        //oSEG_USUARIOS.USU_CONTRASENA = cadenaContraCifrada;
                        //oSEG_USUARIOS.USU_ACTIVO = "1";

                        rpta = db.SaveChanges().ToString();

                        

                    }

                }

                

            }
            return rpta;

        }

    }
}