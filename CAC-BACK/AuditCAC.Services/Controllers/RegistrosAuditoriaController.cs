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
    [Route("api/RegistrosAuditoria")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class RegistrosAuditoriaController : ControllerBase
    {
        private readonly IRegistrosAuditoriaRepository<RegistrosAuditoriaModel> _IRepository;
        private readonly ILogger<RegistrosAuditoriaModel> _logger;

        public RegistrosAuditoriaController(IRegistrosAuditoriaRepository<RegistrosAuditoriaModel> IRepository, ILogger<RegistrosAuditoriaModel> logger)
        {
            this._IRepository = IRepository;
            _logger = logger;
        }

        // GET: api/RegistrosAuditoria
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
            //IEnumerable<RegistrosAuditoriaModel> Data = await _IRepository.GetAll();

            //Retornamos datos.
            return Ok(Data);
        }

        // GET: api/RegistrosAuditoria/5
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

        // POST: api/RegistrosAuditoria
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
        public async Task<IActionResult> Post([FromBody] RegistrosAuditoriaModel entity)
        {
            try
            {
                //Validamos si recibimos una entidad.
                if (entity == null)
                {
                    return BadRequest("The record is null.");
                }

                //Registramos.
                await _IRepository.Add(entity);

                //Retornamos ruta de registro creado.
                //return CreatedAtRoute("Get", new { Id = entity.Id.ToString() }, entity); //Aqui hay un error debido a que no puede generar una ruta "No route matches the supplied values."
                return CreatedAtAction(nameof(Get), new { id = entity.Id }, entity);
                //return Ok(); //Aqui hay un error debido a que no puede generar una ruta "No route matches the supplied values."
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        // PUT: api/RegistrosAuditoria/5
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
        public async Task<IActionResult> Put(int id, [FromBody] RegistrosAuditoriaModel entity)
        {
            //Validamos si recibimos una entidad.
            if (entity == null)
            {
                return BadRequest("The record is null.");
            }

            //Buscamos y validamos que exista el registro buscado.
            RegistrosAuditoriaModel ToUpdate = await _IRepository.Get(id);
            if (ToUpdate == null)
            {
                return NotFound("The record couldn't be found.");
            }

            //Actualizamos el registro.
            await _IRepository.Update(ToUpdate, entity);

            //Retornamos sin contenido.
            return NoContent();
        }

        // DELETE: api/RegistrosAuditoria/5
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

        // POST: api/RegistrosAuditoria/MoverRegistrosAuditoria
        /// <summary>
        /// Mueve un listado de registros a una nueva bolsa.
        /// </summary>
        /// <remarks>
        /// Recibe un listado de registros a una nueva bolsa.
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Configruacion ADO</returns>
        [HttpPost]
        [Route("MoverRegistrosAuditoria")]        
        public async Task<IActionResult> MoverRegistrosAuditoria([FromBody] InputsMoverRegistrosAuditoriaDto inputs)
        {
            try
            {
                await _IRepository.MoverRegistrosAuditoria(inputs);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        // POST: api/RegistrosAuditoria/RegistrosAuditoriaAsignadoAuditorEstado
        /// <summary>
        /// Consultar los registros de Registro auditoria filtrados por el auditor y/o el estado.
        /// </summary>
        /// <remarks>
        /// Regresa todos los registros almacenados que correspondan al auditor y/o al estado ingresado.
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Configruacion ADO</returns>        
        [HttpPost]
        [Route("RegistrosAuditoriaAsignadoAuditorEstado")]
        public async Task<IActionResult> RegistrosAuditoriaAsignadoAuditorEstado([FromBody] InputsRegistrosAuditoriaAsignadoAuditorEstadoDto inputsDto)
        {
            try
            {
                var Data = await _IRepository.GetRegistrosAuditoriaAsignadoAuditorEstado(inputsDto);
                return Ok(Data);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        // POST: api/RegistrosAuditoria/RegistrosAuditoriaFiltrado
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
        [Route("RegistrosAuditoriaFiltrado")]
        public async Task<IActionResult> RegistrosAuditoriaFiltrado([FromBody] InputsRegistrosAuditoriaFiltradoDto inputsDto)
        {
            try
            {                
                var Data = await _IRepository.GetRegistrosAuditoriaFiltrado(inputsDto);
                //HttpContext.Response.Headers.Add("NoRegistrosTotales", Data.Item2);
                //HttpContext.Response.Headers.Add("TotalPages", Daa.Item3);

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

        // POST: api/RegistrosAuditoria/RegistrosAuditoriaFiltros
        /// <summary>
        /// Consultar los filtros y valores correspondientes segun el usuario consultado.
        /// </summary>
        /// <remarks>
        /// Regresa todos los filtros disponibles para cada usuario.
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Configruacion ADO</returns>        
        [HttpPost]
        [Route("RegistrosAuditoriaFiltros")]
        public async Task<IActionResult> RegistrosAuditoriaFiltros([FromBody] InputsRegistrosAuditoriaFiltrosDto inputsDto)
        {
            try
            {
                var Data = await _IRepository.GetRegistrosAuditoriaFiltros(inputsDto);
                return Ok(Data);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        // POST: api/RegistrosAuditoria/RegistrosAuditoriaDetallesAsignacion
        /// <summary>
        /// Consultar los detalles de asignación correspondientes segun el usuario consultado.
        /// </summary>
        /// <remarks>
        /// Regresa todos los detalles de asignación correspondientes segun el usuario consultado.
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Configruacion ADO</returns>        
        [HttpPost]
        [Route("RegistrosAuditoriaDetallesAsignacion")]
        public async Task<IActionResult> RegistrosAuditoriaDetallesAsignacion([FromBody] InputDestalleAsignacion inputsDto)
        {
            try
            {
                var Data = await _IRepository.GetRegistrosAuditoriaDetallesAsignacion(inputsDto);
                return Ok(Data);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        // POST: api/RegistrosAuditoria/RegistrosAuditoriaProgresoDiario
        /// <summary>
        /// Consultar el progreso segun el usuario consultado.
        /// </summary>
        /// <remarks>
        /// Regresa el progreso de un usuario consultado.
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Configruacion ADO</returns>        
        [HttpPost]
        [Route("RegistrosAuditoriaProgresoDiario")]
        public async Task<IActionResult> RegistrosAuditoriaProgresoDiario([FromBody] InputsRegistrosAuditoriaFiltrosDto inputsDto)
        {
            try
            {
                var Data = await _IRepository.GetRegistrosAuditoriaProgresoDiario(inputsDto);
                return Ok(Data);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        // POST: api/RegistrosAuditoria/CambiarEstadoRegistroAuditoria
        /// <summary>
        /// Actualiza el estado de un RegistroAuditoria.
        /// </summary>
        /// <remarks>
        /// Recibe el id del registro auditoria y el nuevo estado y lo actualiza al estado recibido.
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Configruacion ADO</returns>
        [HttpPost]
        [Route("CambiarEstadoRegistroAuditoria")]
        public async Task<IActionResult> CambiarEstadoRegistroAuditoria([FromBody] InputsCambiarEstadoRegistroAuditoriaDto inputs)
        {
            try
            {
                await _IRepository.CambiarEstadoRegistroAuditoria(inputs);
                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        // POST: api/RegistrosAuditoria/CambiarEstadoRegistroAuditoriaMasivo
        /// <summary>
        /// Actualiza el estado de un RegistroAuditoria masivamente.
        /// </summary>
        /// <remarks>
        /// Recibe el id del registro auditoria y el nuevo estado. Tambien recibe el listado de Dato_DC_NC_ND y observaciones. Actualiza segun datos recibidos.
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Configruacion ADO</returns>
        [HttpPost]
        [Route("CambiarEstadoRegistroAuditoriaMasivo")]
        public async Task<IActionResult> CambiarEstadoRegistroAuditoriaMasivo([FromBody] InputsActualizarDC_NC_ND_MotivoDto inputs)
        {
            try
            {
                await _IRepository.CambiarEstadoRegistroAuditoriaMasivo(inputs);
                return Ok(); //AGREGAR RESPUESTA DEL SERVICIO.
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        // POST: api/RegistrosAuditoria/SoportesEntidad
        /// <summary>
        /// Consultar los soportes adjuntos por una entidad.
        /// </summary>
        /// <remarks>
        /// Consultar los soportes adjuntos por una entidad.
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Configruacion ADO</returns>        
        [HttpPost]
        [Route("SoportesEntidad")]
        public async Task<IActionResult> SoportesEntidad([FromBody] InputsSoportesEntidadDto inputsDto)
        {
            try
            {
                var Data = await _IRepository.GetSoportesEntidad(inputsDto);
                return Ok(Data);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        // POST: api/RegistrosAuditoria/ActualizarBanderasRegistrosAuditoria
        /// <summary>
        /// Consultar los soportes adjuntos por una entidad.
        /// </summary>
        /// <remarks>
        /// Consultar los soportes adjuntos por una entidad.
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Configruacion ADO</returns>        
        [HttpPost]
        [Route("ActualizarBanderasRegistrosAuditoria")]
        public async Task<IActionResult> ActualizarBanderasRegistrosAuditoria([FromBody] InputsActualizarBanderasRegistrosAuditoriaDto inputsDto)
        {
            try
            {
                await _IRepository.ActualizarBanderasRegistrosAuditoria(inputsDto);
                return Ok(); //AGREGAR RESPUESTA DEL SERVICIO.
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        // GET: api/RegistrosAuditoria/GetTipificaciónByEstadoId
        /// <summary>
        /// Consultar las tipificaciones correspondientes a cada estado.
        /// </summary>
        /// <remarks>
        /// Regresa todas las tipificaciones que correspondan al estado ingresado.
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Configruacion ADO</returns>        
        [HttpPost]
        [Route("GetTipificaciónByEstadoId")]
        public async Task<IActionResult> GetTipificaciónByEstadoId([FromBody] InputsGetTipificaciónByEstadoDto inputsDto)
        {
            try
            {
                var Data = await _IRepository.GetTipificaciónByEstadoId(inputsDto);
                return Ok(Data);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        // GET: api/RegistrosAuditoria/GetAlertasRegistrosAuditoria
        /// <summary>
        /// Consulta el No de registrosAuditoria que solo el usuario actual (Lider) puede atender.
        /// </summary>
        /// <remarks>
        /// Regresa el numero de RegistrosAuditoria que solo el usuario actual (Lider) puede atender.
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Configruacion ADO</returns>        
        [HttpPost]
        [Route("GetAlertasRegistrosAuditoria")]       
        public async Task<IActionResult> GetAlertasRegistrosAuditoria([FromBody] InputsIdUsuarioDto inputsDto)
        {
            try
            {
                var Data = await _IRepository.GetAlertasRegistrosAuditoria(inputsDto);
                return Ok(Data);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        // GET: api/RegistrosAuditoria/GetConsultaPerfilAccion
        /// <summary>
        /// Consulta listadi de perfiles de accion..
        /// </summary>
        /// <remarks>
        /// Regresa el listado de perfiles de accion, con sus respectivas variables asociadas.
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Configruacion ADO</returns>        
        [HttpPost]
        [Route("GetConsultaPerfilAccion")]
        public async Task<IActionResult> GetConsultaPerfilAccion() //[FromBody] InputsIdUsuarioDto inputsDto
        {
            try
            {
                var Data = await _IRepository.GetConsultaPerfilAccion();
                return Ok(Data);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }


        /// <summary>
        /// Consulta usuarios asignados al lider
        /// </summary>
        /// <param name="idUsuarioLider">Id usuario lider</param>
        /// <returns>Modelo usuarios lider</returns>  
        [HttpGet]
        [Route("ConsultaUsuariosLider/{idUsuarioLider}")]
        public async Task<List<ResponseUsuariosLider>> ConsultaUsuariosLider(string idUsuarioLider)
        {
            try
            {
                var result = await _IRepository.ConsultaUsuariosLider(idUsuarioLider);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        /// <summary>
        /// Reversa un Registro Auditoria en estado finalizado
        /// </summary>
        /// <param name="inputsDto">Entrada de datos</param>
        /// <returns>modelo llave valor</returns>
        [HttpPost]
        [Route("ReversarRegistrosAuditoria")]
        public async Task<ResponseLlaveValor> ReversarRegistrosAuditoria([FromBody] InputsReversarRegistrosAuditoriaDto inputsDto)
        {
            try
            {
                var result = await _IRepository.ReversarRegistrosAuditoria(inputsDto);
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