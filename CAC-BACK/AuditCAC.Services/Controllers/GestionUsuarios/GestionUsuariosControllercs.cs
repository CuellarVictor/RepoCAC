using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks; 
using Newtonsoft.Json;
using AuditCAC.MainCore.Module.Users.Interfaces;
using AuditCAC.Domain.Dto;
using Microsoft.Extensions.Logging;

namespace AuditCAC.Services.Controllers.GestionUsuarios {

    [Route("api/[controller]")]
    [ApiController]
    public class GestionUsuariosControllercs : ControllerBase
    {
        #region Definiciones 
        private readonly IUserManagement _UM;
        private readonly ILogger<UsuarioResponse> _logger;
        #endregion

        #region Constructor 
        public GestionUsuariosControllercs(IUserManagement um, ILogger<UsuarioResponse> logger)
        {
            _UM = um;
            _logger = logger;
        }
        #endregion

        #region Endpoints

        /// <summary>
        /// Consulta Listado de usuarios
        /// </summary>
        /// <returns>Modelo Usuarios</returns>
        [HttpGet]
        [Route("GetUsers")]
        public async Task<List<UsuarioResponse>> GetUsers()
        {
            try
            {
                return await _UM.GetUsers();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
            
        }

        /// <summary>
        /// Crea o actualiza usuario
        /// </summary>
        /// <param name="input">modelo usaurio</param>
        /// <returns>modelo llave valor</returns>
        [HttpPost]
        [Route("Upsertuser")]
        public async Task<ResponseLlaveValor> Upsertuser([FromBody] UsuarioResponse input)
        {
            try
            {
                var result = await _UM.Upsertuser(input);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        [HttpGet]
        [Route("GetPanelEnfermedadesMadre")]
        public async Task<IActionResult> GetPanelEnfermedadesMadre()
        {
            var Data = await _UM.GetPanelEnfermedadesMadre();
            return Ok(JsonConvert.SerializeObject(Data));
        }


        [HttpGet]
        [Route("GetPanel")]
        public async Task<IActionResult> GetPanel()
        {
            var Data = await _UM.GetPanel();
            return Ok(Data);
        }

        [HttpGet]
        [Route("GetEnfermedades")]
        public async Task<IActionResult> GetEnfermedades()
        {
            var resp = await _UM.GetEnfermedades();
            return Ok(JsonConvert.SerializeObject(resp));
        }

        [HttpGet]
        [Route("GetRoles")]
        public async Task<IActionResult> GetRoles()
        {
            var resp = await _UM.GetRoles();
            return Ok(JsonConvert.SerializeObject(resp));
        }

        /// <summary>
        /// Para consultar todos los Logs de Procesos segun los filtrados ingresados.
        /// </summary>
        /// <param name="inputsDto">Entrada de datos</param>
        /// <returns>Modelo de datos</returns>
        [HttpPost]
        [Microsoft.AspNetCore.Mvc.Route("GetProcessLogFiltrado")]
        public async Task<IActionResult> GetProcessLogFiltrado([FromBody] InputsProcessLogFiltradoDto inputsDto)
        {
            try
            {
                var Data = await _UM.GetProcessLogFiltrado(inputsDto);

                var PaginationDto = new PaginationDto();
                PaginationDto.Data = Data.Item1;
                //PaginationDto.NoRegistrosTotales = Data.Item2;
                PaginationDto.NoRegistrosTotalesFiltrado = (int)Data.Item2;
                PaginationDto.TotalPages = (int)Data.Item3;

                //return Ok(Data.Item1);
                return Ok(PaginationDto);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        /// <summary>
        /// Para consultar Asignacion de lider por entidad
        /// </summary>
        /// <param name="inputsDto">Entrada de datos</param>
        /// <returns>Modelo de datos</returns>
        [HttpPost]
        [Microsoft.AspNetCore.Mvc.Route("ConsultaAsignacionLiderEntidad")]
        public async Task<IActionResult> GetConsultaAsignacionLiderEntidad([FromBody] InputsConsultaAsignacionLiderEntidadDto inputsDto)
        {
            try
            {
                var Data = await _UM.GetConsultaAsignacionLiderEntidad(inputsDto);

                var PaginationDto = new PaginationDto();
                PaginationDto.Data = Data.Item1;
                //PaginationDto.NoRegistrosTotales = Data.Item2;
                PaginationDto.NoRegistrosTotalesFiltrado = (int)Data.Item2;
                PaginationDto.TotalPages = (int)Data.Item3;

                //return Ok(Data.Item1);
                return Ok(PaginationDto);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        /// <summary>
        ///	Consulta auditores por Cobertura, EPS, Idperiodo, para asignar el lider
        /// </summary>
        /// <param name="inputsDto">Entrada de datos</param>
        /// <returns>Modelo de datos</returns>
        [HttpPost]
        [Microsoft.AspNetCore.Mvc.Route("ConsultaAuditoresAsignacionLiderEntidad")]
        public async Task<IActionResult> ConsultaAuditoresAsignacionLiderEntidad([FromBody] InputsResponseConsultaEPSCoberturaPeriodoDto inputsDto)
        {
            try
            {
                var data = await _UM.ConsultaAuditoresAsignacionLiderEntidad(inputsDto);
                return Ok(data);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }


        /// <summary>
        /// Consulta periodos con data en cronograma por Id de cobertura
        /// </summary>
        /// <param name="idCobertura">Id Cobertura</param>
        /// <returns>Lista llave valor (idPeriodo)</returns>
        [HttpGet]
        [Route("ConsultaPeriodosCobertura/{idCobertura}")]
        public async Task<List<ResponseLlaveValor>> ConsultaPeriodosCobertura(int idCobertura)
        {
            try
            {
                var result = await _UM.ConsultaPeriodosCobertura(idCobertura);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

        /// <summary>
        /// Crea o Actualiza lideres de una EPS
        /// </summary>
        /// <param name="inputsDto">Entrada de datos</param>
        /// <returns>modelo llave valor</returns>
        [HttpPost]
        [Route("UpsertLiderEPS")]
        public async Task<ResponseLlaveValor> UpsertLiderEPS([FromBody] InputsUpsertLiderEPSDto inputsDto)
        {
            try
            {
                var result = await _UM.UpsertLiderEPS(inputsDto);
                return result;
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

