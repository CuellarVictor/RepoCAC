using AuditCAC.Domain.Dto;
using AuditCAC.Domain.Dto.Calculadora;
using AuditCAC.Domain.Dto.RegistroAuditoria;
using AuditCAC.Domain.Entities;
using AuditCAC.MainCore.Module.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuditCAC.Services.Controllers
{
    [Route("api/RegistrosAuditoriaDetalle")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RegistrosAuditoriaDetalleController : ControllerBase
    {
        private readonly IRegistrosAuditoriaDetalleRepository<RegistrosAuditoriaDetalleModel> _IRepository;
        private readonly ILogger<RegistrosAuditoriaDetalleModel> _logger;

        public RegistrosAuditoriaDetalleController(IRegistrosAuditoriaDetalleRepository<RegistrosAuditoriaDetalleModel> IRepository, ILogger<RegistrosAuditoriaDetalleModel> logger)
        {
            this._IRepository = IRepository;
            _logger = logger;
        }

        // GET: api/RegistrosAuditoriaDetalle
        /// <summary>
        /// Consultar todos los registros disponibles.
        /// </summary>
        /// <remarks>
        /// Regresa todos los registros almacenados.
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Configruacion ADO</returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            //Obtenemos todos los registros.
            var Data = await _IRepository.GetAll();
            //IEnumerable<RegistrosAuditoriaDetalleModel> Data = await _IRepository.GetAll();

            //Retornamos datos.
            return Ok(Data);
        }

        // GET: api/RegistrosAuditoriaDetalle/5
        /// <summary>
        /// Consultar un registro con su Id.
        /// </summary>
        /// <remarks>
        /// Regresa un solo registro, buscado por su Id.
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Configruacion ADO</returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            //Obtenemos registro buscado
            var Data = await _IRepository.Get(id);

            //Validamos si el registro existe.
            if (Data == null)
            {
                return NotFound("The record couldn't be found.");
            }

            //Retornamos datos.
            return Ok(Data);
        }

        // POST: api/RegistrosAuditoriaDetalle
        /// <summary>
        /// Crear un registro nuevo.
        /// </summary>
        /// <remarks>
        /// Permite crear un nuevo registro y regresa ruta para consultar el registro creado.
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Configruacion ADO</returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] RegistrosAuditoriaDetalleModel entity)
        {
            //Validamos si recibimos una entidad.
            if (entity == null)
            {
                return BadRequest("The record is null.");
            }

            //Registramos.
            await _IRepository.Add(entity);

            //Retornamos ruta de registro creado.
            return CreatedAtRoute("Get", new { Id = entity.Id }, entity);
        }

        // PUT: api/RegistrosAuditoriaDetalle/5
        /// <summary>
        /// Actualiza un registro con su Id.
        /// </summary>
        /// <remarks>
        /// Actualiza un registro buscado por su Id.
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Configruacion ADO</returns>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] RegistrosAuditoriaDetalleModel entity)
        {
            //Validamos si recibimos una entidad.
            if (entity == null)
            {
                return BadRequest("The record is null.");
            }

            //Buscamos y validamos que exista el registro buscado.
            RegistrosAuditoriaDetalleModel ToUpdate = await _IRepository.Get(id);
            if (ToUpdate == null)
            {
                return NotFound("The record couldn't be found.");
            }

            //Actualizamos el registro.
            await _IRepository.Update(ToUpdate, entity);

            //Retornamos sin contenido.
            return NoContent();
        }

        // PUT: api/ActualizarDC_NC_ND_Motivo/5
        /// <summary>
        /// Actualiza los campos MotivoVariable y Dato_DC_NC_ND de un registro con su Id.
        /// </summary>
        /// <remarks>
        /// Actualiza los campos MotivoVariable y Dato_DC_NC_ND de un registro buscado por su Id.
        /// 1. DC
        /// 2. NC
        /// 3. ND
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Configruacion ADO</returns>
        [HttpPost]
        [Route("ActualizarDC_NC_ND_Motivo/{buttonAction}")]
        public async Task<IActionResult> ActualizarDC_NC_ND_Motivo([FromBody] InputsRegistrosAuditoriaDetalleUpdate_Actualizar_DC_NC_ND_Motivo_Dto inputs, int buttonAction)
        {
            try
            {
                var result = await _IRepository.ActualizarDC_NC_ND_Motivo(inputs, buttonAction);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        // DELETE: api/RegistrosAuditoriaDetalle/5
        /// <summary>
        /// Eliminar un registro con su Id.
        /// </summary>
        /// <remarks>
        /// Elimina un registro buscado por su Id.
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Configruacion ADO</returns>
        //[HttpDelete("{id:int}")]
        [HttpGet]
        [Route("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            //Obtenemos registro buscado
            var Data = await _IRepository.Get(id);

            //Validamos si el registro existe.
            if (Data == null)
            {
                return NotFound("The record couldn't be found.");
            }

            //Eliminamos el registro.
            await _IRepository.Delete(Data);

            //Retornamos sin contenido.
            return NoContent();
        }

        // POST: api/RegistrosAuditoriaDetalle/GetById/Id
        /// <summary>
        /// Consultar un RegistroAuditoriaDetalle Por Id.
        /// </summary>
        /// <remarks>
        /// Consultar un RegistroAuditoriaDetalle Por Id.
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Configruacion ADO</returns>        
        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<List<ResponseRegistroAuditoriaDetalle>> ConsultarRegistroAuditoriaDetallePorid(int id)
        {
            try
            {
                var result = await _IRepository.ConsultarRegistroAuditoriaDetallePorid(id);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        // POST: api/RegistrosAuditoriaDetalle/GetGetUsuariosByRoleId
        /// <summary>
        /// Consultar un RegistroAuditoriaDetalle Por Id.
        /// </summary>
        /// <remarks>
        /// Consultar un RegistroAuditoriaDetalle Por Id.
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Configruacion ADO</returns>        
        [HttpPost]
        [Route("GetUsuariosByRoleId")]
        public async Task<List<ResponseGetUsuariosByRoleIdDto>> GetUsuariosByRoleId(InputsGetUsuariosByRoleIdDto inputsDto)
        {
            try
            {
                var result = await _IRepository.GetUsuariosByRoleId(inputsDto);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        // POST: api/RegistrosAuditoriaDetalle/GetUsuariosByRoleCoberturaId
        /// <summary>
        /// Consultar listado de usuarios por RoleId, CoberturaId, UsuarioId.
        /// </summary>
        /// <remarks>
        /// Consultar listado de usuarios por RoleId, CoberturaId, UsuarioId. Retorna un listado de usuarios.
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Configruacion ADO</returns>        
        [HttpPost]
        [Route("GetUsuariosByRoleCoberturaId")]
        public async Task<List<ResponseGetUsuariosByRoleIdDto>> GetUsuariosByRoleCoberturaId(InputsGetUsuariosByRoleCoberturaIdDto inputsDto)
        {
            try
            {
                var result = await _IRepository.GetUsuariosByRoleCoberturaId(inputsDto);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        // POST: api/RegistrosAuditoriaDetalle/GetUsuariosConcatByRoleCoberturaId
        /// <summary>
        /// Consultar listado de usuarios por RoleId, CoberturaId, UsuarioId.
        /// </summary>
        /// <remarks>
        /// Consultar listado de usuarios por RoleId, CoberturaId, UsuarioId. Retorna usuarios separados por "-".
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Configruacion ADO</returns>        
        [HttpPost]
        [Route("GetUsuariosConcatByRoleCoberturaId")]
        public async Task<ResponseGetUsuariosConcatByRoleIdDto> GetUsuariosConcatByRoleCoberturaId(InputsGetUsuariosByRoleCoberturaIdDto inputsDto)
        {
            try
            {
                var result = await _IRepository.GetUsuariosConcatByRoleCoberturaId(inputsDto);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        // POST: api/RegistrosAuditoriaDetalle/GetValidacionesRegistroAuditoriaDetalle/registroAuditoriaId/userId
        /// <summary>
        /// Consulta validaciones para registro auditoria detalle
        /// </summary>
        /// <param name="registroAuditoriaId">Id registro auditoria</param>
        /// <param name="userId">Id ausuario</param>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Configruacion ADO</returns>        
        [HttpGet]
        [Route("GetValidacionesRegistroAuditoriaDetalle/{registroAuditoriaId}/{userId}/{buttonAction}")]
        public async Task<ResponseValidacionEstado> GetValidacionesRegistroAuditoriaDetalle(int registroAuditoriaId, string userId, int buttonAction)
        {
            try
            {
                var result = await _IRepository.GetValidacionesRegistroAuditoriaDetalle(registroAuditoriaId, userId, buttonAction);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }


        // GET: api/RegistrosAuditoriaDetalle/ActualizaEstadoRegistroAuditoria/registroAuditoriaId/userId
        /// <summary>
        /// Actualiza estado registro auditoria y registra seguimiento
        /// </summary>
        /// <param name="registroAuditoriaId">Id registro auditoria</param>
        /// <param name="userId">Id Usuario</param>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Modelo validacion</returns>     
        [HttpGet]
        [Route("ActualizaEstadoRegistroAuditoria/{registroAuditoriaId}/{userId}/{buttonAction}")]
        public async Task<ResponseValidacionEstado> ActualizaEstadoRegistroAuditoria(int registroAuditoriaId, string userId, int buttonAction)
        {
            try
            {
                var result = await _IRepository.ActualizaEstadoRegistroAuditoria(registroAuditoriaId, userId, buttonAction);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }


        /// <summary>
        /// Acutaliza registro auditoria detalle, de forma masiva
        /// </summary>
        /// <param name="idRegistroAuditoria">Id registro auditoria</param>
        /// <param name="userId">Id usuario</param>
        /// <param name="buttonAction">Acción</param>
        /// <param name="input">Modelo Variables entrada</param>
        /// <returns>true</returns>     
        [HttpPost]
        [Route("ActualizarRegistroAuditoriaDetalleMultiple/{registroAuditoriaId}/{userId}/{buttonAction}")]
        public async Task<bool> ActualizarRegistroAuditoriaDetalleMultiple(int registroAuditoriaId, string userId, int buttonAction, List<ResponseRegistroAuditoriaDetalle> input)
        {
            try
            {
                var result = await _IRepository.ActualizarRegistroAuditoriaDetalleMultiple(registroAuditoriaId, userId, buttonAction, input);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }


        /// <summary>
        /// Consulta errores de Logica
        /// </summary>
        /// <param name="userId">Id usuario</param>
        /// <param name="input">Modelo detalle registro auditoria</param>
        /// <returns>Modelo detalle registro auditoria</returns>   
        [HttpPost]
        [Route("ValidarErrores/{userId}/{registroAuditoriaId}")]
        public async Task<List<ResponseRegistroAuditoriaDetalle>> ValidarErrores(string userId, int registroAuditoriaId, List<ResponseRegistroAuditoriaDetalle> input)
        {
            try
            {
                var result = await _IRepository.ValidarErrores(userId, registroAuditoriaId, input);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        /// <summary>
        /// Consulta erorres por id de registro auditoria
        /// </summary>
        /// <param name="registroAuditoriaId">id de registro auditoria</param>
        /// <returns>Modelo Errores registro auditoria</returns>
        [HttpGet]
        [Route("ConsultarErroresRegistrosAuditoria/{registroAuditoriaId}")]
        public async Task<List<RegistroAuditoriaDetalleErrorModel>> ConsultarErroresRegistrosAuditoria(int registroAuditoriaId)
        {
            try
            {
                var result = await _IRepository.ConsultarErroresRegistrosAuditoria(registroAuditoriaId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        /// <summary>
        /// Crea o Acutaliza registro auditoria error
        /// </summary>
        /// <param name="inputsDto">Modelo Error registro auditoria</param>
        /// <returns>Modelo Error registro auditoria</returns> 
        [HttpPost]
        [Route("UpsertErroresRegistrosAuditoria")]
        public bool UpsertErroresRegistrosAuditoria(RegistroAuditoriaDetalleErrorModel inputsDto)
        {
            try
            {
                var result = _IRepository.UpsertErroresRegistrosAuditoria(inputsDto);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        /// <summary>
        /// Crea o Acutaliza registro auditoria error Masivo
        /// </summary>
        /// <param name="inputsDto">Modelo Error registro auditoria</param>
        /// <param name="userId">Modelo Error registro auditoria</param>
        /// <returns>Modelo Error registro auditoria</returns> 
        [HttpPost]
        [Route("UpsertErroresRegistrosAuditoriaMasivo")]
        public async Task<bool> UpsertErroresRegistrosAuditoriaMasivo(List<InputErroresRegistrosAuditoriaDto> inputsDto, string userId)
        {
            try
            {
                var result = await _IRepository.UpsertErroresRegistrosAuditoriaMasivo(inputsDto, userId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        ///// <summary>
        ///// Para Actualizar estados e insertar estados de la entidad.
        ///// </summary>
        ///// <param name="inputsDto">Modelo de datos</param>
        ///// <returns>Modelo de respuesta. Indica si finalizo con exito o error</returns>
        //[HttpPost]
        //[Route("CambiarEstadoEntidad")]
        //public async Task<string> CambiarEstadoEntidad(InputsCambiarEstadoEntidadDto inputsDto)
        //{
        //    try
        //    {
        //        var result = await _IRepository.CambiarEstadoEntidad(inputsDto);
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogInformation(ex.ToString());
        //        throw new Exception("Error", ex);
        //    }
        //}

        ///// <summary>
        ///// Para Actualizar estados e insertar estados de la entidad.
        ///// </summary>
        ///// <param name="inputsDto">Modelo de datos</param>
        ///// <returns>Modelo de respuesta.</returns>
        //[HttpPost]
        //[Route("ConsultarEstadosEntidad")]
        //public async Task<List<ResponseConsultarEstadosEntidadDto>> ConsultarEstadosEntidad(InputsConsultarEstadosEntidadDto inputsDto)
        //{
        //    try
        //    {
        //        var result = await _IRepository.ConsultarEstadosEntidad(inputsDto);
        //        return result;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogInformation(ex.ToString());
        //        throw new Exception("Error", ex);
        //    }
        //}


        /// <summary>
        /// Calculadora TFG
        /// </summary>
        /// <param name="input">Input Calculadora</param>
        /// <returns>Valor calculado</returns>
        [HttpPost]
        [Route("CalculadoraTFG")]
        public async Task<ResponseLlaveValor> CalculadoraTFG(InputCalculadoraTFG input)
        {
            try
            {
                var result = await _IRepository.CalculadoraTFG(input);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        /// <summary>
        /// Calculadora KRU
        /// </summary>
        /// <param name="input">Input Calculadora</param>
        /// <returns>Valor calculado</returns>
        [HttpPost]
        [Route("CalculadoraKRU")]
        public async Task<ResponseLlaveValor> CalculadoraKRU(InputCalculadoraKRU input)
        {
            try
            {
                var result = await _IRepository.CalculadoraKRU(input);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        /// <summary>
        /// Calculadora promedio
        /// </summary>
        /// <param name="input">Listado valores</param>
        /// <returns>Promedio</returns>
        [HttpPost]
        [Route("CalculadoraPromedio")]
        public async Task<ResponseLlaveValor> CalculadoraPromedio(List<decimal> input)
        {
            try
            {
                var result = await _IRepository.CalculadoraPromedio(input);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        /// <summary>
        /// Consulta data necesaria de tablas referenciales para el registro a auditar
        /// </summary>
        /// <param name="registroAuditoriaId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ConsultaDataTablasReferencialRegistroAuditoria/{registroAuditoriaId}")]
        public async Task<List<DataTablaReferencial_RegistroAuditoriaModel>> ConsultaDataTablasReferencialRegistroAuditoria(int registroAuditoriaId)
        {
            try
            {
                var result = await _IRepository.ConsultaDataTablasReferencialRegistroAuditoria(registroAuditoriaId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }
    }
}
