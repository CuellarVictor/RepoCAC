using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class ResponseCatalogoCoberturaDto
    {
        public int NoRegistrosTotales { get; set; }
        public int Id { get; set; }
        public string NombreCatalogo { get; set; }
        public bool? Migrados { get; set; }
        public bool? Sincronizar { get; set; }
        public bool? Activo { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public string ModifyBy { get; set; }
        public DateTime? ModificationDate { get; set; }
    }
}
