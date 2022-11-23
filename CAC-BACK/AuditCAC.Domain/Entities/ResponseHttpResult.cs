using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Dal.Entities
{
	public class ResponseHttpResult
	{
		public HttpStatusCode HttpStatusCode { get; set; }
		public string StatusCode { get; set; }
		public string JsonResponse { get; set; }
		public string Token { get; set; }
	}
}
