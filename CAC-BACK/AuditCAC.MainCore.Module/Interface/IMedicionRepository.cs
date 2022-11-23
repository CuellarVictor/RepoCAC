using AuditCAC.Domain.Dto;
using AuditCAC.Domain.Dto.Actas;
using AuditCAC.Domain.Dto.CalificacionMasiva;
using AuditCAC.Domain.Dto.CarguePoblacion;
using AuditCAC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuditCAC.MainCore.Module.Interface
{
    public interface IMedicionRepository<TEntity>
    {
        Task<IEnumerable<MedicionModel>> GetAll();
        Task<MedicionModel> Get(int id);
        Task<int> Add(MedicionModel entity);
        Task Update(MedicionModel dbEntity, MedicionModel entity);
        Task Delete(MedicionModel entity);
        Task<List<MedicionModel>> GetMedicionAll(InputsMedicionAllDto MedicionAll);
        Task<Tuple<FiltrosBolsaMedicionDto, int, int, int>> GetFiltrosBolsaMedicion(InputGetFiltrosBolsaMedicionDto filtrosBolsaMedicionDto);  //Task<Tuple<List<FiltrosBolsaMedicionDto>, int, int, int>> GetFiltrosBolsaMedicion(InputFiltrosBolsaMedicionDto filtrosBolsaMedicionDto);
                                                                                                                                               //Task<FiltrosBolsaMedicionDto> GetFiltrosBolsaMedicion(InputFiltrosBolsaMedicionDto filtrosBolsaMedicionDto);

        #region Cargue Poblacion

        /// <summary>
        /// Inicia proceso para cargue de poblacion registrandolo en la tabla current process
        /// </summary>
        /// <param name="filePath">Ruta del archivo a cargar</param>
        /// <param name="input">Modelo Cargue</param>
        /// <returns>Modelo Current Process</returns>
        Task<CurrentProcessModel> IniciaProcesoCarguePoblacion(string filePath, InputCarguePoblacion input, S3Model s3);

        /// <summary>
        /// Genera plantilla para cargue de poblacion
        /// </summary>
        /// <param name="idMedicion">Id medicion</param>
        /// <param name="path">Ruta fisica</param>
        /// <returns>Archivo base 64</returns>
        Task<string> GenerarTemplateCarguePoblacion(int idMedicion, string path, int idSubgrupo);

        /// <summary>
        /// Retorna listado de Estructuras para Carge Poblacion
        /// </summary>
        /// <param name="inputsDto">Modelo de datos</param>
        /// <returns>Listado de Estructuras para Carge Poblacion</returns>
        Task<List<ResponseConsultaEstructurasCargePoblacionDto>> GetConsultaEstructurasCargePoblacion(InputsBolsasMedicionIdDto inputsDto);
        #endregion


        // --
        //Task<ResponseValidacionEstadoBolsasMedicionDto> GetValidacionEstadoBolsasMedicion(InputsBolsasMedicionIdDto inputsDto);
        Task<List<ResponseValidacionEstadoBolsasMedicionDto>> GetValidacionEstadoBolsasMedicion(InputsBolsasMedicionIdDto inputsDto);
        // --

        Task<List<ResponseUsuariosBolsaMedicionFiltroDto>> GetUsuariosBolsaMedicionFiltro(InputsBolsasMedicionIdDto inputsDto);
        Task<Tuple<List<ResponseUsuariosBolsaMedicionDto>, int, int, int>> GetUsuariosBolsaMedicion(InputsBolsasMedicionDto inputsDto);

        /// <summary>
        /// Carga los filtros usados en vista de Reasignaciones de bolsa
        /// </summary>
        /// <param name="inputsDto">Modelo Input de datos</param>
        /// <returns>Filtros segun Auditor y Medicion</returns>
        Task<List<ResponseRegistrosAuditoriaXBolsaMedicionFiltros>> GetRegistrosAuditoriaXBolsaMedicionFiltros(InputsRegistrosAuditoriaXBolsaMedicionFiltrosDto inputsDto);

        /// <summary>
        /// Carga listado de Registros Auditoria en vista de Reasignaciones de bolsa.
        /// </summary>
        /// <param name="inputsDto">Filtros para la consulta</param>
        /// <returns>Listado de RegistrosAuditoria segun filtros recibidos</returns>
        Task<Tuple<List<ResponseRegistrosAuditoriaXBolsaMedicionDto>, int, int, int>> GetRegistrosAuditoriaXBolsaMedicion(InputsGetRegistrosAuditoriaXBolsaMedicionDto inputsDto);

        /// <summary>
        /// Reasigna RegistrosAuditoria a otros Auditories. Esto es realizado de forma automatica (Equitativa).
        /// </summary>
        /// <param name="inputsDto">Modelo de datos</param>
        /// <returns></returns>
        Task<bool> ReasignacionesBolsaEquitativa(InputReasignacionesBolsaEquitativaDto inputsDto);

        /// <summary>
        /// Para cargar listado de Registros Auditoria en vista de Reasignaciones de bolsa detallada.
        /// </summary>
        /// <param name="filePath">Ruta del archivo a cargar</param>
        /// <param name="input">Modelo Cargue</param>
        /// <returns>Modelo Current Process</returns>        
        Task<ResponseReasignacionesBolsaDetalladaDto> ReasignacionesBolsaDetallada(string filePath, InputReasignacionesBolsaDetalladaDto input, S3Model _s3);

        /// <summary>
        /// Para cargar listado de Bolsas de una enfermedad madre.
        /// </summary>
        /// <param name="inputsDto">Modelo de datos</param>
        /// <returns>Listado de Bolsas segun enfermedad madre</returns>    
        Task<List<ResponseBolsasMedicionXEnfermedadMadreDto>> GetBolsasMedicionXEnfermedadMadre(InputsBolsasMedicionXEnfermedadMadreDto inputsDto);

        /// <summary>
        /// Para la consulta de usuarios segun su Rol.
        /// </summary>
        /// <param name="inputsDto">Modelo Cargue</param>
        /// <returns>Modelo Current Process</returns>
        Task<List<ResponseUsuariosBolsaMedicionFiltroDto>> GetUsuariosByRol(InputsGetUsuariosByRoleIdDto inputsDto);

        /// <summary>
        /// Para Mover todos los registrosAuditorias de una Bolsa a otra.
        /// </summary>
        /// <param name="filePath">Ruta del archivo a cargar</param>
        /// <param name="input">Modelo Cargue</param>
        /// <returns>Modelo Current Process</returns>        
        Task<ResponseReasignacionesBolsaDetalladaDto> MoverTodosRegistrosAuditoriaBolsaMedicion(string filePath, InputMoverTodosRegistrosAuditoriaBolsaMedicionDto input);

        /// <summary>
        /// Para consultar info usada en Mover algunos registrosAuditorias de una Bolsa a otra.
        /// </summary>
        /// <param name="filePath">Ruta del archivo a cargar</param>
        /// <param name="input">Modelo Cargue</param>
        /// <returns>Modelo Current Process</returns>        
        Task<ResponseReasignacionesBolsaDetalladaDto> MoverAlgunosRegistrosAuditoriaBolsaMedicionPlantilla(string filePath, InputMoverTodosRegistrosAuditoriaBolsaMedicionDto input);

        /// <summary>
        /// Para Alugnos registrosAuditorias de una Bolsa a otra. Usando un archivo plantilla.
        /// </summary>
        /// <param name="filePath">Ruta del archivo a cargar</param>
        /// <param name="input">Modelo Cargue</param>
        /// <returns>Modelo Current Process</returns>        
        Task<ResponseReasignacionesBolsaDetalladaDto> MoverAlgunosRegistrosAuditoriaBolsaMedicion(string filePath, InputMoverAlgunosRegistrosAuditoriaBolsaMedicionDto input, S3Model _s3);


        #region Perfil Admin: Eliminar registros de una medición o bolsa durante el proceso de auditoría
        /// <summary>
        /// Para consultar plantilla de eliminacion de registros, es decir poblacion de una medicion (RegistrosAuditoria).
        /// </summary>
        /// <param name="filePath">Ruta del archivo a cargar</param>
        /// <param name="input">Modelo Cargue</param>
        /// <returns>Modelo Current Process</returns>        
        Task<ResponseReasignacionesBolsaDetalladaDto> EliminarRegistrosAuditoriaPlantilla(string filePath); //, InputEliminarRegistrosAuditoriaPlantillaDto input

        /// <summary>
        /// Para Eliminar registrosAuditorias de una Bolsa.
        /// </summary>
        /// <param name="filePath">Ruta del archivo a cargar</param>
        /// <param name="input">Modelo Cargue</param>
        /// <returns>Modelo Current Process</returns>        
        Task<ResponseReasignacionesBolsaDetalladaDto> EliminarRegistrosAuditoria(string filePath, InputEliminarRegistrosAuditoriaDto input, S3Model _s3);
        #endregion

        #region Calificacion Masiva

        /// <summary>
        /// Crea Template Calificacion Masiva
        /// </summary>
        /// <param name="path">ruta fisica</param>
        /// <returns>string template base 64</returns>
        Task<string> CreaPlantillaCalificacionMasiva(string path, int medicionId);


        // <summary>
        /// Inicia proceso para cargue calificacion masiva registrandolo en la tabla current process
        /// </summary>
        /// <param name="filePath">Ruta del archivo a cargar</param>
        /// <param name="input">Modelo Cargue</param>
        /// <returns>Modelo Current Process</returns>
        Task<CurrentProcessModel> IniciaProcesoCalificacionMasiva(string filePath, InputCalificacionMasiva input, S3Model s3);

        #endregion

        Task<IEnumerable<ConsultaEPSItemDto>> ObtenerListadoDeEPS(int idCobertura);
    }
}
