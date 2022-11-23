using AuditCAC.Domain.Dto;
using AuditCAC.Domain.Entities;
using AuditCAC.MainCore.Module.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;

namespace AuditCAC.Services.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/Roles")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RolesController : ControllerBase
    {
        //private readonly IRepository<RolesModelDto> _IRepository;     
        private readonly IRepository<CurrentProcessModel> _IRepository;
        private readonly ILogger<RolesModelDto> _logger;        

        //Constructor.
        public RolesController(IRepository<CurrentProcessModel> IRepository, ILogger<RolesModelDto> logger) //  IRolesRepository<RolesModelDto> IRepository
        {
            this._IRepository = IRepository;
            _logger = logger;
        }

        // Metodos.

        // GET: api/Roles
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
        [HttpGet("GetAll")]
        public async Task<IActionResult> Get()
        {
            //Obtenemos todos los registros.
            var Data = await _IRepository.RolesGetAll();
            //IEnumerable<Roles> Data = await _IRepository.GetAll();

            //Retornamos datos.
            return Ok(Data);
        }

        // GET: api/Roles/5
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
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(string id)
        {
            //Obtenemos registro buscado
            var Data = await _IRepository.RolesGetById(id);

            //Validamos si el registro existe.
            if (Data == null)
            {
                return NotFound("The record couldn't be found.");
            }

            //Retornamos datos.
            return Ok(Data);
        }

        // POST: api/Roles
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
        [HttpPost("Create")]
        public async Task<IActionResult> Post([FromBody] InputRolesModelDto entity)
        {
            try
            {
                //Validamos si recibimos una entidad.
                if (entity == null)
                {
                    return BadRequest("The record is null.");
                }

                //Registramos.
                await _IRepository.RolesAdd(entity);

                //Retornamos ruta de registro creado.
                //return CreatedAtRoute("Get", new { Id = entity.Id.ToString() }, entity); //Aqui hay un error debido a que no puede generar una ruta "No route matches the supplied values."
                return CreatedAtAction(nameof(Get), new { id = entity.Id }, entity);
                //return Ok(); //Aqui hay un error debido a que no puede generar una ruta "No route matches the supplied values."
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        // PUT: api/Roles/5
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
        //[HttpPut("{id:string}")]
        [HttpPost("Update")]
        public async Task<IActionResult> Put(string id, [FromBody] InputRolesModelDto entity)
        {
            //Validamos si recibimos una entidad.
            if (entity == null)
            {
                return BadRequest("The record is null.");
            }

            ////Buscamos y validamos que exista el registro buscado.
            //RolesModelDto ToUpdate = await _IRepository.RolesGetById(id);
            //if (ToUpdate == null)
            //{
            //    return NotFound("The record couldn't be found.");
            //}

            //Actualizamos el registro.
            await _IRepository.RolesUpdate(id, entity);

            //Retornamos sin contenido.
            return NoContent();
        }

        // DELETE: api/Roles/5
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
        //[HttpDelete("{id:string}")]
        [HttpGet]
        [Microsoft.AspNetCore.Mvc.Route("Delete")]
        public async Task<ActionResult<ResponseRolesDeleteDto>> Delete(string id)
        {
            //Obtenemos registro buscado
            //var Data = await _IRepository.RolesGetById(id);

            //Validamos si el registro existe.
            //if (Data == null)
            //{
            //    return NotFound("The record couldn't be found.");
            //}

            //Eliminamos el registro.
            var Data = await _IRepository.RolesDelete(id);

            //Retornamos datos.
            return Data;
        }
    }
}
