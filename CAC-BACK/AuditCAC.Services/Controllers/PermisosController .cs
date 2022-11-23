using AuditCAC.Domain.Dto;
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
    [Route("api/permisos")]
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class PermisosController : ControllerBase
    {
        private readonly IRepository<CurrentProcessModel> _repository;
        private readonly ILogger<CurrentProcessModel> _logger;

        public PermisosController(IRepository<CurrentProcessModel> repository, ILogger<CurrentProcessModel> logger)
        {
            this._repository = repository;
            _logger = logger;
        }


        /// <summary>
        /// Consulta listado de roles
        /// </summary>
        /// <returns>Listado Roles</returns>
        [HttpGet]
        [Route("ConsultaRoles")]
        public async Task<IEnumerable<ResponseRoles>> ConsultaRoles()
        {
            try
            {
                var result = await _repository.ConsultaRoles();
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }


        /// <summary>
        /// Consulta permisos por rol
        /// </summary>
        /// <param name="rolId">Id Rol</param>
        /// <returns>Modelo permisos</returns>
        [HttpGet]
        [Route("ConsultaPermisosRol/{rolId}")]
        public async Task<IEnumerable<PermisoRol>> ConsultaPermisosRol(string rolId)
        {
            try
            {
                var result = await _repository.ConsultaPermisosRol(rolId);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }


        /// <summary>
        /// Crea o Actualiza los permisos del rol
        /// </summary>
        /// <param name="input">Modelo Permisos</param>
        /// <returns>true</returns>
        [HttpPost]
        [Route("UpsertPermisoRol")]
        public async Task<bool> Insert([FromBody] PermisoRol input)
        {
            try
            {
                var result = await _repository.UpsertPermisoRol(input);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw new Exception("Error", ex);
            }
        }

    }
}
