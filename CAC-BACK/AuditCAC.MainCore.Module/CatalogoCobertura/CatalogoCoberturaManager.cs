using AuditCAC.Dal.Data;
using AuditCAC.Domain.Dto;
using AuditCAC.Domain.Entities;
using AuditCAC.MainCore.Module.Catalogo.Interfaces;
using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuditCAC.MainCore.Module.Catalogo
{
    public class CatalogoCoberturaManager : ICatalogoCoberturaManager
    {
        #region Dependencias
        private readonly DBAuditCACContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<CatalogoCoberturaModel> _logger;
        #endregion

        #region Constructor
        public CatalogoCoberturaManager(DBAuditCACContext context, IMapper mapper, ILogger<CatalogoCoberturaModel> logger)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Consulta todos los registros
        /// </summary>
        /// <returns>Todos los registros</returns>
        public async Task<List<CatalogoCoberturaDto>> GetAll()
        {
            try
            {
                var result = await _context.CatalogoCobertura.Where(x => x.Activo == true).ToListAsync();
                var map = _mapper.Map<List<CatalogoCoberturaDto>>(result);
                return map;
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Consulta un registro segun su Id
        /// </summary>
        /// <param name="id">Id del registro</param>
        /// <returns>Un registro segun su Id</returns>
        public async Task<CatalogoCoberturaModel> GetById(int id)
        {
            try
            { 
                var result = await _context.CatalogoCobertura.Where(x => x.Id == id && x.Activo == true).FirstOrDefaultAsync();
                //var map = _mapper.Map<List<CatalogoCoberturaDto>>(result);
                return result;
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Crea un registro
        /// </summary>
        /// <param name="model">Modelo de datos</param>
        /// <returns>Registro creado</returns>
        public async Task<bool> Add(InputsCatalogoCoberturaDto model) //CatalogoCoberturaDto model
        {
            try 
            {

                //Nuevo con AutoMapper 1
                var map = _mapper.Map<CatalogoCoberturaModel>(model);
                map.Migrados = false;
                map.Sincronizar = false;
                var result = _context.CatalogoCobertura.Add(map);

                //Llamamos SP para Insertar Logs de actividad.                
                string sql = "EXEC [SP_Insertar_Process_Log] @ProcessId, @User, @Result, @Observation";
                List<SqlParameter> parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@ProcessId", Value = 32},
                    new SqlParameter { ParameterName = "@User", Value = model.CreatedBy},
                    new SqlParameter { ParameterName = "@Result", Value = "OK"},
                    new SqlParameter { ParameterName = "@Observation", Value = "Configuraciones - Catálogos (Catálogos): Nuevo"}
                };

                var respuesta = _context.Database.ExecuteSqlRaw(sql, parms.ToArray());

                if (result != null)
                {
                    await _context.SaveChangesAsync();
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Edita un registro
        /// </summary>
        /// <param name="dbEntity">Modelo de datos original</param>
        /// <param name="entity">Modelo de datos editado</param>
        /// <returns>Registro editado</returns>
        #region Obsoleto
        //public async Task<bool> Update(CatalogoCoberturaDto model)
        //{
        //    try
        //    { 
        //        var map = _mapper.Map<CatalogoCoberturaModel>(model);
        //        var result = _context.CatalogoCobertura.Update(map);
        //        if (result != null)
        //        {
        //            await _context.SaveChangesAsync();
        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //    catch (Exception Ex)
        //    {
        //        _logger.LogInformation(Ex.ToString());
        //        throw;
        //    }
        //}
        #endregion
        public async Task Update(CatalogoCoberturaModel dbEntity, InputsCatalogoCoberturaDto entity) //CatalogoCoberturaModel dbEntity, CatalogoCoberturaModel entity
        {
            try
            {
                //dbEntity.Id = entity.Id;
                dbEntity.NombreCatalogo = entity.NombreCatalogo;
                dbEntity.Activo = entity.Activo;
                dbEntity.CreatedBy = entity.CreatedBy;
                dbEntity.CreationDate = entity.CreationDate;
                dbEntity.ModifyBy = entity.ModifyBy;
                dbEntity.ModificationDate = entity.ModificationDate;

                await _context.SaveChangesAsync();

                //Llamamos SP para Insertar Logs de actividad.                
                string sql = "EXEC [SP_Insertar_Process_Log] @ProcessId, @User, @Result, @Observation";
                List<SqlParameter> parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@ProcessId", Value = 32},
                    new SqlParameter { ParameterName = "@User", Value = entity.CreatedBy},
                    new SqlParameter { ParameterName = "@Result", Value = "OK"},
                    new SqlParameter { ParameterName = "@Observation", Value = "Configuraciones - Catálogos (Catálogos): Editar"}
                };

                var respuesta = _context.Database.ExecuteSqlRaw(sql, parms.ToArray());
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Elimina un registro
        /// </summary>
        /// <param name="model">Modelo de datos</param>
        /// <returns>Registro elimiado</returns>
        public async Task<bool> Delete(int id, string UsuarioId)
        {
            try 
            {
                #region OBSOLETO
                //var entidad = _context.CatalogoCobertura.Where(x => x.Id == id).FirstOrDefault();
                //if (entidad != null)
                //{
                //    entidad.Activo = false;
                //    var result = _context.CatalogoCobertura.Update(entidad);
                //    await _context.SaveChangesAsync();
                //    return true;
                //}
                //else
                //{
                //    return false;
                //}
                #endregion

                //Variables
                bool Respuesta = false;

                //Validamos si existe el registro
                var entidad = _context.CatalogoCobertura.Where(x => x.Id == id).FirstOrDefault();
                if (entidad != null)
                {
                    string sql = "EXEC [dbo].[SP_Eliminar_CatalogoCobertura] @CatalogoCoberturaId";

                    List<SqlParameter> parms = new List<SqlParameter>
                    {
                        new SqlParameter { ParameterName = "@CatalogoCoberturaId", Value = id},
                    };

                    //Declaramos listado para capturar resultados del proceso.
                    var dataResult = new List<SPResponseDto>();

                    //var Data = await _dBAuditCACContext.Database.ExecuteSqlRawAsync(sql, parms.ToArray());
                    //var Data = await _context.SPResponseDto.FromSqlRaw<SPResponseDto>(sql, parms.ToArray()).FirstOrDefaultAsync();
                    dataResult = await _context.SPResponseDto.FromSqlRaw<SPResponseDto>(sql, parms.ToArray()).ToListAsync();
                    
                    if (dataResult.Where(x => x.Result == "OK").Any())
                    {
                        Respuesta = true;
                    }

                    //Llamamos SP para Insertar Logs de actividad.                
                    string sql2 = "EXEC [SP_Insertar_Process_Log] @ProcessId, @User, @Result, @Observation";
                    List<SqlParameter> parms2 = new List<SqlParameter>
                    {
                        new SqlParameter { ParameterName = "@ProcessId", Value = 32},
                        new SqlParameter { ParameterName = "@User", Value = UsuarioId},
                        new SqlParameter { ParameterName = "@Result", Value = "OK"},
                        new SqlParameter { ParameterName = "@Observation", Value = "Configuraciones - Catálogos (Catálogos): Eliminar"}
                    };

                    var respuesta = _context.Database.ExecuteSqlRaw(sql2, parms2.ToArray());

                    return Respuesta;
                }
                else
                {
                    return false;
                }                
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Para la consultar valores autocompletables en Catalogos coberturas, segun un valor de busqueda
        /// </summary>
        /// <param name="inputsDto">Entrada de datos</param>
        /// <returns>Modelo de datos</returns>
        public async Task<List<ResponseAutocompleteCatalogoCoberturaDto>> GetAutocompleteCatalogoCobertura(AutocompleteCatalogoCoberturaDto inputsDto)
        {
            try
            {
                string sql = "EXEC SP_Autocomplete_CatalogoCobertura @tablaReferencial, @campoReferencial, @valorBusqueda";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters   
                    new SqlParameter { ParameterName = "@tablaReferencial", Value = inputsDto.tablaReferencial},
                    new SqlParameter { ParameterName = "@campoReferencial", Value = inputsDto.campoReferencial},
                    new SqlParameter { ParameterName = "@valorBusqueda", Value = inputsDto.valorBusqueda},
                };

                var Data = await _context.ResponseAutocompleteCatalogoCoberturaDto.FromSqlRaw<ResponseAutocompleteCatalogoCoberturaDto>(sql, parms.ToArray()).ToListAsync();
                return Data;
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Para consultar todos los CatalogosCoberturas creados segun los filtrados ingresados.
        /// </summary>
        /// <param name="inputsDto">Entrada de datos</param>
        /// <returns>Modelo de datos</returns>
        public async Task<Tuple<List<ResponseCatalogoCoberturaDto>, int, int, int>> GetCatalogoCoberturaFiltrado(InputsCatalogoCoberturaFiltradoDto inputsDto)
        {
            try
            {
                string sql = "EXEC SP_Consulta_CatalogoCobertura_Filtrado @PageNumber, @MaxRows, @Id, @NombreCatalogo, @Migrados, @Sincronizar, @Activo";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters    
                    new SqlParameter { ParameterName = "@PageNumber", Value = inputsDto.PageNumber},
                    new SqlParameter { ParameterName = "@MaxRows", Value = inputsDto.MaxRows},
                    //
                    new SqlParameter { ParameterName = "@Id", Value = inputsDto.Id},
                    new SqlParameter { ParameterName = "@NombreCatalogo", Value = inputsDto.NombreCatalogo},
                    new SqlParameter { ParameterName = "@Migrados", Value = inputsDto.Migrados},
                    new SqlParameter { ParameterName = "@Sincronizar", Value = inputsDto.Sincronizar},
                    new SqlParameter { ParameterName = "@Activo", Value = inputsDto.Activo},
                };

                var Data = await _context.ResponseCatalogoCoberturaDto.FromSqlRaw<ResponseCatalogoCoberturaDto>(sql, parms.ToArray()).ToListAsync();

                //Obtenemos No Registros maximos a consultar y los agregamos al Header del Request (API).
                var Queryable = _context.CatalogoCobertura.AsQueryable();
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
        #endregion
    }
}
