using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiAuditCAC.Domain.Dto
{
    public class SpRegistrosAudResponseDto
    {
        public int Id { get; set; }
        public int IdRadicado { get; set; }
        public int IdMedicion { get; set; }
        public int Estado { get; set; }
        public String Nombre { get; set; }
        public String Codigo { get; set; }
        public String Descripcion { get; set; }
        public string EPS { get; set; }
        public string NombreMedicion { get; set; }
        public DateTime? ModifyDate { get; set; }
    }
}
