using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuditCAC.Domain.Entities.ReglaVariable
{
    [Table("ReglasVariable")]
    public class ReglasVariableModel
    {
        public int Id { get; set; }
        public int IdRegla { get; set; }
        public int IdVariable { get; set; }
        public string Concepto { get; set; }
        public DateTime DateCreate { get; set; }
        public string CreatedBy { get; set; }
        public DateTime ModifyDate { get; set; }
        public string ModifyBy { get; set; }
        public bool Enable { get; set; }
    }
}
