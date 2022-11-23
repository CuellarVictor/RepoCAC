using AuditCAC.Domain.Dto;
using AuditCAC.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuditCAC.MainCore.Module.Interface
{
    public interface IProceduresRepository<TEntity>
    {        
        /* Procedures. */
        Task<List<CoberturaModel>> GetcacCoberturas(InputsCoberturaDto inputsCoberturaDto);
        Task<List<CoberturasErrorModel>> GetcacCoberturasError(InputsCoberturasErrorDto inputsCoberturasErrorDto);
        Task<List<ErrorModel>> GetcacError(InputsErrorDto inputsErrorDto);
        Task<List<PeriodoModel>> Getcacperiodo(InputsPeriodoDto inputsPeriodo);
        Task<List<RadicadoModel>> GetcacRadicado(InputsRadicadoDto inputsRadicado);
        Task<List<ReglaModel>> GetcacRegla(InputsReglaDto inputsReglaDto);
        Task<List<VariableModel>> GetCacvariable(InputsVariableDto inputsVariableDto);
        Task<List<VariablesPeriodoModel>> GetcacVariablesPeriodo(InputsVariablesPeriodoDto inputsVariablesPeriodoDto);
    }
}
