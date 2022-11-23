using AuditCAC.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuditCAC.Dal.Data;

namespace AuditCAC.Domain.Helpers
{
    public class ValidarErroresRegistroAuditorias
    {
        //private readonly DBAuditCACContext _dBAuditCACContext;

        //Contructor
        public ValidarErroresRegistroAuditorias(DBAuditCACContext dBAuditCACContext)
        {
            //this._dBAuditCACContext = dBAuditCACContext;
        }

        //Metodos.

        //Validamos errores de logica presentes en solicitud (Estandar) y retornamos Listado de RegistroAuditoriaDetalle, incluyendo errores.
        public async Task<List<ResponseRegistrosAuditoriaFiltrosDto>> Validate(InputsCambiarEstadoRegistroAuditoriaDto inputsDto)
        {
            //Capturamos datos a utilizar.
            var RegistroAuditoriaId = inputsDto.IdEstadoNuevo;
            var IdEstadoNuevo = inputsDto.IdEstadoNuevo;

            //Validamos si la variable ya tiene error registrado en RegistroAuditoriaDetalleError para no registrarlo ni validarlo nuevamente.
            var RegistrosAuditoria = await _dBAuditCACContext.RegistrosAuditoriaModel.FirstOrDefaultAsync(x => x.Id == id);

            //SI en RegistroAuditoriaDetalleError existe un error, no ejecutar este paso Logica de validacion del error.

            //Si se encuentra un error, es guardado en RegistroAuditoriaDetalleError.


            return Ok();
        }

        //Validamos errores de logica presentes en solicitud (Masiva) y retornamos Listado de RegistroAuditoriaDetalle, incluyendo errores.
        public async Task<List<ResponseRegistrosAuditoriaFiltrosDto>> ValidateM(InputsActualizarDC_NC_ND_MotivoDto inputsDto)
        {
            //Validamos si la variable ya tiene error registrado en RegistroAuditoriaDetalleError.


            //Logica de validacion del error.

            //Si se encuentra un error, es guardado en RegistroAuditoriaDetalleError.


            return Ok();
        }
    }
}
