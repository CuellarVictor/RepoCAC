using AuditCAC.Dal.Data;
using AuditCAC.Domain.Dto.BolsasMedicion;
using AuditCAC.Domain.Helpers;
using AuditCAC.MainCore.Module.BolsasDeMedicion.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AuditCAC.Domain.Entities;
using AuditCAC.MainCore.Module.Interface;
using AuditCAC.Domain.Dto;
using Microsoft.Extensions.Logging;

namespace AuditCAC.MainCore.Module.BolsasDeMedicion
{
    public class BolsasMedicionManager : IBolsasMedicionManager
    {
        #region Dependency
        private readonly DBAuditCACContext _dBAuditCACContext;
        private readonly IMapper _mapper;
        private readonly IMedicionRepository<MedicionModel> _MedicionRepository;
        private readonly ILogger<MedicionModel> _logger;
        #endregion

        #region Constructor
        public BolsasMedicionManager(DBAuditCACContext dBAuditCACContext, IMapper mapper, IMedicionRepository<MedicionModel> med, ILogger<MedicionModel> logger)
        {
            _dBAuditCACContext = dBAuditCACContext;
            _mapper = mapper;
            _MedicionRepository = med;
            _logger = logger;
        }
        #endregion

        #region Methods
        public async Task<Tuple<List<BolsasMedicionDto>, int, int, int>>  GetRegistrosAuditoria(MedicionRequest model) //Task<List<BolsasMedicionDto>>
        {
            try
            {
                #region Actualizamos estamos de mediciones.                
                string sqlQueryMedicionId = "EXEC [dbo].[SP_Cambiar_Estados_BolsasMedicion] @IdLider, @IdCobertura, @IdEstado";

                List<SqlParameter> parmsQueryMedicionId = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@IdLider", Value = model.IdLider == null ? "": model.IdLider},
                    new SqlParameter { ParameterName = "@IdCobertura", Value = model.IdCobertura},
                    new SqlParameter { ParameterName = "@IdEstado", Value = String.Join(", ", model.IdEstado.ToArray())}
                };

                //var DataMedicionId = _dBAuditCACContext.ResponseValidacionEstadoBolsasMedicionDto.FromSqlRaw<ResponseValidacionEstadoBolsasMedicionDto>(sqlQueryMedicionId, parmsQueryMedicionId.ToArray()).ToList();
                var DataMedicionId = await _dBAuditCACContext.Database.ExecuteSqlRawAsync(sqlQueryMedicionId, parmsQueryMedicionId.ToArray());
                #endregion

                string sql = "EXEC [dbo].[GetDataMedicioneslider] @PageNumber, @MaxRows, @IdLider, @IdCobertura, @IdEstado";
                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters    
                    new SqlParameter { ParameterName = "@PageNumber", Value = model.PageNumber},
                    new SqlParameter { ParameterName = "@MaxRows", Value = model.maxRows},
                    new SqlParameter { ParameterName = "@IdLider", Value = model.IdLider == null ? "": model.IdLider},
                    new SqlParameter { ParameterName = "@IdCobertura", Value = model.IdCobertura},
                    new SqlParameter { ParameterName = "@IdEstado", Value = String.Join(", ", model.IdEstado.ToArray())}
                };
                var Data = await _dBAuditCACContext.BolsasMedicion.FromSqlRaw<BolsasMedicionDto>(sql, parms.ToArray()).ToListAsync();

                //Validamos si traemos datos y capturamos valores para consultas adicionales.
                var Query = "";
                if (Data.Count > 0)
                {
                    Query = Data.FirstOrDefault().QueryNoRegistrosTotales;
                }

                #region OBSOLETO Consultamos EstadoMedicionId y EstadoMedicionNombre, luego lo reemplazamos en consulta principal.
                //Consultamos Estados de BolsasMedicion
                //int MedicionId;
                //if (Data.Count > 0)
                //{
                //    foreach (var item in Data)
                //    {
                //        MedicionId = item.IdMedicion;

                //        ////Llamamos Procedure que calcula NoRegistrosTotalesFiltrado
                //        //string sqlQueryMedicionId = "EXEC [dbo].[SP_Validacion_Estado_BolsasMedicion] @MedicionId";

