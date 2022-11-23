using ApiAuditCAC.Core.MongoCollection;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiAuditCAC.Services.Controllers
{
    /// <summary>
    /// Api AuditCac
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class ApiAuditCacController : ControllerBase
    {
        private readonly IMongoCollection _mongoCollection;

        public ApiAuditCacController( IMongoCollection mongoCollection) {
            _mongoCollection = mongoCollection;
        }

        /// <summary>
        ///  Metodo para recibir las peticiones de aldeamo
        /// </summary>
        /// <remarks>
        /// El módelo recibe los siguientes valores: <br/>
        /// Mobile: Nro celular origen del mensaje <br/>
        /// Message: Cuerpo mensaje de texto <br/>
        /// </remarks>
        /// <response code="200">Proceso exitoso</response>
        /// <response code="400">Error de validación en los parámetros de entrada</response>
        /// <response code="500">Excepción no controlada</response> 
        [HttpPost]
        [Route("[action]")]
        [ProducesResponseType(typeof(int), 200)]
        [ProducesResponseType(typeof(void), 500)]
        //public async Task<IActionResult> CreateCollection([FromBody] Dictionary<String, String> data)
        public async Task<IActionResult> CreateCollection()
        {
            var result = await _mongoCollection.CreateCollection("");
            
            return Ok(result);
        }

    }
}
