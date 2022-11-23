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
    [Route("api/VariableXMedicion")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class VariableXMedicionController : ControllerBase
    {
        private readonly IVariableXMedicionRepository<VariableXMedicionModel> _IRepository;
        private readonly ILogger<VariableXMedicionModel> _logger;

        //Contructor.
        public VariableXMedicionController(IVariableXMedicionRepository<VariableXMedicionModel> IRepository, ILogger<VariableXMedicionModel> logger)
        {
            this._IRepository = IRepository;
            _logger = logger;
        }

        //Metodos
        // GET: api/VariableXMedicion
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
            //Obtenemos todos los registros.
            var Data = await _IRepository.GetAll();
            //IEnumerable<VariableXMedicionModel> Data = await _IRepository.GetAll();

            //Retornamos datos.
            return Ok(Data);
        }

        // GET: api/VariableXMedicion/5
        /// <summary>
        /// Consultar un registro con su Id de Variable (VariableId).
        /// </summary>
        /// <remarks>
        /// Regresa un solo registro, buscado por su Id de Variable (VariableId).
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Configruacion ADO</returns>
        [HttpGet]
        [Route("GetByVariableId")]
        public async Task<IActionResult> GetByVariableId(int id)
        {
            //Obtenemos registro buscado
            var Data = await _IRepository.GetByVariableId(id);

            //Validamos si el registro existe.
            if (Data == null)
            {
                return NotFound("The record couldn't be found.");
            }

            //Retornamos datos.
            return Ok(Data);
        }

        // GET: api/VariableXMedicion/5
        /// <summary>
        /// Consultar un registro con su Id de Medicion (MedicionId).
        /// </summary>
        /// <remarks>
        /// Regresa un solo registro, buscado por su Id de Medicion (MedicionId).
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Configruacion ADO</returns>
        [HttpGet]
        [Route("GeByMedicion")]
        public async Task<IActionResult> GeByMedicion(int MedicionId)
        {
            //Obtenemos registro buscado
            var Data = await _IRepository.GetByMedicionId(MedicionId);

            //Validamos si el registro existe.
            if (Data == null)
            {
                return NotFound("The record couldn't be found.");
            }

            //Retornamos datos.
            return Ok(Data);
        }

        // POST: api/VariableXMedicion
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
        public async Task<IActionResult> Post([FromBody] VariableXMedicionModel entity)
        {
            //Validamos si recibimos una entidad.
            if (entity == null)
            {
                return BadRequest("The record is null.");
            }

            //Registramos.
            await _IRepository.Add(entity);

            //Retornamos ruta de registro creado.
            return CreatedAtRoute("Get", new { VariableId = entity.VariableId, MedicionId = entity.MedicionId }, entity);
        }

        //// PUT: api/VariableXMedicion/5
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
        ////[HttpPut("{id:int}")]
        //[HttpPut]
        //[Route("Update")]
        //public async Task<IActionResult> Put([FromBody] InputsGetVariableXMedicionDto InputsEntity, [FromBody] VariableXMedicionModel entity)
        //{
        //    //Validamos si recibimos una entidad.
        //    if (entity == null)
        //    {
        //        return BadRequest("The record is null.");
        //    }

        //    //Buscamos y validamos que exista el registro buscado.
        //    VariableXMedicionModel ToUpdate = await _IRepository.GetByVariableIdOrMedicionId(InputsEntity);
        //    if (ToUpdate == null)
        //    {
        //        return NotFound("The record couldn't be found.");
        //    }

        //    //Actualizamos el registro.
        //    await _IRepository.Update(ToUpdate, entity);

        //    //Retornamos sin contenido.
        //    return NoContent();
        //}

        // DELETE: api/VariableXMedicion/5
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
        public async Task<IActionResult> Delete([FromBody] InputsGetVariableXMedicionDto entity)
        {
            //Obtenemos registro buscado
            var Data = await _IRepository.GetByVariableIdOrMedicionId(entity);

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
    }
}
