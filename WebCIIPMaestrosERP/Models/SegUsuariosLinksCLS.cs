using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebCIIPMaestrosERP.Models
{
    public class SegUsuariosLinksCLS
    {

        public int LNK_ID { get; set; }
        public int CUS_ID { get; set; }

        [Required(ErrorMessage = "Por favor ingrese usuario")]
        [Display(Name = "Usuario")]
        public int USU_ID { get; set; }


        [Required(ErrorMessage = "Por favor ingrese curso")]
        [Display(Name = "Curso")]
        public int CUR_ID { get; set; }

        [Display(Name = "Link")]
        //[Required(ErrorMessage = "ingresar link")]
        [DataType(DataType.MultilineText)]
        public string USU_LINK { get; set; }

        [Display(Name = "Lanzamientos")]
        //[Required(ErrorMessage = "Seleccione lanzamiento")]
        public int LAN_ID { get; set; }

        [Display(Name = "Fecha de Lanzamiento")]
        public DateTime LAN_FEC_CAPACITACION { get; set; }
        public string CUR_NOMBRE { get; set; }

        public string CUR_DESCRIPCION { get; set; }

        [Display(Name = "Estado")]
        public string LNK_ACTIVO { get; set; }

        public string USU_NOMBRES { get; set; }

        public string TUT_NOMBRES { get; set; }


        public int IdLanzamiento { get; set; }

        public int IdLanzamientoOriginal { get; set; }

        public int IdCursoOriginal { get; set; }

        public int IdUsuarioOriginal { get; set; }

        [Display(Name = "Link Corto")]
        public string USU_LINK_CORTO { get; set; }


    }
}