using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class AutocompleteCatalogoCoberturaDto
    {
        [Required]
        public string tablaReferencial { get; set; }
        public string campoReferencial { get; set; }
        [Required]
        public string valorBusqueda { get; set; }
    }
}