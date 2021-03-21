using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebCIIPMaestrosERP.Models
{
    public class MktDocenteCursoCLS : BaseModel
    {


        [Display(Name = "ID")]
        public int DOC_ID { get; set; }

        [Display(Name = "ID Lanzamiento")]
        public int LAN_ID { get; set; }


        [Display(Name = "ID Usuario")]
        public int MKT_ID { get; set; }


        [Display(Name = "Nombres")]
        public string DOC_NOMBRES { get; set; }

        [Display(Name = "Apellidos")]
        public string DOC_APELLIDOS { get; set; }

        [Display(Name = "Nombres y Apellidos")]
        public string DOC_NOMBRES_APELLIDOS { get; set; }

        [Display(Name = "Celular")]
        public string DOC_CELULAR { get; set; }

        [Display(Name = "Email")]
        public string DOC_EMAIL { get; set; }

        public int CUR_ID { get; set; }

        public int USU_ID { get; set; }

        public string IdCurso { get; set; }

        [Display(Name = "Curso")]
        public string CUR_NOMBRE { get; set; }

        public string IdLanzamiento { get; set; }

                
        public DateTime LAN_FEC_CAPACITACION { get; set; }

        public string LAN_FEC_CAPACITACIONCADENA { get; set; }

        [Display(Name = "Fecha Registro")]
        public DateTime DCU_FEC { get; set; }

        public string DCU_FECCADENA { get; set; }

        public List<DetalleSeguimiento> DetalleSeguimiento { get; set; }

        [Display(Name = "Turno")]
        public DateTime TurnoFecha { get; set; }

        public string TurnoFechaCadena { get; set; }

        public List<MktDocenteCursoCLS> Listado { get; set; }


    }


    public class DetalleSeguimiento

    { 
        
        public string SGM_TIP { get; set; }
            
        public string SGM_COMENTARIO { get; set; }
    }


}

