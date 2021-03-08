using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebCIIPMaestrosERP.Models
{
    public class SegCursoUsuarioLinksCLS
    {


        [Display(Name = "ID")]
        public int CUS_ID { get; set; }


        [Required(ErrorMessage = "Por favor ingrese usuario")]
        [Display(Name = "Usuario")]
        public int USU_ID { get; set; }

        [Display(Name = "Usuario")]
        public string USU_NOMBRES { get; set; }

        [Required(ErrorMessage = "Por favor ingrese curso")]
        [Display(Name = "Curso")]
        public int CUR_ID { get; set; }

        [Display(Name = "Curso")]
        public string CUR_NOMBRE { get; set; }

        [Display(Name = "Link")]
        //[Required(ErrorMessage = "ingresar link")]
        [DataType(DataType.MultilineText)]
        public string USU_LINK { get; set; }

      


        [Display(Name = "Estado")]
        public string LNK_ACTIVO { get; set; }


        [Display(Name = "Link Corto")]
        public string USU_LINK_CORTO { get; set; }


    }
}