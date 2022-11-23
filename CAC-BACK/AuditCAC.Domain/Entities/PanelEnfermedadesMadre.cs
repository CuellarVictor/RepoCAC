using Microsoft.EntityFrameworkCore;

namespace AuditCAC.Domain.Entities
{
    [Keyless]
    public class PanelEnfermedadesMadre
    { 
        public string Grupo { get; set; }
        public string Enfermedad { get; set; }
        public string Auditor { get; set; }
        public string Codigo { get; set; }
        public string Activo { get; set; }
    }
}
