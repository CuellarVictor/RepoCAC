using AuditCAC.Domain.Dto;
using AuditCAC.Domain.Entities;
using AuditCAC.MainCore.Module.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuditCAC.Services.Controllers
{
    [Route("api/RegistroAuditoriaCalificaciones")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RegistroAuditoriaCalificacionesController : ControllerBase
    {
        private readonly IRegistroAuditoriaCalificacionesRepository<RegistroAuditoriaCalificacionesModel> _IRepository;
        private readonly ILogger<RegistroAuditoriaCalificacionesModel> _logger;

        //Contructor.
        public RegistroAuditoriaCalificacionesController(IRegistroAuditoriaCalificacionesRepository<RegistroAuditoriaCalificacionesModel> IRepository, ILogger<RegistroAuditoriaCalificacionesModel> logger)
        {
            this._IRepository = IRepository;
            _logger = logger;
        }

        //Metodos.
        // POST: api/RegistroAuditoriaCalificaciones/CalificarRegistroAuditoria
        /// <summary>
        /// Califica o actualiza una calificación existente en un RegistroAuditoria.
        /// </summary>
        /// <remarks>
        /// Recibe un listado de calificaciones y las registra o actualiza.
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Configruacion ADO</returns>
        [HttpPost]
        [Route("CalificarRegistroAuditoria")]
        public async Task<IActionResult> CalificarRegistroAuditoria([FromBody] List<InputsRegistroAuditoriaCalificacionesDto> inputs) //InputsRegistroAuditoriaCalificacionesDto
        {
            try
            {
                await _IRepository.CalificarRegistroAuditoria(inputs);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        // POST: api/RegistrosAuditoria/GetCalificacionesRegistroAuditoriaByRegistrosAuditoriaId
        /// <summary>
        /// Consultar las calificaciones de un RegistroAuditoria segun su RegistrosAuditoriaId
        /// </summary>
        /// <remarks>
        /// Regresa todas las calificaciones almacenadas que correspondan al registro auditoria buscado segun su RegistrosAuditoriaId.
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Configruacion ADO</returns>        
        [HttpPost]
        [Route("GetCalificacionesRegistroAuditoriaByRegistrosAuditoriaId")]
        public async Task<IActionResult> GetCalificacionesRegistroAuditoriaByRegistrosAuditoriaId([FromBody] InputsGetRegistroAuditoriaCalificacionesByRegistrosAuditoriaIdDto inputsDto)
        {
            try
            {
                var Data = await _IRepository.GetCalificacionesRegistroAuditoriaByRegistrosAuditoriaId(inputsDto);
                return Ok(Data);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        // POST: api/RegistrosAuditoria/GetCalificacionesRegistroAuditoriaByVariableId
        /// <summary>
        /// Consultar las calificaciones de un RegistroAuditoria segun su VariableId
        /// </summary>
        /// <remarks>
        /// Regresa todas las calificaciones almacenadas que correspondan al registro auditoria buscado segun su VariableId.
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Configruacion ADO</returns>        
        [HttpPost]
        [Route("GetCalificacionesRegistroAuditoriaByVariableId")]
        public async Task<IActionResult> GetCalificacionesRegistroAuditoriaByVariableId([FromBody] InputsGetRegistroAuditoriaCalificacionesByVariableIdDto inputsDto)
        {
            try
            {
                var Data = await _IRepository.GetCalificacionesRegistroAuditoriaByVariableId(inputsDto);
                return Ok(Data);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        // POST: api/RegistrosAuditoria/GetCalificacionEsCompletas
        /// <summary>
        /// Consultar si un RegistroAuditoria tiene todas sus calificaciones
        /// </summary>
        /// <remarks>
        /// Regresa true si un RegistroAuditoria esta totalmente calificado.
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Configruacion ADO</returns>        
        [HttpPost]
        [Route("GetCalificacionEsCompletas")]
        public async Task<IActionResult> GetCalificacionEsCompletas([FromBody] InputsGetCalificacionEsCompletasDto inputsDto)
        {
            try
            {
                var Data = await _IRepository.GetCalificacionEsCompletas(inputsDto);
                return Ok(Data);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        /// <summary>
        /// Para la consultar Items calificable asociados a una variable
        /// </summary>
        /// <param name="VariableId">Id de variable</param>
        /// <returns>Modelo de datos</returns>
        [HttpGet]
        [Route("GetItemsByVariableId/{VariableId}")]
        public async Task<IActionResult> GetItemsByVariableId(int VariableId) //Task<List<IActionResult>>
        {
            try
            {
                var result = await _IRepository.GetItemsByVariableId(VariableId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }
    }
}
