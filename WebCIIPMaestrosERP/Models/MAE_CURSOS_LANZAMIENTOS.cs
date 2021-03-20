//------------------------------------------------------------------------------
// <auto-generated>
//     Este código se generó a partir de una plantilla.
//
//     Los cambios manuales en este archivo pueden causar un comportamiento inesperado de la aplicación.
//     Los cambios manuales en este archivo se sobrescribirán si se regenera el código.
// </auto-generated>
//------------------------------------------------------------------------------

namespace WebCIIPMaestrosERP.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class MAE_CURSOS_LANZAMIENTOS
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MAE_CURSOS_LANZAMIENTOS()
        {
            this.MKT_DOCENTE_CURSO = new HashSet<MKT_DOCENTE_CURSO>();
            this.SEG_USUARIOS_LINKS = new HashSet<SEG_USUARIOS_LINKS>();
        }
    
        public int LAN_ID { get; set; }
        public int CUR_ID { get; set; }
        public int TUT_ID { get; set; }
        public Nullable<System.DateTime> LAN_FEC_CAPACITACION { get; set; }
        public string LAN_ACTIVO { get; set; }
        public string LAN_ID_ENCRIPTADO { get; set; }
        public string IND_TIPO_LAN { get; set; }
        public string DIA_SEMANA { get; set; }
        public string IND_AUTO { get; set; }
        public Nullable<System.DateTime> LAN_CREACION_HORARIO { get; set; }
        public int LAN_NUM_SEMANA { get; set; }
    
        public virtual MAE_CURSOS MAE_CURSOS { get; set; }
        public virtual MAE_TUTOR MAE_TUTOR { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MKT_DOCENTE_CURSO> MKT_DOCENTE_CURSO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<SEG_USUARIOS_LINKS> SEG_USUARIOS_LINKS { get; set; }
    }
}
