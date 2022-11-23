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
    [Route("api/RegistrosAuditoriaDetalleSeguimiento")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RegistrosAuditoriaDetalleSeguimientoController : ControllerBase
    {
        //Constructor Generico.
        //private readonly IRepository<RegistrosAuditoriaDetalleSeguimientoModel> _repository;
        //private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //public RegistrosAuditoriaDetalleSeguimientoController(IRepository<RegistrosAuditoriaDetalleSeguimientoModel> repository)
        //{
        //    this._repository = repository;
        //}

        //Constructor.
        private readonly IRegistrosAuditoriaDetalleSeguimientoRepository<RegistrosAuditoriaDetalleSeguimientoModel> _repository;
        private readonly ILogger<RegistrosAuditoriaDetalleSeguimientoModel> _logger;

        public RegistrosAuditoriaDetalleSeguimientoController(IRegistrosAuditoriaDetalleSeguimientoRepository<RegistrosAuditoriaDetalleSeguimientoModel> repository, ILogger<RegistrosAuditoriaDetalleSeguimientoModel> logger)
        {
            this._repository = repository;
            _logger = logger;
        }


        //Metodos

        // GET: api/RegistrosAuditoriaDetalleSeguimiento
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
            try
            {
                //Obtenemos todos los registros.
                var Data = await _repository.GetAll();
                //IEnumerable<RegistrosAuditoriaDetalleSeguimientoModel> Data = await _IRepository.GetAll();

                //Retornamos datos.
                return Ok(Data);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        // GET: api/RegistrosAuditoriaDetalleSeguimiento/5
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
            try
            {
                //Obtenemos registro buscado
                //var Data = await _repository.GetById(id);
                var Data = await _repository.Get(id);

                //Validamos si el registro existe.
                if (Data == null)
                {
                    return NotFound("The record couldn't be found.");
                }

                //Retornamos datos.
                return Ok(Data);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        // GET: api/RegistrosAuditoriaDetalleSeguimiento
        /// <summary>
        /// Consultar un registro con su RegistroAuditoriaId.
        /// </summary>
        /// <remarks>
        /// Regresa un solo registro, buscado por su RegistroAuditoriaId.
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Configruacion ADO</returns>
        [HttpPost]
        [Route("GetObservacionesByRegistroAuditoriaId")]
        public async Task<IActionResult> GetObservacionesByRegistroAuditoriaId([FromBody] InputsRegistrosAuditoriaDetalleSeguimientoDto inputsDto)
        {
            try
            {
                //Obtenemos registro buscado
                var Data = await _repository.GetObservacionesByRegistroAuditoriaId(inputsDto);

                //Validamos si el registro existe.
                if (Data == null)
                {
                    return NotFound("The record couldn't be found.");
                }

                //Retornamos datos.
                return Ok(Data);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        // POST: api/RegistrosAuditoriaDetalleSeguimiento
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
        public async Task<IActionResult> Post([FromBody] RegistrosAuditoriaDetalleSeguimientoModel entity)
        {
            try
            {
                //Validamos si recibimos una entidad.
                if (entity == null)
                {
                    return BadRequest("The record is null.");
                }

                //Registramos.
                //await _repository.Insert(entity);
                await _repository.Add(entity);

                //Retornamos ruta de registro creado.
                //return CreatedAtRoute("Get", new { Id = entity.Id }, entity);
                return CreatedAtAction(nameof(Get), new { id = entity.Id }, entity);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        // PUT: api/RegistrosAuditoriaDetalleSeguimiento/5
        /// <summary>
        /// Editar un registro por su Id.
        /// </summary>
        /// <remarks>
        /// Edita un registro, buscado por su Id.
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Configruacion ADO</returns>
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] RegistrosAuditoriaDetalleSeguimientoModel entity)
        {
            try
            {
                //Validamos si recibimos una entidad.
                if (entity == null)
                {
                    return BadRequest("The record is null.");
                }

                //Buscamos y validamos que exista el registro buscado.
                //RegistrosAuditoriaDetalleSeguimientoModel ToUpdate = await _repository.GetById(id);
                RegistrosAuditoriaDetalleSeguimientoModel ToUpdate = await _repository.Get(id);
                if (ToUpdate == null)
                {
                    return NotFound("The record couldn't be found.");
                }

                //Actualizamos el registro.
                //await _repository.Update(entity);
                await _repository.Update(ToUpdate, entity);

                //Retornamos sin contenido.
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        // DELETE: api/RegistrosAuditoriaDetalleSeguimiento/5
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
            try
            {
                //Obtenemos registro buscado
                //var Data = await _repository.GetById(id);
                var Data = await _repository.Get(id);

                //Validamos si el registro existe.
                if (Data == null)
                {
                    return NotFound("The record couldn't be found.");
                }

                //Eliminamos el registro.
                //await _repository.Delete(id);
                await _repository.Delete(Data);

                //Retornamos sin contenido.
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        [HttpPost]
        [Route("RegistraObservacionTemporal")]
        public async Task<IActionResult> RegistraObservacionTemporal([FromBody] ResponseLlaveValor input)
        {
            try
            {
                //Obtenemos registro buscado
                await _repository.RegistraObservacionTemporal(input);

                //Retornamos datos.
                return Ok("success");
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        [HttpGet]
        [Route("ConsultaObservacionTemporal/{id}")]
        public async Task<ResponseLlaveValor> ConsultaObservacionTemporal(int id)
        {
            try
            {
                var result = await _repository.ConsultaObservacionTemporal(id);
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
