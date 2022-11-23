using AuditCAC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{

    public class VariableCondicionalDto
    {
        [key]
        public long Id { get; set; }
        public int VariablePadreId { get; set; }
        public int VariableHijaId { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifyDate { get; set; }
        public string ModifyBy { get; set; }
        public bool Enable { get; set; }
    }
}
