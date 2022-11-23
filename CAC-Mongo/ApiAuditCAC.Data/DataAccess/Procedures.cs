using ApiAuditCAC.Domain.Dto;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiAuditCAC.Data.DataAccess
{
    public class Procedures : IProcedures
    {
        #region variables api        
        private readonly string ConnectionStrings;
        private readonly SqlConnection sqlConnection;
        private SqlDataReader sqlDataReader;
        private SqlCommand sqlCommand;
        #endregion
        public Procedures(IConfiguration configuration, ILogger<Procedures> logger)
        {
            _configuration = configuration;
            _logger = logger;

            ConnectionStrings = _configuration["ConnectionStrings:DefaultConnection"];
            sqlConnection = new(ConnectionStrings);
        }

        private readonly ILogger<Procedures> _logger;
        private readonly IConfiguration _configuration;
        public String GetColums(int Id)
        {
            String result = null;
            try {
                sqlCommand = new SqlCommand("[dbo].[Sp_ObtenerColumnas]", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@idRegistroAuditoria", Id);
                sqlConnection.Open();
                sqlDataReader = sqlCommand.ExecuteReader();
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        if (sqlDataReader[0] is not null) {
                            result = sqlDataReader[0].ToString();
                        }
                    }
                }
                else
                {
                    _logger.LogError("No se encontraron resultados, sp:[dbo].[SpObtenerColumnas]");
                }
                sqlConnection.Close();
            }
            catch (Exception e) 
            {
                sqlConnection.Close();
                _logger.LogError(e.Message);
                throw new Exception(e.Message);
            }

            return result;
        }

        public Dictionary<String, Object> GetData(int Id, String column, String tabla)
        {
            Dictionary<String, Object> result = new();
            List<String> columns;
            try
            {
                String variables = GetColums(Id);
                sqlCommand = new SqlCommand("[dbo].[Sp_ObtenerValoresVariables]", sqlConnection); 
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.Parameters.AddWithValue("@idRegistroAuditoria", Id);
                sqlCommand.Parameters.AddWithValue("@variables", variables);
                sqlCommand.Parameters.AddWithValue("@columna", column);
                sqlCommand.Parameters.AddWithValue("@tabla", tabla);
                sqlConnection.Open();

                sqlDataReader = sqlCommand.ExecuteReader();                
                if (sqlDataReader.HasRows)
                {
                    columns = Enumerable.Range(0, sqlDataReader.FieldCount).Select(sqlDataReader.GetName).ToList();
                    sqlDataReader.Read();
                    foreach (String col in columns) 
                    {                         
                        result.Add(col, sqlDataReader[col] is null || sqlDataReader[col] is DBNull ? "null": Convert.ToString(sqlDataReader[col]));
                        //result.Add(column, (String)sqlDataReader[column]);
                    }
                    sqlDataReader.Close();
                }
                else
                {
                    _logger.LogError("No se encontraron resultados, sp: [dbo].[Sp_ObtenerValoresVariables]");
                }

                sqlConnection.Close();
            }
            catch (Exception e) {
                sqlConnection.Close();
                _logger.LogError(e.Message);
            }
            return result;
        }

        public List<SpRegistrosAudResponseDto> GetIdsAudByMedicion(int idMedicion)
        {
            List<SpRegistrosAudResponseDto> result = new();
            try
            {
                var horas = GetParametrosGeneralesByName("HorasActualizarMongo");                
                var fecha = DateTime.Now.AddHours(-1D * double.Parse(horas)).ToString("yyyy-MM-dd 23:59:59");
                sqlCommand = new SqlCommand("[dbo].[Sp_ObtenerIdsRegistrosAuditoria]", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@idMedicion", idMedicion);
                sqlCommand.Parameters.AddWithValue("@fecha", fecha);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlConnection.Open();
                sqlDataReader = sqlCommand.ExecuteReader();
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        SpRegistrosAudResponseDto item = new()
                        {
                            Id = Convert.ToInt32(sqlDataReader[0]),
                            IdRadicado = Convert.ToInt32(sqlDataReader[1]),
                            IdMedicion = Convert.ToInt32(sqlDataReader[2]),
                            Estado = Convert.ToInt32(sqlDataReader[3]),
                            Nombre = (String)sqlDataReader[4],
                            Codigo = (String)sqlDataReader[5],
                            Descripcion = (String)sqlDataReader[6],
                            EPS = (string)sqlDataReader[7],
                            NombreMedicion = (string)sqlDataReader[8],
                            ModifyDate = sqlDataReader[9] == null ? null : Convert.ToDateTime(sqlDataReader[9]),
                        };
                        result.Add(item);                        
                    }
                }
                else {
                    _logger.LogError("No se encontraron resultados, sp: [dbo].[Sp_ObtenerIdsRegistrosAuditoria] idMedicion:"+ idMedicion+",fecha:"+fecha);
                }
                sqlConnection.Close();
            }
            catch (Exception e)
            {
                sqlConnection.Close();
                _logger.LogError(e.Message);
            }
            return result;
        }

        public List<int> GetMediciones()
        {
            List<int> result = new();
            try
            {
                sqlCommand = new SqlCommand("[dbo].[Sp_IdsMedicion]", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlConnection.Open();
                sqlDataReader = sqlCommand.ExecuteReader();
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {                        
                        result.Add(Convert.ToInt32(sqlDataReader[0]));
                    }
                }
                else
                {
                    _logger.LogError("No se encontraron resultados, sp: [dbo].[Sp_IdsMedicion]");
                }
                sqlConnection.Close();
            }
            catch (Exception e)
            {
                sqlConnection.Close();
                _logger.LogError(e.Message);
            }
            return result;
        }

        public String GetParametrosGeneralesByName(string nombre)
        {
            String result = String.Empty;
            try
            {
                sqlCommand = new SqlCommand("[dbo].[SP_ObtenerParametrosGeneralesByName]", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@nombre", nombre);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlConnection.Open();
                sqlDataReader = sqlCommand.ExecuteReader();
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        result = (String)sqlDataReader[0];
                    }
                }
                else
                {                    
                    _logger.LogError("No se encontraron resultados, sp: [dbo].[SP_ObtenerParametrosGeneralesByName] nombre:"+nombre);
                }
                sqlConnection.Close();
            }
            catch (Exception e)
            {
                sqlConnection.Close();
                _logger.LogError(e.Message);
            }
            return result;
        }

        public string GetObservaciones(int id)
        {
            String result = String.Empty;
            try
            {
                sqlCommand = new SqlCommand("[dbo].[Sp_ObtenerObservaciones]", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@idRegistroAuditoria", id);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlConnection.Open();
                sqlDataReader = sqlCommand.ExecuteReader();
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        result = sqlDataReader[0] is null || sqlDataReader[0] is DBNull ? "null" : Convert.ToString(sqlDataReader[0]);
                    }
                }
                else
                {
                    _logger.LogInformation("No se encontraron resultados, sp: [dbo].[Sp_ObtenerObservaciones]");
                }
                sqlConnection.Close();
            }
            catch (Exception e)
            {
                sqlConnection.Close();
                _logger.LogError(e.Message);
            }
            return result;
        }


        public List<CoberturaDto> ConsultarCoberturas()
        {
            List<CoberturaDto> result = new();
            try
            {
                sqlCommand = new SqlCommand("[dbo].[getEnfermedades]", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlConnection.Open();
                sqlDataReader = sqlCommand.ExecuteReader();
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        CoberturaDto item = new()
                        {
                            IdEnfermedad = Convert.ToInt32(sqlDataReader[0]),
                            IdCobertura = Convert.ToInt32(sqlDataReader[1]),
                            Nombre = (String)(sqlDataReader[2]),
                            Status = Convert.ToBoolean(sqlDataReader[3])
                        };
                        result.Add(item);
                    }
                }
                else
                {
                    _logger.LogError("No se encontraron resultados, sp: [dbo].[getEnfermedadesa]"  + ",fecha:" + DateTime.Now);
                }
                sqlConnection.Close();
            }
            catch (Exception e)
            {
                sqlConnection.Close();
                _logger.LogError(e.Message);
            }
            return result;
        }

        public List<RegistrosAuditoriaDto> ConsultaRegistrosAuditoriaPorMedicion(int id)
        {
            List<RegistrosAuditoriaDto> result = new();
            try
            {
                sqlCommand = new SqlCommand("[dbo].[SP_Mongo_Consulta_Registros_Auditar_Medicion]", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@IdMedicion", id);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 99999999;
                sqlConnection.Open();
                sqlDataReader = sqlCommand.ExecuteReader();
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        try
                        {
                            #region Build Detail

                            RegistrosAuditoriaDto item = new RegistrosAuditoriaDto();

                            item.Id = sqlDataReader[0] is System.DBNull ? 0 : Convert.ToInt32(sqlDataReader[0]);
                            item.IdRadicado = sqlDataReader[1] is System.DBNull ? 0 : Convert.ToInt32(sqlDataReader[1]);
                            item.IdMedicion = sqlDataReader[2] is System.DBNull ? 0 : Convert.ToInt32(sqlDataReader[2]);
                            item.IdPeriodo = sqlDataReader[3] is System.DBNull ? 0 : Convert.ToInt32(sqlDataReader[3]);
                            item.IdAuditor = Convert.ToString(sqlDataReader[4]);
                            item.PrimerNombre = Convert.ToString(sqlDataReader[5]);
                            item.SegundoNombre = Convert.ToString(sqlDataReader[6]);
                            item.PrimerApellido = Convert.ToString(sqlDataReader[7]);
                            item.SegundoApellido = Convert.ToString(sqlDataReader[5]);
                            item.Sexo = Convert.ToString(sqlDataReader[9]);
                            item.TipoIdentificacion = Convert.ToString(sqlDataReader[10]);
                            item.Identificacion = Convert.ToString(sqlDataReader[11]);
                            item.FechaNacimiento = sqlDataReader[12] is System.DBNull ? null : Convert.ToDateTime(sqlDataReader[12]);
                            item.FechaCreacion = sqlDataReader[13] is System.DBNull ? null : Convert.ToDateTime(sqlDataReader[13]);
                            item.FechaAuditoria = sqlDataReader[14] is System.DBNull ? null : Convert.ToDateTime(sqlDataReader[14]);
                            item.FechaMinConsultaAudit = sqlDataReader[15] is System.DBNull ? null : Convert.ToDateTime(sqlDataReader[15]);
                            item.FechaMaxConsultaAudit = sqlDataReader[16] is System.DBNull ? null : Convert.ToDateTime(sqlDataReader[16]);
                            item.FechaAsignacion = sqlDataReader[17] is System.DBNull ? null : Convert.ToDateTime(sqlDataReader[17]);
                            item.Activo = Convert.ToBoolean(sqlDataReader[18]);
                            item.Reverse = Convert.ToBoolean(sqlDataReader[19]);
                            item.DisplayOrder = sqlDataReader[20] is System.DBNull ? 0 : Convert.ToInt32(sqlDataReader[20]);
                            item.Ara = Convert.ToBoolean(sqlDataReader[21]);
                            item.Eps = Convert.ToBoolean(sqlDataReader[22]);
                            item.FechaReverso = sqlDataReader[23] is System.DBNull ? null : Convert.ToDateTime(sqlDataReader[23]);
                            item.AraAtendido = Convert.ToBoolean(sqlDataReader[24]);
                            item.EpsAtendido = Convert.ToBoolean(sqlDataReader[25]);
                            item.Revisar = Convert.ToBoolean(sqlDataReader[26]);
                            item.Extemporaneo = Convert.ToBoolean(sqlDataReader[27]);
                            item.EstadoRegistroAuditoria = sqlDataReader[28] is System.DBNull ? 0 : Convert.ToInt32(sqlDataReader[28]);
                            item.CodigoEstadoRegistroAuditoria = Convert.ToString(sqlDataReader[29]);
                            item.DescripcionEstadoRegistroAuditoria = Convert.ToString(sqlDataReader[30]);
                            item.LevantarGlosa = sqlDataReader[31] is System.DBNull ? 0 : Convert.ToInt32(sqlDataReader[31]);
                            item.MantenerCalificacion = sqlDataReader[32] is System.DBNull ? 0 : Convert.ToInt32(sqlDataReader[32]);
                            item.ComiteExperto = sqlDataReader[33] is System.DBNull ? 0 : Convert.ToInt32(sqlDataReader[33]);
                            item.ComiteAdministrativo = sqlDataReader[34] is System.DBNull ? 0 : Convert.ToInt32(sqlDataReader[34]);
                            item.AccionLider = sqlDataReader[35] is System.DBNull ? 0 : Convert.ToInt32(sqlDataReader[35]);
                            item.AccionAuditor = sqlDataReader[36] is System.DBNull ? 0 : Convert.ToInt32(sqlDataReader[36]);
                            item.EncuestaRegistroAuditoria = Convert.ToBoolean(sqlDataReader[37]);
                            item.CreatedByRegistroAuditoria = Convert.ToString(sqlDataReader[38]);
                            item.CreatedDateRegistroAuditoria = sqlDataReader[39] is System.DBNull ? null : Convert.ToDateTime(sqlDataReader[39]);
                            item.ModifyByRegistroAuditoria = Convert.ToString(sqlDataReader[40]);
                            item.ModifyDateRegistroAuditoria = sqlDataReader[41] is System.DBNull ? null : Convert.ToDateTime(sqlDataReader[41]);
                            item.IdEPS = Convert.ToString(sqlDataReader[42]);
                            item.StatusRegistroAuditoria = Convert.ToBoolean(sqlDataReader[43]);
                            item.IdRegistroAuditoriaDetalle = sqlDataReader[44] is System.DBNull ? 0 : Convert.ToInt32(sqlDataReader[44]);
                            item.idVariable = sqlDataReader[45] is System.DBNull ? 0 : Convert.ToInt32(sqlDataReader[45]);
                            item.VariableId = sqlDataReader[46] is System.DBNull ? 0 : Convert.ToInt32(sqlDataReader[46]);
                            item.CreatedByRegistroAuditoriaDetalle = Convert.ToString(sqlDataReader[47]);
                            item.CreatedDateRegistroAuditoriaDetalle = sqlDataReader[48] is System.DBNull ? null : Convert.ToDateTime(sqlDataReader[48]);
                            item.ModifyByRegistroAuditoriaDetalle = Convert.ToString(sqlDataReader[49]);
                            item.ModifyDateRegistroAuditoriaDetalle = sqlDataReader[50] is System.DBNull ? null : Convert.ToDateTime(sqlDataReader[50]);
                            item.DatoReportado = Convert.ToString(sqlDataReader[51]);
                            item.MotivoVariable = Convert.ToString(sqlDataReader[52]);
                            item.Dato_DC_NC_ND = sqlDataReader[53] is System.DBNull ? 0 : Convert.ToInt32(sqlDataReader[53]);
                            item.Dato_DC_NC_NDNombre = Convert.ToString(sqlDataReader[54]);
                            item.AraRegistroAuditoriaDetalle = Convert.ToBoolean(sqlDataReader[55]);
                            item.StatusRegistroAuditoriaDetalle = Convert.ToBoolean(sqlDataReader[56]);
                            item.Contexto = Convert.ToString(sqlDataReader[57]);
                            item.EsGlosa = Convert.ToBoolean(sqlDataReader[58]);
                            item.EsVisible = Convert.ToBoolean(sqlDataReader[59]);
                            item.EsCalificable = Convert.ToBoolean(sqlDataReader[60]);
                            item.EnableDC = Convert.ToBoolean(sqlDataReader[61]);
                            item.EnableNC = Convert.ToBoolean(sqlDataReader[62]);
                            item.EnableND = Convert.ToBoolean(sqlDataReader[63]);
                            item.CalificacionXDefecto = sqlDataReader[64] is System.DBNull ? 0 : Convert.ToInt32(sqlDataReader[64]);
                            item.CalificacionXDefectoNombre = Convert.ToString(sqlDataReader[65]);
                            item.SubGrupoId = sqlDataReader[66] is System.DBNull ? 0 : Convert.ToInt32(sqlDataReader[66]);
                            item.SubGrupoNombre = Convert.ToString(sqlDataReader[67]);
                            item.EncuestaVariablexMedicion = Convert.ToBoolean(sqlDataReader[68]);
                            item.OrdenVariablexMedicion = sqlDataReader[69] is System.DBNull ? 0 : Convert.ToInt32(sqlDataReader[69]);
                            item.StatusVariablexMedicion = Convert.ToBoolean(sqlDataReader[70]);
                            item.TipoCampo = sqlDataReader[71] is System.DBNull ? 0 : Convert.ToInt32(sqlDataReader[71]);
                            item.TipoCampoNombre = Convert.ToString(sqlDataReader[72]);
                            item.Promedio = Convert.ToBoolean(sqlDataReader[73]);
                            item.ValidarEntreRangos = Convert.ToBoolean(sqlDataReader[74]);
                            item.Desde = Convert.ToString(sqlDataReader[75]);
                            item.Hasta = Convert.ToString(sqlDataReader[76]);
                            item.Condicionada = Convert.ToBoolean(sqlDataReader[77]);
                            item.ValorConstante = Convert.ToString(sqlDataReader[78]);
                            item.Lista = Convert.ToBoolean(sqlDataReader[79]);
                            item.ActivoVariableXMedicion = Convert.ToBoolean(sqlDataReader[80]);
                            item.CreatedByVariableXMedicion = Convert.ToString(sqlDataReader[81]);
                            item.CreationDateVariableXMedicion = sqlDataReader[82] is System.DBNull ? null : Convert.ToDateTime(sqlDataReader[82]);
                            item.ModifyByVariableXMedicion = Convert.ToString(sqlDataReader[83]);
                            item.ModificationDateVariableXMedicion = sqlDataReader[84] is System.DBNull ? null : Convert.ToDateTime(sqlDataReader[84]);
                            item.Hallazgos = Convert.ToBoolean(sqlDataReader[85]);
                            item.Activa = Convert.ToBoolean(sqlDataReader[86]);
                            item.OrdenVariable = sqlDataReader[87] is System.DBNull ? 0 : Convert.ToInt32(sqlDataReader[87]);
                            item.idCobertura = sqlDataReader[88] is System.DBNull ? 0 : Convert.ToInt32(sqlDataReader[88]);
                            item.nombreVariable = Convert.ToString(sqlDataReader[89]);
                            item.nemonicoVariable = Convert.ToString(sqlDataReader[90]);
                            item.descripcionVariable = Convert.ToString(sqlDataReader[91]);
                            item.idTipoVariable = Convert.ToString(sqlDataReader[92]);
                            item.longitud = sqlDataReader[93] is System.DBNull ? 0 : Convert.ToInt32(sqlDataReader[93]);
                            item.decimales = sqlDataReader[94] is System.DBNull ? 0 : Convert.ToInt32(sqlDataReader[94]);
                            item.formato = Convert.ToString(sqlDataReader[95]);
                            item.tablaReferencial = Convert.ToString(sqlDataReader[96]);
                            item.campoReferencial = Convert.ToString(sqlDataReader[97]);
                            item.Bot = Convert.ToBoolean(sqlDataReader[98]);
                            item.TipoVariableItem = sqlDataReader[99] is System.DBNull ? 0 : Convert.ToInt32(sqlDataReader[99]);
                            item.ItemTipoVariableNombre = Convert.ToString(sqlDataReader[100]);
                            item.Alerta = Convert.ToBoolean(sqlDataReader[101]);
                            item.AlertaDescripcion = Convert.ToString(sqlDataReader[102]);
                            item.StatusVariable = Convert.ToBoolean(sqlDataReader[103]);
                            item.idErrorTipo = Convert.ToString(sqlDataReader[104]);
                            item.EstadoMedicion = sqlDataReader[105] is System.DBNull ? 0 : Convert.ToInt32(sqlDataReader[105]);
                            item.EstadoMedicionNombre = Convert.ToString(sqlDataReader[106]);
                            item.MedicionNombre = Convert.ToString(sqlDataReader[107]);
                            item.MedicionDescripcion = Convert.ToString(sqlDataReader[108]);
                            item.MedicionIdCobertura = sqlDataReader[109] is System.DBNull ? 0 : Convert.ToInt32(sqlDataReader[109]);
                            item.FechaFinAuditoria = sqlDataReader[110] is System.DBNull ? null : Convert.ToDateTime(sqlDataReader[110]);
                            item.CoberturaNombre = Convert.ToString(sqlDataReader[111]);
                            item.CoberturaNemonico = Convert.ToString(sqlDataReader[112]);
                            item.CoberturaLegislacion = Convert.ToString(sqlDataReader[113]);
                            item.CoberturaDefinicion = Convert.ToString(sqlDataReader[114]);
                            item.RegimenEPS_Id = sqlDataReader[115] is System.DBNull ? 0 : Convert.ToInt32(sqlDataReader[115]);
                            item.RegimenEPS_Nombre = Convert.ToString(sqlDataReader[116]);
                            item.RenglonEPS_Id = sqlDataReader[117] is System.DBNull ? 0 : Convert.ToInt32(sqlDataReader[117]);
                            item.RenglonEPS_Nombre = Convert.ToString(sqlDataReader[118]);

                            item.Calculadora = sqlDataReader[119] is System.DBNull ? null : Convert.ToBoolean(sqlDataReader[119]);
                            item.TipoCalculadora = sqlDataReader[120] is System.DBNull ? null : Convert.ToInt32(sqlDataReader[120]);
                            item.VariablesAsociados = sqlDataReader[121] is System.DBNull ? null : Convert.ToBoolean(sqlDataReader[121]);
                            item.Comodines = sqlDataReader[122] is System.DBNull ? null : Convert.ToBoolean(sqlDataReader[122]);

                            result.Add(item);

                            #endregion
                        }
                        catch (Exception ex)
                        {
                            _logger.LogError("Error" + ex.ToString());
                        }
                            
                    }
                }
                else
                {
                    _logger.LogError("No se encontraron resultados, sp: [dbo].[SP_Consulta_Registros_Auditar_Medicion]" + ",idmedicion :" + id);
                }
                sqlConnection.Close();
            }
            catch (Exception e)
            {
                sqlConnection.Close();
                _logger.LogError(e.Message);
            }
            return result;
        }

        public List<RegistrosAuditoriaSeguimientoDto> ConsultaRegistrosAuditoriaObservacionesPorMedicion(int id)
        {
            List<RegistrosAuditoriaSeguimientoDto> result = new();
            try
            {
                sqlCommand = new SqlCommand("[dbo].[SP_Mongo_Consulta_Registros_Seguimiento_Medicion]", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@IdMedicion", id);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 99999999;
                sqlConnection.Open();
                sqlDataReader = sqlCommand.ExecuteReader();
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        RegistrosAuditoriaSeguimientoDto item = new()
                        {
                            Id = Convert.ToInt32(sqlDataReader[0]),
                            RegistroAuditoriaId = Convert.ToInt32(sqlDataReader[1]),
                            TipoObservacion = Convert.ToInt32(sqlDataReader[2]),
                            TipoObservacionNombre = Convert.ToString(sqlDataReader[3]),
                            Observacion = Convert.ToString(sqlDataReader[4]),
                            Soporte = Convert.ToInt32(sqlDataReader[5]),
                            EstadoActual = Convert.ToInt32(sqlDataReader[6]),
                            EstadoActualNombre = Convert.ToString(sqlDataReader[7]),
                            EstadoNuevo = Convert.ToInt32(sqlDataReader[8]),
                            EstadoNuevoNombre = Convert.ToString(sqlDataReader[9]),
                            CreatedBy = Convert.ToString(sqlDataReader[10]),
                            CreatedDate = Convert.ToDateTime(sqlDataReader[11]),
                            ModifyBy = Convert.ToString(sqlDataReader[12]),
                            ModifyDate = Convert.ToDateTime(sqlDataReader[13]),
                            Status = Convert.ToInt32(sqlDataReader[14])
                        };
                        result.Add(item);
                    }
                }
                else
                {
                    _logger.LogError("No se encontraron resultados, sp: [dbo].[SP_Consulta_Registros_Auditar_Medicion]" + ",idmedicion :" + id);
                }
                sqlConnection.Close();
            }
            catch (Exception e)
            {
                sqlConnection.Close();
                _logger.LogError(e.Message);
            }
            return result;
        }


        public List<HallazgosDto> ConsultaHallazgosPorMedicion(int id)
        {
            List<HallazgosDto> result = new();
            try
            {
                sqlCommand = new SqlCommand("[dbo].[SP_Mongo_Consulta_Registros_Hallazgos_Medicion]", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@IdMedicion", id);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 99999999;
                sqlConnection.Open();
                sqlDataReader = sqlCommand.ExecuteReader();
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        try
                        {
                            HallazgosDto item = new()
                            {
                                Id = Convert.ToInt32(sqlDataReader[0]),
                                RegistrosAuditoriaDetalleId = Convert.ToInt32(sqlDataReader[1]),
                                Observacion = Convert.ToString(sqlDataReader[2]),
                                Estado = Convert.ToInt32(sqlDataReader[3]),
                                EstadoNombre = Convert.ToString(sqlDataReader[4]),
                                CreatedBy = Convert.ToString(sqlDataReader[5]),
                                CreatedDate = Convert.ToDateTime(sqlDataReader[6]),
                                ModifyBy = Convert.ToString(sqlDataReader[7]),
                                ModifyDate = Convert.ToDateTime(sqlDataReader[8]),
                                Status = Convert.ToBoolean(sqlDataReader[9]),
                                Dato_DC_NC_ND = Convert.ToInt32(sqlDataReader[10]),
                                Dato_DC_NC_ND_Nombre = Convert.ToString(sqlDataReader[11])
                            };
                            result.Add(item);
                        }
                        catch (Exception ex)
                        {
                            ;
                        }
                        
                    }
                }
                else
                {
                    _logger.LogError("No se encontraron resultados, sp: [dbo].[SP_Consulta_Registros_Hallazgos_Medicion]" + ",idmedicion :" + id);
                }
                sqlConnection.Close();
            }
            catch (Exception e)
            {
                sqlConnection.Close();
                _logger.LogError(e.Message);
            }
            return result;
        }


        public List<SoportesDto> ConsultaSoportesPorMedicion(int id)
        {
            List<SoportesDto> result = new();
            try
            {
                sqlCommand = new SqlCommand("[dbo].[SP_Mongo_Consulta_Soportes_Medicion]", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@IdMedicion", id);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 99999999;
                sqlConnection.Open();
                sqlDataReader = sqlCommand.ExecuteReader();
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        try
                        {
                            SoportesDto item = new()
                            {
                                Id = Convert.ToInt32(sqlDataReader[0]),
                                IdRegistrosAuditoria = Convert.ToInt32(sqlDataReader[1]),
                                IdSoporte = Convert.ToInt32(sqlDataReader[2]),
                                NombreSoporte = Convert.ToString(sqlDataReader[3]),
                                UrlSoporte = Convert.ToString(sqlDataReader[4]),
                                CreatedBy = Convert.ToString(sqlDataReader[5]),
                                CreatedDate = Convert.ToDateTime(sqlDataReader[6]),
                                ModifyBy = Convert.ToString(sqlDataReader[7]),
                                ModifyDate = Convert.ToDateTime(sqlDataReader[8]),
                                Status = Convert.ToBoolean(sqlDataReader[9]),
                        
                            };
                            result.Add(item);
                        }
                        catch (Exception ex)
                        {
                            ;
                        }

                    }
                }
                else
                {
                    _logger.LogError("No se encontraron resultados, sp: [dbo].[SP_Consulta_Soportes_Medicion]" + ",idmedicion :" + id);
                }
                sqlConnection.Close();
            }
            catch (Exception e)
            {
                sqlConnection.Close();
                _logger.LogError(e.Message);
            }
            return result;
        }


        public List<CalificacionDto> ConsultaCalificacionPorMedicion(int id)
        {
            List<CalificacionDto> result = new();
            try
            {
                sqlCommand = new SqlCommand("[dbo].[SP_Mongo_Consulta_Registros_Calificacion_Medicion]", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@IdMedicion", id);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 99999999;
                sqlConnection.Open();
                sqlDataReader = sqlCommand.ExecuteReader();
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        try
                        {
                            CalificacionDto item = new()
                            {
                                Id = Convert.ToInt32(sqlDataReader[0]),
                                RegistrosAuditoriaId = Convert.ToInt32(sqlDataReader[1]),
                                RegistrosAuditoriaDetalleId = Convert.ToInt32(sqlDataReader[2]),
                                VariableId = Convert.ToInt32(sqlDataReader[3]),
                                IpsId = Convert.ToInt32(sqlDataReader[4]),
                                ItemCalificacionNombre = Convert.ToString(sqlDataReader[5]),
                                Calificacion = Convert.ToInt32(sqlDataReader[6]),
                                Observacion = Convert.ToString(sqlDataReader[7]),
                                CreatedBy = Convert.ToString(sqlDataReader[8]),
                                CreatedDate = sqlDataReader[9] is System.DBNull ? null : Convert.ToDateTime(sqlDataReader[9]),
                                ModifyBy = Convert.ToString(sqlDataReader[10]),
                                ModifyDate = sqlDataReader[11] is System.DBNull ? null : Convert.ToDateTime(sqlDataReader[11])
                            };

                            result.Add(item);
                        }
                        catch (Exception ex)
                        {
                            ;
                        }

                    }
                }
                else
                {
                    _logger.LogError("No se encontraron resultados, sp: [dbo].[SP_Consulta_Registros_Calificacion_Medicion]" + ",idmedicion :" + id);
                }
                sqlConnection.Close();
            }
            catch (Exception e)
            {
                sqlConnection.Close();
                _logger.LogError(e.Message);
            }
            return result;
        }


        public List<RegistroAuditoriaDetalleErrores> ConsultaErroresRegistroAuditarMedicion(int id)
        {
            List<RegistroAuditoriaDetalleErrores> result = new();
            try
            {
                sqlCommand = new SqlCommand("[dbo].[SP_Mongo_Consulta_Errores_Registro_Auditar_Medicion]", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@IdMedicion", id);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 99999999;
                sqlConnection.Open();
                sqlDataReader = sqlCommand.ExecuteReader();
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        RegistroAuditoriaDetalleErrores item = new(){};

                        item.Id = sqlDataReader[0] is System.DBNull ? 0 : Convert.ToInt32(sqlDataReader[0]);
                        item.RegistrosAuditoriaDetalleId = sqlDataReader[1] is System.DBNull ? 0 : Convert.ToInt32(sqlDataReader[1]);
                        item.IdRegla = sqlDataReader[2] is System.DBNull ? 0 : Convert.ToInt32(sqlDataReader[2]);
                        item.IdRestriccion = sqlDataReader[3] is System.DBNull ? 0 : Convert.ToInt32(sqlDataReader[3]);
                        item.Reducido = Convert.ToString(sqlDataReader[4]);
                        item.VariableId = sqlDataReader[5] is System.DBNull ? 0 : Convert.ToInt32(sqlDataReader[5]);
                        item.ErrorId = Convert.ToString(sqlDataReader[6]);
                        item.Descripcion = Convert.ToString(sqlDataReader[7]);
                        item.Sentencia = Convert.ToString(sqlDataReader[8]);
                        item.NoCorregible = sqlDataReader[9] is System.DBNull ? false : Convert.ToBoolean(sqlDataReader[9]);
                        item.Enable = sqlDataReader[10] is System.DBNull ? false : Convert.ToBoolean(sqlDataReader[10]);
                        item.CreatedBy = Convert.ToString(sqlDataReader[11]);
                        item.CreatedDate = sqlDataReader[12] is System.DBNull ? null : Convert.ToDateTime(sqlDataReader[12]);
                        item.ModifyBy = Convert.ToString(sqlDataReader[13]);
                        item.ModifyDate = sqlDataReader[14] is System.DBNull ? null : Convert.ToDateTime(sqlDataReader[14]);
                        result.Add(item);
                    }
                }
                else
                {
                    _logger.LogError("No se encontraron resultados, sp: [dbo].[SP_Mongo_Consulta_Errores_Registro_Auditar_Medicion]" + ",idmedicion :" + id);
                }
                sqlConnection.Close();
            }
            catch (Exception e)
            {
                sqlConnection.Close();
                _logger.LogError(e.Message);
            }
            return result;
        }



        public List<VariablesAsociadasDto> ConsultaVariablesAsociadaMedicion(int id)
        {
            List<VariablesAsociadasDto> result = new();
            try
            {
                sqlCommand = new SqlCommand("[dbo].[SP_Mongo_Consulta_Variables_Asociada_Medicion]", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@IdMedicion", id);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 99999999;
                sqlConnection.Open();
                sqlDataReader = sqlCommand.ExecuteReader();
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        VariablesAsociadasDto item = new() { };

                        item.VariableId = Convert.ToInt32(sqlDataReader[0]);
                        item.MedicionId = Convert.ToInt32(sqlDataReader[1]);
                        item.VariableAssociated = Convert.ToInt32(sqlDataReader[2]);
                        item.Nombre = Convert.ToString(sqlDataReader[3]);
                        item.Nemonico = Convert.ToString(sqlDataReader[4]);
                        item.Status = Convert.ToBoolean(sqlDataReader[5]);
                        item.CreatedBy = Convert.ToString(sqlDataReader[6]);
                        item.CreatedDate = sqlDataReader[7] is System.DBNull ? null : Convert.ToDateTime(sqlDataReader[7]);
                        item.ModifyBy = Convert.ToString(sqlDataReader[8]);
                        item.ModifyDate = sqlDataReader[9] is System.DBNull ? null : Convert.ToDateTime(sqlDataReader[9]);
                        result.Add(item);
                    }
                }
                else
                {
                    _logger.LogError("No se encontraron resultados, sp: [dbo].[SP_Mongo_Consulta_Variables_Asociada_Medicion]]" + ",idmedicion :" + id);
                }
                sqlConnection.Close();
            }
            catch (Exception e)
            {
                sqlConnection.Close();
                _logger.LogError(e.Message);
            }
            return result;
        }


        public List<ComodinVariableDto> ConsultaComodinesMedicion(int id)
        {
            List<ComodinVariableDto> result = new();
            try
            {
                sqlCommand = new SqlCommand("[dbo].[SP_Mongo_Consulta_Comodines_Medicion]", sqlConnection);
                sqlCommand.Parameters.AddWithValue("@IdMedicion", id);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 99999999;
                sqlConnection.Open();
                sqlDataReader = sqlCommand.ExecuteReader();
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        ComodinVariableDto item = new() { };

                        item.ComodinId = Convert.ToInt32(sqlDataReader[0]);
                        item.VariableId = Convert.ToInt32(sqlDataReader[1]);
                        item.MedicionId = Convert.ToInt32(sqlDataReader[2]);
                        item.Comodin = Convert.ToString(sqlDataReader[3]);
                        item.Status = Convert.ToBoolean(sqlDataReader[4]);
                        item.CreatedBy = Convert.ToString(sqlDataReader[5]);
                        item.CreatedDate = sqlDataReader[6] is System.DBNull ? null : Convert.ToDateTime(sqlDataReader[6]);
                        item.ModifyBy = Convert.ToString(sqlDataReader[7]);
                        item.ModifyDate = sqlDataReader[8] is System.DBNull ? null : Convert.ToDateTime(sqlDataReader[8]);
                        result.Add(item);
                    }
                }
                else
                {
                    _logger.LogError("No se encontraron resultados, sp: [dbo].[SP_Mongo_Consulta_Comodines_Medicion]" + ",idmedicion :" + id);
                }
                sqlConnection.Close();
            }
            catch (Exception e)
            {
                sqlConnection.Close();
                _logger.LogError(e.Message);
            }
            return result;
        }

        public List<UsuarioDto> ConsultaUsuarios()
        {
            List<UsuarioDto> result = new();
            try
            {
                sqlCommand = new SqlCommand("[AUTH].[SP_Consulta_Usuarios]", sqlConnection);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                sqlCommand.CommandTimeout = 99999999;
                sqlConnection.Open();
                sqlDataReader = sqlCommand.ExecuteReader();
                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        UsuarioDto item = new() { };

                        item.UserId = Convert.ToString(sqlDataReader[0]);
                        item.Usuario = Convert.ToString(sqlDataReader[1]);
                        item.Email = Convert.ToString(sqlDataReader[2]);
                        item.Password = Convert.ToString(sqlDataReader[3]);
                        item.Telefono = Convert.ToString(sqlDataReader[4]);
                        item.RolId = Convert.ToInt32(sqlDataReader[5]);
                        item.NormalizedName = Convert.ToString(sqlDataReader[6]);
                        item.Codigo = Convert.ToInt32(sqlDataReader[7]);
                        item.Nombre = Convert.ToString(sqlDataReader[8]);
                        item.Apellidos = Convert.ToString(sqlDataReader[9]);
                        item.Estado = Convert.ToInt32(sqlDataReader[10]);
                        item.NombreEstado = Convert.ToString(sqlDataReader[11]);
                        item.CreatedDate = sqlDataReader[12] is System.DBNull ? null : Convert.ToDateTime(sqlDataReader[12]);
                        item.ModifyDate = sqlDataReader[13] is System.DBNull ? null : Convert.ToDateTime(sqlDataReader[13]);
                        item.ModifyPasswordDate = sqlDataReader[14] is System.DBNull ? null : Convert.ToDateTime(sqlDataReader[14]);
                        item.Enable = Convert.ToBoolean(sqlDataReader[15]);
                        result.Add(item);
                    }
                }
                else
                {
                    _logger.LogError("No se encontraron resultados, sp: [dbo].[SP_Consulta_Usuarios]");
                }
                sqlConnection.Close();
            }
            catch (Exception e)
            {
                sqlConnection.Close();
                _logger.LogError(e.Message);
            }
            return result;
        }
    }
}
