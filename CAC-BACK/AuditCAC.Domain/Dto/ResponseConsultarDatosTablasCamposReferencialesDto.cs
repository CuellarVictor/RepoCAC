using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class ResponseConsultarDatosTablasCamposReferencialesDto
    {
        public string Status { get; set; }
        public string Mensaje { get; set; }
        public string TablaReferencial { get; set; }
        public string CampoReferencial { get; set; }
        public string CampoReferencialNombre { get; set; }
    }
}
