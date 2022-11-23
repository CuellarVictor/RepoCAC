using AuditCAC.Domain.Helpers;
using AuditCAC.MainCore.Module.BolsasDeMedicion.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AuditCAC.Domain.Dto;
using AuditCAC.MainCore.Module;
using Microsoft.Extensions.Logging;
using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using AuditCAC.MainCore.Module.Interface;
using AuditCAC.Domain.Dto.Actas;

namespace AuditCAC.Services.Controllers.ActaController
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ActaController : ControllerBase
    {

        private readonly IActaManager<ParametroTemplateDto> _IRepository;
        private readonly ILogger<ParametroTemplateDto> _logger;

        //Constructor
        public ActaController(IActaManager<ParametroTemplateDto> IRepository, ILogger<ParametroTemplateDto> logger)
        {
            this._IRepository = IRepository;
            _logger = logger;
        }

        [HttpGet]
        [Route("ConsultaParametrosTemplate")]
        public async Task<IActionResult> ConsultaParametrosTemplate()
        {
            try
            {
                var result = await _IRepository.ConsultaParametrosTemplate();
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                return Ok(ex);
            }

        }

        /// <summary>
        /// Crea o actualiza parametro de los templates
        /// </summary>
        /// <param name="input">Modelo parametro</param>
        /// <returns>true</returns>
        [HttpPost]
        [Route("UpsertParametroTemplate")]
        public async Task<IActionResult> UpsertParametroTemplate([FromBody] ParametroTemplateDto input)
        {
            try
            {
                var result = await _IRepository.UpsertParametroTemplate(input);
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                return Ok(ex);
            }

        }

        #region Actas

        [HttpPost]
        [Route("GenerarActa")]
        public async Task<IActionResult> GenerarActa([FromBody] GenerarActaInputDto input)
        {
            try
            {
                string result = "";
                if (input.IdTemplate == (int)Enumeration.Actas.Template.ActaApertura)
                {
                    result = await _IRepository.GenerarActaApertura(input);
                }
                if (input.IdTemplate == (int)Enumeration.Actas.Template.ActaCierre)
                {
                    result = await _IRepository.GenerarActaCierre(input);
                }

                var response = new ResponseLlaveValor();
                response.Valor = result;
                return Ok(response);
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
