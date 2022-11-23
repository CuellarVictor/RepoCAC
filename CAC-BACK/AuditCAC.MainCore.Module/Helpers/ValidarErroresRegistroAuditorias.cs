using AuditCAC.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuditCAC.Dal.Data;
using Microsoft.EntityFrameworkCore;
using AuditCAC.Domain.Entities;

namespace AuditCAC.MainCore.Module.Helpers
{
    public class ValidarErroresRegistroAuditorias
    {
        private readonly DBAuditCACContext _dBAuditCACContext;

        //Contructor
        public ValidarErroresRegistroAuditorias(DBAuditCACContext dBAuditCACContext)
        {
            this._dBAuditCACContext = dBAuditCACContext;
        }

        //Metodos.

        ////Validamos errores de logica presentes en solicitud (Estandar) y retornamos Listado de RegistroAuditoriaDetalle, incluyendo errores.
        //public async Task<List<ResponseRegistrosAuditoriaFiltrosDto>> Validate(InputsCambiarEstadoRegistroAuditoriaDto inputsDto)
        //{
        //    //Capturamos datos a utilizar.
        //    var RegistroAuditoriaId = inputsDto.RegistroAuditoriaId;
        //    var IdEstadoNuevo = inputsDto.IdEstadoNuevo;

        //    //Validamos si la variable ya tiene error registrado en RegistroAuditoriaDetalleError para no registrarlo ni validarlo nuevamente.
        //    var RegistroAuditoriaDetalleError = await _dBAuditCACContext.RegistroAuditoriaDetalleErrorModel.FirstOrDefaultAsync(x => x.RegistrosAuditoriaDetalleId == RegistroAuditoriaId);

        //    //SI en RegistroAuditoriaDetalleError existe un error, no ejecutar este paso Logica de validacion del error.

        //    //Si se encuentra un error, es guardado en RegistroAuditoriaDetalleError.


        //    return Ok();
        //}

        ////Validamos errores de logica presentes en solicitud (Masiva) y retornamos Listado de RegistroAuditoriaDetalle, incluyendo errores.
        //public async Task<List<ResponseRegistrosAuditoriaFiltrosDto>> Validate(InputsActualizarDC_NC_ND_MotivoDto inputsDto)
        //{
        //    //Declaramos variables usadas.
        //    var ErrorInsertar = new RegistroAuditoriaDetalleErrorModel();
        //    var RegistroAuditoriaDetalleId = "";
        //    //var Dato_DC_NC_ND = "";
        //    //var Motivo = "";

        //    //Recorremos objeto y capturamos datos a utilizar.
        //    var ListCalificaciones = inputsDto.ListadoCalificaciones; //[0].variables;
        //    foreach (var itemListCalificaciones in ListCalificaciones)
        //    {
        //        foreach (var itemVariables in itemListCalificaciones.variables)
        //        {
        //            //Capturamos datos a utilizar.
        //            //Dato_DC_NC_ND = (itemVariables.Dato_DC_NC_ND != null) ? itemVariables.Dato_DC_NC_ND.ToString() : "NULL"; //Si viene Null, guardar un Null.
        //            //Motivo = (itemVariables.motivo != null) ? itemVariables.motivo.ToString() : "";
        //            RegistroAuditoriaDetalleId = itemVariables.registroAuditoriaDetalleId.ToString();

        //            //Validamos si la variable ya tiene error registrado en RegistroAuditoriaDetalleError para no registrarlo ni validarlo nuevamente.
        //            var RegistroAuditoriaDetalleError = await _dBAuditCACContext.RegistroAuditoriaDetalleErrorModel.FirstOrDefaultAsync(x => x.RegistrosAuditoriaDetalleId == int.Parse(RegistroAuditoriaDetalleId));

        //            //Validamos si ya existe un error para la variable. SI ya existe un error, no ejecutar este paso Logica de validacion del error.
        //            if (RegistroAuditoriaDetalleError != null)
        //            {
        //                //Evaluar registro mediante logica suministrada. En caso de tener error, es guardado en RegistroAuditoriaDetalleError. 
        //                //
        //                // ---

        //                //Validamos si tiene error. En ese caso, almacenamos el error.
        //                var HaveError = await _dBAuditCACContext.RegistrosAuditoriaDetalleModel.FirstOrDefaultAsync(x => x.RegistrosAuditoriaId == int.Parse(RegistroAuditoriaDetalleId)); //Logica temporal.
        //                if (HaveError.Ara == true)
        //                {
        //                    //Armamos objeto de RegistroAuditoriaDetalleError con detalles de errores.
        //                    //var ErrorInsertar = new RegistroAuditoriaDetalleErrorModel();
        //                    //ErrorInsertar.Id = 0;
        //                    ErrorInsertar.RegistrosAuditoriaDetalleId = int.Parse(RegistroAuditoriaDetalleId);
        //                    ErrorInsertar.ErrorId = "Id del Error";
        //                    ErrorInsertar.Descripcion = "Descripcion del error";
        //                    ErrorInsertar.Estado = 1;
        //                    ErrorInsertar.CreatedBy = "UserId";
        //                    ErrorInsertar.CreatedDate = DateTime.Now;
        //                    ErrorInsertar.ModifyBy = "UserId";
        //                    ErrorInsertar.ModifyDate = DateTime.Now;

        //                    //_dBAuditCACContext.RegistroAuditoriaDetalleErrorModel.Add(ErrorInsertar);
        //                    //await _dBAuditCACContext.SaveChangesAsync();
        //                }                        
        //            }
        //        }
        //    }

        //    //Insertamos en RegistroAuditoriaDetalleError el error
        //    _dBAuditCACContext.RegistroAuditoriaDetalleErrorModel.Add(ErrorInsertar);
        //    await _dBAuditCACContext.SaveChangesAsync();

        //    //Consultamos RegistroAuditoriaDetalle, debe incluir los errores. MODIFICAR MODELO.


        //    return Ok();
        //}
    }
}
