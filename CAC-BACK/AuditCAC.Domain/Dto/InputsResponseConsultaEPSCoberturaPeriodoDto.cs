using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class InputsResponseConsultaEPSCoberturaPeriodoDto
    {
        public string IdCobertura { get; set; }
        public string IdPeriodo { get; set; }
        public string IdEPS { get; set; }
        
    }
}
