using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Entities //AuditCAC.Dal.Entities
{
    public class CoberturaModel
    {
        [Key]              
        public int idCobertura { get; set; }
        public string nombre { get; set; }
        public string nemonico { get; set; }
        public string legislacion { get; set; }
        public string definicion { get; set; }
        public int? tiempoEstimado { get; set; }
        public bool? ExcluirNovedades { get; set; }
        public string Novedades { get; set; }
        public int? idResolutionSiame { get; set; }
        public string NovedadesCompartidosUnicos { get; set; }
        public int? CantidadVariables { get; set; }
        public int? idCoberturaPadre { get; set; }
        public int? idCoberturaAdicionales { get; set; }
    }
}
