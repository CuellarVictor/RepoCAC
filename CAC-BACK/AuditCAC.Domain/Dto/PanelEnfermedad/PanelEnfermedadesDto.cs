using System.Collections.Generic;

namespace AuditCAC.Domain.Dto.PanelEnfermedad
{
    public class PanelEnfermedadesDto
    {
        public string Grupo { get; set; }
        public List<MedicionesDto> Mediciones { get; set; }
    }
}
