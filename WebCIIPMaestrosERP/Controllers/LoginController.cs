using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Mvc;
using WebCIIPMaestrosERP.Models;

namespace WebCIIPMaestrosERP.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult CerrarSession()
        {
            Session["Usuario"] = null;
            Session["RolID"] = null;
            Session["UsuID"] = null;
            Session["UsuNombre"] = null;
            return RedirectToAction("Index");
        }

        public string IniciarSesionUsuario(SegUsuariosCLS oSegUsuariosCLS)
        {

            string rpta = "";
            


            if (!ModelState.IsValid)
            { //Validar los datos ingresados esten completos
                rpta = "-10";
            }


            using (var dbFind = new DB_WebCIIPEntities()) //primera validacion - recuperamos el registro del docente de acuerod al email y contrasena ingresados
            {

                SHA256Managed sha = new SHA256Managed();
                byte[] byteContra = Encoding.Default.GetBytes(oSegUsuariosCLS.USU_CONTRASENA.ToString());
                byte[] byteContraCifrado = sha.ComputeHash(byteContra);
                string cadenaContraCifrada = BitConverter.ToString(byteContraCifrado).Replace("-", "");

                int numeroVeces = dbFind.SEG_USUARIOS.Where(p => p.USU_EMAIL == oSegUsuariosCLS.USU_EMAIL
                                                                         && p.USU_CONTRASENA == cadenaContraCifrada).Count();



                if (numeroVeces == 0)
                {
                    rpta = "-11";
                }
                else if (numeroVeces == 1)
                {
                    SEG_USUARIOS oSEG_USUARIOS = dbFind.SEG_USUARIOS.Where(p => p.USU_EMAIL == oSegUsuariosCLS.USU_EMAIL
                                                                         && p.USU_CONTRASENA == cadenaContraCifrada).First();
                    rpta = "1";
                    Session["Usuario"] = oSEG_USUARIOS;
                    Session["RolID"] = oSEG_USUARIOS.ROL_ID;
                    Session["UsuID"] = oSEG_USUARIOS.USU_ID;
                    Session["UsuNombre"] = oSEG_USUARIOS.USU_NOMBRES+" "+ oSEG_USUARIOS.USU_APELLIDO;
                }


            }
            return rpta;
        }


    }



}