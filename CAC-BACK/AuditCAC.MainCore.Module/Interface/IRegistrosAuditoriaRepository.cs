using AuditCAC.Domain.Dto;
using AuditCAC.Domain.Dto.RegistroAuditoria;
using AuditCAC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.MainCore.Module.Interface
{
    public interface IRegistrosAuditoriaRepository<TEntity>
    {
        Task<IEnumerable<RegistrosAuditoriaModel>> GetAll();
        Task<RegistrosAuditoriaModel> Get(int id);
        Task Add(RegistrosAuditoriaModel entity);        
        Task Update(RegistrosAuditoriaModel dbEntity, RegistrosAuditoriaModel entity);
        Task Delete(RegistrosAuditoriaModel entity);
        Task MoverRegistrosAuditoria(InputsMoverRegistrosAuditoriaDto inputs);
        Task<List<ResponseRegistrosAuditoriaAsignadoAuditorEstadoDto>> GetRegistrosAuditoriaAsignadoAuditorEstado(InputsRegistrosAuditoriaAsignadoAuditorEstadoDto inputsDto); //RegistrosAuditoriaModel
        Task<Tuple<List<ResponseRegistrosAuditoriaModelDto>, int, int, int>> GetRegistrosAuditoriaFiltrado(InputsRegistrosAuditoriaFiltradoDto inputsDto); //RegistrosAuditoriaResponseModel, RegistrosAuditoriaResponseModel
        Task<List<ResponseRegistrosAuditoriaFiltros>> GetRegistrosAuditoriaFiltros(InputsRegistrosAuditoriaFiltrosDto inputsDto);
        Task<List<DetalleAsignacionOutputModel>> GetRegistrosAuditoriaDetallesAsignacion(InputDestalleAsignacion inputsDto);
        Task<List<ResponseRegistrosAuditoriaProgresoDiarioDto>> GetRegistrosAuditoriaProgresoDiario(InputsRegistrosAuditoriaFiltrosDto inputsDto);
        Task CambiarEstadoRegistroAuditoria(InputsCambiarEstadoRegistroAuditoriaDto inputsDto);
        Task CambiarEstadoRegistroAuditoriaMasivo(InputsActualizarDC_NC_ND_MotivoDto inputsDto);  //Task<List<ResponseRegistrosAuditoriaFiltrosDto>> CambiarEstadoRegistroAuditoriaMasivo(InputsActualizarDC_NC_ND_MotivoDto inputsDto)
        Task<List<RegistroAuditoriaSoporteModel>> GetSoportesEntidad(InputsSoportesEntidadDto inputsDto);
        Task ActualizarBanderasRegistrosAuditoria(InputsActualizarBanderasRegistrosAuditoriaDto inputsDto);
        Task<List<ItemModel>> GetTipificaciónByEstadoId(InputsGetTipificaciónByEstadoDto inputsDto);        
        Task<List<ResponseAlertasRegistrosAuditoriaDto>> GetAlertasRegistrosAuditoria(InputsIdUsuarioDto inputsDto);
        Task<List<ResponseGetConsultaPerfilAccionDetallesDto>> GetConsultaPerfilAccion(); //InputsGetConsultaPerfilAccionDto inputsDto

        /// <summary>
        /// Consulta usuarios asignados al lider
        /// </summary>
        /// <param name="idUsuarioLider">Id usuario lider</param>
        /// <returns>Modelo usuarios lider</returns>
        Task<List<ResponseUsuariosLider>> ConsultaUsuariosLider(string idUsuarioLider);

        /// <summary>
        /// Reversa un Registro Auditoria en estado finalizado
        /// </summary>
        /// <param name="inputsDto">Entrada de datos</param>
        /// <returns>modelo llave valor</returns>
        Task<ResponseLlaveValor> ReversarRegistrosAuditoria(InputsReversarRegistrosAuditoriaDto input);
    }
}
