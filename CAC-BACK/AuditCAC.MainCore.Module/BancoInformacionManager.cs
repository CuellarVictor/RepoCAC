using AuditCAC.Dal.Data;
using AuditCAC.Domain.Dto;
using AuditCAC.Domain.Dto.CarguePoblacion;
using AuditCAC.Domain.Entities;
using AuditCAC.Domain.Helpers;
using AuditCAC.MainCore.Module.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.MainCore.Module
{
    public class BancoInformacionManager : IBancoInformacionRepository<BancoInformacionModel>
    {

        private readonly DBAuditCACContext _dBAuditCACContext;
        private readonly ILogger<BancoInformacionModel> _logger;
        private Util _util = new Util();
        IConfigurationRoot configuration;
        private readonly IGenerateExcel _generate;

        //Constructor
        public BancoInformacionManager(DBAuditCACContext dBAuditCACContext, ILogger<BancoInformacionModel> logger, IGenerateExcel generate)
        {
            this._dBAuditCACContext = dBAuditCACContext;
            _logger = logger;
            configuration = new ConfigurationBuilder()
             .SetBasePath(Directory.GetCurrentDirectory())
             .AddJsonFile("appsettings.json")
             .Build();
            _generate = generate;
        }

        //Metodos.
        public async Task<IEnumerable<BancoInformacionModel>> GetAll()
        {
            try
            {
                return await _dBAuditCACContext.BancoInformacionModel.Where(x => x.Status == true).ToListAsync();
                //return await this.dBAuditCACContext.BancoInformacionModel.ToListAsync();
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        public async Task<BancoInformacionModel> Get(int id)
        {
            try
            {
                return await _dBAuditCACContext.BancoInformacionModel.Where(x => x.Status == true).FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        public async Task Add(BancoInformacionModel entity)
        {
            try
            {
                _dBAuditCACContext.BancoInformacionModel.Add(entity);
                await _dBAuditCACContext.SaveChangesAsync();
                //return NoContent;

                //Llamamos SP para Insertar Logs de actividad.                
                string sql = "EXEC [SP_Insertar_Process_Log] @ProcessId, @User, @Result, @Observation";
                List<SqlParameter> parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@ProcessId", Value = 26},
                    new SqlParameter { ParameterName = "@User", Value = entity.CreatedBy},
                    new SqlParameter { ParameterName = "@Result", Value = "OK"},
                    new SqlParameter { ParameterName = "@Observation", Value = "Configuraciones - Códigos Administrativos: Nuevo"}
                };

                var respuesta = _dBAuditCACContext.Database.ExecuteSqlRaw(sql, parms.ToArray());

            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        public async Task Update(BancoInformacionModel dbEntity, BancoInformacionModel entity)
        {
            try
            {
                //dbEntity.Id = entity.Id;
                dbEntity.Nombre = entity.Nombre;
                dbEntity.Tipo = entity.Tipo;
                dbEntity.Codigo = entity.Codigo;
                dbEntity.CreatedBy = entity.CreatedBy;
                dbEntity.CreatedDate = entity.CreatedDate;
                dbEntity.ModifyBy = entity.ModifyBy;
                dbEntity.ModifyDate = entity.ModifyDate;
                dbEntity.IdCobertura = entity.IdCobertura;

                await _dBAuditCACContext.SaveChangesAsync();

                //Llamamos SP para Insertar Logs de actividad.                
                string sql = "EXEC [SP_Insertar_Process_Log] @ProcessId, @User, @Result, @Observation";
                List<SqlParameter> parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@ProcessId", Value = 27},
                    new SqlParameter { ParameterName = "@User", Value = entity.CreatedBy},
                    new SqlParameter { ParameterName = "@Result", Value = "OK"},
                    new SqlParameter { ParameterName = "@Observation", Value = "Configuraciones - Códigos Administrativos: Editar"}
                };

                var respuesta = _dBAuditCACContext.Database.ExecuteSqlRaw(sql, parms.ToArray());
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        public async Task Delete(BancoInformacionModel entity)
        {
            try
            {
                //_dBAuditCACContext.BancoInformacionModel.Remove(entity);
                //await _dBAuditCACContext.SaveChangesAsync();

                entity.Status = false;

                await _dBAuditCACContext.SaveChangesAsync();

                //Llamamos SP para Insertar Logs de actividad.                
                string sql = "EXEC [SP_Insertar_Process_Log] @ProcessId, @User, @Result, @Observation";
                List<SqlParameter> parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@ProcessId", Value = 28},
                    new SqlParameter { ParameterName = "@User", Value = entity.CreatedBy},
                    new SqlParameter { ParameterName = "@Result", Value = "OK"},
                    new SqlParameter { ParameterName = "@Observation", Value = "Configuraciones - Códigos Administrativos: Eliminar"}
                };

                var respuesta = _dBAuditCACContext.Database.ExecuteSqlRaw(sql, parms.ToArray());
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        //Metodo para llamar procedure GetBancoInformacionByPalabraClave - Para la consulta de los registros filtrados por una palabra clave (campso codigo y nombre).
        public async Task<List<ResponseBancoInformacionDto>> GetBancoInformacionByPalabraClave(InputsBancoInformacionDto inputsDto)
        {
            try
            {
                string sql = "EXEC [dbo].[GetBancoInformacionByPalabraClave] @PalabraClave"; //@PageNumber, @MaxRows

                List<SqlParameter> parms = new List<SqlParameter>
                {
                    //new SqlParameter { ParameterName = "@PageNumber", Value = inputsDto.PageNumber},
                    //new SqlParameter { ParameterName = "@MaxRows", Value = inputsDto.MaxRows},
                    //                    
                    new SqlParameter { ParameterName = "@PalabraClave", Value = inputsDto.PalabraClave},

                };

                var Data = await _dBAuditCACContext.ResponseBancoInformacionDto.FromSqlRaw<ResponseBancoInformacionDto>(sql, parms.ToArray()).ToListAsync();
                return Data;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Para consultar todos los registros de Banco de Informacion creados segun los filtrados ingresados.
        /// </summary>
        /// <param name="inputsDto">Entrada de datos</param>
        /// <returns>Modelo de datos</returns>
        public async Task<Tuple<List<ResponseBancoInformacionFiltradoDto>, int, int, int>> GetBancoInformacionFiltrado(InputsBancoInformacionFiltradoDto inputsDto)
        {
            try
            {
                string sql = "EXEC SP_Consulta_BancoInformacion_Filtrado @PageNumber, @MaxRows, @Id, @Nombre, @Tipo, @IdCobertura, @Codigo, @PalabraClave";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters    
                    new SqlParameter { ParameterName = "@PageNumber", Value = inputsDto.PageNumber},
                    new SqlParameter { ParameterName = "@MaxRows", Value = inputsDto.MaxRows},
                    //
                    new SqlParameter { ParameterName = "@Id", Value = inputsDto.Id},
                    new SqlParameter { ParameterName = "@Nombre", Value = inputsDto.Nombre},
                    new SqlParameter { ParameterName = "@Tipo", Value = inputsDto.Tipo},
                    new SqlParameter { ParameterName = "@IdCobertura", Value = inputsDto.IdCobertura}, 
                    new SqlParameter { ParameterName = "@Codigo", Value = inputsDto.Codigo},
                    new SqlParameter { ParameterName = "@PalabraClave", Value = inputsDto.PalabraClave},
                };

                var Data = await _dBAuditCACContext.ResponseBancoInformacionFiltradoDto.FromSqlRaw<ResponseBancoInformacionFiltradoDto>(sql, parms.ToArray()).ToListAsync();

                //Obtenemos No Registros maximos a consultar y los agregamos al Header del Request (API).
                var Queryable = _dBAuditCACContext.BancoInformacionModel.AsQueryable();
                var NoRegistrosTotales = Queryable.Count();

                var NoRegistrosTotalesFiltrado = Data.Count() > 0 ? Data.FirstOrDefault().NoRegistrosTotales : 0;

                var TotalPages = NoRegistrosTotalesFiltrado / inputsDto.MaxRows;
                //Data.Select(c => { c.NoRegistrosTotales = ""; return c; }).ToList();

                return Tuple.Create(Data, NoRegistrosTotales, NoRegistrosTotalesFiltrado, TotalPages);
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Para consultar info usada en cargar datos de Banco de Informacion.
        /// </summary>
        /// <param name="filePath">Ruta del archivo a cargar</param>
        /// <param name="input">Modelo Cargue</param>
        /// <returns>Modelo Current Process</returns>        
        public async Task<ResponseReasignacionesBolsaDetalladaDto> CargarBancoInformacionPlantilla(string filePath)
        {
            try
            {
                //_logger.LogInformation("Inicia Proceso de Reasignacion de Bolsa Detallado");

                //Declaramos variables usadas para generar archivo
                var dataResult = new List<ResponseCargarBancoInformacionPlantillaDto>();
                List<string> resultDetail = new List<string>(); //Listado usado para capturar resultados del proceso.                
                var plainTextBytes = "";

                //Llamamos SP.
                string sql = "EXEC [dbo].[SP_Cargar_BancoInformacion_Plantilla]";

                List<SqlParameter> parms = new List<SqlParameter>{ };

                //Ejecutamos SP y capturamos resultados del proceso.
                dataResult = await _dBAuditCACContext.ResponseCargarBancoInformacionPlantillaDto.FromSqlRaw<ResponseCargarBancoInformacionPlantillaDto>(sql, parms.ToArray()).ToListAsync();
                _dBAuditCACContext.SaveChanges();
               
                plainTextBytes = await _generate.ReporteCargarBancoInformacionPlantillaData(dataResult, filePath);

                //Retornamos valores.
                ResponseReasignacionesBolsaDetalladaDto respuesta = new ResponseReasignacionesBolsaDetalladaDto()
                {
                    status = true,
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
        /// Para cargar datos de Banco de Informacion.
        /// </summary>
        /// <param name="filePath">Ruta del archivo a cargar</param>
        /// <param name="input">Modelo Cargue</param>
        /// <returns>Modelo Current Process</returns>        
        public async Task<ResponseReasignacionesBolsaDetalladaDto> CargarBancoInformacion(string filePath, InputCargarBancoInformacionDto input, S3Model _s3)
        {
            try
            {
                _logger.LogInformation("Inicia Proceso de Cargue de datos Banco de Informacion");

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

                //Declaramos listado para capturar resultados del proceso.
                var dataResult = new List<ResponseCargarBancoInformacionDto>();

                //Recorremos todas las lineas del archivo cargado.
                for (int i = 1; i < lines.Count(); i++)
                {                    

                    //Obtenemos la linea actual del archivo cargado.
                    currentline = lines[i];

                    if (currentline != "" && currentline.Length > 0)
                    {
                        try
                        {
                            //Llamamos SP.
                            string sql = "EXEC [dbo].[SP_Cargar_BancoInformacion] @userId, @header, @line";

                            List<SqlParameter> parms = new List<SqlParameter>
                            {
                                new SqlParameter { ParameterName = "@userId", Value = input.UserId},
                                new SqlParameter { ParameterName = "@header", Value = headerFile},
                                new SqlParameter { ParameterName = "@line", Value = currentline},
                            };

                            //Ejecutamos SP y capturamos resultados del proceso.
                            dataResult = await _dBAuditCACContext.ResponseCargarBancoInformacionDto.FromSqlRaw<ResponseCargarBancoInformacionDto>(sql, parms.ToArray()).ToListAsync();
                            _dBAuditCACContext.SaveChanges();

                            //Almacenamos resultados del proceso
                            _util.EscribirArchivoPlano(outputPath, dataResult.FirstOrDefault().Result);
                            resultDetail.Add(dataResult.FirstOrDefault().Result);
                        }
                        catch (Exception ex)
                        {
                            _logger.LogInformation(ex.ToString());
                            _util.EscribirArchivoPlano(outputPath, "ERROR: " + currentline);
                            ;
                        }   
                        
                    }

                }

                // NOTA: ESTO DEBE BUSCAR SI HAY UN ERROR, EN ESE CASO.. PONER EL STATUS EN ERROR.

                //Validamos si ha ocurrido un error en el proceso.
                //bool status = true;
                //if (resultDetail.Where(x => x.Contains("ERROR")).Any())
                //{
                //    status = false;
                //}

                // --

                //Validamos si ha ocurrido un error en el proceso.
                bool status = true;
                if (dataResult != null)
                {
                    //if (resultDetail.Where(x => x.Contains("ERROR")).Any())
                    foreach (var item in dataResult)
                    {
                        if (item.Result.Contains("ERROR") ==  true)
                        {
                            status = false;
                        }
                    }
                }

                // --

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

                if(status == true)
                {                
                    //Llamamos SP para Insertar Logs de actividad.                
                    string sql = "EXEC [SP_Insertar_Process_Log] @ProcessId, @User, @Result, @Observation";
                    List<SqlParameter> parms = new List<SqlParameter>
                    {
                        new SqlParameter { ParameterName = "@ProcessId", Value = 26},
                        new SqlParameter { ParameterName = "@User", Value = input.UserId},
                        new SqlParameter { ParameterName = "@Result", Value = "OK"},
                        new SqlParameter { ParameterName = "@Observation", Value = "Configuraciones - Códigos Administrativos: Nuevo"}
                    };

                    var respuestaLog = _dBAuditCACContext.Database.ExecuteSqlRaw(sql, parms.ToArray());
                }

                return respuesta;
                //return "OK";

            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }
    }
}
