using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebCIIPMaestrosERP.Models
{
    public class PrvDocenteSeguimiento
    {
        public int Id { get; set; }
        public DateTime SgmFecha { get; set; }
        [Required(ErrorMessage = "El campo tipo es requerido")]
        public string Tipo { get; set; }
        [Required(ErrorMessage = "El Comentario es requerido")]
        public string SgmComentario { get; set; }
    }
}