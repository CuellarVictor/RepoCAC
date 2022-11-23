using AuditCAC.Domain.Dto;
using AuditCAC.Domain.Entities;
using AuditCAC.MainCore.Module.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AuditCAC.Services.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/BancoInformacion")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BancoInformacionController : ControllerBase
    {
        private readonly IBancoInformacionRepository<BancoInformacionModel> _IRepository;
        private static IConfiguration Configuration;
        private readonly ILogger<BancoInformacionModel> _logger;        

        //Constructor.
        public BancoInformacionController(IBancoInformacionRepository<BancoInformacionModel> IRepository, IConfiguration configuration, ILogger<BancoInformacionModel> logger)
        {
            this._IRepository = IRepository;
            Configuration = configuration;
            _logger = logger;
        }

        //Metodos.

        // GET: api/BancoInformacion
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
            //IEnumerable<BancoInformacionModel> Data = await _IRepository.GetAll();

            //Retornamos datos.
            return Ok(Data);
        }

        // GET: api/BancoInformacion/5
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

        // POST: api/BancoInformacion
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
        public async Task<IActionResult> Post([FromBody] BancoInformacionModel entity)
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
                throw;
            }
        }

        // PUT: api/BancoInformacion/5
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
        //[HttpPut("{id:int}")]
        [HttpPost("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] BancoInformacionModel entity)
        {
            //Validamos si recibimos una entidad.
            if (entity == null)
            {
                return BadRequest("The record is null.");
            }

            //Buscamos y validamos que exista el registro buscado.
            BancoInformacionModel ToUpdate = await _IRepository.Get(id);
            if (ToUpdate == null)
            {
                return NotFound("The record couldn't be found.");
            }

            //Actualizamos el registro.
            await _IRepository.Update(ToUpdate, entity);

            //Retornamos sin contenido.
            return NoContent();
        }

        // DELETE: api/BancoInformacion/5
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
        [Microsoft.AspNetCore.Mvc.Route("Delete")]
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

        // POST: api/BancoInformacion/GetBancoInformacionByPalabraClave
        /// <summary>
        /// Consultar los registros de Banco de información (BancoInformacion).
        /// </summary>
        /// <remarks>
        /// Regresa los registros almacenados en Banco de información (BancoInformacion). Filtra por una palabra clave, que corresponde al codigo o nombre.
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Configruacion ADO</returns>        
        [HttpPost]
        [Microsoft.AspNetCore.Mvc.Route("GetBancoInformacionByPalabraClave")]
        public async Task<IActionResult> BancoInformacionAsignadoAuditorEstado([FromBody] InputsBancoInformacionDto inputsDto)
        {
            try
            {
                var Data = await _IRepository.GetBancoInformacionByPalabraClave(inputsDto);
                return Ok(Data);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        /// <summary>
        /// Para consultar todos los registros de Banco de Informacion creados segun los filtrados ingresados.
        /// </summary>
        /// <param name="inputsDto">Entrada de datos</param>
        /// <returns>Modelo de datos</returns>
        [HttpPost]
        [Microsoft.AspNetCore.Mvc.Route("GetBancoInformacionFiltrado")]
        public async Task<IActionResult> GetBancoInformacionFiltrado([FromBody] InputsBancoInformacionFiltradoDto inputsDto)
        {
            try
            {
                var Data = await _IRepository.GetBancoInformacionFiltrado(inputsDto);

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

        /// <summary>
        /// 
        /// </summary>
        /// 
        /// <returns>Modelo Current Process</returns>    
        [HttpPost]
        [Microsoft.AspNetCore.Mvc.Route("CargarBancoInformacionPlantilla")]
        public async Task<IActionResult> CargarBancoInformacionPlantilla()
        {
            try
            {
                var filePath = Configuration["PathList:LoadPoblationPath"];

                var result = await _IRepository.CargarBancoInformacionPlantilla(filePath);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                return Ok(ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputModel">Modelo Cargue</param>
        /// <returns>Modelo Current Process</returns>    
        [HttpPost]
        [Microsoft.AspNetCore.Mvc.Route("CargarBancoInformacion")]
        public async Task<IActionResult> CargarBancoInformacion([FromBody] InputCargarBancoInformacionDto inputModel)
        {
            try
            {
                _logger.LogInformation("INGRESA CargarBancoInformacion");
                var filePath = Configuration["PathList:LoadPoblationPath"];

                S3Model s3 = new S3Model()
                {
                    S3AccessKeyId = Configuration["S3:S3AccessKeyId"],
                    S3SecretAccessKey = Configuration["S3:S3SecretAccessKey"],
                    S3RegionEndpoint = Configuration["S3:S3RegionEndpoint"],
                    S3BucketName = Configuration["S3:S3BucketName"],
                    S3StorageClass = Configuration["S3:S3StorageClass"],
                    UrlFileS3Uploaded = Configuration["S3:UrlFileS3Uploaded"]
                };

                var result = await _IRepository.CargarBancoInformacion(filePath, inputModel, s3);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                return Ok(ex);
            }
        }
    }
}
