using System;

namespace AuditCAC.Domain.Dto.Catalogo
{
    public class ItemDto
    {
        public int? Id { get; set; }
        public int CatalogId { get; set; }
        public string ItemName { get; set; }
        public string Concept { get; set; }
        public bool Enable { get; set; }
        public bool Status { get; set; }
    }
}
