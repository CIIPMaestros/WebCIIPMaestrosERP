//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
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
        }
    
        public int LAN_ID { get; set; }
        public int CUR_ID { get; set; }
        public int TUT_ID { get; set; }
        public Nullable<System.DateTime> LAN_FEC_CAPACITACION { get; set; }
        public string LAN_ACTIVO { get; set; }
        public string LAN_ID_ENCRIPTADO { get; set; }
        public string IND_TIPO_LAN { get; set; }
    
        public virtual MAE_CURSOS MAE_CURSOS { get; set; }
        public virtual MAE_TUTOR MAE_TUTOR { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MKT_DOCENTE_CURSO> MKT_DOCENTE_CURSO { get; set; }
    }
}
