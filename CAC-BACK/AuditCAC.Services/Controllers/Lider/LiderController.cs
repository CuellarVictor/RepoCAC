using AuditCAC.MainCore.Module.Lider.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuditCAC.Services.Controllers.Lider
{
    [Route("api/[controller]")]
    [ApiController]
    public class LiderController : ControllerBase
    {
        #region Definiciones 
        private readonly ILiderManager _lider;
        #endregion

        #region Constructor 
        public LiderController(ILiderManager lider)
        {
            _lider = lider;
        }
        #endregion

        #region Endpoints
        [HttpGet]
        [Route("GetIssuesLeader/{idAuditor}")] 
        public async Task<IActionResult> GetIssuesLeader(string idAuditor)
        {
            var Data = await _lider.GetIssuesLider(idAuditor);
            return Ok(JsonConvert.SerializeObject(Data));
        }
        #endregion
    }
}
