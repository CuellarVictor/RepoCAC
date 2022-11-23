using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class ResponseGetTablasReferencialDto
    {
        [Key]
        public Guid Idk { get; set; }
        public string TablaReferencial { get; set; }
        public string Id { get; set; }
        public string Nombre { get; set; }        
    }
    public class ResponseGetTablasReferencial
    {
        public string TablaReferencial { get; set; }
        public List<ListaContenido> Contenido { get; set; }
    }

    public class ListaContenido
    {
        public string Id { get; set; }
        public string Nombre { get; set; }
    }
}
