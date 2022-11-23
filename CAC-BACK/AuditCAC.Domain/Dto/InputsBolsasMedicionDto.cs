using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class InputsBolsasMedicionDto
    {
        public int PageNumber { get; set; } = 0;
        public int MaxRows { get; set; } = 1000;
        public int MedicionId { get; set; }
        public List<string> AuditorId { get; set; } //int?
        public string KeyWord { get; set; }
    }
}
