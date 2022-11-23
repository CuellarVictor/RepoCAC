using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class ResponseBancoInformacionFiltradoDto
    {
        public int NoRegistrosTotales { get; set; }
        public int Id { get; set; }
        public string Nombre { get; set; }
        public int? Tipo { get; set; }
        public string NombreTipo { get; set; }
        public string Codigo { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifyBy { get; set; }
        public DateTime ModifyDate { get; set; }
        public int IdCobertura { get; set; }
        public string nombreCobertura { get; set; }

    }
}
