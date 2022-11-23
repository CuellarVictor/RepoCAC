using AuditCAC.Dal.Data;
using AuditCAC.Domain.Dto;
using AuditCAC.Domain.Dto.BolsasMedicion;
using AuditCAC.Domain.Dto.RegistroAuditoria;
using AuditCAC.Domain.Entities;
using AuditCAC.Domain.Helpers;
using AuditCAC.MainCore.Module.Interface;
using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Threading.Tasks;

namespace AuditCAC.MainCore.Module.Helpers
{
    public class GenerateExcel : IGenerateExcel
    {
        private readonly DBAuditCACContext _dBAuditCACContext;
        private readonly IConfiguration _config;
        private Util _util = new Util();

        public GenerateExcel(DBAuditCACContext dBAuditCACContext, IConfiguration config)
        {
            this._dBAuditCACContext = dBAuditCACContext;
            _config = config;
        }


        public async Task<string> BuildReportFile(List<ResponseRegistrosAuditoriaModelDto> data, string ruta)
        {
            try
            {
                var filename = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".csv";
                var pathComplete = ruta + "\\" + filename;

                if (data.Count != 0)
                {
                    

                    //Write Head
                    string head = "";
                    head += "ID" + ";";
                    head += "Estado" + ";";
                    head += "E Madre" + ";";
                    head += "Medicion" + ";";
                    head += "Modificacion" + ";";
                    head += "Asignacion" + ";";
                    head += "Codigo" + ";";
                    head += "Auditor" + ";";
                    head += "Entidad" + ";";
                   // head += "IPS" + ";";
                    head += "Fecha Apertura" + ";";
                    head += "Hora Apertura" + ";";
                    head += "Fecha Cierre" + ";";
                    head += "Hora Cierre" + ";";

                    WriteFile(pathComplete, head);

                    string line = "";

                    foreach (var item in data)
                    {
                        line = "";

                        //Build line
                        line += item.IdRadicado  == null ? " " + ";" : item.IdRadicado + ";";
                        line += item.EstadoCodigo == null ? " " + ";" : item.EstadoCodigo + ";";
                        line += item.EnfermedadMadre == null ? " " + ";" : item.EnfermedadMadre + ";";
                        line += item.NombreMedicion == null ? " " + ";" : item.NombreMedicion + ";";
                        line += item.ModifyDate == null ? " " + ";" : Convert.ToDateTime(item.ModifyDate).ToString("yyyy-MM-dd") + ";";
                        line += item.FechaAsignacion == null ? "" + ";" : Convert.ToDateTime(item.FechaAsignacion).ToString("yyyy-MM-dd") + ";";
                        line += item.CodigoUsuario == null ? " " + ";" : item.CodigoUsuario + ";";
                        line += item.CreatedBy == null ? " " + ";" : item.CreatedBy + ";";
                        line += item.Entidad == null ? " " + ";" : item.Entidad + ";";
                       // line += item.NombreIPS == null ?  " " + ";" : item.NombreIPS == "" ? " " : item.NombreIPS + ";";
                        line += item.FechaApertura == null ? " " + ";" : item.FechaApertura + ";";
                        line += item.HoraApertura == null ? " " + ";" : item.HoraApertura + ";";
                        line += item.FechaCierre == null ? " " + ";" : item.FechaCierre + ";";
                        line += item.HoraCierre == null ? " " + ";" : item.HoraCierre + ";";                       

                        WriteFile(pathComplete, _util.EliminarAcentos(line));                        
                    }
                    


                    return filename;
                }
                else
                {
                    return await Task.FromResult("");
                }
            }
            catch (Exception e)
            {
                return await Task.FromResult(e.Message);
            }
        }

        #region Write

        /// <summary>
        /// Escribir archivo plano
        /// </summary>
        /// <param name="path">ruta</param>
        /// <param name="line">texto</param>
        public void WriteFile(string path, string line)
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

