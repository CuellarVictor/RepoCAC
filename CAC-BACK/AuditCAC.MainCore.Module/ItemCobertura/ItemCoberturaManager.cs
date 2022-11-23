using AuditCAC.Dal.Data;
using AuditCAC.Domain.Dto;
using AuditCAC.Domain.Entities;
using AuditCAC.MainCore.Module.ItemCobertura.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.MainCore.Module.ItemCobertura
{
    public class ItemCoberturaManager : IItemCoberturaRepository<CatalogoItemCobertura>
    {
        private readonly DBAuditCACContext _dBAuditCACContext;        
        private readonly ILogger<CatalogoItemCobertura> _logger;

        public ItemCoberturaManager(DBAuditCACContext dBAuditCACContext, ILogger<CatalogoItemCobertura> logger)
        {
            this._dBAuditCACContext = dBAuditCACContext;
            _logger = logger;
        }


        #region Metodos.

        ///// <summary>
        ///// Consulta todos los registros
        ///// </summary>
        ///// <returns>Todos los registros</returns>
        //public async Task<List<CatalogoItemCobertura>> GetAll()
        //{
        //    try
        //    {
        //        return await _dBAuditCACContext.CatalogoItemCobertura.Where(x => x.ItemActivo == true).ToListAsync();
        //        //return await this.dBAuditCACContext.CatalogoItemCobertura.ToListAsync();
        //    }
        //    catch (Exception Ex)
        //    {
        //        _logger.LogInformation(Ex.ToString());
        //        throw;
        //    }
        //}

        ///// <summary>
        ///// Consulta un registro segun su Id
        ///// </summary>
        ///// <param name="id">Id del registro</param>
        ///// <returns>Un registro segun su Id</returns>
        //public async Task<CatalogoItemCobertura> Get(int id)
        //{
        //    try
        //    {
        //        return await _dBAuditCACContext.CatalogoItemCobertura.Where(x => x.ItemActivo == true).FirstOrDefaultAsync(x => x.Id == id);
        //    }
        //    catch (Exception Ex)
        //    {
        //        _logger.LogInformation(Ex.ToString());
        //        throw;
        //    }
        //}

        ///// <summary>
        ///// Consulta un registro segun su Id de catalogo (CatalogoCoberturaId)
        ///// </summary>
        ///// <param name="CatalogoCoberturaId">Id del catalogo cobertura</param>
        ///// <returns>Un registro segun su CatalogoCoberturaId</returns>
        //public async Task<List<CatalogoItemCobertura>> GetItemByCatalogoCoberturaId(int CatalogoCoberturaId)
        //{
        //    try
        //    {
        //        return await _dBAuditCACContext.CatalogoItemCobertura.Where(x => x.ItemActivo == true).Where(x => x.CatalogoCoberturaId == CatalogoCoberturaId).ToListAsync();
        //    }
        //    catch (Exception Ex)
        //    {
        //        _logger.LogInformation(Ex.ToString());
        //        throw;
        //    }
        //}

        public IEnumerable<CatalogoItemCobertura> GetAll()
        {
            try
            {
                var Data = _dBAuditCACContext.CatalogoItemCobertura.Where(x => x.ItemActivo == true).ToList();
                return Data;
                //return await this.dBAuditCACContext.ItemModel.ToListAsync();
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        public CatalogoItemCobertura Get(int id)
        {
            try
            {
                var Data = _dBAuditCACContext.CatalogoItemCobertura.Where(x => x.Id == id).FirstOrDefault();
                return Data;
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        public IEnumerable<CatalogoItemCobertura> GetItemByCatalogoCoberturaId(int CatalogoCoberturaId)
        {
            try
            {
                var Data = _dBAuditCACContext.CatalogoItemCobertura.Where(x => x.ItemActivo == true).Where(x => x.CatalogoCoberturaId == CatalogoCoberturaId).ToList();
                return Data;
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
        public async Task Add(CatalogoItemCobertura entity)
        {
            try
            {
                _dBAuditCACContext.CatalogoItemCobertura.Add(entity);
                await _dBAuditCACContext.SaveChangesAsync();
                //return NoContent;
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
        public async Task Update(CatalogoItemCobertura dbEntity, CatalogoItemCobertura entity)
        {
            try
            {
                //dbEntity.Id = entity.Id;
                dbEntity.CatalogoCoberturaId = entity.CatalogoCoberturaId; //CatalogoCoberturaId
                dbEntity.ItemId = entity.ItemId;
                dbEntity.ItemDescripcion = entity.ItemDescripcion;
                dbEntity.ItemActivo = entity.ItemActivo;
                dbEntity.ItemOrden = entity.ItemOrden;
                dbEntity.ItemGlosa = entity.ItemGlosa;
                dbEntity.CreatedBy = entity.CreatedBy;
                dbEntity.CreationDate = entity.CreationDate;
                dbEntity.ModifyBy = entity.ModifyBy;
                dbEntity.ModificationDate = entity.ModificationDate;

                await _dBAuditCACContext.SaveChangesAsync();
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
        public async Task Delete(CatalogoItemCobertura entity)
        {
            try
            {
                //_dBAuditCACContext.CatalogoItemCobertura.Remove(entity);
                //await _dBAuditCACContext.SaveChangesAsync();

                entity.ItemActivo = false;

                await _dBAuditCACContext.SaveChangesAsync();
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Para consultar todos los CatalogosItemCoberturas creados segun los filtrados ingresados.
        /// </summary>
        /// <param name="inputsDto">Entrada de datos</param>
        /// <returns>Modelo de datos</returns>
        public Tuple<List<ResponseCatalogoItemCoberturaDto>, int, int, int> GetCatalogoItemCoberturaFiltrado(InputsCatalogoItemCoberturaFiltradoDto inputsDto)
        {
            try
            {                
                string sql = "EXEC SP_Consulta_CatalogoItemCobertura_Filtrado @PageNumber, @MaxRows, @Id, @CatalogoCoberturaId, @ItemId, @ItemDescripcion, @ItemActivo, @ItemOrden, @ItemGlosa";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters    
                    new SqlParameter { ParameterName = "@PageNumber", Value = inputsDto.PageNumber},
                    new SqlParameter { ParameterName = "@MaxRows", Value = inputsDto.MaxRows},
                    //
                    new SqlParameter { ParameterName = "@Id", Value = inputsDto.Id},
                    new SqlParameter { ParameterName = "@CatalogoCoberturaId", Value = inputsDto.CatalogoCoberturaId},
                    new SqlParameter { ParameterName = "@ItemId", Value = inputsDto.ItemId},
                    new SqlParameter { ParameterName = "@ItemDescripcion", Value = inputsDto.ItemDescripcion},
                    new SqlParameter { ParameterName = "@ItemActivo", Value = inputsDto.ItemActivo},
                    new SqlParameter { ParameterName = "@ItemOrden", Value = inputsDto.ItemOrden},
                    new SqlParameter { ParameterName = "@ItemGlosa", Value = inputsDto.ItemGlosa},
                };

                var Data = _dBAuditCACContext.ResponseCatalogoItemCoberturaDto.FromSqlRaw<ResponseCatalogoItemCoberturaDto>(sql, parms.ToArray()).ToList();

                //Obtenemos No Registros maximos a consultar y los agregamos al Header del Request (API).
                var Queryable = _dBAuditCACContext.CatalogoItemCobertura.AsQueryable();
                var NoRegistrosTotales = Queryable.Count();

                var NoRegistrosTotalesFiltrado = Data.FirstOrDefault().NoRegistrosTotales;

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
