using System;
using System.Collections.Generic;

namespace AuditCAC.Domain.Dto.Catalogo
{
    public class CatalogoDto
    {
        public int? Id { get; set; }
        public string CatalogName { get; set; }
        public bool Enable { get; set; }
        public bool Status { get; set; }
        public List<ItemDto> Items { get; set; }
    }
}
