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
    [Route("api/ParametrosGeneral")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ParametrosGeneralController : ControllerBase
    {
        private readonly IRepository<ParametroGeneralModel> _repository;
        private readonly ILogger<ParametroGeneralModel> _logger;

        public ParametrosGeneralController(IRepository<ParametroGeneralModel> repository, ILogger<ParametroGeneralModel> logger)
        {
            this._repository = repository;
            _logger = logger;
        }


        // GET: api/ParametrosGeneral
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
        public async Task<IEnumerable<ParametroGeneralModel>> GetAll()
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


        // GET: api/ParametrosGeneral
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
        public async Task<ParametroGeneralModel> GetById(int id)
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



        // POST: api/ParametrosGeneral
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
        public async Task<string> Insert([FromBody] ParametroGeneralModel input)
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

        // POST: api/ParametrosGeneral
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
        public async Task<string> Update([FromBody] ParametroGeneralModel input)
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

        // DELETE: api/ParametrosGeneral
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


    }
}
