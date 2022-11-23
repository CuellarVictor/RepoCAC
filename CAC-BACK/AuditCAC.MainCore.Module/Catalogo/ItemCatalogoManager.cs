using AuditCAC.Dal.Data;
using AuditCAC.Domain.Dto.Catalogo;
using AuditCAC.Domain.Entities;
using AuditCAC.MainCore.Module.Catalogo.Interfaces;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuditCAC.MainCore.Module.Catalogo
{
    public class ItemCatalogoManager : IItemCatalogoManager
    {
        #region Dependencias
        private readonly DBAuditCACContext _context;
        private readonly IMapper _mapper;
        #endregion

        #region Constructor
        public ItemCatalogoManager(DBAuditCACContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        #endregion

        #region Methods
        public async Task<List<ItemDto>> GetAll()
        {

            var result = _context.ItemModel.Where(x => x.Status == true).ToList();
            var map = _mapper.Map<List<ItemDto>>(result);
            return await Task.FromResult(map);
        }

        public async Task<List<ItemDto>> GetById(int id)
        {

            var result = _context.ItemModel.Where(x => x.CatalogId == id && x.Status == true).ToList();
            var map = _mapper.Map<List<ItemDto>>(result);
            return await Task.FromResult(map);
        }

        public async Task<bool> AddOrUpdate(ItemDto model)
        {
            try
            {
                if (model.Id != 0)
                {
                    var itemUpdate = _context.ItemModel.Where(x => x.Id == model.Id).FirstOrDefault();
                    var map = _mapper.Map<ItemModel>(itemUpdate);
                    map.LastModify = DateTime.Now;
                    map.ItemName = model.ItemName;
                    var res = _context.ItemModel.Update(map);
                    await _context.SaveChangesAsync();
                    return res.State != 0 ? true : false;
                }
                else
                {
                    var map = _mapper.Map<ItemModel>(model);
                    map.CreateDate = DateTime.Now;
                    map.LastModify = DateTime.Now;
                    var res = _context.ItemModel.Add(map);
                    await _context.SaveChangesAsync();
                    return res.State != 0 ? true : false;
                }
            }
            catch (Exception e)
            {

                throw;
            }
        }

        public async Task<bool> Delete(int id)
        {
            var entidad = _context.ItemModel.Where(x => x.Id == id).FirstOrDefault();
            if (entidad != null)
            {
                entidad.Status = false;
                var result = _context.ItemModel.Update(entidad);
                await _context.SaveChangesAsync();
                return true;
            }
            else
            {
                return false;
            }
        }
        #endregion
    }
}

