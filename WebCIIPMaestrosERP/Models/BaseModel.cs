using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebCIIPMaestrosERP.Models
{
    public class BaseModel
    {

        public int Page { get; set; }
        public int RegPerPage { get; set; }
        public int TotalReg { get; set; }

    }
}