﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class DB_WebCIIPEntitiesERP : DbContext
    {
        public DB_WebCIIPEntitiesERP()
            : base("name=DB_WebCIIPEntitiesERP")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<MAE_CATEGORIAS> MAE_CATEGORIAS { get; set; }
        public virtual DbSet<MAE_CURSOS_PUNTUACION> MAE_CURSOS_PUNTUACION { get; set; }
        public virtual DbSet<MAE_DEPARTAMENTOS> MAE_DEPARTAMENTOS { get; set; }
        public virtual DbSet<MAE_TUTOR> MAE_TUTOR { get; set; }
        public virtual DbSet<MKT_DOCENTE_CURSO> MKT_DOCENTE_CURSO { get; set; }
        public virtual DbSet<PRV_DOCENTE_SEGUIMIENTO> PRV_DOCENTE_SEGUIMIENTO { get; set; }
        public virtual DbSet<SEG_USUARIOS> SEG_USUARIOS { get; set; }
        public virtual DbSet<MAE_TABLAS> MAE_TABLAS { get; set; }
        public virtual DbSet<MAE_ROLES> MAE_ROLES { get; set; }
        public virtual DbSet<MAE_CURSOS_LANZAMIENTOS> MAE_CURSOS_LANZAMIENTOS { get; set; }
        public virtual DbSet<SEG_USUARIOS_LINKS> SEG_USUARIOS_LINKS { get; set; }
        public virtual DbSet<MKT_DOCENTES> MKT_DOCENTES { get; set; }
        public virtual DbSet<MAE_CURSOS_HORARIOS> MAE_CURSOS_HORARIOS { get; set; }
        public virtual DbSet<SEG_CURSOS_USUARIOS_LINKS> SEG_CURSOS_USUARIOS_LINKS { get; set; }
        public virtual DbSet<MAE_CURSOS> MAE_CURSOS { get; set; }
    }
}
