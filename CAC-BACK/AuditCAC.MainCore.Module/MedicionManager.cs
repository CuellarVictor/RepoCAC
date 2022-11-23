using Amazon;
using Amazon.S3;
using Amazon.S3.Model;
using AuditCAC.Core.Core;
using AuditCAC.Dal.Data;
using AuditCAC.Domain.Dto;
using AuditCAC.Domain.Dto.Actas;
using AuditCAC.Domain.Dto.CalificacionMasiva;
using AuditCAC.Domain.Dto.CarguePoblacion;
using AuditCAC.Domain.Entities;
using AuditCAC.Domain.Helpers;
using AuditCAC.MainCore.Module.Interface;
using DocumentFormat.OpenXml.InkML;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.MainCore.Module
{
    public class MedicionManager : IMedicionRepository<MedicionModel>
    {
        private readonly DBAuditCACContext _dBAuditCACContext;
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ILogger<MedicionManager> _logger;
        private Util _util = new Util();
        private readonly IGenerateExcel _generate;

        IConfigurationRoot configuration;
        //Constructor
        public MedicionManager(DBAuditCACContext dBAuditCACContext, ILogger<MedicionManager> logger, IGenerateExcel generate)
        {           

            this._dBAuditCACContext = dBAuditCACContext;
            _dBAuditCACContext.Database.SetCommandTimeout(5000);
            _logger = logger;
            configuration = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json")
                 .Build();
            _generate = generate;

        }

        //Metodos.
        public async Task<IEnumerable<MedicionModel>> GetAll()
        {
            try
            {
                return await _dBAuditCACContext.MedicionModel.Where(x => x.Status == true).ToListAsync();
                //return await this.dBAuditCACContext.MedicionModel.ToListAsync();
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        public async Task<MedicionModel> Get(int id)
        {
            try
            {
                return await _dBAuditCACContext.MedicionModel.Where(x => x.Status == true).FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        public async Task<int> Add(MedicionModel entity)
        {
            try
            {

                _dBAuditCACContext.MedicionModel.Add(entity);
                await _dBAuditCACContext.SaveChangesAsync();

                //Llamamos SP para Insertar Logs de actividad.
                string sql = "EXEC [SP_Insertar_Process_Log] @ProcessId, @User, @Result, @Observation";
                List<SqlParameter> parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@ProcessId", Value = 3},
                    new SqlParameter { ParameterName = "@User", Value = entity.CreatedBy},
                    new SqlParameter { ParameterName = "@Result", Value = "OK"},
                    new SqlParameter { ParameterName = "@Observation", Value = "Gestor Mediciones: Nueva"}
                };

                var respuesta = _dBAuditCACContext.Database.ExecuteSqlRaw(sql, parms.ToArray());

                return entity.Id;
                //return NoContent;
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        public async Task Update(MedicionModel dbEntity, MedicionModel entity)
        {
            try
            {
                //dbEntity.Id = entity.Id;
                dbEntity.IdCobertura = entity.IdCobertura;
                dbEntity.IdPeriodo = entity.IdPeriodo;
                dbEntity.Descripcion = entity.Descripcion;
                dbEntity.Activo = entity.Activo;
                dbEntity.ModifyBy = entity.ModifyBy;
                dbEntity.ModifyDate = entity.ModifyDate;
                dbEntity.CreatedDate = entity.CreatedDate;
                dbEntity.Resolucion = entity.Resolucion;
                dbEntity.Estado = entity.Estado;
                dbEntity.FechaCorteAuditoria = entity.FechaCorteAuditoria;
                dbEntity.FechaFinAuditoria = entity.FechaFinAuditoria;
                dbEntity.FechaInicioAuditoria = entity.FechaInicioAuditoria;
                dbEntity.Lider = entity.Lider;
                dbEntity.Nombre = entity.Nombre;

                await _dBAuditCACContext.SaveChangesAsync();

                //Llamamos SP para Insertar Logs de actividad.                
                string sql = "EXEC [SP_Insertar_Process_Log] @ProcessId, @User, @Result, @Observation";
                List<SqlParameter> parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@ProcessId", Value = 11},
                    new SqlParameter { ParameterName = "@User", Value = entity.CreatedBy},
                    new SqlParameter { ParameterName = "@Result", Value = "OK"},
                    new SqlParameter { ParameterName = "@Observation", Value = "Gestor Mediciones: Editar"}
                };

                var respuesta = _dBAuditCACContext.Database.ExecuteSqlRaw(sql, parms.ToArray());

            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        public async Task Delete(MedicionModel entity)
        {
            try
            {
                //_dBAuditCACContext.MedicionModel.Remove(entity);
                //await _dBAuditCACContext.SaveChangesAsync();

                entity.Status = false;

                await _dBAuditCACContext.SaveChangesAsync();

                //Llamamos SP para Insertar Logs de actividad.
                string sql = "EXEC [SP_Eliminar_Tbl_LogRequests] @ProcessId, @User, @Result, @Observation";
                List<SqlParameter> parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@ProcessId", Value = 12},
                    new SqlParameter { ParameterName = "@User", Value = entity.CreatedBy},
                    new SqlParameter { ParameterName = "@Result", Value = "OK"},
                    new SqlParameter { ParameterName = "@Observation", Value = "Gestor Mediciones: Eliminar"}
                };

                var respuesta = _dBAuditCACContext.Database.ExecuteSqlRaw(sql, parms.ToArray());
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        public async Task<List<MedicionModel>> GetMedicionAll(InputsMedicionAllDto inputMedicionAll)
        {
            try
            {
                string sql = "EXEC [dbo].[GetMedicionCriterio] " +
                "@PageNumber, " +
                "@MaxRows, " +
                "@Id, " +
                "@IdCobertura, " +
                "@IdPeriodo, " +
                "@Descripcion, " +
                "@Activo, " +
                "@CreatedBy,	" +
                "@CreatedDate, " +
                "@ModifyBy," +
                "@ModifyDate";

                List<SqlParameter> parms = new List<SqlParameter>
                {
                     //Create parameters
                    new SqlParameter { ParameterName = "@PageNumber", Value = inputMedicionAll.PageNumber},
                    new SqlParameter { ParameterName = "@MaxRows", Value = inputMedicionAll.MaxRows},
                    //
                    new SqlParameter { ParameterName = "@Id", Value = inputMedicionAll.Id},
                    new SqlParameter { ParameterName = "@IdCobertura", Value = inputMedicionAll.IdCobertura},
                    new SqlParameter { ParameterName = "@IdPeriodo", Value = inputMedicionAll.IdPeriodo},
                    new SqlParameter { ParameterName = "@Descripcion", Value = inputMedicionAll.Descripcion},
                    new SqlParameter { ParameterName = "@Activo", Value = inputMedicionAll.Activo},
                    new SqlParameter { ParameterName = "@CreatedBy", Value = inputMedicionAll.CreatedBy},
                    new SqlParameter { ParameterName = "@CreatedDate", Value =inputMedicionAll.CreatedDate},
                    new SqlParameter { ParameterName = "@ModifyBy", Value = inputMedicionAll.ModifyBy},
                    new SqlParameter { ParameterName = "@ModifyDate", Value = inputMedicionAll.ModifyDate}
                };
                var Data = await _dBAuditCACContext.MedicionModel.FromSqlRaw<MedicionModel>(sql, parms.ToArray()).ToListAsync();
                return Data;
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }


        //metodo para llenar la pantalla de gestion de bolsas de auditoria
        //Task<Tuple<List<FiltrosBolsaMedicionDto>, int, int, int>> | Task<Tuple<FiltrosBolsaMedicionDto, int, int, int>>
        public async Task<Tuple<FiltrosBolsaMedicionDto, int, int, int>> GetFiltrosBolsaMedicion(InputGetFiltrosBolsaMedicionDto filtrosBolsaMedicionDto) //InputFiltrosBolsaMedicionDto filtrosBolsaMedicionDto
        {
            try
            {

                #region Hacer login y obtener token
                //Consulta parametros generales para request
                var parameterList = _dBAuditCACContext.ParametrosGenerales.Where(x => x.Activo).ToList();
                RequestCore _requestCore = new RequestCore();
                Util _util = new Util();
                var tokenInfo = _requestCore.requestAuthenticationCAC(
                    _util.GetAndValidateParameter(parameterList, Enumeration.Parametros.URLAuditCACCoreCACServices), //Url parameter
                    _util.GetAndValidateParameter(parameterList, Enumeration.Parametros.UsuarioTokenAuditCACCoreCACServices), //User Parameter
                    _util.GetAndValidateParameter(parameterList, Enumeration.Parametros.PasswordTokenAuditCACCoreCACServices) //Password Paramter
                    );
                #endregion

                #region obtener las enfermedades madre asociadas al lider
                string sql = "EXEC [dbo].[GetUsuarioXEnfermedad] " +
               "@PageNumber, " +
               "@MaxRows, " +
               "@IdLider ";

                List<SqlParameter> parms = new List<SqlParameter>
                {

                     //Create parameters
                    new SqlParameter { ParameterName = "@PageNumber", Value = filtrosBolsaMedicionDto.PageNumber},
                    new SqlParameter { ParameterName = "@MaxRows", Value = filtrosBolsaMedicionDto.MaxRows},
                    //
                    new SqlParameter { ParameterName = "@IdLider", Value = filtrosBolsaMedicionDto.IdUsuario}, //IdLider


                };
                var UsuarioXEnfermedad = await _dBAuditCACContext.GetFiltrosBolsaMedicion.FromSqlRaw<UsuarioXEnfermedadModel>(sql, parms.ToArray()).ToListAsync();


                #endregion

                #region obtener todas las mediciones

                string query = "EXEC [GetMedicionCriterio]" +
                   "@PageNumber, " +
                   "@MaxRows, " +
                   "@IdUsuario, " +
                   "@Id, " +
                   "@IdCobertura, " +
                   "@IdPeriodo, " +
                   "@Descripcion, " +
                   "@Activo, " +
                   "@CreatedBy,	" +
                   "@CreatedDate, " +
                   "@ModifyBy," +
                   "@ModifyDate";

                List<SqlParameter> parametros = new List<SqlParameter>
                { 

                    // Create parameters    
                    new SqlParameter { ParameterName = "@PageNumber", Value = 0},
                    new SqlParameter { ParameterName = "@MaxRows", Value = 1000},
                    //
                    new SqlParameter { ParameterName = "@Id", Value = ""},
                    new SqlParameter { ParameterName = "@IdUsuario", Value = ""},
                    new SqlParameter { ParameterName = "@IdCobertura", Value = ""},
                    new SqlParameter { ParameterName = "@IdPeriodo", Value = ""},
                    new SqlParameter { ParameterName = "@Descripcion", Value = ""},
                    new SqlParameter { ParameterName = "@Activo", Value = ""},
                    new SqlParameter { ParameterName = "@CreatedBy", Value = ""},
                    new SqlParameter { ParameterName = "@CreatedDate", Value = ""},
                    new SqlParameter { ParameterName = "@ModifyBy", Value = ""},
                    new SqlParameter { ParameterName = "@ModifyDate", Value = ""},
                };


                var listMediciones = await _dBAuditCACContext.ResponseMedicionCrudoDto.FromSqlRaw(query, parametros.ToArray()).ToListAsync(); //MedicionModel
                #endregion


                # region objeto de inicializacion de coberturas
                //objeto de inicializacion de coberturas

                InputsCoberturaDto CoberturaModel = new InputsCoberturaDto
                {
                    CantidadVariables = "",
                    definicion = "",
                    ExcluirNovedades = "",
                    idCobertura = filtrosBolsaMedicionDto.IdCobertura,
                    idCoberturaAdicionales = "",
                    idCoberturaPadre = "",
                    idResolutionSiame = "",
                    legislacion = "",
                    MaxRows = filtrosBolsaMedicionDto.MaxRows,
                    nemonico = "",
                    nombre = "",
                    Novedades = "",
                    NovedadesCompartidosUnicos = "",
                    PageNumber = filtrosBolsaMedicionDto.PageNumber,
                    tiempoEstimado = ""
                };


                //obtengo todas las coberturas de CAC
                var listCoberturas = _requestCore.requestGetCoberturas(_util.GetAndValidateParameter(parameterList, Enumeration.Parametros.URLAuditCACCoreCACServices), CoberturaModel, tokenInfo.Token);


                //objeto de inicializacion de coberturas
                InputsCoberturaDto CoberturaModelAsociada = new InputsCoberturaDto
                {
                    CantidadVariables = "",
                    definicion = "",
                    ExcluirNovedades = "",
                    idCobertura = "",
                    idCoberturaAdicionales = "",
                    idCoberturaPadre = "",
                    idResolutionSiame = "",
                    legislacion = "",
                    MaxRows = filtrosBolsaMedicionDto.MaxRows,
                    nemonico = "",
                    nombre = "",
                    Novedades = "",
                    NovedadesCompartidosUnicos = "",
                    PageNumber = filtrosBolsaMedicionDto.PageNumber,
                    tiempoEstimado = ""
                };


                //obtengo todas las coberturas de CAC
                var listCoberturasAsociadas = _requestCore.requestGetCoberturas(_util.GetAndValidateParameter(parameterList, Enumeration.Parametros.URLAuditCACCoreCACServices), CoberturaModelAsociada, tokenInfo.Token);


                InputsPeriodoDto periodoModel = new InputsPeriodoDto
                {
                    fechaCorteFin = "",
                    fechaCorteIni = "",
                    fechaFinalReporteFin = "",
                    fechaFinalReporteIni = "",
                    FechaMaxConsultaAuditFin = "",
                    FechaMaxConsultaAuditIni = "",
                    fechaMaximaConciliacionesFin = "",
                    fechaMaximaConciliacionesIni = "",
                    fechaMaximaCorreccionesFin = "",
                    fechaMaximaCorreccionesIni = "",
                    fechaMaximaSoportesFin = "",
                    fechaMaximaSoportesIni = "",
                    FechaMinConsultaAuditFin = "",
                    FechaMinConsultaAuditIni = "",
                    idCobertura = filtrosBolsaMedicionDto.IdCobertura,
                    idPeriodo = "",
                    idPeriodoAnt = "",
                    MaxRows = filtrosBolsaMedicionDto.MaxRows,
                    nombre = "",
                    PageNumber = filtrosBolsaMedicionDto.PageNumber
                };

                //obtengo todos los periodos de CAC
                var listPeriodos = _requestCore.requestGetPeriodos(_util.GetAndValidateParameter(parameterList, Enumeration.Parametros.URLAuditCACCoreCACServices), periodoModel, tokenInfo.Token);

                List<CoberturaModel> coberturasAsignadas = new List<CoberturaModel>();
                List<CoberturaModel> coberturasAsociadas = new List<CoberturaModel>();
                List<PeriodoModel> periodosAsignados = new List<PeriodoModel>();
                List<ResponseMedicionCrudoDto> bolsasDeMedicion = new List<ResponseMedicionCrudoDto>();  //MedicionModel
                List<CantidadRegistrosModel> cantidadRegistros = new List<CantidadRegistrosModel>();

                foreach (var idCoberturaAsiganada in UsuarioXEnfermedad)
                {
                    foreach (var coberturas in listCoberturas)
                    {
                        if (coberturas.idCobertura == idCoberturaAsiganada.IdCobertura)
                        {
                            coberturasAsignadas.Add(coberturas);
                        }
                    }
                    foreach (var coberturas in listCoberturasAsociadas)
                    {
                        if (coberturas.idCobertura == idCoberturaAsiganada.IdCobertura)
                        {
                            coberturasAsociadas.Add(coberturas);
                        }
                    }
                }

                foreach (var coberturaAsignada in coberturasAsignadas)
                {
                    foreach (var periodoAsignado in listPeriodos)
                    {
                        if (coberturaAsignada.idCobertura == periodoAsignado.idCobertura)
                        {
                            periodosAsignados.Add(periodoAsignado);
                        }
                    }

                    foreach (var medicion in listMediciones)
                    {

                        if (filtrosBolsaMedicionDto.IdPeriodo != "")
                        {
                            if ((int.Parse(filtrosBolsaMedicionDto.IdPeriodo) == medicion.IdPeriodo) && (int.Parse(filtrosBolsaMedicionDto.IdCobertura) == medicion.IdCobertura))
                            {
                                bolsasDeMedicion.Add(medicion);

                                string query2 = "EXEC [GetRegistrosXMedicion]" +
                                "@IdMedicion";

                                List<SqlParameter> parametroRegistroId = new List<SqlParameter>
                                        {
                                            new SqlParameter { ParameterName = "@IdMedicion", Value = medicion.Id},
                                        };
                                var countRegistros = await _dBAuditCACContext.CantidadRegistros.FromSqlRaw(query2, parametroRegistroId.ToArray()).ToListAsync();
                                cantidadRegistros.Add(countRegistros[0]);
                            }
                        }
                        else
                        {
                            if (coberturaAsignada.idCobertura == medicion.IdCobertura)
                            {
                                bolsasDeMedicion.Add(medicion);

                                string query2 = "EXEC [GetRegistrosXMedicion]" +
                                "@IdMedicion";

                                List<SqlParameter> parametroRegistroId = new List<SqlParameter>
                                        {
                                            new SqlParameter { ParameterName = "@IdMedicion", Value = medicion.Id},
                                        };
                                var countRegistros = await _dBAuditCACContext.CantidadRegistros.FromSqlRaw(query2, parametroRegistroId.ToArray()).ToListAsync();
                                cantidadRegistros.Add(countRegistros[0]);
                            }
                        }
                    }
                }

                FiltrosBolsaMedicionDto Resp = new FiltrosBolsaMedicionDto
                {
                    coberturasAsignadas = coberturasAsignadas,
                    coberturasAsociadas = coberturasAsociadas,
                    periodosAsignados = periodosAsignados,
                    bolsasDeMedicion = bolsasDeMedicion, //bolsasDeMedicion
                    cantidadRegistros = cantidadRegistros
                };

                #endregion

                var Query = "";

                //Llamamos Procedure que calcula NoRegistrosTotalesFiltrado
                string sqlQuery = "EXEC [dbo].[GetCountPaginator] @Query";

                List<SqlParameter> parmsQuery = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Query", Value = Query},
                };

                var DataQuery = await _dBAuditCACContext.ResponseQueryPaginatorDto.FromSqlRaw<ResponseQueryPaginatorDto>(sqlQuery, parmsQuery.ToArray()).ToListAsync();

                //Obtenemos No Registros maximos a consultar y los agregamos al Header del Request (API).
                var Queryable = _dBAuditCACContext.MedicionModel.AsQueryable();
                var NoRegistrosTotales = Queryable.Count();

                var NoRegistrosTotalesFiltrado = DataQuery.FirstOrDefault().NoRegistrosTotalesFiltrado;

                var TotalPages = NoRegistrosTotalesFiltrado / filtrosBolsaMedicionDto.MaxRows;

                //Data.Select(x => x.QueryNoRegistrosTotales) = "";
                //oReturn.Select(c => { c.QueryNoRegistrosTotales = ""; return c; }).ToList();

                return Tuple.Create(Resp, NoRegistrosTotales, NoRegistrosTotalesFiltrado, TotalPages);

                //return Resp;
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        #region Cargue Poblacion

        /// <summary>
        /// Inicia proceso para cargue de poblacion registrandolo en la tabla current process
        /// </summary>
        /// <param name="filePath">Ruta del archivo a cargar</param>
        /// <param name="input">Modelo Cargue</param>
        /// <returns>Modelo Current Process</returns>
        public async Task<CurrentProcessModel> IniciaProcesoCarguePoblacion(string filePath, InputCarguePoblacion input, S3Model s3)
        {
            try
            {
                _logger.LogInformation("Inicia core cargue poblacion");
                _logger.LogInformation(input.FileBase64);

                //Traemos utilidades.
                Util _util = new Util();

                string fileName = (DateTime.Now.ToString("yyyy:MM:dd:HH:mm:ss").Replace(":", "") + input.FileName);

                //Eliminamos caracteres no deseados
                //input.FileBase64 = input.FileBase64.Replace("data:application/vnd.ms-excel;base64,", "");
                var SeparadorBase64 = _dBAuditCACContext.ParametrosGenerales.Where(x => x.Id == (int)Enumeration.Parametros.SeparadorBase64).Where(x => x.Activo).Select(x => new { x.Valor }).FirstOrDefault().Valor;
                if (SeparadorBase64 == null || SeparadorBase64 == "") { SeparadorBase64 = "data:application/vnd.ms-excel;base64,[data:text/csv;base64,"; } //Valor por defecto si no encuentra parametro.                
                input.FileBase64 = _util.EliminarTextoBase64(input.FileBase64, SeparadorBase64);

                File.WriteAllBytes(filePath + fileName, Convert.FromBase64String(input.FileBase64));
                _logger.LogInformation("Convierte base 64 a archivo");

                var file = File.ReadAllLines(filePath + fileName);

                //Save File S3                
                var result = await _util.LoadFileS3(filePath + fileName, fileName, s3);
                _logger.LogInformation("Guarda archivo s3");

                //Inicializa modelo para proceso cargue de poblacion
                CurrentProcessModel inputCurrentProcess = new CurrentProcessModel()
                {
                    ProcessId = (int)Enumeration.Procesos.CarguePoblacion,
                    Progress = -1,
                    InitDate = DateTime.Now,
                    User = input.Usuario,
                    Result = input.Medicion + ",0," + (file.Count() - 1).ToString() + "," + input.idSubgrupo,//Id medicion, inicio 0, maximo de lineas -1 de la cabecera, idSubgrupo
                    File = true,
                };

                //Registra proceso actual para iniciar el cargue de poblacion
                var resultCurrentProcess = _dBAuditCACContext.CurrentProcessModel.Add(inputCurrentProcess);
                _dBAuditCACContext.SaveChanges();

                //Consulta Id de registro agregado
                inputCurrentProcess.Id = _dBAuditCACContext.CurrentProcessModel.Where(x =>
                    x.ProcessId == inputCurrentProcess.ProcessId && x.User == inputCurrentProcess.User && x.Progress == x.Progress)
                        .OrderByDescending(x => x.Id)
                        .Select(x => x.Id).FirstOrDefault();

                //Inicializa modelo para parametros de proceso cargue de poblacion
                CurrentProcessParamModel inputparam = new CurrentProcessParamModel()
                {
                    CurrentProcessId = inputCurrentProcess.Id,
                    Name = "filePath",
                    Position = 0,
                    Value = filePath + fileName
                };

                //Registra parametros proceso actual
                var resultCurrentProcessParam = _dBAuditCACContext.CurrentProcessParamModel.Add(inputparam);
                _dBAuditCACContext.SaveChanges();


                _logger.LogInformation("Termina registro para proceso de cargue");

                return inputCurrentProcess;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }


        /// <summary>
        /// Genera plantilla para cargue de poblacion
        /// </summary>
        /// <param name="idMedicion">Id medicion</param>
        /// <param name="path">Ruta fisica</param>
        /// <returns>Archivo base 64</returns>
        public async Task<string> GenerarTemplateCarguePoblacion(int idMedicion, string path, int idSubgrupo)
        {
            try
            {
                string script = "EXEC [dbo].[SP_Crea_Plantilla_Cargue_Poblacion] @IdMedicion, @idSubgrupo";

                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@IdMedicion", Value = idMedicion},
                    new SqlParameter { ParameterName = "@idSubgrupo", Value = idSubgrupo},
                };

                var data = await _dBAuditCACContext.LLaveValor.FromSqlRaw<ResponseLlaveValor>(script, parameters.ToArray()).ToListAsync();

                var headerString = "";


                var filename = "plantillacargue_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".csv";

                var pathComplete = path + "\\" + filename;

                data.ToList()?.All(x =>
                {
                    headerString += x.Valor + "\t";
                    return true;
                });

                _util.EscribirArchivoPlano(pathComplete, headerString);

                var plainTextBytes = Convert.ToBase64String(File.ReadAllBytes(pathComplete));

                // Eliminamos archivo creado.
                if (File.Exists(pathComplete))
                {
                    try
                    {
                        File.Delete(pathComplete);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation(ex.ToString());
                    }
                }

                return plainTextBytes;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        //Metodo para llamar procedure SP_Validacion_Estado_BolsasMedicion -  
        //public async Task<IEnumerable<ResponseConsultaEstructurasCargePoblacionDto>> GetConsultaEstructurasCargePoblacion(InputsBolsasMedicionIdDto inputsDto)
        //{
        //    try
        //    {
        //        string sql = "EXEC [dbo].[SP_Consulta_EstructurasCargePoblacion] @IdMedicion";

        //        List<SqlParameter> parms = new List<SqlParameter>
        //        {
        //            new SqlParameter { ParameterName = "@IdMedicion", Value = inputsDto.MedicionId},
        //        };

        //        IEnumerable<ResponseConsultaEstructurasCargePoblacionDto> DataMedicionId =
        //           _dBAuditCACContext.ResponseConsultaEstructurasCargePoblacionDto.FromSqlRaw(sql, parms.ToArray()).ToList();


        //       // var DataMedicionId = await _dBAuditCACContext.ResponseConsultaEstructurasCargePoblacionDto.FromSqlRaw<ResponseConsultaEstructurasCargePoblacionDto>(sql, parms.ToArray()).ToListAsync();

        //        return DataMedicionId;

        //    }
        //    catch (Exception Ex)
        //    {
        //        _logger.LogInformation(Ex.ToString());
        //        throw;
        //    }
        //}

        public async Task<List<ResponseConsultaEstructurasCargePoblacionDto>> GetConsultaEstructurasCargePoblacion(InputsBolsasMedicionIdDto inputsDto)
        {
            try
            {
                string sql = "EXEC [dbo].[SP_Consulta_EstructurasCargePoblacion] @IdMedicion";

                List<SqlParameter> parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@IdMedicion", Value = inputsDto.MedicionId},
                };

                //IEnumerable<ResponseConsultaEstructurasCargePoblacionDto> DataMedicionId =
                //   _dBAuditCACContext.ResponseConsultaEstructurasCargePoblacionDto.FromSqlRaw(sql, parms.ToArray()).ToList();


                var DataMedicionId = await _dBAuditCACContext.ResponseConsultaEstructurasCargePoblacionDto.FromSqlRaw<ResponseConsultaEstructurasCargePoblacionDto>(sql, parms.ToArray()).ToListAsync();

                return DataMedicionId;

            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }
        #endregion


        // -- --
        //Metodo para llamar procedure SP_Validacion_Estado_BolsasMedicion -  
        public async Task<List<ResponseValidacionEstadoBolsasMedicionDto>> GetValidacionEstadoBolsasMedicion(InputsBolsasMedicionIdDto inputsDto)
        {
            try
            {
                string sqlQueryMedicionId = "EXEC [dbo].[SP_Validacion_Estado_BolsasMedicion] @MedicionId";

                List<SqlParameter> parmsQueryMedicionId = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@MedicionId", Value = inputsDto.MedicionId},
                };

                var DataMedicionId = await _dBAuditCACContext.ResponseValidacionEstadoBolsasMedicionDto.FromSqlRaw<ResponseValidacionEstadoBolsasMedicionDto>(sqlQueryMedicionId, parmsQueryMedicionId.ToArray()).ToListAsync();

                return DataMedicionId;

            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }
        // --
        //public async Task<List<ResponseValidacionEstadoBolsasMedicionDto>> GetValidacionEstadoBolsasMedicion(InputsBolsasMedicionIdDto inputsDto)
        //{
        //    try
        //    {
        //        string sql = "EXEC [dbo].[SP_Validacion_Estado_BolsasMedicion] @MedicionId";

        //        List<SqlParameter> parms = new List<SqlParameter>
        //        {
        //            new SqlParameter { ParameterName = "@MedicionId", Value = inputsDto.MedicionId},

        //        };

        //        var Data = await _dBAuditCACContext.ResponseValidacionEstadoBolsasMedicionDto.FromSqlRaw<ResponseValidacionEstadoBolsasMedicionDto>(sql, parms.ToArray()).ToListAsync();
        //        return Data;
        //    }
        //    catch (Exception Ex)
        //    {
        //        _logger.LogInformation(Ex.ToString());
        //        throw;
        //    }
        //}
        // -- --

        //Metodo para llamar procedure SP_Usuarios_BolsaMedicion_Filtro - Para la consulta de usuarios (filtros) de una bolsa.
        public async Task<List<ResponseUsuariosBolsaMedicionFiltroDto>> GetUsuariosBolsaMedicionFiltro(InputsBolsasMedicionIdDto inputsDto)
        {
            try
            {
                string sql = "EXEC [dbo].[SP_Usuarios_BolsaMedicion_Filtro] @MedicionId";

                List<SqlParameter> parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@MedicionId", Value = inputsDto.MedicionId},

                };

                var Data = await _dBAuditCACContext.ResponseUsuariosBolsaMedicionFiltroDto.FromSqlRaw<ResponseUsuariosBolsaMedicionFiltroDto>(sql, parms.ToArray()).ToListAsync();
                return Data;
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        //Metodo para llamar procedure SP_Usuarios_BolsaMedicion - Para la consulta de los registros asignados a un auditor y a un estado      
        public async Task<Tuple<List<ResponseUsuariosBolsaMedicionDto>, int, int, int>> GetUsuariosBolsaMedicion(InputsBolsasMedicionDto inputsDto)
        {
            try
            {
                string sql = "EXEC [dbo].[SP_Usuarios_BolsaMedicion] @PageNumber, @MaxRows, @MedicionId, @AuditorId, @KeyWord";

                List<SqlParameter> parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@PageNumber", Value = inputsDto.PageNumber},
                    new SqlParameter { ParameterName = "@MaxRows", Value = inputsDto.MaxRows},
                    //
                    new SqlParameter { ParameterName = "@MedicionId", Value = inputsDto.MedicionId},
                    new SqlParameter { ParameterName = "@AuditorId", Value = String.Join(", ", inputsDto.AuditorId.ToArray())},
                    new SqlParameter { ParameterName = "@KeyWord", Value = inputsDto.KeyWord},

                };

                var Data = await _dBAuditCACContext.ResponseUsuariosBolsaMedicionDto.FromSqlRaw<ResponseUsuariosBolsaMedicionDto>(sql, parms.ToArray()).ToListAsync();

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

                var DataQuery = await _dBAuditCACContext.ResponseQueryPaginatorDto.FromSqlRaw<ResponseQueryPaginatorDto>(sqlQuery, parmsQuery.ToArray()).ToListAsync();

                //Obtenemos No Registros maximos a consultar y los agregamos al Header del Request (API).
                var Queryable = _dBAuditCACContext.MedicionModel.AsQueryable();
                var NoRegistrosTotales = Queryable.Count();

                var NoRegistrosTotalesFiltrado = DataQuery.FirstOrDefault().NoRegistrosTotalesFiltrado;

                var TotalPages = NoRegistrosTotalesFiltrado / inputsDto.MaxRows;

                Data.Select(c => { c.QueryNoRegistrosTotales = ""; return c; }).ToList();

                return Tuple.Create(Data, NoRegistrosTotales, NoRegistrosTotalesFiltrado, TotalPages);
                //return Data;
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Carga los filtros usados en vista de Reasignaciones de bolsa
        /// </summary>
        /// <param name="inputsDto">Modelo Input de datos</param>
        /// <returns>Filtros segun Auditor y Medicion</returns>
        public async Task<List<ResponseRegistrosAuditoriaXBolsaMedicionFiltros>> GetRegistrosAuditoriaXBolsaMedicionFiltros(InputsRegistrosAuditoriaXBolsaMedicionFiltrosDto inputsDto)
        {
            try
            {
                string sql = "EXEC [dbo].[SP_RegistrosAuditoria_XBolsaMedicion_Filtros] @IdAuditor, @MedicionId";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters   
                    new SqlParameter { ParameterName = "@IdAuditor", Value = inputsDto.IdAuditor},
                    new SqlParameter { ParameterName = "@MedicionId", Value = inputsDto.MedicionId}
                };

                var Data = await _dBAuditCACContext.ResponseRegistrosAuditoriaXBolsaMedicionFiltrosDto.FromSqlRaw<ResponseRegistrosAuditoriaXBolsaMedicionFiltrosDto>(sql, parms.ToArray()).ToListAsync();

                var Result = new List<ResponseRegistrosAuditoriaXBolsaMedicionFiltros>();

                var filtros = Data
                    .GroupBy(x => new { x.NombreFiltro })
                    .Select(y => new ResponseRegistrosAuditoriaXBolsaMedicionFiltros()
                    {
                        NombreFiltro = y.Key.NombreFiltro
                    }
                    );

                filtros?.All(x =>
                {
                    var row = new ResponseRegistrosAuditoriaXBolsaMedicionFiltros();
                    row.NombreFiltro = x.NombreFiltro;
                    row.Detalle = new List<DetallesRegistrosAuditoriaXBolsaMedicion>();

                    Data.Where(d => d.NombreFiltro == x.NombreFiltro)?.All(z =>
                    {
                        row.Detalle.Add(new DetallesRegistrosAuditoriaXBolsaMedicion()
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
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Carga listado de Registros Auditoria en vista de Reasignaciones de bolsa.
        /// </summary>
        /// <param name="inputsDto">Filtros para la consulta</param>
        /// <returns>Listado de RegistrosAuditoria segun filtros recibidos</returns>
        public async Task<Tuple<List<ResponseRegistrosAuditoriaXBolsaMedicionDto>, int, int, int>> GetRegistrosAuditoriaXBolsaMedicion(InputsGetRegistrosAuditoriaXBolsaMedicionDto inputsDto)
        {
            try
            {
                string sql = "EXEC [dbo].[SP_RegistrosAuditoria_XBolsaMedicion] @PageNumber, @MaxRows, @IdRadicado, @AuditorId, @MedicionId, @FechaAsignacionIni, @FechaAsignacionFin, @Estado, @CodigoEps, @Finalizados, @KeyWord";

                List<SqlParameter> parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@PageNumber", Value = inputsDto.PageNumber},
                    new SqlParameter { ParameterName = "@MaxRows", Value = inputsDto.MaxRows},
                    //
                    new SqlParameter { ParameterName = "@IdRadicado", Value = String.Join(", ", inputsDto.IdRadicado.ToArray())},
                    new SqlParameter { ParameterName = "@AuditorId", Value = inputsDto.AuditorId},
                    new SqlParameter { ParameterName = "@MedicionId", Value = String.Join(", ", inputsDto.MedicionId.ToArray())},
                    new SqlParameter { ParameterName = "@FechaAsignacionIni", Value = inputsDto.FechaAsignacionIni},
                    new SqlParameter { ParameterName = "@FechaAsignacionFin", Value = inputsDto.FechaAsignacionFin},
                    new SqlParameter { ParameterName = "@Estado", Value = String.Join(", ", inputsDto.Estado.ToArray())},
                    new SqlParameter { ParameterName = "@CodigoEps", Value = String.Join(", ", inputsDto.CodigoEps.ToArray())},
                    new SqlParameter { ParameterName = "@Finalizados", Value = inputsDto.Finalizados},
                    new SqlParameter { ParameterName = "@KeyWord", Value = inputsDto.KeyWord},
                };

                var Data = await _dBAuditCACContext.ResponseRegistrosAuditoriaXBolsaMedicionDto.FromSqlRaw<ResponseRegistrosAuditoriaXBolsaMedicionDto>(sql, parms.ToArray()).ToListAsync();

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

                var DataQuery = await _dBAuditCACContext.ResponseQueryPaginatorDto.FromSqlRaw<ResponseQueryPaginatorDto>(sqlQuery, parmsQuery.ToArray()).ToListAsync();

                //Obtenemos No Registros maximos a consultar y los agregamos al Header del Request (API).
                var Queryable = _dBAuditCACContext.MedicionModel.AsQueryable();
                var NoRegistrosTotales = Queryable.Count();

                var NoRegistrosTotalesFiltrado = DataQuery.FirstOrDefault().NoRegistrosTotalesFiltrado;

                var TotalPages = NoRegistrosTotalesFiltrado / inputsDto.MaxRows;

                Data.Select(c => { c.QueryNoRegistrosTotales = ""; return c; }).ToList();

                return Tuple.Create(Data, NoRegistrosTotales, NoRegistrosTotalesFiltrado, TotalPages);
                //return Data;
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Reasigna RegistrosAuditoria a otros Auditories. Esto es realizado de forma automatica (Equitativa).
        /// </summary>
        /// <param name="inputsDto">Modelo de datos</param>
        /// <returns></returns>
        public async Task<bool> ReasignacionesBolsaEquitativa(InputReasignacionesBolsaEquitativaDto inputsDto)
        {
            try
            {
                _logger.LogInformation("Inicia Proceso de Reasignacion de Bolsa Equitativo");

                //Declarar variables y obtener datos.
                int CountAuditores = 1;
                int CountRadicados = 1;
                var NoAuditores = inputsDto.AuditoresId.Count;
                var NoRadicados = inputsDto.IdRadicado.Count;
                var NoRegistrosXAuditor = NoRadicados / NoAuditores; //Math.Ceiling(NoRegistrosXAuditor). Calculamos No de registros por auditor.                

                string sql = "EXEC [dbo].[SP_Reasignaciones_Bolsa] @Reasignaciones_Bolsa, @IdUsuario";
                //var Parameter = new SqlParameter("@Reasignaciones_Bolsa", SqlDbType.Structured);

                List<SqlParameter> Parameter = new List<SqlParameter>
                    {
                        new SqlParameter { ParameterName = "@Reasignaciones_Bolsa", Value = SqlDbType.Structured},
                        new SqlParameter { ParameterName = "@IdUsuario", Value = inputsDto.IdUsuario == null ? "": inputsDto.IdUsuario },
                    };

                #region Datos de la tabla
                DataTable ListData = new DataTable();
                ListData.Columns.Add("AuditorId", typeof(string));
                ListData.Columns.Add("IdRadicado", typeof(int));
                ListData.Columns.Add("FechaAsignacion", typeof(DateTime));
                #endregion

                //Recorremos Auditores recibidos (AuditoriesId).
                foreach (var itemAuditor in inputsDto.AuditoresId)
                {
                    //Validamos
                    while (CountAuditores <= NoRegistrosXAuditor)
                    {
                        if (CountRadicados <= NoRadicados)
                        {
                            //Asignamos registros
                            ListData.Rows.Add(itemAuditor, inputsDto.IdRadicado[CountRadicados - 1], DateTime.Now);
                        }

                        //Sumamos +1 No de registro acutual.
                        CountAuditores++;
                        CountRadicados++;
                    }

                    //Reiniciamos Count
                    CountAuditores = 1;
                }

                Parameter[0].Value = ListData;
                Parameter[0].TypeName = "dbo.DT_Reasignaciones_Bolsa";
                var insertData = await _dBAuditCACContext.Database.ExecuteSqlRawAsync(sql, Parameter);

                return true;
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Para cargar listado de Registros Auditoria en vista de Reasignaciones de bolsa detallada.
        /// </summary>
        /// <param name="filePath">Ruta del archivo a cargar</param>
        /// <param name="input">Modelo Cargue</param>
        /// <returns>Modelo Current Process</returns>        
        public async Task<ResponseReasignacionesBolsaDetalladaDto> ReasignacionesBolsaDetallada(string filePath, InputReasignacionesBolsaDetalladaDto input, S3Model _s3) //InputReasignacionesBolsaDetalladaDto inputsDto
        {
            try
            {
                _logger.LogInformation("Inicia Proceso de Reasignacion de Bolsa Detallado");

                //Declaramos variables usadas,
                string currentline = "";
                List<string> resultDetail = new List<string>(); //Listado usado para capturar resultados del proceso.
                var _pathLog = configuration["PathList:LogBash"];
                Util _util = new Util();

                //Obtenemos info del arhivo y lo pasamos a Base64, para almacenarlo.
                string fileName = (DateTime.Now.ToString("yyyy:MM:dd:HH:mm:ss").Replace(":", "") + input.FileName);

                //Eliminamos caracteres no deseados
                var SeparadorBase64 = _dBAuditCACContext.ParametrosGenerales.Where(x => x.Id == 15).Where(x => x.Activo == true).Select(x => new { x.Valor }).FirstOrDefault().Valor;
                if (SeparadorBase64 == null || SeparadorBase64 == "") { SeparadorBase64 = "data:application/vnd.ms-excel;base64,[data:text/csv;base64,"; } //Valor por defecto si no encuentra parametro.                
                input.FileBase64 = _util.EliminarTextoBase64(input.FileBase64, SeparadorBase64);

                File.WriteAllBytes(filePath + fileName, Convert.FromBase64String(input.FileBase64));
                _logger.LogInformation("Convierte base 64 a archivo");

                //Output file                
                string filenameoutput = "resultado_" + (fileName) + ".csv";
                var outputPath = filePath + filenameoutput;

                //Obtenemos todas las lineas del archivo CSV Cargado.
                var lines = File.ReadLines(filePath + fileName).ToList();

                //Obtenemos Header del archivo cargado, es decir.. los nombres de columnas a insertar.
                var headerFile = lines[0];

                //Recorremos todas las lineas del archivo cargado.
                for (int i = 1; i < lines.Count(); i++)
                {
                    //Declaramos listado para capturar resultados del proceso.
                    var dataResult = new List<ResponseCarguePoblacionDto>();

                    //Obtenemos la linea actual del archivo cargado.
                    currentline = lines[i];

                    //Llamamos SP.
                    string sql = "EXEC [dbo].[SP_Reasignaciones_Bolsa_Detallada] @header, @line, @IdMedicion, @IdUsuario";

                    List<SqlParameter> parms = new List<SqlParameter>
                    {
                        new SqlParameter { ParameterName = "@header", Value = headerFile},
                        new SqlParameter { ParameterName = "@line", Value = currentline},
                        new SqlParameter { ParameterName = "@IdMedicion", Value = String.Join(", ", input.IdMedicion.ToArray())},
                        new SqlParameter { ParameterName = "@IdUsuario", Value = input.User == null ? "": input.User},
                    };

                    //Ejecutamos SP y capturamos resultados del proceso.
                    dataResult = await _dBAuditCACContext.ResponseCarguePoblacion.FromSqlRaw<ResponseCarguePoblacionDto>(sql, parms.ToArray()).ToListAsync();
                    _dBAuditCACContext.SaveChanges();

                    //Almacenamos resultados del proceso
                    _util.EscribirArchivoPlano(outputPath, dataResult.FirstOrDefault().Result);
                    resultDetail.Add(dataResult.FirstOrDefault().Result);
                }

                //Validamos si ha ocurrido un error en el proceso.
                bool status = true;
                if (resultDetail.Where(x => x.Contains("ERROR")).Any())
                {
                    status = false;
                }

                //Save File output S3                
                var results3 = await _util.LoadFileS3(outputPath, filenameoutput, _s3);

                //Registra carga
                ControlArchivos_CarguePoblacionModel controlCarga = new ControlArchivos_CarguePoblacionModel()
                {
                    CurrentProcessId = 1, //Numero ramdon
                    IdMedicion = input.IdMedicion[0],
                    ArchivoCargado = _s3.UrlFileS3Uploaded + fileName,
                    ArchivoDescargado = _s3.UrlFileS3Uploaded + filenameoutput,
                    Enable = true,
                    FechaCreacion = DateTime.Now,
                    FecchaActualizacion = DateTime.Now,
                    Usuario = input.User
                };

                _dBAuditCACContext.ControlArchivos_CarguePoblacion.Add(controlCarga);
                _dBAuditCACContext.SaveChanges();

                //Delete input file
                if (File.Exists(filePath + fileName))
                {
                    try
                    {
                        File.Delete(filePath + fileName);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation(_pathLog, "ERROR: " + ex.ToString() + " " + DateTime.Now.ToString());
                    }
                }
                _logger.LogInformation(_pathLog, "Elimina archivo origen  " + DateTime.Now.ToString());

                //Delete input file
                if (File.Exists(outputPath))
                {
                    try
                    {
                        File.Delete(outputPath);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation(_pathLog, "ERROR: " + ex.ToString() + " " + DateTime.Now.ToString());
                    }
                }
                _logger.LogInformation(_pathLog, "Elimina archivo origen  " + DateTime.Now.ToString());

                //Retornamos valores.
                ResponseReasignacionesBolsaDetalladaDto respuesta = new ResponseReasignacionesBolsaDetalladaDto()
                {
                    status = status,
                    //file = _util.EliminarAcentos(controlCarga.ArchivoDescargado)
                    file = controlCarga.ArchivoDescargado
                };

                return respuesta;
                //return "OK";

            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Para cargar listado de Bolsas de una enfermedad madre.
        /// </summary>
        /// <param name="inputsDto">Modelo de datos</param>
        /// <returns>Listado de Bolsas segun enfermedad madre</returns>    
        public async Task<List<ResponseBolsasMedicionXEnfermedadMadreDto>> GetBolsasMedicionXEnfermedadMadre(InputsBolsasMedicionXEnfermedadMadreDto inputsDto)
        {
            try
            {
                string sql = "EXEC [dbo].[SP_BolsasMedicion_XEnfermedadMadre] @IdEnfermedadMadre, @IdMedicion";

                List<SqlParameter> parms = new List<SqlParameter>
                    {
                        new SqlParameter { ParameterName = "@IdEnfermedadMadre", Value = inputsDto.IdEnfermedadMadre},
                        new SqlParameter { ParameterName = "@IdMedicion", Value = inputsDto.IdMedicion},

                    };

                var Data = await _dBAuditCACContext.ResponseBolsasMedicionXEnfermedadMadreDto.FromSqlRaw<ResponseBolsasMedicionXEnfermedadMadreDto>(sql, parms.ToArray()).ToListAsync();
                return Data;
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }


        /// <summary>
        /// Para la consulta de usuarios segun su Rol.
        /// </summary>
        /// <param name="inputsDto">Modelo Cargue</param>
        /// <returns>Modelo Current Process</returns>
        public async Task<List<ResponseUsuariosBolsaMedicionFiltroDto>> GetUsuariosByRol(InputsGetUsuariosByRoleIdDto inputsDto)
        {
            try
            {
                string sql = "EXEC [dbo].[SP_Usuarios_By_Rol] @RolId";

                List<SqlParameter> parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@RolId", Value = inputsDto.RoleId},

                };

                var Data = await _dBAuditCACContext.ResponseUsuariosBolsaMedicionFiltroDto.FromSqlRaw<ResponseUsuariosBolsaMedicionFiltroDto>(sql, parms.ToArray()).ToListAsync();
                return Data;
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Para Mover todos los registrosAuditorias de una Bolsa a otra.
        /// </summary>
        /// <param name="filePath">Ruta del archivo a cargar</param>
        /// <param name="input">Modelo Cargue</param>
        /// <returns>Modelo Current Process</returns>        
        public async Task<ResponseReasignacionesBolsaDetalladaDto> MoverTodosRegistrosAuditoriaBolsaMedicion(string filePath, InputMoverTodosRegistrosAuditoriaBolsaMedicionDto input)
        {
            try
            {
                //_logger.LogInformation("Inicia Proceso de Reasignacion de Bolsa Detallado");

                //Declaramos variables usadas para generar archivo
                var dataResult = new List<ResponseMoverTodosRegistrosAuditoriaBolsaMedicionDto>();
                List<string> resultDetail = new List<string>(); //Listado usado para capturar resultados del proceso.                
                var plainTextBytes = "";

                //Llamamos SP.
                string sql = "EXEC [dbo].[SP_MoverTodos_RegistrosAuditoria_BolsaMedicion] @MedicionIdOriginal, @MedicionIdDestino, @IdUsuario";

                List<SqlParameter> parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@MedicionIdOriginal", Value = input.MedicionIdOriginal},
                    new SqlParameter { ParameterName = "@MedicionIdDestino", Value = input.MedicionIdDestino},
                    new SqlParameter { ParameterName = "@IdUsuario", Value = input.IdUsuario == null ? "": input.IdUsuario},
                };

                //Ejecutamos SP y capturamos resultados del proceso.
                dataResult = await _dBAuditCACContext.ResponseMoverTodosRegistrosAuditoriaBolsaMedicionDto.FromSqlRaw<ResponseMoverTodosRegistrosAuditoriaBolsaMedicionDto>(sql, parms.ToArray()).ToListAsync();
                _dBAuditCACContext.SaveChanges();

                // resultDetail.Add(dataResult.FirstOrDefault().EstadoEjecucion);

                //Validamos si ha ocurrido un error en el proceso.
                bool status = true;
                if (dataResult != null)
                {
                    //if (resultDetail.Where(x => x.Contains("ERROR")).Any())
                    foreach (var item in dataResult)
                    {
                        if (item.EstadoEjecucion == "ERROR")
                        {
                            status = false;
                        }
                    }
                }

                //(List<ResponseMoverTodosRegistrosAuditoriaBolsaMedicionDto> dataResult, string ruta, bool camposError = false)
                plainTextBytes = await _generate.ReporteMoverRegistrosAuditoriaBolsaMedicionData(dataResult, filePath, true);

                //Retornamos valores.
                ResponseReasignacionesBolsaDetalladaDto respuesta = new ResponseReasignacionesBolsaDetalladaDto()
                {
                    status = status,
                    file = plainTextBytes
                };

                return respuesta;
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Para consultar info usada en Mover algunos registrosAuditorias de una Bolsa a otra.
        /// </summary>
        /// <param name="filePath">Ruta del archivo a cargar</param>
        /// <param name="input">Modelo Cargue</param>
        /// <returns>Modelo Current Process</returns>        
        public async Task<ResponseReasignacionesBolsaDetalladaDto> MoverAlgunosRegistrosAuditoriaBolsaMedicionPlantilla(string filePath, InputMoverTodosRegistrosAuditoriaBolsaMedicionDto input)
        {
            try
            {
                //_logger.LogInformation("Inicia Proceso de Reasignacion de Bolsa Detallado");

                //Declaramos variables usadas para generar archivo
                var dataResult = new List<ResponseMoverTodosRegistrosAuditoriaBolsaMedicionDto>();
                List<string> resultDetail = new List<string>(); //Listado usado para capturar resultados del proceso.                
                var plainTextBytes = "";

                //Llamamos SP.
                string sql = "EXEC [dbo].[SP_MoverTodos_RegistrosAuditoria_BolsaMedicion_Plantilla] @MedicionIdOriginal";

                List<SqlParameter> parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@MedicionIdOriginal", Value = input.MedicionIdOriginal},
                };

                //Ejecutamos SP y capturamos resultados del proceso.
                dataResult = await _dBAuditCACContext.ResponseMoverTodosRegistrosAuditoriaBolsaMedicionDto.FromSqlRaw<ResponseMoverTodosRegistrosAuditoriaBolsaMedicionDto>(sql, parms.ToArray()).ToListAsync();
                _dBAuditCACContext.SaveChanges();

                //Validamos si ha ocurrido un error en el proceso.
                bool status = true;
                if (dataResult != null)
                {
                    //if (resultDetail.Where(x => x.Contains("ERROR")).Any())
                    foreach (var item in dataResult)
                    {
                        if (item.EstadoEjecucion == "ERROR")
                        {
                            status = false;
                        }
                    }
                }

                plainTextBytes = await _generate.ReporteMoverRegistrosAuditoriaBolsaMedicionData(dataResult, filePath);

                //Retornamos valores.
                ResponseReasignacionesBolsaDetalladaDto respuesta = new ResponseReasignacionesBolsaDetalladaDto()
                {
                    status = status,
                    file = plainTextBytes
                };

                return respuesta;
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Para Alugnos registrosAuditorias de una Bolsa a otra. Usando un archivo plantilla.
        /// </summary>
        /// <param name="filePath">Ruta del archivo a cargar</param>
        /// <param name="input">Modelo Cargue</param>
        /// <returns>Modelo Current Process</returns>        
        public async Task<ResponseReasignacionesBolsaDetalladaDto> MoverAlgunosRegistrosAuditoriaBolsaMedicion(string filePath, InputMoverAlgunosRegistrosAuditoriaBolsaMedicionDto input, S3Model _s3)
        {
            try
            {
                _logger.LogInformation("Inicia Proceso de Mover registros entre bolsas");

                //Declaramos variables usadas,
                string currentline = "";
                List<string> resultDetail = new List<string>(); //Listado usado para capturar resultados del proceso.
                var _pathLog = configuration["PathList:LogBash"];
                Util _util = new Util();

                //Obtenemos info del arhivo y lo pasamos a Base64, para almacenarlo.
                string fileName = (DateTime.Now.ToString("yyyy:MM:dd:HH:mm:ss").Replace(":", "") + input.FileName);

                //Eliminamos caracteres no deseados
                var SeparadorBase64 = _dBAuditCACContext.ParametrosGenerales.Where(x => x.Id == 15).Where(x => x.Activo == true).Select(x => new { x.Valor }).FirstOrDefault().Valor;
                if (SeparadorBase64 == null || SeparadorBase64 == "") { SeparadorBase64 = "data:application/vnd.ms-excel;base64,[data:text/csv;base64,"; } //Valor por defecto si no encuentra parametro.                
                input.FileBase64 = _util.EliminarTextoBase64(input.FileBase64, SeparadorBase64);

                File.WriteAllBytes(filePath + fileName, Convert.FromBase64String(input.FileBase64));
                _logger.LogInformation("Convierte base 64 a archivo");

                //Output file                
                string filenameoutput = "resultado_" + (fileName) + ".csv";
                var outputPath = filePath + filenameoutput;

                //Obtenemos todas las lineas del archivo CSV Cargado.
                var lines = File.ReadLines(filePath + fileName).ToList();

                //Obtenemos Header del archivo cargado, es decir.. los nombres de columnas a insertar.
                var headerFile = lines[0];

                //Recorremos todas las lineas del archivo cargado.
                for (int i = 1; i < lines.Count(); i++)
                {
                    //Declaramos listado para capturar resultados del proceso.
                    var dataResult = new List<ResponseCarguePoblacionDto>();

                    //Obtenemos la linea actual del archivo cargado.
                    currentline = lines[i];

                    if (currentline != "" && currentline.Length > 0)
                    {
                        //Llamamos SP.
                        string sql = "EXEC [dbo].[SP_MoverAlgunos_RegistrosAuditoria_BolsaMedicion] @header, @line, @IdUsuario";

                        List<SqlParameter> parms = new List<SqlParameter>
                        {
                            new SqlParameter { ParameterName = "@header", Value = headerFile},
                            new SqlParameter { ParameterName = "@line", Value = currentline},
                            new SqlParameter { ParameterName = "@IdUsuario", Value = input.IdUsuario == null ? "": input.IdUsuario},
                        };

                        //Ejecutamos SP y capturamos resultados del proceso.
                        dataResult = await _dBAuditCACContext.ResponseCarguePoblacion.FromSqlRaw<ResponseCarguePoblacionDto>(sql, parms.ToArray()).ToListAsync();
                        _dBAuditCACContext.SaveChanges();

                        //Almacenamos resultados del proceso
                        _util.EscribirArchivoPlano(outputPath, dataResult.FirstOrDefault().Result);
                        resultDetail.Add(dataResult.FirstOrDefault().Result);
                    }

                }

                //Validamos si ha ocurrido un error en el proceso.
                bool status = true;
                if (resultDetail.Where(x => x.Contains("ERROR")).Any())
                {
                    status = false;
                }

                ////Validamos si ha ocurrido un error en el proceso.
                //bool status = true;
                //if (dataResult != null)
                //{
                //    //if (resultDetail.Where(x => x.Contains("ERROR")).Any())
                //    foreach (var item in dataResult)
                //    {
                //        if (item.EstadoEjecucion == "ERROR")
                //        {
                //            status = false;
                //        }
                //    }
                //}

                //Save File output S3                
                var results3 = await _util.LoadFileS3(outputPath, filenameoutput, _s3);

                //Registra carga
                ControlArchivos_CarguePoblacionModel controlCarga = new ControlArchivos_CarguePoblacionModel()
                {
                    CurrentProcessId = 1, //Numero ramdon
                    IdMedicion = 1,
                    ArchivoCargado = _s3.UrlFileS3Uploaded + fileName,
                    ArchivoDescargado = _s3.UrlFileS3Uploaded + filenameoutput,
                    Enable = true,
                    FechaCreacion = DateTime.Now,
                    FecchaActualizacion = DateTime.Now,
                    Usuario = ""
                };

                _dBAuditCACContext.ControlArchivos_CarguePoblacion.Add(controlCarga);
                _dBAuditCACContext.SaveChanges();

                //Delete input file
                if (File.Exists(filePath + fileName))
                {
                    try
                    {
                        File.Delete(filePath + fileName);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation(_pathLog, "ERROR: " + ex.ToString() + " " + DateTime.Now.ToString());
                    }
                }
                _logger.LogInformation(_pathLog, "Elimina archivo origen  " + DateTime.Now.ToString());

                //Delete input file
                if (File.Exists(outputPath))
                {
                    try
                    {
                        File.Delete(outputPath);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation(_pathLog, "ERROR: " + ex.ToString() + " " + DateTime.Now.ToString());
                    }
                }
                _logger.LogInformation(_pathLog, "Elimina archivo origen  " + DateTime.Now.ToString());

                //Retornamos valores.
                ResponseReasignacionesBolsaDetalladaDto respuesta = new ResponseReasignacionesBolsaDetalladaDto()
                {
                    status = status,
                    file = controlCarga.ArchivoDescargado
                };

                return respuesta;
                //return "OK";

            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        #region Perfil Admin: Eliminar registros de una medición o bolsa durante el proceso de auditoría
        /// <summary>
        /// Para consultar plantilla de eliminacion de registros, es decir poblacion de una medicion (RegistrosAuditoria).
        /// </summary>
        /// <param name="filePath">Ruta del archivo a cargar</param>
        /// <param name="input">Modelo Cargue</param>
        /// <returns>Modelo Current Process</returns>        
        public async Task<ResponseReasignacionesBolsaDetalladaDto> EliminarRegistrosAuditoriaPlantilla(string filePath) //, InputEliminarRegistrosAuditoriaPlantillaDto input
        {
            try
            {
                //Declaramos variables usadas para generar archivo -- ResponseEliminarRegistrosAuditoriaPlantillaDto
                var dataResult = new List<ResponseEliminarRegistrosAuditoriaPlantillaDto>();
                List<string> resultDetail = new List<string>(); //Listado usado para capturar resultados del proceso.                
                var plainTextBytes = "";

                //Llamamos SP.
                string sql = "EXEC [dbo].[SP_Eliminar_RegistrosAuditoria_Plantilla]"; // @MedicionId

                List<SqlParameter> parms = new List<SqlParameter>
                {
                    //new SqlParameter { ParameterName = "@MedicionId", Value = input.MedicionId},
                };

                //Ejecutamos SP y capturamos resultados del proceso.
                dataResult = await _dBAuditCACContext.ResponseEliminarRegistrosAuditoriaPlantillaDto.FromSqlRaw<ResponseEliminarRegistrosAuditoriaPlantillaDto>(sql, parms.ToArray()).ToListAsync();
                _dBAuditCACContext.SaveChanges();

                //Validamos si ha ocurrido un error en el proceso.
                bool status = true;
                //if (dataResult != null)
                //{
                //    //if (resultDetail.Where(x => x.Contains("ERROR")).Any())
                //    foreach (var item in dataResult)
                //    {
                //        if (item.EstadoEjecucion == "ERROR")
                //        {
                //            status = false;
                //        }
                //    }
                //}

                plainTextBytes = await _generate.EliminarRegistrosAuditoriaPlantillaData(dataResult, filePath);

                //Retornamos valores.
                ResponseReasignacionesBolsaDetalladaDto respuesta = new ResponseReasignacionesBolsaDetalladaDto()
                {
                    status = status,
                    file = plainTextBytes
                };

                return respuesta;
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Para Eliminar registrosAuditorias de una Bolsa.
        /// </summary>
        /// <param name="filePath">Ruta del archivo a cargar</param>
        /// <param name="input">Modelo Cargue</param>
        /// <returns>Modelo Current Process</returns>        
        public async Task<ResponseReasignacionesBolsaDetalladaDto> EliminarRegistrosAuditoria(string filePath, InputEliminarRegistrosAuditoriaDto input, S3Model _s3)
        {
            try
            {
                _logger.LogInformation("Inicia Proceso de eliminar registros de bolsas");

                //Declaramos variables usadas,
                string currentline = "";
                List<string> resultDetail = new List<string>(); //Listado usado para capturar resultados del proceso.
                var _pathLog = configuration["PathList:LogBash"];
                Util _util = new Util();

                //Obtenemos info del arhivo y lo pasamos a Base64, para almacenarlo.
                string fileName = (DateTime.Now.ToString("yyyy:MM:dd:HH:mm:ss").Replace(":", "") + input.FileName);

                //Eliminamos caracteres no deseados
                var SeparadorBase64 = _dBAuditCACContext.ParametrosGenerales.Where(x => x.Id == 15).Where(x => x.Activo == true).Select(x => new { x.Valor }).FirstOrDefault().Valor;
                if (SeparadorBase64 == null || SeparadorBase64 == "") { SeparadorBase64 = "data:application/vnd.ms-excel;base64,[data:text/csv;base64,"; } //Valor por defecto si no encuentra parametro.                
                input.FileBase64 = _util.EliminarTextoBase64(input.FileBase64, SeparadorBase64);

                File.WriteAllBytes(filePath + fileName, Convert.FromBase64String(input.FileBase64));
                _logger.LogInformation("Convierte base 64 a archivo");

                //Output file                
                string filenameoutput = "resultado_" + (fileName) + ".csv";
                var outputPath = filePath + filenameoutput;

                //Obtenemos todas las lineas del archivo CSV Cargado.
                var lines = File.ReadLines(filePath + fileName).ToList();

                //Obtenemos Header del archivo cargado, es decir.. los nombres de columnas a insertar.
                var headerFile = lines[0];

                //Recorremos todas las lineas del archivo cargado.
                for (int i = 1; i < lines.Count(); i++)
                {
                    //Declaramos listado para capturar resultados del proceso.
                    var dataResult = new List<ResponseCarguePoblacionDto>();

                    //Obtenemos la linea actual del archivo cargado.
                    currentline = lines[i];

                    if (currentline != "" && currentline.Length > 0)
                    {
                        //Llamamos SP.
                        string sql = "EXEC [dbo].[SP_Eliminar_RegistrosAuditoria] @header, @line, @MedicionId, @Observacion, @IdUsuario";

                        List<SqlParameter> parms = new List<SqlParameter>
                        {
                            new SqlParameter { ParameterName = "@header", Value = headerFile},
                            new SqlParameter { ParameterName = "@line", Value = currentline},
                            new SqlParameter { ParameterName = "@MedicionId", Value = input.MedicionId},
                            new SqlParameter { ParameterName = "@Observacion", Value = input.Observacion},
                            new SqlParameter { ParameterName = "@IdUsuario", Value = input.IdUsuario == null ? "": input.IdUsuario},
                        };

                        //Ejecutamos SP y capturamos resultados del proceso.
                        dataResult = await _dBAuditCACContext.ResponseCarguePoblacion.FromSqlRaw<ResponseCarguePoblacionDto>(sql, parms.ToArray()).ToListAsync();
                        _dBAuditCACContext.SaveChanges();

                        //Almacenamos resultados del proceso
                        _util.EscribirArchivoPlano(outputPath, dataResult.FirstOrDefault().Result);
                        resultDetail.Add(dataResult.FirstOrDefault().Result);
                    }

                }

                //Validamos si ha ocurrido un error en el proceso.
                bool status = true;
                if (resultDetail.Where(x => x.Contains("ERROR")).Any())
                {
                    status = false;
                }

                ////Validamos si ha ocurrido un error en el proceso.
                //bool status = true;
                //if (dataResult != null)
                //{
                //    //if (resultDetail.Where(x => x.Contains("ERROR")).Any())
                //    foreach (var item in dataResult)
                //    {
                //        if (item.EstadoEjecucion == "ERROR")
                //        {
                //            status = false;
                //        }
                //    }
                //}

                //Save File output S3                
                var results3 = await _util.LoadFileS3(outputPath, filenameoutput, _s3);

                //Registra carga
                ControlArchivos_CarguePoblacionModel controlCarga = new ControlArchivos_CarguePoblacionModel()
                {
                    CurrentProcessId = 1, //Numero ramdon
                    IdMedicion = 1,
                    ArchivoCargado = _s3.UrlFileS3Uploaded + fileName,
                    ArchivoDescargado = _s3.UrlFileS3Uploaded + filenameoutput,
                    Enable = true,
                    FechaCreacion = DateTime.Now,
                    FecchaActualizacion = DateTime.Now,
                    Usuario = ""
                };

                _dBAuditCACContext.ControlArchivos_CarguePoblacion.Add(controlCarga);
                _dBAuditCACContext.SaveChanges();

                //Delete input file
                if (File.Exists(filePath + fileName))
                {
                    try
                    {
                        File.Delete(filePath + fileName);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation(_pathLog, "ERROR: " + ex.ToString() + " " + DateTime.Now.ToString());
                    }
                }
                _logger.LogInformation(_pathLog, "Elimina archivo origen  " + DateTime.Now.ToString());

                //Delete input file
                if (File.Exists(outputPath))
                {
                    try
                    {
                        File.Delete(outputPath);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation(_pathLog, "ERROR: " + ex.ToString() + " " + DateTime.Now.ToString());
                    }
                }
                _logger.LogInformation(_pathLog, "Elimina archivo origen  " + DateTime.Now.ToString());

                //Retornamos valores.
                ResponseReasignacionesBolsaDetalladaDto respuesta = new ResponseReasignacionesBolsaDetalladaDto()
                {
                    status = status,
                    file = controlCarga.ArchivoDescargado
                };

                return respuesta;
                //return "OK";
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }
        #endregion


        #region Calificacion Masiva

        /// <summary>
        /// Crea Template Calificacion Masiva
        /// </summary>
        /// <param name="path">ruta fisica</param>
        /// <returns>string template base 64</returns>
        public async Task<string> CreaPlantillaCalificacionMasiva(string path, int medicionId)
        {
            try
            {
                string script = "EXEC [dbo].[SP_Crea_Plantilla_Calificacion_Masiva] @MedicionId";

                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@MedicionId", Value = medicionId},
                };


                var data = await _dBAuditCACContext.TemplateCalificacionMasivaDto.FromSqlRaw<TemplateCalificacionMasivaDto>(script, parameters.ToArray()).ToListAsync();

                var line = "";


                var filename = "plantillacalificacionmasiva_" + DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".csv";

                var pathComplete = path + "\\" + filename;

                data.ToList()?.All(x =>
                {
                    line = "";

                    line += x.IdRadicado + "\t" + x.NemonicoVariable + "\t" + x.Calificacion + "\t" + x.Motivo + "\t" + x.Observacion;

                    _util.EscribirArchivoPlano(pathComplete, line);
                    return true;
                });



                var plainTextBytes = Convert.ToBase64String(File.ReadAllBytes(pathComplete));

                // Eliminamos archivo creado.
                if (File.Exists(pathComplete))
                {
                    try
                    {
                        File.Delete(pathComplete);
                    }
                    catch (Exception ex)
                    {
                        _logger.LogInformation(ex.ToString());
                    }
                }

                return plainTextBytes;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        // <summary>
        /// Inicia proceso para cargue calificacion masiva registrandolo en la tabla current process
        /// </summary>
        /// <param name="filePath">Ruta del archivo a cargar</param>
        /// <param name="input">Modelo Cargue</param>
        /// <returns>Modelo Current Process</returns>
        public async Task<CurrentProcessModel> IniciaProcesoCalificacionMasiva(string filePath, InputCalificacionMasiva input, S3Model s3)
        {
            try
            {
                _logger.LogInformation("Inicia core calificacion masiva ");
                _logger.LogInformation(input.FileBase64);

                //Traemos utilidades.
                Util _util = new Util();

                string fileName = (DateTime.Now.ToString("yyyy:MM:dd:HH:mm:ss").Replace(":", "") + input.FileName);

                //Eliminamos caracteres no deseados
                //input.FileBase64 = input.FileBase64.Replace("data:application/vnd.ms-excel;base64,", "");
                var SeparadorBase64 = _dBAuditCACContext.ParametrosGenerales.Where(x => x.Id == (int)Enumeration.Parametros.SeparadorBase64).Where(x => x.Activo).Select(x => new { x.Valor }).FirstOrDefault().Valor;
                if (SeparadorBase64 == null || SeparadorBase64 == "") { SeparadorBase64 = "data:application/vnd.ms-excel;base64,[data:text/csv;base64,"; } //Valor por defecto si no encuentra parametro.                
                input.FileBase64 = _util.EliminarTextoBase64(input.FileBase64, SeparadorBase64);

                File.WriteAllBytes(filePath + fileName, Convert.FromBase64String(input.FileBase64));
                _logger.LogInformation("Convierte base 64 a archivo");

                var file = File.ReadAllLines(filePath + fileName);

                //Save File S3                
                var result = await _util.LoadFileS3(filePath + fileName, fileName, s3);
                _logger.LogInformation("Guarda archivo s3");

                //Inicializa modelo para proceso cargue de poblacion
                CurrentProcessModel inputCurrentProcess = new CurrentProcessModel()
                {
                    ProcessId = (int)Enumeration.Procesos.CalificacionMasiva,
                    Progress = -1,
                    InitDate = DateTime.Now,
                    User = input.Usuario,
                    Result = "Medicion" + input.Medicion + ",0," + (file.Count() - 1).ToString(),//Id medicion, inicio 0, maximo de lineas -1 de la cabecera
                    File = true,
                };

                //Registra proceso actual para iniciar el cargue de poblacion
                var resultCurrentProcess = _dBAuditCACContext.CurrentProcessModel.Add(inputCurrentProcess);
                _dBAuditCACContext.SaveChanges();

                //Consulta Id de registro agregado
                inputCurrentProcess.Id = _dBAuditCACContext.CurrentProcessModel.Where(x =>
                    x.ProcessId == inputCurrentProcess.ProcessId && x.User == inputCurrentProcess.User && x.Progress == x.Progress)
                        .OrderByDescending(x => x.Id)
                        .Select(x => x.Id).FirstOrDefault();

                //Inicializa modelo para parametros de proceso cargue de poblacion
                CurrentProcessParamModel inputparam = new CurrentProcessParamModel()
                {
                    CurrentProcessId = inputCurrentProcess.Id,
                    Name = "filePath",
                    Position = 0,
                    Value = filePath + fileName
                };

                //Registra parametros proceso actual
                var resultCurrentProcessParam = _dBAuditCACContext.CurrentProcessParamModel.Add(inputparam);
                _dBAuditCACContext.SaveChanges();


                _logger.LogInformation("Termina registro para proceso de calificacion masiva");

                return inputCurrentProcess;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }




        #endregion

        public async Task<IEnumerable<ConsultaEPSItemDto>> ObtenerListadoDeEPS(int idCobertura)
        {

            string sql = "EXEC [dbo].[SP_Consulta_EPS_Nombres] @idCobertura";

            List<SqlParameter> parms = new List<SqlParameter>
            {
                new SqlParameter { ParameterName = "@idCobertura", Value = idCobertura }
            };

            IEnumerable<ConsultaEPSItemDto> data = 
                    _dBAuditCACContext.ConsultaEPSItemDto.FromSqlRaw(sql, parms.ToArray()).ToList();

            return data;
        }
    }
}
