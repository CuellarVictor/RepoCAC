using AuditCAC.Domain.Dto;
using AuditCAC.Domain.Entities;
using AuditCAC.MainCore.Module.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuditCAC.CoreCAC.Services.Controllers
{
    [Route("api/MoverRegistrosPascientes")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class MoverRegistrosAuditoriaPascientesController : ControllerBase
    {
        private readonly ITransferenciaRegistrosRepository<NemonicoPascientesConsultaModel> _IRepository;

        public MoverRegistrosAuditoriaPascientesController(ITransferenciaRegistrosRepository<NemonicoPascientesConsultaModel> IRepository)
        {
            this._IRepository = IRepository;
        }
        // POST: api/RegistrosAuditoria/MoverRegistrosAuditoria
        /// <summary>
        /// Consulta el listado de los pascientes .
        /// </summary>
        /// <remarks>
        /// Recibe los id de los pascientes y el id de medicion.
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Listado de pascientes</returns>
        [HttpPost]
        [Route("ConsultarRegistrosAuditoriaPascientes")]
        public async Task<object> ConsultarRegistrosAuditoriaPascientes([FromBody] InputsMoverRegistrosAuditoriaPascientesDto inputs)
        {
            try
            {
                var Data = await _IRepository.ConsultarRegistrosAuditoriaPascientes(inputs);
                return Data;
            }
            catch (Exception ex)
            {                
                throw new Exception("Error", ex);
            }
        }


        // POST: api/RegistrosAuditoria/MoverRegistrosAuditoria
        /// <summary>
        /// Consulta el listado de los pascientes .
        /// </summary>
        /// <remarks>
        /// Recibe los id de los pascientes y el id de medicion.
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Listado de pascientes</returns>
        [HttpPost]
        [Route("InsertarRegistrosAuditoriaPascientes")]
        public async Task<IActionResult> InsertarRegistrosAuditoriaPascientes([FromBody] List<NemonicoPascientesConsultaModel> inputs)
        {
            try
            {
                await _IRepository.InsertarRegistrosAuditoriaPascientes(inputs);
                return Ok();
            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);
            }
        }        
    }
}
