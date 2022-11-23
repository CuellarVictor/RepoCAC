using AuditCAC.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.MainCore.Module.Interface
{
    public interface ITablasReferencialRepository<TEntity>
    {
        Task<List<ResponseGetTablasReferencial>> GetTablasReferencial(); //InputsGetTablasReferencialDto inputsDto
        Task<List<ResponseGetTablasReferencialDto>> GetTablasReferencialByValorReferencial(List<InputsTablasReferencialByValorReferencialDto> entity);
    }
}
