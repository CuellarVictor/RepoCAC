using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Entities
{
	public class ResponseApiServiceResult
	{
		public Guid ProccesTx { get; set; }
		public string HttpStatusCode { get; set; }
		public string CodeTx { get; set; }
		public object Result { get; set; }
		public string Message { get; set; }
		public string Token { get; set; }
	}
}
