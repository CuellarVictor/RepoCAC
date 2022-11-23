using AuditCAC.Dal.Data;
using AuditCAC.Domain.Dto;
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

namespace AuditCAC.MainCore.Module
{
    public class transferenciaRegistrosManager : ITransferenciaRegistrosRepository<NemonicoPascientesConsultaModel>
    {
        private readonly DBAuditCACContext _dBAuditCACContext;
        private readonly DBCACContext _dBCACContext;
        private readonly ILogger<NemonicoPascientesConsultaModel> _logger;

        public transferenciaRegistrosManager(DBAuditCACContext dBAuditCACContext, DBCACContext _dBCACContext, ILogger<NemonicoPascientesConsultaModel> logger)
        {
            this._dBAuditCACContext = dBAuditCACContext;
            this._dBCACContext = _dBCACContext;
            _logger = logger;
        }

        public async Task<object>ConsultarRegistrosAuditoriaPascientes(InputsMoverRegistrosAuditoriaPascientesDto inputs)
        {
            try
            {                
                int countPascientes = 0;
                int countIteraccion = 0;
                string idPasciente = "";
                List<object> totalInsersion = new List<object>();

                foreach (var item in inputs.Id)
                {
                    countPascientes++;
                    countIteraccion++;
                    idPasciente = idPasciente + ',' + item;
                   
                    if (countPascientes == 500 && idPasciente.Length <= 7900 && countIteraccion < inputs.Id.Length) {
                        idPasciente = idPasciente.Substring(1);
                        string sql = "EXEC [dbo].[consultaPascientes] @Id, @IdMedicion";
                        List<SqlParameter> parms = new List<SqlParameter>
                            {
                                new SqlParameter { ParameterName = "@Id", Value = idPasciente},
                                new SqlParameter { ParameterName = "@IdMedicion", Value = inputs.IdMedicion}
                            };
                        var getData = await _dBCACContext.NemonicoPascientesConsultaModel.FromSqlRaw(sql, parms.ToArray()).ToListAsync();
                        totalInsersion.AddRange(getData);
                        countPascientes = 0;
                        idPasciente = "";
                    } else if(countIteraccion == inputs.Id.Length){   
                         idPasciente = idPasciente.Substring(1);
                    string sql = "EXEC [dbo].[consultaPascientes] @Id, @IdMedicion";
                    List<SqlParameter> parms = new List<SqlParameter>
                            {
                                new SqlParameter { ParameterName = "@Id", Value = idPasciente},
                                new SqlParameter { ParameterName = "@IdMedicion", Value = inputs.IdMedicion}
                            };
                        var getData = await _dBCACContext.NemonicoPascientesConsultaModel.FromSqlRaw(sql, parms.ToArray()).ToListAsync();
                        totalInsersion.AddRange(getData);
                    }
                }

                return totalInsersion;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        public async Task InsertarRegistrosAuditoriaPascientes(List<NemonicoPascientesConsultaModel> inputs)
        {
            try
            {
                string sql = "EXEC [dbo].[InsertarPascientes] @Pascientes";
                var Parameter = new SqlParameter("@Pascientes", SqlDbType.Structured);

                #region Datos de la tabla
                DataTable ListData = new DataTable();
                ListData.Columns.Add("Id", typeof(int));
                ListData.Columns.Add("IdRadicado", typeof(int));
                ListData.Columns.Add("IdMedicion", typeof(int));
                ListData.Columns.Add("IdPeriodo", typeof(int));
                ListData.Columns.Add("PrimerNombre", typeof(string));
                ListData.Columns.Add("SegundoNombre", typeof(string));
                ListData.Columns.Add("PrimerApellido", typeof(string));
                ListData.Columns.Add("SegundoApellido", typeof(string));
                ListData.Columns.Add("TipoIdentificacion", typeof(string));//varchar(2)
                ListData.Columns.Add("Identificacion", typeof(int));
                ListData.Columns.Add("FechaNacimiento", typeof(string));
                ListData.Columns.Add("FechaCreacion", typeof(string));
                ListData.Columns.Add("FechaAuditoria", typeof(string));
                ListData.Columns.Add("Activo", typeof(bool));
                ListData.Columns.Add("Conclusion", typeof(string));
                ListData.Columns.Add("UrlSoportes", typeof(string));
                ListData.Columns.Add("Reverse", typeof(bool));
                ListData.Columns.Add("DisplayOrder", typeof(int));
                ListData.Columns.Add("Ara", typeof(bool));
                ListData.Columns.Add("Eps", typeof(bool));
                ListData.Columns.Add("FechaReverso", typeof(string));
                ListData.Columns.Add("AraAtendido", typeof(bool));
                ListData.Columns.Add("EpsAtendido", typeof(bool));
                ListData.Columns.Add("Revisar", typeof(bool));
                ListData.Columns.Add("Estado", typeof(int));
                ListData.Columns.Add("CreatedBy", typeof(int));
                ListData.Columns.Add("CreatedDate", typeof(string));
                ListData.Columns.Add("ModifyBy", typeof(int));
                ListData.Columns.Add("ModifyDate", typeof(string));
                #endregion

                foreach (var item in inputs)
                {
                    ListData.Rows.Add(item.id, 2, 3, item.idPeriodo, item.PrimerNombre, item.SegundoNombre, item.PrimerApellido, item.SegundoApellido, item.TipoIdentificacion, item.Identificacion, "2021-09-23T22:26:46.357Z", "2021-09-23T22:26:46.357Z", "2021-09-23T22:26:46.357Z", 1, 1, "llllllllll", 0, 0, 0, 0, "2021-09-23T22:26:46.357Z", 1, 1, 1, item.estado, 0, "2021-09-23T22:26:46.357Z", 0, "2021-09-23T22:26:46.357Z");
                }
                Parameter.Value = ListData;
                Parameter.TypeName = "dbo.DT_Pascientes";                
                var insertData = await _dBAuditCACContext.Database.ExecuteSqlRawAsync(sql, Parameter);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }           
        }
    }
}
