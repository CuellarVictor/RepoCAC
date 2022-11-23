using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.MainCore.Module.Lider.Interfaces
{
    public interface ILiderManager
    {
        Task<string> GetIssuesLider(string id_auditor);
    }
}
