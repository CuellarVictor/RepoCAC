using AuditCAC.Domain.Dto;
using AuditCAC.Domain.Entities;
using AuditCAC.MainCore.Module.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuditCAC.Services.Controllers
{
    [Route("api/Hallazgos")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class HallazgosController : ControllerBase
    {

        private readonly IHallazgosRepository<HallazgosModel> _IRepository;
        private readonly ILogger<HallazgosModel> _logger;

        public HallazgosController(IHallazgosRepository<HallazgosModel> IRepository, ILogger<HallazgosModel> logger)
        {
            this._IRepository = IRepository;
            _logger = logger;
        }


        /// <summary>
        /// Consutar Hallazgos por id de radicado
        /// </summary>
        /// <param name="inputsDto">Id radicado</param>
        /// <returns>MOdelo Hallazgos</returns>
        [HttpPost]
        [Route("ConsultarHallazgosByRadicadoId")]
        [AllowAnonymous]
        public async Task<List<ResponseConsultarHallazgosDto>> ConsultarHallazgosByRadicadoId(InputsConsultarHallazgosDto inputsDto)
        {
            try
            {
                var result = await _IRepository.ConsultarHallazgosByRadicadoId(inputsDto);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        /// <summary>
        /// Consulta de registros con hallazgos pendientes para enviar a la entidad por Id de Medicion (Suministro para implementacion de Modulo de hallazgos)
        /// </summary>
        /// <param name="inputsDto">(Medicion y Rango de Fechas)</param>
        /// <returns>Data para enviar a la entidad</returns>
        [HttpPost]
        [Route("ConsultaHallazgosGenerados")]
        [AllowAnonymous]
        public async Task<List<ResponseConsultaHallazgosGeneradosDto>> ConsultaHallazgosGenerados(InputsConsultaHallazgosGeneradosDto inputsDto)
        {
            try
            {
                var result = await _IRepository.ConsultaHallazgosGenerados(inputsDto);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        /// <summary>
        /// Registra respuestas de las entidades sobre los hallazgos
        /// </summary>
        /// <param name="inputsDto">RegistroAuditoriaId, Estado, Observacion, Usuario</param>
        /// <returns>true</returns>
        [HttpPost]
        [Route("RegistraRespuestaHallazgos")]
        public async Task<bool> RegistraRespuestaHallazgos(List<InputRegistraRespuestaHallazgosDto> inputsDto)
        {
            try
            {
                var result = await _IRepository.RegistraRespuestaHallazgos(inputsDto);
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
