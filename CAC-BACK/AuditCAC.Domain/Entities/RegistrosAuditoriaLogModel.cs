using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;

namespace AuditCAC.Domain.Entities
{
    [Keyless]
    public class RegistrosAuditoriaLogModel
    {
        public int CountQuery { get; set; }
        public int Paginas { get; set; }
        public int IdRadicado { get; set; }
        public int Id { get; set; }
        public int RegistroAuditoriaId { get; set; }
        public string Proceso { get; set; }
        public string Observacion { get; set; }
        public int EstadoAnterioId { get; set; }
        public int EstadoActual { get; set; }
        public string Codigo { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string NombreUsuario { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifyBy { get; set; }
        public DateTime ModificationDate { get; set; }
        public bool Status { get; set; }

        public string Fecha { get; set; }
        public string Hora { get; set; }
        public int MedicionId { get; set; }
        public string NombreMedicion { get; set; }
    }
}
