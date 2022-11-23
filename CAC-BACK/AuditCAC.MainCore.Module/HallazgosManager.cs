using AuditCAC.Dal.Data;
using AuditCAC.Domain.Dto;
using AuditCAC.Domain.Entities;
using AuditCAC.Domain.Helpers;
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
    public class HallazgosManager : IHallazgosRepository<HallazgosModel>    
    {
        private readonly DBAuditCACContext _dBAuditCACContext;
        private readonly ILogger<HallazgosModel> _logger;
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //Constructor
        public HallazgosManager(DBAuditCACContext dBAuditCACContext, ILogger<HallazgosModel> logger)
        {
            this._dBAuditCACContext = dBAuditCACContext;
            _logger = logger;
        }


        /// <summary>
        /// Consutar Hallazgos por id de radicado
        /// </summary>
        /// <param name="inputsDto">Id radicado</param>
        /// <returns>MOdelo Hallazgos</returns>
        public async Task<List<ResponseConsultarHallazgosDto>> ConsultarHallazgosByRadicadoId(InputsConsultarHallazgosDto inputsDto)
        {
            try
            {
                string sql = "EXEC [dbo].[SP_Consultar_Hallazgos_RegistrosAuditoria] @IdRadicado";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters   
                    new SqlParameter { ParameterName = "@IdRadicado", Value = inputsDto.IdRadicado},
                };

                // var Data = _dBAuditCACContext.Database.ExecuteSqlRaw(sql, parms.ToArray());                
                var Data = await _dBAuditCACContext.ResponseConsultarHallazgosDto.FromSqlRaw<ResponseConsultarHallazgosDto>(sql, parms.ToArray()).ToListAsync();

                return Data;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Consulta de registros con hallazgos pendientes para enviar a la entidad por Id de Medicion (Suministro para implementacion de Modulo de hallazgos)
        /// </summary>
        /// <param name="inputsDto">(Medicion y Rango de Fechas)</param>
        /// <returns>Data para enviar a la entidad</returns>
        public async Task<List<ResponseConsultaHallazgosGeneradosDto>> ConsultaHallazgosGenerados(InputsConsultaHallazgosGeneradosDto inputsDto)
        {
            try
            {
                string sql = "EXEC [dbo].[SP_Consulta_Hallazgos_Generados] @MedicionId, @FechaInicial, @FechaFinal ";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    new SqlParameter { ParameterName = "@MedicionId", Value = inputsDto.MedicionId },
                    new SqlParameter { ParameterName = "@FechaInicial", Value = inputsDto.FechaInicial },
                    new SqlParameter { ParameterName = "@FechaFinal", Value = inputsDto.FechaFinal },
                };

                var timeout = _dBAuditCACContext.ParametrosGenerales.Where(x => x.Id == (int)Enumeration.Parametros.TimeOutCarguePoblacion).Select(x => x.Valor).FirstOrDefault();
                _dBAuditCACContext.Database.SetCommandTimeout(Convert.ToInt32(timeout));

                var Data = await _dBAuditCACContext.ResponseConsultarVariablesHallazgosDto.FromSqlRaw<ResponseConsultaHallazgosGeneradosDto>(sql, parms.ToArray()).ToListAsync();

                return Data;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Registra respuestas de las entidades sobre los hallazgos
        /// </summary>
        /// <param name="inputsDto">RegistroAuditoriaId, Estado, Observacion, Usuario</param>
        /// <returns>true</returns>
        public async Task<bool> RegistraRespuestaHallazgos(List<InputRegistraRespuestaHallazgosDto> inputsDto)
        {
            try
            {

                string sql = "DECLARE @TestData DT_Registrar_Respuesta_Hallazgos ";


                inputsDto.All(x =>
                {

                    sql += "INSERT INTO @TestData VALUES(" + x.RegistroAuditoriaDetalleId + "," + x.Estado + ", '" + x.Observacion + "', '" + x.Usuario + "') ";
                    return true;
                });


                sql += " EXEC [dbo].[SP_Registra_Respuesta_Hallazgos]  @InputData =  @TestData";

                var timeout = _dBAuditCACContext.ParametrosGenerales.Where(x => x.Id == (int)Enumeration.Parametros.TimeOutCarguePoblacion).Select(x => x.Valor).FirstOrDefault();
                _dBAuditCACContext.Database.SetCommandTimeout(Convert.ToInt32(timeout));

                var insertData = await _dBAuditCACContext.Database.ExecuteSqlRawAsync(sql);

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }
    }
}
