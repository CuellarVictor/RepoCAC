using AuditCAC.Domain.Dto.Catalogo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuditCAC.MainCore.Module.Catalogo.Interfaces
{
    public interface ICatalogoManager
    {
        Task<List<CatalogoDto>> GetAll();
        Task<List<CatalogoDto>> GetById(int id);
        Task<bool> AddOrUpdate(CatalogoDto model);
        Task<bool> Delete(int id);
    }
}
