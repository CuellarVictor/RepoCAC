using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiAuditCAC.Domain.Dto
{
    public class CalificacionDto
    {
        public CalificacionDto()
        {

        }

        public int Id { get; set; }
        public int RegistrosAuditoriaId { get; set; }
        public int RegistrosAuditoriaDetalleId { get; set; }
        public int VariableId { get; set; }
        public int IpsId { get; set; }
        public string ItemCalificacionNombre { get; set; }
        public int Calificacion { get; set; }
        public string Observacion { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifyBy { get; set; }
        public DateTime? ModifyDate { get; set; }
    }
}
