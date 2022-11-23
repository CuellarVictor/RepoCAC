using AuditCAC.Domain.Dto;
using AuditCAC.Domain.Dto.Actas;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.MainCore.Module
{
    public interface IActaManager<TEntity>
    {

        /// <summary>
        /// Consulta parametros de los templates
        /// </summary>
        /// <returns>Lista Modelo parametros</returns>
        Task<List<ParametroTemplateDto>> ConsultaParametrosTemplate();

        /// <summary>
        /// Crea o actualiza parametro de los templates
        /// </summary>
        /// <param name="input">Modelo parametro</param>
        /// <returns>true</returns>
        Task<bool> UpsertParametroTemplate(ParametroTemplateDto input);
        Task<string> GenerarActaApertura(GenerarActaInputDto inputDto);
        Task<string> GenerarActaCierre(GenerarActaInputDto inputDto);
    }
}
