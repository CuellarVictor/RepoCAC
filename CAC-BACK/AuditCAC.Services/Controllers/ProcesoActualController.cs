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
    [Route("api/procesoactual")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProcesoActualController : ControllerBase
    {
        private readonly IRepository<CurrentProcessModel> _repository;
        private readonly ILogger<CurrentProcessModel> _logger;

        public ProcesoActualController(IRepository<CurrentProcessModel> repository, ILogger<CurrentProcessModel> logger)
        {
            this._repository = repository;
            _logger = logger;
        }


        // GET: api/procesoactual
        /// <summary>
        /// Consultar procesos actuales
        /// </summary>
        /// <remarks>
        /// Retorna listado de procesos actuales
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Listado de procesos actuales</returns>
        [HttpGet]
        [Route("GetAll")]
        public async Task<IEnumerable<CurrentProcessModel>> GetAll()
        {
            try
            {
                var result = await _repository.GetAll();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }


        // GET: api/procesoactual
        /// <summary>
        /// Consultar proceso actual por id
        /// </summary>
        /// <remarks>
        /// Retorna proceso
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Proceso Actual</returns>
        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<CurrentProcessModel> GetById(int id)
        {
            try
            {
                var result = await _repository.GetById(id);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }



        // POST: api/procesoactual
        /// <summary>
        /// Insertar proceso actual
        /// </summary>
        /// <remarks>
        /// OK
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>OK</returns>
        [HttpPost]
        [Route("Insert")]
        public async Task<string> Insert([FromBody] CurrentProcessModel input)
        {
            try
            {
                var result = await _repository.Insert(input);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        // POST: api/procesoactual
        /// <summary>
        /// Actualizar proceso actual
        /// </summary>
        /// <remarks>
        /// OK
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>OK</returns>
        [HttpPost]
        [Route("Update")]
        public async Task<string> Update([FromBody] CurrentProcessModel input)
        {
            try
            {
                var result = await _repository.Update(input);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        // DELETE: api/procesoactual
        /// <summary>
        /// Actualizar proceso actual
        /// </summary>
        /// <remarks>
        /// OK
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>OK</returns>
        //[HttpDelete]
        [HttpGet]
        [Route("Delete")]
        public async Task<string> Delete(int id)
        {
            try
            {
                var result = await _repository.Delete(id);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        /// <summary>
        /// Valida  si se encuentra corriendo un proceso actual con el Id indicado
        /// </summary>
        /// <param name="id">Id Proceso actual</param>
        /// <param name="result">Campo Result</param>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Modelo Proceso Actual</returns>
        [HttpGet]
        [Route("ValidationCurrentProcess/{id}/{result}")]
        public async Task<CurrentProcessModel> ValidationCurrentProcess(int id, string result)
        {
            try
            {
                var oReturn = await _repository.ValidationCurrentProcess(id, result);
                return oReturn;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
           
        }

        /// <summary>
        /// Elimina proceso actual
        /// </summary>
        /// <param name="id">Id Proceso</param>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Modelo Proceso Actual</returns>
        [HttpGet]
        [Route("DeleteCurrentProcess/{id}/{result}")]
        public async Task<CurrentProcessModel> DeleteCurrentProcess(int id, string result)
        {
            try
            {
                var oReturn = await _repository.DeleteCurrentProcess(id, result);
                return oReturn;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }


    }
}
