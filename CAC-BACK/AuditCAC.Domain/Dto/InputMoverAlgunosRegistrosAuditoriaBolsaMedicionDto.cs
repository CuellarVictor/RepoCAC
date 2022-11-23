using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class InputMoverAlgunosRegistrosAuditoriaBolsaMedicionDto
    {
        public InputMoverAlgunosRegistrosAuditoriaBolsaMedicionDto()
        {

        }

        //public string User { get; set; }
        //public List<int> IdMedicion { get; set; }
        public string FileName { get; set; }
        public string FileBase64 { get; set; }
        public string IdUsuario { get; set; }
    }
}
