using AuditCAC.Core.Core;
using AuditCAC.Dal.Data;
using AuditCAC.Domain.Dto;
using AuditCAC.Domain.Dto.Calculadora;
using AuditCAC.Domain.Dto.RegistroAuditoria;
using AuditCAC.Domain.Entities;
using AuditCAC.Domain.Helpers;
using AuditCAC.MainCore.Module.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace AuditCAC.MainCore.Module
{
    public class RegistrosAuditoriaDetalleManager : IRegistrosAuditoriaDetalleRepository<RegistrosAuditoriaDetalleModel>
    {
        private readonly DBAuditCACContext _dBAuditCACContext;
        private readonly ILogger<RegistrosAuditoriaDetalleModel> _logger;
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //Constructor
        public RegistrosAuditoriaDetalleManager(DBAuditCACContext dBAuditCACContext, ILogger<RegistrosAuditoriaDetalleModel> logger)
        {
            this._dBAuditCACContext = dBAuditCACContext;
            _logger = logger;
        }

        //Metodos.
        public async Task<IEnumerable<RegistrosAuditoriaDetalleModel>> GetAll()
        {
            try
            {
                return await _dBAuditCACContext.RegistrosAuditoriaDetalleModel.Where(x => x.Status == true).ToListAsync();
                //return await this.dBAuditCACContext.RegistrosAuditoriaDetalleModel.ToListAsync();
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        public async Task<RegistrosAuditoriaDetalleModel> Get(int id)
        {
            try
            {
                return await _dBAuditCACContext.RegistrosAuditoriaDetalleModel.Where(x => x.Status == true).FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        public async Task<RegistrosAuditoriaDetalleModel> GetRegistrosAuditoriaId(int RegistrosAuditoriaId)
        {
            try
            {
                return await _dBAuditCACContext.RegistrosAuditoriaDetalleModel.Where(x => x.Status == true).FirstOrDefaultAsync(x => x.RegistrosAuditoriaId == RegistrosAuditoriaId);
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        public async Task Add(RegistrosAuditoriaDetalleModel entity)
        {
            try
            {
                _dBAuditCACContext.RegistrosAuditoriaDetalleModel.Add(entity);
                await _dBAuditCACContext.SaveChangesAsync();
                //return NoContent;
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        public async Task Update(RegistrosAuditoriaDetalleModel dbEntity, RegistrosAuditoriaDetalleModel entity)
        {
            try
            {
                //dbEntity.Id = entity.Id;
                dbEntity.VariableId = entity.VariableId;
                dbEntity.RegistrosAuditoriaId = entity.RegistrosAuditoriaId;
                dbEntity.EstadoVariableId = entity.EstadoVariableId;
                dbEntity.Observacion = entity.Observacion;
                dbEntity.Activo = entity.Activo;
                dbEntity.ModifyBy = entity.ModifyBy;
                dbEntity.ModifyDate = entity.ModifyDate;
                dbEntity.EsGlosa = entity.EsGlosa;
                dbEntity.Visible = entity.Visible;
                dbEntity.Calificable = entity.Calificable;
                dbEntity.SubgrupoId = entity.SubgrupoId;
                dbEntity.DatoReportado = entity.DatoReportado;
                dbEntity.MotivoVariable = entity.MotivoVariable;
                dbEntity.Dato_DC_NC_ND = entity.Dato_DC_NC_ND;
                dbEntity.Ara = entity.Ara;

                await _dBAuditCACContext.SaveChangesAsync();
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }


        /// <summary>
        /// Actualiza calificacion registro auditoria detalle
        /// </summary>
        /// <param name="inputsDto">Datos entrada calificacion</param>
        /// <returns>Modelo validacion registro auditoria detalle</returns>
        public async Task<ResponseValidacionEstado> ActualizarDC_NC_ND_Motivo(InputsRegistrosAuditoriaDetalleUpdate_Actualizar_DC_NC_ND_Motivo_Dto inputsDto, int buttonAction)
        {
            try
            {
                #region Update detail

                string sql = "EXEC [dbo].[ActualizarCampo_Dato_DC_NC_ND] @RegistrosAuditoriaDetalle, @MotivoVariable, @Dato_DC_NC_ND, @UserId";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters   
                    new SqlParameter { ParameterName = "@RegistrosAuditoriaDetalle", Value = inputsDto.RegistrosAuditoriaDetalle},
                    new SqlParameter { ParameterName = "@MotivoVariable", Value = inputsDto.MotivoVariable},
                    new SqlParameter { ParameterName = "@Dato_DC_NC_ND", Value = inputsDto.Dato_DC_NC_ND},
                    new SqlParameter { ParameterName = "@UserId", Value = inputsDto.UserId}, //inputsDto.UserId
            };

                var Data = await _dBAuditCACContext.Database.ExecuteSqlRawAsync(sql, parms.ToArray());

                #endregion

                #region Return validation object

                var oReturn = await GetValidacionesRegistroAuditoriaDetalle(inputsDto.RegistroauditoriaId, inputsDto.UserId, buttonAction);

                return oReturn;
                #endregion
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        public async Task Delete(RegistrosAuditoriaDetalleModel entity)
        {
            try
            {
                //_dBAuditCACContext.RegistrosAuditoriaDetalleModel.Remove(entity);
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

        ////Metodo para llamar procedure RegistroAuditoriaDetalles - Para cargar registros de RegistroAuditoriaDetalles, agrupado segun corresponde
        //public async Task<List<ResponseRegistroAuditoriaDetallesDto>> GetRegistroAuditoriaDetalles(InputsRegistroAuditoriaDetallesDto inputsDto)
        //{
        //    try
        //    {
        //        string sql = "EXEC [dbo].[RegistroAuditoriaDetalles] @Id";

        //        List<SqlParameter> parms = new List<SqlParameter>
        //        { 
        //            // Create parameters   
        //            new SqlParameter { ParameterName = "@Id", Value = inputsDto.Id}
        //        };

        //        var Data = await _dBAuditCACContext.ResponseRegistroAuditoriaDetallesDto.FromSqlRaw<ResponseRegistroAuditoriaDetallesDto>(sql, parms.ToArray()).ToListAsync();
        //        return Data;
        //    }
        //    catch (Exception ex)
        //    {
        //        //var attempsCheckState = 1;
        //        //attempsCheckState++;
        //        _log.Info(ex);
        //        _log.Error(ex);
        //        throw;
        //    }
        //}

        /// <summary>
        /// Consulta detalle de registro auditoria por Id
        /// </summary>
        /// <param name="Id">Id</param>
        /// <returns>Lista Modelo registro de auditoria</returns>
        public async Task<List<ResponseRegistroAuditoriaDetalle>> ConsultarRegistroAuditoriaDetallePorid(int Id)
        {
            try
            {
                List<ResponseRegistroAuditoriaDetalleInfo> oResult = new List<ResponseRegistroAuditoriaDetalleInfo>();
                List<ResponseRegistroAuditoriaDetalle> oReturn = new List<ResponseRegistroAuditoriaDetalle>();
                bool isHepatitis = false;

                #region Is Hepatitis               

                try
                {
                    //Validation for know if is hepatitis
                    string sqlH = "EXEC  @returnValue = [dbo].[SP_Valida_Medicion_Hepatitis] @MedicionId, @RegistroAuditoriaId";

                    List<SqlParameter> paramsH = new List<SqlParameter>
                    {
                        new SqlParameter { ParameterName = "@MedicionId", Value = 0},
                        new SqlParameter { ParameterName = "@RegistroAuditoriaId", Value = Id},
                         new SqlParameter { ParameterName = "ReturnValue", SqlDbType = System.Data.SqlDbType.Int, Direction = ParameterDirection.Output},
                    };


                    var DataH2 = await _dBAuditCACContext.Database.ExecuteSqlRawAsync(sqlH, paramsH.ToArray());
                    var returnValue = paramsH[2].Value;

                    //Get return value from sp
                    if (Convert.ToInt32(returnValue) == 1)
                    {
                        isHepatitis = true;
                    }

                }
                catch (Exception ex)
                {
                    _logger.LogInformation(ex.ToString());
                }
                #endregion


                string sql = "EXEC [dbo].[SP_Consulta_Detalle_Registros_Auditoria] @Id";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters   
                    new SqlParameter { ParameterName = "@Id", Value = Id}
                };

                var Data = await _dBAuditCACContext.InputsRegistroAuditoriaDetalle.FromSqlRaw<InputsRegistroAuditoriaDetalle>(sql, parms.ToArray()).ToListAsync();

                #region REGISTRO AUDITORIA INFO

                List<ResponseRegistroAuditoriaDetalle> Return2 = new List<ResponseRegistroAuditoriaDetalle>();
                List<RegistroAuditoriaDetalleErrorGroupModel> errores = new List<RegistroAuditoriaDetalleErrorGroupModel>();

                if (Data.Count > 0)
                {
                    //Get Errores
                    //errores = await ConsultarErroresRegistrosAuditoriaAgrupado(Id);
                    //errores = errores.ToList();


                    #region Obtenemos Info de la respuesta del SP SP_Consulta_Detalle_Registros_Auditoria. Variable Data.
                    //String.IsNullOrEmpty(String)
                    var dataInfoGruoped = Data.Where(x => !String.IsNullOrEmpty(x.DatoReportado) && x.DatoReportado != " " && 
                        !String.IsNullOrEmpty(x.TablaReferencial) && x.TablaReferencial != " " && 
                        !String.IsNullOrEmpty(x.CampoReferencial) && x.CampoReferencial != " ").ToList();
                    #endregion

                    //Usar para enviar datos.
                    //dataInfoGruoped

                    #region COMENTADO POR Tablas Referenciales
                    //#region Request Tablas Referenciales
                    ////Con la info adicional consultade de IPS Y EPS consulta los nombres del request de tabla referencial

                    ////Consulta parametros generales para request
                    //var parameterList = _dBAuditCACContext.ParametrosGenerales.Where(x => x.Activo).ToList();

                    //RequestCore _requestCore = new RequestCore();
                    //Util _util = new Util();

                    //string campoReferencialIPS = _util.GetAndValidateParameter(parameterList, Enumeration.Parametros.CampoReferencialIps);
                    //string campoReferencialEPS = _util.GetAndValidateParameter(parameterList, Enumeration.Parametros.CampoReferencialEps);

                    ////Request autenticacion para tomar token
                    //var tokenInfo = _requestCore.requestAuthenticationCAC(
                    //    _util.GetAndValidateParameter(parameterList, Enumeration.Parametros.URLAuditCACCoreCACServices), //Url parameter
                    //    _util.GetAndValidateParameter(parameterList, Enumeration.Parametros.UsuarioTokenAuditCACCoreCACServices), //User Parameter
                    //    _util.GetAndValidateParameter(parameterList, Enumeration.Parametros.PasswordTokenAuditCACCoreCACServices) //Password Paramter
                    //    );


                    ////Modelo para consultar info
                    //List<InputsTablasReferencialByValorReferencialDto> requestInput = new List<InputsTablasReferencialByValorReferencialDto>();

                    //dataInfoGruoped?.All(x =>
                    //{
                    //    requestInput.Add(new InputsTablasReferencialByValorReferencialDto()
                    //    {
                    //        TablaReferencial = x.TablaReferencial,
                    //        ValorReferencial = x.DatoReportado
                    //    });
                    //    return true;
                    //});

                    //var aditionalInfo = _requestCore.requestGetTablasReferencialesPorId(_util.GetAndValidateParameter(parameterList, Enumeration.Parametros.URLAuditCACCoreCACServices), requestInput, tokenInfo.Token);                    
                    //#endregion

                    //#region Construir Data Respuesta

                    //Data.Select(c =>
                    //{
                    //    c.Nombre = aditionalInfo.Where(a =>
                    //    a.TablaReferencial == c.TablaReferencial &&
                    //    a.Id == c.DatoReportado).Select(s => s.Nombre).FirstOrDefault(); return c;
                    //}).ToList();

                    //#endregion
                    #endregion
                }

                #endregion

                Data?.All(x =>
                {
                    oResult.Add(new ResponseRegistroAuditoriaDetalleInfo(x));
                    return true;
                });

                var group = Data.GroupBy(x => new { x.SubGrupoId, x.SubGrupoDescripcion, x.SubGrupoOrden })
                    .Select(y => new ResponseRegistroAuditoriaDetalle()
                    {
                        name = y.Key.SubGrupoDescripcion,
                        idgrupo = y.Key.SubGrupoId,
                        SubGrupoOrden = y.Key.SubGrupoOrden
                    }
                    );

                group?.All(x =>
                    {

                    ResponseRegistroAuditoriaDetalle record = new ResponseRegistroAuditoriaDetalle();

                        record.name = x.name;
                        record.idgrupo = x.idgrupo;
                        record.variables = new List<ResponseRegistroAuditoriaDetalle.Variable>();
                        record.SubGrupoOrden = x.SubGrupoOrden;

                        int countErrores = 0;

                        var variables = Data.Where(d => d.SubGrupoDescripcion == x.name)?.All( a =>
                            {

                                var variable = new ResponseRegistroAuditoriaDetalle.Variable()
                                {
                                    reducido = a.CodigoVariable,
                                    detalle = a.NombreVariable,
                                    seleccion = a.Motivo,
                                    registroAuditoriaDetalleId = a.Id,
                                    dato_reportado = a.DatoReportado,
                                    motivo = a.Motivo,
                                    Dato_DC_NC_ND = a.Dato_DC_NC_ND, //Dato_DC_NC_ND,
                                    Visible = a.Visible,             //Visible
                                    Bot = a.Bot,
                                    VariableId = a.VariableId,
                                    VariableEncuesta = a.VariableEncuesta,
                                    //IpsId = a.IpsId,
                                    Nombre = a.Nombre, //IpsNombre = a.IpsNombre
                                    TablaReferencial = a.TablaReferencial,
                                    CampoReferencial = a.CampoReferencial, 
                                    IdTipoVariable = a.IdTipoVariable,
                                    Longitud = a.Longitud,
                                    Decimales = a.Longitud,
                                    Formato = a.Formato,
                                    ValorDatoReportado = a.ValorDatoReportado,
                                    Orden = a.Orden,
                                    TipoVariableId = a.TipoVariableId,
                                    TipoVariableNombre = a.TipoVariableNombre,
                                    EnableDC = a.EnableDC,
                                    EnableNC = a.EnableNC,
                                    EnableND = a.EnableND,
                                    Condicionada = a.Condicionada,
                                    ValorConstante = a.ValorConstante,
                                    DescripcionVariable = a.DescripcionVariable,
                                    Alerta = a.Alerta,
                                    AlertaDescripcion = a.AlertaDescripcion,
                                    MedicionId = a.MedicionId,
                                    EsVisible = a.EsVisible,
                                    EsCalificable = a.EsCalificable,
                                    CountHallazgos = a.CountHallazgos,
                                    Calculadora = a.Calculadora,
                                    TipoCalculadora = a.TipoCalculadora,
                                    EstadoMedicion = a.EstadoMedicion
                                };


                                //Get Context
                                if (a.Contexto != null && a.Contexto != "")
                                {
                                    try
                                    {
                                        XmlSerializer serializer = new XmlSerializer(typeof(ContextoDto));
                                        using (TextReader reader = new StringReader(a.Contexto))
                                        {
                                            ContextoDto result = (ContextoDto)serializer.Deserialize(reader);
                                            variable.Contexto = result;
                                        }
                                    }
                                    catch (Exception ex)
                                    {
                                        _logger.LogInformation("ERROR CONTRUYENDO CONTEXTO REgistroAuditoriaDetalle: "  + a.Id);
                                        _logger.LogInformation(ex.ToString());
                                    }
                                }
                               
                                #region ERRORES

                                //Validation if error is registered previously
                                //if (errores.Where(e => e.RegistrosAuditoriaDetalleId == a.Id && e.VariableId == a.VariableId).Any())
                                //{
                                //    variable.error = new List<RegistroAuditoriaDetalleErrorGroupModel>();

                                //    errores.Where(e => e.RegistrosAuditoriaDetalleId == a.Id && e.VariableId == a.VariableId)?.All(e =>
                                //    {
                                //        variable.error.Add(e);
                                //        countErrores++;
                                //        return true;
                                //    });
                                    
                                //}
                                

           
                                #endregion

                                variable.listaMotivos = new List<string>();
                              

                                record.variables.Add(variable);
                                return true;
                            });

                        record.error = countErrores == 0 ? "" : countErrores.ToString();

                        if (isHepatitis && record.idgrupo == (int)Enumeration.SubGrupoVariable.Estructura2)
                        {
                            record.variables = record.variables.OrderBy(x => x.registroAuditoriaDetalleId).ToList();
                        }
                        else
                        {
                            record.variables = record.variables.OrderBy(x => x.Orden).ToList();
                        }

                        oReturn.Add(record);
                        return true;

                        
                    });
                return oReturn.OrderBy(x => x.SubGrupoOrden).ToList();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }


        /// <summary>
        /// Valida errores por Id registro auditoria 
        /// </summary>
        /// <param name="idRegistroAuditoria"></param>
        /// <param name="userId"></param>
        /// <returns>Listado Errores</returns>
        public async Task<List<RegistroAuditoriaDetalleErrorGroupModel>> ValidarErroresRegistroAuditoria(int idRegistroAuditoria, string userId)
        {
            try
            {
                List<RegistroAuditoriaDetalleErrorGroupModel> oReturn = new List<RegistroAuditoriaDetalleErrorGroupModel>();

                #region Consulta Reglas

                string sql = "EXEC SP_Valida_Errores @RegistroAuditoria, @User";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters
                    new SqlParameter { ParameterName = "@RegistroAuditoria", Value = idRegistroAuditoria},
                    new SqlParameter { ParameterName = "@User", Value = userId}
                };

                _dBAuditCACContext.Database.SetCommandTimeout(500);

                _logger.LogInformation("Inicia Ejecucion SP_Valida_Errores, usuario: " + userId + ", RegistroAuditoria: " + idRegistroAuditoria);
                var Data = await _dBAuditCACContext.RegistroAuditoriaDetalleErrorModel.FromSqlRaw<RegistroAuditoriaDetalleErrorModel>(sql, parms.ToArray()).ToListAsync();
                 _logger.LogInformation("Resultado Ejecucion SP_Valida_Errores, RegistroAuditoria: " + idRegistroAuditoria + ": " + Data.Count() + " registros");

                #endregion

                //Agrupa respuesta por id de regla
                var group = Data.GroupBy(x => new { x.ErrorId, x.VariableId, x.RegistrosAuditoriaDetalleId }).Select(x => new RegistroAuditoriaDetalleErrorGroupModel()
                {
                    RegistrosAuditoriaDetalleId = x.Key.RegistrosAuditoriaDetalleId,
                    IdError = x.Key.ErrorId,
                    VariableId = x.Key.VariableId
                }).ToList();

              

                group?.All(x =>
                {
                    RegistroAuditoriaDetalleErrorGroupModel regla = new RegistroAuditoriaDetalleErrorGroupModel();
                    regla.RegistrosAuditoriaDetalleId = Data.Where(d => d.RegistrosAuditoriaDetalleId == x.RegistrosAuditoriaDetalleId).Select(d => d.RegistrosAuditoriaDetalleId).FirstOrDefault();
                    regla.DescripcionError = Data.Where(d => d.ErrorId == x.IdError && d.VariableId == x.VariableId).Select(d => d.Descripcion).FirstOrDefault();
                    regla.IdError = x.IdError;
                    regla.VariableId = x.VariableId;
                    regla.Restricciones = Data.Where(d => d.ErrorId == x.IdError && d.VariableId == x.VariableId && d.Enable && d.RegistrosAuditoriaDetalleId == regla.RegistrosAuditoriaDetalleId).ToList();
                    oReturn.Add(regla);
                    return true;
                });


                _logger.LogInformation("Resultado Valida errores : " + idRegistroAuditoria + ": " + oReturn.Count() + " registros");

                return oReturn;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        //Metodo para llamar procedure GetUsuariosByRoleId - Para cargar detalles de asignación
        public async Task<List<ResponseGetUsuariosByRoleIdDto>> GetUsuariosByRoleId(InputsGetUsuariosByRoleIdDto inputsDto)
        {
            try
            {
                string sql = "EXEC [dbo].[GetUsuariosByRoleId] @RoleId, @UserId";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters   
                    new SqlParameter { ParameterName = "@RoleId", Value = inputsDto.RoleId},
                    new SqlParameter { ParameterName = "@UserId", Value = inputsDto.UserId}
                };

                var Data = await _dBAuditCACContext.ResponseGetUsuariosByRoleIdDto.FromSqlRaw<ResponseGetUsuariosByRoleIdDto>(sql, parms.ToArray()).ToListAsync();
                return Data;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        //Metodo para llamar procedure GetUsuariosByRoleCoberturaId - Para cargar detalles de asignación.
        public async Task<List<ResponseGetUsuariosByRoleIdDto>> GetUsuariosByRoleCoberturaId(InputsGetUsuariosByRoleCoberturaIdDto inputsDto)
        {
            try
            {
                string sql = "EXEC [dbo].[GetUsuariosByRoleCoberturaId] @RoleId, @UserId, @CoberturaId";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters   
                    new SqlParameter { ParameterName = "@RoleId", Value = inputsDto.RoleId},
                    new SqlParameter { ParameterName = "@UserId", Value = inputsDto.UserId},
                    new SqlParameter { ParameterName = "@CoberturaId", Value = inputsDto.CoberturaId}
                };

                var Data = await _dBAuditCACContext.ResponseGetUsuariosByRoleIdDto.FromSqlRaw<ResponseGetUsuariosByRoleIdDto>(sql, parms.ToArray()).ToListAsync();
                return Data;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        //Metodo para llamar procedure GetUsuariosConcatByRoleCoberturaId - Para cargar detalles de asignación.
        public async Task<ResponseGetUsuariosConcatByRoleIdDto> GetUsuariosConcatByRoleCoberturaId(InputsGetUsuariosByRoleCoberturaIdDto inputsDto) //ResponseGetUsuariosConcatByRoleIdDto
        {
            try
            {
                string Usuarios = "";
                string Ids = "";

                string sql = "EXEC [dbo].[GetUsuariosByRoleCoberturaId] @CoberturaId";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters
                    new SqlParameter { ParameterName = "@CoberturaId", Value = inputsDto.CoberturaId}
                };

                var Data = await _dBAuditCACContext.ResponseGetUsuariosByRoleIdDto.FromSqlRaw<ResponseGetUsuariosByRoleIdDto>(sql, parms.ToArray()).ToListAsync();

                if (Data.Count > 0) {
                    foreach (var item in Data)
                    {
                        Usuarios += " - " + item.UserName;
                        Ids += " - " + item.Id;
                    }

                    //Eliminamos caracteres no deseados al inicio.
                    Usuarios = Usuarios.Substring(2, (Usuarios.Length - 2));
                    Ids = Ids.Substring(2, (Ids.Length - 2));
                }

                ResponseGetUsuariosConcatByRoleIdDto ObjUsuarios = new ResponseGetUsuariosConcatByRoleIdDto();
                ObjUsuarios.Usuarios = Usuarios;
                ObjUsuarios.Id = Ids;


                return ObjUsuarios;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        #region Validacion Estados

        /// <summary>
        /// Consulta validaciones para registro auditoria detalle
        /// </summary>
        /// <param name="registroAuditoriaId">Id registro auditoria</param>
        /// <param name="userId">Id ausuario</param>
        /// <returns>Modelo Validacion</returns>
        public async Task<ResponseValidacionEstado> GetValidacionesRegistroAuditoriaDetalle(int registroAuditoriaId, string userId, int buttonAction)
        {
            try
            {
                int? estado = _dBAuditCACContext.RegistrosAuditoriaModel.Where(x => x.Id == registroAuditoriaId).Select(x => x.Estado).FirstOrDefault();


                if (estado == (int)Enumeration.EstadoRegistroAuditoria.Registronuevo || estado == (int)Enumeration.EstadoRegistroAuditoria.Registropendiente)
                {
                    var result = await ValidacionEstadoRN(registroAuditoriaId);
                    return result;
                }
                else if (estado == (int)Enumeration.EstadoRegistroAuditoria.Glosaenrevisionporlaentidad)
                {
                    var result = await ValidacionEstadoGRE(registroAuditoriaId);
                    return result;
                }
                else if (estado == (int)Enumeration.EstadoRegistroAuditoria.Glosaobjetada1)
                {
                    var result = await ValidacionEstadoGO1(registroAuditoriaId, buttonAction);
                    return result;
                }
                else if (estado == (int)Enumeration.EstadoRegistroAuditoria.Glosaobjetada2)
                {
                    var result = await ValidacionEstadoGO2(registroAuditoriaId, buttonAction, userId);
                    return result;
                }
                else if (estado == (int)Enumeration.EstadoRegistroAuditoria.Comiteadministrativo)
                {
                    var result = await ValidacionEstadoCA(registroAuditoriaId, buttonAction, userId);
                    return result;
                }
                else if (estado == (int)Enumeration.EstadoRegistroAuditoria.Comiteexpertos)
                {
                    var result = await ValidacionEstadoCE(registroAuditoriaId, buttonAction, userId);
                    return result;
                }
                else if (estado == (int)Enumeration.EstadoRegistroAuditoria.Errorlogicamarcaciónauditor)
                {
                    var result = await ValidacionEstadoELA(registroAuditoriaId, buttonAction, userId);
                    return result;
                }
                else if (estado == (int)Enumeration.EstadoRegistroAuditoria.Errorlogicamarcacionlíder)
                {
                    var result = await ValidacionEstadoELL(registroAuditoriaId, buttonAction, userId);
                    return result;
                }
                else if (estado == (int)Enumeration.EstadoRegistroAuditoria.Hallazgo1)
                {
                    var result = await ValidacionEstadoH1(registroAuditoriaId, buttonAction, userId);
                    return result;
                }
                else if (estado == (int)Enumeration.EstadoRegistroAuditoria.Hallazgo2lider)
                {
                    var result = await ValidacionEstadoH2L(registroAuditoriaId, buttonAction, userId);
                    return result;
                }
                else if (estado == (int)Enumeration.EstadoRegistroAuditoria.Hallazgo2auditor)
                {
                    var result = await ValidacionEstadoH2A(registroAuditoriaId, buttonAction, userId);
                    return result;
                }
                else if (estado == (int)Enumeration.EstadoRegistroAuditoria.Registrocerrado)
                {
                    var result = await ValidacionEstadoRC(registroAuditoriaId, buttonAction, userId);
                    return result;
                }
                else
                {
                    return new ResponseValidacionEstado();
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }




        /// <summary>
        /// Consulta validaciones para registro auditoria detalle cuando el estado es registro nuevo
        /// </summary>
        /// <param name="registroAuditoriaId">Id registro auditoria</param>
        /// <returns>Modelo Validacion</returns>
        public async Task<ResponseValidacionEstado> ValidacionEstadoRN(int registroAuditoriaId)
        {
            try
            {
                string sql = "EXEC SP_Validacion_Estado_RN @RegistroAuditoriaId";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters
                    new SqlParameter { ParameterName = "@RegistroAuditoriaId", Value = registroAuditoriaId}
                };

                var data = await _dBAuditCACContext.ResponseValidacionEstado.FromSqlRaw<ResponseValidacionEstado>(sql, parms.ToArray()).ToListAsync();
                return data[0];
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Consulta validaciones para registro auditoria detalle cuando el estado glosa en revision por la entidad GRE
        /// </summary>
        /// <param name="registroAuditoriaId">Id registro auditoria</param>
        /// <returns>Modelo Validacion</returns>
        public async Task<ResponseValidacionEstado> ValidacionEstadoGRE(int registroAuditoriaId)
        {
            try
            {
                string sql = "EXEC SP_Validacion_Estado_GRE @RegistroAuditoriaId";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters
                    new SqlParameter { ParameterName = "@RegistroAuditoriaId", Value = registroAuditoriaId}
                };

                var data = await _dBAuditCACContext.ResponseValidacionEstado.FromSqlRaw<ResponseValidacionEstado>(sql, parms.ToArray()).ToListAsync();
                return data[0];
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Consulta validaciones para registro auditoria detalle cuando el estado es glolosa objetada 1 GO1
        /// </summary>
        /// <param name="registroAuditoriaId">Id registro auditoria</param>
        /// <returns>Modelo Validacion</returns>
        public async Task<ResponseValidacionEstado> ValidacionEstadoGO1(int registroAuditoriaId, int buttonAction)
        {
            try
            {
                string sql = "EXEC SP_Validacion_Estado_GO1 @RegistroAuditoriaId, @BotonAccion";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters
                    new SqlParameter { ParameterName = "@RegistroAuditoriaId", Value = registroAuditoriaId},
                    new SqlParameter { ParameterName = "@BotonAccion", Value = buttonAction}

                };

                var data = await _dBAuditCACContext.ResponseValidacionEstado.FromSqlRaw<ResponseValidacionEstado>(sql, parms.ToArray()).ToListAsync();
                return data[0];
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Consulta validaciones para registro auditoria detalle cuando el estado es glolosa objetada 2 GO2
        /// </summary>
        /// <param name="registroAuditoriaId">Id registro auditoria</param>
        /// <returns>Modelo Validacion</returns>
        public async Task<ResponseValidacionEstado> ValidacionEstadoGO2(int registroAuditoriaId, int buttonAction, string userId)
        {
            try
            {
                string sql = "EXEC SP_Validacion_Estado_GO2 @RegistroAuditoriaId, @BotonAccion, @IdUsuario";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters
                    new SqlParameter { ParameterName = "@RegistroAuditoriaId", Value = registroAuditoriaId},
                    new SqlParameter { ParameterName = "@BotonAccion", Value = buttonAction},
                    new SqlParameter { ParameterName = "@IdUsuario", Value = userId}

                };

                var data = await _dBAuditCACContext.ResponseValidacionEstado.FromSqlRaw<ResponseValidacionEstado>(sql, parms.ToArray()).ToListAsync();
                return data[0];
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Consulta validaciones para registro auditoria detalle cuando el estado es comite administrativo CA
        /// </summary>
        /// <param name="registroAuditoriaId">Id registro auditoria</param>
        /// <returns>Modelo Validacion</returns>
        public async Task<ResponseValidacionEstado> ValidacionEstadoCA(int registroAuditoriaId, int buttonAction, string userId)
        {
            try
            {
                string sql = "EXEC SP_Validacion_Estado_CA @RegistroAuditoriaId, @BotonAccion, @IdUsuario";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters
                    new SqlParameter { ParameterName = "@RegistroAuditoriaId", Value = registroAuditoriaId},
                    new SqlParameter { ParameterName = "@BotonAccion", Value = buttonAction},
                    new SqlParameter { ParameterName = "@IdUsuario", Value = userId}

                };

                var data = await _dBAuditCACContext.ResponseValidacionEstado.FromSqlRaw<ResponseValidacionEstado>(sql, parms.ToArray()).ToListAsync();
                return data[0];
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Consulta validaciones para registro auditoria detalle cuando el estado es comite experto CE
        /// </summary>
        /// <param name="registroAuditoriaId">Id registro auditoria</param>
        /// <returns>Modelo Validacion</returns>
        public async Task<ResponseValidacionEstado> ValidacionEstadoCE(int registroAuditoriaId, int buttonAction, string userId)
        {
            try
            {
                string sql = "EXEC SP_Validacion_Estado_CE @RegistroAuditoriaId, @BotonAccion, @IdUsuario";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters
                    new SqlParameter { ParameterName = "@RegistroAuditoriaId", Value = registroAuditoriaId},
                    new SqlParameter { ParameterName = "@BotonAccion", Value = buttonAction},
                    new SqlParameter { ParameterName = "@IdUsuario", Value = userId}

                };

                var data = await _dBAuditCACContext.ResponseValidacionEstado.FromSqlRaw<ResponseValidacionEstado>(sql, parms.ToArray()).ToListAsync();
                return data[0];
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Consulta validaciones para registro auditoria detalle cuando el estado es Error lógica marcación auditor ELA
        /// </summary>
        /// <param name="registroAuditoriaId">Id registro auditoria</param>
        /// <returns>Modelo Validacion</returns>
        public async Task<ResponseValidacionEstado> ValidacionEstadoELA(int registroAuditoriaId, int buttonAction, string userId)
        {
            try
            {
                string sql = "EXEC SP_Validacion_Estado_ELA @RegistroAuditoriaId, @BotonAccion, @IdUsuario";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters
                    new SqlParameter { ParameterName = "@RegistroAuditoriaId", Value = registroAuditoriaId},
                    new SqlParameter { ParameterName = "@BotonAccion", Value = buttonAction},
                    new SqlParameter { ParameterName = "@IdUsuario", Value = userId}

                };

                var data = await _dBAuditCACContext.ResponseValidacionEstado.FromSqlRaw<ResponseValidacionEstado>(sql, parms.ToArray()).ToListAsync();
                return data[0];
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Consulta validaciones para registro auditoria detalle cuando el estado es Error lógica marcación auditor ELA
        /// </summary>
        /// <param name="registroAuditoriaId">Id registro auditoria</param>
        /// <returns>Modelo Validacion</returns>
        public async Task<ResponseValidacionEstado> ValidacionEstadoELL(int registroAuditoriaId, int buttonAction, string userId)
        {
            try
            {
                string sql = "EXEC SP_Validacion_Estado_ELL @RegistroAuditoriaId, @BotonAccion, @IdUsuario";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters
                    new SqlParameter { ParameterName = "@RegistroAuditoriaId", Value = registroAuditoriaId},
                    new SqlParameter { ParameterName = "@BotonAccion", Value = buttonAction},
                    new SqlParameter { ParameterName = "@IdUsuario", Value = userId}

                };

                var data = await _dBAuditCACContext.ResponseValidacionEstado.FromSqlRaw<ResponseValidacionEstado>(sql, parms.ToArray()).ToListAsync();
                return data[0];
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }


        /// <summary>
        /// Consulta validaciones para registro auditoria detalle cuando el estado es hallazgos1 H1
        /// </summary>
        /// <param name="registroAuditoriaId">Id registro auditoria</param>
        /// <returns>Modelo Validacion</returns>
        public async Task<ResponseValidacionEstado> ValidacionEstadoH1(int registroAuditoriaId, int buttonAction, string userId)
        {
            try
            {
                string sql = "EXEC SP_Validacion_Estado_H1 @RegistroAuditoriaId, @BotonAccion";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters
                    new SqlParameter { ParameterName = "@RegistroAuditoriaId", Value = registroAuditoriaId},
                    new SqlParameter { ParameterName = "@BotonAccion", Value = buttonAction}

                };

                var data = await _dBAuditCACContext.ResponseValidacionEstado.FromSqlRaw<ResponseValidacionEstado>(sql, parms.ToArray()).ToListAsync();
                return data[0];
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }


        /// <summary>
        /// Consulta validaciones para registro auditoria detalle cuando el estado es Hallazgo 2 lider H2L
        /// </summary>
        /// <param name="registroAuditoriaId">Id registro auditoria</param>
        /// <returns>Modelo Validacion</returns>
        public async Task<ResponseValidacionEstado> ValidacionEstadoH2L(int registroAuditoriaId, int buttonAction, string userId)
        {
            try
            {
                string sql = "EXEC SP_Validacion_Estado_H2L @RegistroAuditoriaId, @BotonAccion, @IdUsuario";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters
                    new SqlParameter { ParameterName = "@RegistroAuditoriaId", Value = registroAuditoriaId},
                    new SqlParameter { ParameterName = "@BotonAccion", Value = buttonAction},
                    new SqlParameter { ParameterName = "@IdUsuario", Value = userId}

                };

                var data = await _dBAuditCACContext.ResponseValidacionEstado.FromSqlRaw<ResponseValidacionEstado>(sql, parms.ToArray()).ToListAsync();
                return data[0];
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Consulta validaciones para registro auditoria detalle cuando el estado es Hallazgo 2 Auditor  H2A
        /// </summary>
        /// <param name="registroAuditoriaId">Id registro auditoria</param>
        /// <returns>Modelo Validacion</returns>
        public async Task<ResponseValidacionEstado> ValidacionEstadoH2A(int registroAuditoriaId, int buttonAction, string userId)
        {
            try
            {
                string sql = "EXEC SP_Validacion_Estado_H2A @RegistroAuditoriaId, @BotonAccion, @IdUsuario";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters
                    new SqlParameter { ParameterName = "@RegistroAuditoriaId", Value = registroAuditoriaId},
                    new SqlParameter { ParameterName = "@BotonAccion", Value = buttonAction},
                    new SqlParameter { ParameterName = "@IdUsuario", Value = userId}

                };

                var data = await _dBAuditCACContext.ResponseValidacionEstado.FromSqlRaw<ResponseValidacionEstado>(sql, parms.ToArray()).ToListAsync();
                return data[0];
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Consulta validaciones para registro auditoria detalle cuando el estado es Registro Cerrado
        /// </summary>
        /// <param name="registroAuditoriaId">Id registro auditoria</param>
        /// <returns>Modelo Validacion</returns>
        public async Task<ResponseValidacionEstado> ValidacionEstadoRC(int registroAuditoriaId, int buttonAction, string userId)
        {
            try
            {
                string sql = "EXEC SP_Validacion_Estado_RC @RegistroAuditoriaId, @BotonAccion, @IdUsuario";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters
                    new SqlParameter { ParameterName = "@RegistroAuditoriaId", Value = registroAuditoriaId},
                    new SqlParameter { ParameterName = "@BotonAccion", Value = buttonAction},
                    new SqlParameter { ParameterName = "@IdUsuario", Value = userId}

                };

                var data = await _dBAuditCACContext.ResponseValidacionEstado.FromSqlRaw<ResponseValidacionEstado>(sql, parms.ToArray()).ToListAsync();
                return data[0];
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        #endregion

        /// <summary>
        /// Actualiza estado registro auditoria y registra seguimiento
        /// </summary>
        /// <param name="registroAuditoriaId">Id registro auditoria</param>
        /// <param name="userId">Id Usuario</param>
        /// <returns>Modelo validacion</returns>
        public async Task<ResponseValidacionEstado> ActualizaEstadoRegistroAuditoria(int registroAuditoriaId, string userId, int buttonAction)
        {
            try
            {
                string sql = "EXEC SP_Actualiza_Estado_Registro_Auditoria @userId, @RegistroAuditoriaId, @BotonAccion";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters   
                    new SqlParameter { ParameterName = "@userId", Value = userId},
                    new SqlParameter { ParameterName = "@RegistroAuditoriaId", Value = registroAuditoriaId},
                    new SqlParameter { ParameterName = "@BotonAccion", Value = buttonAction}
                };

                var Data = await _dBAuditCACContext.Database.ExecuteSqlRawAsync(sql, parms.ToArray());

                #region Return validation object

                var oReturn = await GetValidacionesRegistroAuditoriaDetalle(registroAuditoriaId, userId, buttonAction);

                return oReturn;
                #endregion
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }


        /// <summary>
        /// Acutaliza registro auditoria detalle, de forma masiva
        /// </summary>
        /// <param name="idRegistroAuditoria">Id registro auditoria</param>
        /// <param name="userId">Id usuario</param>
        /// <param name="buttonAction">Acción</param>
        /// <param name="input">Modelo Variables entrada</param>
        /// <returns>true</returns>
        public async Task<bool> ActualizarRegistroAuditoriaDetalleMultiple(int idRegistroAuditoria, string userId, int buttonAction, List<ResponseRegistroAuditoriaDetalle> input)
        {
            try
            {
                //Get Previous info for compare
                var queryPreviousData = await ConsultarRegistroAuditoriaDetallePorid(idRegistroAuditoria);


                //Declare variables for compare
                List<ResponseRegistroAuditoriaDetalle> previousObject = queryPreviousData;
                ResponseRegistroAuditoriaDetalle previousGroup;
                ResponseRegistroAuditoriaDetalle.Variable previousVariable;


                string scriptBase = "UPDATE [RegistrosAuditoriaDetalle] SET MotivoVariable = $Motivo, Dato_DC_NC_ND = $Calificacion WHERE Id = $id;";
                string scriptComplete = "";

                input?.All(x =>
                {
                    //Get Previous Group
                    previousGroup = previousObject.Where(po => po.idgrupo == x.idgrupo).FirstOrDefault();

                    x.variables?.All(v =>
                    {

                        //Get Previous Group
                        previousVariable = previousGroup.variables.Where(pv => pv.registroAuditoriaDetalleId == v.registroAuditoriaDetalleId).FirstOrDefault();

                        //compare data, if exist change, do update
                        if (v.motivo != previousVariable.motivo ||
                            v.Dato_DC_NC_ND != previousVariable.Dato_DC_NC_ND)
                        {
                            //Add register to update
                            scriptComplete += scriptBase.Replace("$Motivo", v.motivo == "" ? "''" : "'" + v.motivo + "'")
                                                        .Replace("$Calificacion", v.Dato_DC_NC_ND == null ? "" : v.Dato_DC_NC_ND.ToString())
                                                        .Replace("$id", v.registroAuditoriaDetalleId.ToString());
                        }

                        return true;
                    });

                    return true;
                });

                //Update detail
                List<SqlParameter> parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@QueryUpdate", Value = scriptComplete},
                };

                if(scriptComplete != "")
                {
                    var Data = await _dBAuditCACContext.Database.ExecuteSqlRawAsync(scriptComplete, parms.ToArray());
                }

                //Update register Audit
                var exeUpdate = await ActualizaEstadoRegistroAuditoria(idRegistroAuditoria, userId, buttonAction);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Consulta errores de Logica
        /// </summary>
        /// <param name="userId">Id usuario</param>
        /// <param name="input">Modelo detalle registro auditoria</param>
        /// <returns>Modelo detalle registro auditoria</returns>
        public async Task<List<ResponseRegistroAuditoriaDetalle>> ValidarErrores(string userId, int registroAuditoriaId, List<ResponseRegistroAuditoriaDetalle> input)
        {
            try
            {
                #region Validar Errores


                var simulator = _dBAuditCACContext.ParametrosGenerales.Where(x => x.Nombre == "SimulatorErrors").Select(x => x.Valor).FirstOrDefault();
                string desc = "";

                if (Convert.ToBoolean(simulator)) //Simulation on
                {

                    var validacionErrores = await ValidarErroresRegistroAuditoria(registroAuditoriaId, userId);

                   int id = input[0].variables[0].registroAuditoriaDetalleId;


                    input?.All(x =>
                    {
                        int countErrores = 0;
                        x.variables?.All(v =>
                        {
                            v.error = validacionErrores.Where(x => x.VariableId == v.VariableId && x.RegistrosAuditoriaDetalleId == v.registroAuditoriaDetalleId).ToList();
                            countErrores += validacionErrores.Where(x => x.VariableId == v.VariableId).Count();
                            return true;
                        });

                        x.error = countErrores == 0 ? "" : countErrores.ToString();
                        return true;
                    });

                }
                else
                {
                    return input;
                }

                #endregion
                


                return input;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }


        /// <summary>
        /// Consulta erorres por id de registro auditoria
        /// </summary>
        /// <param name="registroAuditoriaId">id de registro auditoria</param>
        /// <returns>Modelo Errores registro auditoria</returns>
        public async Task<List<RegistroAuditoriaDetalleErrorModel>> ConsultarErroresRegistrosAuditoria(int registroAuditoriaId)
        {
            try
            {

                string sql = "EXEC [dbo].[SP_Consulta_ErrorRegistroAuditoria_Corregible] @RegistrosAuditoriaId";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters   
                    new SqlParameter { ParameterName = "@RegistrosAuditoriaId", Value = registroAuditoriaId}
                };

                var Data = await _dBAuditCACContext.RegistroAuditoriaDetalleErrorModel.FromSqlRaw<RegistroAuditoriaDetalleErrorModel>(sql, parms.ToArray()).ToListAsync();


                return Data;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }


        /// <summary>
        /// Consulta erorres por id de registro auditoria
        /// </summary>
        /// <param name="registroAuditoriaId">id de registro auditoria</param>
        /// <returns>Modelo Errores registro auditoria</returns>
        public async Task<List<RegistroAuditoriaDetalleErrorGroupModel>> ConsultarErroresRegistrosAuditoriaAgrupado(int registroAuditoriaId)
        {
            try
            {
                List<RegistroAuditoriaDetalleErrorGroupModel> oReturn = new List<RegistroAuditoriaDetalleErrorGroupModel>();

                string sql = "EXEC [dbo].[SP_Consulta_ErrorRegistroAuditoria] @RegistrosAuditoriaId";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters   
                    new SqlParameter { ParameterName = "@RegistrosAuditoriaId", Value = registroAuditoriaId}
                };

                var Data = await _dBAuditCACContext.RegistroAuditoriaDetalleErrorModel.FromSqlRaw<RegistroAuditoriaDetalleErrorModel>(sql, parms.ToArray()).ToListAsync();

                //Agrupa respuesta por id de regla
                var group = Data.GroupBy(x => new { x.ErrorId, x.VariableId }).Select(x => new RegistroAuditoriaDetalleErrorGroupModel()
                {
                    IdError = x.Key.ErrorId,
                    VariableId = x.Key.VariableId
                }).ToList();



                group?.All(x =>
                {
                    RegistroAuditoriaDetalleErrorGroupModel regla = new RegistroAuditoriaDetalleErrorGroupModel();
                    regla.DescripcionError = Data.Where(d => d.ErrorId == x.IdError && d.VariableId == x.VariableId).Select(d => d.Descripcion).FirstOrDefault();
                    regla.IdError = x.IdError;
                    regla.RegistrosAuditoriaDetalleId = Data.Where(d => d.ErrorId == x.IdError && d.VariableId == x.VariableId).Select(d => d.RegistrosAuditoriaDetalleId).FirstOrDefault();
                    regla.VariableId = x.VariableId;
                    regla.Restricciones = Data.Where(d => d.ErrorId == x.IdError && d.VariableId == x.VariableId && d.Enable).ToList();
                    oReturn.Add(regla);
                    return true;
                });

                return oReturn;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        public void LimpiaErroresRegistrosAuditoria(int registroAuditoriaDetalleId, string errorId, int idRegla, int idRestriccion)
        {
            try
            {
                string sql = "EXEC [dbo].[SP_Limpia_ErrorRegistroAuditoria] @RegistroAuditoriaDetalleId, @ErrorId, @IdRegla, @IdRestriccion";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters   
                    new SqlParameter { ParameterName = "@RegistroAuditoriaDetalleId", Value = registroAuditoriaDetalleId},
                    new SqlParameter { ParameterName = "@ErrorId", Value = errorId},
                    new SqlParameter { ParameterName = "@IdRegla", Value = idRegla},
                    new SqlParameter { ParameterName = "@IdRestriccion", Value = idRestriccion}
                };

                var Data = _dBAuditCACContext.Database.ExecuteSqlRaw(sql, parms.ToArray());
                return;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Crea o Acutaliza registro auditoria error
        /// </summary>
        /// <param name="inputsDto">Modelo Error registro auditoria</param>
        /// <returns>Modelo Error registro auditoria</returns>
        public bool UpsertErroresRegistrosAuditoria(RegistroAuditoriaDetalleErrorModel inputsDto)
        {
            try
            {
                string sql = "EXEC [dbo].[SP_Upsert_ErrorRegistroAuditoria] @RegistrosAuditoriaDetalleId, @Reducido, @IdRegla, @IdRestriccion, @VariableId, @ErrorId, @Descripcion, @Enable, @Usuario, @NoCorregible";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters   
                    new SqlParameter { ParameterName = "@RegistrosAuditoriaDetalleId", Value = inputsDto.RegistrosAuditoriaDetalleId},
                    new SqlParameter { ParameterName = "@Reducido", Value = inputsDto.Reducido},
                    new SqlParameter { ParameterName = "@IdRegla", Value = inputsDto.IdRegla},
                    new SqlParameter { ParameterName = "@IdRestriccion", Value = inputsDto.IdRestriccion},
                    new SqlParameter { ParameterName = "@VariableId", Value = inputsDto.VariableId},
                    new SqlParameter { ParameterName = "@ErrorId", Value = inputsDto.ErrorId},
                    new SqlParameter { ParameterName = "@Descripcion", Value = inputsDto.Descripcion},
                    new SqlParameter { ParameterName = "@Enable", Value = inputsDto.Enable},
                    new SqlParameter { ParameterName = "@Usuario", Value = inputsDto.CreatedBy},
                    new SqlParameter { ParameterName = "@NoCorregible", Value = inputsDto.NoCorregible}
                };

                var Data = _dBAuditCACContext.Database.ExecuteSqlRaw(sql, parms.ToArray());
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Crea o Acutaliza registro auditoria error Masivo
        /// </summary>
        /// <param name="inputsDto">Modelo Error registro auditoria</param>
        /// <returns>Modelo Error registro auditoria</returns>
        public async Task<bool> UpsertErroresRegistrosAuditoriaMasivo(List<InputErroresRegistrosAuditoriaDto> inputsDto, string userId)
        {
            try
            {
                //_logger.LogInformation("Inicia Proceso de actualizacion de errores en Errores RegistrosAuditoria");                               

                //Declarar variables.
                int Count = 1;

                string sql = "EXEC [dbo].[SP_Upsert_ErrorRegistroAuditoria_Masivo] @Listado, @userId";
                List<SqlParameter> Parameter = new List<SqlParameter>
                { 
                    // Create parameters   
                    //new SqlParameter { ParameterName = "@Listado", Value = SqlDbType.Structured},
                    new SqlParameter { ParameterName = "@Listado", Value = SqlDbType.Structured},
                    new SqlParameter { ParameterName = "@userId", Value = userId}
                };
                //var Parameter = new SqlParameter("@Listado", SqlDbType.Structured);
                //Parameter = new SqlParameter("@userId", userId);

                #region Datos de la tabla
                DataTable ListData = new DataTable();
                ListData.Columns.Add("Id", typeof(int));
                ListData.Columns.Add("RegistrosAuditoriaDetalleId", typeof(int));
                ListData.Columns.Add("Reducido", typeof(string));
                ListData.Columns.Add("IdRegla", typeof(int));
                ListData.Columns.Add("IdRestriccion", typeof(int));
                ListData.Columns.Add("VariableId", typeof(int));
                ListData.Columns.Add("ErrorId", typeof(string));
                ListData.Columns.Add("Descripcion", typeof(string));
                ListData.Columns.Add("Enable", typeof(bool));
                //ListData.Columns.Add("Usuario", typeof(string));
                ListData.Columns.Add("NoCorregible", typeof(bool));
                #endregion

                //Recorremos Auditores recibidos (AuditoriesId).
                foreach (var item in inputsDto)
                {
                    //Asignamos registros
                    ListData.Rows.Add(Count, item.RegistrosAuditoriaDetalleId, item.Reducido, item.IdRegla, item.IdRestriccion, item.VariableId, item.ErrorId, item.Descripcion, item.Enable, item.NoCorregible);

                    Count++;
                }
                
                Parameter[0].Value = ListData;
                Parameter[0].TypeName = "dbo.DT_ErrorRegistroAuditoriaMasivo";
                var insertData = await _dBAuditCACContext.Database.ExecuteSqlRawAsync(sql, Parameter);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        ///// <summary>
        ///// Para Actualizar estados e insertar estados de la entidad.
        ///// </summary>
        ///// <param name="inputsDto">Modelo de datos</param>
        ///// <returns>Modelo de respuesta. Indica si finalizo con exito o error</returns>
        //public async Task<string> CambiarEstadoEntidad(InputsCambiarEstadoEntidadDto inputsDto)
        //{
        //    try
        //    {
        //        string Status = "ERROR";
        //        string sql = "EXEC [dbo].[SP_Cambiar_Estado_Entidad] @radicadoId, @Observacion";

        //        List<SqlParameter> parms = new List<SqlParameter>
        //        { 
        //            // Create parameters   
        //            //new SqlParameter { ParameterName = "@usuarioId", Value = inputsDto.usuarioId},
        //            new SqlParameter { ParameterName = "@radicadoId", Value = inputsDto.radicadoId},
        //            new SqlParameter { ParameterName = "@Observacion", Value = inputsDto.Observacion},
        //            //new SqlParameter { ParameterName = "@estadoAEstado", Value = inputsDto.estadoAEstado},                    
        //        };

        //        // var Data = _dBAuditCACContext.Database.ExecuteSqlRaw(sql, parms.ToArray());
        //        var dataResult = new List<ResponseCambiarEstadoEntidadDto>();
        //        //_dBAuditCACContext.Database.SetCommandTimeout(100);
        //        dataResult = await _dBAuditCACContext.ResponseCambiarEstadoEntidadDto.FromSqlRaw<ResponseCambiarEstadoEntidadDto>(sql, parms.ToArray()).ToListAsync();                              
        //        //_dBAuditCACContext.SaveChanges();

        //        if (dataResult != null)
        //        {
        //            Status = dataResult.FirstOrDefault().Status + ": " + dataResult.FirstOrDefault().Result;
        //        }

        //        return Status;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogInformation(ex.ToString());
        //        throw;
        //    }
        //}

        ///// <summary>
        ///// Para consultar estados estados de la entidad.
        ///// </summary>
        ///// <param name="inputsDto">Modelo de datos</param>
        ///// <returns>Modelo de respuesta.</returns>
        //public async Task<List<ResponseConsultarEstadosEntidadDto>> ConsultarEstadosEntidad(InputsConsultarEstadosEntidadDto inputsDto)
        //{
        //    try
        //    {
        //        string sql = "EXEC [dbo].[SP_Consultar_Estados_Entidad] @radicadoId, @EPSId";

        //        List<SqlParameter> parms = new List<SqlParameter>
        //        { 
        //            // Create parameters   
        //            new SqlParameter { ParameterName = "@radicadoId", Value = inputsDto.radicadoId},
        //            new SqlParameter { ParameterName = "@EPSId", Value = inputsDto.EPSId},
        //        };

        //        // var Data = _dBAuditCACContext.Database.ExecuteSqlRaw(sql, parms.ToArray());                
        //        var Data = await _dBAuditCACContext.ResponseConsultarEstadosEntidadDto.FromSqlRaw<ResponseConsultarEstadosEntidadDto>(sql, parms.ToArray()).ToListAsync();

        //        return Data;
        //    }
        //    catch (Exception ex)
        //    {
        //        _logger.LogInformation(ex.ToString());
        //        throw;
        //    }
        //}

        #region Calculadora

        /// <summary>
        /// Calculadora TFG
        /// </summary>
        /// <param name="input">Input Calculadora</param>
        /// <returns>Valor calculado</returns>
        public async Task<ResponseLlaveValor> CalculadoraTFG(InputCalculadoraTFG input)
        {
            try
            {
                string sql = "EXEC [dbo].[SP_Calculadora_TFG] @edad, @hombre, @creatinina, @peso, @estatura";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters   
                    new SqlParameter { ParameterName = "@edad", Value = input.Edad},
                    new SqlParameter { ParameterName = "@hombre", Value = input.hombre},
                    new SqlParameter { ParameterName = "@creatinina", Value = input.Creatinina},
                    new SqlParameter { ParameterName = "@peso", Value = input.Peso},
                    new SqlParameter { ParameterName = "@estatura", Value = input.Estatura}
                };

                // var Data = _dBAuditCACContext.Database.ExecuteSqlRaw(sql, parms.ToArray());                
                var Data = await _dBAuditCACContext.LLaveValor.FromSqlRaw<ResponseLlaveValor>(sql, parms.ToArray()).ToListAsync();

                return Data[0];
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Calculadora KRU
        /// </summary>
        /// <param name="input">Input Calculadora</param>
        /// <returns>Valor calculado</returns>
        public async Task<ResponseLlaveValor> CalculadoraKRU(InputCalculadoraKRU input)
        {
            try
            {
                string sql = "EXEC [dbo].[SP_Calculadora_KRU] @Hemodialisis, @NitrogenoUrinario, @VolumenUrinario, @BrunPre, @BrunPost";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters   
                    new SqlParameter { ParameterName = "@Hemodialisis", Value = input.Hemodialisis},
                    new SqlParameter { ParameterName = "@NitrogenoUrinario", Value = input.NitrogenoUrinario},
                    new SqlParameter { ParameterName = "@VolumenUrinario", Value = input.VolumenUrinario},
                    new SqlParameter { ParameterName = "@BrunPre", Value = input.BrunPre},
                    new SqlParameter { ParameterName = "@BrunPost", Value = input.BrunPost}
                };

                // var Data = _dBAuditCACContext.Database.ExecuteSqlRaw(sql, parms.ToArray());                
                var Data = await _dBAuditCACContext.LLaveValor.FromSqlRaw<ResponseLlaveValor>(sql, parms.ToArray()).ToListAsync();

                return Data[0];
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }


        /// <summary>
        /// Calculadora promedio
        /// </summary>
        /// <param name="input">Listado valores</param>
        /// <returns>Promedio</returns>
        public async Task<ResponseLlaveValor> CalculadoraPromedio(List<decimal> input)
        {
            try
            {
                var sum = input.Sum(x => x);
                var result = sum / input.Count();

                return new ResponseLlaveValor()
                {
                    Id = 1,
                    Valor = Decimal.Round(result,2).ToString()
                };
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }



        #endregion

        /// <summary>
        /// Consulta data necesaria de tablas referenciales para el registro a auditar
        /// </summary>
        /// <param name="registroAuditoriaId"></param>
        /// <returns></returns>
        public async Task<List<DataTablaReferencial_RegistroAuditoriaModel>> ConsultaDataTablasReferencialRegistroAuditoria(int registroAuditoriaId)
        {
            try
            {
                string sql = "EXEC [dbo].[Consulta_Data_TablasReferencial_RegistroAuditoria] @RegistroAuditoriaId";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters   
                    new SqlParameter { ParameterName = "@RegistroAuditoriaId", Value = registroAuditoriaId},
                };

                var Data = await _dBAuditCACContext.DataTablaReferencial_RegistroAuditoriaModel.FromSqlRaw<DataTablaReferencial_RegistroAuditoriaModel>(sql, parms.ToArray()).ToListAsync();
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
