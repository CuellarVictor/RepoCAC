using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class InputsGetRegistrosAuditoriaXBolsaMedicionDto
    {
        public int PageNumber { get; set; } = 0;
        public int MaxRows { get; set; } = 1000;
        public List<string> IdRadicado { get; set; }
        public string AuditorId { get; set; }
        public List<int> MedicionId { get; set; }
        public string FechaAsignacionIni { get; set; }
        public string FechaAsignacionFin { get; set; }
        public List<string> Estado { get; set; }
        public List<string> CodigoEps { get; set; }
        public bool Finalizados { get; set; }
        public string KeyWord { get; set; }
    }
}
