using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Entities //AuditCAC.Dal.Entities
{
    public class ErrorModel
    {
        [Key]        
        public string idError { get; set; }
        public string descripcion { get; set; }
        public string idTipoError { get; set; }
    }
}
