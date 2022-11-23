using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto.RegistroAuditoria
{
    public class DataTablaReferencial_RegistroAuditoriaModel
    {
        public DataTablaReferencial_RegistroAuditoriaModel()
        {

        }

        public int Id { get; set; }
        public string NombreCatalogo { get; set; }
        public string ItemId { get; set; }
        public string ItemDescripcion { get; set; }
    }
}
