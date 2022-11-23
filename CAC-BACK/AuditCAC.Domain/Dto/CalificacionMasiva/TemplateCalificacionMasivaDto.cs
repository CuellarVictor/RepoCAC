using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto.CalificacionMasiva
{
    public class TemplateCalificacionMasivaDto
    {
        public TemplateCalificacionMasivaDto()
        {

        }

        public int Id { get; set; }
        public string IdRadicado { get; set; }
        public string NemonicoVariable { get; set; }
        public string Calificacion { get; set; }
        public string Motivo { get; set; }
        public string Observacion { get; set; }
    }
}
