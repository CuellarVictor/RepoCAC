using AuditCAC.Domain.Dto;
using AuditCAC.Domain.Entities;
using AuditCAC.MainCore.Module.Catalogo.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AuditCAC.Services.Controllers.CatalogoCobertura
{
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class CatalogoCoberturaController : ControllerBase
    {
        #region Dependency
        private readonly ICatalogoCoberturaManager _catalogo;
        //private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly ILogger<CatalogoCoberturaModel> _logger;
        #endregion

        #region Constructor
        public CatalogoCoberturaController(ICatalogoCoberturaManager catalogo, ILogger<CatalogoCoberturaModel> logger)
        {
            _catalogo = catalogo;
            _logger = logger;
        }
        #endregion

        #region Methods
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        /// 
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> Get()
        {
            try
            {
                //Obtenemos todos los registros.
                var Data = await _catalogo.GetAll();

                //Retornamos datos.
                return Ok(Data);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Id del registro</param>
        /// <returns></returns>
        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                //Obtenemos registro buscado
                var Data = await _catalogo.GetById(id);

                //Validamos si el registro existe.
                if (Data == null)
                {
                    return NotFound("The record couldn't be found.");
                }

                //Retornamos datos.
                return Ok(Data);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="entity">Modelo de datos</param>
        /// <returns></returns>
        [HttpPost]
        [Route("Post")]
        public async Task<IActionResult> Post([FromBody] InputsCatalogoCoberturaDto entity)
        {
            //Validamos si recibimos una entidad.
            if (entity == null)
            {
                return BadRequest("The record is null.");
            }

            //Registramos.
            var res = await _catalogo.Add(entity);

            //Retornamos ruta de registro creado.
            return res != false ? Ok(true) : (IActionResult)(NoContent());

        }

        /// <summary>
        /// Edita un registro
        /// </summary>
        /// <param name="id">Id del registro</param>
        /// <param name="entity">Modelo de datos editado</param>
        /// <returns>Registro editado</returns>
        [HttpPost]
        [Route("Update/{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] InputsCatalogoCoberturaDto entity)
        {
            //Validamos si recibimos una entidad.
            if (entity == null)
            {
                return BadRequest("The record is null.");
            }

            //Buscamos y validamos que exista el registro buscado.
            CatalogoCoberturaModel ToUpdate = await _catalogo.GetById(id);
            if (ToUpdate == null)
            {
                return NotFound("The record couldn't be found.");
            }

            //Actualizamos el registro.
            await _catalogo.Update(ToUpdate, entity);

            //Retornamos sin contenido.
            return NoContent();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="id">Id del registro</param>
        /// <returns></returns>
        //[HttpDelete]
        [HttpGet]
        [Route("Delete/{id}")]
        public async Task<IActionResult> Delete(int id, string UsuarioId)
        {
            try
            {
                //Obtenemos registro buscado
                var Data = await _catalogo.GetById(id);

                //Validamos si el registro existe.
                if (Data == null)
                {
                    return NotFound("The record couldn't be found.");
                }

                //Eliminamos el registro.
                await _catalogo.Delete(id, UsuarioId); //await _catalogo.Delete(Data);

                //Retornamos sin contenido.
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        /// <summary>
        /// Para la consultar valores autocompletables en Catalogos coberturas, segun un valor de busqueda
        /// </summary>
        /// <param name="inputsDto">Entrada de datos</param>
        /// <returns>Modelo de datos</returns>
        [HttpPost]
        [Route("GetAutocompleteCatalogoCobertura")]
        public async Task<List<ResponseAutocompleteCatalogoCoberturaDto>> GetAutocompleteCatalogoCobertura([FromBody] AutocompleteCatalogoCoberturaDto inputsDto)
        {
            try
            {
                var result = await _catalogo.GetAutocompleteCatalogoCobertura(inputsDto);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        /// <summary>
        /// Para consultar todos los CatalogosCoberturas creados segun los filtrados ingresados.
        /// </summary>
        /// <param name="inputsDto">Entrada de datos</param>
        /// <returns>Modelo de datos</returns>
        [HttpPost]
        [Route("GetCatalogoCoberturaFiltrado")]
        public async Task<IActionResult> GetCatalogoCoberturaFiltrado([FromBody] InputsCatalogoCoberturaFiltradoDto inputsDto)
        {
            try
            {
                var Data = await _catalogo.GetCatalogoCoberturaFiltrado(inputsDto);

                var PaginationDto = new PaginationDto();
                PaginationDto.Data = Data.Item1;
                PaginationDto.NoRegistrosTotales = Data.Item2;
                PaginationDto.NoRegistrosTotalesFiltrado = Data.Item3;
                PaginationDto.TotalPages = Data.Item4;

                //return Ok(Data.Item1);
                return Ok(PaginationDto);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }
        #endregion
    }
}
