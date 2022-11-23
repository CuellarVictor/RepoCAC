using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class ResponseRegistrosAuditoriaFiltrosDto
    {
        [Key]
        public Guid Idk { get; set; }
        public string NombreFiltro { get; set; }        
        public string Id { get; set; }
        public string Valor { get; set; }
    }
    
    public class ResponseRegistrosAuditoriaFiltros
    {
        public string NombreFiltro { get; set; }
        public List<Detalles> Detalle { get; set; }
    }

    //public class ResponseRegistrosAuditoriaFiltrosDto
    //{
    //    List<Detalles> NombreFiltro { get; set; }
    //}
    public class Detalles
    {
        public string Id { get; set; }
        public string Valor { get; set; }
    }
}
