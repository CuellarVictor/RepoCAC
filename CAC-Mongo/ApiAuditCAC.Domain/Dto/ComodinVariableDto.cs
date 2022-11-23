using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiAuditCAC.Domain.Dto
{
    public class ComodinVariableDto
    {
        public ComodinVariableDto()
        {

        }

        public int ComodinId { get; set; }
        public int VariableId { get; set; }
        public int MedicionId { get; set; }
        public string Comodin { get; set; }
        public bool Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifyBy { get; set; }
        public DateTime? ModifyDate { get; set; }
    }
}
