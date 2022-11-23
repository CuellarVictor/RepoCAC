using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace AuditCAC.Domain.Entities //AuditCAC.Dal.Entities
{
    public class VariablesPeriodoModel
    {
        [Key]
        public int? idPeriodo { get; set; }
        public int? idVariable { get; set; }
        public int? orden { get; set; }
        public string variable_num_segun_resoucion { get; set; }

    }
}
