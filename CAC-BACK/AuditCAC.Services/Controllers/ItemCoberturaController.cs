using AuditCAC.Domain.Dto;
using AuditCAC.Domain.Entities;
using AuditCAC.MainCore.Module.Interface;
using AuditCAC.MainCore.Module.ItemCobertura.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AuditCAC.Services.Controllers
{
    [Route("api/ItemCobertura")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ItemCoberturaController : ControllerBase
    {
        private readonly IItemCoberturaRepository<CatalogoItemCobertura> _repository;
        private readonly ILogger<CatalogoItemCobertura> _logger;

        public ItemCoberturaController(IItemCoberturaRepository<CatalogoItemCobertura> repository, ILogger<CatalogoItemCobertura> logger)
        {
            this._repository = repository;
            _logger = logger;
        }

        #region Metodos

        [HttpGet]
        [Route("GetAll")]
        public IActionResult Get()
        {
            try
            {
                //Obtenemos todos los registros.
                var Data = _repository.GetAll();
                //IEnumerable<CatalogoItemCobertura> Data = await _IRepository.GetAll();

                //Retornamos datos.
                return Ok(Data);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public IActionResult Get(int id)
        {
            try
            {
                //Obtenemos registro buscado
                //var Data = await _repository.GetById(id);
                var Data = _repository.Get(id);

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

        [HttpGet]
        [Route("GetItemByCatalogoCoberturaId/{CatalogId}")]
        public IActionResult GetItemByCatalogId(int CatalogId)
        {
            try
            {
                //Obtenemos registro buscado
                //var Data = await _repository.GetById(id);
                var Data = _repository.GetItemByCatalogoCoberturaId(CatalogId);

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

        [HttpPost]
        [Route("Create")]
        public async Task<IActionResult> Post([FromBody] CatalogoItemCobertura entity)
        {
            try
            {
                //Validamos si recibimos una entidad.
                if (entity == null)
                {
                    return BadRequest("The record is null.");
                }

                //Registramos.
                //await _repository.Insert(entity);
                await _repository.Add(entity);

                //Retornamos ruta de registro creado.
                //return CreatedAtRoute("Get", new { Id = entity.Id }, entity);
                return CreatedAtAction(nameof(Get), new { id = entity.Id }, entity);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        [HttpPost]
        [Route("Update")]
        public async Task<IActionResult> Update([FromBody] CatalogoItemCobertura entity)
        {
            try
            {
                //Validamos si recibimos una entidad.
                if (entity == null)
                {
                    return BadRequest("The record is null.");
                }

                //Buscamos y validamos que exista el registro buscado.
                //CatalogoItemCobertura ToUpdate = await _repository.GetById(id);
                CatalogoItemCobertura ToUpdate = _repository.Get(entity.Id);
                if (ToUpdate == null)
                {
                    return NotFound("The record couldn't be found.");
                }

                //Actualizamos el registro.
                //await _repository.Update(entity);
                await _repository.Update(ToUpdate, entity);

                //Retornamos sin contenido.
                return Ok(entity);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        //[HttpDelete]
        [HttpGet]
        [Route("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                //Obtenemos registro buscado
                //var Data = await _repository.GetById(id);
                var Data = _repository.Get(id);

                //Validamos si el registro existe.
                if (Data == null)
                {
                    return NotFound("The record couldn't be found.");
                }

                //Eliminamos el registro.
                //await _repository.Delete(id);
                await _repository.Delete(Data);

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
        /// Para consultar todos los CatalogosItemCoberturas creados segun los filtrados ingresados.
        /// </summary>
        /// <param name="inputsDto">Entrada de datos</param>
        /// <returns>Modelo de datos</returns>
        [HttpPost]
        [Route("GetCatalogoItemCoberturaFiltrado")]
        public IActionResult GetCatalogoItemCoberturaFiltrado([FromBody] InputsCatalogoItemCoberturaFiltradoDto inputsDto)
        {
            try
            {
                var Data = _repository.GetCatalogoItemCoberturaFiltrado(inputsDto);

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
