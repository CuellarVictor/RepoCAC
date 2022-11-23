using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Entities
{
    public class CantidadRegistrosModel
    {
        [Key]
        public Guid Idk { get; set; }
        public int Registros { get; set; }
        public int Asignados { get; set; }
        
    }
}
