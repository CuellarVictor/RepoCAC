using AuditCAC.Domain.Dto;
using AuditCAC.Domain.Entities;
using AuditCAC.MainCore.Module.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuditCAC.Services.Controllers
{
    [Route("api/Catalog")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CatalogController : ControllerBase
    {
        private readonly IRepository<CatalogModel> _repository;
        private readonly ILogger<CatalogModel> _logger;
        private readonly IConfiguration configuration;

        //public string connectionString { get; set; }
        //Constructor
        public CatalogController(IRepository<CatalogModel> repository, ILogger<CatalogModel> logger, IConfiguration configuration)
        {
            this._repository = repository;
            _logger = logger;

            //connectionString = configuration["ConnectionStrings:DefaultConnection"];
        }

        //Metodos
        // GET: api/Catalog
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
                //IEnumerable<CatalogModel> Data = await _IRepository.GetAll();

                //Retornamos datos.
                return Ok(Data);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        // GET: api/Catalog/5
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
                var Data = await _repository.GetById(id);

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

        // POST: api/Catalog
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
        public async Task<IActionResult> Post([FromBody] CatalogModel entity)
        {
            try
            {
                //Validamos si recibimos una entidad.
                if (entity == null)
                {
                    return BadRequest("The record is null.");
                }

                //Registramos.
                await _repository.Insert(entity);

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

        // PUT: api/Catalog/5
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
        public async Task<IActionResult> Put(int id, [FromBody] CatalogModel entity)
        {
            try
            {
                //Validamos si recibimos una entidad.
                if (entity == null)
                {
                    return BadRequest("The record is null.");
                }

                //Buscamos y validamos que exista el registro buscado.
                CatalogModel ToUpdate = await _repository.GetById(id);
                if (ToUpdate == null)
                {
                    return NotFound("The record couldn't be found.");
                }

                //Actualizamos el registro.
                //await _repository.Update(ToUpdate, entity);
                await _repository.Update(entity);

                //Retornamos sin contenido.
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        // DELETE: api/Catalog/5
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
                var Data = await _repository.GetById(id);

                //Validamos si el registro existe.
                if (Data == null)
                {
                    return NotFound("The record couldn't be found.");
                }

                //Eliminamos el registro.
                //await _repository.Delete(Data);
                await _repository.Delete(id);

                //Retornamos sin contenido.
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        //[HttpPost("ExecuteScript")]
        //public async Task<IActionResult> ExecuteScript([FromBody] ResponseLlaveValor input)
        //{
        //    try
        //    {
        //        var Data = await _repository.ExecuteScript(connectionString, input.Valor);
        //        return Ok(Data);
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogInformation(ex.ToString());
        //        throw new Exception("Error", ex);
        //    }
        //}
    }
}
