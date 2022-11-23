using AuditCAC.Domain.Helpers;
using AuditCAC.MainCore.Module.BolsasDeMedicion.Interface;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using AuditCAC.Domain.Dto;


namespace AuditCAC.Services.Controllers.BolsasMedicion
{
    [Route("api/[controller]")]
    [ApiController]
    public class BolsasMedicionController : ControllerBase
    {
        #region Dependency
        private readonly IBolsasMedicionManager _getRegistros;
        #endregion

        #region Constructor
        public BolsasMedicionController(IBolsasMedicionManager getRegistros)
        {
            _getRegistros = getRegistros;
        }
        #endregion

        #region Methods
        [HttpPost]
        [Route("GetRegistros")]
        public async Task<IActionResult> Generate(MedicionRequest model)
        {
            //var Data = await _getRegistros.GetRegistrosAuditoria(model);
            //return Ok(Data);

            var Data = await _getRegistros.GetRegistrosAuditoria(model);
            //HttpContext.Response.Headers.Add("NoRegistrosTotales", Data.Item2);
            //HttpContext.Response.Headers.Add("TotalPages", Daa.Item3);

            var PaginationDto = new PaginationDto();
            PaginationDto.Data = Data.Item1;
            PaginationDto.NoRegistrosTotales = Data.Item2;
            PaginationDto.NoRegistrosTotalesFiltrado = Data.Item3;
            PaginationDto.TotalPages = Data.Item4;

            //return Ok(Data.Item1);
            return Ok(PaginationDto);

        }
        #endregion
    }
}
