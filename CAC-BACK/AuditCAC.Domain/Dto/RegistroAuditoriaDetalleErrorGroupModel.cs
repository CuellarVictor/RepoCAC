using AuditCAC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class RegistroAuditoriaDetalleErrorGroupModel
    {
        public RegistroAuditoriaDetalleErrorGroupModel()
        {

        }

        public string IdError { get; set; }
        public string DescripcionError { get; set; }
        public int RegistrosAuditoriaDetalleId { get; set; }
        public int VariableId { get; set; }
        public List<RegistroAuditoriaDetalleErrorModel> Restricciones { get; set; }
    }
}
