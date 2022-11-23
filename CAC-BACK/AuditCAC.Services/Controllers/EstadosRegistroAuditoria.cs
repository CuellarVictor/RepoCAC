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
    [Route("api/EstadosRegistroAuditoria")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class EstadosRegistroAuditoria: ControllerBase
    {
        private readonly IEstadosRegistroAuditoriaRepository<EstadosRegistroAuditoriaModel> _IRepository;
        private readonly ILogger<EstadosRegistroAuditoriaModel> _logger;

        //Costructor
        public EstadosRegistroAuditoria(IEstadosRegistroAuditoriaRepository<EstadosRegistroAuditoriaModel> IRepository, ILogger<EstadosRegistroAuditoriaModel> logger)
        {
            this._IRepository = IRepository;
            _logger = logger;
        }

        //Metodos.

        // GET: api/EstadosRegistroAuditoria
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
        //[Route("/GetAllERA")]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                //Obtenemos todos los registros.
                var Data = await _IRepository.GetAll();
                //IEnumerable<EstadosRegistroAuditoriaModel> Data = await _IRepository.GetAll();

                //Retornamos datos.
                return Ok(Data);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }            
        }

        // GET: api/EstadosRegistroAuditoria/5
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
        //[Route("/GetByIdERA")]
        public async Task<IActionResult> Get(int id)
        {
            try
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
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }  
        }

        //// POST: api/EstadosRegistroAuditoria
        ///// <summary>
        ///// Crear un registro nuevo.
        ///// </summary>
        ///// <remarks>
        ///// Permite crear un nuevo registro y regresa ruta para consultar el registro creado.
        ///// </remarks>
        ///// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        ///// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        ///// <response code="500">Error general, informar al administrador del servicio.</response>
        ///// <returns>Configruacion ADO</returns>
        //[HttpPost]
        //public async Task<IActionResult> Post([FromBody] EstadosRegistroAuditoriaModel entity)
        //{
        //    try
        //    {
        //        //Validamos si recibimos una entidad.
        //        if (entity == null)
        //        {
        //            return BadRequest("The record is null.");
        //        }

        //        //Registramos.
        //        await _IRepository.Add(entity);

        //        //Retornamos ruta de registro creado.
        //        return CreatedAtRoute("Get", new { Id = entity.Id }, entity);
        //    }
        //    catch (Exception ex)
        //    {
        //        _log.Error("Error", ex);
        //        throw new Exception("Error", ex);
        //    }
        //}

        //// PUT: api/EstadosRegistroAuditoria/5
        ///// <summary>
        ///// Editar un registro por su Id.
        ///// </summary>
        ///// <remarks>
        ///// Edita un registro, buscado por su Id.
        ///// </remarks>
        ///// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        ///// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        ///// <response code="500">Error general, informar al administrador del servicio.</response>
        ///// <returns>Configruacion ADO</returns>
        //[HttpPut("{id:int}")]
        //public async Task<IActionResult> Put(int id, [FromBody] EstadosRegistroAuditoriaModel entity)
        //{
        //    try
        //    {
        //        //Validamos si recibimos una entidad.
        //        if (entity == null)
        //        {
        //            return BadRequest("The record is null.");
        //        }

        //        //Buscamos y validamos que exista el registro buscado.
        //        EstadosRegistroAuditoriaModel ToUpdate = await _IRepository.Get(id);
        //        if (ToUpdate == null)
        //        {
        //            return NotFound("The record couldn't be found.");
        //        }

        //        //Actualizamos el registro.
        //        await _IRepository.Update(ToUpdate, entity);

        //        //Retornamos sin contenido.
        //        return NoContent();
        //    }
        //    catch (Exception ex)
        //    {
        //        _log.Error("Error", ex);
        //        throw new Exception("Error", ex);
        //    }
        //}

        //// DELETE: api/EstadosRegistroAuditoria/5
        ///// <summary>
        ///// Eliminar un registro con su Id.
        ///// </summary>
        ///// <remarks>
        ///// Elimina un registro buscado por su Id.
        ///// </remarks>
        ///// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        ///// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        ///// <response code="500">Error general, informar al administrador del servicio.</response>
        ///// <returns>Configruacion ADO</returns>
        //[HttpDelete("{id:int}")]
        //public async Task<IActionResult> Delete(int id)
        //{
        //    try
        //    {
        //        //Obtenemos registro buscado
        //        var Data = await _IRepository.Get(id);

        //        //Validamos si el registro existe.
        //        if (Data == null)
        //        {
        //            return NotFound("The record couldn't be found.");
        //        }

        //        //Eliminamos el registro.
        //        await _IRepository.Delete(Data);

        //        //Retornamos sin contenido.
        //        return NoContent();
        //    }
        //    catch (Exception ex)
        //    {
        //        _log.Error("Error", ex);
        //        throw new Exception("Error", ex);
        //    }
        //}
    }
}
