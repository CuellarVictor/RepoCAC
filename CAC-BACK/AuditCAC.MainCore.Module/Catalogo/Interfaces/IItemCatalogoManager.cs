using AuditCAC.Domain.Dto.Catalogo;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuditCAC.MainCore.Module.Catalogo.Interfaces
{
    public interface IItemCatalogoManager
    {
        Task<List<ItemDto>> GetAll();
        Task<List<ItemDto>> GetById(int id);
        Task<bool> AddOrUpdate(ItemDto model);
        Task<bool> Delete(int id);
    }
}
