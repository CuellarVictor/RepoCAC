using AuditCAC.Domain.Dto;
using AuditCAC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuditCAC.MainCore.Module.Catalogo.Interfaces
{
    public interface ICatalogoCoberturaManager
    {
        /// <summary>
        /// Consulta todos los registros
        /// </summary>
        /// <returns>Todos los registros</returns>
        Task<List<CatalogoCoberturaDto>> GetAll();

        /// <summary>
        /// Consulta un registro segun su Id
        /// </summary>
        /// <param name="id">Id del registro</param>
        /// <returns>Un registro segun su Id</returns>
        Task<CatalogoCoberturaModel> GetById(int id);

        /// <summary>
        /// Crea un registro
        /// </summary>
        /// <param name="model">Modelo de datos</param>
        /// <returns>Registro creado</returns>
        Task<bool> Add(InputsCatalogoCoberturaDto model);

        /// <summary>
        /// Edita un registro
        /// </summary>
        /// <param name="dbEntity">Modelo de datos original</param>
        /// <param name="entity">Modelo de datos editado</param>
        /// <returns>Registro editado</returns>
        Task Update(CatalogoCoberturaModel dbEntity, InputsCatalogoCoberturaDto entity);

        /// <summary>
        /// Elimina un registro
        /// </summary>
        /// <param name="model">Modelo de datos</param>
        /// <returns>Registro elimiado</returns>
        Task<bool> Delete(int id, string UsuarioId);

        /// <summary>
        /// Para la consultar valores autocompletables en Catalogos coberturas, segun un valor de busqueda
        /// </summary>
        /// <param name="inputsDto">Entrada de datos</param>
        /// <returns>Modelo de datos</returns>
        Task<List<ResponseAutocompleteCatalogoCoberturaDto>> GetAutocompleteCatalogoCobertura(AutocompleteCatalogoCoberturaDto inputsDto);

        /// <summary>
        /// Para consultar todos los CatalogosCoberturas creados segun los filtrados ingresados.
        /// </summary>
        /// <param name="inputsDto">Entrada de datos</param>
        /// <returns>Modelo de datos</returns>
        Task<Tuple<List<ResponseCatalogoCoberturaDto>, int, int, int>> GetCatalogoCoberturaFiltrado(InputsCatalogoCoberturaFiltradoDto inputsDto);
    }
}
