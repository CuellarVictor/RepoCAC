using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class ResponseRegistroAuditoriaDetalleInfo
    {
        public ResponseRegistroAuditoriaDetalleInfo()
        {

        }

        public ResponseRegistroAuditoriaDetalleInfo(InputsRegistroAuditoriaDetalle input)
        {
            this.Id = input.Id;
            this.RegistrosAuditoriaId = input.RegistrosAuditoriaId;
            this.CodigoVariable = input.CodigoVariable;
            this.VariableId = input.VariableId;
            this.NombreVariable = input.NombreVariable;
            this.DatoReportado = input.DatoReportado;
            this.EstadoVariableId = input.EstadoVariableId;
            this.NombreEstadoVariable = input.NombreEstadoVariable;
            this.SubGrupoId = input.SubGrupoId;
            this.SubGrupoDescripcion = input.SubGrupoDescripcion;
            this.MotivoVariable = input.MotivoVariable;
            this.Motivo = input.Motivo;
            this.Nombre = input.Nombre;

            this.MotivoVariablelist = new List<string>();

        }
        public int Id { get; set; }
        public int RegistrosAuditoriaId { get; set; }
        public string CodigoVariable { get; set; }
        public int VariableId { get; set; }
        public string NombreVariable { get; set; }
        public string DatoReportado { get; set; }
        public int EstadoVariableId { get; set; }
        public string NombreEstadoVariable { get; set; }
        public int SubGrupoId { get; set; }
        public string SubGrupoDescripcion { get; set; }
        public string MotivoVariable { get; set; }
        public List<string> MotivoVariablelist { get; set; }
        public string Motivo { get; set; }
        public string Nombre { get; set; }
    }
}
