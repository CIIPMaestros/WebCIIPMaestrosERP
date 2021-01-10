using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebCIIPMaestrosERP.Models
{
    public class MktDocenteCLS
    {
        public int Id { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Correo")]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Nombres")]
        public string names { get; set; }

        [Required]
        [Display(Name = "Apellidos")]
        public string lastnames { get; set; }

        [Display(Name = "Documento de Identidad:")]
        public string DocumentoDeIdentidad { get; set; }

        public List<PRV_DOCENTE_SEGUIMIENTO> ComentarioList { get; set; }
        public string Telefono { get; set; }

        public string SgmComentarioTipo { get; set; }

        [Display(Name = "Foto de perfil")]
        public string Region { get; set; }

        public int SituacionLaboral { get; set; }

        public int Cargo { get; set; }

        public int Nivel { get; set; }
    }
}