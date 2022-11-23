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
    [Route("api/procesoactualparametro")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProcesoActualParametroController : ControllerBase
    {
        private readonly IRepository<CurrentProcessParamModel> _repository;
        private readonly ILogger<CurrentProcessParamModel> _logger;

        public ProcesoActualParametroController(IRepository<CurrentProcessParamModel> repository, ILogger<CurrentProcessParamModel> logger)
        {
            this._repository = repository;
            _logger = logger;
        }


        // GET: api/procesoactualparametro
        /// <summary>
        /// Consultar parametros proceso actual
        /// </summary>
        /// <remarks>
        /// Retorna listado de paramaetros
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Listado de parametros</returns>
        [HttpGet]
        [Route("GetAll")]
        public async Task<IEnumerable<CurrentProcessParamModel>> GetAll()
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


        // GET: api/procesoactualparametro
        /// <summary>
        /// Consultar parametrol por id
        /// </summary>
        /// <remarks>
        /// Retorna parametro
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Proceso Actual</returns>
        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<CurrentProcessParamModel> GetById(int id)
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



        // POST: api/procesoactualparametro
        /// <summary>
        /// Insertar parametro
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
        public async Task<string> Insert([FromBody] CurrentProcessParamModel input)
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

        // POST: api/procesoactualparametro
        /// <summary>
        /// Actualizar parametro
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
        public async Task<string> Update([FromBody] CurrentProcessParamModel input)
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

        // DELETE: api/procesoactualparametro
        /// <summary>
        /// Actualizar parametro
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
