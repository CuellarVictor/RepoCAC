using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class InputsRegistroAuditoriaCalificacionesDto //List<InputsRegistroAuditoriaCalificacionesDto>
    {
        [Key]
        public int Id { get; set; }
        public int RegistrosAuditoriaId { get; set; }
        public int RegistrosAuditoriaDetalleId { get; set; }
        public int VariableId { get; set; }
        public int IpsId { get; set; }
        public int ItemId { get; set; }
        public int Calificacion { get; set; }
        public string Observacion { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifyBy { get; set; }
        public DateTime? ModifyDate { get; set; }

        //public InputsRegistroAuditoriaCalificacionesDto()
        //{

        //}

        //public int Id { get; set; }
        //public int RegistrosAuditoriaId { get; set; }
        //public int IpsId { get; set; }
        //public List<RegistroAuditoriaCalificaciones> ListadoDetalles { get; set; }
        //public string Observacion { get; set; }
        //public string CreatedBy { get; set; }
        //public string ModifyBy { get; set; }

        //public class RegistroAuditoriaCalificaciones
        //{
        //    public int Id { get; set; }
        //    public int ItemId { get; set; }
        //    public int Calificacion { get; set; }
        //}
    }
}
