using AuditCAC.Domain.Dto;
using AuditCAC.Domain.Entities;
using AuditCAC.Domain.Helpers;
using AuditCAC.MainCore.Module.BolsasDeMedicion.Interface;
using AuditCAC.MainCore.Module.Interface;
using AuditCAC.MainCore.Module.Trazabilidad;
using AuditCAC.MainCore.Module.Trazabilidad.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AuditCAC.Services.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GenerateExcelController : ControllerBase
    {
        private readonly IGenerateExcel _generate;
        private readonly IHostingEnvironment Environment;
        private readonly IConfiguration _config;
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IRegistrosAuditoriaRepository<RegistrosAuditoriaModel> _IRepositoryRegistroAuditoria;
        private readonly IMedicionRepository<MedicionModel> _MedicionRepository;
        private readonly IRegistroAuditoriaLogManager _RegistroAuditoriaLogManager;
        public GenerateExcelController(IGenerateExcel generate, IHostingEnvironment _environment, IConfiguration config, IRegistrosAuditoriaRepository<RegistrosAuditoriaModel>  registrosAuditoriaRepository, IMedicionRepository<MedicionModel> MedicionRepository, IRegistroAuditoriaLogManager registroAuditoriaLogManager)
        {
            _generate = generate;
            Environment = _environment;
            _config = config;
            _IRepositoryRegistroAuditoria = registrosAuditoriaRepository;
            this._MedicionRepository = MedicionRepository;
            _RegistroAuditoriaLogManager = registroAuditoriaLogManager;
        }

        /// <summary>
        /// Consulta info y genera archivo para reporte
        /// </summary>
        /// <param name="inputsDto">Modelo de consulta filtrado</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Generate")]
        public async Task<IActionResult> Generate([FromBody] InputsRegistrosAuditoriaFiltradoDto inputsDto)
        {
            try
            {
                var ruta = _config["PathList:LoadPoblationPath"];

                var dataGenerate = await _IRepositoryRegistroAuditoria.GetRegistrosAuditoriaFiltrado(inputsDto);

                var Data = await _generate.BuildReportFile(dataGenerate.Item1, ruta);
                return Ok(JsonConvert.SerializeObject(Data));
            }
            catch (Exception ex)
            {
                _log.Fatal("Fatal", ex);
                throw;
            }
            
        }

        /// <summary>
        /// Consulta info y genera archivo para reporte
        /// </summary>
        /// <param name="inputsDto">Modelo de consulta filtrado</param>
        /// <param name="enumReport"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("GenerateRegistrosAuditoriaXBolsaMedicion")]
        public async Task<IActionResult> GenerateRegistrosAuditoriaXBolsaMedicion([FromBody] InputsGetRegistrosAuditoriaXBolsaMedicionDto inputsDto, int enumReport)
        {
            try
            {
                var ruta = _config["PathList:LoadPoblationPath"];
                
                var dataGenerate = await _MedicionRepository.GetRegistrosAuditoriaXBolsaMedicion(inputsDto);

                var Data = await _generate.BuildMedicionesReportFile(dataGenerate.Item1, ruta, enumReport);
                return Ok(JsonConvert.SerializeObject(Data));
            }
            catch (Exception ex)
            {
                _log.Fatal("Fatal", ex);
                throw;
            }
        }

        [HttpGet]
        [Route("Download/{fileName}")]
        public async Task<IActionResult> Download(string fileName)
        {
            try
            {
                if (fileName.Length != 0)
                {
                    var ruta = _config["PathList:LoadPoblationPath"];
                    string path = Path.Combine(ruta) + fileName;

                    byte[] bytes = System.IO.File.ReadAllBytes(path);

                    return Ok(File(bytes, "application/octet-stream", fileName));
                }
                else
                {
                    throw new FileNotFoundException();
                }
            }
            catch (Exception ex)
            {
                _log.Fatal("Fatal", ex);
                throw;
            }
            
        }


        [HttpPost]
        [Route("GeneraFileLogAccion")]
        public async Task<IActionResult> GeneraFileLogAccion([FromBody] InputsGetRegistrosAuditoriaLogDto model)
        {
            try
            {
                var ruta = _config["PathList:LoadPoblationPath"];

                var dataGenerate = await _RegistroAuditoriaLogManager.ConsultaLogAccion(model);

                var Data = await _generate.BuildReportFileLogAccion(dataGenerate, ruta);
                return Ok(JsonConvert.SerializeObject(Data));
            }
            catch (Exception ex)
            {
                _log.Fatal("Fatal", ex);
                throw;
            }

        }
    }
}
