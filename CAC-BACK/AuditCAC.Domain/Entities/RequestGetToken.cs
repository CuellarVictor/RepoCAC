using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Entities
{
	public class RequestGetToken
	{
		public string User { get; set; }
		public string Password { get; set; }
	}
}
