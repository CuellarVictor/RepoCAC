using AuditCAC.Core.Core;
using AuditCAC.Dal.Data;
using AuditCAC.Domain.Dto;
using AuditCAC.Domain.Dto.RegistroAuditoria;
using AuditCAC.Domain.Entities;
using AuditCAC.Domain.Helpers;
//using AuditCAC.Domain.Helpers;
//using AuditCAC.MainCore.Module.Helpers;
using AuditCAC.MainCore.Module.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.MainCore.Module
{
    public class RegistrosAuditoriaManager : IRegistrosAuditoriaRepository<RegistrosAuditoriaModel>
    {
        private readonly DBAuditCACContext _dBAuditCACContext;
        //private readonly ValidarErroresRegistroAuditorias _validarErroresRegistroAuditorias;
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ILogger<RegistrosAuditoriaModel> _logger;

        //Constructor
        public RegistrosAuditoriaManager(DBAuditCACContext dBAuditCACContext, ILogger<RegistrosAuditoriaModel> logger) //, ValidarErroresRegistroAuditorias validarErroresRegistroAuditorias
        {
            this._dBAuditCACContext = dBAuditCACContext;
            _logger = logger;
            //this._validarErroresRegistroAuditorias = validarErroresRegistroAuditorias;
        }

        //Metodos.
        public async Task<IEnumerable<RegistrosAuditoriaModel>> GetAll()
        {
            try
            {
                return await _dBAuditCACContext.RegistrosAuditoriaModel.Where(x => x.Status == true).ToListAsync();
                //return await this.dBAuditCACContext.RegistrosAuditoriaModel.ToListAsync();
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        public async Task<RegistrosAuditoriaModel> Get(int id)
        {
            try
            {
                return await _dBAuditCACContext.RegistrosAuditoriaModel.Where(x => x.Status == true).FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        public async Task Add(RegistrosAuditoriaModel entity)
        {
            try
            {
                _dBAuditCACContext.RegistrosAuditoriaModel.Add(entity);
                await _dBAuditCACContext.SaveChangesAsync();
                //return NoContent;
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        public async Task Update(RegistrosAuditoriaModel dbEntity, RegistrosAuditoriaModel entity)
        {
            try
            {
                //dbEntity.Id = entity.Id;
                dbEntity.IdRadicado = entity.IdRadicado;
                dbEntity.IdMedicion = entity.IdMedicion;                
                dbEntity.IdPeriodo = entity.IdPeriodo;
                //dbEntity.IdAuditor = entity.IdAuditor; //Revisar si esto va.
                dbEntity.PrimerNombre = entity.PrimerNombre;
                dbEntity.SegundoNombre = entity.SegundoNombre;
                dbEntity.PrimerApellido = entity.PrimerApellido;
                dbEntity.SegundoApellido = entity.SegundoApellido;
                dbEntity.Sexo = entity.Sexo;                
                dbEntity.TipoIdentificacion = entity.TipoIdentificacion;
                dbEntity.Identificacion = entity.Identificacion;
                dbEntity.FechaNacimiento = entity.FechaNacimiento;
                dbEntity.FechaCreacion = entity.FechaCreacion;
                dbEntity.FechaAuditoria = entity.FechaAuditoria;

                dbEntity.FechaMinConsultaAudit = entity.FechaMinConsultaAudit;
                dbEntity.FechaMaxConsultaAudit = entity.FechaMaxConsultaAudit;
                
                dbEntity.FechaAsignacion = entity.FechaAsignacion;

                dbEntity.Activo = entity.Activo;
                dbEntity.Conclusion = entity.Conclusion;
                dbEntity.UrlSoportes = entity.UrlSoportes;
                dbEntity.Reverse = entity.Reverse;
                dbEntity.DisplayOrder = entity.DisplayOrder;
                dbEntity.Ara = entity.Ara;
                dbEntity.Eps = entity.Eps;
                dbEntity.FechaReverso = entity.FechaReverso;
                dbEntity.AraAtendido = entity.AraAtendido;
                dbEntity.EpsAtendido = entity.EpsAtendido;
                dbEntity.Revisar = entity.Revisar;
                dbEntity.Extemporaneo = entity.Extemporaneo;
                dbEntity.Estado = entity.Estado;
                dbEntity.LevantarGlosa = entity.LevantarGlosa;
                dbEntity.MantenerCalificacion = entity.MantenerCalificacion;
                dbEntity.ComiteExperto = entity.ComiteExperto;
                dbEntity.ComiteAdministrativo = entity.ComiteAdministrativo;
                dbEntity.AccionLider = entity.AccionLider;
                dbEntity.AccionAuditor = entity.AccionAuditor;
                dbEntity.Encuesta = entity.Encuesta;
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

        public async Task Delete(RegistrosAuditoriaModel entity)
        {
            try
            {
                //_dBAuditCACContext.RegistrosAuditoriaModel.Remove(entity);
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

        //Metodo para llamar procedure - Transferencia de registro entre bolsas de la enfermedad madre.
        public async Task MoverRegistrosAuditoria(InputsMoverRegistrosAuditoriaDto inputs)
        {
            try
            {
                string sql = "EXEC [dbo].[MoverRegistrosAuditoria] @Id, @IdMedicion";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    new SqlParameter { ParameterName = "@Id", Value = inputs.Id},
                    new SqlParameter { ParameterName = "@IdMedicion", Value = inputs.IdMedicion},

                };

                var Data = await _dBAuditCACContext.Database.ExecuteSqlRawAsync(sql, parms.ToArray());
                //ResponseMoverRegistrosAuditoriaDto
                //var Data = await _dBAuditCACContext.RegistrosAuditoriaModel.FromSqlRaw<RegistrosAuditoriaModel>(sql, parms.ToArray()).ToListAsync();
                //return Data;
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        //Metodo para llamar procedure RegistrosAuditoriaAsignadoAuditorEstado - Para la consulta de los registros asignados a un auditor y a un estado      
        public async Task<List<ResponseRegistrosAuditoriaAsignadoAuditorEstadoDto>> GetRegistrosAuditoriaAsignadoAuditorEstado(InputsRegistrosAuditoriaAsignadoAuditorEstadoDto inputsDto) //ResponseRegistrosAuditoriaAsignadoAuditorEstadoDto  RegistrosAuditoriaModel
        {
            try
            {
                string sql = "EXEC [dbo].[RegistrosAuditoriaAsignadoAuditorEstado] @PageNumber, @MaxRows, @IdAuditor, @Estado";

                List<SqlParameter> parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@PageNumber", Value = inputsDto.PageNumber},
                    new SqlParameter { ParameterName = "@MaxRows", Value = inputsDto.MaxRows},
                    //                    
                    new SqlParameter { ParameterName = "@IdAuditor", Value = inputsDto.IdAuditor},
                    new SqlParameter { ParameterName = "@Estado", Value = inputsDto.Estado},

                };

                var Data = await _dBAuditCACContext.ResponseRegistrosAuditoriaAsignadoAuditorEstadoDto.FromSqlRaw<ResponseRegistrosAuditoriaAsignadoAuditorEstadoDto>(sql, parms.ToArray()).ToListAsync();
                return Data;
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        //Metodo para llamar procedure GetRegistrosAuditoriaFiltrado - Para la consulta de los registros de registro auditoria   
        public async Task<Tuple<List<ResponseRegistrosAuditoriaModelDto>, int, int, int>> GetRegistrosAuditoriaFiltrado(InputsRegistrosAuditoriaFiltradoDto inputsDto) //RegistrosAuditoriaResponseModel, ResponseRegistrosAuditoriaModelDto
        {
            try
            {

                var input = Newtonsoft.Json.JsonConvert.SerializeObject(inputsDto);
                _logger.LogInformation(input.Replace("\n",""));

                //string sql = "EXEC [dbo].[GetRegistrosAuditoria] @PageNumber, @MaxRows, @Id, @IdRadicado, @IdMedicion, @IdPeriodo, @IdAuditor, @IdLider, @PrimerNombre, @SegundoNombre, @PrimerApellido, @SegundoApellido, @Sexo, @TipoIdentificacion, @Identificacion, @FechaNacimientoIni, @FechaNacimientoFin, @FechaCreacionIni, @FechaCreacionFin, @FechaAuditoriaIni, @FechaAuditoriaFin, @FechaMinConsultaAudit, @FechaMaxConsultaAudit, @FechaAsignacionIni, @FechaAsignacionFin, @Activo, @Conclusion, @UrlSoportes, @Reverse, @DisplayOrder, @Ara, @Eps, @FechaReversoIni, @FechaReversoFin, @AraAtendido, @EpsAtendido, @Revisar, @Estado, @AccionLider, @AccionAuditor, @CodigoEps";

                #region OBSOLETO. Para agrupar cuando son Hepatitis
                //// Consultamos Ids de Mediciones de Hepatitis.
                //var IdsHEPATITIS = _dBAuditCACContext.ParametrosGenerales.Where(x => x.Id == (int)Enumeration.Parametros.IdsHEPATITIS && x.Activo == true).Select(x => new { x.Valor }).FirstOrDefault().Valor;
                //if (IdsHEPATITIS == null || IdsHEPATITIS == "") { IdsHEPATITIS = "9,10,11"; } //Valor por defecto si no encuentra parametro.
                //// --

                //// Convertimos Ids en un Listado
                //var Ids = IdsHEPATITIS.Split(',').Select(Int32.Parse).ToList();
                //// --

                //// Consultamos Enfermedad madre
                //int Valid = 0;
                //if (inputsDto.IdMedicion != "")
                //{
                //    Valid = _dBAuditCACContext.MedicionModel.Where(x => Ids.Contains((int)x.IdCobertura) && x.Status == true && x.Id == int.Parse(inputsDto.IdMedicion)).Count();
                //}
                //// --

                //// Validamos si es Hepatitis.
                //string sql = "";
                //if (Valid == 0)
                //{
                //    sql = "EXEC [dbo].[GetRegistrosAuditoria] @PageNumber, @MaxRows, @Id, @IdRadicado, @IdMedicion, @IdPeriodo, @IdAuditor, @IdLider, @PrimerNombre, @SegundoNombre, @PrimerApellido, @SegundoApellido, @Sexo, @TipoIdentificacion, @Identificacion, @FechaNacimientoIni, @FechaNacimientoFin, @FechaCreacionIni, @FechaCreacionFin, @FechaAuditoriaIni, @FechaAuditoriaFin, @FechaMinConsultaAudit, @FechaMaxConsultaAudit, @FechaAsignacionIni, @FechaAsignacionFin, @Activo, @Conclusion, @UrlSoportes, @Reverse, @DisplayOrder, @Ara, @Eps, @FechaReversoIni, @FechaReversoFin, @AraAtendido, @EpsAtendido, @Revisar, @Estado, @AccionLider, @AccionAuditor, @CodigoEps";
                //}
                //else
                //{
                //    sql = "EXEC [dbo].[GetRegistrosAuditoriaHepatitis] @PageNumber, @MaxRows, @Id, @IdRadicado, @IdMedicion, @IdPeriodo, @IdAuditor, @IdLider, @PrimerNombre, @SegundoNombre, @PrimerApellido, @SegundoApellido, @Sexo, @TipoIdentificacion, @Identificacion, @FechaNacimientoIni, @FechaNacimientoFin, @FechaCreacionIni, @FechaCreacionFin, @FechaAuditoriaIni, @FechaAuditoriaFin, @FechaMinConsultaAudit, @FechaMaxConsultaAudit, @FechaAsignacionIni, @FechaAsignacionFin, @Activo, @Conclusion, @UrlSoportes, @Reverse, @DisplayOrder, @Ara, @Eps, @FechaReversoIni, @FechaReversoFin, @AraAtendido, @EpsAtendido, @Revisar, @Estado, @AccionLider, @AccionAuditor, @CodigoEps";
                //}
                // --
                #endregion                

                string sql = "EXEC [dbo].[GetRegistrosAuditoria] @PageNumber, @MaxRows, @Id, @IdRadicado, @IdMedicion, @IdPeriodo, @IdAuditor, @IdLider, @PrimerNombre, @SegundoNombre, @PrimerApellido, @SegundoApellido, @Sexo, @TipoIdentificacion, @Identificacion, @FechaNacimientoIni, @FechaNacimientoFin, @FechaCreacionIni, @FechaCreacionFin, @FechaAuditoriaIni, @FechaAuditoriaFin, @FechaMinConsultaAudit, @FechaMaxConsultaAudit, @FechaAsignacionIni, @FechaAsignacionFin, @Activo, @Conclusion, @UrlSoportes, @Reverse, @DisplayOrder, @Ara, @Eps, @FechaReversoIni, @FechaReversoFin, @AraAtendido, @EpsAtendido, @Revisar, @Estado, @AccionLider, @AccionAuditor, @CodigoEps";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters    
                    new SqlParameter { ParameterName = "@PageNumber", Value = inputsDto.PageNumber},
                    new SqlParameter { ParameterName = "@MaxRows", Value = inputsDto.MaxRows},
                    //                    
                    new SqlParameter { ParameterName = "@Id", Value = inputsDto.Id},
                    new SqlParameter { ParameterName = "@IdRadicado", Value = inputsDto.IdRadicado},
                    new SqlParameter { ParameterName = "@IdMedicion", Value = inputsDto.IdMedicion},
                    new SqlParameter { ParameterName = "@IdPeriodo", Value = inputsDto.IdPeriodo},
                    new SqlParameter { ParameterName = "@IdAuditor", Value = inputsDto.IdAuditor},
                    new SqlParameter { ParameterName = "@IdLider", Value = inputsDto.IdLider},                    
                    new SqlParameter { ParameterName = "@PrimerNombre", Value = inputsDto.PrimerNombre},
                    new SqlParameter { ParameterName = "@SegundoNombre", Value = inputsDto.SegundoNombre},
                    new SqlParameter { ParameterName = "@PrimerApellido", Value = inputsDto.PrimerApellido},
                    new SqlParameter { ParameterName = "@SegundoApellido", Value = inputsDto.SegundoApellido},
                    new SqlParameter { ParameterName = "@Sexo", Value = inputsDto.Sexo},                    
                    new SqlParameter { ParameterName = "@TipoIdentificacion", Value = inputsDto.TipoIdentificacion},
                    new SqlParameter { ParameterName = "@Identificacion", Value = inputsDto.Identificacion},
                    
                    new SqlParameter { ParameterName = "@FechaNacimientoIni", Value = inputsDto.FechaNacimientoIni},
                    new SqlParameter { ParameterName = "@FechaNacimientoFin", Value = inputsDto.FechaNacimientoFin},

                    new SqlParameter { ParameterName = "@FechaCreacionIni", Value = inputsDto.FechaCreacionIni},
                    new SqlParameter { ParameterName = "@FechaCreacionFin", Value = inputsDto.FechaCreacionFin},

                    new SqlParameter { ParameterName = "@FechaAuditoriaIni", Value = inputsDto.FechaAuditoriaIni},
                    new SqlParameter { ParameterName = "@FechaAuditoriaFin", Value = inputsDto.FechaAuditoriaFin},

                    new SqlParameter { ParameterName = "@FechaMinConsultaAudit", Value = inputsDto.FechaMinConsultaAudit},
                    new SqlParameter { ParameterName = "@FechaMaxConsultaAudit", Value = inputsDto.FechaMaxConsultaAudit},

                    new SqlParameter { ParameterName = "@FechaAsignacionIni", Value = inputsDto.FechaAsignacionIni},
                    new SqlParameter { ParameterName = "@FechaAsignacionFin", Value = inputsDto.FechaAsignacionFin},

                    new SqlParameter { ParameterName = "@Activo", Value = inputsDto.Activo},
                    new SqlParameter { ParameterName = "@Conclusion", Value = inputsDto.Conclusion},
                    new SqlParameter { ParameterName = "@UrlSoportes", Value = inputsDto.UrlSoportes},
                    new SqlParameter { ParameterName = "@Reverse", Value = inputsDto.Reverse},
                    new SqlParameter { ParameterName = "@DisplayOrder", Value = inputsDto.DisplayOrder},
                    new SqlParameter { ParameterName = "@Ara", Value = inputsDto.Ara},
                    new SqlParameter { ParameterName = "@Eps", Value = inputsDto.Eps},
                    
                    new SqlParameter { ParameterName = "@FechaReversoIni", Value = inputsDto.FechaReversoIni},
                    new SqlParameter { ParameterName = "@FechaReversoFin", Value = inputsDto.FechaReversoFin},

                    new SqlParameter { ParameterName = "@AraAtendido", Value = inputsDto.AraAtendido},
                    new SqlParameter { ParameterName = "@EpsAtendido", Value = inputsDto.EpsAtendido},
                    new SqlParameter { ParameterName = "@Revisar", Value = inputsDto.Revisar},
                    new SqlParameter { ParameterName = "@Estado", Value = String.Join(", ", inputsDto.Estado.ToArray())}, //inputsDto.Estado 
                    new SqlParameter { ParameterName = "@AccionLider", Value = inputsDto.AccionLider},
                    new SqlParameter { ParameterName = "@AccionAuditor", Value = inputsDto.AccionAuditor},
                    new SqlParameter { ParameterName = "@CodigoEps", Value = inputsDto.CodigoEps}

                };


                _logger.LogInformation("Inicia Consulta DB Registros Auditoria");
                var Data = await _dBAuditCACContext.ResponseRegistrosAuditoriaModelDto.FromSqlRaw<ResponseRegistrosAuditoriaModelDto>(sql, parms.ToArray()).ToListAsync();
                _logger.LogInformation("Finaliza Consulta DB Registros Auditoria");

                var Query = "";
                if (Data.Count > 0)
                {
                    Query = Data.FirstOrDefault().QueryNoRegistrosTotales;
                }

                //Llamamos Procedure que calcula NoRegistrosTotalesFiltrado
                string sqlQuery = "EXEC [dbo].[GetCountPaginator] @Query";

                List<SqlParameter> parmsQuery = new List<SqlParameter>
                { 
                    new SqlParameter { ParameterName = "@Query", Value = Query},
                };

                _logger.LogInformation("Inicia Consulta Paginador DB Registros Auditoria");
                var DataQuery = await _dBAuditCACContext.ResponseQueryPaginatorDto.FromSqlRaw<ResponseQueryPaginatorDto>(sqlQuery, parmsQuery.ToArray()).ToListAsync();
                _logger.LogInformation("Finaliza Consulta Paginador DB Registros Auditoria");

               // List<RegistrosAuditoriaResponseModel> oReturn = new List<RegistrosAuditoriaResponseModel>();
                //if (Data.Count > 0)
                //{
                //    #region Regisro Auditoria  SP

                //    //Consulta la informacion adicional del registro auditoriad del sp SP_Consulta_RegistrosAuditoria_Info
                //    string idRegistroAuditoriaParametro = "";

                //    //Construye Id de registroAuditoria concatenado
                //    Data?.All(x =>
                //    {
                //        idRegistroAuditoriaParametro += x.Id + ",";
                //        return true;
                //    });

                //    idRegistroAuditoriaParametro = idRegistroAuditoriaParametro.Substring(0, idRegistroAuditoriaParametro.Length - 1);

                //    string sqlInfo = "EXEC [dbo].[SP_Consulta_RegistrosAuditoria_Info] @ListadoId";


                //    List<SqlParameter> parmsInfo = new List<SqlParameter>
                //                { 
                //                    // Create parameters   
                //                    new SqlParameter { ParameterName = "@ListadoId", Value = idRegistroAuditoriaParametro},
                //                };

                //    var dataInfo = await _dBAuditCACContext.RegistroAuditoriaInfo.FromSqlRaw<RegistroAuditoriaInfo>(sqlInfo, parmsInfo.ToArray()).ToListAsync();
                //    dataInfo = dataInfo.OrderBy(x => x.RegistroAuditoriaId).ToList();
                //    var dataInfoGruoped = dataInfo
                //        .GroupBy(x => new { x.DatoReportado, x.TablaReferencial, x.CampoReferencial })
                //        .Select(y => new RegistroAuditoriaInfo()
                //        {
                //            TablaReferencial = y.Key.TablaReferencial,
                //            DatoReportado = y.Key.DatoReportado,
                //            CampoReferencial = y.Key.CampoReferencial
                //        }
                //        );
                //    #endregion

                //}
                //Obtenemos No Registros maximos a consultar y los agregamos al Header del Request (API).
                var Queryable = _dBAuditCACContext.RegistrosAuditoriaModel.AsQueryable();
                var NoRegistrosTotales = Queryable.Count();

                var NoRegistrosTotalesFiltrado = DataQuery.FirstOrDefault().NoRegistrosTotalesFiltrado;

                var TotalPages = NoRegistrosTotalesFiltrado / inputsDto.MaxRows;

                //Data?.All(x =>
                //{
                //    oReturn.Add(new RegistrosAuditoriaResponseModel(x));
                //    return true;
                //});

                Data.Select(c => { c.QueryNoRegistrosTotales = ""; return c; }).ToList();
                //Data.Select(c => { c.QueryNoRegistrosTotales = ""; return c; }).ToList();

                _logger.LogInformation("RETURN Consulta registros auditoria");
                return Tuple.Create(Data, NoRegistrosTotales, NoRegistrosTotalesFiltrado, TotalPages);
                //return Tuple.Create(Data, NoRegistrosTotales, NoRegistrosTotalesFiltrado, TotalPages);

                //return Data;
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        //Metodo para llamar procedure RegistrosAuditoriaFiltros - Para cargar los filtros usados en vista cronograma.  
        public async Task<List<ResponseRegistrosAuditoriaFiltros>> GetRegistrosAuditoriaFiltros(InputsRegistrosAuditoriaFiltrosDto inputsDto) //ResponseRegistrosAuditoriaFiltrosDto, RegistrosAuditoriaModel
        {
            try
            {
                string sql = "EXEC [dbo].[RegistrosAuditoriaFiltros] @IdAuditor";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters   
                    new SqlParameter { ParameterName = "@IdAuditor", Value = inputsDto.IdAuditor}                    
                };

                var Data = await _dBAuditCACContext.ResponseRegistrosAuditoriaFiltrosDto.FromSqlRaw<ResponseRegistrosAuditoriaFiltrosDto>(sql, parms.ToArray()).ToListAsync();
                
                var Result = new List<ResponseRegistrosAuditoriaFiltros>();

                var filtros = Data
                    .GroupBy(x => new { x.NombreFiltro})
                    .Select(y => new ResponseRegistrosAuditoriaFiltros()
                        {
                            NombreFiltro = y.Key.NombreFiltro
                        }
                    );

                filtros?.All(x =>
                {
                    var row = new ResponseRegistrosAuditoriaFiltros();
                    row.NombreFiltro = x.NombreFiltro;
                    row.Detalle = new List<Detalles>();

                    Data.Where(d => d.NombreFiltro == x.NombreFiltro)?.All(z =>
                    {
                        row.Detalle.Add(new Detalles()
                        {
                            Id = z.Id,
                            Valor = z.Valor
                        });
                        return true;
                    });
                    Result.Add(row);

                    return true;
                });

                return Result;

                #region COMENTADO POR CODIGO FUERA DE USO (ESTO ANTES IBA A SERVICIO DE TABLA REFERENCIALES).

                //#region REGISTRO AUDITORIA INFO

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

                //var entidad = Result.Where(x => x.NombreFiltro == "Entidad").FirstOrDefault();
                //if (entidad != null)
                //{

                //    entidad.Detalle?.All(x =>
                //    {
                //        requestInput.Add(new InputsTablasReferencialByValorReferencialDto()
                //        {
                //            TablaReferencial = x.Valor,
                //            ValorReferencial = x.Id
                //        });

                //        return true;
                //    });



                //    var aditionalInfo = _requestCore.requestGetTablasReferencialesPorId(_util.GetAndValidateParameter(parameterList, Enumeration.Parametros.URLAuditCACCoreCACServices), requestInput, tokenInfo.Token);

                //    #endregion

                //    #region Construir Data Respuesta

                //    entidad.Detalle.Select(c =>
                //    {
                //        c.Valor = aditionalInfo.Where(a =>a.Id == c.Id).Select(s => s.Nombre).FirstOrDefault(); return c;
                //    }).ToList();


                //    Result.Where(x => x.NombreFiltro == "Entidad").FirstOrDefault().Detalle = entidad.Detalle;

                //    #endregion

                //    #endregion

                //    return Result;
                //}
                //else {
                //    return Result;
                //}
                #endregion
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        //Metodo para llamar procedure RegistrosAuditoriaDetallesAsignacion - Para cargar detalles de asignación
        public async Task<List<DetalleAsignacionOutputModel>> GetRegistrosAuditoriaDetallesAsignacion(InputDestalleAsignacion inputsDto)
        {
            try
            {
                string sql = "EXEC [dbo].[RegistrosAuditoriaDetallesAsignacion] @Usuario, @AuditorLider";

                string idauditorParametro = "";
                //Construye Id de auditor concatenados
                inputsDto.IdAuditor?.All(x =>
                    {
                        idauditorParametro += x + ",";
                        return true;
                    });

                idauditorParametro = idauditorParametro.Substring(0, idauditorParametro.Length - 1);

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters   
                    new SqlParameter { ParameterName = "@Usuario", Value = idauditorParametro },
                    new SqlParameter { ParameterName = "@AuditorLider", Value = inputsDto.AuditorLider }
                };

                var Data = await _dBAuditCACContext.ResponseRegistrosAuditoriaDetallesAsignacionDto.FromSqlRaw<ResponseRegistrosAuditoriaDetallesAsignacionDto>(sql, parms.ToArray()).ToListAsync();

                List<DetalleAsignacionOutputModel> oReturn = new List<DetalleAsignacionOutputModel>();

                Data?.All(x =>
                {
                    oReturn.Add(new DetalleAsignacionOutputModel(x));
                    return true;
                });

                return oReturn;
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        //Metodo para llamar procedure RegistrosAuditoriaProgresoDiario - Para cargar el progeso diario
        public async Task<List<ResponseRegistrosAuditoriaProgresoDiarioDto>> GetRegistrosAuditoriaProgresoDiario(InputsRegistrosAuditoriaFiltrosDto inputsDto)
        {
            try
            {
                string sql = "EXEC [dbo].[RegistrosAuditoriaProgresoDiario] @IdAuditor";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters   
                    new SqlParameter { ParameterName = "@IdAuditor", Value = inputsDto.IdAuditor}
                };

                var Data = await _dBAuditCACContext.ResponseRegistrosAuditoriaProgresoDiarioDto.FromSqlRaw<ResponseRegistrosAuditoriaProgresoDiarioDto>(sql, parms.ToArray()).ToListAsync();
                return Data;
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        //Metodo para llamar procedure CambiarEstadoRegistroAuditoria - Para cambiar estado de RegistroAuditoria.
        public async Task CambiarEstadoRegistroAuditoria(InputsCambiarEstadoRegistroAuditoriaDto inputsDto) //public async Task<List<ResponseRegistrosAuditoriaFiltrosDto>> CambiarEstadoRegistroAuditoria(InputsCambiarEstadoRegistroAuditoriaDto inputsDto)
        {
            try
            {
                /* ZONA DE VALIDACION */
                //
                /* ZONA DE VALIDACION */

                //string sql = "EXEC [dbo].[CambiarEstadoRegistroAuditoria] @RegistroAuditoriaId, @IdEstadoNuevo";
                string sql = "EXEC [dbo].[CambiarEstadoRegistroAuditoria] @RegistroAuditoriaId, @Proceso, @Observacion, @EstadoAnterioId, @EstadoActual, @AsignadoA, @AsingadoPor, @CreatedBy";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters   
                    new SqlParameter { ParameterName = "@RegistroAuditoriaId", Value = inputsDto.RegistroAuditoriaId},
                    new SqlParameter { ParameterName = "@Proceso", Value = inputsDto.Proceso},
                    new SqlParameter { ParameterName = "@Observacion", Value = inputsDto.Observacion},
                    new SqlParameter { ParameterName = "@EstadoAnterioId", Value = inputsDto.EstadoAnterioId},
                    new SqlParameter { ParameterName = "@EstadoActual", Value = inputsDto.EstadoActual},
                    new SqlParameter { ParameterName = "@AsignadoA", Value = inputsDto.AsignadoA},
                    new SqlParameter { ParameterName = "@AsingadoPor", Value = inputsDto.AsingadoPor},
                    new SqlParameter { ParameterName = "@CreatedBy", Value = inputsDto.CreatedBy}
                };

                var Data = await _dBAuditCACContext.Database.ExecuteSqlRawAsync(sql, parms.ToArray());
                //return Data;
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        //Metodo para llamar procedure CambiarEstadoRegistroAuditoriaMasivo - Para cambiar estado de RegistroAuditoria.
        public async Task CambiarEstadoRegistroAuditoriaMasivo(InputsActualizarDC_NC_ND_MotivoDto inputsDto) //public async Task<List<ResponseRegistrosAuditoriaFiltrosDto>> CambiarEstadoRegistroAuditoriaMasivo(InputsActualizarDC_NC_ND_MotivoDto inputsDto)
        {
            try
            {
                ///* ZONA DE VALIDACION */
                //var Valid = _validarErroresRegistroAuditorias.Validate(inputsDto);
                //if (Valid == false)
                //{
                //    return false;
                //}
                ///* ZONA DE VALIDACION */

                //string sql = "EXEC [dbo].[CambiarEstadoRegistroAuditoriaMasivo] @RegistroAuditoriaId, @IdEstadoNuevo, @QueryUpdate";
                string sql = "EXEC [dbo].[CambiarEstadoRegistroAuditoriaMasivo] @RegistroAuditoriaId, @Proceso, @Observacion, @EstadoAnterioId, @EstadoActual, @AsignadoA, @AsingadoPor, @CreatedBy, @QueryUpdate";

                //Declaramos variables usadas.
                var RegistroAuditoriaDetalleId = "";
                var Dato_DC_NC_ND = "";
                var Motivo = "";
                
                var QueryUpdate = "";

                //Capturamos Listados.                
                var ListCalificaciones = inputsDto.ListadoCalificaciones; //[0].variables;
                foreach (var itemListCalificaciones in ListCalificaciones)
                {
                    foreach (var itemVariables in itemListCalificaciones.variables)
                    {
                        //Dato_DC_NC_ND = itemVariables.Dato_DC_NC_ND.ToString();
                        Dato_DC_NC_ND = (itemVariables.Dato_DC_NC_ND != null) ? itemVariables.Dato_DC_NC_ND.ToString() : "NULL"; //Si viene Null, guardar un Null.

                        //Motivo = itemVariables.motivo.ToString();
                        Motivo = (itemVariables.motivo != null) ? itemVariables.motivo.ToString() : "";

                        RegistroAuditoriaDetalleId = itemVariables.registroAuditoriaDetalleId.ToString();
                        //RegistroAuditoriaDetalleId = (itemVariables.registroAuditoriaDetalleId != null) ? itemVariables.registroAuditoriaDetalleId.ToString() : "";

                        //Validamos
                        if (Dato_DC_NC_ND != "null" || Motivo != "null" || RegistroAuditoriaDetalleId != "null")
                        {
                            QueryUpdate = QueryUpdate + "UPDATE [RegistrosAuditoriaDetalle] SET Dato_DC_NC_ND = " + Dato_DC_NC_ND + ", MotivoVariable = '" + Motivo + "' WHERE Id = " + RegistroAuditoriaDetalleId + ";";
                        }

                        //Concatenamos valores.
                        //RegistroAuditoriaDetalleId = RegistroAuditoriaDetalleId + ", " + itemVariables.registroAuditoriaDetalleId.ToString();
                        //Dato_DC_NC_ND = Dato_DC_NC_ND + ", " + itemVariables.Dato_DC_NC_ND.ToString();
                        //Motivo = Motivo + ", " + itemVariables.motivo.ToString();
                    }
                }

                //Eliminamos caracteres no deseados al inicio (, ).
                //Dato_DC_NC_ND = Dato_DC_NC_ND.Substring(1, Dato_DC_NC_ND.Length - 2);
                //Motivo = Motivo.Substring(1, Motivo.Length - 2);
                //RegistroAuditoriaDetalleId = RegistroAuditoriaDetalleId.Substring(1, RegistroAuditoriaDetalleId.Length - 2);                

                //Asignamos valores al procedimiento.
                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters   
                    //new SqlParameter { ParameterName = "@RegistroAuditoriaId", Value = inputsDto.RegistroAuditoriaId},
                    //new SqlParameter { ParameterName = "@IdEstadoNuevo", Value = inputsDto.IdEstadoNuevo},
                    new SqlParameter { ParameterName = "@RegistroAuditoriaId", Value = inputsDto.RegistroAuditoriaId},
                    new SqlParameter { ParameterName = "@Proceso", Value = inputsDto.Proceso},
                    new SqlParameter { ParameterName = "@Observacion", Value = inputsDto.Observacion},
                    new SqlParameter { ParameterName = "@EstadoAnterioId", Value = inputsDto.EstadoAnterioId},
                    new SqlParameter { ParameterName = "@EstadoActual", Value = inputsDto.EstadoActual},
                    new SqlParameter { ParameterName = "@AsignadoA", Value = inputsDto.AsignadoA},
                    new SqlParameter { ParameterName = "@AsingadoPor", Value = inputsDto.AsingadoPor},
                    new SqlParameter { ParameterName = "@CreatedBy", Value = inputsDto.CreatedBy},
                    new SqlParameter { ParameterName = "@QueryUpdate", Value = QueryUpdate},
                };

                //Ejecutamos procedimiento.
                var Data = await _dBAuditCACContext.Database.ExecuteSqlRawAsync(sql, parms.ToArray());
                //return Data;
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        //Metodo para consultar los Soportes adjuntos por una entidad.
        public async Task<List<RegistroAuditoriaSoporteModel>> GetSoportesEntidad(InputsSoportesEntidadDto inputsDto)
        //public List<ResponseSoportesEntidadDto> GetSoportesEntidad(InputsSoportesEntidadDto inputsDto)
        {
            try
            {
                //List<ResponseSoportesEntidadDto> Data = new List<ResponseSoportesEntidadDto>()
                //{
                //    new ResponseSoportesEntidadDto() { IdEntidad = 1, EntidadNombre = "EntidadFake1", NombreSoporte = "Historia clínica", UrlSoporte = "2346543HistoClinic014.pdf" },
                //    new ResponseSoportesEntidadDto() { IdEntidad = 1, EntidadNombre = "EntidadFake2", NombreSoporte = "Laboratorio", UrlSoporte = "2346543Laboratorio22.pdf" },
                //    new ResponseSoportesEntidadDto() { IdEntidad = 1, EntidadNombre = "EntidadFake3", NombreSoporte = "Medicamentos", UrlSoporte = "2346543Mecicamento.pdf" },
                //    new ResponseSoportesEntidadDto() { IdEntidad = 1, EntidadNombre = "EntidadFake4", NombreSoporte = "Fotocopia de la cedula 150%", UrlSoporte = "CedulaCliente.pdf" },
                //};

                string sql = "EXEC [dbo].[GetSoportesEntidad] @IdRegistrosAuditoria";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters   
                    new SqlParameter { ParameterName = "@IdRegistrosAuditoria", Value = inputsDto.IdRegistrosAuditoria}
                };

                var Data = await _dBAuditCACContext.RegistroAuditoriaSoporteModel.FromSqlRaw<RegistroAuditoriaSoporteModel>(sql, parms.ToArray()).ToListAsync();
                return Data;
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        //Metodo para llamar procedure ActualizarBanderasRegistrosAuditoria - Para actualizar campos bandera en RegistrosAuditoria.
        public async Task ActualizarBanderasRegistrosAuditoria(InputsActualizarBanderasRegistrosAuditoriaDto inputsDto)
        {
            try
            {                
                string sql = "EXEC [dbo].[ActualizarBanderasRegistrosAuditoria] @IdUnico, @LevantarGlosa, @MantenerCalificacion, @ComiteExperto, @ComiteAdministrativo, @AccionLider, @AccionAuditor";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters   
                    new SqlParameter { ParameterName = "@IdUnico", Value = inputsDto.IdUnico},
                    new SqlParameter { ParameterName = "@LevantarGlosa", Value = inputsDto.LevantarGlosa},
                    new SqlParameter { ParameterName = "@MantenerCalificacion", Value = inputsDto.MantenerCalificacion},
                    new SqlParameter { ParameterName = "@ComiteExperto", Value = inputsDto.ComiteExperto},
                    new SqlParameter { ParameterName = "@ComiteAdministrativo", Value = inputsDto.ComiteAdministrativo},
                    new SqlParameter { ParameterName = "@AccionLider", Value = inputsDto.AccionLider},
                    new SqlParameter { ParameterName = "@AccionAuditor", Value = inputsDto.AccionAuditor},
                };

                var Data = await _dBAuditCACContext.Database.ExecuteSqlRawAsync(sql, parms.ToArray());
                //return Data;
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        //Metodo para consultar la tificación correspondiente a cada estado.
        public async Task<List<ItemModel>> GetTipificaciónByEstadoId(InputsGetTipificaciónByEstadoDto inputsDto)
        {
            try
            {               
                //Consultamos todos los Items que pertenecen al Catalog 1 (Catalogo de TipoObservacion).
                var Data = await _dBAuditCACContext.ItemModel.Where(x => x.CatalogId == 1).ToListAsync();   

                switch (inputsDto.EstadoId)
                {
                    case 1:
                        //1, 2
                        Data =  Data.Where(x => x.Id == 1 || x.Id == 2).ToList();
                        break;
                    case 2:                        
                        Data.RemoveRange(0, Data.Count);
                        break;
                    case 3:
                        //3, 14
                        Data = Data.Where(x => x.Id == 3 || x.Id == 14).ToList();
                        break;
                    case 4:
                        Data.RemoveRange(0, Data.Count);
                        break;
                    case 5:
                        //12, 13, 14
                        Data = Data.Where(x => x.Id == 12 || x.Id == 13 || x.Id == 14).ToList();
                        break;
                    case 6:
                        //4
                        Data = Data.Where(x => x.Id == 4).ToList();
                        break;
                    case 7:
                        //1
                        Data = Data.Where(x => x.Id == 1).ToList();
                        break;
                    case 8:
                        //NINGUNA.
                        Data.RemoveRange(0, Data.Count);
                        break;
                    case 9:
                        //8
                        Data = Data.Where(x => x.Id == 8).ToList();
                        break;
                    case 10:
                        //9
                        Data = Data.Where(x => x.Id == 9).ToList();
                        break;
                    case 11:
                        //1, 14
                        Data = Data.Where(x => x.Id == 1 || x.Id == 14).ToList();
                        break;
                    case 12:
                        //6
                        Data = Data.Where(x => x.Id == 6).ToList();
                        break;
                    case 13:
                        Data.RemoveRange(0, Data.Count);
                        break;
                    case 14:
                        //6
                        Data = Data.Where(x => x.Id == 6).ToList();
                        break;
                    case 15:
                        //6
                        Data = Data.Where(x => x.Id == 6).ToList();
                        break;
                    case 16:
                        Data.RemoveRange(0, Data.Count);
                        break;
                    case 17:
                        //1, 2
                        Data = Data.Where(x => x.Id == 1 || x.Id == 2).ToList();
                        break;

                    default:
                        //Data = Data;
                        break;
                }

                return Data;
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        //Metodo para llamar procedure GetAlertasRegistrosAuditoria - Para consultar No de RegistrosAuditoria que solo puede resolver el lider.
        public async Task<List<ResponseAlertasRegistrosAuditoriaDto>> GetAlertasRegistrosAuditoria(InputsIdUsuarioDto inputsDto)
        {
            try
            {
                string sql = "EXEC [dbo].[GetAlertasRegistrosAuditoria] @IdUsuario";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters   
                    new SqlParameter { ParameterName = "@IdUsuario", Value = inputsDto.IdUsuario}
                };

                var Data = await _dBAuditCACContext.ResponseAlertasRegistrosAuditoriaDto.FromSqlRaw<ResponseAlertasRegistrosAuditoriaDto>(sql, parms.ToArray()).ToListAsync();
                return Data;
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        //Metodo para llamar procedure GetConsultaPerfilAccion - Para consultar Listado de Roles con perfiles de accion correspondientes.
        public async Task<List<ResponseGetConsultaPerfilAccionDetallesDto>> GetConsultaPerfilAccion() //InputsGetConsultaPerfilAccionDto inputsDto
        {
            try
            {
                string sql = "EXEC [dbo].[SP_Consulta_Perfil_Accion]";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters   
                    //new SqlParameter { ParameterName = "@IdUsuario", Value = inputsDto.IdUsuario}
                };

                var Data = await _dBAuditCACContext.ResponseGetConsultaPerfilAccionDto.FromSqlRaw<ResponseGetConsultaPerfilAccionDto>(sql, parms.ToArray()).ToListAsync();

                //Iniciamos un listado, del tipo que vamos a responder en el metodo.
                var Result = new List<ResponseGetConsultaPerfilAccionDetallesDto>();

                //if (Data.Count > 0)
                //{
                //    foreach (var item in Data)
                //    {
                //        var row = new ResponseGetConsultaPerfilAccionDetallesDto();
                //        row.RoleId = item.RoleId;
                //        row.RoleName = item.RoleName;
                //        row.ActionStatus = new List<ListaActionStatus>();
                //        foreach (var item2 in row.ActionStatus)
                //        {

                //        }
                //    }
                //}


                //Armamos objeto a responder
                //string split = "this, needs, to, split";
                //List<string> list = new List<string>();
                //list = split.Split(',').ToList();

                if (Data.Count > 0)
                {
                    Data?.All(x =>
                    {
                        var row = new ResponseGetConsultaPerfilAccionDetallesDto();
                        row.RoleId = x.RoleId;
                        row.RoleName = x.RoleName;
                        //var campo = x.ActionStatus.Split(',').ToList();
                        //row.ActionStatus = new List<string>(campo);
                        row.ActionStatus = x.ActionStatus.Split(',').ToList();
                       
                        //Data.Where(d => d.RoleId == x.RoleId)?.All(z =>
                        //{
                        //    //var campo = z.ActionStatus.Split(',').ToList();
                        //    row.ActionStatus.Add(new ListaActionStatus()
                        //    {
                        //        ActionStatus = {
                        //            z.ActionStatus.
                        //        }
                        //        z.ActionStatus.Split(',').ToList(),
                        //        //z.actionStatus.Split(',').ToList();
                        //    });
                        //    return true;
                        //});

                        Result.Add(row);

                        return true;
                    });

                    //return Result;                    
                }

                return Result;
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }


        /// <summary>
        /// Consulta usuarios asignados al lider
        /// </summary>
        /// <param name="idUsuarioLider">Id usuario lider</param>
        /// <returns>Modelo usuarios lider</returns>
        public async Task<List<ResponseUsuariosLider>> ConsultaUsuariosLider(string idUsuarioLider)
        {
            try
            {
                string sql = "EXEC SP_Consulta_Usuarios_Lider @idUserLider";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters   
                    new SqlParameter { ParameterName = "@idUserLider", Value = idUsuarioLider}
                };

                var Data = await _dBAuditCACContext.ResponseUsuariosLider.FromSqlRaw<ResponseUsuariosLider>(sql, parms.ToArray()).ToListAsync();
                return Data;
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Reversa un Registro Auditoria en estado finalizado
        /// </summary>
        /// <param name="inputsDto">Entrada de datos</param>
        /// <returns>modelo llave valor</returns>
        public async Task<ResponseLlaveValor> ReversarRegistrosAuditoria(InputsReversarRegistrosAuditoriaDto input)
        {
            try
            {
                var query = "EXEC SP_Reversar_RegistrosAuditoria @IdRegistrosAuditoria, @Estado, @Observacion, @IdUsuario;";

                List<SqlParameter> parametros = new List<SqlParameter> {
                        new SqlParameter{ ParameterName = "@IdRegistrosAuditoria", Value = input.IdRegistrosAuditoria },
                        new SqlParameter{ ParameterName = "@Estado", Value = input.Estado },
                        new SqlParameter{ ParameterName = "@Observacion", Value = input.Observacion },
                        new SqlParameter{ ParameterName = "@IdUsuario", Value = input.IdUsuario },
                };

                var Data = await _dBAuditCACContext.LLaveValor.FromSqlRaw<ResponseLlaveValor>(query, parametros.ToArray()).ToListAsync();

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
