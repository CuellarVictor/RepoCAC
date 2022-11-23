using System.Collections.Generic;

namespace AuditCAC.Domain.Helpers
{
    public class MedicionRequest
    {
        public int PageNumber { get; set; }
        public int maxRows { get; set; }
        public string IdLider { get; set; }
        public string IdCobertura { get; set; }
        public List<int> IdEstado { get; set; }
    }
}
