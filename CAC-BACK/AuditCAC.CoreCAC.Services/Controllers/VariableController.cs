using AuditCAC.Dal.Entities;
using AuditCAC.Domain.Dto;
using AuditCAC.Domain.Entities;
using AuditCAC.MainCore.Module.Interface;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.ObjectRenderer;
using log4net.Plugin;
using log4net.Repository;
using log4net.Util;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.IO;
using System.Threading.Tasks;

namespace AuditCAC.CoreCAC.Services.Controllers
{
    //[Route("api/procedures")]
    [Route("api/Variable")]    
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class VariableController : ControllerBase
    {
        private readonly IVariableRepository<VariableModel> _repository;

        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //Constructor.
        public VariableController(IVariableRepository<VariableModel> Repository)
        {
            this._repository = Repository;
        }

        // POST: api/Variable
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
        //[Route("Variable")]
        public async Task<IActionResult> Variable([FromBody] InputsVariableDto inputsVariableDto)
        {
            try
            {
                var Data = await _repository.GetCacvariable(inputsVariableDto);
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
