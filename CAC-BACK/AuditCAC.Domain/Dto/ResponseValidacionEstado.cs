using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class ResponseValidacionEstado
    {
        [Key]
        public string Id { get; set; }
        public bool HabilitarVariablesCalificar { get; set; }
        public bool VisibleBotonGuardar { get; set; }
        public bool HabilitadoBotonGuardar { get; set; }
        public bool VisibleBotonMantenerCalificacion { get; set; }
        public bool VisibleBotonEditarCalificacion { get; set; }
        public bool VisibleBotonComiteExperto { get; set; }
        public bool VisibleBotonComiteAdministrativo { get; set; }
        public bool VisibleBotonLevantarGlosa { get; set; }        
        public bool ValidarErroresLogica { get; set; }
        public bool ObservacionHabilitada { get; set; }
        public bool ObservacionObligatoria { get; set; }
        public bool ObservacionRegistrada { get; set; }
        public bool CalificacionObligatoriaIPS { get; set; }
        public bool CalificacionIPSRegistrada { get; set; }
        public int IdItemGlosa { get; set; }
        public int IdItemDC { get; set; }
        public int IdItemNC { get; set; }
        public int TipificacionObservacionDefault { get; set; }
        public bool TipificacionObservasionHabilitada { get; set; }
        public int IdRegistroNuevo { get; set; }
        public string CodigoRegistroPendiente { get; set; }
        public bool HabilitarBotonMantenerCalificacion { get; set; }
        public bool HabilitarBotonEditarCalificacion { get; set; }
        public bool HabilitarBotonComiteExperto { get; set; }
        public bool HabilitarBotonComiteAdministrativo { get; set; }
        public bool HabilitarBotonLevantarGlosa { get; set; }
        public bool SolicitarMotivo { get; set; }
        public bool GuardarCadaCambioVariable { get; set; }
        public bool HabilitarGlosa { get; set; }
        public bool ErroresReportados { get; set; }
        //
        public bool VisibleBotonReversar { get; set; }
        public bool HabilitadoBotonReversar { get; set; }

    }
}
