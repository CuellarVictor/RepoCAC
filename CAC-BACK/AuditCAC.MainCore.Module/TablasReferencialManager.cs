using AuditCAC.Dal.Data;
using AuditCAC.Domain.Dto;
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
    public class TablasReferencialManager : ITablasReferencialRepository<ResponseGetTablasReferencial>
    {
        private readonly DBCACContext dBCACContext;
        private readonly ILogger<ResponseGetTablasReferencial> _logger;

        public TablasReferencialManager(DBCACContext dBCACContext, ILogger<ResponseGetTablasReferencial> logger)
        {
            this.dBCACContext = dBCACContext;
            _logger = logger;
        }

        //Metodo para llamar procedure GetTablasReferencial - Para cargar listados, proviene de CAC.
        public async Task<List<ResponseGetTablasReferencial>> GetTablasReferencial() //InputsGetTablasReferencialDto inputsDto
        {
            try
            {
                string sql = "EXEC [dbo].[GetTablasReferencial] @MaximosRegistros";

                //Obtenemos valor de MaximosRegistros.
                var MaximosRegistros = 50;

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters   
                    new SqlParameter { ParameterName = "@MaximosRegistros", Value = MaximosRegistros}, //inputsDto.MaximosRegistros
                };

                var Data = await dBCACContext.ResponseGetTablasReferencialDto.FromSqlRaw<ResponseGetTablasReferencialDto>(sql, parms.ToArray()).ToListAsync();

                //Armamos objeto a responder

                //Iniciamos un listado, del tipo que vamos a responder en el metodo.
                var Result = new List<ResponseGetTablasReferencial>();

                var Filtros = Data.GroupBy(x => new { x.TablaReferencial })
                  .Select(y => new ResponseGetTablasReferencial()
                  {
                      TablaReferencial = y.Key.TablaReferencial
                  });

                //var Filtros = Data.Distinct()
                //  .Select(y => new ResponseGetTablasReferencial()
                //  {
                //      TablaReferencial = y.TablaReferencial
                //  });

                Filtros?.All(x =>
                {
                    var row = new ResponseGetTablasReferencial();
                    row.TablaReferencial = x.TablaReferencial;
                    row.Contenido = new List<ListaContenido>();

                    Data.Where(d => d.TablaReferencial == x.TablaReferencial)?.All(z =>
                    {
                        row.Contenido.Add(new ListaContenido()
                        {
                            Id = z.Id,
                            Nombre = z.Nombre
                        });
                        return true;
                    });

                    Result.Add(row);

                    return true;
                });

                return Result;
                //return Data;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        //Metodo para llamar procedure - Insertar Variables masivas.
        public async Task<List<ResponseGetTablasReferencialDto>> GetTablasReferencialByValorReferencial(List<InputsTablasReferencialByValorReferencialDto> entity)
        {
            try
            {
                //Creamos Query para ejecutar procedure.
                string sql = "EXEC [dbo].[GetTablasReferencialByValorReferencial] @Listado";

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
                ListData.Columns.Add("TablaReferencial", typeof(string));
                ListData.Columns.Add("ValorReferencial", typeof(string));

                //
                foreach (var item in entity)
                {
                    ListData.Rows.Add(item.TablaReferencial, item.ValorReferencial);
                }

                //Agregamos valores a Parametros.
                Parameter.Value = ListData;
                Parameter.TypeName = "dbo.DT_TablasReferencia";

                //Ejecutamos procedimiento.ResponseGetTablasReferencialDto
                var Data = await dBCACContext.ResponseGetTablasReferencialDto.FromSqlRaw<ResponseGetTablasReferencialDto>(sql, Parameter).ToListAsync();

                return Data;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }
    }
}
