using AuditCAC.Dal.Data;
using AuditCAC.Domain.Dto.ReglaVariable;
using AuditCAC.Domain.Entities.ReglaVariable;
using AuditCAC.MainCore.Module.ReglaVariable.Interfaces;
using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.MainCore.Module.ReglaVariable
{
    public class ReglaVariableManager : IReglaVariableManager
    {
        #region Dependency
        private readonly DBAuditCACContext _context;
        private readonly IMapper _map;
        #endregion

        #region Constructor
        public ReglaVariableManager(DBAuditCACContext context, IMapper map)
        {
            _context = context;
            _map = map;
        }
        #endregion

        #region Methods
        public async Task<List<ReglasVariablesDto>> GetAll()
        {
            var res = _context.ReglaVariable.ToList();
            var map = _map.Map<List<ReglasVariablesDto>>(res);
            return await Task.FromResult(map != null ? map : null);
        }

        public async Task<List<ReglasVariablesDto>> GetById(int id)
        {
            var res = _context.ReglaVariable.Where(x => x.IdRegla == id).ToList();
            var map = _map.Map<List<ReglasVariablesDto>>(res);
            return await Task.FromResult(map != null ? map : null);
        }

        public async Task<bool> AddOrUpdate(ReglasVariablesDto model)
        {
            var map = _map.Map<ReglasVariableModel>(model);
            if (model.IdRegla != 0)
            {

                var res = _context.ReglaVariable.Add(map);
                return await Task.FromResult(res.State != 0 ? true : false);
            }
            else
            {
                var res = _context.ReglaVariable.Update(map);
                return await Task.FromResult(res.State != 0 ? true : false);
            }
        }

        public async Task<bool> Delete(int id)
        {
            var data = _context.ReglaVariable.Where(x => x.IdRegla == id).FirstOrDefault();
            
            return await Task.FromResult(1 != 0 ? true : false);
        }
        #endregion
    }
}
