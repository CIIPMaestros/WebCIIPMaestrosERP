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
    
    public partial class MAE_CURSOS_PUNTUACION
    {
        public int PUN_ID { get; set; }
        public Nullable<int> DOC_ID { get; set; }
        public Nullable<int> CUR_ID { get; set; }
        public Nullable<decimal> PUN_VALOR { get; set; }
    
        public virtual MAE_CURSOS MAE_CURSOS { get; set; }
    }
}
