using System.Collections.Generic;

namespace AuditCAC.Domain.Dto.PanelEnfermedad
{
    public class MedicionesDto
    {
        public string Enfermedad { get; set; }
        public List<UsuariosDto> Usuarios { get; set; }
    }
}
