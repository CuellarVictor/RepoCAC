using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto.Calculadora
{
    public class InputCalculadoraTFG
    {
        public InputCalculadoraTFG()
        {

        }

        public decimal Edad { get; set; }
        public bool hombre { get; set; }
        public decimal Creatinina { get; set; }
        public decimal Peso { get; set; }
        public decimal Estatura { get; set; }

    }
}
