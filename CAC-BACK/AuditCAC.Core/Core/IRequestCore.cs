using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuditCAC.Domain.Entities;

namespace AuditCAC.Core.Core
{
    interface IRequestCore
    {
        ResponseApiServiceResult requestAuthenticationCAC(string urlBase, string email, string password);
        ResponseApiServiceResult requestGetpatient(string url, object Data, string token);

    }
}
