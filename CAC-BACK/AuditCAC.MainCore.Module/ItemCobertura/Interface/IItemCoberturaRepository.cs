using AuditCAC.Domain.Dto;
using AuditCAC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.MainCore.Module.ItemCobertura.Interface
{
    public interface IItemCoberturaRepository<TEntity>
    {
        /// <summary>
        /// Consulta todos los registros
        /// </summary>
        /// <returns>Todos los registros</returns>
        //Task<List<CatalogoItemCobertura>> GetAll();
        IEnumerable<CatalogoItemCobertura> GetAll();

        /// <summary>
        /// Consulta un registro segun su Id
        /// </summary>
        /// <param name="id">Id del registro</param>
        /// <returns>Un registro segun su Id</returns>
        CatalogoItemCobertura Get(int id);

        /// <summary>
        /// Consulta un registro segun su Id de catalogo (CatalogoCoberturaId)
        /// </summary>
        /// <param name="CatalogoCoberturaId">Id del catalogo cobertura</param>
        /// <returns>Un registro segun su CatalogoCoberturaId</returns>
        //Task<List<CatalogoItemCobertura>> GetItemByCatalogoCoberturaId(int CatalogoCoberturaId);
        IEnumerable<CatalogoItemCobertura> GetItemByCatalogoCoberturaId(int CatalogoCoberturaId);

        /// <summary>
        /// Crea un registro
        /// </summary>
        /// <param name="model">Modelo de datos</param>
        /// <returns>Registro creado</returns>
        Task Add(CatalogoItemCobertura entity);

        /// <summary>
        /// Edita un registro
        /// </summary>
        /// <param name="dbEntity">Modelo de datos original</param>
        /// <param name="entity">Modelo de datos editado</param>
        /// <returns>Registro editado</returns>
        Task Update(CatalogoItemCobertura dbEntity, CatalogoItemCobertura entity);

        /// <summary>
        /// Elimina un registro
        /// </summary>
        /// <param name="model">Modelo de datos</param>
        /// <returns>Registro elimiado</returns>
        Task Delete(CatalogoItemCobertura entity);

        /// <summary>
        /// Para consultar todos los CatalogosItemCoberturas creados segun los filtrados ingresados.
        /// </summary>
        /// <param name="inputsDto">Entrada de datos</param>
        /// <returns>Modelo de datos</returns>
        Tuple<List<ResponseCatalogoItemCoberturaDto>, int, int, int> GetCatalogoItemCoberturaFiltrado(InputsCatalogoItemCoberturaFiltradoDto inputsDto);
    }
}
