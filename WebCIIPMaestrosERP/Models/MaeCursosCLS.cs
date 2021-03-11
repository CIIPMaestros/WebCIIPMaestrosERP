﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebCIIPMaestrosERP.Models
{
    public class MaeCursosCLS : BaseModel
    {

        [Display(Name = "Id")]
        public int CUR_ID { get; set; }
        [Display(Name = "Nombre Curso")]

        [Required]
        public string CUR_NOMBRE { get; set; }
        [Display(Name = "Descripcion")]
        [Required]
        public string CUR_DESCRIPCION { get; set; }
        [Display(Name = "Certificacion")]
        public string CUR_CERTIFICACION { get; set; }
        
        [Display(Name = "Resultados")]
        
        public string CUR_RESULTADOS { get; set; }
        [Required]
        [Display(Name = "Precio")]
        [Range(0, 100000, ErrorMessage = "Fuera de rango")]
        public decimal CUR_PRECIO { get; set; }
        [Display(Name = "Activo")]
        public string CUR_ACTIVO { get; set; }


        [Display(Name = "Estado")]
        public string CUR_ESTADO { get; set; }


        [Display(Name = "Categoria")]
        //[Required]
        public int CAT_ID { get; set; }


        [Display(Name = "Categoria")]
        //[Required]
        public string CAT_DESCRIPCION { get; set; }

        [Display(Name = "Imagen")]
        public string CUR_IMAGEN { get; set; }

        public List<Horarios> horarios { get; set; }

        public List<MAE_CURSOS_HORARIOS> GetHorarios { get; set; }


        public string CUR_ID_ENCRIPTADO { get; set; }

        public List<MAE_CURSOS_HORARIOS> getHorarios { get; set; }

        public List<MaeCursosCLS> ListadoCursos { get; set; }

    }

    public class Horarios { 
        
        public int CURS_ID { get; set; }

        public string SCH_DIA { get; set; }
                
        public string SCH_HORA { get; set; }

        public string SCH_MT { get; set; }

    }



}