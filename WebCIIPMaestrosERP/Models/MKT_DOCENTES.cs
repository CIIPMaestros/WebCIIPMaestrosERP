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
    
    public partial class MKT_DOCENTES
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public MKT_DOCENTES()
        {
            this.MAE_CURSOS_PUNTUACION = new HashSet<MAE_CURSOS_PUNTUACION>();
            this.PRV_DOCENTE_SEGUIMIENTO = new HashSet<PRV_DOCENTE_SEGUIMIENTO>();
            this.MKT_DOCENTE_CURSO = new HashSet<MKT_DOCENTE_CURSO>();
        }
    
        public int DOC_ID { get; set; }
        public string DOC_NOMBRES { get; set; }
        public string DOC_NICK { get; set; }
        public string DOC_APELLIDOS { get; set; }
        public string DOC_DNI { get; set; }
        public string DOC_CELULAR { get; set; }
        public string DOC_EMAIL { get; set; }
        public string DOC_SIT_LAB { get; set; }
        public string DOC_CARGO { get; set; }
        public string DOC_NIVEL { get; set; }
        public string DOC_CONTRASENA { get; set; }
        public string DOC_DISPOSITIVO { get; set; }
        public string DOC_PAIS { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MAE_CURSOS_PUNTUACION> MAE_CURSOS_PUNTUACION { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<PRV_DOCENTE_SEGUIMIENTO> PRV_DOCENTE_SEGUIMIENTO { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<MKT_DOCENTE_CURSO> MKT_DOCENTE_CURSO { get; set; }
    }
}