                throw;
            }
        }

        #endregion


        #region Reportes Medicion

        //Funcion principal
        public async Task<string> BuildMedicionesReportFile(List<ResponseRegistrosAuditoriaXBolsaMedicionDto> data, string ruta, int enumReport)
        {
            try
            {
                if (enumReport == (int)Enumeration.ReporteMedicion.ReasignacionBolsaPlantilla) 
                {
                    return await ReporteReasignacionBolsaPlantilla(data, ruta);
                }
                else if (enumReport == (int)Enumeration.ReporteMedicion.ReasignacionBolsaData)
                {
                    return await ReporteReasignacionBolsaData(data, ruta);
                }
                else if (enumReport == (int)Enumeration.ReporteMedicion.ReasignacionTotalData)
                {
                    return await ReporteReasignacionTotalData(data, ruta); //ReasignacionTotalData
                }
                else 
                {
                    return "";
                } 
            }
            catch (Exception)
            {

                throw;
            }
        }

        //Funciones

        public async Task<string> ReporteReasignacionBolsaPlantilla(List<ResponseRegistrosAuditoriaXBolsaMedicionDto> data, string ruta)
        {
            try
            {
                var filename = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".csv";
                var pathComplete = ruta + "\\" + filename;

                if (data.Count != 0)
                {
                    //Write Head
                    string head = "";
                    head += "ID Registro" + ";";
                    head += "Estado registro" + ";";
                    head += "Entidad" + ";";
                    head += "Asignacion" + ";";
                    head += "Codigo Auditor" + ";";                     
                    head += "Bolsa Medicion" + ";";
                    WriteFile(pathComplete, head);

                    string line = "";

                    foreach (var item in data)
                    {
                        line = "";

                        //Build line
                        line += item.IdRadicado == null ? " " : item.IdRadicado + ";";
                        line += item.EstadoCodigo == null ? " " : item.EstadoCodigo + ";";
                        line += item.Data_NombreEPS == null ? " " : item.Data_NombreEPS + ";";
                        line += item.FechaAsignacion == null ? "" : Convert.ToDateTime(item.FechaAsignacion).ToString("yyyy-MM-dd") + ";";
                        line += item.CodigoUsuario == null ? " " : item.CodigoUsuario + ";";
                        line += item.NombreMedicion == null ? " " : item.NombreMedicion + ";";
                        WriteFile(pathComplete, _util.EliminarAcentos(line));
                    }

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
                            return await Task.FromResult(ex.Message);
                            //_logger.LogInformation(_pathLog, "ERROR: " + ex.ToString() + " " + DateTime.Now.ToString());
                        }
                    }

                    return plainTextBytes;
                    //Regex.Replace(palabaConTildes.Normalize(NormalizationForm.FormD), @"[^a-zA-z0-9 ]+", "");
                }
                else
                {
                    return await Task.FromResult("");
                }
            }
            catch (Exception e)
            {
                return await Task.FromResult(e.Message);
            }
        }

        public async Task<string> ReporteReasignacionBolsaData(List<ResponseRegistrosAuditoriaXBolsaMedicionDto> data, string ruta)
        {
            try
            {
                var filename = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".csv";
                var pathComplete = ruta + "\\" + filename;

                if (data.Count != 0)
                {
                    //Write Head
                    string head = "";
                    head += "ID Registro" + ";";
                    head += "Estado registro" + ";";
                    head += "Entidad" + ";";
                    head += "Asignacion" + ";";
                    head += "Enfermedad Madre" + ";";
                    head += "Bolsa Medicion" + ";";
                    WriteFile(pathComplete, head);

                    string line = "";

                    foreach (var item in data)
                    {
                        line = "";

                        //Build line
                        line += item.IdRadicado == null ? " " : item.IdRadicado + ";";
                        line += item.EstadoCodigo == null ? " " : item.EstadoCodigo + ";";
                        line += item.Data_NombreEPS == null ? " " : item.Data_NombreEPS + ";";
                        line += item.FechaAsignacion == null ? "" : Convert.ToDateTime(item.FechaAsignacion).ToString("yyyy-MM-dd") + ";";
                        line += item.NombreEnfermedadMadre == null ? " " : item.NombreEnfermedadMadre + ";";
                        line += item.NombreMedicion == null ? " " : item.NombreMedicion + ";";
                        WriteFile(pathComplete, _util.EliminarAcentos(line));
                    }

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
                            return await Task.FromResult(ex.Message);
                            //_logger.LogInformation(_pathLog, "ERROR: " + ex.ToString() + " " + DateTime.Now.ToString());
                        }
                    }

                    return plainTextBytes;
                }
                else
                {
                    return await Task.FromResult("");
                }
            }
            catch (Exception e)
            {
                return await Task.FromResult(e.Message);
            }
        }

        public async Task<string> ReporteReasignacionTotalData(List<ResponseRegistrosAuditoriaXBolsaMedicionDto> data, string ruta)
        {
            try
            {
                var filename = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".csv";
                var pathComplete = ruta + "\\" + filename;

                if (data.Count != 0)
                {
                    //Write Head
                    string head = "";
                    head += "Codigo Auditor original" + ";";
                    head += "Id registro" + ";"; //IdRadicado
                    head += "Estado del registro" + ";"; //La sigla del estado
                    head += "Entidad" + ";";
                    head += "Fecha asignacion" + ";";
                    head += "Codigo Auditor nuevo" + ";";
                    WriteFile(pathComplete, head);

                    string line = "";

                    foreach (var item in data)
                    {
                        line = "";

                        //Build line
                        line += item.CodigoUsuario == null ? " " : item.CodigoUsuario + ";";
                        line += item.IdRadicado == null ? " " : item.IdRadicado + ";";
                        line += item.EstadoCodigo == null ? " " : item.EstadoCodigo + ";";
                        line += item.Data_NombreEPS == null ? " " : item.Data_NombreEPS + ";";
                        line += item.FechaAsignacion == null ? "" : Convert.ToDateTime(item.FechaAsignacion).ToString("yyyy-MM-dd") + ";";
                        line += "" + ";";
                        WriteFile(pathComplete, _util.EliminarAcentos(line));
                    }

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
                            return await Task.FromResult(ex.Message);
                            //_logger.LogInformation(_pathLog, "ERROR: " + ex.ToString() + " " + DateTime.Now.ToString());
                        }
                    }

                    return plainTextBytes;
                }
                else
                {
                    return await Task.FromResult("");
                }
            }
            catch (Exception e)
            {
                return await Task.FromResult(e.Message);
            }
        }
        #endregion

        #region Mover Bolsas de medicion.
        public async Task<string> ReporteMoverRegistrosAuditoriaBolsaMedicionData(List<ResponseMoverTodosRegistrosAuditoriaBolsaMedicionDto> dataResult, string ruta, bool camposError = false)
        {
            try
            {
                //Escribimos archivo
                var filename = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".csv";
                var pathComplete = ruta + "\\" + filename;

                if (dataResult.Count != 0)
                {
                    //Write Head
                    string head = "";
                    head += "ID Bolsa origen" + ";";
                    head += "Bolsa origen" + ";";
                    head += "ID Bolsa destino" + ";";
                    head += "Id de registro" + ";";
                    head += "Estado del registro" + ";";
                    head += "Entidad" + ";";
                    head += "Auditor" + ";";
                    head += "Fecha de asignacion" + ";";
                    // --
                    if(camposError ==  true)
                    {
                        head += "Estado de Ejecucion" + ";";
                        head += "Mensaje de Ejecucion" + ";";
                    }                    
                    // --
                    _util.EscribirArchivoPlano(pathComplete, head);

                    string line = "";

                    foreach (var item in dataResult)
                    {
                        line = "";

                        //Build line
                        line += item.IdBolsaOrigen == null ? " " : item.IdBolsaOrigen + ";";
                        line += item.BolsaOrigen == null ? " " : item.BolsaOrigen + ";";
                        line += item.IdBolsaDestino == null ? " " : item.IdBolsaDestino + ";";
                        line += item.IdRadicado == null ? " " : item.IdRadicado + ";";
                        line += item.EstadoRegistro == null ? "" : item.EstadoRegistro + ";";
                        line += item.Entidad == null ? " " : item.Entidad + ";";
                        line += item.CodigoAuditor == null ? " " : item.CodigoAuditor + ";";
                        line += item.FechaAsignacion == null ? "" : Convert.ToDateTime(item.FechaAsignacion).ToString("yyyy-MM-dd") + ";";
                        // --
                        if(camposError == true)
                        {
                            line += item.EstadoEjecucion == null ? " " : item.EstadoEjecucion + ";";
                            line += item.MensajeEjecucion == null ? " " : item.MensajeEjecucion + ";";
                        }                        
                        // --
                        line += "" + ";";
                        _util.EscribirArchivoPlano(pathComplete, _util.EliminarAcentos(line));
                    }

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
                            return await Task.FromResult(ex.Message);
                            //_logger.LogInformation(pathComplete, "ERROR: " + ex.ToString() + " " + DateTime.Now.ToString());
                        }
                    }

                    //return plainTextBytes;
                    return plainTextBytes;
                }
                else
                {
                    //plainTextBytes = "";
                    return await Task.FromResult("");
                }
            }
            catch (Exception e)
            {
                return await Task.FromResult(e.Message);
            }
        }
        #endregion

        #region Banco de Informacion.
        public async Task<string> ReporteCargarBancoInformacionPlantillaData(List<ResponseCargarBancoInformacionPlantillaDto> dataResult, string ruta)
        {
            try
            {
                //Escribimos archivo
                var filename = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".csv";
                var pathComplete = ruta + "\\" + filename;

                //if (dataResult.Count != 0)
                //{
                    //Write Head	
                    string head = "";
                    //head += "Id" + ";";
                    head += "Nombre" + ";";
                    head += "Tipo" + ";";
                    //head += "Nombre Tipo" + ";";
                    head += "Codigo" + ";";
                    head += "IdCobertura";

                    _util.EscribirArchivoPlano(pathComplete, head);

                    string line = "";

                    foreach (var item in dataResult)
                    {
                        line = "";

                        //Build line
                        //line += item.Id + ";"; // line += item.Id == null ? " " : item.Id + ";";
                        line += item.Nombre == null ? " " : item.Nombre + ";";
                        line += item.Tipo == null ? " " : item.Tipo + ";";
                        line += item.Codigo == null ? " " : item.Codigo + ";";
                        line += item.IdCobertura == item.IdCobertura;
                        _util.EscribirArchivoPlano(pathComplete, _util.EliminarAcentos(line));
                    }

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
                            return await Task.FromResult(ex.Message);
                            //_logger.LogInformation(pathComplete, "ERROR: " + ex.ToString() + " " + DateTime.Now.ToString());
                        }
                    }

                    //return plainTextBytes;
                    return plainTextBytes;
                //}
                //else
                //{
                //    //plainTextBytes = "";
                //    return await Task.FromResult("");
                //}
            }
            catch (Exception e)
            {
                return await Task.FromResult(e.Message);
            }
        }
        #endregion

        #region EliminarRegistrosAuditoriaPlantillaData
        public async Task<string> EliminarRegistrosAuditoriaPlantillaData(List<ResponseEliminarRegistrosAuditoriaPlantillaDto> dataResult, string ruta)
        {
            try
            {
                //Escribimos archivo
                var filename = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".csv";
                var pathComplete = ruta + "\\" + filename;

                if (dataResult.Count != 0)
                {
                    //Write Head
                    string head = "";
                    head += "Id Radicado" + ";";
                    _util.EscribirArchivoPlano(pathComplete, head);

                    string line = "";

                    foreach (var item in dataResult)
                    {
                        line = "";

                        //Build line
                        line += item.IdRadicado == null ? " " : item.IdRadicado + ";";
                        line += "" + ";";
                        _util.EscribirArchivoPlano(pathComplete, _util.EliminarAcentos(line));
                    }

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
                            return await Task.FromResult(ex.Message);
                            //_logger.LogInformation(pathComplete, "ERROR: " + ex.ToString() + " " + DateTime.Now.ToString());
                        }
                    }

                    //return plainTextBytes;
                    return plainTextBytes;
                }
                else
                {
                    //plainTextBytes = "";
                    return await Task.FromResult("");
                }
            }
            catch (Exception e)
            {
                return await Task.FromResult(e.Message);
            }
        }
        #endregion

        #region Log Accion

        public async Task<string> BuildReportFileLogAccion(List<RegistrosAuditoriaLogModel> data, string ruta)
        {
            try
            {
                var filename = DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss") + ".csv";
                var pathComplete = ruta + "\\" + filename;

                if (data.Count != 0)
                {


                    //Write Head
                    string head = "";
                    head += "Id Registro" + ";";
                    head += "Observacion" + ";";
                    head += "Codigo" + ";";
                    head += "Nombre" + ";";
                    head += "Fecha" + ";";
                    head += "Hora" + ";";
                    head += "Id Medicion" + ";";
                    head += "Nombre Medicion" + ";";

                    WriteFile(pathComplete, head);

                    string line = "";

                    foreach (var item in data)
                    {
                        line = "";

                        //Build line
                        line += item.IdRadicado == null ? " " + ";" : item.IdRadicado + ";";
                        line += item.Observacion == null ? " " + ";" : item.Observacion + ";";
                        line += item.Codigo == null ? " " + ";" : item.Codigo + ";";
                        line += item.Nombres == null ? " " + " " : item.Nombres + " ";
                        line += item.Apellidos == null ? " " + ";" : item.Apellidos + ";";
                        line += item.Fecha == null ? " " + ";" : item.Fecha + ";";
                        line += item.Hora == null ? " " + ";" : item.Hora + ";";
                        line += item.MedicionId == null ? " " + ";" : item.MedicionId + ";";
                        line += item.NombreMedicion == null ? " " + ";" : item.NombreMedicion + ";";

                        WriteFile(pathComplete, _util.EliminarAcentos(line));
                    }



                    return filename;
                }
                else
                {
                    return await Task.FromResult("");
                }
            }
            catch (Exception e)
            {
                return await Task.FromResult(e.Message);
            }
        }

        #endregion
    }
}
