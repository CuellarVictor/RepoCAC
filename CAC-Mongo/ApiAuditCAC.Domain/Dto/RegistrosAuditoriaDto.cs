using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiAuditCAC.Domain.Dto
{
    public class RegistrosAuditoriaDto
    {
        public RegistrosAuditoriaDto()
        {

        }
        public int Id { get; set; }
        public int IdRadicado { get; set; }
        public int IdMedicion { get; set; }
        public int IdPeriodo { get; set; }
        public string IdAuditor { get; set; }
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public string Sexo { get; set; }
        public string TipoIdentificacion { get; set; }
        public string Identificacion { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaAuditoria { get; set; }
        public DateTime? FechaMinConsultaAudit { get; set; }
        public DateTime? FechaMaxConsultaAudit { get; set; }
        public DateTime? FechaAsignacion { get; set; }
        public bool Activo { get; set; }
        public bool Reverse { get; set; }
        public int DisplayOrder { get; set; }
        public bool Ara { get; set; }
        public bool Eps { get; set; }
        public DateTime? FechaReverso { get; set; }
        public bool AraAtendido { get; set; }
        public bool EpsAtendido { get; set; }
        public bool Revisar { get; set; }
        public bool Extemporaneo { get; set; }
        public int EstadoRegistroAuditoria { get; set; }
        public string CodigoEstadoRegistroAuditoria { get; set; }
        public string DescripcionEstadoRegistroAuditoria { get; set; }
        public int LevantarGlosa { get; set; }
        public int MantenerCalificacion { get; set; }
        public int ComiteExperto { get; set; }
        public int ComiteAdministrativo { get; set; }
        public int AccionLider { get; set; }
        public int AccionAuditor { get; set; }
        public bool EncuestaRegistroAuditoria { get; set; }
        public string CreatedByRegistroAuditoria { get; set; }
        public DateTime? CreatedDateRegistroAuditoria { get; set; }
        public string ModifyByRegistroAuditoria { get; set; }
        public DateTime? ModifyDateRegistroAuditoria { get; set; }
        public string IdEPS { get; set; }
        public bool StatusRegistroAuditoria { get; set; }
        public int IdRegistroAuditoriaDetalle { get; set; }
        public int idVariable { get; set; }
        public int VariableId { get; set; }
        public string CreatedByRegistroAuditoriaDetalle { get; set; }
        public DateTime? CreatedDateRegistroAuditoriaDetalle { get; set; }
        public string ModifyByRegistroAuditoriaDetalle { get; set; }
        public DateTime? ModifyDateRegistroAuditoriaDetalle { get; set; }
        public string DatoReportado { get; set; }
        public string MotivoVariable { get; set; }
        public int Dato_DC_NC_ND { get; set; }
        public string Dato_DC_NC_NDNombre { get; set; }
        public bool AraRegistroAuditoriaDetalle { get; set; }
        public bool StatusRegistroAuditoriaDetalle { get; set; }
        public string Contexto { get; set; }
        public bool EsGlosa { get; set; }
        public bool EsVisible { get; set; }
        public bool EsCalificable { get; set; }
        public bool EnableDC { get; set; }
        public bool EnableNC { get; set; }
        public bool EnableND { get; set; }
        public int CalificacionXDefecto { get; set; }
        public string CalificacionXDefectoNombre { get; set; }
        public int SubGrupoId { get; set; }
        public string SubGrupoNombre { get; set; }
        public bool EncuestaVariablexMedicion { get; set; }
        public int OrdenVariablexMedicion { get; set; }
        public bool StatusVariablexMedicion { get; set; }
        public int TipoCampo { get; set; }
        public string TipoCampoNombre { get; set; }
        public bool Promedio { get; set; }
        public bool ValidarEntreRangos { get; set; }
        public string Desde { get; set; }
        public string Hasta { get; set; }
        public bool Condicionada { get; set; }
        public string ValorConstante { get; set; }
        public bool Lista { get; set; }
        public bool ActivoVariableXMedicion { get; set; }
        public string CreatedByVariableXMedicion { get; set; }
        public DateTime? CreationDateVariableXMedicion { get; set; }
        public string ModifyByVariableXMedicion { get; set; }
        public DateTime? ModificationDateVariableXMedicion { get; set; }
        public bool Hallazgos { get; set; }
        public bool? Calculadora { get; set; }
        public int? TipoCalculadora { get; set; }
        public bool? VariablesAsociados { get; set; }
        public bool? Comodines { get; set; }
        public bool Activa { get; set; }
        public int OrdenVariable { get; set; }
        public int idCobertura { get; set; }
        public string nombreVariable { get; set; }
        public string nemonicoVariable { get; set; }
        public string descripcionVariable { get; set; }
        public string idTipoVariable { get; set; }
        public int longitud { get; set; }
        public int decimales { get; set; }
        public string formato { get; set; }
        public string tablaReferencial { get; set; }
        public string campoReferencial { get; set; }
        public bool Bot { get; set; }
        public int TipoVariableItem { get; set; }
        public string ItemTipoVariableNombre { get; set; }
        public bool Alerta { get; set; }
        public string AlertaDescripcion { get; set; }
        public bool StatusVariable { get; set; }
        public string idErrorTipo { get; set; }
        public int EstadoMedicion { get; set; }
        public string EstadoMedicionNombre { get; set; }
        public string MedicionNombre { get; set; }

        public string MedicionDescripcion { get; set; }
        public int MedicionIdCobertura { get; set; }
        public DateTime? FechaFinAuditoria { get; set; }
        public string CoberturaNombre { get; set; }
        public string CoberturaNemonico { get; set; }
        public string CoberturaLegislacion { get; set; }
        public string CoberturaDefinicion { get; set; }
        public int RegimenEPS_Id { get; set; }
        public string RegimenEPS_Nombre { get; set; }
        public int RenglonEPS_Id { get; set; }
        public string RenglonEPS_Nombre { get; set; }

    }
}
