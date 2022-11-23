using ApiAuditCAC.Data.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using ApiAuditCAC.Data.MongoDataAccess;
using ApiAuditCAC.Data;
using ApiAuditCAC.Domain.Dto;
using System.Text.Json;
using System.Dynamic;
using System.IO;
using ApiAuditCAC.Domain.MongoModels;

namespace ApiAuditCAC.Core.MongoCollection
{
    public class MongoCollection : IMongoCollection
    {
        private readonly ILogger<MongoCollection> _logger;
        private readonly IProcedures _procedures;
        private readonly IMongoCollections _mongoCollections;
        public string pathLog;
        public MongoCollection(ILogger<MongoCollection> logger
            ,IProcedures procedures
            ,IMongoCollections mongoCollections ) {
            _procedures = procedures;
            _mongoCollections = mongoCollections;
            _logger = logger;
        }

        public Task<List<Dictionary<String, Object>>> CreateCollection(string pathLog)
        {

            pathLog += DateTime.Now.ToString("yyy:MM:dd").Replace(":","") + ".txt";

            WriteFile(pathLog, "\n");

            WriteFile(pathLog, "=================================================================================================================================");
            WriteFile(pathLog, "Inicia Proceso");
            WriteFile(pathLog, "=================================================================================================================================");


            WriteFile(pathLog, "\nInicia SP_Consulta_Usuarios");
            var userList = _procedures.ConsultaUsuarios();
            WriteFile(pathLog, "Finaliza SP_Consulta_Usuarios medicion");

            List<Dictionary<String, Object>> collection = new();
            List<SpRegistrosAudResponseDto> ids;
            List<int> mediciones;
            String columns;
            List<String> columnsList;
            int currentId = 0;
            try 
            {
                //string medicion = "Auditoria";

                // var enfermedades =  get enfermedades; // call sp
              
                mediciones = _procedures.GetMediciones();

                WriteFile(pathLog, "Consulta " + mediciones.Count() + " mediciones");

                foreach (int idMedicion in mediciones) 
                {

                    List<RegistroAuditoriaMongo> itemList = new List<RegistroAuditoriaMongo>();


                    WriteFile(pathLog, "--------------------------------------------------------------------------------------");
                    WriteFile(pathLog, "Inicia Recorrido medicion: " + idMedicion);
                    WriteFile(pathLog, "--------------------------------------------------------------------------------------");

                    //ids = _procedures.GetIdsAudByMedicion(idMedicion);

                    WriteFile(pathLog, "\nInicia ConsultaRegistrosAuditoriaPorMedicion medicion: " + idMedicion);
                    var recordList = _procedures.ConsultaRegistrosAuditoriaPorMedicion(idMedicion);
                    WriteFile(pathLog, "Finaliza ConsultaRegistrosAuditoriaPorMedicion medicion: " + idMedicion +  ", " + recordList.Count() + " registros consultados");

                    WriteFile(pathLog, "\nInicia ConsultaRegistrosAuditoriaObservacionesPorMedicion medicion: " + idMedicion);
                    var recordTracing = _procedures.ConsultaRegistrosAuditoriaObservacionesPorMedicion(idMedicion);
                    WriteFile(pathLog, "Finaliza ConsultaRegistrosAuditoriaObservacionesPorMedicion medicion: " + idMedicion + ", " + recordTracing.Count() + " registros consultados");


                    WriteFile(pathLog, "\nInicia ConsultaHallazgosPorMedicion medicion: " + idMedicion);
                    var recordHallazgos = _procedures.ConsultaHallazgosPorMedicion(idMedicion);
                    WriteFile(pathLog, "Finaliza ConsultaHallazgosPorMedicion medicion: " + idMedicion + ", " + recordHallazgos.Count() + " registros consultados");

                    WriteFile(pathLog, "\nInicia ConsultaSoportesPorMedicion medicion: " + idMedicion);
                    var recordSoportes = _procedures.ConsultaSoportesPorMedicion(idMedicion);
                    WriteFile(pathLog, "Finaliza ConsultaSoportesPorMedicion medicion: " + idMedicion + ", " + recordSoportes.Count() + " registros consultados");

                    WriteFile(pathLog, "\nInicia ConsultaCalificacionPorMedicion medicion: " + idMedicion);
                    var recordCalificacion = _procedures.ConsultaCalificacionPorMedicion(idMedicion);
                    WriteFile(pathLog, "Finaliza ConsultaCalificacionPorMedicion medicion: " + idMedicion + ", " + recordCalificacion.Count() + " registros consultados");

                    WriteFile(pathLog, "\nInicia ConsultaErroresRegistroAuditarMedicion medicion: " + idMedicion);
                    var recordErrors = _procedures.ConsultaErroresRegistroAuditarMedicion(idMedicion);
                    WriteFile(pathLog, "Finaliza ConsultaErroresRegistroAuditarMedicion medicion: " + idMedicion + ", " + recordErrors.Count() + " registros consultados");


                    WriteFile(pathLog, "\nInicia ConsultaVariablesAsociadaMedicion medicion: " + idMedicion);
                    var recordVariablesAssociated = _procedures.ConsultaVariablesAsociadaMedicion(idMedicion);
                    WriteFile(pathLog, "Finaliza ConsultaVariablesAsociadaMedicion medicion: " + idMedicion + ", " + recordVariablesAssociated.Count() + " registros consultados");

                    WriteFile(pathLog, "\nInicia ConsultaComodinesMedicion medicion: " + idMedicion);
                    var recordComodines = _procedures.ConsultaComodinesMedicion(idMedicion);
                    WriteFile(pathLog, "Finaliza ConsultaComodinesMedicion medicion: " + idMedicion + ", " + recordComodines.Count() + " registros consultados");


                    //group recordlist
                    var groupRecordList = recordList.GroupBy(x => new { x.Id, x.IdRadicado }).Select(y => new RegistrosAuditoriaDto()
                    {
                        Id = y.Key.Id,
                        IdRadicado = y.Key.IdRadicado
                    }).ToList();

                    collection = new();

                    var recordHeader = new RegistrosAuditoriaDto();
                    var recordDetail = new List<RegistrosAuditoriaDto>();

                    string collectionName = "";
                    foreach (RegistrosAuditoriaDto record in groupRecordList)
                    {
                        RegistroAuditoriaMongo currentRecord = new RegistroAuditoriaMongo();

                        int variable = 0;
                        WriteFile(pathLog, "Inicia IdRadicado: " + record.IdRadicado);
                        try
                        {
                            //Sett mongo Ids
                            currentRecord._id = record.Id;
                            currentRecord.IdRadicado = record.IdRadicado;
                            currentRecord.Body = new Dictionary<string, object>();

                            //Get header
                            recordHeader = new RegistrosAuditoriaDto();
                            recordHeader = recordList.Where(x => x.Id == record.Id).FirstOrDefault();
                            collectionName = recordHeader.CoberturaNemonico;

                            //Get Detail
                            recordDetail =  new List<RegistrosAuditoriaDto>();
                            recordDetail = recordList.Where(x => x.Id == record.Id).ToList();


                            currentId = record.Id;
                            Dictionary<String, Object> item = new();


                            //columns = _procedures.GetColums(id.Id);

                            //columnsList = columns.Replace("[", "").Replace("]", "").Split(",").ToList();
                            //Dictionary<String, Object> datoReportado = _procedures.GetData(id.Id, "DatoReportado", String.Empty);
                            //Dictionary<String, Object> motivoVariable = _procedures.GetData(id.Id, "MotivoVariable", "rad.");
                            //Dictionary<String, Object> datoDcNcNd = _procedures.GetData(id.Id, "Dato_DC_NC_ND", String.Empty);
                            //Dictionary<String, Object> idVariable = _procedures.GetData(id.Id, "idVariable", String.Empty);
                            //Dictionary<String, Object> tipoVariableItem = _procedures.GetData(id.Id, "TipoVariableItem", String.Empty);

                            item.Add(nameof(recordHeader.IdRadicado), recordHeader.IdRadicado);
                            item.Add("IdRegistrosAuditoria", recordHeader.Id);

                            #region Header Info

                            item.Add("Id", recordHeader.Id);
                           // item.Add("IdRadicado", recordHeader.IdRadicado);
                            item.Add("IdMedicion", recordHeader.IdMedicion);
                            item.Add("IdPeriodo", recordHeader.IdPeriodo);
                            item.Add("IdAuditor", recordHeader.IdAuditor);
                            item.Add("PrimerNombre", recordHeader.PrimerNombre);
                            item.Add("SegundoNombre", recordHeader.SegundoNombre);
                            item.Add("PrimerApellido", recordHeader.PrimerApellido);
                            item.Add("SegundoApellido", recordHeader.SegundoApellido);
                            item.Add("Sexo", recordHeader.Sexo);
                            item.Add("TipoIdentificacion", recordHeader.TipoIdentificacion);
                            item.Add("Identificacion", recordHeader.Identificacion);
                            item.Add("FechaNacimiento", recordHeader.FechaNacimiento);
                            item.Add("FechaCreacion", recordHeader.FechaCreacion);
                            item.Add("FechaAuditoria", recordHeader.FechaAuditoria);
                            item.Add("FechaMinConsultaAudit", recordHeader.FechaMinConsultaAudit);
                            item.Add("FechaMaxConsultaAudit", recordHeader.FechaMaxConsultaAudit);
                            item.Add("FechaAsignacion", recordHeader.FechaAsignacion);
                            item.Add("Activo", recordHeader.Activo);
                            item.Add("Reverse", recordHeader.Reverse);
                            item.Add("DisplayOrder", recordHeader.DisplayOrder);
                            item.Add("Ara", recordHeader.Ara);
                            item.Add("Eps", recordHeader.Eps);
                            item.Add("FechaReverso", recordHeader.FechaReverso);
                            item.Add("AraAtendido", recordHeader.AraAtendido);
                            item.Add("EpsAtendido", recordHeader.EpsAtendido);
                            item.Add("Revisar", recordHeader.Revisar);
                            item.Add("Extemporaneo", recordHeader.Extemporaneo);
                            item.Add("EstadoRegistroAuditoria", recordHeader.EstadoRegistroAuditoria);
                            item.Add("DescripcionEstadoRegistroAuditoria", recordHeader.DescripcionEstadoRegistroAuditoria);
                            item.Add("LevantarGlosa", recordHeader.LevantarGlosa);
                            item.Add("MantenerCalificacion", recordHeader.MantenerCalificacion);
                            item.Add("ComiteExperto", recordHeader.ComiteExperto);
                            item.Add("ComiteAdministrativo", recordHeader.ComiteAdministrativo);
                            item.Add("AccionLider", recordHeader.AccionLider);
                            item.Add("AccionAuditor", recordHeader.AccionAuditor);
                            item.Add("EncuestaRegistroAuditoria", recordHeader.EncuestaRegistroAuditoria);
                            item.Add("CreatedByRegistroAuditoria", recordHeader.CreatedByRegistroAuditoria);
                            item.Add("CreatedDateRegistroAuditoria", recordHeader.CreatedDateRegistroAuditoria);
                            item.Add("ModifyByRegistroAuditoria", recordHeader.ModifyByRegistroAuditoria);
                            item.Add("ModifyDateRegistroAuditoria", recordHeader.ModifyDateRegistroAuditoria);
                            item.Add("IdEPS", recordHeader.IdEPS);
                            item.Add("StatusRegistroAuditoria", recordHeader.StatusRegistroAuditoria);
                            item.Add("EstadoMedicion", recordHeader.EstadoMedicion);
                            item.Add("EstadoMedicionNombre", recordHeader.EstadoMedicionNombre);
                            item.Add("MedicionNombre", recordHeader.MedicionNombre);
                            item.Add("MedicionDescripcion", recordHeader.MedicionIdCobertura);
                            item.Add("FechaFinAuditoria", recordHeader.FechaFinAuditoria);
                            item.Add("CoberturaNombre", recordHeader.CoberturaNombre);
                            item.Add("CoberturaNemonico", recordHeader.CoberturaNemonico);
                            item.Add("CoberturaLegislacion", recordHeader.CoberturaLegislacion);
                            item.Add("CoberturaDefinicion", recordHeader.CoberturaDefinicion);
                            item.Add("RegimenEPS_Id", recordHeader.RegimenEPS_Id);
                            item.Add("RegimenEPS_Nombre", recordHeader.RegimenEPS_Nombre);
                            item.Add("RenglonEPS_Id", recordHeader.RenglonEPS_Id);
                            item.Add("RenglonEPS_Nombre", recordHeader.RenglonEPS_Nombre);
                           



                            //User Created and Modify
                            item.Add("UserCreated", Getuser(recordHeader.CreatedByRegistroAuditoria, userList));
                            item.Add("UserModify", Getuser(recordHeader.ModifyByRegistroAuditoria, userList));

                            #endregion


                            #region Detail

                            Dictionary<String, Object> element;

                            int count = 1;
                            foreach (RegistrosAuditoriaDto detail in recordDetail)
                            {
                                
                                variable = detail.VariableId;
                                element = new();
                                element.Add("IdRegistroAuditoriaDetalle", detail.IdRegistroAuditoriaDetalle);
                                element.Add("idVariable", detail.idVariable);
                                element.Add("VariableId", detail.VariableId);
                                element.Add("CreatedByRegistroAuditoriaDetalle", detail.CreatedByRegistroAuditoriaDetalle);
                                element.Add("CreatedDateRegistroAuditoriaDetalle", detail.CreatedDateRegistroAuditoriaDetalle);
                                element.Add("ModifyByRegistroAuditoriaDetalle", detail.ModifyByRegistroAuditoriaDetalle);
                                element.Add("ModifyDateRegistroAuditoriaDetalle", detail.ModifyDateRegistroAuditoriaDetalle);
                                element.Add("DatoReportado", detail.DatoReportado);
                                element.Add("MotivoVariable", detail.MotivoVariable);
                                element.Add("Dato_DC_NC_ND", detail.Dato_DC_NC_ND);
                                element.Add("Dato_DC_NC_NDNombre", detail.Dato_DC_NC_NDNombre);
                                element.Add("AraRegistroAuditoriaDetalle", detail.AraRegistroAuditoriaDetalle);
                                element.Add("StatusRegistroAuditoriaDetalle", detail.StatusRegistroAuditoriaDetalle);
                                element.Add("Contexto", detail.Contexto);
                                element.Add("EsGlosa", detail.EsGlosa);
                                element.Add("EsVisible", detail.EsVisible);
                                element.Add("EsCalificable", detail.EsCalificable);
                                element.Add("EnableDC", detail.EnableDC);
                                element.Add("EnableNC", detail.EnableNC);
                                element.Add("EnableND", detail.EnableND);
                                element.Add("CalificacionXDefecto", detail.CalificacionXDefecto);
                                element.Add("CalificacionXDefectoNombre", detail.CalificacionXDefectoNombre);
                                element.Add("SubGrupoId", detail.SubGrupoId);
                                element.Add("SubGrupoNombre", detail.SubGrupoNombre);
                                element.Add("EncuestaVariablexMedicion", detail.EncuestaVariablexMedicion);
                                element.Add("OrdenVariablexMedicion", detail.OrdenVariablexMedicion);
                                element.Add("StatusVariablexMedicion", detail.StatusVariablexMedicion);
                                element.Add("TipoCampo", detail.TipoCampo);
                                element.Add("TipoCampoNombre", detail.TipoCampoNombre);
                                element.Add("Promedio", detail.Promedio);
                                element.Add("ValidarEntreRangos", detail.ValidarEntreRangos);
                                element.Add("Desde", detail.Desde);
                                element.Add("Hasta", detail.Hasta);
                                element.Add("Condicionada", detail.Condicionada);
                                element.Add("ValorConstante", detail.ValorConstante);
                                element.Add("Lista", detail.Lista);
                                element.Add("ActivoVariableXMedicion", detail.ActivoVariableXMedicion);
                                element.Add("CreatedByVariableXMedicion", detail.CreatedByVariableXMedicion);
                                element.Add("CreationDateVariableXMedicion", detail.CreationDateVariableXMedicion);
                                element.Add("ModifyByVariableXMedicion", detail.ModifyByVariableXMedicion);
                                element.Add("ModificationDateVariableXMedicion", detail.ModificationDateVariableXMedicion);
                                element.Add("Hallazgos", detail.Hallazgos);
                                element.Add("Activa", detail.Activa);
                                element.Add("OrdenVariable", detail.OrdenVariable);
                                element.Add("idCobertura", detail.idCobertura);
                                element.Add("nombreVariable", detail.nombreVariable);
                                element.Add("nemonicoVariable", detail.nemonicoVariable);
                                element.Add("descripcionVariable", detail.descripcionVariable);
                                element.Add("idTipoVariable", detail.idTipoVariable);
                                element.Add("longitud", detail.longitud);
                                element.Add("decimales", detail.decimales);
                                element.Add("formato", detail.formato);
                                element.Add("tablaReferencial", detail.tablaReferencial);
                                element.Add("campoReferencial", detail.campoReferencial);
                                element.Add("Bot", detail.Bot);
                                element.Add("TipoVariableItem", detail.TipoVariableItem);
                                element.Add("ItemTipoVariableNombre", detail.ItemTipoVariableNombre);
                                element.Add("Alerta", detail.Alerta);
                                element.Add("AlertaDescripcion", detail.AlertaDescripcion);
                                element.Add("StatusVariable", detail.StatusVariable);
                                element.Add("idErrorTipo", detail.idErrorTipo);
                                element.Add("AplicaCalculadora", detail.Calculadora);
                                element.Add("TipoCalculadora", detail.TipoCalculadora);
                                element.Add("AplicaVariablesAsociados", detail.VariablesAsociados);
                                element.Add("AplicaComodines", detail.Comodines);

                                //User Created and Modify
                                element.Add("UserCreated",  Getuser(detail.CreatedByRegistroAuditoriaDetalle, userList));
                                element.Add("UserModify", Getuser(detail.ModifyByRegistroAuditoriaDetalle, userList));

                                //Build Object
                                var hallazgos = recordHallazgos.Where(x => x.RegistrosAuditoriaDetalleId == detail.IdRegistroAuditoriaDetalle).ToList();
                                element.Add("Hallagos_Respuestas", hallazgos);

                                //Build Califications
                                var calificaciones = recordCalificacion.Where(x => x.RegistrosAuditoriaDetalleId == detail.IdRegistroAuditoriaDetalle).ToList();
                                element.Add("Calificaciones", calificaciones);

                                //Buils Errors
                                var errores = recordErrors.Where(x => x.RegistrosAuditoriaDetalleId == detail.IdRegistroAuditoriaDetalle).ToList();
                                element.Add("Errores", errores);


                                //Build Asociated variables
                                var varAsociated = recordVariablesAssociated.Where(x => x.VariableId == detail.VariableId).ToList();
                                element.Add("ValriablesAsociadas", varAsociated);

                                //Build Comodin variables
                                var comidines = recordComodines.Where(x => x.VariableId == detail.VariableId).ToList();
                                element.Add("Comodines", comidines);

                                item.Add(count + "_" + detail.nemonicoVariable + "_Variable_" + detail.idVariable.ToString(), element);
                                count++;
                            }

                            #endregion



                            var observaciones = new List<RegistrosAuditoriaSeguimientoDto>();
                            observaciones = recordTracing.Where(x => x.RegistroAuditoriaId == recordHeader.Id).ToList();
                            item.Add("Observaciones", observaciones);


                            var soportes = new List<SoportesDto>();
                            soportes = recordSoportes.Where(x => x.IdRegistrosAuditoria == recordHeader.Id).ToList();
                            item.Add("Soportes", soportes);


                            currentRecord.Body = item; 
                            itemList.Add(currentRecord);


                            //if (idMongo is null)
                            //{
                            //    _mongoCollections.Insert(item, recordHeader.CoberturaNemonico); // Todo: collecion dinamica
                            //   // _mongoCollections.Insert(item, medicion);
                            //}
                            //else
                            //{
                            //    _mongoCollections.Remove(item, recordHeader.CoberturaNemonico);  // todo: collecion dinamica
                            //    _mongoCollections.Insert(item, recordHeader.CoberturaNemonico); // todo: collecion dinamica
                            //    //_mongoCollections.Remove(idMongo, medicion);  // Todo: collecion dinamica
                            //    //_mongoCollections.Insert(item, medicion);
                            //}
                        }
                        catch (Exception ex)
                        {
                            WriteFile(pathLog, "ERROR Procesando Id: " + recordHeader.Id);
                            WriteFile(pathLog, ex.ToString());
                        }



                        WriteFile(pathLog, "Finaliza IdRadicado: " + record.IdRadicado);


                    }
                    //var test = _mongoCollections.Insert(collection, "M" + idMedicion);


                    if (itemList.Count() > 0)
                    {
                        try
                        {
                            WriteFile(pathLog, "Inicia Upsert Masivo " + collectionName);
                            _mongoCollections.AddOrUpdateMassive(itemList, collectionName);
                            WriteFile(pathLog, "Finaliza Upsert Masivo " + collectionName);
                        }
                        catch (Exception ex)
                        {

                            WriteFile(pathLog, ex.ToString());
                            _logger.LogError(ex.Message);
                        }
                    }
                    
                   
                }
            }
            catch (Exception e) {
                WriteFile(pathLog, e.ToString());
                _logger.LogError(e.Message);
            }


            WriteFile(pathLog, "=================================================================================================================================");
            WriteFile(pathLog, "Finaliza Proceso");
            WriteFile(pathLog, "=================================================================================================================================");
            return Task.FromResult(collection);
        }


        public List<UsuarioDto> Getuser(string userId, List<UsuarioDto> userList)
        {
            var oReturn = new List<UsuarioDto>();

            if (userList.Where(x => x.UserId == userId).Any())
            {
                oReturn = userList.Where(x => x.UserId == userId).ToList();
            }

            return oReturn;
        }

        public void WriteFile(string path, string line)
        {
            try
            {
                using (StreamWriter outputFile = new StreamWriter(path, true))
                {
                    outputFile.WriteLine(line + " " + DateTime.Now.ToString());
                }
            }
            catch (Exception ex)
            {

                //throw;
            }
        }
    }
}
