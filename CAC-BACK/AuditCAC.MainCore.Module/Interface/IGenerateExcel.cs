using AuditCAC.Domain.Dto;
using AuditCAC.Domain.Dto.BolsasMedicion;
using AuditCAC.Domain.Dto.RegistroAuditoria;
using AuditCAC.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuditCAC.MainCore.Module.Interface
{
    public interface IGenerateExcel
    {
        Task<string> BuildReportFile(List<ResponseRegistrosAuditoriaModelDto> data, string ruta);
        Task<string> BuildMedicionesReportFile(List<ResponseRegistrosAuditoriaXBolsaMedicionDto> data, string ruta, int enumReport);
        Task<string> ReporteMoverRegistrosAuditoriaBolsaMedicionData(List<ResponseMoverTodosRegistrosAuditoriaBolsaMedicionDto> dataResult, string ruta, bool camposError = false);        
        Task<string> ReporteCargarBancoInformacionPlantillaData(List<ResponseCargarBancoInformacionPlantillaDto> dataResult, string ruta);
        Task<string> EliminarRegistrosAuditoriaPlantillaData(List<ResponseEliminarRegistrosAuditoriaPlantillaDto> dataResult, string ruta);

        #region Log Accion

        Task<string> BuildReportFileLogAccion(List<RegistrosAuditoriaLogModel> data, string ruta);

        #endregion
     }
}
