using AuditCAC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.MainCore.Module.Interface
{
    public interface IItemRepository<TEntity>
    {
        Task<IEnumerable<ItemModel>> GetAll();
        Task<ItemModel> Get(int id);
        Task<IEnumerable<ItemModel>> GetItemByCatalogId(int CatalogId);
        Task Add(ItemModel entity, string UsuarioId);
        Task Update(ItemModel entity, string UsuarioId);
        Task Delete(ItemModel entity, string UsuarioId);
    }
}
