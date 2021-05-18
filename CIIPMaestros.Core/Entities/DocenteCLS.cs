using System;
using System.Collections.Generic;
using System.Text;

namespace CIIPMaestros.Core.Entities
{
    public class DocenteCLS
    {
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
		public int DEP_ID { get; set; }
	}
}
