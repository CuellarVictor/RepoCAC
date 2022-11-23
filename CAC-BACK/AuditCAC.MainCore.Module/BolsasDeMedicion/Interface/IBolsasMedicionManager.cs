using AuditCAC.Domain.Dto.BolsasMedicion;
using AuditCAC.Domain.Helpers;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuditCAC.MainCore.Module.BolsasDeMedicion.Interface
{
    public interface IBolsasMedicionManager
    {
        //Task<List<BolsasMedicionDto>> GetRegistrosAuditoria(MedicionRequest model);
        Task<Tuple<List<BolsasMedicionDto>, int, int, int>> GetRegistrosAuditoria(MedicionRequest model); //Task<List<BolsasMedicionDto>>
        Task<bool> AddOrUpdate(BolsaMedicionNuevaDto model);
    }
}
