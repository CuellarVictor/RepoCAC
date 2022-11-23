using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AuditCAC.Domain.Entities
{   
    [Table("UsuarioXEnfermedad")]
    public class UsuarioXEnfermedadModel
    {
        [Key]
        //public Guid Idk { get; set; }
        public int Id { get; set; }
        public int? IdCobertura { get; set; }
        public string IdUsuario { get; set; }
        public bool? Activo { get; set; }
        public string CreatedBy { get; set; }
        public DateTime CreatedDate { get; set; }
        public string ModifyBy { get; set; }
        public DateTime ModifyDate { get; set; }
        public bool Status { get; set; }
    }
}

