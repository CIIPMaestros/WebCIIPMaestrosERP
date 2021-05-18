using CIIPMaestros.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CIIPMaestros.Infrastructure.Repositories
{
    public class DocenteRepository
    {
        public IEnumerable<DocenteCLS> GetDocentes()
        {
            var Docente = Enumerable.Range(1, 10).Select(x => new DocenteCLS
            {
                DOC_ID = x,
                DOC_NOMBRES = $"Nombre {x}"

            });

            return (Docente);

        }
    }
}
