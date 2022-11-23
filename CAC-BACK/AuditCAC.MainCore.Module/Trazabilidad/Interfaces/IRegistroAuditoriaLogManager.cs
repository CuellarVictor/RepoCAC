using AuditCAC.Domain.Dto;
using AuditCAC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuditCAC.MainCore.Module.Trazabilidad.Interfaces
{
    public interface IRegistroAuditoriaLogManager
    {
        /// <summary>
        /// Consuta registros de log auditoria
        /// </summary>
        /// <param name="inputsDto">Modelo parametros de consulta</param>
        /// <returns>Modelo Registros Auditoria Log</returns>
        Task<List<RegistrosAuditoriaLogModel>> ConsultaLogAccion(InputsGetRegistrosAuditoriaLogDto inputsDto);
    }
}
