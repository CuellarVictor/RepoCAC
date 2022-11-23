using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto.CalificacionMasiva
{
    public class InputCalificacionMasiva
    {
        public InputCalificacionMasiva()
        {

        }

        public string Usuario { get; set; }
        public string Medicion { get; set; }
        public string FileName { get; set; }
        public string FileBase64 { get; set; }
    }
}
