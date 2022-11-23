using AuditCAC.Domain.Dto;
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
    [Route("api/TablasReferencial")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TablasReferencialController : ControllerBase
    {
        private readonly ITablasReferencialRepository<ResponseGetTablasReferencial> _repository;

        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public TablasReferencialController(ITablasReferencialRepository<ResponseGetTablasReferencial> Repository)
        {
            this._repository = Repository;
        }

        // POST: api/GetTablasReferencial
        /// <summary>
        /// Consultar la coleccion de Tablas refrenciales almacenadas en CAC.
        /// </summary>
        /// <remarks>
        /// Consultar la coleccion de Tablas refrenciales almacenadas en CAC. No recibe parametros.
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Configruacion ADO</returns>        
        [HttpPost]
        [Route("GetTablasReferencial")]
        public async Task<List<ResponseGetTablasReferencial>> GetTablasReferencial() //InputsGetTablasReferencialDto inputsDto
        {
            try
            {
                var result = await _repository.GetTablasReferencial();
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);
            }
        }

        // POST: api/GetTablasReferencialByValorReferencial
        /// <summary>
        /// Consultar la coleccion de Tablas refrenciales almacenadas en CAC.
        /// </summary>
        /// <remarks>
        /// Consultar la coleccion de Tablas refrenciales almacenadas en CAC. No recibe parametros.
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Configruacion ADO</returns>        
        [HttpPost]
        [Route("GetTablasReferencialByValorReferencial")]
        public async Task<List<ResponseGetTablasReferencialDto>> GetTablasReferencialByValorReferencial(List<InputsTablasReferencialByValorReferencialDto> entity)
        {
            try
            {
                var result = await _repository.GetTablasReferencialByValorReferencial(entity);
                return result;
            }
            catch (Exception ex)
            {
                throw new Exception("Error", ex);
            }
        }
    }
}
