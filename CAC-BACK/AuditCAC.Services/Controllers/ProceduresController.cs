using AuditCAC.Dal.Entities;
using AuditCAC.Domain.Dto;
using AuditCAC.Domain.Entities;
using AuditCAC.MainCore.Module.Interface;
using log4net;
using log4net.Appender;
using log4net.Config;
using log4net.Core;
using log4net.ObjectRenderer;
using log4net.Plugin;
using log4net.Repository;
using log4net.Util;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.IO;
using System.Threading.Tasks;

namespace AuditCAC.Services.Controllers
{
    [Route("api/procedures")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class ProceduresController: ControllerBase
    {
        private readonly IProceduresRepository<CoberturaModel> _procedureRepository;       

        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        //Constructor.
        public ProceduresController(IProceduresRepository<CoberturaModel> procedureRepository)
        {
            this._procedureRepository = procedureRepository;           
        }     

        public string Name { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public RendererMap RendererMap => throw new NotImplementedException();

        public PluginMap PluginMap => throw new NotImplementedException();

        public LevelMap LevelMap => throw new NotImplementedException();

        public Level Threshold { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public bool Configured { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public ICollection ConfigurationMessages { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public PropertiesDictionary Properties => throw new NotImplementedException();

        public event LoggerRepositoryShutdownEventHandler ShutdownEvent;
        public event LoggerRepositoryConfigurationResetEventHandler ConfigurationReset;
        public event LoggerRepositoryConfigurationChangedEventHandler ConfigurationChanged;

        /* Para Procedures */

        // POST: api/Coberturas
        [HttpPost]
        [Route("Coberturas")]
        public async Task<IActionResult> Coberturas([FromBody] InputsCoberturaDto inputsCoberturaDto)
        {
            try
            {
                var Data = await _procedureRepository.GetcacCoberturas(inputsCoberturaDto);
                return Ok(Data);
            }
            catch (Exception ex)
            {
                _log.Fatal("Fatal", ex);
                throw new Exception("Error", ex);
            }
        }

        // POST: api/CoberturasError
        [HttpPost]
        [Route("CoberturasError")]
        public async Task<IActionResult> CoberturasError([FromBody] InputsCoberturasErrorDto inputsCoberturasErrorDto)
        {
            try
            {
                var Data = await _procedureRepository.GetcacCoberturasError(inputsCoberturasErrorDto);
                return Ok(Data);
            }
            catch (Exception ex)
            {
                _log.Fatal("Fatal", ex);
                throw new Exception("Error",ex);
            }
           
        }

        // POST: api/Error
        [HttpPost]
        [Route("Error")]
        public async Task<IActionResult> Error([FromBody] InputsErrorDto inputsErrorDto)
        {
            try
            {
                var Data = await _procedureRepository.GetcacError(inputsErrorDto);
                return Ok(Data);
            }
            catch (Exception ex)
            {
                _log.Fatal("Fatal", ex);
                throw new Exception("Error", ex);
            }

        }
    
        // POST: api/Periodo
        [HttpPost]
        [Route("Periodo")]
        public async Task<IActionResult> Periodo([FromBody] InputsPeriodoDto inputsPeriodo)
        {
            try
            {
                var Data = await _procedureRepository.Getcacperiodo(inputsPeriodo);
                return Ok(Data);
            }
            catch (Exception ex)
            {

                _log.Fatal("Fatal", ex);
                throw new Exception("Error", ex);
            }

        }

        // POST: api/Radicado
        [HttpPost]
        [Route("Radicado")]
        public async Task<IActionResult> Radicado([FromBody] InputsRadicadoDto inputsRadicado)
        {
            try
            {
                var Data = await _procedureRepository.GetcacRadicado(inputsRadicado);
                return Ok(Data);
            }
            catch (Exception ex)
            {

                _log.Fatal("Fatal", ex);
                throw new Exception("Error", ex);
            }

        }

        // POST: api/Regla
        [HttpPost]
        [Route("Regla")]
        public async Task<IActionResult> Regla([FromBody] InputsReglaDto inputsReglaDto)
        {
            try
            {
                var Data = await _procedureRepository.GetcacRegla(inputsReglaDto);
                return Ok(Data);
            }
            catch (Exception ex)
            {
                _log.Fatal("Fatal", ex);
                throw new Exception("Error", ex);
            }

        }

        //public void ResetConfiguration()
        //{
        //    throw new NotImplementedException();
        //}

        //public void Shutdown()
        //{
        //    throw new NotImplementedException();
        //}

        // POST: api/Variable
        [HttpPost]
        [Route("Variable")]
        public async Task<IActionResult> Variable([FromBody] InputsVariableDto inputsVariableDto)
        {
            try
            {
                var Data = await _procedureRepository.GetCacvariable(inputsVariableDto);
                return Ok(Data);
            }
            catch (Exception ex)
            {
                _log.Fatal("Fatal", ex);
                throw new Exception("Error", ex);
            }

        }

        // POST: api/VariablesPeriodo
        [HttpPost]
        [Route("VariablesPeriodo")]
        public async Task<IActionResult> VariablesPeriodo([FromBody] InputsVariablesPeriodoDto inputsVariablesPeriodoDto)
        {
            try
            {
                var Data = await _procedureRepository.GetcacVariablesPeriodo(inputsVariablesPeriodoDto);
                return Ok(Data);
            }
            catch (Exception ex)
            {
                _log.Fatal("Fatal", ex);
                throw new Exception("Error", ex);
            }

        }        
    }
}
