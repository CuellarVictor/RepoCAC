using AuditCAC.Domain.Dto.CalificacionMasiva;
using AuditCAC.Domain.Dto.CarguePoblacion;
using AuditCAC.Domain.Entities;
using AuditCAC.MainCore.Module.BolsasDeMedicion.Interface;
using AuditCAC.MainCore.Module.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace AuditCAC.Services.Controllers
{
    [Route("api/cargue")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CargueController : ControllerBase
    {

        private readonly ILogger<CargueController> _logger;
        private readonly IMedicionRepository<MedicionModel> _MedicionRepository;
        private readonly IBolsasMedicionManager _medicion;
        private static IConfiguration Configuration;

        //Constructor
        public CargueController(IMedicionRepository<MedicionModel> MedicionRepository,
            ILogger<CargueController> logger,
            IBolsasMedicionManager medicion,
            IConfiguration configuration)

        {
            this._MedicionRepository = MedicionRepository;
            _medicion = medicion;
            Configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="inputModel"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("CargarArchivoPoblacion")]
        public async Task<IActionResult> CargarArchivoPoblacion([FromBody] InputCarguePoblacion inputModel)
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
                return Ok(inputModel);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                return Ok(ex);
            }

        }

        /// <summary>
        /// Genera plantilla para cargue de poblacion
        /// </summary>
        /// <param name="idMedicion">Id medicion</param>
        /// <param name="path">Ruta fisica</param>
        /// <returns>Archivo base 64</returns>
        [HttpGet]
        [Route("GenerarTemplateCarguePoblacion/{IdMedicion}/{idSubgrupo}")] // 
        public async Task<IActionResult> GenerarTemplateCarguePoblacion(int IdMedicion, int idSubgrupo) //
        {
            try
            {
                var path = Configuration["PathList:LoadPoblationPath"];

                var Data = await _MedicionRepository.GenerarTemplateCarguePoblacion(IdMedicion, path, idSubgrupo); //
                return Ok(JsonConvert.SerializeObject(Data));
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }


        #region Calificacion Masiva

        /// <summary>
        /// Crea Template Calificacion Masiva
        /// </summary>
        /// <param name="path">ruta fisica</param>
        /// <returns>string template base 64</returns>
        [HttpGet]
        [Route("CreaPlantillaCalificacionMasiva/{medicionId}")] // 
        public async Task<IActionResult> CreaPlantillaCalificacionMasiva(int medicionId) //
        {
            try
            {
                var path = Configuration["PathList:LoadPoblationPath"];

                var Data = await _MedicionRepository.CreaPlantillaCalificacionMasiva(path, medicionId); //
                return Ok(JsonConvert.SerializeObject(Data));
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }


        // <summary>
        /// Inicia proceso para cargue calificacion masiva registrandolo en la tabla current process
        /// </summary>
        /// <param name="filePath">Ruta del archivo a cargar</param>
        /// <param name="input">Modelo Cargue</param>
        /// <returns>Modelo Current Process</returns>
        [HttpPost]
        [Route("IniciaProcesoCalificacionMasiva")]
        public async Task<IActionResult> IniciaProcesoCalificacionMasiva([FromBody] InputCalificacionMasiva inputModel)
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
                var result = await _MedicionRepository.IniciaProcesoCalificacionMasiva(filePath, inputModel, s3);
                return Ok(inputModel);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                return Ok(ex);
            }

        }
        #endregion
        
    }
}
