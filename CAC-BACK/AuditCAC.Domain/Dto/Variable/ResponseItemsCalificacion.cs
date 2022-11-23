using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto.Variable
{
    public class ResponseItemsCalificacion
    {
        [Key]
        public string Id { get; set; }
        public int VariableId { get; set; }
        public int ItemId { get; set; }
    }
}
