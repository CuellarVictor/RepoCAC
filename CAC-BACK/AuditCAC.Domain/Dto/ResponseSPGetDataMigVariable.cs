using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    [Keyless]
    public class ResponseSPGetDataMigVariable
    {

		public Int32 idVariable { get; set; }
		public Int32 idCobertura { get; set; }
		public string nombre { get; set; }
		public string nemonico { get; set; }
		public string descripcion  { get; set; }
		public string idTipoVariable { get; set; }
		public Nullable<Int32> longitud { get; set; }
		public Nullable<Int32> decimales { get; set; }
		public string formato  { get; set; }
		public String tablaReferencial  { get; set; }
		public String campoReferencial { get; set; }
		public string idErrorTipo { get; set; }
        public int? orden { get; set; }

    }
}
