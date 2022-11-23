using AuditCAC.Domain.Dto;
using AuditCAC.Domain.Dto.PanelEnfermedad;
using AuditCAC.Domain.Entities;
using AuditCAC.Domain.Entities.Usuarios;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.MainCore.Module.Users.Interfaces
{
    public interface IUserManagement
    {
        /// <summary>
        /// Consulta Listado de usuarios
        /// </summary>
        /// <returns>Modelo Usuarios</returns>
        Task<List<UsuarioResponse>> GetUsers();

        /// <summary>
        /// Crea o actualiza usuario
        /// </summary>
        /// <param name="input">modelo usaurio</param>
        /// <returns>modelo llave valor</returns>
        Task<ResponseLlaveValor> Upsertuser(UsuarioResponse input);

        Task<List<EnfermedadModel>> GetEnfermedades();
        Task<List<RolesUsuarioModel>> GetRoles(); 
        Task<List<PanelEnfermedadesMadre>> GetPanelEnfermedadesMadre();
        Task<List<PanelEnfermedadesDto>> GetPanel();

        /// <summary>
        /// Para consultar todos los Logs de Procesos segun los filtrados ingresados.
        /// </summary>
        /// <param name="inputsDto">Entrada de datos</param>
        /// <returns>Modelo de datos</returns>
        Task<Tuple<List<ResponseProcessLogFiltradoDto>, int?, int?>> GetProcessLogFiltrado(InputsProcessLogFiltradoDto inputsDto);

        /// <summary>
        /// Para consultar Asignacion de lider por entidad
        /// </summary>
        /// <param name="inputsDto">Entrada de datos</param>
        /// <returns>Modelo de datos</returns>
        Task<Tuple<List<ResponseConsultaAsignacionLiderEntidadDto>, int, int>> GetConsultaAsignacionLiderEntidad(InputsConsultaAsignacionLiderEntidadDto inputsDto);

        /// <summary>
        ///	Consulta auditores por Cobertura, EPS, Idperiodo, para asignar el lider
        /// </summary>
        /// <param name="inputsDto">Entrada de datos</param>
        /// <returns>Modelo de datos</returns>
        Task<List<ResponseConsultaEPSCoberturaPeriodoDto>> ConsultaAuditoresAsignacionLiderEntidad(InputsResponseConsultaEPSCoberturaPeriodoDto inputsDto);


        /// <summary>
        /// Consulta periodos con data en cronograma por Id de cobertura
        /// </summary>
        /// <param name="idCobertura">Id Cobertura</param>
        /// <returns>Lista llave valor (idPeriodo)</returns>
        Task<List<ResponseLlaveValor>> ConsultaPeriodosCobertura(int idCobertura);

        /// <summary>
        /// Crea o Actualiza lideres de una EPS
        /// </summary>
        /// <param name="inputsDto">Entrada de datos</param>
        /// <returns>modelo llave valor</returns>
        Task<ResponseLlaveValor> UpsertLiderEPS(InputsUpsertLiderEPSDto input);
    }
}
