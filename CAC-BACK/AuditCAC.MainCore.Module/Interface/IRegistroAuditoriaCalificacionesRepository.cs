using AuditCAC.Domain.Dto;
using AuditCAC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.MainCore.Module.Interface
{
    public interface IRegistroAuditoriaCalificacionesRepository<TEntity>
    {
        Task CalificarRegistroAuditoria(List<InputsRegistroAuditoriaCalificacionesDto> inputsDto);
        Task<List<ResponseRegistroAuditoriaCalificacionesDto>> GetCalificacionesRegistroAuditoriaByRegistrosAuditoriaId(InputsGetRegistroAuditoriaCalificacionesByRegistrosAuditoriaIdDto inputsDto);
        Task<List<ResponseRegistroAuditoriaCalificacionesDto>> GetCalificacionesRegistroAuditoriaByVariableId(InputsGetRegistroAuditoriaCalificacionesByVariableIdDto inputsDto);        
        Task<bool> GetCalificacionEsCompletas(InputsGetCalificacionEsCompletasDto inputsDto);

        /// <summary>
        /// Para la consultar Items calificable asociados a una variable
        /// </summary>
        /// <param name="VariableId">Id de variable</param>
        /// <returns>Modelo de datos</returns>
        Task<List<ItemModel>> GetItemsByVariableId(int VariableId);
    }
}
