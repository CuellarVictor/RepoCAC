using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiAuditCAC.Domain.Dto
{
    public class VariablesAsociadasDto
    {
        public VariablesAsociadasDto()
        {

        }

        public int VariableId { get; set; }
        public int MedicionId { get; set; }
        public int VariableAssociated { get; set; }
        public string Nombre { get; set; }
        public string Nemonico { get; set; }
        public bool Status { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifyBy { get; set; }
        public DateTime? ModifyDate { get; set; }
    }
}
