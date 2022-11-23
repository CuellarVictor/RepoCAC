using AuditCAC.Dal.Data;
using AuditCAC.Domain.Dto;
using AuditCAC.Domain.Entities;
using AuditCAC.MainCore.Module.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.MainCore.Module
{
    public class RegistrosAuditoriaDetalleSeguimientoManager : IRegistrosAuditoriaDetalleSeguimientoRepository<RegistrosAuditoriaDetalleSeguimientoModel>
    {
        private readonly DBAuditCACContext _dBAuditCACContext;
        private readonly ILogger<RegistrosAuditoriaDetalleSeguimientoModel> _logger;

        //Constructor
        public RegistrosAuditoriaDetalleSeguimientoManager(DBAuditCACContext dBAuditCACContext, ILogger<RegistrosAuditoriaDetalleSeguimientoModel> logger)
        {
            this._dBAuditCACContext = dBAuditCACContext;
            _logger = logger;
        }

        //Metodos.
        public async Task<IEnumerable<RegistrosAuditoriaDetalleSeguimientoModel>> GetAll()
        {
            try
            {
                return await _dBAuditCACContext.RegistrosAuditoriaDetalleSeguimientoModel.Where(x => x.Status == true).ToListAsync();
                //return await this.dBAuditCACContext.RegistrosAuditoriaDetalleSeguimientoModel.ToListAsync();
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        public async Task<RegistrosAuditoriaDetalleSeguimientoModel> Get(int id)
        {
            try
            {
                return await _dBAuditCACContext.RegistrosAuditoriaDetalleSeguimientoModel.Where(x => x.Status == true).FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        //Metodo para llamar procedure RegistrosAuditoriaAsignadoAuditorEstado - Para la consulta de los registros asignados a un auditor y a un estado      
        public async Task<List<ResponseRegistrosAuditoriaDetalleSeguimientoDto>> GetObservacionesByRegistroAuditoriaId(InputsRegistrosAuditoriaDetalleSeguimientoDto inputsDto)
        {
            try
            {
                string sql = "EXEC [dbo].[GetObservacionesByRegistroAuditoriaId] @RegistroAuditoriaId";

                List<SqlParameter> parms = new List<SqlParameter>
                {
                    //new SqlParameter { ParameterName = "@PageNumber", Value = inputsDto.PageNumber},
                    //new SqlParameter { ParameterName = "@MaxRows", Value = inputsDto.MaxRows},
                    //                    
                    new SqlParameter { ParameterName = "@RegistroAuditoriaId", Value = inputsDto.RegistroAuditoriaId},

                };

                var Data = await _dBAuditCACContext.ResponseRegistrosAuditoriaDetalleSeguimientoDto.FromSqlRaw<ResponseRegistrosAuditoriaDetalleSeguimientoDto>(sql, parms.ToArray()).ToListAsync();
                return Data;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        public async Task Add(RegistrosAuditoriaDetalleSeguimientoModel entity)
        {
            try
            {
                _dBAuditCACContext.RegistrosAuditoriaDetalleSeguimientoModel.Add(entity);
                await _dBAuditCACContext.SaveChangesAsync();


                ResponseLlaveValor clear = new ResponseLlaveValor()
                {
                    Id = Convert.ToInt32(entity.RegistroAuditoriaId),
                    Valor = ""
                };
                await RegistraObservacionTemporal(clear);
                //return NoContent;

                //Llamamos SP para Insertar Logs de actividad.                
                string sql = "EXEC [SP_Insertar_Process_Log] @ProcessId, @User, @Result, @Observation";
                List<SqlParameter> parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@ProcessId", Value = 17},
                    new SqlParameter { ParameterName = "@User", Value = entity.CreatedBy},
                    new SqlParameter { ParameterName = "@Result", Value = "OK"},
                    new SqlParameter { ParameterName = "@Observation", Value = "Cronograma: Auditar (Registrar Observación)"}
                };

                var respuesta = _dBAuditCACContext.Database.ExecuteSqlRaw(sql, parms.ToArray());
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        public async Task Update(RegistrosAuditoriaDetalleSeguimientoModel dbEntity, RegistrosAuditoriaDetalleSeguimientoModel entity)
        {
            try
            {
                //dbEntity.Id = entity.Id;
                //dbEntity.RegistroAuditoriaId = entity.RegistroAuditoriaId;

                dbEntity.TipoObservacion = entity.TipoObservacion;
                dbEntity.Observacion = entity.Observacion;                
                dbEntity.Soporte = entity.Soporte;
                dbEntity.EstadoActual = entity.EstadoActual;
                dbEntity.EstadoNuevo = entity.EstadoNuevo;
                //dbEntity.CreatedBy = entity.CreatedBy;
                //dbEntity.CreatedDate = entity.CreatedDate;               
                dbEntity.ModifyBy = entity.ModifyBy;
                dbEntity.ModifyDate = entity.ModifyDate;

                await _dBAuditCACContext.SaveChangesAsync();
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        public async Task Delete(RegistrosAuditoriaDetalleSeguimientoModel entity)
        {
            try
            {
                //_dBAuditCACContext.RegistrosAuditoriaDetalleSeguimientoModel.Remove(entity);
                //await _dBAuditCACContext.SaveChangesAsync();

                entity.Status = false;

                await _dBAuditCACContext.SaveChangesAsync();
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }


        public async Task RegistraObservacionTemporal(ResponseLlaveValor input)
        {
            try
            {

                //return NoContent;

                //Llamamos SP para Insertar Logs de actividad.                
                string sql = "EXEC [SP_Upsert_Observacion_Temporal] @RegistroAuditoriaId, @Observacion";
                List<SqlParameter> parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@RegistroAuditoriaId", Value = input.Id},
                    new SqlParameter { ParameterName = "@Observacion", Value = input.Valor}
                };

                var respuesta = _dBAuditCACContext.Database.ExecuteSqlRaw(sql, parms.ToArray());

            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        public async Task<ResponseLlaveValor> ConsultaObservacionTemporal(int id)
        {
            try
            {
                string sql = "EXEC [dbo].[SP_Consulta_Observacion_Temporal] @RegistroAuditoriaId";

                List<SqlParameter> parms = new List<SqlParameter>
                {
            
                    new SqlParameter { ParameterName = "@RegistroAuditoriaId", Value = id},

                };

                var Data = await _dBAuditCACContext.LLaveValor.FromSqlRaw<ResponseLlaveValor>(sql, parms.ToArray()).ToListAsync();
                return Data[0];
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }
    }
}
