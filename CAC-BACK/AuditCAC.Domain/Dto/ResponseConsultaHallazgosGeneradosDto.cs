using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class ResponseConsultaHallazgosGeneradosDto
    {
        public int RegistroAuditoriaId { get; set; }
        public int RadicadoId { get; set; }
        public string CodigoEstado { get; set; }
        public string DescripcioneEstado { get; set; }
        public int RegistroAuditoriaDetalleId { get; set; }
        public int IdVariableSISCAC { get; set; }
        public string Nemonico { get; set; }
        public int TipoVariableId { get; set; }
        public string TipoVariableNombre { get; set; }
        public DateTime FechaModificacion { get; set; }
        public int IdMedicion { get; set; }
        public string Observacion { get; set; }

    }
}
