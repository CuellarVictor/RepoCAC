using AuditCAC.Domain.Dto.Cobertura;
using AuditCAC.Domain.Entities.Cobertura;
using AuditCAC.MainCore.Module.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Net;
using System.Threading.Tasks;

namespace AuditCAC.Services.Controllers.Cobertura
{
    [Route("api/Cobertura")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]

    public class CoberturaController : ControllerBase
    {
        private readonly ICoberturaRepository<CoberturaXUsuarioModel> _CoberturaRepository;
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //Constructor
        public CoberturaController(ICoberturaRepository<CoberturaXUsuarioModel> CoberturaRepository)
        {
            this._CoberturaRepository = CoberturaRepository;
        }

        [HttpPost]
        public async Task<IActionResult> GetEnfermedadesMadreXUsuario([FromBody] InputCoberturaDto objCobertura)
        {
            try
            {
                var Data = await _CoberturaRepository.GetEnfermedadesMadreXUsuario(objCobertura);
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
