using AuditCAC.Dal.Data;
using AuditCAC.Domain.Dto;
using AuditCAC.Domain.Dto.CalificacionMasiva;
using AuditCAC.Domain.Dto.CarguePoblacion;
using AuditCAC.Domain.Entities;
using AuditCAC.Domain.Helpers;
using AuditCAC.ExternalServices.ApiServices;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Dynamic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AuditCAC.Core.Core
{
    public class ProcesoCore : IProcesoCore
    {
        public static IConfiguration configuration;
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        DBAuditCACContext _dBAuditCACContext;
        DBCACContext _dBCACContext;
        private Util _util = new Util();
        private RequestCore _requestCore;
        public List<ParametroGeneralModel> parameterList;
        private BaseServiceApi _requestBaseApi;
        public string _pathLog;
        S3Model _s3;


        private readonly SqlConnection CacConnection;
        private readonly SqlConnection AudConnection;
        private SqlDataReader sqlDataReader;
        private SqlCommand sqlCommand;

        //Constructor para procceso Bash de cargue de poblacion.
        public ProcesoCore(DBAuditCACContext dBAuditCACContext, S3Model s3, string pathLog)
        {
            this._dBAuditCACContext = dBAuditCACContext;
            this._s3 = s3;
            this._pathLog = pathLog;
        }

        //Constructor para consola de migracion de datos.        
        public ProcesoCore(string pathLog, DBCACContext dBCACContext, DBAuditCACContext dBAuditCACContext)
        {
            _dBCACContext = dBCACContext;
            _dBCACContext.Database.SetCommandTimeout(5000);
            _dBAuditCACContext = dBAuditCACContext;
            _dBAuditCACContext.Database.SetCommandTimeout(5000);
            _pathLog = pathLog;

            CacConnection = (SqlConnection)dBCACContext.Database.GetDbConnection();
            AudConnection = (SqlConnection)dBAuditCACContext.Database.GetDbConnection();

        }

        public async void ExecuteCurrentProcess()
        {
            try
            {
                _pathLog = _pathLog.Replace(".txt", "") + DateTime.Now.ToString("yyyy:MM:dd").Replace(":", "") + ".txt";

                var currentProcess = _dBAuditCACContext.CurrentProcessModel.Where(x => x.Progress == -1).FirstOrDefault();
                EscribirArchivoPlano(_pathLog, " ");

                //Actualizar version
                EscribirArchivoPlano(_pathLog, "--------------------------------------- \n V.1.0.22 \n--------------------------------------- ");

                EscribirArchivoPlano(_pathLog, "Consulta procesos " + DateTime.Now.ToString());
                if (currentProcess != null)

                {
                    EscribirArchivoPlano(_pathLog, "Toma proceso actual  " + currentProcess.Id + " " + DateTime.Now.ToString());

                    currentProcess.Progress = 0;
                    _dBAuditCACContext.SaveChanges();

                    EscribirArchivoPlano(_pathLog, "Actualiza estado a 0 " + currentProcess.Id + " " + DateTime.Now.ToString());


                    parameterList = _dBAuditCACContext.ParametrosGenerales.Where(x => x.Activo).ToList();

                    var prueba = new InputsMoverRegistrosAuditoriaPascientesDto();


                    switch (currentProcess.ProcessId)
                    {
                        case (int)Enumeration.Procesos.CarguePoblacion:
                            EscribirArchivoPlano(_pathLog, "Inicia Cargue Población  " + currentProcess.Id + " " + DateTime.Now.ToString());
                            var result = await CarguePoblacion(currentProcess.Id);

                            EscribirArchivoPlano(_pathLog, "Resultado  progreso: " + result + " " + DateTime.Now.ToString());

                            currentProcess.Progress = Convert.ToInt32(result);
                            _dBAuditCACContext.SaveChanges();

                            EscribirArchivoPlano(_pathLog, "Termina Cargue Población  " + currentProcess.Id + " " + DateTime.Now.ToString());

                            break;

                        case (int)Enumeration.Procesos.CalificacionMasiva:
                            EscribirArchivoPlano(_pathLog, "Inicia Calificacion Masiva  " + currentProcess.Id + " " + DateTime.Now.ToString());

                            var resultCalificacion = await Calificacionmasiva(currentProcess.Id);

                            EscribirArchivoPlano(_pathLog, "Resultado  progreso: " + resultCalificacion + " " + DateTime.Now.ToString());

                            currentProcess.Progress = Convert.ToInt32(resultCalificacion);
                            _dBAuditCACContext.SaveChanges();

                            EscribirArchivoPlano(_pathLog, "Termina Calificacion masiva  " + currentProcess.Id + " " + DateTime.Now.ToString());

                            break;

                        default:
                            Console.WriteLine($"Measured value");
                            break;
                    }
                }
                else
                {
                    EscribirArchivoPlano(_pathLog, "No hay procesos pendientes por ejecutar" + DateTime.Now.ToString());

                }

            }
            catch (Exception ex)
            {
                EscribirArchivoPlano(_pathLog, "ERROR: " + ex.ToString() + " " + DateTime.Now.ToString());

                throw ex;
            }
        }

        #region CarguePoblacion

        public async Task<decimal> CarguePoblacion(int currentProcessId)
        {
            var currentProcess = new CurrentProcessModel();

            try
            {
                //Actualiza progreso del proceso a 0
                currentProcess = _dBAuditCACContext.CurrentProcessModel.Where(x => x.Id == currentProcessId).FirstOrDefault();

                //Consulta parametros del proceso
                var currentProcessParam = _dBAuditCACContext.CurrentProcessParamModel.Where(x => x.CurrentProcessId == currentProcessId).FirstOrDefault();
                currentProcess.Progress = 0;
                _dBAuditCACContext.SaveChanges();
                string result = currentProcess.Result;
                string medicion = "";
                string EstructuraVariable = ""; 
                int current = 0;
                int max = 0;
                string currentline = "";
                var separed = result.Split(",");
                current = Convert.ToInt32(separed[1]);
                medicion = separed[0];
                EstructuraVariable = separed[3];
                int idmedicion = Convert.ToInt32(medicion.Replace("Medicion", ""));
                //int idmedicion = Convert.ToInt32(medicion);

                max = Convert.ToInt32(separed[2]);

                var lines = File.ReadLines(currentProcessParam.Value).ToList();

                EscribirArchivoPlano(_pathLog, "Lee  " + lines.Count() + " lineas de arhcivo para cargue de poblacion " + DateTime.Now.ToString());


                List<string> resultDetail = new List<string>();

                decimal unidad = 100M / lines.Count();
                decimal progress = 0;


                #region Cantidad campos requeridos

                EscribirArchivoPlano(_pathLog, "SP_Consulta_Cantidad_Campos_Cargue_Poblacion " + DateTime.Now.ToString());

                string script = "EXEC [dbo].[SP_Consulta_Cantidad_Campos_Cargue_Poblacion] @IdMedicion, @EstructuraVariable";

                List<SqlParameter> parameters = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@IdMedicion", Value = idmedicion},
                    new SqlParameter { ParameterName = "@EstructuraVariable", Value = EstructuraVariable},
                };


                var data = await _dBAuditCACContext.LLaveValor.FromSqlRaw<ResponseLlaveValor>(script, parameters.ToArray()).ToListAsync();

                int columnsCountRequired = data.ToList()[0].Id;

                EscribirArchivoPlano(_pathLog, "Consulta campos requeridos " + columnsCountRequired + " " + DateTime.Now.ToString());

                #endregion;

                EscribirArchivoPlano(_pathLog, "separador" + "\t" + ".......");

                //Output file
                EscribirArchivoPlano(_pathLog, "Archivo entrada " + currentProcessParam.Value);

                var inputPath = currentProcessParam.Value.Split("\\");

                string filename = inputPath[inputPath.Count() - 1];
                string filenameoutput = "resultado_" + filename;
                var outputPath = currentProcessParam.Value.Replace(filename, filenameoutput);

                EscribirArchivoPlano(_pathLog, "Inicia recorrido registros");

                EscribirArchivoPlano(_pathLog, "Primer linea " + lines[0].ToString());

                //Declare Header datatable
                DataTable headerDataTable = new DataTable();
                headerDataTable.Columns.Add("Id", typeof(int));
                headerDataTable.Columns.Add("Valor", typeof(string));

                var headerLine = lines[0].Split(new char[] { '\t' });



                //Header Validation
                if (headerLine.Count() != columnsCountRequired)
                {
                    string rs = "ERROR\tCabecera con " + headerLine.Count() + " columnas, no cumple con la cantidad de columnas requeridas: " + columnsCountRequired;
                    EscribirArchivoPlano(outputPath, rs);
                    resultDetail.Add(rs);
                }
                else //Go through detail
                {

                    //Set data for header data table
                    var tempHeaderTableString = "";
                    for (int i = 0; i < headerLine.Count(); i++)
                    {
                        headerDataTable.Rows.Add(i + 1, headerLine[i]);
                        //tempHeaderTableString += "INSERT INTO @HeaderTable VALUES (" + (i + 1) + ",'" + headerLine[i] + "'); "; //ToDo Delete for release

                    }

                    try
                    {
                        //Declare record datatable
                        DataTable recordDataTable = new DataTable();
                        recordDataTable.Columns.Add("Id", typeof(int));
                        recordDataTable.Columns.Add("Valor", typeof(string));

                        Util util = new Util();
                        var separator = util.GetAndValidateParameter(parameterList, Enumeration.Parametros.SeparadorArchivos);

                        var tempDetailTableString = "";


                        EscribirArchivoPlano(_pathLog, "Inicia construccion data table detalle " + DateTime.Now.ToString());
 
                        //Recorre lineas archivo para construir data table
                        for (int i = 1; i < lines.Count(); i++)
                        {
                            recordDataTable.Rows.Add(i, lines[i].Replace("\t", separator));
                           // tempDetailTableString += "INSERT INTO @LineTable VALUES (" + i + ",'" + lines[i].Replace("\t", separator) + "'); ";  //ToDo Delete for release
                        }

                        EscribirArchivoPlano(_pathLog, "Finaliza construccion data table detalle " + DateTime.Now.ToString());


                        //progress += unidad;
                        //currentProcess.Progress = (int)Math.Floor(progress);
                        //currentProcess.Result = "Medicion" + idmedicion + "," + i + ", " + max;

                        string sql = "EXEC [dbo].[SP_Realiza_Cargue_Poblacion] " +
                                        "@HeaderTable, " +
                                        "@Lines, " +
                                        "@IdMedicion, " +
                                        "@User, " +
                                        "@CurrentProcessId, " +
                                        "@ResultCurrentProcess, " +
                                        "@Progreso, " +
                                        "@CantidadColumnas";


                        var parameterHeader = new SqlParameter("@HeaderTable", SqlDbType.Structured);
                        parameterHeader.Value = headerDataTable;
                        parameterHeader.TypeName = "dbo.DT_LLave_Valor";

                        var parameterRecord = new SqlParameter("@Lines", SqlDbType.Structured);
                        parameterRecord.Value = recordDataTable;
                        parameterRecord.TypeName = "dbo.DT_LLave_Valor";

                        List<SqlParameter> parms = new List<SqlParameter>
                                {
                                    parameterHeader,
                                    parameterRecord,
                                    new SqlParameter { ParameterName = "@IdMedicion", Value = idmedicion},
                                    new SqlParameter { ParameterName = "@User", Value = currentProcess.User},
                                    new SqlParameter { ParameterName = "@CurrentProcessId", Value = currentProcess.Id},
                                    new SqlParameter { ParameterName = "@ResultCurrentProcess", Value = currentProcess.Result},
                                    new SqlParameter { ParameterName = "@Progreso", Value = currentProcess.Progress},
                                    new SqlParameter { ParameterName = "@CantidadColumnas", Value = columnsCountRequired}


                                };

                        
                        var timeout = util.GetAndValidateParameter(parameterList, Enumeration.Parametros.TimeOutCarguePoblacion);

                       
                        
                        _dBAuditCACContext.Database.SetCommandTimeout(Convert.ToInt32(timeout));
                        
                        var initDate = DateTime.Now;
                        EscribirArchivoPlano(_pathLog, "Inicia ejecucion SP_Realiza_Cargue_Poblacion, ProcessId " + currentProcess.Id + " " + DateTime.Now.ToString());

                        var dataResult = await _dBAuditCACContext.Database.ExecuteSqlRawAsync(sql, parms.ToArray());

                        var FinishDate = DateTime.Now;
                        EscribirArchivoPlano(_pathLog, "Finaliza ejecucion SP_Realiza_Cargue_Poblacion, ProcessId " + currentProcess.Id + " " + DateTime.Now.ToString());


                        _dBAuditCACContext.SaveChanges();



                    }
                    catch (Exception ex )
                    {
                        throw;
                        
                    }

                }


                List<ResponseLlaveValor> dataResultado = new List<ResponseLlaveValor>();
                #region Consulta y escribe archivo plano

                try
                {
                    EscribirArchivoPlano(_pathLog, "SP_Consulta_Resultado_Cargue_Poblacion" + DateTime.Now.ToString());

                    string scriptResultado = "EXEC [dbo].[SP_Consulta_Resultado_Cargue_Poblacion] @CurrentProcessId";

                    List<SqlParameter> parametersResultado = new List<SqlParameter>
                    {
                        new SqlParameter { ParameterName = "@CurrentProcessId", Value = currentProcess.Id},
                    };


                    dataResultado = await _dBAuditCACContext.LLaveValor.FromSqlRaw<ResponseLlaveValor>(scriptResultado, parametersResultado.ToArray()).ToListAsync();


                    EscribirArchivoPlano(_pathLog, "Resultado SP_Consulta_Resultado_Cargue_Poblacion, ProcessId " + currentProcess.Id + " , " + dataResultado.Count() + " registros");
                    EscribirArchivoPlano(_pathLog, "Termina de escribir en archivo detalle" + DateTime.Now.ToString() + " " + outputPath);


                    dataResultado?.All(x =>
                    {
                        EscribirArchivoPlano(outputPath, x.Valor);
                        return true;
                    });

                    EscribirArchivoPlano(_pathLog, "Termina de escribir en archivo detalle" + DateTime.Now.ToString() + " " + outputPath);

                }
                catch (Exception ex)
                {
                    EscribirArchivoPlano(_pathLog, "Error escribiendo archivo detalle" + DateTime.Now.ToString() + " " + ex.ToString());
                }


                #endregion

                if (dataResultado.Count() == 0)
                {
                    EscribirArchivoPlano(outputPath, "Resultado sin regisros, por favor valide el archivo de cargue");
                }

                string resultfile = "OK";

                if (dataResultado.Where(x => x.Valor.Contains("ERROR")).Any())
                {
                    resultfile = "ERROR";
                }

                if (dataResultado.Where(x => x.Valor.Contains("OK")).Any())
                {
                    // actualiza estado de medicion
                    var medicionRow = _dBAuditCACContext.MedicionModel.Where(x => x.Id == idmedicion).FirstOrDefault();
                    medicionRow.Estado = 29; //Asignada
                    _dBAuditCACContext.SaveChanges();
                }

                currentProcess.Progress = -2;
                currentProcess.Result = currentProcess.Result + "," + resultfile + "," + this._s3.UrlFileS3Uploaded + filenameoutput;
                _dBAuditCACContext.SaveChanges();

                //Save File output S3
                Util _util = new Util();
                var results3 = await _util.LoadFileS3(outputPath, filenameoutput, this._s3);

                //Registra carga
                ControlArchivos_CarguePoblacionModel controlCarga = new ControlArchivos_CarguePoblacionModel()
                {
                    CurrentProcessId = currentProcess.Id,
                    IdMedicion = idmedicion,
                    ArchivoCargado = this._s3.UrlFileS3Uploaded + filename,
                    ArchivoDescargado = this._s3.UrlFileS3Uploaded + filenameoutput,
                    Enable = true,
                    FechaCreacion = DateTime.Now,
                    FecchaActualizacion = DateTime.Now,
                    Usuario = currentProcess.User
                };

                _dBAuditCACContext.ControlArchivos_CarguePoblacion.Add(controlCarga);
                _dBAuditCACContext.SaveChanges();

                EscribirArchivoPlano(_pathLog, "Guarda control en db " + DateTime.Now.ToString());


                //Delete input file
                if (File.Exists(currentProcessParam.Value))
                {
                    try
                    {
                        File.Delete(currentProcessParam.Value);
                    }
                    catch (Exception ex)
                    {
                        EscribirArchivoPlano(_pathLog, "ERROR: " + ex.ToString() + " " + DateTime.Now.ToString());
                    }
                }
                EscribirArchivoPlano(_pathLog, "Elimina archivo origen  " + DateTime.Now.ToString());

                //Delete input file
                if (File.Exists(outputPath))
                {
                    try
                    {
                        File.Delete(outputPath);
                    }
                    catch (Exception ex)
                    {
                        EscribirArchivoPlano(_pathLog, "ERROR: " + ex.ToString() + " " + DateTime.Now.ToString());
                    }
                }
                EscribirArchivoPlano(_pathLog, "Elimina archivo origen  " + DateTime.Now.ToString());


                return -2;

            }
            catch (Exception ex)
            {
                EscribirArchivoPlano(_pathLog, "ERROR: " + ex.ToString() + " " + DateTime.Now.ToString());
                currentProcess.Progress = -3;
                _dBAuditCACContext.SaveChanges();
                return -3;
            }
        }

        #endregion
        private object RegistrosAuditoriaPascientes(int currentProcessId)
        {
            try
            {
                this._requestCore = new RequestCore();
                var tokenInfo = _requestCore.requestAuthenticationCAC(
                    _util.GetAndValidateParameter(parameterList, Enumeration.Parametros.URLAuditCACCoreCACServices), //Url parameter
                    _util.GetAndValidateParameter(parameterList, Enumeration.Parametros.UsuarioTokenAuditCACCoreCACServices), //User Parameter
                    _util.GetAndValidateParameter(parameterList, Enumeration.Parametros.PasswordTokenAuditCACCoreCACServices) //Password Paramter
                    );
                var currentProcess = _dBAuditCACContext.CurrentProcessParamModel.Where(x => x.CurrentProcessId == currentProcessId).ToList();
                var ids = currentProcess.Where(x => x.Name == "Id").FirstOrDefault().Value;
                ids = ids.Substring(1, ids.Length - 2);
                String[] Separator = { "," };
                int[] ArrayId = ids.Split(Separator, StringSplitOptions.RemoveEmptyEntries).Select(n => Convert.ToInt32(n)).ToArray();
                var Data = new
                {
                    Id = ArrayId,
                    IdMedicion = currentProcess.Where(x => x.Name == "IdMedicion").FirstOrDefault().Value
                };
                var getData = _requestCore.requestGetpatient(_util.GetAndValidateParameter(parameterList, Enumeration.Parametros.URLAuditCACCoreCACServices), Data, tokenInfo.Token);
                var insertData = _requestCore.requestPostpatient(_util.GetAndValidateParameter(parameterList, Enumeration.Parametros.URLAuditCACCoreCACServices), getData.Result, tokenInfo.Token);
                return insertData;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        #region Test

        public async Task<string> TestProcess(int currentProcessId)
        {
            try
            {
                var currentProcess = _dBAuditCACContext.CurrentProcessParamModel.Where(x => x.CurrentProcessId == currentProcessId).ToList();


                string sql = "EXEC [dbo].[SP_Test_Proceso] @CurrentProcessId, @TestParam";

                string param2 = currentProcess.Where(x => x.Name == "TestParam").FirstOrDefault().Value;
                List<SqlParameter> parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@CurrentProcessId", Value = currentProcessId },
                    new SqlParameter { ParameterName = "@TestParam", Value = param2 },

                };


                //var Data = await _dBAuditCACContext.Database.ExecuteSqlRawAsync(sql, parms.ToArray(), Task.Delay(60000));

                _dBAuditCACContext.Database.SetCommandTimeout(5000);
                var Data = _dBAuditCACContext.Database.ExecuteSqlRaw(sql, parms.ToArray());


                return "OK";

            }
            catch (Exception ex)
            {
                EscribirArchivoPlano(_pathLog, "ERROR: " + ex.ToString() + " " + DateTime.Now.ToString());

                throw ex;
            }
        }


        #endregion

        #region Write

        /// <summary>
        /// Escribir archivo plano
        /// </summary>
        /// <param name="path">ruta</param>
        /// <param name="line">texto</param>
        public void EscribirArchivoPlano(string path, string line)
        {
            try
            {
                using (StreamWriter outputFile = new StreamWriter(path, true))
                {
                    outputFile.WriteLine(line);
                }
            }
            catch (Exception ex)
            {

                //throw;
            }
        }

        #endregion


        #region MigrarDataTablasReferenciales
        public async Task<bool> MigrarDataTablasReferenciales()
        {
            try
            {
                _dBCACContext.Database.SetCommandTimeout(15000);
                _dBAuditCACContext.Database.SetCommandTimeout(15000);

                EscribirArchivoPlano(_pathLog, "\n");
                EscribirArchivoPlano(_pathLog, " ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- ");
                EscribirArchivoPlano(_pathLog, " ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- ");
                EscribirArchivoPlano(_pathLog, "INICIA PROCESO DE MIGRACION TABLAS REFERENCIALES: " + DateTime.Now.ToString());
                EscribirArchivoPlano(_pathLog, " ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- ");
                EscribirArchivoPlano(_pathLog, " ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- ");                                

                // Variables
                bool status = false;
                int CountId = 1;
                int CountTablas = 1;

                // 1. Consultamos listado de tablas (SP_Consultar_Listado_TablasCamposReferenciales).
                EscribirArchivoPlano(_pathLog, "Exito: " + " Inicia consulta de tablas referenciales." + " " + DateTime.Now.ToString());
                string sqlListado = "EXEC [dbo].[SP_Consultar_Listado_TablasCamposReferenciales]";

                List<SqlParameter> parmsListado = new List<SqlParameter> { };

                var Data_Listado_TablasCamposReferenciales = await _dBCACContext.ResponseConsultarListadoTablasCamposReferencialesDto.FromSqlRaw<ResponseConsultarListadoTablasCamposReferencialesDto>(sqlListado, parmsListado.ToArray()).ToListAsync();
                EscribirArchivoPlano(_pathLog, "Exito: " + " Proceso de consulta de campos referenciales finalizado con exito." + " " + DateTime.Now.ToString());
                // --

                EscribirArchivoPlano(_pathLog, "Exito: " + " Resultado consulta. " + Data_Listado_TablasCamposReferenciales.Count + ". " + DateTime.Now.ToString());

                // 1.1. Validamos si existen tablas.
                if (Data_Listado_TablasCamposReferenciales.Count > 0)
                {

                    // 2. Recorremos listado de datos recibidos y consultamos en las respectivas tablas. (SP_Consultar_Datos_TablasCamposReferenciales)                    
                    foreach (var item in Data_Listado_TablasCamposReferenciales)
                    {
                        EscribirArchivoPlano(_pathLog, " ----------------------------------------------------------------------------------------------------------- ");
                        EscribirArchivoPlano(_pathLog, "Exito: " + " Inicia proceso tabla referencial: " + item.tablaReferencial + ". " + DateTime.Now.ToString());
                        EscribirArchivoPlano(_pathLog, "Registro No: " + CountTablas + ". De: " + Data_Listado_TablasCamposReferenciales.Count + ". " + DateTime.Now.ToString());

                        var Data_TablasCamposReferenciales = new List<ResponseConsultarDatosTablasCamposReferencialesDto>();
                        try
                        {
                            string sqlDatos = "EXEC [dbo].[SP_Consultar_Datos_TablasCamposReferenciales] @TablaReferencial, @CampoReferencial";

                            List<SqlParameter> parmsDatos = new List<SqlParameter>
                            {
                                new SqlParameter { ParameterName = "@TablaReferencial", Value = item.tablaReferencial},
                                new SqlParameter { ParameterName = "@CampoReferencial", Value = item.campoReferencial},
                            };

                            Data_TablasCamposReferenciales = await _dBCACContext.ResponseConsultarDatosTablasCamposReferencialesDto.FromSqlRaw<ResponseConsultarDatosTablasCamposReferencialesDto>(sqlDatos, parmsDatos.ToArray()).ToListAsync();
                            var TEMP = Data_TablasCamposReferenciales;

                            if (Data_TablasCamposReferenciales.FirstOrDefault().Status == "ERROR")
                            {
                                EscribirArchivoPlano(_pathLog, "ERROR: " + Data_TablasCamposReferenciales.FirstOrDefault().Mensaje + ". " + DateTime.Now.ToString());
                            }
                            else
                            {
                                EscribirArchivoPlano(_pathLog, "Exito: " + " Proceso de consulta consulta de datos en tablas y campos referenciales finalizado con exito." + " " + DateTime.Now.ToString());

                                try
                                {
                                    // 3. Ejecutamos migracion de datos. (SP_Insertar_Datos_TablasCamposReferenciales)
                                    EscribirArchivoPlano(_pathLog, "Exito: " + " Inicia insercion de datos en campos y tablas temporales. " + " " + DateTime.Now.ToString());

                                    //Llamamos SP.
                                    string sql = "EXEC [dbo].[SP_Insertar_Datos_TablasCamposReferenciales] @Listado";

                                    //Declaramos parametro.
                                    var Parameter = new SqlParameter("@Listado", SqlDbType.Structured);

                                    //Convertimos List recibida a DataTable
                                    DataTable ListData = new DataTable();
                                    ListData.Columns.Add("Id", typeof(int));
                                    ListData.Columns.Add("TablaReferencial", typeof(string));
                                    ListData.Columns.Add("CampoReferencial", typeof(string));
                                    ListData.Columns.Add("CampoReferencialNombre", typeof(string));
                                    ListData.Columns.Add("UserId", typeof(string));

                                    //
                                    foreach (var itemDatos in Data_TablasCamposReferenciales)
                                    {
                                        ListData.Rows.Add(CountId, itemDatos.TablaReferencial, itemDatos.CampoReferencial, itemDatos.CampoReferencialNombre, ""); //CAMBIAR ESTE ID QUEMADO POR UN ID ENVIADO
                                        CountId++;
                                    }
                                    CountId = 0;

                                    //Agregamos valores a Parametros.
                                    Parameter.Value = ListData;
                                    Parameter.TypeName = "dbo.DT_TablasCamposReferenciales";

                                    //Ejecutamos procedimiento.
                                    var Data = await _dBAuditCACContext.Database.ExecuteSqlRawAsync(sql, Parameter);
                                    //Version que captura el error, pendiente de modificar. 
                                    //dataResult = _dBAuditCACContext.ResponseCarguePoblacion.FromSqlRaw<ResponseCarguePoblacionDto>(sql, Parameter).ToList();
                                    //_dBAuditCACContext.SaveChanges();
                                    //dataResult.FirstOrDefault().Result  //Obtenemos datos de resultado.

                                    EscribirArchivoPlano(_pathLog, "Exito: " + " Proceso de insercion de datos en campos y tablas temporales finalizado con exito." + " " + DateTime.Now.ToString());
                                }
                                catch (Exception ex)
                                {
                                    EscribirArchivoPlano(_pathLog, "ERROR: En insercion de datos. Ex: " + ex.ToString() + " " + DateTime.Now.ToString());
                                    //return false;
                                }
                                // --

                                EscribirArchivoPlano(_pathLog, "Exito: " + " Finaliza proceso tabla referencial: " + item.tablaReferencial + ". " + DateTime.Now.ToString());                                
                            }
                        }
                        catch (Exception ex)
                        {
                            EscribirArchivoPlano(_pathLog, "ERROR: En consulta de datos en tablas y campos referenciales. Ex: " + ex.ToString() + " " + DateTime.Now.ToString());
                            //return false;
                        }
                        // --
                        CountTablas++;
                        EscribirArchivoPlano(_pathLog, " ----------------------------------------------------------------------------------------------------------- ");
                    } // END Foreach
                }
                else
                {
                    EscribirArchivoPlano(_pathLog, "Advertencia: " + " Proceso de consulta de campos referenciales finalizado con exito, pero no genero resultados." + " " + DateTime.Now.ToString());
                }


                // Validamos resultado final de ejecucion
                // --

                // Retornamos datos
                EscribirArchivoPlano(_pathLog, "\n");
                EscribirArchivoPlano(_pathLog, " ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- ");
                EscribirArchivoPlano(_pathLog, " ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- ");
                EscribirArchivoPlano(_pathLog, "FINALIZA PROCESO DE MIGRACION TABLAS REFERENCIALES: " + DateTime.Now.ToString());
                EscribirArchivoPlano(_pathLog, " ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- ");
                EscribirArchivoPlano(_pathLog, " ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- ");
                EscribirArchivoPlano(_pathLog, "\n");
                return status;
            }
            catch (Exception ex)
            {
                EscribirArchivoPlano(_pathLog, "ERROR: " + ex.ToString() + " " + DateTime.Now.ToString());
                return false;
            }
        }
        #endregion



        #region Migracion Tablas

        public void MigCACCobertura()
        {
            List<ResponseSpGetDataMigEnfermedad> datos;
            try
            {
                EscribirArchivoPlano(_pathLog, "\n");
                EscribirArchivoPlano(_pathLog, " ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- ");
                EscribirArchivoPlano(_pathLog, " ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- ");
                EscribirArchivoPlano(_pathLog, "INICIA PROCESO DE MIGRACION TABLA COBERTURA: " + DateTime.Now.ToString());
                EscribirArchivoPlano(_pathLog, " ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- ");
                EscribirArchivoPlano(_pathLog, " ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- ");

                EscribirArchivoPlano(_pathLog, "Inicia adquisicion de datos para MigCACCobertura");
                datos = _dBCACContext.ResponseSpGetDataMigEnfermedadDTO
                    .FromSqlRaw<ResponseSpGetDataMigEnfermedad>("EXEC [dbo].[SP_GetDataMigEnfermedad]").ToList();
                EscribirArchivoPlano(_pathLog, "Finaliza adquisicion de datos para MigCACCobertura");

                sqlCommand = new SqlCommand("[dbo].[SP_MigCobertura]", AudConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                SqlParameter tabla = new("@CacTable", SqlDbType.Structured);

                DataTable ListData = new DataTable();
                ListData.Columns.Add("idCobertura", typeof(int));
                ListData.Columns.Add("nombre", typeof(string));
                ListData.Columns.Add("nemonico", typeof(string));
                ListData.Columns.Add("legislacion", typeof(string));
                ListData.Columns.Add("definicion", typeof(string));
                ListData.Columns.Add("tiempoEstimado", typeof(int));
                ListData.Columns.Add("ExcluirNovedades", typeof(bool));
                ListData.Columns.Add("Novedades", typeof(String));
                ListData.Columns.Add("idResolutionSiame", typeof(int));
                ListData.Columns.Add("NovedadesCompartidosUnicos", typeof(String));
                ListData.Columns.Add("CantidadVariables", typeof(int));
                ListData.Columns.Add("idCoberturaPadre", typeof(int));
                ListData.Columns.Add("idCoberturaAdicionales", typeof(int));

                EscribirArchivoPlano(_pathLog, "Inicia creacion de DataTable de MigCACCobertura");
                foreach (ResponseSpGetDataMigEnfermedad item in datos)
                {
                    ListData.Rows.Add(item.idCobertura, item.nombre, item.nemonico, item.legislacion,
                        item.definicion, item.tiempoEstimado, item.ExcluirNovedades, item.Novedades,
                        item.idResolutionSiame, item.NovedadesCompartidosUnicos, item.CantidadVariables
                        , item.idCoberturaPadre, item.idCoberturaAdicionales);
                }
                EscribirArchivoPlano(_pathLog, "finaliza creacion de DataTable de MigCACCobertura");

                tabla.Value = ListData;
                tabla.TypeName = "dbo.MigCACCobertura";

                sqlCommand.Parameters.Add(tabla);
                AudConnection.Open();
                EscribirArchivoPlano(_pathLog, "Inicia proeceso de insercion de datos de MigCACCobertura");
                sqlDataReader = sqlCommand.ExecuteReader();
                EscribirArchivoPlano(_pathLog, "Finaliza proeceso de insercion de datos de MigCACCobertura");

                EscribirArchivoPlano(_pathLog, "\n");
                EscribirArchivoPlano(_pathLog, " ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- ");
                EscribirArchivoPlano(_pathLog, " ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- ");
                EscribirArchivoPlano(_pathLog, "FINALIZA PROCESO DE MIGRACION TABLA COBERTURA: " + DateTime.Now.ToString());
                EscribirArchivoPlano(_pathLog, " ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- ");
                EscribirArchivoPlano(_pathLog, " ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- ");

                AudConnection.Close();
            }
            catch (Exception ex)
            {
                AudConnection.Close();
                EscribirArchivoPlano(_pathLog, "Ha ocurrido un error en MigCACCobertura:" + ex.Message);
            }
        }
        public void MigCACError() {
            List<ResponseSPGetDataMigError> datos;
            try
            {
                EscribirArchivoPlano(_pathLog, "\n");
                EscribirArchivoPlano(_pathLog, " ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- ");
                EscribirArchivoPlano(_pathLog, " ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- ");
                EscribirArchivoPlano(_pathLog, "INICIA PROCESO DE MIGRACION TABLA ERROR: " + DateTime.Now.ToString());
                EscribirArchivoPlano(_pathLog, " ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- ");
                EscribirArchivoPlano(_pathLog, " ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- ");

                EscribirArchivoPlano(_pathLog, "Inicia adquisicion de datos para MigCACError");
                datos = _dBCACContext.ResponseSPGetDataMigErrorDTO
                    .FromSqlRaw<ResponseSPGetDataMigError>("EXEC [dbo].[SP_GetDataMigError]").ToList();
                EscribirArchivoPlano(_pathLog, "Finaliza adquisicion de datos para MigCACError");

                sqlCommand = new SqlCommand("[dbo].[SP_MigError]", AudConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                SqlParameter tabla = new("@CacErrorTable", SqlDbType.Structured);

                DataTable ListData = new DataTable();
                ListData.Columns.Add("idError", typeof(string));
                ListData.Columns.Add("descripcion", typeof(String));
                ListData.Columns.Add("idTipoError", typeof(String));

                EscribirArchivoPlano(_pathLog, "Inicia creacion de DataTable de MigCACError");
                foreach (ResponseSPGetDataMigError item in datos)
                {
                    ListData.Rows.Add(item.idError, item.descripcion, item.idTipoError);
                }
                EscribirArchivoPlano(_pathLog, "finaliza creacion de DataTable de MigCACError");

                tabla.Value = ListData;
                tabla.TypeName = "dbo.MigCACError";

                sqlCommand.Parameters.Add(tabla);
                AudConnection.Open();
                EscribirArchivoPlano(_pathLog, "Inicia proeceso de insercion de datos de MigCACError");
                sqlDataReader = sqlCommand.ExecuteReader();
                EscribirArchivoPlano(_pathLog, "Finaliza proeceso de insercion de datos de MigCACError");

                EscribirArchivoPlano(_pathLog, "\n");
                EscribirArchivoPlano(_pathLog, " ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- ");
                EscribirArchivoPlano(_pathLog, " ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- ");
                EscribirArchivoPlano(_pathLog, "FINALIZA PROCESO DE MIGRACION TABLA ERROR: " + DateTime.Now.ToString());
                EscribirArchivoPlano(_pathLog, " ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- ");
                EscribirArchivoPlano(_pathLog, " ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- ");

                AudConnection.Close();
            }
            catch (Exception ex)
            {
                AudConnection.Close();
                EscribirArchivoPlano(_pathLog, "Ha ocurrido un error en MigCACError:" + ex.Message);
            }
        }

        public void MigCACRestriccionesConsistencia() {
            List<ResponseSPGetDataMigRestriccionesConsistencia> datos;
            try
            {
                EscribirArchivoPlano(_pathLog, "\n");
                EscribirArchivoPlano(_pathLog, " ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- ");
                EscribirArchivoPlano(_pathLog, " ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- ");
                EscribirArchivoPlano(_pathLog, "INICIA PROCESO DE MIGRACION TABLA RESTRICCIONES CONSISTENCIA: " + DateTime.Now.ToString());
                EscribirArchivoPlano(_pathLog, " ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- ");
                EscribirArchivoPlano(_pathLog, " ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- ");

                EscribirArchivoPlano(_pathLog, "Inicia adquisicion de datos para MigRestriccionesConsistencia");
                datos = _dBCACContext.ResponseSPGetDataMigRestriccionesConsistenciaDto
                    .FromSqlRaw<ResponseSPGetDataMigRestriccionesConsistencia>
                    ("EXEC [dbo].[SP_GetDataMigRestriccionesConsistencia]").ToList();
                EscribirArchivoPlano(_pathLog, "Finaliza adquisicion de datos para MigRestriccionesConsistencia");

                sqlCommand = new SqlCommand("[dbo].[SP_MigRestriccionesConsistencia]", AudConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                SqlParameter tabla = new("@CacTable", SqlDbType.Structured);

                DataTable ListData = new DataTable();
                ListData.Columns.Add("idRegla", typeof(int));
                ListData.Columns.Add("idRestriccionConsistencia", typeof(int));
                ListData.Columns.Add("idSignoComparacion", typeof(string));
                ListData.Columns.Add("idCompararCon", typeof(Byte));
                ListData.Columns.Add("idVariableComparacion", typeof(Int32));
                ListData.Columns.Add("idTipoValorComparacion", typeof(Byte));
                ListData.Columns.Add("valorEspecifico", typeof(String)).MaxLength=100;
                ListData.Columns.Add("idVariableAsociada", typeof(Int32));
                ListData.Columns.Add("idSignoComparacionAsociada", typeof(String));
                ListData.Columns.Add("idCompararConAsociada", typeof(Byte));
                ListData.Columns.Add("idVariableComparacionAsociada", typeof(Int32));
                ListData.Columns.Add("idTipoValorComparacionAsociada", typeof(Byte));
                ListData.Columns.Add("valorEspecificoAsociada", typeof(String)).MaxLength = 100;

                EscribirArchivoPlano(_pathLog, "Inicia creacion de DataTable de MigRestriccionesConsistencia");
                foreach (ResponseSPGetDataMigRestriccionesConsistencia item in datos)
                {
                    ListData.Rows.Add(item.idRegla, item.idRestriccionConsistencia, item.idSignoComparacion,
                        item.idCompararCon, item.idVariableComparacion, item.idTipoValorComparacion,
                        item.valorEspecifico, item.idVariableAsociada, item.idSignoComparacionAsociada,
                        item.idCompararConAsociada, item.idVariableComparacionAsociada,
                        item.idTipoValorComparacionAsociada, item.valorEspecificoAsociada);
                }
                EscribirArchivoPlano(_pathLog, "finaliza creacion de DataTable de MigRestriccionesConsistencia");

                tabla.Value = ListData;
                tabla.TypeName = "dbo.MigCACRestriccionesConsistencia";

                sqlCommand.Parameters.Add(tabla);
                AudConnection.Open();
                EscribirArchivoPlano(_pathLog, "Inicia proeceso de insercion de datos de MigRestriccionesConsistencia");
                sqlDataReader = sqlCommand.ExecuteReader();
                EscribirArchivoPlano(_pathLog, "Finaliza proeceso de insercion de datos de MigRestriccionesConsistencia");

                EscribirArchivoPlano(_pathLog, "\n");
                EscribirArchivoPlano(_pathLog, " ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- ");
                EscribirArchivoPlano(_pathLog, " ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- ");
                EscribirArchivoPlano(_pathLog, "FINALIZA PROCESO DE MIGRACION TABLA RESTRICCIONES CONSISTENCIA: " + DateTime.Now.ToString());
                EscribirArchivoPlano(_pathLog, " ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- ");
                EscribirArchivoPlano(_pathLog, " ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- ");

                AudConnection.Close();
            }
            catch (Exception ex)
            {
                AudConnection.Close();
                EscribirArchivoPlano(_pathLog, "Ha ocurrido un error en MigRestriccionesConsistencia:" + ex.Message);
            }
        }

        public void MigCACPeriodo() {
            List<ResponseSPGetDataMigPeriodo> datos;
            try {

                EscribirArchivoPlano(_pathLog, "\n");
                EscribirArchivoPlano(_pathLog, " ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- ");
                EscribirArchivoPlano(_pathLog, " ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- ");
                EscribirArchivoPlano(_pathLog, "INICIA PROCESO DE MIGRACION TABLA PERIODO: " + DateTime.Now.ToString());
                EscribirArchivoPlano(_pathLog, " ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- ");
                EscribirArchivoPlano(_pathLog, " ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- ");

                EscribirArchivoPlano(_pathLog, "Inicia adquisicion de datos para MigCACPeriodo");
                datos = _dBCACContext.ResponseSPGetDataMigPeriodoDto
                    .FromSqlRaw<ResponseSPGetDataMigPeriodo>
                    ("EXEC [dbo].[SP_GetDataMigPeriodo]").ToList();
                EscribirArchivoPlano(_pathLog, "Finaliza adquisicion de datos para MigCACPeriodo");

                sqlCommand = new SqlCommand("[dbo].[SP_MigPeriodo]", AudConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                SqlParameter tabla = new("@CacTable", SqlDbType.Structured);

                DataTable ListData = new DataTable();
                ListData.Columns.Add("idPeriodo", typeof(int));
                ListData.Columns.Add("nombre", typeof(string));
                ListData.Columns.Add("fechaCorte", typeof(DateTime));
                ListData.Columns.Add("fechaFinalReporte", typeof(DateTime));
                ListData.Columns.Add("fechaMaximaCorrecciones", typeof(DateTime));
                ListData.Columns.Add("fechaMaximaSoportes", typeof(DateTime));
                ListData.Columns.Add("fechaMaximaConciliaciones", typeof(DateTime));
                ListData.Columns.Add("idCobertura", typeof(Int32));
                ListData.Columns.Add("idPeriodoAnt", typeof(Int32));
                ListData.Columns.Add("FechaMinConsultaAudit", typeof(DateTime));
                ListData.Columns.Add("FechaMaxConsultaAudit", typeof(DateTime));                

                EscribirArchivoPlano(_pathLog, "Inicia creacion de DataTable de MigCACPeriodo");
                foreach (ResponseSPGetDataMigPeriodo item in datos)
                {
                    ListData.Rows.Add(item.idPeriodo, item.nombre, item.fechaCorte,
                        item.fechaFinalReporte, item.fechaMaximaCorrecciones, item.fechaMaximaSoportes,
                        item.fechaMaximaConciliaciones, item.idCobertura, item.idPeriodoAnt,
                        item.FechaMinConsultaAudit, item.FechaMaxConsultaAudit);
                }
                EscribirArchivoPlano(_pathLog, "finaliza creacion de DataTable de MigCACPeriodo");

                tabla.Value = ListData;
                tabla.TypeName = "dbo.MigCACPeriodo";

                sqlCommand.Parameters.Add(tabla);
                AudConnection.Open();
                EscribirArchivoPlano(_pathLog, "Inicia proeceso de insercion de datos de MigCACPeriodo");
                sqlDataReader = sqlCommand.ExecuteReader();
                EscribirArchivoPlano(_pathLog, "Finaliza proeceso de insercion de datos de MigCACPeriodo");

                EscribirArchivoPlano(_pathLog, "\n");
                EscribirArchivoPlano(_pathLog, " ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- ");
                EscribirArchivoPlano(_pathLog, " ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- ");
                EscribirArchivoPlano(_pathLog, "FINALIZA PROCESO DE MIGRACION TABLA PERIODO: " + DateTime.Now.ToString());
                EscribirArchivoPlano(_pathLog, " ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- ");
                EscribirArchivoPlano(_pathLog, " ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- ");

                AudConnection.Close();

            }
            catch (Exception ex)
            {
                AudConnection.Close();
                EscribirArchivoPlano(_pathLog, "Ha ocurrido un error en MigCACPeriodo:" + ex.Message);
            }


        }

        public void MigCACRegla() {
            List<ResponseSPGetDataMigRegla> datos;
            try
            {
                EscribirArchivoPlano(_pathLog, "\n");
                EscribirArchivoPlano(_pathLog, " ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- ");
                EscribirArchivoPlano(_pathLog, " ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- ");
                EscribirArchivoPlano(_pathLog, "INICIA PROCESO DE MIGRACION TABLA REGLA: " + DateTime.Now.ToString());
                EscribirArchivoPlano(_pathLog, " ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- ");
                EscribirArchivoPlano(_pathLog, " ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- ");

                EscribirArchivoPlano(_pathLog, "Inicia adquisicion de datos para MigCACRegla");
                datos = _dBCACContext.ResponseSPGetDataMigReglaDto
                    .FromSqlRaw<ResponseSPGetDataMigRegla>
                    ("EXEC [dbo].[SP_GetDataMigRegla]").ToList();
                EscribirArchivoPlano(_pathLog, "Finaliza adquisicion de datos para MigCACRegla");

                sqlCommand = new SqlCommand("[dbo].[SP_MigRegla]", AudConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                SqlParameter tabla = new("@CacReglaTable", SqlDbType.Structured);

                DataTable ListData = new DataTable();
                ListData.Columns.Add("idRegla", typeof(Int32));
                ListData.Columns.Add("idCobertura", typeof(Int32));
                ListData.Columns.Add("nombre", typeof(string));
                ListData.Columns.Add("idTipoRegla", typeof(Byte));
                ListData.Columns.Add("idTiempoAplicacion", typeof(Byte));
                ListData.Columns.Add("habilitado", typeof(bool));
                ListData.Columns.Add("idError", typeof(string));
                ListData.Columns.Add("idVariable", typeof(Int32));
                ListData.Columns.Add("idTipoEnvioLimbo", typeof(Byte));

                EscribirArchivoPlano(_pathLog, "Inicia creacion de DataTable de MigCACPeriodo");
                foreach (ResponseSPGetDataMigRegla item in datos)
                {
                    ListData.Rows.Add(item.idRegla, item.idCobertura, item.nombre,
                        item.idTipoRegla, item.idTiempoAplicacion, item.habilitado,
                        item.idError, item.idVariable, item.idTipoEnvioLimbo);
                }
                EscribirArchivoPlano(_pathLog, "finaliza creacion de DataTable de MigCACPeriodo");

                tabla.Value = ListData;
                tabla.TypeName = "dbo.MigCACRegla";

                sqlCommand.Parameters.Add(tabla);
                AudConnection.Open();
                EscribirArchivoPlano(_pathLog, "Inicia proeceso de insercion de datos de MigCACPeriodo");
                sqlDataReader = sqlCommand.ExecuteReader();
                EscribirArchivoPlano(_pathLog, "Finaliza proeceso de insercion de datos de MigCACPeriodo");

                EscribirArchivoPlano(_pathLog, "\n");
                EscribirArchivoPlano(_pathLog, " ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- ");
                EscribirArchivoPlano(_pathLog, " ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- ");
                EscribirArchivoPlano(_pathLog, "FINALIZA PROCESO DE MIGRACION TABLA REGLA: " + DateTime.Now.ToString());
                EscribirArchivoPlano(_pathLog, " ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- ");
                EscribirArchivoPlano(_pathLog, " ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- ");

                AudConnection.Close();
            }
            catch (Exception ex)
            {
                AudConnection.Close();
                EscribirArchivoPlano(_pathLog, "Ha ocurrido un error en MigCACRegla:" + ex.Message);
            }

        }

        public void MigCACVariables()
        {
            List<ResponseSPGetDataMigVariable> datos;
            try

            {
                EscribirArchivoPlano(_pathLog, "\n");
                EscribirArchivoPlano(_pathLog, " ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- ");
                EscribirArchivoPlano(_pathLog, " ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- ");
                EscribirArchivoPlano(_pathLog, "VERSION V.1.0.22 : " + DateTime.Now.ToString());
                EscribirArchivoPlano(_pathLog, " ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- ");
                EscribirArchivoPlano(_pathLog, " ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- ");




                EscribirArchivoPlano(_pathLog, "\n");
                EscribirArchivoPlano(_pathLog, " ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- ");
                EscribirArchivoPlano(_pathLog, " ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- ");
                EscribirArchivoPlano(_pathLog, "INICIA PROCESO DE MIGRACION TABLA VARIABLES: " + DateTime.Now.ToString());
                EscribirArchivoPlano(_pathLog, " ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- ");
                EscribirArchivoPlano(_pathLog, " ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- ");

                EscribirArchivoPlano(_pathLog, "Inicia adquisicion de datos para MigCACVariables");
                datos = _dBCACContext.ResponseSPGetDataMigVariableDto
                    .FromSqlRaw<ResponseSPGetDataMigVariable>
                    ("EXEC [dbo].[SP_GetDataMigVariable]").ToList();
                EscribirArchivoPlano(_pathLog, "Finaliza adquisicion de datos para MigCACVariables");

                sqlCommand = new SqlCommand("[dbo].[SP_MigVariables]", AudConnection)
                {
                    CommandType = CommandType.StoredProcedure
                };
                SqlParameter tabla = new("@CacTable", SqlDbType.Structured);

                DataTable ListData = new DataTable();
                ListData.Columns.Add("idVariable", typeof(Int32));
                ListData.Columns.Add("idCobertura", typeof(Int32));
                ListData.Columns.Add("nombre", typeof(string));
                ListData.Columns.Add("nemonico", typeof(string));
                ListData.Columns.Add("descripcion", typeof(string));
                ListData.Columns.Add("idTipoVariable", typeof(string));
                ListData.Columns.Add("longitud", typeof(Int32));
                ListData.Columns.Add("decimales", typeof(Int32));
                ListData.Columns.Add("formato", typeof(string));
                ListData.Columns.Add("tablaReferencial", typeof(String));
                ListData.Columns.Add("campoReferencial", typeof(String));
                ListData.Columns.Add("idErrorTipo", typeof(String));
                ListData.Columns.Add("orden", typeof(int));
                EscribirArchivoPlano(_pathLog, "Inicia creacion de DataTable de MigCACVariables");
                foreach (ResponseSPGetDataMigVariable item in datos)
                {
                    ListData.Rows.Add(item.idVariable, item.idCobertura, item.nombre,
                        item.nemonico, item.descripcion, item.idTipoVariable,
                        item.longitud, item.decimales, item.formato,
                        item.tablaReferencial, item.campoReferencial,
                        item.idErrorTipo, item.orden == null ? 0 : item.orden);
                }
                EscribirArchivoPlano(_pathLog, "finaliza creacion de DataTable de MigCACVariables");

                tabla.Value = ListData;
                tabla.TypeName = "dbo.MigCACVariables";

                sqlCommand.Parameters.Add(tabla);
                AudConnection.Open();
                EscribirArchivoPlano(_pathLog, "Inicia proeceso de insercion de datos de MigCACVariables");
                sqlDataReader = sqlCommand.ExecuteReader();
                EscribirArchivoPlano(_pathLog, "Finaliza proeceso de insercion de datos de MigCACVariables");

                EscribirArchivoPlano(_pathLog, "\n");
                EscribirArchivoPlano(_pathLog, " ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- ");
                EscribirArchivoPlano(_pathLog, " ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- ");
                EscribirArchivoPlano(_pathLog, "FINALIZA PROCESO DE MIGRACION TABLA VARIABLES: " + DateTime.Now.ToString());
                EscribirArchivoPlano(_pathLog, " ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- ");
                EscribirArchivoPlano(_pathLog, " ---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------- ");

                AudConnection.Close();
            }
            catch (Exception ex)
            {
                AudConnection.Close();
                EscribirArchivoPlano(_pathLog, "Ha ocurrido un error en MigCACVariables:" + ex.Message);
            }

        }

        #endregion

        #region CalificacionMasiva

        public async Task<decimal> Calificacionmasiva(int currentProcessId)
        {
            var currentProcess = new CurrentProcessModel();

            try
            {
                //Actualiza progreso del proceso a 0
                currentProcess = _dBAuditCACContext.CurrentProcessModel.Where(x => x.Id == currentProcessId).FirstOrDefault();

                //Consulta parametros del proceso
                var currentProcessParam = _dBAuditCACContext.CurrentProcessParamModel.Where(x => x.CurrentProcessId == currentProcessId).FirstOrDefault();
                currentProcess.Progress = 0;
                _dBAuditCACContext.SaveChanges();
                string result = currentProcess.Result;
                string medicion = "";
                string EstructuraVariable = "";
                int current = 0;
                int max = 0;
                string currentline = "";
                var separed = result.Split(",");
                current = Convert.ToInt32(separed[1]);
                medicion = separed[0];
                int idmedicion = Convert.ToInt32(medicion.Replace("Medicion", ""));
                //int idmedicion = Convert.ToInt32(medicion);

                max = Convert.ToInt32(separed[2]);


                var lines = File.ReadLines(currentProcessParam.Value).ToList();

                var inputPath = currentProcessParam.Value.Split("\\");

                string filename = inputPath[inputPath.Count() - 1];
                string filenameoutput = "resultado_" + filename;
                var outputPath = currentProcessParam.Value.Replace(filename, filenameoutput);

                EscribirArchivoPlano(_pathLog, "Lee  " + lines.Count() + " lineas de arhcivo para calificacion masiva " + DateTime.Now.ToString());


                List<string> resultDetail = new List<string>();

                decimal unidad = 100M / lines.Count();
                decimal progress = 0;


                #region Cantidad campos requeridos

                EscribirArchivoPlano(_pathLog, "SP_Consulta_Cantidad_Campos_Calificacion_Masiva " + DateTime.Now.ToString());

                string script = "EXEC [dbo].[SP_Consulta_Cantidad_Campos_Calificacion_Masiva]";

                List<SqlParameter> parameters = new List<SqlParameter>
                {                 
                };


                var data = await _dBAuditCACContext.LLaveValor.FromSqlRaw<ResponseLlaveValor>(script, parameters.ToArray()).ToListAsync();

                int columnsCountRequired = data.ToList()[0].Id;

                EscribirArchivoPlano(_pathLog, "Consulta campos requeridos " + columnsCountRequired + " " + DateTime.Now.ToString());

                #endregion;

                EscribirArchivoPlano(_pathLog, "separador" + "\t" + ".......");

                //Output file
                EscribirArchivoPlano(_pathLog, "Archivo entrada " + currentProcessParam.Value);



                // ToDo Test 

                currentProcess.Progress = 0;
                _dBAuditCACContext.SaveChanges();

                #region Construye Input data

                //Declare record datatable
                DataTable recordDataTable = new DataTable();
                recordDataTable.Columns.Add("Id", typeof(int));
                recordDataTable.Columns.Add("IdRadicado", typeof(int));
                recordDataTable.Columns.Add("NemonicoVariable", typeof(string));
                recordDataTable.Columns.Add("Calificacion", typeof(string));
                recordDataTable.Columns.Add("Motivo", typeof(string));
                recordDataTable.Columns.Add("Observacion", typeof(string));


                try
                {
                    var separator = this._util.GetAndValidateParameter(parameterList, Enumeration.Parametros.SeparadorArchivos);

                    var tempDetailTableString = "";


                    EscribirArchivoPlano(_pathLog, "Inicia construccion data table calificacion masiva " + DateTime.Now.ToString());

                    //Recorre lineas archivo para construir data table
                    for (int i = 1; i < lines.Count(); i++)
                    {
                        var currentLine = lines[i].Split("\t");

                        if (currentLine.Length == 5)
                        {
                            recordDataTable.Rows.Add(i, currentLine[0], currentLine[1], currentLine[2], currentLine[3], currentLine[4]);
                        }
                        else
                        {

                        }
                        
                    }

                    EscribirArchivoPlano(_pathLog, "Finaliza construccion data table calificacion masiva " + DateTime.Now.ToString());
                }
                catch (Exception ex)
                {

                    throw;
                }

                #endregion

                #region SP Calificacion Masiva

                string sql = "EXEC [dbo].[SP_Realiza_Calificacion_Masiva] " +
                                        "@InputData, " +
                                        "@IdMedicion, " +
                                        "@User, " +
                                        "@CurrentProcessId, " +
                                        "@ResultCurrentProcess, " +
                                        "@Progreso ";


                var parameterDataTable = new SqlParameter("@InputData", SqlDbType.Structured);
                parameterDataTable.Value = recordDataTable;
                parameterDataTable.TypeName = "dbo.DT_Input_Calificacion_Masiva";

                List<SqlParameter> parms = new List<SqlParameter>
                                {
                                    parameterDataTable,
                                    new SqlParameter { ParameterName = "@IdMedicion", Value = idmedicion},
                                    new SqlParameter { ParameterName = "@User", Value = currentProcess.User},
                                    new SqlParameter { ParameterName = "@CurrentProcessId", Value = currentProcess.Id},
                                    new SqlParameter { ParameterName = "@ResultCurrentProcess", Value = currentProcess.Result},
                                    new SqlParameter { ParameterName = "@Progreso", Value = currentProcess.Progress}

                                };

                Util util = new Util();
                var timeout = util.GetAndValidateParameter(parameterList, Enumeration.Parametros.TimeOutCarguePoblacion);



                _dBAuditCACContext.Database.SetCommandTimeout(Convert.ToInt32(timeout));

                var initDate = DateTime.Now;
                EscribirArchivoPlano(_pathLog, "Inicia ejecucion SP_Realiza_Calificacion_Masiva, ProcessId " + currentProcess.Id + " " + DateTime.Now.ToString());

                var dataResult = await _dBAuditCACContext.Database.ExecuteSqlRawAsync(sql, parms.ToArray());

                var FinishDate = DateTime.Now;
                EscribirArchivoPlano(_pathLog, "Finaliza ejecucion SP_Realiza_Calificacion_Masiva, ProcessId " + currentProcess.Id + " " + DateTime.Now.ToString());


                _dBAuditCACContext.SaveChanges();

                #endregion

                #region Construye Respuesta

                #region Consulta y escribe archivo plano


                    List<ResultadoCalificacionMasiva> dataResultado = new List<ResultadoCalificacionMasiva>();

                    try
                    {
                        EscribirArchivoPlano(_pathLog, "SP_Consulta_Resultado_Calificacion_Masiva " + DateTime.Now.ToString());

                        string scriptResultado = "EXEC [dbo].[SP_Consulta_Resultado_Calificacion_Masiva] @CurrentProcessId";

                        List<SqlParameter> parametersResultado = new List<SqlParameter>
                        {
                            new SqlParameter { ParameterName = "@CurrentProcessId", Value = currentProcess.Id},
                        };


                        dataResultado = await _dBAuditCACContext.ResultadoCalificacionMasiva.FromSqlRaw<ResultadoCalificacionMasiva>(scriptResultado, parametersResultado.ToArray()).ToListAsync();


                        EscribirArchivoPlano(_pathLog, "Resultado SP_Consulta_Resultado_Calificacion_Masiva, ProcessId " + currentProcess.Id + " , " + dataResultado.Count() + " registros");

                        string resultLine = "";

                        dataResultado?.All(x =>
                        {
                            resultLine = "";

                            resultLine += x.Tipo + ", ";
                            resultLine += x.IdRadicado == null ? " " : "RADICADO: " + x.IdRadicado.ToString() + ", ";
                            resultLine += x.NemonicoVariable == null ? " " : "VARIABLE: " + x.NemonicoVariable.ToString() + ", ";
                            resultLine += x.Result == null ? " " : " RESULTADO: " + x.Result.ToString();

                            EscribirArchivoPlano(outputPath, resultLine);
                            return true;
                        });

                        EscribirArchivoPlano(_pathLog, "Termina de escribir en archivo resultado calificacion masiva" + DateTime.Now.ToString() + " " + outputPath);

                    }
                    catch (Exception ex)
                    {
                        EscribirArchivoPlano(_pathLog, "Error escribiendo archivo resultado calificacion masiva" + DateTime.Now.ToString() + " " + ex.ToString());
                    }


                #endregion

                if (dataResultado.Count() == 0)
                {
                    EscribirArchivoPlano(outputPath, "Resultado sin regisros, por favor valide el archivo de cargue");
                }

                string resultfile = "OK";

                if (dataResultado.Where(x => x.Tipo.Contains("ERROR")).Any())
                {
                    resultfile = "ERROR";

                }


                currentProcess.Progress = -2;
                currentProcess.Result = currentProcess.Result + "," + resultfile + "," + this._s3.UrlFileS3Uploaded + filenameoutput;
                _dBAuditCACContext.SaveChanges();

                //Save File output S3
                Util _util = new Util();
                var results3 = await _util.LoadFileS3(outputPath, filenameoutput, this._s3);

                //Registra carga
                ControlArchivos_CarguePoblacionModel controlCarga = new ControlArchivos_CarguePoblacionModel()
                {
                    CurrentProcessId = currentProcess.Id,
                    IdMedicion = idmedicion,
                    ArchivoCargado = this._s3.UrlFileS3Uploaded + filename,
                    ArchivoDescargado = this._s3.UrlFileS3Uploaded + filenameoutput,
                    Enable = true,
                    FechaCreacion = DateTime.Now,
                    FecchaActualizacion = DateTime.Now,
                    Usuario = currentProcess.User
                };

                _dBAuditCACContext.ControlArchivos_CarguePoblacion.Add(controlCarga);
                _dBAuditCACContext.SaveChanges();

                EscribirArchivoPlano(_pathLog, "Guarda control en db " + DateTime.Now.ToString());


                //Delete input file
                if (File.Exists(currentProcessParam.Value))
                {
                    try
                    {
                        File.Delete(currentProcessParam.Value);
                    }
                    catch (Exception ex)
                    {
                        EscribirArchivoPlano(_pathLog, "ERROR: " + ex.ToString() + " " + DateTime.Now.ToString());
                    }
                }
                EscribirArchivoPlano(_pathLog, "Elimina archivo origen  " + DateTime.Now.ToString());

                //Delete input file
                if (File.Exists(outputPath))
                {
                    try
                    {
                        File.Delete(outputPath);
                    }
                    catch (Exception ex)
                    {
                        EscribirArchivoPlano(_pathLog, "ERROR: " + ex.ToString() + " " + DateTime.Now.ToString());
                    }
                }
                EscribirArchivoPlano(_pathLog, "Elimina archivo origen  " + DateTime.Now.ToString());

                #endregion



                currentProcess.Progress = -2;
                _dBAuditCACContext.SaveChanges();



                return -2;

            }
            catch (Exception ex)
            {
                EscribirArchivoPlano(_pathLog, "ERROR: " + ex.ToString() + " " + DateTime.Now.ToString());
                currentProcess.Progress = -3;
                _dBAuditCACContext.SaveChanges();
                return -3;
            }
        }

        #endregion



    }
}
