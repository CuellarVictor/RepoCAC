using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Entities
{
    [Table("Current_Process_Param")]
    public class CurrentProcessParamModel : BaseEntity
    {
        [Key]
        public int Id { get; set; }
        public int CurrentProcessId { get; set; }
        public string Name { get; set; }
        public int Position { get; set; }
        public string Value { get; set; }
    }
}
