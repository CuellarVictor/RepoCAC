using AuditCAC.Domain.Dto;
using AuditCAC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.MainCore.Module.Interface
{
    public interface IRegistrosAuditoriaDetalleSeguimientoRepository<TEntity>
    {
        Task<IEnumerable<RegistrosAuditoriaDetalleSeguimientoModel>> GetAll();
        Task<RegistrosAuditoriaDetalleSeguimientoModel> Get(int id);
        Task<List<ResponseRegistrosAuditoriaDetalleSeguimientoDto>> GetObservacionesByRegistroAuditoriaId(InputsRegistrosAuditoriaDetalleSeguimientoDto inputsDto);
        Task Add(RegistrosAuditoriaDetalleSeguimientoModel entity);
        Task Update(RegistrosAuditoriaDetalleSeguimientoModel dbEntity, RegistrosAuditoriaDetalleSeguimientoModel entity);
        Task Delete(RegistrosAuditoriaDetalleSeguimientoModel entity);

        Task RegistraObservacionTemporal(ResponseLlaveValor input);

        Task<ResponseLlaveValor> ConsultaObservacionTemporal(int id);
    }
}
