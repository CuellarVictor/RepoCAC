using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class InputsRegistroAuditoriaDetalle
    {
        public InputsRegistroAuditoriaDetalle()
        {

        }

        [Key]
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
        public int SubGrupoOrden { get; set; }
        public string MotivoVariable { get; set; }
        public string Motivo { get; set; }
        public int? Dato_DC_NC_ND { get; set; }
        public bool? Visible { get; set; }
        public bool? Bot { get; set; }
        public bool? VariableEncuesta { get; set; }
        public bool? RegistrosAuditoriaEncuesta { get; set; }        
        public string Nombre { get; set; } //public string IpsNombre { get; set; }
        public string TablaReferencial { get; set; }
        public string CampoReferencial { get; set; }
        public string IdTipoVariable { get; set; }
        public int? Longitud { get; set; }
        public int? Decimales { get; set; }
        public string Formato { get; set; }
        public string ValorDatoReportado { get; set; }
        public int Orden { get; set; }
        public int TipoVariableId { get; set; }
        public string TipoVariableNombre { get; set; }
        public bool EnableDC { get; set; }
        public bool EnableNC { get; set; }
        public bool EnableND { get; set; }
        public bool Condicionada { get; set; }
        public string ValorConstante { get; set; }
        public string DescripcionVariable { get; set; }
        public bool Alerta { get; set; }
        public string AlertaDescripcion { get; set; }
        public int MedicionId { get; set; }
        public bool EsVisible { get; set; }
        public bool EsCalificable { get; set; }
        public int CountHallazgos { get; set; }
        public string Contexto { get; set; }
        public bool? Calculadora { get; set; }
        public int? TipoCalculadora { get; set; }
        public int EstadoMedicion { get; set; }
    }
}
