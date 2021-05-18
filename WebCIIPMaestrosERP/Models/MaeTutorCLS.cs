using System.ComponentModel.DataAnnotations;

namespace WebCIIPMaestrosERP.Models
{
    public class MaeTutorCLS: BaseModel
    {
        [Display(Name = "ID Ponente")]
        public int TUT_ID { get; set; }

        [Display(Name = "Nombre")]
        [Required]
        [StringLength(100, ErrorMessage = "La longitud maxima es 100 caracteres")]
        public string TUT_NOMBRES { get; set; }

        [Display(Name = "Apellidos")]
        [Required]
        [StringLength(100, ErrorMessage = "La longitud maxima es de 100 caracteres")]
        public string TUT_APELLIDOS { get; set; }


        [Display(Name = "Email")]
        [Required]
        [EmailAddress(ErrorMessage = "Ingrese un email valido")]
        public string TUT_EMAIL { get; set; }


        [Display(Name = "Movil")]
        public string TUT_TELEFONO { get; set; }

        [Display(Name = "Foto")]
        //[Required]
        public string TUT_FOTO { get; set; }

        [Display(Name = "Activo")]
        public string TUT_ACTIVO { get; set; }

        [Display(Name = "Estado")]
        public string TUT_ESTADO { get; set; }

        public string extension { get; set; }

        public string fotoCadena { get; set; }
    }
}