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
    public class RegistroAuditoriaCalificacionesManager : IRegistroAuditoriaCalificacionesRepository<RegistroAuditoriaCalificacionesModel>
    {
        private readonly DBAuditCACContext _dBAuditCACContext;
        private readonly ILogger<RegistroAuditoriaCalificacionesModel> _logger;

        //Constructor.
        public RegistroAuditoriaCalificacionesManager(DBAuditCACContext dBAuditCACContext, ILogger<RegistroAuditoriaCalificacionesModel> logger)
        {
            this._dBAuditCACContext = dBAuditCACContext;
            _logger = logger;
        }

        //Metodos
        //Metodo para llamar procedure CalificarRegistroAuditoria - Para insertar calificaciones a una IPS.
        public async Task CalificarRegistroAuditoria(List<InputsRegistroAuditoriaCalificacionesDto> inputsDto) //RegistroAuditoriaCalificacionesModel
        {
            try
            {
                string sql = "EXEC [dbo].[CalificarRegistroAuditoria] @QueryToExecute";

                //Declaramos variables usadas.
                int Id;
                int RegistrosAuditoriaId;
                int RegistrosAuditoriaDetalleId;
                int VariableId;
                int IpsId;
                int ItemId;
                int Calificacion;
                var Observacion = "";
                var CreatedBy = "";
                var CreatedDate = "";
                var ModifyBy = "";
                var ModifyDate = "";

                var QueryToExecute = "";

                //Capturamos Listados.                
                //var NoListaEntrada = inputsDto.Count;

                //Consultamos Numero de calificaciones esperadas.
                //var NoCalificaciones = _dBAuditCACContext.ItemModel.Where(x => x.CatalogId == 3).Count();

                //Validamos si se enviaron el numero de calificaciones esperados.
                //if (NoListaEntrada == NoCalificaciones)
                //{
                //Recorremos listado.
                foreach (var itemNoListaEntrada in inputsDto)
                {

                    Id = itemNoListaEntrada.Id;
                    RegistrosAuditoriaId = itemNoListaEntrada.RegistrosAuditoriaId;
                    RegistrosAuditoriaDetalleId = itemNoListaEntrada.RegistrosAuditoriaDetalleId;
                    VariableId = itemNoListaEntrada.VariableId;
                    IpsId = itemNoListaEntrada.IpsId;
                    ItemId = itemNoListaEntrada.ItemId;
                    Calificacion = itemNoListaEntrada.Calificacion;
                    Observacion = itemNoListaEntrada.Observacion;
                    CreatedBy = itemNoListaEntrada.CreatedBy;
                    CreatedDate = "GETDATE()"; //CreatedDate = DateTime.Now;
                    ModifyBy = itemNoListaEntrada.CreatedBy;
                    ModifyDate = "GETDATE()"; //ModifyDate = DateTime.Now;

                    //QueryToExecute = "";

                    //Validamos si existe una calificacion.
                    var RegistroAuditoriaCalificaciones = _dBAuditCACContext.RegistroAuditoriaCalificacionesModel.Where(x => x.RegistrosAuditoriaDetalleId == RegistrosAuditoriaDetalleId).Count();
                    //if (RegistroAuditoriaCalificaciones == null ) { RegistroAuditoriaCalificaciones = 0; } //Valor por defecto si no encuentra datos.                                

                    //Concatenamos primera linea para Insert/Update, ya que debe ser ejecutada una sola vez.
                    if (RegistroAuditoriaCalificaciones == 0)
                    {
                        QueryToExecute = QueryToExecute + "INSERT INTO [RegistroAuditoriaCalificaciones] ([RegistrosAuditoriaId], [RegistrosAuditoriaDetalleId], [VariableId], [IpsId], [ItemId], [Calificacion], [Observacion], [CreatedBy], [CreatedDate], [ModifyBy], [ModifyDate]) " +
                            "VALUES (" + RegistrosAuditoriaId + ", " + RegistrosAuditoriaDetalleId + ", " + VariableId + ", " + IpsId + ", " + ItemId + ", " + Calificacion + ",'" + Observacion + "','" + CreatedBy + "'," + CreatedDate + ",'" + ModifyBy + "'," + ModifyDate + "); ";
                    }
                    else
                    {
                        QueryToExecute = QueryToExecute + "UPDATE [RegistroAuditoriaCalificaciones] SET [Calificacion] = " + Calificacion + ", [Observacion] = '" + Observacion + "', [ModifyBy] = '" + ModifyBy + "', [ModifyDate] = " + ModifyDate + " WHERE Id = " + Id + "; ";
                    }
                }

                //Asignamos valores al procedimiento.
                List<SqlParameter> parms = new List<SqlParameter>
                    { 
                        // Create parameters   
                        new SqlParameter { ParameterName = "@QueryToExecute", Value = QueryToExecute }, //"'" + QueryToExecute + "'"
                    };

                //Ejecutamos procedimiento.
                var Data = await _dBAuditCACContext.Database.ExecuteSqlRawAsync(sql, parms.ToArray());
                //return Data;
                //}

                #region OBSOLETO
                //OLD Temp1
                //var Id = inputsDto.Id;
                //int RegistrosAuditoriaId = inputsDto.RegistrosAuditoriaId;
                //int RegistrosAuditoriaDetalleId = inputsDto.RegistrosAuditoriaDetalleId;
                //int VariableId = inputsDto.VariableId;
                //var IpsId = inputsDto.IpsId;
                //var ItemId = inputsDto.ItemId;
                //var Calificacion = inputsDto.Calificacion;
                //var Observacion = inputsDto.Observacion;
                //var CreatedBy = inputsDto.CreatedBy;
                //var CreatedDate = "GETDATE()"; //var CreatedDate = DateTime.Now;
                //var ModifyBy = inputsDto.CreatedBy;
                //var ModifyDate = "GETDATE()"; //var ModifyDate = DateTime.Now;

                //var QueryToExecute = "";

                ////Validamos si existe una calificacion.
                //var RegistroAuditoriaCalificaciones = _dBAuditCACContext.RegistroAuditoriaCalificacionesModel.Where(x => x.RegistrosAuditoriaId == RegistrosAuditoriaId).Count();
                ////if (RegistroAuditoriaCalificaciones == null ) { RegistroAuditoriaCalificaciones = 0; } //Valor por defecto si no encuentra datos.                                

                ////Concatenamos primera linea para Insert/Update, ya que debe ser ejecutada una sola vez.
                //if (RegistroAuditoriaCalificaciones == 0)
                //{
                //    QueryToExecute = QueryToExecute + "INSERT INTO [RegistroAuditoriaCalificaciones] ([RegistrosAuditoriaId], [RegistrosAuditoriaDetalleId], [VariableId], [IpsId], [ItemId], [Calificacion], [Observacion], [CreatedBy], [CreatedDate], [ModifyBy], [ModifyDate]) " +
                //        "VALUES (" + RegistrosAuditoriaId + ", " + RegistrosAuditoriaDetalleId + ", " + VariableId + ", " + IpsId + ", " + ItemId + ", " + Calificacion + ",'" + Observacion + "','" + CreatedBy + "'," + CreatedDate + ",'" + ModifyBy + "'," + ModifyDate + "); ";
                //}
                //else
                //{
                //    QueryToExecute = QueryToExecute + "UPDATE [RegistroAuditoriaCalificaciones] SET [Calificacion] = " + Calificacion + ", [Observacion] = '" + Observacion + "', [ModifyBy] = '" + ModifyBy + "', [ModifyDate] = " + ModifyDate + " WHERE Id = " + Id + "; ";
                //}


                ////Asignamos valores al procedimiento.
                //List<SqlParameter> parms = new List<SqlParameter>
                //    { 
                //        // Create parameters   
                //        new SqlParameter { ParameterName = "@QueryToExecute", Value = QueryToExecute }, //"'" + QueryToExecute + "'"
                //    };

                ////Ejecutamos procedimiento.
                //var Data = await _dBAuditCACContext.Database.ExecuteSqlRawAsync(sql, parms.ToArray());
                ////return Data;


                //OLD Temp2
                ////Declaramos variables usadas.
                //var Id = inputsDto.Id;
                //int RegistrosAuditoriaId = inputsDto.RegistrosAuditoriaId;
                //var IpsId = inputsDto.IpsId;
                //var Observacion = inputsDto.Observacion;
                //var CreatedBy = inputsDto.CreatedBy;
                //var CreatedDate = "GETDATE()"; //var CreatedDate = DateTime.Now;
                //var ModifyBy = inputsDto.CreatedBy;
                //var ModifyDate = "GETDATE()"; //var ModifyDate = DateTime.Now;

                //int RegistroAuditoriaCalificacionesId;
                //var ItemId = "";
                //var Calificacion = "";                
                //var QueryToExecute = "";                

                ////Validamos si existe una calificacion.
                //var RegistroAuditoriaCalificaciones = _dBAuditCACContext.RegistroAuditoriaCalificacionesModel.Where(x => x.RegistrosAuditoriaId == RegistrosAuditoriaId).Count();
                ////if (RegistroAuditoriaCalificaciones == null ) { RegistroAuditoriaCalificaciones = 0; } //Valor por defecto si no encuentra datos.                                

                ////Concatenamos primera linea para Insert/Update, ya que debe ser ejecutada una sola vez.
                //if (RegistroAuditoriaCalificaciones == 0)
                //{
                //    QueryToExecute = QueryToExecute + "DECLARE @IdInsertado TABLE (IdInsertado INT); " +
                //        "INSERT INTO [RegistroAuditoriaCalificaciones] ([RegistrosAuditoriaId], [IpsId], [Observacion], [CreatedBy], [CreatedDate], [ModifyBy], [ModifyDate]) " +
                //        "OUTPUT INSERTED.Id INTO @IdInsertado " +
                //        "VALUES (" + RegistrosAuditoriaId + "," + IpsId + ",'" + Observacion + "','" + CreatedBy + "'," + CreatedDate + ",'" + ModifyBy + "'," + ModifyDate + "); " +
                //        "DECLARE @RegistroAuditoriaCalificacionesId INT = (SELECT * FROM @IdInsertado); ";
                //}
                //else
                //{
                //    QueryToExecute = QueryToExecute + "UPDATE [RegistroAuditoriaCalificaciones] SET [Observacion] = '" + Observacion + "', [ModifyBy] = '" + ModifyBy + "', [ModifyDate] = " + ModifyDate + " WHERE Id = " + Id + "; ";
                //}

                ////Capturamos Listados.                
                //var ListaDetalles = inputsDto.ListadoDetalles;

                ////Consultamos Numero de calificaciones esperadas.
                //var NoCalificaciones = _dBAuditCACContext.ItemModel.Where(x => x.CatalogId == 3).Count();

                ////Validamos si se enviaron el numero de calificaciones esperados.
                //if (ListaDetalles.Count == NoCalificaciones)
                //{                                   
                //    //Recorremos listado.
                //    foreach (var itemListaDetalles in ListaDetalles)
                //    {

                //        ItemId = itemListaDetalles.ItemId.ToString();

                //        //Calificacion = (itemListaDetalles.Calificacion != null) ? itemListaDetalles.Calificacion.ToString() : "NULL"; //Si viene Null, guardar un Null.
                //        Calificacion = itemListaDetalles.Calificacion.ToString();

                //        //Validamos si vamos actualizar o a insertar medidas. (SI existe una medida, Actualizamos. SI NO existe una medida, Insertamos)
                //        if (RegistroAuditoriaCalificaciones == 0)
                //        {                            
                //            QueryToExecute = QueryToExecute + "INSERT INTO [RegistroAuditoriaCalificacionesDetalle] ([RegistroAuditoriaCalificacionesId], [ItemId], [Calificacion], [CreatedBy], [CreatedDate], [ModifyBy], [ModifyDate]) " +
                //                "VALUES (@RegistroAuditoriaCalificacionesId," + ItemId + ",'" + Calificacion + "','" + CreatedBy + "'," + CreatedDate + ",'" + ModifyBy + "'," + ModifyDate + "); ";
                //        }
                //        else
                //        {                         
                //            RegistroAuditoriaCalificacionesId = itemListaDetalles.Id;
                //            QueryToExecute = QueryToExecute + "UPDATE [RegistroAuditoriaCalificacionesDetalle] SET [Calificacion] = " + Calificacion + ", [ModifyBy] = '" + ModifyBy + "', [ModifyDate] = " + ModifyDate + " WHERE Id = " + RegistroAuditoriaCalificacionesId + ";";

                //        }
                //    }

                //    //Asignamos valores al procedimiento.
                //    List<SqlParameter> parms = new List<SqlParameter>
                //    { 
                //        // Create parameters   
                //        new SqlParameter { ParameterName = "@QueryToExecute", Value = QueryToExecute }, //"'" + QueryToExecute + "'"
                //    };

                //    //Ejecutamos procedimiento.
                //    var Data = await _dBAuditCACContext.Database.ExecuteSqlRawAsync(sql, parms.ToArray());
                //    //return Data;
                //}
                #endregion
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        //Metodo para consultar las calificaciones de un registro auditoria segun su RegistrosAuditoriaId.
        public async Task<List<ResponseRegistroAuditoriaCalificacionesDto>> GetCalificacionesRegistroAuditoriaByRegistrosAuditoriaId(InputsGetRegistroAuditoriaCalificacionesByRegistrosAuditoriaIdDto inputsDto)
        {
            try
            {
                string sql = "EXEC [dbo].[GetCalificacionesRegistroAuditoriaByRegistrosAuditoriaId] @RegistrosAuditoriaId";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters   
                    new SqlParameter { ParameterName = "@RegistrosAuditoriaId", Value = inputsDto.RegistrosAuditoriaId}
                };

                //Ejecutamos SP y obtenemos Datos.
                var Data = await _dBAuditCACContext.ResponseRegistroAuditoriaCalificacionesDto.FromSqlRaw<ResponseRegistroAuditoriaCalificacionesDto>(sql, parms.ToArray()).ToListAsync();

                return Data;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        //Metodo para consultar las calificaciones de un registro auditoria segun su ItemId.
        public async Task<List<ResponseRegistroAuditoriaCalificacionesDto>> GetCalificacionesRegistroAuditoriaByVariableId(InputsGetRegistroAuditoriaCalificacionesByVariableIdDto inputsDto)
        {
            try
            {
                string sql = "EXEC [dbo].[GetCalificacionesRegistroAuditoriaByVariableId] @VariableId, @RegistrosAuditoriaId";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters   
                    new SqlParameter { ParameterName = "@VariableId", Value = inputsDto.VariableId},
                    new SqlParameter { ParameterName = "@RegistrosAuditoriaId", Value = inputsDto.RegistrosAuditoriaId}
                };

                //Ejecutamos SP y obtenemos Datos.
                var Data = await _dBAuditCACContext.ResponseRegistroAuditoriaCalificacionesDto.FromSqlRaw<ResponseRegistroAuditoriaCalificacionesDto>(sql, parms.ToArray()).ToListAsync();

                return Data;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        //Metodo para consultar si un registro auditoria esta totalmente calificado.
        public async Task<bool> GetCalificacionEsCompletas(InputsGetCalificacionEsCompletasDto inputsDto)
        {
            try
            {
                var Valid = false;

                ////Consultamos Numero de calificaciones esperadas.
                //var NoCalificaciones = _dBAuditCACContext.ItemModel.Where(x => x.CatalogId == 3).Where(x => x.Enable == true).Count();

                ////Validamos si existe una calificacion.
                //int RegistrosAuditoriaId = inputsDto.RegistrosAuditoriaId;
                //int RegistrosAuditoriaDetalleId = inputsDto.RegistrosAuditoriaDetalleId;
                //var NoRegistroAuditoriaCalificaciones = _dBAuditCACContext.RegistroAuditoriaCalificacionesModel.Where(x => x.RegistrosAuditoriaId == RegistrosAuditoriaId).Where(x => x.RegistrosAuditoriaDetalleId == RegistrosAuditoriaDetalleId).Count();
                ////var NoRegistroAuditoriaCalificaciones = _dBAuditCACContext.RegistroAuditoriaCalificacionesModel.Where(x => x.RegistroAuditoriaCalificacionesId == RegistroAuditoriaCalificacionesId).Count();

                ////Validamos calificaiones.
                //if (NoCalificaciones == NoRegistroAuditoriaCalificaciones)
                //{
                //    Valid = true;
                //}

                string sql = "EXEC [dbo].[GetCalificacionEsCompletas] @RegistrosAuditoriaId";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters   
                    new SqlParameter { ParameterName = "@RegistrosAuditoriaId", Value = inputsDto.RegistrosAuditoriaId}
                };

                //Ejecutamos SP y obtenemos Datos.
                var Data = await _dBAuditCACContext.ResponseGetCalificacionEsCompletasDto.FromSqlRaw<ResponseGetCalificacionEsCompletasDto>(sql, parms.ToArray()).ToListAsync();

                //Validamos calificaiones.
                if (Data.FirstOrDefault().Valid == true)
                {
                    Valid = true;
                }

                return await Task.FromResult(Valid);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Para la consultar Items calificable asociados a una variable
        /// </summary>
        /// <param name="VariableId">Id de variable</param>
        /// <returns>Modelo de datos</returns>
        public async Task<List<ItemModel>> GetItemsByVariableId(int VariableId)
        {
            try
            {
                string sql = "EXEC [dbo].[SP_Consultar_Items_By_VariableId] @VariableId";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters   
                    new SqlParameter { ParameterName = "@VariableId", Value = VariableId},
                };

                //Ejecutamos SP y obtenemos Datos.
                var Data = await _dBAuditCACContext.ItemModel.FromSqlRaw<ItemModel>(sql, parms.ToArray()).ToListAsync();

                return Data;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }
    }
}
