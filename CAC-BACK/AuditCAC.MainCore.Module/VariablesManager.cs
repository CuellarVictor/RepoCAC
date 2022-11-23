using AuditCAC.Dal.Data;
using AuditCAC.Domain.Dto;
using AuditCAC.Domain.Dto.Variable;
using AuditCAC.Domain.Entities;
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
//using FastMember;

namespace AuditCAC.MainCore.Module
{
    public class VariablesManager : IVariablesRepository<VariablesModel>
    {
        private readonly DBAuditCACContext _dBAuditCACContext;
        //private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType); //Esta sin uso, deberia ser removido.
        private readonly ILogger<VariablesModel> _logger; //Para guardar Logs.

        //Constructor
        public VariablesManager(DBAuditCACContext dBAuditCACContext, ILogger<VariablesModel> logger)
        {
            this._dBAuditCACContext = dBAuditCACContext;
            _logger = logger;
        }

        //Metodos.
        public async Task<IEnumerable<VariablesModel>> GetAll()
        {
            try
            {
                return await _dBAuditCACContext.VariablesModel.Where(x => x.Status == true).ToListAsync();
                //return await this.dBAuditCACContext.VariablesModel.ToListAsync();
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());                
                throw;
            }
        }

        public async Task<VariablesModel> Get(int id)
        {
            try
            {
                return await _dBAuditCACContext.VariablesModel.Where(x => x.Status == true).FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        public async Task Add(VariablesModel entity)
        {
            try
            {
                _dBAuditCACContext.VariablesModel.Add(entity);
                await _dBAuditCACContext.SaveChangesAsync();
                //return NoContent;
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        public async Task Update(VariablesModel dbEntity, VariablesModel entity)
        {
            try
            {
                //dbEntity.Id = entity.Id;
                dbEntity.Activa = entity.Activa;
                dbEntity.Orden = entity.Orden;
                dbEntity.idVariable = entity.idVariable;
                dbEntity.idCobertura = entity.idCobertura;
                dbEntity.nombre = entity.nombre;
                dbEntity.nemonico = entity.nemonico;
                dbEntity.descripcion = entity.descripcion;
                dbEntity.idTipoVariable = entity.idTipoVariable;
                dbEntity.longitud = entity.longitud;
                dbEntity.decimales = entity.decimales;
                dbEntity.formato = entity.formato;
                dbEntity.tablaReferencial = entity.tablaReferencial;
                dbEntity.campoReferencial = entity.campoReferencial;

                dbEntity.ModifyBy = entity.ModifyBy;
                dbEntity.ModifyDate = entity.ModifyDate;      
                dbEntity.MotivoVariable = entity.MotivoVariable;
                dbEntity.Bot = entity.Bot;
                dbEntity.TipoVariableItem = entity.TipoVariableItem;
                dbEntity.EstructuraVariable = entity.EstructuraVariable;

                await _dBAuditCACContext.SaveChangesAsync();

            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        public async Task Delete(VariablesModel entity)
        {
            try
            {
                //_dBAuditCACContext.VariablesModel.Remove(entity);
                //await _dBAuditCACContext.SaveChangesAsync();

                //Eliminar en: Variables, VariableXMedicion, VariablesXItems, ReglasVariable

                ////Consultamos datos asociados a la variable.
                //var DataVariableXMedicion = await _dBAuditCACContext.VariableXMedicionModel.Where(x => x.VariableId == entity.Id).FirstOrDefaultAsync();
                //var DataVariablesXItems = await _dBAuditCACContext.VariablesXItemsModel.Where(x => x.VariablesId == entity.Id).FirstOrDefaultAsync();                
                //var DataReglasVariable = await _dBAuditCACContext.ReglaVariable.Where(x => x.IdVariable == entity.Id).FirstOrDefaultAsync();

                ////Eliminamos (cambiamos estado) a datos asociados.
                //DataVariableXMedicion.Status = false;
                //await _dBAuditCACContext.SaveChangesAsync();

                ////Eliminamos (cambiamos estado) a variable.
                //entity.Status = false;
                //await _dBAuditCACContext.SaveChangesAsync();



                //DataVariablesXItems.Status = false;
                //await _dBAuditCACContext.SaveChangesAsync();

                //DataReglasVariable.Enable = false;
                //await _dBAuditCACContext.SaveChangesAsync();

                string sql = "EXEC [dbo].[EliminarVariable] @VariableId";

                List<SqlParameter> parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@VariableId", Value = entity.Id},
                };

                var Data = await _dBAuditCACContext.Database.ExecuteSqlRawAsync(sql, parms.ToArray());
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        //Metodo para llamar procedure - Insertar Variables masivas.
        public async Task AtarListadoVariablesMedicion(List<InputsCreateVariablesDto> entity)
        {
            try
            {
                //Creamos Query para ejecutar procedure.
                string sql = "EXEC [dbo].[AtarListadoVariablesMedicion] @Listado";

                //Declaramos parametro.
                var Parameter = new SqlParameter("@Listado", SqlDbType.Structured);

                //Convertimos List recibida a DataTable
                //DataTable ListData = new DataTable();
                //using (var reader = ObjectReader.Create(entity))
                //{
                //    List.Load(reader);
                //}

                //Convertimos List recibida a DataTable
                DataTable ListData = new DataTable();
                //ListData.Columns.Add("Id", typeof(int));
                ListData.Columns.Add("Activa", typeof(bool));
                ListData.Columns.Add("Orden", typeof(int));
                ListData.Columns.Add("idVariable", typeof(int));
                ListData.Columns.Add("idCobertura", typeof(int));
                ListData.Columns.Add("nombre", typeof(string));
                ListData.Columns.Add("nemonico", typeof(string));
                ListData.Columns.Add("descripcion", typeof(string));
                ListData.Columns.Add("idTipoVariable", typeof(string));
                ListData.Columns.Add("longitud", typeof(int));
                ListData.Columns.Add("decimales", typeof(int));
                ListData.Columns.Add("formato", typeof(string));
                ListData.Columns.Add("tablaReferencial", typeof(string));
                ListData.Columns.Add("campoReferencial", typeof(string));                
                ListData.Columns.Add("CreatedBy", typeof(string));                
                ListData.Columns.Add("ModifyBy", typeof(string));
                ListData.Columns.Add("MotivoVariable", typeof(string));
                ListData.Columns.Add("Bot", typeof(bool));
                ListData.Columns.Add("TipoVariableItem", typeof(int));
                ListData.Columns.Add("EstructuraVariable", typeof(int));
                //
                ListData.Columns.Add("MedicionId", typeof(int));
                ListData.Columns.Add("EsGlosa", typeof(bool));
                ListData.Columns.Add("EsVisible", typeof(bool));
                ListData.Columns.Add("EsCalificable", typeof(bool));
                ListData.Columns.Add("Activo", typeof(bool));
                ListData.Columns.Add("EnableDC", typeof(bool));
                ListData.Columns.Add("EnableNC", typeof(bool));
                ListData.Columns.Add("EnableND", typeof(bool));
                ListData.Columns.Add("CalificacionXDefecto", typeof(bool));
                ListData.Columns.Add("SubGrupoId", typeof(int));
                ListData.Columns.Add("Encuesta", typeof(bool));
                ListData.Columns.Add("VxM_Orden", typeof(int));

                //
                foreach (var item in entity)
                {
                    //ListData.Rows.Add(item.NaturalezaVariableId, item.MedicionId, item.Activa, item.Orden, item.idVariable, item.idCobertura, item.nombre, item.nemonico, item.descripcion, item.idTipoVariable, item.longitud, item.decimales, item.formato, item.idErrorTipo, item.tablaReferencial, item.campoReferencial, item.idErrorReferencial, item.idTipoVariableAlterno, item.formatoAlterno, item.permiteVacio, item.idErrorPermiteVacio, item.identificadorRegistro, item.clavePrimaria, item.idTipoAnalisisEpidemiologico, item.sistema, item.exportable, item.enmascarado, item.CreatedBy, item.CreatedDate, item.ModifyBy, item.ModifyDate);
                    ListData.Rows.Add(item.Activa, item.Orden, item.idVariable, item.idCobertura, item.nombre, item.nemonico, item.descripcion, item.idTipoVariable, item.longitud, item.decimales, item.formato, item.tablaReferencial, item.campoReferencial, item.CreatedBy, item.ModifyBy, 
                        item.MotivoVariable, item.Bot, item.TipoVariableItem, item.EstructuraVariable,
                        item.MedicionId, item.EsGlosa, item.EsVisible, item.EsCalificable, item.Activo, item.EnableDC, item.EnableNC, item.EnableND, item.CalificacionXDefecto, item.SubGrupoId, item.Encuesta, item.VxM_Orden);
                }
                //ListData.Rows.Add(item.Id, item.NaturalezaVariableId, item.MedicionId, item.Activa, item.Orden, item.idVariable, item.idCobertura, item.nombre, item.nemonico, item.descripcion, item.idTipoVariable, item.longitud, item.decimales, item.formato, item.idErrorTipo, item.tablaReferencial, item.campoReferencial, item.idErrorReferencial, item.idTipoVariableAlterno, item.formatoAlterno, item.permiteVacio, item.idErrorPermiteVacio, item.identificadorRegistro, item.clavePrimaria, item.idTipoAnalisisEpidemiologico, item.sistema, item.exportable, item.enmascarado, item.CreatedBy, item.CreatedDate, item.ModifyBy, item.ModifyDate);

                //Agregamos valores a Parametros.
                Parameter.Value = ListData;
                Parameter.TypeName = "dbo.DT_Variables";

                //Ejecutamos procedimiento.
                var Data = await _dBAuditCACContext.Database.ExecuteSqlRawAsync(sql, Parameter);
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        //Metodo para llamar procedure - Duplicar variables.
        public async Task<bool> DuplicarVariables(InputsDuplicarVariablesDto entity)
        {
            try
            {
                string sql = "EXEC [dbo].[DuplicarvariablesMedicion] @MedicionIdOrigional, @MedicionIdNuevo, @UserCreatedBy, @Descripcion";

                List<SqlParameter> parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@MedicionIdOrigional", Value = entity.MedicionIdOrigional},
                    new SqlParameter { ParameterName = "@MedicionIdNuevo", Value = entity.MedicionIdNuevo},
                    new SqlParameter { ParameterName = "@UserCreatedBy", Value = entity.UserCreatedBy},
                    new SqlParameter { ParameterName = "@Descripcion", Value = entity.Descripcion},

                };

                var Data = await _dBAuditCACContext.Database.ExecuteSqlRawAsync(sql, parms.ToArray());
                return true;
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

        //Metodo para llamar procedure - Duplicar mediciones.
        public async Task<bool> DuplicarMedicion(InputsDuplicarMedicionDto entity)
        {
            try
            {
                string sql = "EXEC [dbo].[DuplicarMedicion] @MedicionId, @UserCreatedBy";

                List<SqlParameter> parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@MedicionId", Value = entity.MedicionId},
                    new SqlParameter { ParameterName = "@UserCreatedBy", Value = entity.UserCreatedBy},
                };

                var Data = await _dBAuditCACContext.Database.ExecuteSqlRawAsync(sql, parms.ToArray());
                return true;
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        //Metodo para llamar procedure - GetVariablesFiltrado.
        public async Task<Tuple<List<ResponseVariablesDetails>, int, int, int>> GetVariablesFiltrado(InputsVariablesDto inputsDto)
        {
            try
            {
                //Creamos Query para ejecutar procedure.
                string sql = "EXEC [dbo].[GetVariablesFiltrado] @PageNumber, @MaxRows, @Id, @Variable, @Activa, @Orden, @idVariable, @idCobertura, @nombre, @nemonico, @descripcion, @idTipoVariable, @longitud, @decimales, @formato, @tablaReferencial, @campoReferencial, " +
                             "@CreatedBy, @CreatedDate, @ModifyBy, @ModifyDate, @MotivoVariable,  " +
                             "@Bot, @TipoVariableItem, @EstructuraVariable, @MedicionId, @EsGlosa, @EsVisible, @EsCalificable, @Activo, @EnableDC, @EnableNC, @EnableND, @CalificacionXDefecto, @SubGrupoId, @Encuesta, @VxM_Orden, @Alerta, @AlertaDescripcion, @calificacionIPSItem, @IdRegla, @Concepto";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters
                    new SqlParameter { ParameterName = "@PageNumber", Value = inputsDto.PageNumber},
                    new SqlParameter { ParameterName = "@MaxRows", Value = inputsDto.MaxRows},
                    // 
                    new SqlParameter { ParameterName = "@Id", Value = inputsDto.Id},
                    new SqlParameter { ParameterName = "@Variable", Value = inputsDto.Variable},
                    new SqlParameter { ParameterName = "@Activa", Value = inputsDto.Activa},
                    new SqlParameter { ParameterName = "@Orden", Value = inputsDto.Orden},
                    new SqlParameter { ParameterName = "@idVariable", Value = inputsDto.idVariable},
                    new SqlParameter { ParameterName = "@idCobertura", Value = inputsDto.idCobertura},
                    new SqlParameter { ParameterName = "@nombre", Value = inputsDto.nombre},
                    new SqlParameter { ParameterName = "@nemonico", Value = inputsDto.nemonico},
                    new SqlParameter { ParameterName = "@descripcion", Value = inputsDto.descripcion},
                    new SqlParameter { ParameterName = "@idTipoVariable", Value = inputsDto.idTipoVariable},
                    new SqlParameter { ParameterName = "@longitud", Value = inputsDto.longitud},
                    new SqlParameter { ParameterName = "@decimales", Value = inputsDto.decimales},
                    new SqlParameter { ParameterName = "@formato", Value = inputsDto.formato},
                    new SqlParameter { ParameterName = "@tablaReferencial", Value = inputsDto.tablaReferencial},
                    new SqlParameter { ParameterName = "@campoReferencial", Value = inputsDto.campoReferencial},
                    new SqlParameter { ParameterName = "@CreatedBy", Value = inputsDto.CreatedBy},
                    new SqlParameter { ParameterName = "@CreatedDate", Value = inputsDto.CreatedDate},
                    new SqlParameter { ParameterName = "@ModifyBy", Value = inputsDto.ModifyBy},
                    new SqlParameter { ParameterName = "@ModifyDate", Value = inputsDto.ModifyDate},
                    new SqlParameter { ParameterName = "@MotivoVariable", Value = inputsDto.MotivoVariable},
                    new SqlParameter { ParameterName = "@Bot", Value = inputsDto.Bot},
                    new SqlParameter { ParameterName = "@TipoVariableItem",  Value = String.Join(", ", inputsDto.TipoVariableItem.ToArray())},
                    new SqlParameter { ParameterName = "@EstructuraVariable", Value = inputsDto.EstructuraVariable},
                    //
                    new SqlParameter { ParameterName = "@MedicionId", Value = inputsDto.MedicionId},
                    new SqlParameter { ParameterName = "@EsGlosa", Value = inputsDto.EsGlosa},
                    new SqlParameter { ParameterName = "@EsVisible", Value = inputsDto.EsVisible},
                    new SqlParameter { ParameterName = "@EsCalificable", Value = inputsDto.EsCalificable},
                    new SqlParameter { ParameterName = "@Activo", Value = inputsDto.Activo},
                    new SqlParameter { ParameterName = "@EnableDC", Value = inputsDto.EnableDC},
                    new SqlParameter { ParameterName = "@EnableNC", Value = inputsDto.EnableNC},
                    new SqlParameter { ParameterName = "@EnableND", Value = inputsDto.EnableND},
                    new SqlParameter { ParameterName = "@CalificacionXDefecto", Value = inputsDto.CalificacionXDefecto},
                    new SqlParameter { ParameterName = "@SubGrupoId", Value = String.Join(", ", inputsDto.SubGrupoId.ToArray())},
                    new SqlParameter { ParameterName = "@Encuesta", Value = inputsDto.Encuesta},
                    new SqlParameter { ParameterName = "@VxM_Orden", Value = inputsDto.VxM_Orden},

                    new SqlParameter { ParameterName = "@Alerta", Value = inputsDto.Alerta},
                    new SqlParameter { ParameterName = "@AlertaDescripcion", Value = inputsDto.AlertaDescripcion},
                    new SqlParameter { ParameterName = "@calificacionIPSItem", Value = inputsDto.calificacionIPSItem},
                    new SqlParameter { ParameterName = "@IdRegla", Value = inputsDto.IdRegla},
                    new SqlParameter { ParameterName = "@Concepto", Value = inputsDto.Concepto}
                };
                var Data = await _dBAuditCACContext.ResponseVariablesDto.FromSqlRaw<ResponseVariablesDto>(sql, parms.ToArray()).ToListAsync();

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

                #region Consulta Items Calificaicon

                string sqlitems = "EXEC [dbo].[SP_Consulta_Items_Calificacion] @MedicionId";

                List<SqlParameter> parmsItems = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@MedicionId", Value = Convert.ToInt32(inputsDto.MedicionId) }

                };

                var DataItems = await _dBAuditCACContext.ResponseItemsCalificacion.FromSqlRaw<ResponseItemsCalificacion>(sqlitems, parmsItems.ToArray()).ToListAsync();


                #endregion

                //Obtenemos No Registros maximos a consultar y los agregamos al Header del Request (API).
                var Queryable = _dBAuditCACContext.VariablesModel.AsQueryable();
                var NoRegistrosTotales = Queryable.Count();

                var NoRegistrosTotalesFiltrado = DataQuery.FirstOrDefault().NoRegistrosTotalesFiltrado;

                var TotalPages = NoRegistrosTotalesFiltrado / inputsDto.MaxRows;

                Data.Select(c => { c.QueryNoRegistrosTotales = ""; return c; }).ToList();

                //Agrupamos consulta y retornamos valores de "calificacionIPSItem" Agrupados.
                var Result = new List<ResponseVariablesDetails>();

                var filtros = Data
                    .GroupBy(x => new { x.Id, x.VariableId })
                    .Select(y => new ResponseVariablesDetails()
                    {
                        Id = y.FirstOrDefault().Id,
                        Variable = y.FirstOrDefault().Variable,
                        Activa = y.FirstOrDefault().Activa,
                        Orden = y.FirstOrDefault().Orden,
                        idVariable = y.FirstOrDefault().idVariable,
                        idCobertura = y.FirstOrDefault().idCobertura,
                        nombre = y.FirstOrDefault().nombre,
                        nemonico = y.FirstOrDefault().nemonico,
                        descripcion = y.FirstOrDefault().descripcion,
                        idTipoVariable = y.FirstOrDefault().idTipoVariable,
                        longitud = y.FirstOrDefault().longitud,
                        decimales = y.FirstOrDefault().decimales,
                        formato = y.FirstOrDefault().formato == null ? "" : y.FirstOrDefault().formato,
                        tablaReferencial = y.FirstOrDefault().tablaReferencial,
                        campoReferencial = y.FirstOrDefault().campoReferencial == null ? "" : y.FirstOrDefault().campoReferencial,
                        CreatedBy = y.FirstOrDefault().CreatedBy,
                        CreatedDate = y.FirstOrDefault().CreatedDate,
                        ModifyBy = y.FirstOrDefault().ModifyBy,
                        ModifyDate = y.FirstOrDefault().ModifyDate,
                        MotivoVariable = y.FirstOrDefault().MotivoVariable,
                        Bot = y.FirstOrDefault().Bot,
                        TipoVariableItem = y.FirstOrDefault().TipoVariableItem,
                        EstructuraVariable = y.FirstOrDefault().EstructuraVariable,
                        VariableId = y.FirstOrDefault().VariableId,
                        MedicionId = y.FirstOrDefault().MedicionId,
                        EsGlosa = y.FirstOrDefault().EsGlosa,
                        EsVisible = y.FirstOrDefault().EsVisible,
                        EsCalificable = y.FirstOrDefault().EsCalificable,
                        Hallazgos = y.FirstOrDefault().Hallazgos,
                        Activo = y.FirstOrDefault().Activo,
                        EnableDC = y.FirstOrDefault().EnableDC,
                        EnableNC = y.FirstOrDefault().EnableNC,
                        EnableND = y.FirstOrDefault().EnableND,
                        CalificacionXDefecto = y.FirstOrDefault().CalificacionXDefecto,
                        SubGrupoId = y.FirstOrDefault().SubGrupoId,
                        SubGrupoNombre = y.FirstOrDefault().SubGrupoNombre,
                        Encuesta = y.FirstOrDefault().Encuesta,
                        VxM_Orden = y.FirstOrDefault().VxM_Orden,
                        Alerta = y.FirstOrDefault().Alerta == null ? false : y.FirstOrDefault().Alerta,
                        AlertaDescripcion = y.FirstOrDefault().AlertaDescripcion == null ? "" : y.FirstOrDefault().AlertaDescripcion,
                        calificacionIPSItem = new List<int>(),// new List<ResponseVariablesDtoDetalles>(), //y.FirstOrDefault().calificacionIPSItem,
                        //calificacionIPSItemNombre = new List<ResponseVariablesDtoDetalles>(), //y.FirstOrDefault().calificacionIPSItemNombre,
                        IdRegla = y.FirstOrDefault().IdRegla == null ? "" : y.FirstOrDefault().IdRegla,
                        NombreRegla = y.FirstOrDefault().NombreRegla == null ? "" : y.FirstOrDefault().NombreRegla,
                        Concepto = y.FirstOrDefault().Concepto == null ? "" : y.FirstOrDefault().Concepto,
                        TipoCampo = y.FirstOrDefault().TipoCampo,
                        Promedio = y.FirstOrDefault().Promedio,
                        ValidarEntreRangos = y.FirstOrDefault().ValidarEntreRangos,
                        Desde = y.FirstOrDefault().Desde,
                        Hasta = y.FirstOrDefault().Hasta,
                        Condicionada = y.FirstOrDefault().Condicionada,
                        ValorConstante = y.FirstOrDefault().ValorConstante,
                        Lista = y.FirstOrDefault().Lista,
                        VariableCondicional = new List<VariableCondicionalDto>(),
                        Calculadora = y.FirstOrDefault().Calculadora,
                        TipoCalculadora = y.FirstOrDefault().TipoCalculadora
                    });

                Result.AddRange(filtros);
                Result?.All(x =>
                {
                    var row = new ResponseVariablesDetails();
                    row.Id = x.Id;

                    row.calificacionIPSItem = new List<int>();// new List<ResponseVariablesDtoDetalles>();


                    DataItems.Where(d => d.VariableId.ToString() == x.VariableId)?.All(d =>
                    {
                        row.calificacionIPSItem.Add(d.ItemId);
                        return true;
                    });

                    //Data.Where(d => d.Id == x.Id && x.calificacionIPSItem != null)?.All(z =>
                    //{
                    //    row.calificacionIPSItem.Add(Convert.ToInt32(z.calificacionIPSItem));
                    //    //row.calificacionIPSItem.Add(new ResponseVariablesDtoDetalles()
                    //    //{
                    //    //    Id = z.calificacionIPSItem == null ? "" : z.calificacionIPSItem,
                    //    //    Valor = z.calificacionIPSItemNombre == null ? "" : z.calificacionIPSItemNombre,
                    //    //});
                    //    return true;
                    //});
                    //Result.Add(row);
                    x.calificacionIPSItem = row.calificacionIPSItem;

                    return true;
                });

                return Tuple.Create(Result, NoRegistrosTotales, NoRegistrosTotalesFiltrado, TotalPages);
                //return Data;
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        //Metodo para llamar procedure - ActualizarVariablesLider.
        public async Task ActualizarVariablesLider(InputsActualizarVariablesLiderDto entity)
        {
            try
            {
                string sql = "EXEC [dbo].[ActualizarVariablesLider] @VariableId, @SubGrupoId, @Default, @Auditable, @Visible";

                List<SqlParameter> parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@VariableId", Value = entity.VariableId},
                    new SqlParameter { ParameterName = "@SubGrupoId", Value = entity.SubGrupoId},
                    new SqlParameter { ParameterName = "@Default", Value = entity.Default},
                    new SqlParameter { ParameterName = "@Auditable", Value = entity.Auditable},
                    new SqlParameter { ParameterName = "@Visible", Value = entity.Visible},
                };

                var Data = await _dBAuditCACContext.Database.ExecuteSqlRawAsync(sql, parms.ToArray());
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        //Metodo para llamar procedure - ActualizarVariablesLiderMasivo.
        public async Task ActualizarVariablesLiderMasivo(List<InputsActualizarVariablesLiderDto> entity)
        {
            try
            {
                string sql = "EXEC [dbo].[ActualizarVariablesLiderMasivo] @QueryUpdate, @IdUsuario";

                //Declaramos variables usadas.
                var VariableId = "";
                var MedicionId = "";
                var Condition = "";
                var QueryUpdate = "";
                var IdUsuario = "";

                //Capturamos Listados.
                foreach (var itemList in entity)
                {
                    VariableId = itemList.VariableId.ToString();
                    MedicionId = itemList.MedicionId.ToString();
                    IdUsuario = itemList.IdUsuario == null ? "" : itemList.IdUsuario;

                    //SubGrupoId = itemList.SubGrupoId.ToString();
                    //SubGrupoId = (itemList.SubGrupoId != null) ? "SubGrupoId = " + itemList.SubGrupoId.ToString() : ""; //Si viene Null, guardar un Null.
                    if (itemList.SubGrupoId != "")
                    {
                        if (Condition == "")
                        {
                            Condition += "SubGrupoId = " + itemList.SubGrupoId.ToString();
                        }
                        else
                        {
                            Condition += ", SubGrupoId = " + itemList.SubGrupoId.ToString();
                        }
                    }                    

                    //Default = ", CalificacionXDefecto = " + Default;
                    //Default = (itemList.Default != null) ? "CalificacionXDefecto = " + itemList.Default.ToString() : ""; //Si viene Null, guardar un "".
                    if (itemList.Default != "")
                    {
                        if (Condition == "")
                        {
                            Condition += "CalificacionXDefecto = " + itemList.Default.ToString();
                        }
                        else
                        {
                            Condition += ", CalificacionXDefecto = " + itemList.Default.ToString();
                        }
                    }

                    //Auditable = itemList.Auditable.ToString();
                    //Auditable = (itemList.Auditable != null) ? "EsCalificable = " + itemList.Auditable.ToString() : ""; //Si viene Null, guardar un Null.
                    if (itemList.Auditable != "")
                    {
                        var Value = "";
                        if (itemList.Auditable.ToString().ToLower() == "true")
                        { Value = "1"; }

                        if (itemList.Auditable.ToString().ToLower() == "false")
                        { Value = "0"; }

                        if (Condition == "")
                        {
                            //Condition += "EsCalificable = " + itemList.Auditable.ToString();
                            Condition += "EsCalificable = " + Value;
                        }
                        else
                        {
                            //Condition += ", EsCalificable = " + itemList.Auditable.ToString();
                            Condition += ", EsCalificable = " + Value;
                        }
                    }

                    //Visible = itemList.Visible.ToString();
                    //Visible = (itemList.Visible != null) ? "EsVisible = " + itemList.Visible.ToString() : ""; //Si viene Null, guardar un Null.
                    if (itemList.Visible != "")
                    {
                        var Value = "";
                        if (itemList.Visible.ToString().ToLower() == "true")
                        { Value = "1"; }

                        if (itemList.Visible.ToString().ToLower() == "false")
                        { Value = "0"; }

                        if (Condition == "")
                        {
                            //Condition += "EsVisible = " + itemList.Visible.ToString();
                            Condition += "EsVisible = " + Value;
                        }
                        else
                        {
                            //Condition += ", EsVisible = " + itemList.Visible.ToString();
                            Condition += ", EsVisible = " + Value;
                        }
                    }

                    //Hallazgos = itemList.Hallazgos.ToString();
                    //Hallazgos = (itemList.Hallazgos != null) ? "Hallazgos = " + itemList.Hallazgos.ToString() : ""; //Si viene Null, guardar un Null.
                    if (itemList.Hallazgos != "")
                    {
                        var Value = "";
                        if (itemList.Hallazgos.ToString().ToLower() == "true")
                        { Value = "1"; }

                        if (itemList.Hallazgos.ToString().ToLower() == "false")
                        { Value = "0"; }

                        if (Condition == "")
                        {
                            //Condition += "Hallazgos = " + itemList.Hallazgos.ToString();
                            Condition += "Hallazgos = " + Value;
                        }
                        else
                        {
                            //Condition += ", Hallazgos = " + itemList.Hallazgos.ToString();
                            Condition += ", Hallazgos = " + Value;
                        }
                    }

                    //Armamos Query final.
                    //QueryUpdate = QueryUpdate + "UPDATE [VariableXMedicion] SET " + SubGrupoId + Default + Auditable + Visible + " WHERE VariableId = " + VariableId + ";";
                    QueryUpdate = QueryUpdate + "UPDATE [VariableXMedicion] SET " + Condition + " WHERE VariableId = " + VariableId + " AND MedicionId = " + MedicionId + ";";
                    Condition = "";
                }

                List<SqlParameter> parms = new List<SqlParameter>
                {                    
                    new SqlParameter { ParameterName = "@QueryUpdate", Value = QueryUpdate},
                    new SqlParameter { ParameterName = "@IdUsuario", Value = IdUsuario},     
                };

                var Data = await _dBAuditCACContext.Database.ExecuteSqlRawAsync(sql, parms.ToArray());
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        //Metodo para llamar procedure - CrearVariables.
        public async Task<bool> CrearVariables(InputsCEVariablesDto inputsDto)
        {
            try
            {
                string sql = "EXEC [dbo].[CrearVariables] @Variable, @Orden, @idCobertura, @nombre, @descripcion, @idTipoVariable, @longitud, @decimales, @formato, @tablaReferencial, @CreatedBy, @ModifyBy, @TipoVariableItem, @EstructuraVariable, @Alerta, @AlertaDescripcion, " +
                    "@MedicionId, @EsVisible, @EsCalificable, @Hallazgos, @EnableDC, @EnableNC, @EnableND, @CalificacionXDefecto, @SubGrupoId, @Encuesta, @CalificacionIPSItem," +
                    " @TipoCampo,@Promedio,@ValidarEntreRangos,@Desde,@Hasta,@Condicionada,@ValorConstante,@Lista, @VariablesCondicional, @Calculadora, @TipoCalculadora";

                //Validamos variables.
                string CalificacionIPSItem = "";
                CalificacionIPSItem = (inputsDto.CalificacionIPSItem.ToArray() != null) ? String.Join(", ", inputsDto.CalificacionIPSItem.ToArray()) : "";

                if (inputsDto.TipoVariableItem == 0)
                {
                    return false;
                }

                //Declare record datatable
                DataTable recordDataTable = new DataTable();
                recordDataTable.Columns.Add("Id", typeof(int));
                recordDataTable.Columns.Add("Valor", typeof(string));


                //Set data for detail data table
                for (int l = 0; l < inputsDto.VariableCondicional.Count(); l++)
                {
                    recordDataTable.Rows.Add(inputsDto.VariableCondicional[l].VariableHijaId, inputsDto.VariableCondicional[l].Nombre);
                }

                var parameterDataTable = new SqlParameter("@VariablesCondicional", SqlDbType.Structured);
                parameterDataTable.Value = recordDataTable;
                parameterDataTable.TypeName = "dbo.DT_LLave_Valor";


                List<SqlParameter> parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Variable", Value = inputsDto.Variable},
                    new SqlParameter { ParameterName = "@Orden", Value = inputsDto.Orden},
                    new SqlParameter { ParameterName = "@idCobertura", Value = inputsDto.idCobertura},
                    new SqlParameter { ParameterName = "@nombre", Value = inputsDto.nombre},
                    new SqlParameter { ParameterName = "@descripcion", Value = inputsDto.descripcion},
                    new SqlParameter { ParameterName = "@idTipoVariable", Value = inputsDto.idTipoVariable},
                    new SqlParameter { ParameterName = "@longitud", Value = inputsDto.Longitud},
                    new SqlParameter { ParameterName = "@decimales", Value = inputsDto.Decimales},
                    new SqlParameter { ParameterName = "@formato", Value = inputsDto.Formato},
                    new SqlParameter { ParameterName = "@tablaReferencial", Value = inputsDto.tablaReferencial},
                    new SqlParameter { ParameterName = "@CreatedBy", Value = inputsDto.CreatedBy},
                    new SqlParameter { ParameterName = "@ModifyBy", Value = inputsDto.ModifyBy},
                    new SqlParameter { ParameterName = "@TipoVariableItem", Value = inputsDto.TipoVariableItem}, //String.Join(", ", inputsDto.TipoVariableItem.ToArray()) | inputsDto.TipoVariableItem
                    new SqlParameter { ParameterName = "@EstructuraVariable", Value = inputsDto.EstructuraVariable},
                    new SqlParameter { ParameterName = "@Alerta", Value = inputsDto.Alerta},
                    new SqlParameter { ParameterName = "@AlertaDescripcion", Value = inputsDto.AlertaDescripcion},
                    new SqlParameter { ParameterName = "@MedicionId", Value = String.Join(", ", inputsDto.MedicionId.ToArray())},
                    new SqlParameter { ParameterName = "@EsVisible", Value = inputsDto.EsVisible},
                    new SqlParameter { ParameterName = "@EsCalificable", Value = inputsDto.EsCalificable},
                    new SqlParameter { ParameterName = "@Hallazgos", Value = inputsDto.Hallazgos},
                    new SqlParameter { ParameterName = "@EnableDC", Value = inputsDto.EnableDC},
                    new SqlParameter { ParameterName = "@EnableNC", Value = inputsDto.EnableNC},
                    new SqlParameter { ParameterName = "@EnableND", Value = inputsDto.EnableND},
                    new SqlParameter { ParameterName = "@CalificacionXDefecto", Value = inputsDto.CalificacionXDefecto},
                    new SqlParameter { ParameterName = "@SubGrupoId", Value = inputsDto.SubGrupoId}, //inputsDto.SubGrupoId},
                    new SqlParameter { ParameterName = "@Encuesta", Value = inputsDto.Encuesta},
                    new SqlParameter { ParameterName = "@CalificacionIPSItem", Value = CalificacionIPSItem},                    //
                    new SqlParameter { ParameterName = "@TipoCampo", Value = inputsDto.TipoCampo},
                    new SqlParameter { ParameterName = "@Promedio", Value = inputsDto.Promedio},
                    new SqlParameter { ParameterName = "@ValidarEntreRangos", Value = inputsDto.ValidarEntreRangos},
                    new SqlParameter { ParameterName = "@Desde", Value = inputsDto.Desde ?? ""},
                    new SqlParameter { ParameterName = "@Hasta", Value = inputsDto.Hasta ?? ""},
                    new SqlParameter { ParameterName = "@Condicionada", Value = inputsDto.Condicionada},
                    new SqlParameter { ParameterName = "@ValorConstante", Value = inputsDto.ValorConstante},
                    new SqlParameter { ParameterName = "@Lista", Value = inputsDto.Lista},
                    new SqlParameter { ParameterName = "@Calculadora", Value = inputsDto.Calculadora},
                    new SqlParameter { ParameterName = "@TipoCalculadora", Value = inputsDto.TipoCalculadora == null ? 0 : inputsDto.TipoCalculadora },

                    parameterDataTable
                };

                var Data = await _dBAuditCACContext.Database.ExecuteSqlRawAsync(sql, parms.ToArray());
                //var Data = await _dBAuditCACContext.LLaveValor.FromSqlRaw<ResponseLlaveValor>(sql, parms.ToArray()).ToListAsync();
                return true;
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        //Metodo para llamar procedure - CrearVariables.
        public async Task<bool> ActualizarVariables(InputsCEVariablesDto inputsDto)
        {
            try
            {
                string sql = "EXEC [dbo].[EditarVariables] @Variable, @Orden, @idCobertura, @nombre, @descripcion, @idTipoVariable, @longitud, @decimales, @formato, @tablaReferencial, @CreatedBy, @ModifyBy, @TipoVariableItem, @EstructuraVariable, @Alerta, @AlertaDescripcion, " +
                    "@MedicionId, @EsVisible, @EsCalificable, @Hallazgos, @EnableDC, @EnableNC, @EnableND, @CalificacionXDefecto, @SubGrupoId, @Encuesta, @CalificacionIPSItem," +
                    " @TipoCampo,@Promedio,@ValidarEntreRangos,@Desde,@Hasta,@Condicionada,@ValorConstante, @Lista, @VariablesCondicional, @Calculadora, @TipoCalculadora";


                //Validamos variables.
                string CalificacionIPSItem = "";
                CalificacionIPSItem = (inputsDto.CalificacionIPSItem.ToArray() != null) ? String.Join(", ", inputsDto.CalificacionIPSItem.ToArray()) : "";

                if (inputsDto.TipoVariableItem == 0)
                {
                    return false;
                }


                //Declare record datatable
                DataTable recordDataTable = new DataTable();
                recordDataTable.Columns.Add("Id", typeof(int));
                recordDataTable.Columns.Add("Valor", typeof(string));


                //Set data for detail data table
                for (int l = 0; l < inputsDto.VariableCondicional.Count(); l++)
                {
                    recordDataTable.Rows.Add(inputsDto.VariableCondicional[l].VariableHijaId, inputsDto.VariableCondicional[l].Nombre);
                }

                var parameterDataTable = new SqlParameter("@VariablesCondicional", SqlDbType.Structured);
                parameterDataTable.Value = recordDataTable;
                parameterDataTable.TypeName = "dbo.DT_LLave_Valor";

                List<SqlParameter> parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Variable", Value = inputsDto.Variable},
                    new SqlParameter { ParameterName = "@Orden", Value = inputsDto.Orden},
                    new SqlParameter { ParameterName = "@idCobertura", Value = inputsDto.idCobertura},
                    new SqlParameter { ParameterName = "@nombre", Value = inputsDto.nombre},
                    new SqlParameter { ParameterName = "@descripcion", Value = inputsDto.descripcion},
                    new SqlParameter { ParameterName = "@idTipoVariable", Value = inputsDto.idTipoVariable},
                    new SqlParameter { ParameterName = "@longitud", Value = inputsDto.Longitud},
                    new SqlParameter { ParameterName = "@decimales", Value = inputsDto.Decimales},
                    new SqlParameter { ParameterName = "@formato", Value = inputsDto.Formato},
                    new SqlParameter { ParameterName = "@tablaReferencial", Value = inputsDto.tablaReferencial ?? ""},
                    new SqlParameter { ParameterName = "@CreatedBy", Value = inputsDto.CreatedBy},
                    new SqlParameter { ParameterName = "@ModifyBy", Value = inputsDto.ModifyBy},
                    new SqlParameter { ParameterName = "@TipoVariableItem", Value = inputsDto.TipoVariableItem}, 
                    new SqlParameter { ParameterName = "@EstructuraVariable", Value = inputsDto.EstructuraVariable},
                    new SqlParameter { ParameterName = "@Alerta", Value = inputsDto.Alerta},
                    new SqlParameter { ParameterName = "@AlertaDescripcion", Value = inputsDto.AlertaDescripcion},                    
                    new SqlParameter { ParameterName = "@MedicionId", Value = String.Join(", ", inputsDto.MedicionId.ToArray())},
                    new SqlParameter { ParameterName = "@EsVisible", Value = inputsDto.EsVisible},
                    new SqlParameter { ParameterName = "@EsCalificable", Value = inputsDto.EsCalificable},
                    new SqlParameter { ParameterName = "@Hallazgos", Value = inputsDto.Hallazgos},
                    new SqlParameter { ParameterName = "@EnableDC", Value = inputsDto.EnableDC},
                    new SqlParameter { ParameterName = "@EnableNC", Value = inputsDto.EnableNC},
                    new SqlParameter { ParameterName = "@EnableND", Value = inputsDto.EnableND},
                    new SqlParameter { ParameterName = "@CalificacionXDefecto", Value = inputsDto.CalificacionXDefecto},
                    new SqlParameter { ParameterName = "@SubGrupoId", Value = inputsDto.SubGrupoId},
                    new SqlParameter { ParameterName = "@Encuesta", Value = inputsDto.Encuesta},
                    new SqlParameter { ParameterName = "@CalificacionIPSItem", Value = String.Join(", ", inputsDto.CalificacionIPSItem.ToArray())},
                    new SqlParameter { ParameterName = "@TipoCampo", Value = inputsDto.TipoCampo},
                    new SqlParameter { ParameterName = "@Promedio", Value = inputsDto.Promedio},
                    new SqlParameter { ParameterName = "@ValidarEntreRangos", Value = inputsDto.ValidarEntreRangos},
                    new SqlParameter { ParameterName = "@Desde", Value = inputsDto.Desde ?? ""},
                    new SqlParameter { ParameterName = "@Hasta", Value = inputsDto.Hasta ?? ""},
                    new SqlParameter { ParameterName = "@Condicionada", Value = inputsDto.Condicionada},
                    new SqlParameter { ParameterName = "@ValorConstante", Value = inputsDto.ValorConstante},
                    new SqlParameter { ParameterName = "@Lista", Value = inputsDto.Lista},
                    new SqlParameter { ParameterName = "@Calculadora", Value = inputsDto.Calculadora == null ? false : inputsDto.Calculadora },
                    new SqlParameter { ParameterName = "@TipoCalculadora", Value = inputsDto.TipoCalculadora == null ? 0 : inputsDto.TipoCalculadora },
                    parameterDataTable

                };

                var Data = await _dBAuditCACContext.Database.ExecuteSqlRawAsync(sql, parms.ToArray());
                //var Data = await _dBAuditCACContext.LLaveValor.FromSqlRaw<ResponseLlaveValor>(sql, parms.ToArray()).ToListAsync();
                return true;
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        //Metodo para llamar procedure ConsultarOrderVariables - Para consultar Orden de Variables Y VariablexMedicion
        public async Task<List<ResponseDetalleConsultarOrderVariablesDto>> ConsultarOrderVariables(InputConsultarOrderVariablesDto inputsDto)
        {
            try
            {
                string sql = "EXEC [dbo].[ConsultarOrderVariables] @Variable, @idCobertura, @MedicionId";
                              
                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters   
                    new SqlParameter { ParameterName = "@Variable", Value = inputsDto.Variable},
                    new SqlParameter { ParameterName = "@idCobertura", Value = inputsDto.idCobertura},
                    new SqlParameter { ParameterName = "@MedicionId", Value = inputsDto.MedicionId }
                };

                var Data = await _dBAuditCACContext.ResponseDetalleConsultarOrderVariablesDto.FromSqlRaw<ResponseDetalleConsultarOrderVariablesDto>(sql, parms.ToArray()).ToListAsync();

                return Data;
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Consulta variables condicionadas por id de variable
        /// </summary>
        /// <param name="variableId"></param>
        /// <returns></returns>
        public async Task<List<VariableCondicionalDto>> ConsultaVariablesCondicionadas(int variableId, int medicionId)
        {
            try
            {
                string sql = "EXEC [dbo].[SP_Consulta_Variables_Condicionadas] @VariableId, @MedicionId";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters   
                    new SqlParameter { ParameterName = "@VariableId", Value = variableId },
                    new SqlParameter { ParameterName = "@MedicionId", Value = medicionId }
                };

                var Data = await _dBAuditCACContext.VariableCondicionalDto.FromSqlRaw<VariableCondicionalDto>(sql, parms.ToArray()).ToListAsync();

                return Data;
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }
    }
}
