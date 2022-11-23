using AuditCAC.Domain.Dto;
using AuditCAC.Domain.Dto.BolsasMedicion;
using AuditCAC.Domain.Entities;
using AuditCAC.MainCore.Module.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AuditCAC.MainCore.Module.BolsasDeMedicion.Interface;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.IO;
using Microsoft.Extensions.Configuration;
using AuditCAC.Domain.Helpers;
using AuditCAC.Domain.Dto.CarguePoblacion;
using Microsoft.Extensions.Logging;

namespace AuditCAC.Services.Controllers
{
    [Route("api/Medicion")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MedicionController : ControllerBase
    {
        private readonly IMedicionRepository<MedicionModel> _MedicionRepository;        
        private readonly IBolsasMedicionManager _medicion;
        private static IConfiguration Configuration;
        private readonly ILogger<MedicionModel> _logger;

        //Constructor
        public MedicionController(IMedicionRepository<MedicionModel> MedicionRepository,
            IBolsasMedicionManager medicion,
            IConfiguration configuration,
            IRepository<CurrentProcessModel> _repositoryCurrentProcess,
            IRepository<CurrentProcessParamModel> _repositoryCurrentProcessParam, ILogger<MedicionModel> logger)
        {
            this._MedicionRepository = MedicionRepository;
            _medicion = medicion;
            Configuration = configuration;
            _logger = logger;
        }

        // GET: api/Medicion
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
        [Route("GetAllVa")]
        public async Task<IActionResult> Get()
        {
            //Obtenemos todos los registros.
            var Data = await _MedicionRepository.GetAll();
            //IEnumerable<MedicionModel> Data = await _MedicionRepository.GetAll();

            //Retornamos datos.
            return Ok(Data);
        }

        // GET: api/Medicion/5
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
        //[HttpGet("{id:int}")]
        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<IActionResult> Get(int id)
        {
            //Obtenemos registro buscado
            var Data = await _MedicionRepository.Get(id);

            //Validamos si el registro existe.
            if (Data == null)
            {
                return NotFound("The record couldn't be found.");
            }

            //Retornamos datos.
            return Ok(Data);
        }

        // POST: api/Medicion
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
        [Route("Post")]
        public async Task<IActionResult> Post([FromBody] BolsaMedicionNuevaDto entity)
        {
            //Validamos si recibimos una entidad.
            if (entity == null)
            {
                return BadRequest("The record is null.");
            }

            //Registramos.
            var res = await _medicion.AddOrUpdate(entity);

            //Retornamos ruta de registro creado.
            //return CreatedAtRoute("Get", new { Id = entity.Id }, entity); //Aqui hay un error debido a que no puede generar una ruta "No route matches the supplied values."
            return res != false ? Ok(true) : (IActionResult)(NoContent());

        }

        // PUT: api/Medicion/5
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
        //[HttpPut] //Comentado debido a error en ambiente del cliente.
        [HttpPost]
        [Route("Update/{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] MedicionModel entity)
        {
            //Validamos si recibimos una entidad.
            if (entity == null)
            {
                return BadRequest("The record is null.");
            }

            //Buscamos y validamos que exista el registro buscado.
            MedicionModel ToUpdate = await _MedicionRepository.Get(id);
            if (ToUpdate == null)
            {
                return NotFound("The record couldn't be found.");
            }

            //Actualizamos el registro.
            await _MedicionRepository.Update(ToUpdate, entity);

            //Retornamos sin contenido.
            return NoContent();
        }

        // DELETE: api/Medicion/5
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
        //[HttpDelete]
        [HttpGet]
        [Route("Delete/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                //Obtenemos registro buscado
                var Data = await _MedicionRepository.Get(id);

                //Validamos si el registro existe.
                if (Data == null)
                {
                    return NotFound("The record couldn't be found.");
                }

                //Eliminamos el registro.
                await _MedicionRepository.Delete(Data);

                //Retornamos sin contenido.
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }


        [HttpPost("/getAll")]
        public async Task<IActionResult> GetAllMedicion([FromBody] InputsMedicionAllDto MedicionAllDto)
        {
            try
            {
                var Data = await _MedicionRepository.GetMedicionAll(MedicionAllDto);
                return Ok(Data);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        // Post: api/GetFiltrosBolsaMedicion
        /// <summary>
        /// Obtener las bolsas de medicion con el periodo asociados a un lider.
        /// </summary>
        /// <remarks>
        /// Obtiene las bolsas de medicion asociadas al lider.
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Configruacion ADO</returns>
        [HttpPost]
        [Route("GetFiltrosBolsaMedicion")]
        public async Task<IActionResult> Post([FromBody] InputGetFiltrosBolsaMedicionDto filtrosBolsaMedicionDto) //InputFiltrosBolsaMedicionDto filtrosBolsaMedicionDto
        {
            try
            {
                var Data = await _MedicionRepository.GetFiltrosBolsaMedicion(filtrosBolsaMedicionDto);
                var PaginationDto = new PaginationDto();
                PaginationDto.Data = Data.Item1;
                PaginationDto.NoRegistrosTotales = Data.Item2;
                PaginationDto.NoRegistrosTotalesFiltrado = Data.Item3;
                PaginationDto.TotalPages = Data.Item4;

                //return Ok(Data.Item1);
                //return Ok(Data);
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
        /// <param name="inputModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CargarArchivoPoblacionTest")]
        public async Task<IActionResult> CargarArchivoPoblacionTest([FromBody]  InputCarguePoblacion inputModel)
        {
            try
            {
                _logger.LogInformation("INGRESA CargarArchivoPoblacionTest");
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
                var result = await _MedicionRepository.IniciaProcesoCarguePoblacion(filePath, inputModel, s3);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                return Ok(ex);
            }

        }

        // Post: api/GetConsultaEstructurasCargePoblacion
        /// <summary>
        /// Retorna listado de Estructuras para Carge Poblacion
        /// </summary>
        /// <remarks>
        /// Retorna listado de Estructuras para Carge Poblacion
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Configruacion ADO</returns>
        [HttpPost]
        [Route("GetConsultaEstructurasCargePoblacion")]
        public async Task<IActionResult> GetConsultaEstructurasCargePoblacion([FromBody] InputsBolsasMedicionIdDto inputsBolsasMedicionIdDto)
        {
            try
            {
                var Data = await _MedicionRepository.GetConsultaEstructurasCargePoblacion(inputsBolsasMedicionIdDto);
                return Ok(Data);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        // Post: api/GetValidacionEstadoBolsasMedicion
        /// <summary>
        /// Obtener el estado de una bolsas de medicion.
        /// </summary>
        /// <remarks>
        /// Obtiene el estado de una bolsas de medicion.
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Configruacion ADO</returns>
        [HttpPost]
        [Route("GetValidacionEstadoBolsasMedicion")]
        public async Task<IActionResult> GetValidacionEstadoBolsasMedicion([FromBody] InputsBolsasMedicionIdDto inputsBolsasMedicionIdDto) //InputFiltrosBolsaMedicionDto filtrosBolsaMedicionDto
        {
            try
            {
                var Data = await _MedicionRepository.GetValidacionEstadoBolsasMedicion(inputsBolsasMedicionIdDto);                
                return Ok(Data);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        // Post: api/GetUsuariosBolsaMedicionFiltro
        /// <summary>
        /// Obtener usuarios de una bolsa de medicion
        /// </summary>
        /// <remarks>
        /// Obtiene el listado de una bolsa de medicion
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Configruacion ADO</returns>
        [HttpPost]
        [Route("GetUsuariosBolsaMedicionFiltro")]
        public async Task<IActionResult> GetUsuariosBolsaMedicionFiltro([FromBody] InputsBolsasMedicionIdDto inputsBolsasMedicionIdDto)
        {
            try
            {
                var Data = await _MedicionRepository.GetUsuariosBolsaMedicionFiltro(inputsBolsasMedicionIdDto);
                return Ok(Data);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        // Post: api/GetUsuariosBolsaMedicion
        /// <summary>
        /// Obtener usuarios de una bolsa de medicion con sus detalles.
        /// </summary>
        /// <remarks>
        /// Obtiene el listado de una bolsa de medicion con sus detalles.
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Configruacion ADO</returns>
        [HttpPost]
        [Route("GetUsuariosBolsaMedicion")]
        public async Task<IActionResult> GetUsuariosBolsaMedicion([FromBody] InputsBolsasMedicionDto inputsBolsasMedicionDto)
        {
            try
            {
                var Data = await _MedicionRepository.GetUsuariosBolsaMedicion(inputsBolsasMedicionDto);

                var PaginationDto = new PaginationDto();
                PaginationDto.Data = Data.Item1;
                PaginationDto.NoRegistrosTotales = Data.Item2;
                PaginationDto.NoRegistrosTotalesFiltrado = Data.Item3;
                PaginationDto.TotalPages = Data.Item4;

                return Ok(PaginationDto);
                //return Ok(Data);
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
        /// <param name="inputsDto"></param>
        /// <returns></returns>     
        [HttpPost]
        [Route("GetRegistrosAuditoriaXBolsaMedicionFiltros")]
        public async Task<IActionResult> GetRegistrosAuditoriaXBolsaMedicionFiltros([FromBody] InputsRegistrosAuditoriaXBolsaMedicionFiltrosDto inputsDto)
        {
            try
            {
                var Data = await _MedicionRepository.GetRegistrosAuditoriaXBolsaMedicionFiltros(inputsDto);
                return Ok(Data);
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
        /// <param name="inputsBolsasMedicionDto"></param>
        /// <returns></returns>    
        [HttpPost]
        [Route("GetRegistrosAuditoriaXBolsaMedicion")]
        public async Task<IActionResult> GetRegistrosAuditoriaXBolsaMedicion([FromBody] InputsGetRegistrosAuditoriaXBolsaMedicionDto inputsBolsasMedicionDto)
        {
            try
            {
                var Data = await _MedicionRepository.GetRegistrosAuditoriaXBolsaMedicion(inputsBolsasMedicionDto);

                var PaginationDto = new PaginationDto();
                PaginationDto.Data = Data.Item1;
                PaginationDto.NoRegistrosTotales = Data.Item2;
                PaginationDto.NoRegistrosTotalesFiltrado = Data.Item3;
                PaginationDto.TotalPages = Data.Item4;

                return Ok(PaginationDto);
                //return Ok(Data);
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
        /// <param name="inputs"></param>
        /// <returns></returns>  
        [HttpPost]
        [Route("ReasignacionesBolsaEquitativa")]
        public async Task<IActionResult> ReasignacionesBolsaEquitativa([FromBody] InputReasignacionesBolsaEquitativaDto inputs)
        {
            try
            {
                var Exito = await _MedicionRepository.ReasignacionesBolsaEquitativa(inputs);
                return Exito != false ? Ok(Exito) : NoContent();                
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
        /// <param name="inputModel">Modelo Cargue</param>
        /// <returns>Modelo Current Process</returns>    
        [HttpPost]
        [Route("ReasignacionesBolsaDetallada")]
        public async Task<IActionResult> ReasignacionesBolsaDetallada([FromBody] InputReasignacionesBolsaDetalladaDto inputModel)
        {
            try
            {
                _logger.LogInformation("INGRESA ReasignacionesBolsaDetallada");
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

                var result = await _MedicionRepository.ReasignacionesBolsaDetallada(filePath, inputModel, s3);
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
        /// <param name="inputsDto"></param>
        /// <returns></returns>     
        [HttpPost]
        [Route("GetBolsasMedicionXEnfermedadMadre")]
        public async Task<IActionResult> GetBolsasMedicionXEnfermedadMadre([FromBody] InputsBolsasMedicionXEnfermedadMadreDto inputsDto)
        {
            try
            {
                var Data = await _MedicionRepository.GetBolsasMedicionXEnfermedadMadre(inputsDto);
                return Ok(Data);
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
        /// <param name="inputsDto">Modelo Cargue</param>
        /// <returns>Modelo Current Process</returns>    
        [HttpPost]
        [Route("GetUsuariosByRol")]
        public async Task<IActionResult> GetUsuariosByRol([FromBody] InputsGetUsuariosByRoleIdDto inputsDto)
        {
            try
            {
                var Data = await _MedicionRepository.GetUsuariosByRol(inputsDto);
                return Ok(Data);
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
        /// <param name="inputModel">Modelo Cargue</param>
        /// <returns>Modelo Current Process</returns>    
        [HttpPost]
        [Route("MoverTodosRegistrosAuditoriaBolsaMedicion")]
        public async Task<IActionResult> MoverTodosRegistrosAuditoriaBolsaMedicion([FromBody] InputMoverTodosRegistrosAuditoriaBolsaMedicionDto inputModel)
        {
            try
            {
                var filePath = Configuration["PathList:LoadPoblationPath"];               

                var result = await _MedicionRepository.MoverTodosRegistrosAuditoriaBolsaMedicion(filePath, inputModel);
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
        [Route("MoverAlgunosRegistrosAuditoriaBolsaMedicionPlantilla")]
        public async Task<IActionResult> MoverAlgunosRegistrosAuditoriaBolsaMedicionPlantilla([FromBody] InputMoverTodosRegistrosAuditoriaBolsaMedicionDto inputModel)
        {
            try
            {
                var filePath = Configuration["PathList:LoadPoblationPath"];

                var result = await _MedicionRepository.MoverAlgunosRegistrosAuditoriaBolsaMedicionPlantilla(filePath, inputModel);
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
        [Route("MoverAlgunosRegistrosAuditoriaBolsaMedicion")]
        public async Task<IActionResult> MoverAlgunosRegistrosAuditoriaBolsaMedicion([FromBody] InputMoverAlgunosRegistrosAuditoriaBolsaMedicionDto inputModel)
        {
            try
            {
                _logger.LogInformation("INGRESA MoverAlgunosRegistrosAuditoriaBolsaMedicion");
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

                var result = await _MedicionRepository.MoverAlgunosRegistrosAuditoriaBolsaMedicion(filePath, inputModel, s3);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                return Ok(ex);
            }
        }

        #region Perfil Admin: Eliminar registros de una medición o bolsa durante el proceso de auditoría
        /// <summary>
        /// 
        /// </summary>
        /// <returns>Modelo Current Process</returns>    
        [HttpPost]
        [Route("EliminarRegistrosAuditoriaPlantilla")]
        public async Task<IActionResult> EliminarRegistrosAuditoriaPlantilla() // , InputEliminarRegistrosAuditoriaPlantillaDto input
        {
            try
            {
                var filePath = Configuration["PathList:LoadPoblationPath"];

                var result = await _MedicionRepository.EliminarRegistrosAuditoriaPlantilla(filePath); //, inputModel
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
        [Route("EliminarRegistrosAuditoria")]
        public async Task<IActionResult> EliminarRegistrosAuditoria([FromBody] InputEliminarRegistrosAuditoriaDto inputModel)
        {
            try
            {
                _logger.LogInformation("INGRESA EliminarRegistrosAuditoria");
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

                var result = await _MedicionRepository.EliminarRegistrosAuditoria(filePath, inputModel, s3);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                return Ok(ex);
            }
        }
        #endregion

        [HttpGet]
        [Route("ObtenerListadoDeEPS")]
        public async Task<IActionResult> ObtenerListadoDeEPS([FromQuery] int idCobertura)
        {
            try
            {
                var result = await _MedicionRepository.ObtenerListadoDeEPS(idCobertura);
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