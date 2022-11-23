using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Entities
{
    [Table("ControlArchivos_CarguePoblacion")]
    public class ControlArchivos_CarguePoblacionModel
    {
        [Key]
        public int Id { get; set; }
        public int CurrentProcessId { get; set; }
        public int IdMedicion { get; set; }
        public string ArchivoCargado { get; set; }
        public string ArchivoDescargado { get; set; }
        public bool Enable { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime FecchaActualizacion { get; set; }
        public string Usuario { get; set; }
    }
}
