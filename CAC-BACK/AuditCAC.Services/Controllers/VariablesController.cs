using AuditCAC.Domain.Dto;
using AuditCAC.Domain.Entities;
using AuditCAC.MainCore.Module.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuditCAC.Services.Controllers
{
    [Route("api/Variables")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class VariablesController : ControllerBase
    {
        private readonly IVariablesRepository<VariablesModel> _IRepository;
        private readonly ILogger<VariablesModel> _logger;

        //Constructor
        public VariablesController(IVariablesRepository<VariablesModel> IRepository, ILogger<VariablesModel> logger)
        {
            this._IRepository = IRepository;
            _logger = logger;
        }

        // GET: api/Variables
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
            //Obtenemos todos los registros.
            var Data = await _IRepository.GetAll();
            //IEnumerable<VariablesModel> Data = await _IRepository.GetAll();

            //Retornamos datos.
            return Ok(Data);
        }

        // GET: api/Variables/5
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

        // POST: api/Variables
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
        public async Task<IActionResult> Post([FromBody] VariablesModel entity)
        {
            //Validamos si recibimos una entidad.
            if (entity == null)
            {
                return BadRequest("The record is null.");
            }

            //Registramos.
            await _IRepository.Add(entity);

            //Retornamos ruta de registro creado.
            return CreatedAtRoute("Get", new { Id = entity.Id }, entity);
        }

        // PUT: api/Variables/5
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
        public async Task<IActionResult> Put(int id, [FromBody] VariablesModel entity)
        {
            //Validamos si recibimos una entidad.
            if (entity == null)
            {
                return BadRequest("The record is null.");
            }

            //Buscamos y validamos que exista el registro buscado.
            VariablesModel ToUpdate = await _IRepository.Get(id);
            if (ToUpdate == null)
            {
                return NotFound("The record couldn't be found.");
            }

            //Actualizamos el registro.
            await _IRepository.Update(ToUpdate, entity);

            //Retornamos sin contenido.
            return NoContent();
        }

        // DELETE: api/Variables/5
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
        //[HttpGet("Delete/{id}")]
        [HttpGet]
        [Route("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            //Obtenemos registro buscado
            var Data = await _IRepository.Get(id);

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

        // POST: api/Variables/AtarListadoVariablesMedicion
        /// <summary>
        /// Atar listado de variables a medición.
        /// </summary>
        /// <remarks>
        /// Recibe un listado de variables (con su estructura) y las inserta.
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Configruacion ADO</returns>
        [HttpPost]
        [Route("AtarListadoVariablesMedicion")]
        public async Task<IActionResult> AtarListadoVariablesMedicion([FromBody] List<InputsCreateVariablesDto> entity) //VariablesModel
        {
            try
            {
                await _IRepository.AtarListadoVariablesMedicion(entity);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        // POST: api/Variables/DuplicarVariables
        /// <summary>
        /// Duplicar Variables.
        /// </summary>
        /// <remarks>
        /// Recibe un Id de medición Original, un Ide de medición Nuevo y el Id de del usuario creador. Duplica las variables en la nueva medición. Se puede enviar una descripcion de forma opcional para crear una nueva medición.
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Configruacion ADO</returns>
        [HttpPost]
        [Route("DuplicarVariables")]
        public async Task<IActionResult> DuplicarVariables([FromBody] InputsDuplicarVariablesDto entity)
        {
            try
            {
                var Exito = await _IRepository.DuplicarVariables(entity);
                return Exito != false ? Ok(Exito) : NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        // POST: api/Variables/DuplicarMediciones
        /// <summary>
        /// Duplicar Mediciones.
        /// </summary>
        /// <remarks>
        /// Recibe un Id de medición Original y el Id de del usuario creador. Duplica la medición y la asocia con sus variables.
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Configruacion ADO</returns>
        [HttpPost]
        [Route("DuplicarMedicion")]
        public async Task<IActionResult> DuplicarMedicion([FromBody] InputsDuplicarMedicionDto entity)
        {
            try
            {
                var Exito = await _IRepository.DuplicarMedicion(entity);
                return Exito != false ? Ok(Exito) : NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        // POST: api/Variables/GetVariablesFiltrado
        /// <summary>
        /// Consultar los registros de Registro auditoria filtrados por todos los campos de la tabla.
        /// </summary>
        /// <remarks>
        /// Regresa todos los registros almacenados que correspondan a los filtros aplicados.
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Configruacion ADO</returns>        
        [HttpPost]
        [Route("VariablesFiltrado")]
        public async Task<IActionResult> VariablesFiltrado([FromBody] InputsVariablesDto inputsDto)
        {
            try
            {
                var Data = await _IRepository.GetVariablesFiltrado(inputsDto);
                var PaginationDto = new PaginationDto();
                PaginationDto.Data = Data.Item1;
                PaginationDto.NoRegistrosTotales = Data.Item2;
                PaginationDto.NoRegistrosTotalesFiltrado = Data.Item3;
                PaginationDto.TotalPages = Data.Item4;

                //return Ok(Data.Item1);
                return Ok(PaginationDto);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        // POST: api/Variables/ActualizarVariablesLider
        /// <summary>
        /// Actualiza datos de Variables en el perfil Lider.
        /// </summary>
        /// <remarks>
        /// Recibe un Id de variable, y los campos que se van actualizar (subGrupoId, default, auditable, visible).
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Configruacion ADO</returns>
        [HttpPost]
        [Route("ActualizarVariablesLider")]
        public async Task<IActionResult> ActualizarVariablesLider([FromBody] InputsActualizarVariablesLiderDto entity)
        {
            try
            {
                await _IRepository.ActualizarVariablesLider(entity);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        // POST: api/Variables/ActualizarVariablesLiderMasivo
        /// <summary>
        /// Actualiza datos de Variables en el perfil Lider de forma masiva.
        /// </summary>
        /// <remarks>
        /// Recibe Los Ids de variables y su listado de los campos que se van actualizar (subGrupoId, default, auditable, visible).
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Configruacion ADO</returns>
        [HttpPost]
        [Route("ActualizarVariablesLiderMasivo")]
        public async Task<IActionResult> ActualizarVariablesLiderMasivo([FromBody] List<InputsActualizarVariablesLiderDto> entity)
        {
            try
            {
                await _IRepository.ActualizarVariablesLiderMasivo(entity);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        // POST: api/Variables/CrearVariables
        /// <summary>
        /// Crea una nueva Variable
        /// </summary>
        /// <remarks>
        /// Recibe Los datos de una Variable y la crea. Ademas tambien inserta datos en tablas relacionadas a variables.
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Configruacion ADO</returns>
        [HttpPost]
        [Route("CrearVariables")]
        public async Task<IActionResult> CrearVariables([FromBody] InputsCEVariablesDto entity)
        {
            try
            {
                var Exito = await _IRepository.CrearVariables(entity);
                return Exito != false ? Ok(Exito): NoContent();
                
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        // POST: api/Variables/ActualizarVariables
        /// <summary>
        /// Edita una Variable existente
        /// </summary>
        /// <remarks>
        /// Recibe Los datos de una Variable y la edita. Edita tambien sus tablas relacionadas.
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Configruacion ADO</returns>
        [HttpPost]
        [Route("ActualizarVariables")]
        public async Task<IActionResult> ActualizarVariables([FromBody] InputsCEVariablesDto entity)
        {
            try
            {
                var Exito = await _IRepository.ActualizarVariables(entity);
                return Exito != false ? Ok(Exito) : NoContent();
                //return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        // POST: api/Variables/ConsultarOrderVariables
        /// <summary>
        /// Consultar los campos Orden de Variables.
        /// </summary>
        /// <remarks>
        /// Regresa el Order maximo de las tablas Variables y VariableXMedicion.
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Configruacion ADO</returns>        
        [HttpPost]
        [Route("ConsultarOrderVariables")]
        public async Task<IActionResult> ConsultarOrderVariables([FromBody] InputConsultarOrderVariablesDto inputsDto)
        {
            try
            {
                var Data = await _IRepository.ConsultarOrderVariables(inputsDto);                

                return Ok(Data);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }


        /// <summary>
        /// Consulta variables condicionadas por id de variable
        /// </summary>
        /// <param name="variableId"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("ConsultaVariablesCondicionadas/{variableId}/{medicionId}")]
        public async Task<IActionResult> ConsultaVariablesCondicionadas(int variableId, int medicionId)
        {
            try
            {
                var data = await _IRepository.ConsultaVariablesCondicionadas(variableId, medicionId);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }
    }
}
