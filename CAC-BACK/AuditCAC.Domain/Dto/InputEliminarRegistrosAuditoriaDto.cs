using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class InputEliminarRegistrosAuditoriaDto
    {
        public InputEliminarRegistrosAuditoriaDto()
        {

        }

        public string FileName { get; set; }
        public string FileBase64 { get; set; }
        public string MedicionId { get; set; }
        public string Observacion { get; set; }
        public string IdUsuario { get; set; }
    }
}
