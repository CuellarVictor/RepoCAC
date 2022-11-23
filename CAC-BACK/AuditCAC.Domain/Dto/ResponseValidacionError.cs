using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class ResponseValidacionError
    {
        public ResponseValidacionError()
        {

        }

        [Key]
        public int Id { get; set; }
        public int IdRegistroAuditoriaDetalle { get; set; }
        public int IdT { get; set; }
        public bool? Result { get; set; }
        public bool? Obligatoria { get; set; }
        public string Sentencia { get; set; }
        public int? VariableId { get; set; }
        public int? IdRestriccion { get; set; }
        public int? IdRegla { get; set; }
        public string IdError { get; set; }
        public string DescripcionError { get; set; }
        public bool ResultGroup { get; set; }
    }
}
