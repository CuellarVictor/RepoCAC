using AuditCAC.Domain.Entities;
using AuditCAC.Domain.MongoEntities;
using AuditCAC.MainCore.Module;
using AuditCAC.MainCore.Module.Interface;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;


namespace AuditCAC.CoreCAC.Services.Controllers
{
    [Microsoft.AspNetCore.Mvc.Route("api/TestMongo")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class TestMongoController: Controller
    {
        private ITestMongoRepository db = new TestMongoManager();

        // GET: api/TestMongo
        /// <summary>
        /// Consultar todos los registros disponibles.
        /// </summary>
        /// <remarks>
        /// Regresa todos los registros almacenados.
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Configruacion ADO</returns>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            //Obtenemos todos los registros.
            var Data = await db.GetAll();
            //IEnumerable<TestMongoModel> Data = await db.GetAll();

            //Retornamos datos.
            return Ok(Data);
        }

        // GET: api/TestMongo/5
        /// <summary>
        /// Consultar un registro con su Id.
        /// </summary>
        /// <remarks>
        /// Regresa un solo registro, buscado por su Id.
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Configruacion ADO</returns>
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(string Id)
        {
            //Obtenemos registro buscado
            var Data = await db.Get(Id);

            //Validamos si el registro existe.
            if (Data == null)
            {
                return NotFound("The record couldn't be found.");
            }

            //Retornamos datos.
            return Ok(Data);
        }

        // POST: api/TestMongo
        /// <summary>
        /// Crear un registro nuevo.
        /// </summary>
        /// <remarks>
        /// Permite crear un nuevo registro y regresa ruta para consultar el registro creado.
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Configruacion ADO</returns>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TestMongoModel entity)
        {
            try
            {
                //Validamos si recibimos una entidad.
                if (entity == null)
                {
                    return BadRequest("The record is null.");
                }

                //Registramos.
                await db.Add(entity);

                //Retornamos ruta de registro creado.
                //return CreatedAtRoute("Get", new { Id = entity.Id.ToString() }, entity); //Aqui hay un error debido a que no puede generar una ruta "No route matches the supplied values."
                return CreatedAtAction(nameof(Get), new { id = entity.Id }, entity);
                //return Ok(); //Aqui hay un error debido a que no puede generar una ruta "No route matches the supplied values."
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        // PUT: api/TestMongo/5
        /// <summary>
        /// Editar un registro por su Id.
        /// </summary>
        /// <remarks>
        /// Edita un registro, buscado por su Id.
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Configruacion ADO</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(string Id, [FromBody] TestMongoModel entity)
        {
            //Validamos si recibimos una entidad.
            if (entity == null)
            {
                return BadRequest("The record is null.");
            }

            //Buscamos y validamos que exista el registro buscado.
            TestMongoModel ToUpdate = await db.Get(Id);
            if (ToUpdate == null)
            {
                return NotFound("The record couldn't be found.");
            }

            //Actualizamos el registro.
            entity.Id = new MongoDB.Bson.ObjectId(Id);
            await db.Update(entity);

            //Retornamos sin contenido.
            return NoContent();
        }

        // DELETE: api/TestMongo/5
        /// <summary>
        /// Eliminar un registro con su Id.
        /// </summary>
        /// <remarks>
        /// Elimina un registro buscado por su Id.
        /// </remarks>
        /// <response code="200">El proceso se ejecuto de forma correcta.</response>        
        /// <response code="400">BadRequest. Error al realizar la petición, verifique la ruta o los parametros ingresados.</response>
        /// <response code="500">Error general, informar al administrador del servicio.</response>
        /// <returns>Configruacion ADO</returns>
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(string Id)
        {
            //Obtenemos registro buscado
            //var Data = await db.Get(Id);

            //Validamos si el registro existe.
            //if (Data == null)
            //{
            //    return NotFound("The record couldn't be found.");
            //}

            //Eliminamos el registro.
            await db.Delete(Id);

            //Retornamos sin contenido.
            return NoContent();
        }
    }
}
