using AuditCAC.Domain.Dto;
using AuditCAC.MainCore.Module.Trazabilidad.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;

namespace AuditCAC.Services.Controllers.Trazabilidad
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrosAuditoriaLogController : ControllerBase
    {
        #region Dependency
        private readonly IRegistroAuditoriaLogManager _getRegistros;
        #endregion

        #region Constructor
        public RegistrosAuditoriaLogController(IRegistroAuditoriaLogManager getRegistros)
        {
            _getRegistros = getRegistros;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Consuta registros de log auditoria
        /// </summary>
        /// <param name="inputsDto">Modelo parametros de consulta</param>
        /// <returns>Modelo Registros Auditoria Log</returns>
        [HttpPost]
        [Route("ConsultaLogAccion")]
        public async Task<IActionResult> ConsultaLogAccion([FromBody] InputsGetRegistrosAuditoriaLogDto inputsDto)
        {
            var Data = await _getRegistros.ConsultaLogAccion(inputsDto);

            return Ok(Data);
        }
        #endregion
    }
}
