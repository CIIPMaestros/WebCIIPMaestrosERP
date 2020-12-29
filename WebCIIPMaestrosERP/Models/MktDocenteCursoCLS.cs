using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebCIIPMaestrosERP.Models
{
    public class MktDocenteCursoCLS
    {


        [Display(Name = "ID Docente")]
        public int DOC_ID { get; set; }

        [Display(Name = "ID Lanzamiento")]
        public int LAN_ID { get; set; }


        [Display(Name = "ID Usuario")]
        public int MKT_ID { get; set; }


        [Display(Name = "Nombres")]
        public string DOC_NOMBRES { get; set; }

        [Display(Name = "Apellidos")]
        public string DOC_APELLIDOS { get; set; }

        [Display(Name = "Celular")]
        public string DOC_CELULAR { get; set; }

        [Display(Name = "Email")]
        public string DOC_EMAIL { get; set; }

        public int CUR_ID { get; set; }

        public int USU_ID { get; set; }

        public string IdCurso { get; set; }

        public string IdLanzamiento { get; set; }



    }
}