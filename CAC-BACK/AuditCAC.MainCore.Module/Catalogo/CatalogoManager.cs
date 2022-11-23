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
    public class CatalogoManager : ICatalogoManager
    {
        #region Dependencias
        private readonly DBAuditCACContext _context;
        private readonly IMapper _mapper;
        private readonly IItemCatalogoManager _itemCatalogoManager;
        #endregion

        #region Constructor
        public CatalogoManager(DBAuditCACContext context, IMapper mapper, IItemCatalogoManager item)
        {
            _context = context;
            _mapper = mapper;
            _itemCatalogoManager = item;
        }
        #endregion

        #region Methods
        public async Task<List<CatalogoDto>> GetAll()
        {

            var result = _context.CatalogModel.Where(x => x.Status == true).ToList();
            var map = _mapper.Map<List<CatalogoDto>>(result);
            foreach (var item in map)
            {
                var itemOne = _context.ItemModel.Where(c => c.CatalogId == item.Id && c.Status == true).ToList();
                var itemsMap = _mapper.Map<List<ItemDto>>(itemOne);
                item.Items = itemsMap;
            }

            return await Task.FromResult(map);
        }

        public async Task<List<CatalogoDto>> GetById(int id)
        {

            var result = _context.CatalogModel.Where(x => x.Id == id && x.Status == true).ToList();
            var map = _mapper.Map<List<CatalogoDto>>(result);
            foreach (var item in map)
            {
                var itemOne = _context.ItemModel.Where(c => c.CatalogId == item.Id && c.Status == true).ToList();
                var itemsMap = _mapper.Map<List<ItemDto>>(itemOne);
                item.Items = itemsMap;
            }
            return await Task.FromResult(map);
        }

        public async Task<bool> AddOrUpdate(CatalogoDto model)
        {
            if (model.Id != 0)
            {
                var result = _context.CatalogModel.Where(x => x.Id == model.Id).FirstOrDefault();
                if (result != null)
                {
                    result.LastModify = DateTime.Now;
                    result.CatalogName = model.CatalogName;
                    _context.CatalogModel.Update(result);
                    await _context.SaveChangesAsync();
                    if (model.Items.Count != 0)
                    {
                        foreach (var item in model.Items)
                        {
                            item.CatalogId = Convert.ToInt32(model.Id);
                            await _itemCatalogoManager.AddOrUpdate(item);
                        }
                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                var map = _mapper.Map<CatalogModel>(model);
                map.LastModify = DateTime.Now;
                map.CreateDate = DateTime.Now;
                var result = _context.CatalogModel.Add(map);
                if (result != null)
                {
                    await _context.SaveChangesAsync();
                    if (model.Items.Count != 0)
                    {
                        foreach (var item in model.Items)
                        {
                            item.CatalogId = map.Id;
                            await _itemCatalogoManager.AddOrUpdate(item);
                        }

                    }
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        public async Task<bool> Delete(int id)
        {
            var entidad = _context.CatalogModel.Where(x => x.Id == id).FirstOrDefault();
            if (entidad != null)
            {
                entidad.Status = false;
                var result = _context.CatalogModel.Update(entidad);
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
