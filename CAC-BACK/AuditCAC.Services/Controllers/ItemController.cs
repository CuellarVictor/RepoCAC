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
    [Route("api/Item")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ItemController : ControllerBase
    {
        //Constructor.
        private readonly IItemRepository<ItemModel> _repository;
        private readonly ILogger<ItemModel> _logger;

        public ItemController(IItemRepository<ItemModel> repository, ILogger<ItemModel> logger)
        {
            this._repository = repository;
            _logger = logger;
        }


        //Metodos

        // GET: api/Item
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
        [Route("GetAll")]
        public async Task<IActionResult> Get()
        {
            try
            {
                //Obtenemos todos los registros.
                var Data = await _repository.GetAll();
                //IEnumerable<ItemModel> Data = await _IRepository.GetAll();

                //Retornamos datos.
                return Ok(Data);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        // GET: api/Item/5
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
        [HttpGet]
        [Route("GetById/{id}")]
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

        // GET: api/Item/5 (CatalogId)
        /// <summary>
        /// Consulta registros registro con su CatalogId.
        /// </summary>
        /// <remarks>
        /// Regresa registros, buscado por su CatalogId.
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Configruacion ADO</returns>
        [HttpGet]
        [Route("GetByCatalogId/{CatalogId}")]
        public async Task<IActionResult> GetItemByCatalogId(int CatalogId)
        {
            try
            {
                //Obtenemos registro buscado
                //var Data = await _repository.GetById(id);
                var Data = await _repository.GetItemByCatalogId(CatalogId);

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

        // POST: api/Item
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
        [Route("Create")]
        public async Task<IActionResult> Post([FromBody] ItemModel entity, string UsuarioId)
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
                await _repository.Add(entity, UsuarioId);

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

        // PUT: api/Item/5
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
        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] ItemModel entity, string UsuarioId)
        {
            try
            {
                //Validamos si recibimos una entidad.
                if (entity == null)
                {
                    return BadRequest("The record is null.");
                }

                //Buscamos y validamos que exista el registro buscado.
                //ItemModel ToUpdate = await _repository.GetById(id);
                //ItemModel ToUpdate = await _repository.Get(entity.Id);
                if (entity == null)
                {
                    return NotFound("The record couldn't be found.");
                }

                //Actualizamos el registro.
                //await _repository.Update(entity);
                await _repository.Update(entity, UsuarioId);

                //Retornamos sin contenido.
                return Ok(entity);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        // DELETE: api/Item/5
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
        //[HttpDelete]
        [HttpGet]
        [Route("Delete")]
        public async Task<IActionResult> Delete(int id, string UsuarioId)
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
                await _repository.Delete(Data, UsuarioId);

                //Retornamos sin contenido.
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }
    }
}
