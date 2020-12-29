using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebCIIPMaestrosERP.Models
{
    public class SegUsuariosCLS
    {
        [Display(Name = "ID Usuario")]
        public int USU_ID { get; set; }

        [Display(Name = "Nombres")]
        [Required]
        public string USU_NOMBRES { get; set; }

        [Display(Name = "Apellidos")]
        [Required]
        public string USU_APELLIDO { get; set; }

        [Display(Name = "Email")]
        [EmailAddress(ErrorMessage = "Ingrese un email valido")]
        [Required]
        public string USU_EMAIL { get; set; }

        [Display(Name = "Direccion")]
        
        public string USU_DIRECCION { get; set; }

        [Display(Name = "Celular")]
        [Required]
        public string USU_CELULAR { get; set; }

        [Display(Name = "Rol")]
        //[Required]
        public int ROL_ID { get; set; }

        [Display(Name = "Rol")]
        //[Required]
        public string ROL_DESCRIPCION { get; set; }

        [Display(Name = "Estado")]
        
        public string USU_ACTIVO { get; set; }

        
        [Display(Name = "Codigo Encriptado")]
        
        public string USU_ID_ENCRIPTADO { get; set; }

        [Display(Name = "Contraseña")]
        [Required]
        public string USU_CONTRASENA { get; set; }

    }
}