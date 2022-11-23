using AuditCAC.Domain.Dto;
using AuditCAC.Domain.Dto.Calculadora;
using AuditCAC.Domain.Dto.RegistroAuditoria;
using AuditCAC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.MainCore.Module.Interface
{
    public interface IRegistrosAuditoriaDetalleRepository<TEntity>
    {
        Task<IEnumerable<RegistrosAuditoriaDetalleModel>> GetAll();
        Task<RegistrosAuditoriaDetalleModel> Get(int id);
        Task Add(RegistrosAuditoriaDetalleModel entity);        
        Task Update(RegistrosAuditoriaDetalleModel dbEntity, RegistrosAuditoriaDetalleModel entity);

        /// <summary>
        /// Actualiza calificacion registro auditoria detalle
        /// </summary>
        /// <param name="inputsDto">Datos entrada calificacion</param>
        /// <returns>Modelo validacion registro auditoria detalle</returns>
        Task<ResponseValidacionEstado> ActualizarDC_NC_ND_Motivo(InputsRegistrosAuditoriaDetalleUpdate_Actualizar_DC_NC_ND_Motivo_Dto inputsDto, int buttonAction);

        Task Delete(RegistrosAuditoriaDetalleModel entity);

        /// <summary>
        /// Consulta detalle de registro auditoria por Id
        /// </summary>
        /// <param name="Id">Id</param>
        /// <returns>Lista Modelo registro de auditoria</returns>
        Task<List<ResponseRegistroAuditoriaDetalle>> ConsultarRegistroAuditoriaDetallePorid(int Id);
        Task<List<ResponseGetUsuariosByRoleIdDto>> GetUsuariosByRoleId(InputsGetUsuariosByRoleIdDto inputsDto);
        Task<List<ResponseGetUsuariosByRoleIdDto>> GetUsuariosByRoleCoberturaId(InputsGetUsuariosByRoleCoberturaIdDto inputsDto);
        Task<ResponseGetUsuariosConcatByRoleIdDto> GetUsuariosConcatByRoleCoberturaId(InputsGetUsuariosByRoleCoberturaIdDto inputsDto); //ResponseGetUsuariosConcatByRoleIdDto

        #region Validacion Estados

        /// <summary>
        /// Consulta validaciones para registro auditoria detalle
        /// </summary>
        /// <param name="registroAuditoriaId">Id registro auditoria</param>
        /// <param name="userId">Id ausuario</param>
        /// <returns>Modelo Validacion</returns>
        Task<ResponseValidacionEstado> GetValidacionesRegistroAuditoriaDetalle(int registroAuditoriaId, string userId, int buttonAction);


        /// <summary>
        /// Consulta validaciones para registro auditoria detalle cuando el estado es registro nuevo
        /// </summary>
        /// <param name="registroAuditoriaId">Id registro auditoria</param>
        /// <returns>Modelo Validacion</returns>
        Task<ResponseValidacionEstado> ValidacionEstadoRN(int registroAuditoriaId);

        /// <summary>
        /// Consulta validaciones para registro auditoria detalle cuando el estado glosa en revision por la entidad GRE
        /// </summary>
        /// <param name="registroAuditoriaId">Id registro auditoria</param>
        /// <returns>Modelo Validacion</returns>
        Task<ResponseValidacionEstado> ValidacionEstadoGRE(int registroAuditoriaId);

        /// <summary>
        /// Consulta validaciones para registro auditoria detalle cuando el estado es glolosa objetada 1 GO1
        /// </summary>
        /// <param name="registroAuditoriaId">Id registro auditoria</param>
        /// <returns>Modelo Validacion</returns>
        Task<ResponseValidacionEstado> ValidacionEstadoGO1(int registroAuditoriaId, int buttonAction);


        #endregion


        /// <summary>
        /// Actualiza estado registro auditoria y registra seguimiento
        /// </summary>
        /// <param name="registroAuditoriaId">Id registro auditoria</param>
        /// <param name="userId">Id Usuario</param>
        /// <returns>Modelo validacion</returns>
        Task<ResponseValidacionEstado> ActualizaEstadoRegistroAuditoria(int registroAuditoriaId, string userId, int buttonAction);

        /// <summary>
        /// Acutaliza registro auditoria detalle, de forma masiva
        /// </summary>
        /// <param name="idRegistroAuditoria">Id registro auditoria</param>
        /// <param name="userId">Id usuario</param>
        /// <param name="buttonAction">Acción</param>
        /// <param name="input">Modelo Variables entrada</param>
        /// <returns>true</returns>
        Task<bool> ActualizarRegistroAuditoriaDetalleMultiple(int idRegistroAuditoria, string userId, int buttonAction, List<ResponseRegistroAuditoriaDetalle> input);

        /// <summary>
        /// Consulta errores de Logica
        /// </summary>
        /// <param name="userId">Id usuario</param>
        /// <param name="input">Modelo detalle registro auditoria</param>
        /// <returns>Modelo detalle registro auditoria</returns>
        Task<List<ResponseRegistroAuditoriaDetalle>> ValidarErrores(string userId, int registroAuditoriaId, List<ResponseRegistroAuditoriaDetalle> input);

        /// <summary>
        /// Consulta erorres por id de registro auditoria
        /// </summary>
        /// <param name="registroAuditoriaId">id de registro auditoria</param>
        /// <returns>Modelo Errores registro auditoria</returns>
        Task<List<RegistroAuditoriaDetalleErrorModel>> ConsultarErroresRegistrosAuditoria(int registroAuditoriaId);


        /// <summary>
        /// Crea o Acutaliza registro auditoria error
        /// </summary>
        /// <param name="inputsDto">Modelo Error registro auditoria</param>
        /// <returns>Modelo Error registro auditoria</returns>
        bool UpsertErroresRegistrosAuditoria(RegistroAuditoriaDetalleErrorModel inputsDto);

        /// <summary>
        /// Crea o Acutaliza registro auditoria error Masivo
        /// </summary>
        /// <param name="inputsDto">Modelo Error registro auditoria</param>
        /// <returns>Modelo Error registro auditoria</returns> 
        Task<bool> UpsertErroresRegistrosAuditoriaMasivo(List<InputErroresRegistrosAuditoriaDto> inputsDto, string userId);

        ///// <summary>
        ///// Para Actualizar estados e insertar estados de la entidad.
        ///// </summary>
        ///// <param name="inputsDto">Modelo de datos</param>
        ///// <returns>Modelo de respuesta. Indica si finalizo con exito o error</returns>
        //Task<string> CambiarEstadoEntidad(InputsCambiarEstadoEntidadDto inputsDto);

        ///// <summary>
        ///// Para Actualizar estados e insertar estados de la entidad.
        ///// </summary>
        ///// <param name="inputsDto">Modelo de datos</param>
        ///// <returns>Modelo de respuesta.</returns>
        //Task<List<ResponseConsultarEstadosEntidadDto>> ConsultarEstadosEntidad(InputsConsultarEstadosEntidadDto inputsDto);

        #region  Calculadora

        /// <summary>
        /// Calculadora TFG
        /// </summary>
        /// <param name="input">Input Calculadora</param>
        /// <returns>Valor calculado</returns>
        Task<ResponseLlaveValor> CalculadoraTFG(InputCalculadoraTFG input);


        /// <summary>
        /// Calculadora KRU
        /// </summary>
        /// <param name="input">Input Calculadora</param>
        /// <returns>Valor calculado</returns>
        Task<ResponseLlaveValor> CalculadoraKRU(InputCalculadoraKRU input);

        /// <summary>
        /// Calculadora promedio
        /// </summary>
        /// <param name="input">Listado valores</param>
        /// <returns>Promedio</returns>
        Task<ResponseLlaveValor> CalculadoraPromedio(List<decimal> input);

        #endregion

        /// <summary>
        /// Consulta data necesaria de tablas referenciales para el registro a auditar
        /// </summary>
        /// <param name="registroAuditoriaId"></param>
        /// <returns></returns>
        Task<List<DataTablaReferencial_RegistroAuditoriaModel>> ConsultaDataTablasReferencialRegistroAuditoria(int registroAuditoriaId);
    }
}
