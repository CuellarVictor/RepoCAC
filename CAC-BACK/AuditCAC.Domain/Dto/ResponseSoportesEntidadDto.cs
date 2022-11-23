using AuditCAC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class ResponseSoportesEntidadDto
    {
        [key]
        public int Id { get; set; } //Validar tipo de dato
        public int IdRegistrosAuditoria { get; set; }
        public int IdSoporte { get; set; }
        public int IdEntidad { get; set; }
        public string EntidadNombre { get; set; }
        public string NombreSoporte { get; set; }
        public string UrlSoporte { get; set; }


        //Id: bigint. OK 
        //IdRegistroAuditoria: int. OK 
        //IdSoporte: nvarchar(255) null. OK
        //UrlSoporte: nvarchar(255) null.  OK
    }
}
