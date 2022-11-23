using AuditCAC.Dal.Data;
using AuditCAC.Domain.Dto;
using AuditCAC.Domain.Dto.PanelEnfermedad;
using AuditCAC.Domain.Entities;
using AuditCAC.Domain.Entities.Usuarios;
using AuditCAC.MainCore.Module.Users.Interfaces;
using AutoMapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace AuditCAC.MainCore.Module.UserManagement
{
    public class UserManagement: IUserManagement
    {
        #region Dependencias
        private readonly DBAuditCACContext _context;
        private readonly IMapper _map;
        private readonly ILogger<UsuarioDto> _logger;

        #endregion

        #region Constructor
        public UserManagement(DBAuditCACContext context, IMapper map, ILogger<UsuarioDto> logger)
        {
            _context = context;
            _map = map;
            _logger = logger;
        }
        #endregion

        #region Metodos

        /// <summary>
        /// Consulta Listado de usuarios
        /// </summary>
        /// <returns>Modelo Usuarios</returns>
        public async Task<List<UsuarioResponse>> GetUsers()
        {
            try
            {
                List<UsuarioResponse> response = new List<UsuarioResponse>();


                // Consulta Usuarios
                string sql = "EXEC [AUTH].[SP_Consulta_Usuarios]";

                List<SqlParameter> parametros = new List<SqlParameter> { };
                var users = await _context.UsuarioDto.FromSqlRaw<UsuarioDto>(sql, parametros.ToArray()).ToListAsync();


                //Consulta Usuarios x enfermedad
                string sql2 = "EXEC [AUTH].[SP_Consulta_UsuariosxEnfermedad]";

                List<SqlParameter> parametros2 = new List<SqlParameter> { };
                var usuariosxEnfermedad = await _context.UsuarioxEnfermedadDto.FromSqlRaw<UsuarioxEnfermedadDto>(sql2, parametros2.ToArray()).ToListAsync();

                //Construye modelo response
                users?.All(x =>
                {
                    response.Add(new UsuarioResponse(x, usuariosxEnfermedad));
                    return true;
                });


                return response;
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
        public async Task<ResponseLlaveValor> Upsertuser(UsuarioResponse input)
        {
            try
            {
                var query = "EXEC [AUTH].[SP_Upsert_Usuario]  @Id, @Usuario, @Email, @Telefono, @Password, @RolId, @Nombres, @Apellidos, @Estado, @ModifyBy, @Enable, @UsuarioxEnfermedad";

                //Declare Header datatable
                DataTable usuarioxEnfermedadDataTable = new DataTable();
                usuarioxEnfermedadDataTable.Columns.Add("Id", typeof(int));
                usuarioxEnfermedadDataTable.Columns.Add("Valor", typeof(string));

                var dtUsuarioxEnfermedad = new SqlParameter("@UsuarioxEnfermedad", SqlDbType.Structured);
                dtUsuarioxEnfermedad.Value = usuarioxEnfermedadDataTable;
                dtUsuarioxEnfermedad.TypeName = "dbo.DT_LLave_Valor";

                input.UsuariosxEnfermedad?.All(x =>
                {
                    usuarioxEnfermedadDataTable.Rows.Add(x, ("Cobertura +" + x));
                    return true;
                });

                List<SqlParameter> parametros = new List<SqlParameter> {
                        new SqlParameter{ ParameterName = "@Id", Value = input.Id },
                        new SqlParameter{ ParameterName = "@Usuario", Value = input.Usuario },
                        new SqlParameter{ ParameterName = "@Email", Value = input.Email },
                        new SqlParameter{ ParameterName = "@Telefono", Value = input.Telefono },
                        new SqlParameter{ ParameterName = "@Password", Value = input.Password },
                        new SqlParameter{ ParameterName = "@RolId", Value = input.RolId },
                        new SqlParameter{ ParameterName = "@Nombres", Value = input.Nombre },
                        new SqlParameter{ ParameterName = "@Apellidos", Value = input.Apellidos },
                        new SqlParameter{ ParameterName = "@Estado", Value = input.Estado },
                        new SqlParameter{ ParameterName = "@ModifyBy", Value = input.ModifyBy },
                        new SqlParameter{ ParameterName = "@Enable", Value = input.Enable },
                        dtUsuarioxEnfermedad
                };

                var Data = await _context.LLaveValor.FromSqlRaw<ResponseLlaveValor>(query, parametros.ToArray()).ToListAsync();

                return Data[0];
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }


        /// <summary>
        /// GetPanelEnfermedadesMadre Lista el panel de enfermedades madre
        /// </summary>
        /// <returns>Listado de Usuarios</returns>
        public async Task<List<PanelEnfermedadesMadre>> GetPanelEnfermedadesMadre()
        {
            var query = "select * from dbo.[GetPanelEnfermedadesMadre];";

            List<SqlParameter> parametros = new List<SqlParameter> { };

            var Data = await _context.PanelEnfermedadesMadre.FromSqlRaw<PanelEnfermedadesMadre>(query, parametros.ToArray()).ToListAsync();

            return Data;
        }
        

        public async Task<List<PanelEnfermedadesDto>> GetPanel()
        {
            List<PanelEnfermedadesDto> panel = new List<PanelEnfermedadesDto>();           
                     
            var enfMadre = _context.Enfermedad.ToList();
            var usEnfermedad = _context.GetFiltrosBolsaMedicion.ToList();
            foreach (var item in enfMadre)
            {
                List<MedicionesDto> Mediciones = new List<MedicionesDto>();
                PanelEnfermedadesDto UnaEnf = new PanelEnfermedadesDto();
                UnaEnf.Grupo = item.Nombre;               
                var medEnfermedad = _context.MedicionModel.Where(x => x.IdCobertura == item.IdCobertura).ToList();               
                foreach (var enf in medEnfermedad)
                {
                    List<UsuariosDto> usuariosDtos = new List<UsuariosDto>();
                    MedicionesDto UnaMed = new MedicionesDto();
                    UnaMed.Enfermedad = enf.Nombre;                    
                    foreach (var us in usEnfermedad)
                    {                       
                        UsuariosDto Usuarios = new UsuariosDto();                         
                        if (us.IdCobertura == enf.IdCobertura)
                        {
                            var User = _context.AspNetUsersDetellesModel.Where(x => x.AspNetUsersId == us.IdUsuario).FirstOrDefault();
                            Usuarios.Codigo = User.Codigo;
                            Usuarios.Nombre = User.Nombres + " " + User.Apellidos;
                            usuariosDtos.Add(Usuarios);
                        }                       
                    }
                    Mediciones.Add(UnaMed);
                    UnaEnf.Mediciones = Mediciones;
                    for (int i = 0; i < Mediciones.Count; i++)
                    {
                        UnaEnf.Mediciones[i].Usuarios = usuariosDtos;
                    }
                }               
                panel.Add(UnaEnf);
            }
            return await Task.FromResult(panel);
        }

        /// <summary>
        /// GetEnfermedades Lista las enfermedades
        /// </summary>
        /// <returns>Listado de Enfermedades</returns>
        public async Task<List<EnfermedadModel>> GetEnfermedades()
        {
            var query = "EXEC dbo.[getEnfermedades];";

            List<SqlParameter> parametros = new List<SqlParameter> { };

            var Data = await _context.Enfermedad.FromSqlRaw<EnfermedadModel>(query, parametros.ToArray()).ToListAsync();

            return Data;
        }

        /// <summary>
        /// GetRoles Lista los roles de aplicación
        /// </summary>
        /// <returns>Listado de Enfermedades</returns>
        public async Task<List<RolesUsuarioModel>> GetRoles()
        {
            var query = "select * from [dbo].[GetUserRoles];";

            List<SqlParameter> parametros = new List<SqlParameter> { };

            var Data = await _context.RolesUsuarios.FromSqlRaw<RolesUsuarioModel>(query, parametros.ToArray()).ToListAsync();

            return Data;
        }


        /// <summary>
        /// Para consultar todos los Logs de Procesos segun los filtrados ingresados.
        /// </summary>
        /// <param name="inputsDto">Entrada de datos</param>
        /// <returns>Modelo de datos</returns>
        public async Task<Tuple<List<ResponseProcessLogFiltradoDto>, int?, int?>> GetProcessLogFiltrado(InputsProcessLogFiltradoDto inputsDto)
        {
            try
            {
                string sql = "EXEC SP_Consulta_Process_Log_Filtrado @PageNumber, @MaxRows, @ProcessLogId, @ProcessId, @User, @DateIni, @DateFin, @Result, @Observation, @PalabraClave";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters    
                    new SqlParameter { ParameterName = "@PageNumber", Value = inputsDto.PageNumber},
                    new SqlParameter { ParameterName = "@MaxRows", Value = inputsDto.MaxRows},
                    //
                    new SqlParameter { ParameterName = "@ProcessLogId", Value = inputsDto.ProcessLogId},
                    new SqlParameter { ParameterName = "@ProcessId", Value = inputsDto.ProcessId},
                    new SqlParameter { ParameterName = "@User", Value = inputsDto.User},
                    new SqlParameter { ParameterName = "@DateIni", Value = inputsDto.DateIni},
                    new SqlParameter { ParameterName = "@DateFin", Value = inputsDto.DateFin},
                    new SqlParameter { ParameterName = "@Result", Value = inputsDto.Result},
                    new SqlParameter { ParameterName = "@Observation", Value = inputsDto.Observation},
                    new SqlParameter { ParameterName = "@PalabraClave", Value = inputsDto.PalabraClave},
                };

                var Data = await _context.ResponseProcessLogFiltradoDto.FromSqlRaw<ResponseProcessLogFiltradoDto>(sql, parms.ToArray()).ToListAsync();

                //Obtenemos No Registros maximos a consultar y los agregamos al Header del Request (API).                
                var NoRegistrosTotalesFiltrado = Data.Count() > 0 ? Data.FirstOrDefault().NoRegistrosTotales : 0;

                var TotalPages = NoRegistrosTotalesFiltrado / inputsDto.MaxRows;
                //Data.Select(c => { c.NoRegistrosTotales = ""; return c; }).ToList();

                return Tuple.Create(Data, NoRegistrosTotalesFiltrado, TotalPages);
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Para consultar Asignacion de lider por entidad
        /// </summary>
        /// <param name="inputsDto">Entrada de datos</param>
        /// <returns>Modelo de datos</returns>
        public async Task<Tuple<List<ResponseConsultaAsignacionLiderEntidadDto>, int, int>> GetConsultaAsignacionLiderEntidad(InputsConsultaAsignacionLiderEntidadDto inputsDto)
        {
            try
            {
                string sql = "EXEC SP_Consulta_Asignacion_Lider_Entidad @PageNumber, @MaxRows, @IdCobertura, @IdPeriodo";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters    
                    new SqlParameter { ParameterName = "@PageNumber", Value = inputsDto.PageNumber},
                    new SqlParameter { ParameterName = "@MaxRows", Value = inputsDto.MaxRows},
                    //
                    new SqlParameter { ParameterName = "@IdCobertura", Value = inputsDto.IdCobertura},
                    new SqlParameter { ParameterName = "@IdPeriodo", Value = inputsDto.IdPeriodo},
                };

                var Data = await _context.ResponseConsultaAsignacionLiderEntidadDto.FromSqlRaw<ResponseConsultaAsignacionLiderEntidadDto>(sql, parms.ToArray()).ToListAsync();

                int NoRegistrosTotalesFiltrado = 0;
                int TotalPages = 0;
                #region QueryPaginador

                if (Data.Count() > 0)
                {
                    string sqlQuery = "EXEC [dbo].[GetCountPaginator] @Query";

                    List<SqlParameter> parmsQuery = new List<SqlParameter>
                    {
                        new SqlParameter { ParameterName = "@Query", Value = Data[0].NoRegistrosTotales},
                    };

                    var DataQuery = await _context.ResponseQueryPaginatorDto.FromSqlRaw<ResponseQueryPaginatorDto>(sqlQuery, parmsQuery.ToArray()).ToListAsync();

                    //Obtenemos No Registros maximos a consultar y los agregamos al Header del Request (API).                
                    NoRegistrosTotalesFiltrado = DataQuery.Count();

                    TotalPages = NoRegistrosTotalesFiltrado / inputsDto.MaxRows;
                }

                

                #endregion


                //Data.Select(c => { c.NoRegistrosTotales = ""; return c; }).ToList();

                return Tuple.Create(Data, NoRegistrosTotalesFiltrado, TotalPages);
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        /// <summary>
        ///	Consulta auditores por Cobertura, EPS, Idperiodo, para asignar el lider
        /// </summary>
        /// <param name="inputsDto">Entrada de datos</param>
        /// <returns>Modelo de datos</returns>
        public async Task<List<ResponseConsultaEPSCoberturaPeriodoDto>> ConsultaAuditoresAsignacionLiderEntidad(InputsResponseConsultaEPSCoberturaPeriodoDto inputsDto)
        {
            try
            {
                string sql = "EXEC SP_Consulta_Auditores_Asignacion_Lider_Entidad  @IdPeriodo, @IdCobertura, @IdEPS";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    new SqlParameter { ParameterName = "@IdPeriodo", Value = inputsDto.IdPeriodo},
                    new SqlParameter { ParameterName = "@IdCobertura", Value = inputsDto.IdCobertura},
                    new SqlParameter { ParameterName = "@IdEPS", Value = inputsDto.IdEPS},
                };

                var Data = await _context.ResponseConsultaEPSCoberturaPeriodoDto.FromSqlRaw<ResponseConsultaEPSCoberturaPeriodoDto>(sql, parms.ToArray()).ToListAsync();
                return Data;
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }            
        }


        /// <summary>
        /// Consulta periodos con data en cronograma por Id de cobertura
        /// </summary>
        /// <param name="idCobertura">Id Cobertura</param>
        /// <returns>Lista llave valor (idPeriodo)</returns>
        public async Task<List<ResponseLlaveValor>> ConsultaPeriodosCobertura(int idCobertura)
        {
            try
            {
                string sql = "EXEC SP_Consulta_Periodos_Cobertura  @IdCobertura";

                List<SqlParameter> parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@IdCobertura", Value = idCobertura}
                };

                var Data = await _context.LLaveValor.FromSqlRaw<ResponseLlaveValor>(sql, parms.ToArray()).ToListAsync();
                return Data;
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Crea o Actualiza lideres de una EPS
        /// </summary>
        /// <param name="inputsDto">Entrada de datos</param>
        /// <returns>modelo llave valor</returns>
        public async Task<ResponseLlaveValor> UpsertLiderEPS(InputsUpsertLiderEPSDto input)
        {
            try
            {
                var query = "EXEC SP_Upsert_Lider_EPS @IdEPS, @IdAuditorLider, @IdCobertura, @IdPeriodo, @Usuario";

                List<SqlParameter> parametros = new List<SqlParameter> {
                        new SqlParameter{ ParameterName = "@IdEPS", Value = input.IdEPS },
                        new SqlParameter{ ParameterName = "@IdAuditorLider", Value = input.IdAuditorLider },
                        new SqlParameter{ ParameterName = "@IdCobertura", Value = input.IdCobertura },
                        new SqlParameter{ ParameterName = "@IdPeriodo", Value = input.IdPeriodo },
                        new SqlParameter{ ParameterName = "@Usuario", Value = input.Usuario },
                    };

                var Data = await _context.LLaveValor.FromSqlRaw<ResponseLlaveValor>(query, parametros.ToArray()).ToListAsync();

                return Data[0];
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }
        #endregion
    }
}
