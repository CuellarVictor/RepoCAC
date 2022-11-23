using AuditCAC.Domain.Dto;
using AuditCAC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.MainCore.Module.Interface
{
    public interface IVariablesRepository<TEntity>
    {
        Task<IEnumerable<VariablesModel>> GetAll();
        Task<VariablesModel> Get(int id);
        Task Add(VariablesModel entity);
        Task Update(VariablesModel dbEntity, VariablesModel entity);
        Task Delete(VariablesModel entity);
        Task AtarListadoVariablesMedicion(List<InputsCreateVariablesDto> entity); //Task AtarListadoVariablesMedicion(List<VariablesModel> entity);
        Task<bool> DuplicarVariables(InputsDuplicarVariablesDto entity);
        Task<bool> DuplicarMedicion(InputsDuplicarMedicionDto entity);
        Task<Tuple<List<ResponseVariablesDetails>, int, int, int>> GetVariablesFiltrado(InputsVariablesDto inputsDto); //Task<Tuple<List<ResponseVariablesDto>, int, int, int>> GetVariablesFiltrado(InputsVariablesDto inputsDto);
        Task ActualizarVariablesLider(InputsActualizarVariablesLiderDto entity);
        Task ActualizarVariablesLiderMasivo(List<InputsActualizarVariablesLiderDto> entity);
        Task<bool> CrearVariables(InputsCEVariablesDto inputsDto);
        Task<bool> ActualizarVariables(InputsCEVariablesDto inputsDto);
        Task<List<ResponseDetalleConsultarOrderVariablesDto>> ConsultarOrderVariables(InputConsultarOrderVariablesDto inputsDto);

        /// <summary>
        /// Consulta variables condicionadas por id de variable
        /// </summary>
        /// <param name="variableId"></param>
        /// <returns></returns>
        Task<List<VariableCondicionalDto>> ConsultaVariablesCondicionadas(int variableId, int medicionId);
    }
}
