using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebCIIPMaestrosERP.Models
{
    public class MaeCursoLanzamientoCLS
    {
        [Required]
        [Display(Name = "Id")]
        public int LAN_ID { get; set; }

        [Required]
        [Display(Name = "Curso")]
        public int CUR_ID { get; set; }

        [Required]
        [Display(Name = "Curso")]
        public string CUR_NOMBRE { get; set; }


        [Display(Name = "Ponente")]
        [Required]
        public int TUT_ID { get; set; }

        [Display(Name = "Ponente")]
        [Required]
        public string TUT_NOMBRES { get; set; }

        [Display(Name = "Estado")]
        public string LAN_ESTADO { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]


        [Display(Name = "Fecha de capacitacion")]
        public DateTime LAN_FEC_CAPACITACION { get; set; }
        public string LAN_FEC_CAPACITACIONCADENA { get; set; }


        public string LAN_ID_ENCRIPTADO { get; set; }


        
        [Display(Name = "Tipo de Lanzamiento")]
        public string IND_TIPO_LAN { get; set; }
    }
}