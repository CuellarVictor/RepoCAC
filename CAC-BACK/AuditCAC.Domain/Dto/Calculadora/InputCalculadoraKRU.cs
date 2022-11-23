using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto.Calculadora
{
    public class InputCalculadoraKRU
    {

        public InputCalculadoraKRU()
        {

        }

        public bool @Hemodialisis { get; set; }
        public decimal @NitrogenoUrinario { get; set; }
        public decimal @VolumenUrinario { get; set; }
        public decimal @BrunPre { get; set; }
        public decimal @BrunPost { get; set; }
    }
}
