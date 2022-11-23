using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class ResponseMoverTodosRegistrosAuditoriaBolsaMedicionDto
    {
        public int? Id { get; set; }
        public int? IdBolsaOrigen { get; set; }
        public string BolsaOrigen { get; set; }
        public int? IdBolsaDestino { get; set; }
        public int? IdRadicado { get; set; }
        public string EstadoRegistro { get; set; }
        public string Entidad { get; set; }
        public string CodigoAuditor { get; set; }
        public DateTime? FechaAsignacion { get; set; }
        public string EstadoEjecucion { get; set; }
        public string MensajeEjecucion { get; set; }
    }
}
