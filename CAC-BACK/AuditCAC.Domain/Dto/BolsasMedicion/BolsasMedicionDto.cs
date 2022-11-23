using System;

namespace AuditCAC.Domain.Dto.BolsasMedicion
{
    public class BolsasMedicionDto
    {
        public string QueryNoRegistrosTotales { get; set; }        
        public int IdMedicion { get; set; }
        public string Medicion { get; set; }
        public int TotalRegistros { get; set; }
        public int TotalAsignados { get; set; }
        public int IdEnfMadre { get; set; }
        public string EnfMadre { get; set; }
        public int TotalAuditados { get; set; }
        public int IdEstadoAuditoria { get; set; }
        public string EstadoAuditoria { get; set; }                                      
        //public string Lider { get; set; }
        public string Resolucion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }
        public DateTime UltimaModificacion { get; set; }
        public string Progreso { get; set; }
        public bool EsConSubGrupo { get; set; }
    }
}
