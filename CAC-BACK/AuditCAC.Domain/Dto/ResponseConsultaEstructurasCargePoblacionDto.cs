using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class ResponseConsultaEstructurasCargePoblacionDto
    {
        [Key]
        public int SubGrupoId { get; set; }
        public string SubGrupoNombre { get; set; }
    }
}