                //        //List<SqlParameter> parmsQueryMedicionId = new List<SqlParameter>
                //        //{
                //        //    new SqlParameter { ParameterName = "@MedicionId", Value = MedicionId},
                //        //};

                //        //var DataMedicionId = _dBAuditCACContext.ResponseValidacionEstadoBolsasMedicionDto.FromSqlRaw<ResponseValidacionEstadoBolsasMedicionDto>(sqlQueryMedicionId, parmsQueryMedicionId.ToArray()).ToList();

                //        string sqlQueryMedicionId = "EXEC [dbo].[SP_Validacion_Estado_BolsasMedicion] @MedicionId";

                //        List<SqlParameter> parmsQueryMedicionId = new List<SqlParameter>
                //        {
                //            new SqlParameter { ParameterName = "@MedicionId", Value = MedicionId},
                //        };

                //        var DataMedicionId = _dBAuditCACContext.ResponseValidacionEstadoBolsasMedicionDto.FromSqlRaw<ResponseValidacionEstadoBolsasMedicionDto>(sqlQueryMedicionId, parmsQueryMedicionId.ToArray()).ToList();

                //        if (DataMedicionId != null)
                //        {
                //            //Buscamos registro y reemplazamos el valor de EstadoMedicionId y EstadoMedicionNombre.
                //            Data.Where(x => x.IdMedicion == MedicionId).FirstOrDefault().IdEstadoAuditoria = DataMedicionId.FirstOrDefault().EstadoMedicionId;
                //            Data.Where(x => x.IdMedicion == MedicionId).FirstOrDefault().EstadoAuditoria = DataMedicionId.FirstOrDefault().EstadoMedicionNombre;
                //        }                        
                //    }                    
                //}
                // --
                #endregion

                //Llamamos Procedure que calcula NoRegistrosTotalesFiltrado
                string sqlQuery = "EXEC [dbo].[GetCountPaginator] @Query";

                List<SqlParameter> parmsQuery = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Query", Value = Query},
                };
                    
                var DataQuery = await _dBAuditCACContext.ResponseQueryPaginatorDto.FromSqlRaw<ResponseQueryPaginatorDto>(sqlQuery, parmsQuery.ToArray()).ToListAsync();

                //Obtenemos No Registros maximos a consultar y los agregamos al Header del Request (API).
                var Queryable = _dBAuditCACContext.MedicionModel.AsQueryable(); //BolsasMedicion
                var NoRegistrosTotales = Queryable.Count();

                var NoRegistrosTotalesFiltrado = DataQuery.FirstOrDefault().NoRegistrosTotalesFiltrado;

                var TotalPages = NoRegistrosTotalesFiltrado / model.maxRows;


                Data.Select(c => { c.QueryNoRegistrosTotales = ""; return c; }).ToList();

                return Tuple.Create(Data, NoRegistrosTotales, NoRegistrosTotalesFiltrado, TotalPages);
                //return Data;
            }
            catch (Exception e)
            {
                _logger.LogInformation(e.ToString());
                throw;
            }
        }

        public async Task<bool> AddOrUpdate(BolsaMedicionNuevaDto model)
        {

            try
            {
                var map = _mapper.Map<MedicionModel>(model);
                if (map.Id == 0)
                {
                    map.Status = true;
                    map.Estado = 31;
                    var res = await  _MedicionRepository.Add(map);               

                    string sql = "EXEC [SP_AsignaVariablesMedicion] @IdMedicion, @UserId";
                    List<SqlParameter> parms = new List<SqlParameter>
                    {
                        new SqlParameter { ParameterName = "@IdMedicion", Value = res},
                        new SqlParameter { ParameterName = "@UserId", Value = model.CreatedBy}
                    };

                    var respuesta = _dBAuditCACContext.Database.ExecuteSqlRaw(sql, parms.ToArray());
                    
                    return await Task.FromResult(res != 0 ? true : false);

                }
                else 
                {
                    var res = _MedicionRepository.Update(map, map);
                    return await Task.FromResult(res.Status != 0 ? true : false);
                }
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }
        #endregion
    }
}
