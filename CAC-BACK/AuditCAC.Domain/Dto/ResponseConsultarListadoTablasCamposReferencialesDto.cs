using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    [Table("Cacvariable")]
    public class ResponseConsultarListadoTablasCamposReferencialesDto
    {
        [Key]
        public int Id { get; set; }
        public string tablaReferencial { get; set; }
        public string campoReferencial { get; set; }
    }
}
