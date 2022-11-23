using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto.CalificacionMasiva
{
    public class ResultadoCalificacionMasiva
    {
        public ResultadoCalificacionMasiva()
        {

        }

        public long Id { get; set; }
        public int? CurrentProcessId { get; set; }
        public int? RegistroAuditoriaDetalleId { get; set; }
        public int? IdRadicado { get; set; }
        public int? VariableId { get; set; }
        public string NemonicoVariable { get; set; }
        public string Tipo { get; set; }
        public string Result { get; set; }
        public string RegistroEstadoAnterior { get; set; }
        public string RegistroEstadoNuevo { get; set; }
        public DateTime CreateDate { get; set; }
    }
}
