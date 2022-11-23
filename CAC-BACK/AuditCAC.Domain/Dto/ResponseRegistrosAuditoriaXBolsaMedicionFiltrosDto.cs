using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class ResponseRegistrosAuditoriaXBolsaMedicionFiltrosDto
    {
        [Key]
        public Guid Idk { get; set; }
        public string NombreFiltro { get; set; }
        public string Id { get; set; }
        public string Valor { get; set; }
    }

    public class ResponseRegistrosAuditoriaXBolsaMedicionFiltros
    {
        public string NombreFiltro { get; set; }
        public List<DetallesRegistrosAuditoriaXBolsaMedicion> Detalle { get; set; }
    }

    public class DetallesRegistrosAuditoriaXBolsaMedicion
    {
        public string Id { get; set; }
        public string Valor { get; set; }
    }
}
