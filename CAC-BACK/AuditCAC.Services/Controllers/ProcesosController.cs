using AuditCAC.Domain.Entities;
using AuditCAC.MainCore.Module.Interface;
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
    [Route("api/Procesos")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProcesosController : ControllerBase
    {
        private readonly IProcesosRepository<ProcessModel> _ProcesosRepository;
        private readonly ILogger<ProcessModel> _logger;

        public ProcesosController(IProcesosRepository<ProcessModel> ProcesosRepository, ILogger<ProcessModel> logger)
        {
            this._ProcesosRepository = ProcesosRepository;
            _logger = logger;
        }

        // GET: api/Medicion
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            //Obtenemos todos los registros.
            var Data = await _ProcesosRepository.GetAll();
            //IEnumerable<MedicionModel> Data = await _MedicionRepository.GetAll();

            //Retornamos datos.
            return Ok(Data);
        }

        // GET: api/Medicion/5
        [HttpGet("{id:int}")]
        public async Task<IActionResult> Get(int id)
        {
            //Obtenemos registro buscado
            var Data = await _ProcesosRepository.Get(id);

            //Validamos si el registro existe.
            if (Data == null)
            {
                return NotFound("The record couldn't be found.");
            }

            //Retornamos datos.
            return Ok(Data);
        }

        // POST: api/Medicion
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] ProcessModel entity)
        {
            //Validamos si recibimos una entidad.
            if (entity == null)
            {
                return BadRequest("The record is null.");
            }

            //Registramos.
            await _ProcesosRepository.Add(entity);

            //Retornamos ruta de registro creado.
            return CreatedAtRoute("Get", new { Id = entity.ProcessId }, entity);
        }

        // PUT: api/Medicion/5
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Put(int id, [FromBody] ProcessModel entity)
        {
            //Validamos si recibimos una entidad.
            if (entity == null)
            {
                return BadRequest("The record is null.");
            }

            //Buscamos y validamos que exista el registro buscado.
            ProcessModel ToUpdate = await _ProcesosRepository.Get(id);
            if (ToUpdate == null)
            {
                return NotFound("The record couldn't be found.");
            }

            //Actualizamos el registro.
            await _ProcesosRepository.Update(ToUpdate, entity);

            //Retornamos sin contenido.
            return NoContent();
        }

        // DELETE: api/Medicion/5
        //[HttpDelete("{id:int}")]
        [HttpGet]
        [Route("Delete")]
        public async Task<IActionResult> Delete(int id)
        {
            //Obtenemos registro buscado
            var Data = await _ProcesosRepository.Get(id);

            //Validamos si el registro existe.
            if (Data == null)
            {
                return NotFound("The record couldn't be found.");
            }

            //Eliminamos el registro.
            await _ProcesosRepository.Delete(Data);

            //Retornamos sin contenido.
            return NoContent();
        }

        [HttpPost("/Procesos")]
        public async Task<IActionResult> GetAllMedicion([FromBody] InputsProcesosDto Procesos)
        {
            try
            {
                var Data = await _ProcesosRepository.GetProcesos(Procesos);
                return Ok(Data);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

    }
}
