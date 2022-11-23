using AuditCAC.Domain.Dto;
using AuditCAC.Domain.Entities;
using AuditCAC.MainCore.Module.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AuditCAC.CoreCAC.Services.Controllers
{
    //[Route("api/procedures")]
    [Route("api/Periodo")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PeriodoController : ControllerBase
    {
        private readonly IPeriodoRepository<PeriodoModel> _repository;

        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //Constructor.
        public PeriodoController(IPeriodoRepository<PeriodoModel> Repository)
        {
            this._repository = Repository;
        }

        // POST: api/Periodo
        /// <summary>
        /// Consultar todos los registros disponibles con filtros.
        /// </summary>
        /// <remarks>
        /// Regresa todos los registros almacenados. Permite el filtrado por cada campo del registro.
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Configruacion ADO</returns>
        [HttpPost]        
        //[Route("Periodo")]
        public async Task<IActionResult> Periodo([FromBody] InputsPeriodoDto inputsPeriodo)
        {
            try
            {
                var Data = await _repository.Getcacperiodo(inputsPeriodo);
                return Ok(Data);
            }
            catch (Exception ex)
            {

                _log.Fatal("Fatal", ex);
                throw new Exception("Error", ex);
            }

        }
    }
}
