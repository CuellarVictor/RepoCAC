using AuditCAC.Domain.Dto;
using AuditCAC.Domain.Dto.Cobertura;
using AuditCAC.Domain.Entities.Cobertura;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuditCAC.MainCore.Module.Interface
{
    public interface ICoberturaRepository<TEntity>
    {
        Task<List<CoberturaXUsuarioModel>> GetEnfermedadesMadreXUsuario(InputCoberturaDto objCobertura);      

    }
}
