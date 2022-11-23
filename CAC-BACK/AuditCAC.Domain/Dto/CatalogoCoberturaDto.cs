using System;

namespace AuditCAC.Domain.Dto
{
    public class CatalogoCoberturaDto
    {
        public int Id { get; set; }
        public string NombreCatalogo { get; set; }
        public bool? Migrados { get; set; }
        public bool? Sincronizar { get; set; }
        public bool? Activo { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreationDate { get; set; }
        public string ModifyBy { get; set; }
        public DateTime? ModificationDate { get; set; }
        //public bool Status { get; set; }
    }
}
