using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CIIPMaestros.Infrastructure.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CIIPMaestros.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DocenteController : ControllerBase
    {
        public IActionResult GetDocente() {

            var Docente = new DocenteRepository().GetDocentes();
            return Ok(Docente);

        }
    }
}