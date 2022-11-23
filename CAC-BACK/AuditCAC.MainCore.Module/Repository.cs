using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuditCAC.Dal.Data;
using AuditCAC.Domain.Dto;
using AuditCAC.Domain.Entities;
using AuditCAC.Domain.Helpers;
using AuditCAC.MainCore.Module.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AuditCAC.MainCore.Module
{
    public class Repository<T> : IRepository<T> where T : BaseEntity
    {
        protected readonly DBAuditCACContext context;
        private DbSet<T> entities;
        string errorMessage = string.Empty;
        private readonly ILogger<BaseEntity> _logger;

        public Repository(DBAuditCACContext context, ILogger<BaseEntity> logger)
        {

            this.context = context;
            entities = context.Set<T>();
            _logger = logger;
        }
        public async Task<IEnumerable<T>> GetAll()
        {
            try
            {
                return entities.AsEnumerable();
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        public async Task<T> GetById(int id)
        {
            try
            {
                return entities.SingleOrDefault(s => s.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }


        public async Task<string> Insert(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                entities.Add(entity);
                context.SaveChanges();
                return "OK";
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
            
        }
        public async Task<string> Update(T entity)
        {
            try
            {
                if (entity == null)
                    throw new ArgumentNullException("entity");

                entities.Update(entity);
                context.SaveChanges();
                return "OK";
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
            
        }
        public async Task<string> Delete(int id)
        {
            
            try
            {
                T entity = entities.SingleOrDefault(s => s.Id == id);
                entities.Remove(entity);
                context.SaveChanges();
                return "OK";
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        #region Proceso

        /// <summary>
        /// Valida  si se encuentra corriendo un proceso actual con el Id indicado
        /// </summary>
        /// <param name="id">Id Proceso</param>
        /// <param name="result">Campo Result</param>
        /// <returns>Modelo Proceso Actual</returns>
        public async Task<CurrentProcessModel> ValidationCurrentProcess(int id, string result)
        {
            try
            {
                CurrentProcessModel oReturn = new CurrentProcessModel();
                oReturn = context.CurrentProcessModel.Where(s => s.ProcessId == id && s.Result.Contains(result)).FirstOrDefault();

                if (oReturn == null)
                {
                    oReturn = new CurrentProcessModel();
                }
                return oReturn;

            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Elimina proceso actual
        /// </summary>
        /// <param name="id">Id Proceso</param>
        /// <returns>Modelo Proceso Actual</returns>
        public async Task<CurrentProcessModel> DeleteCurrentProcess(int id, string result)
        {
            try
            {
                //Get Process
                var currentProcessList = context.CurrentProcessModel.Where(s => s.ProcessId == id && s.Result.Contains(result)).ToList();

                currentProcessList?.All(cp =>
                {
                    //Delete Process param
                    var currepntProcessparam = context.CurrentProcessParamModel.Where(x => x.CurrentProcessId == cp.Id).ToList();
                    context.CurrentProcessParamModel.RemoveRange(currepntProcessparam);
                    context.SaveChanges();
                    return true;
                });                

                //Delete process
                context.CurrentProcessModel.RemoveRange(currentProcessList);
                context.SaveChanges();

                return new CurrentProcessModel();

            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }
        #endregion

        #region Permisos
        
        /// <summary>
        /// Consulta listado de roles
        /// </summary>
        /// <returns>Listado Roles</returns>
        public async Task<List<ResponseRoles>> ConsultaRoles()
        {
            try
            {
                string sql = "EXEC [dbo].[SP_Consulta_Roles]";

                List<SqlParameter> parms = new List<SqlParameter>();

                var Data = await context.ResponseRoles.FromSqlRaw<ResponseRoles>(sql, parms.ToArray()).ToListAsync();
                return Data;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Consulta permisos por rol
        /// </summary>
        /// <param name="rolId">Id Rol</param>
        /// <returns>Modelo permisos</returns>
        public async Task<List<PermisoRol>> ConsultaPermisosRol(string rolId)
        {
            try
            {
                string sql = "EXEC [dbo].[SP_Consulta_Permisos_Rol] @RolId";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters   
                    new SqlParameter { ParameterName = "@RolId", Value = rolId }
                };

                var Data = await context.PermisoRol.FromSqlRaw<PermisoRol>(sql, parms.ToArray()).ToListAsync();
                return Data;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Crea o Actualiza los permisos del rol
        /// </summary>
        /// <param name="input">Modelo Permisos</param>
        /// <returns>true</returns>
        public async Task<bool> UpsertPermisoRol(PermisoRol input)
        {
            try
            {
                string sql = "EXEC [dbo].[SP_Upsert_Permisos_Rol] @FuncionalidadId, @RolId, @Enable, @Visible, @User";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters   
                    new SqlParameter { ParameterName = "@FuncionalidadId", Value = input.Id},
                    new SqlParameter { ParameterName = "@RolId", Value = input.RolId},
                    new SqlParameter { ParameterName = "@Enable", Value = input.Habilitado},
                    new SqlParameter { ParameterName = "@Visible", Value = input.Visible},
                    new SqlParameter { ParameterName = "@User", Value = input.User}
                };

                var Data = await context.Database.ExecuteSqlRawAsync(sql, parms.ToArray());
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        #endregion

        #region Auth

       /// <summary>
       /// Autentica Usuario
       /// </summary>
       /// <param name="input"></param>
       /// <returns>Modelo autenticacion</returns>
        public async Task<ResponseAutenticacionDto> Login(UserCredentialsDto input)
        {
            try
            {
                string sql = "EXEC [AUTH].[SP_Realiza_Autenticacion] @Usuario, @Password";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters   
                    new SqlParameter { ParameterName = "@Usuario", Value = input.Email},
                    new SqlParameter { ParameterName = "@Password", Value = input.Password}
                };

                var Data = await context.ResponseAutenticacionDto.FromSqlRaw<ResponseAutenticacionDto>(sql, parms.ToArray()).ToListAsync();

                return Data[0];
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }


        /// <summary>
        /// Recuperacion contraseña
        /// </summary>
        /// <param name="input">Modelo Usuario</param>
        public void EnviarCorreoRecuperacionPassword(ResponseAutenticacionDto input)
        {
            try
            {
                Util _util = new Util();


                var parameters = context.ParametrosGenerales.Where(x => x.Activo).ToList();

                string emailOrigen = parameters.Where(x => x.Id == (int)Enumeration.Parametros.EmailOrigen).Select(x => x.Valor).FirstOrDefault();
                string emailOrigenPassword = parameters.Where(x => x.Id == (int)Enumeration.Parametros.EmailOrigenPassword).Select(x => x.Valor).FirstOrDefault();
                string emailCopia = parameters.Where(x => x.Id == (int)Enumeration.Parametros.EmailCopia).Select(x => x.Valor).FirstOrDefault();
                string asuntoCorreoRecuperacion = parameters.Where(x => x.Id == (int)Enumeration.Parametros.AsuntoCorreoRecuperacion).Select(x => x.Valor).FirstOrDefault();
                string emailPuerto = parameters.Where(x => x.Id == (int)Enumeration.Parametros.EmailPuerto).Select(x => x.Valor).FirstOrDefault();
                string emailHost = parameters.Where(x => x.Id == (int)Enumeration.Parametros.EmailHost).Select(x => x.Valor).FirstOrDefault();
                string emailRecuperacionBody = parameters.Where(x => x.Id == (int)Enumeration.Parametros.EmailRecuperacionBody).Select(x => x.Valor).FirstOrDefault();
                string urlRecuperacionPassword = parameters.Where(x => x.Id == (int)Enumeration.Parametros.UrlRecuperacionPassword).Select(x => x.Valor).FirstOrDefault();

                if (emailOrigen == null || emailOrigenPassword == null || emailCopia == null || asuntoCorreoRecuperacion == null || emailPuerto == null ||
                    emailHost == null || emailRecuperacionBody == null)
                {
                    throw new ArgumentException("Parametros para envio de correo invalidos");
                }

                urlRecuperacionPassword = urlRecuperacionPassword.Replace("{{token}}", input.TokenBloqueo);
                emailRecuperacionBody = emailRecuperacionBody.Replace("{{url}}", urlRecuperacionPassword).Replace("{{nombre}}", input.Nombres);
                asuntoCorreoRecuperacion = asuntoCorreoRecuperacion.Replace("{{nombre}}", input.Nombres);

                _util.SendEmail(
                    input.Email,
                    emailOrigen,
                    emailOrigenPassword,
                    Convert.ToInt32(emailPuerto),
                    emailHost,
                    asuntoCorreoRecuperacion,
                    emailRecuperacionBody,
                    null,
                    null,
                    true,
                    emailCopia
                    );
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }


        /// <summary>
        /// Valida token recuperacion de contraseña
        /// </summary>
        /// <param name="token">token</param>
        /// <returns>Mensaje</returns>
        public async Task<ResponseLlaveValor> ValidaTokenRecuperacion(string token)
        {
            try
            {
                string sql = "EXEC [AUTH].[SP_Valida_Token_Recuperacion] @Token";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters   
                    new SqlParameter { ParameterName = "@Token", Value = token}
                };

                var Data = await context.LLaveValor.FromSqlRaw<ResponseLlaveValor>(sql, parms.ToArray()).ToListAsync();

                return Data[0];
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Actualiza password, proceso recuperación contraseña
        /// </summary>
        /// <param name="input">Modelo: token, password</param>
        public async Task<ResponseLlaveValor> ActualizaPassword(ActualizaPassword input)
        {
            try
            {
                string sql = "EXEC [AUTH].[SP_Actualiza_Password] @Token, @Password";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters   
                    new SqlParameter { ParameterName = "@Token", Value = input.Token},
                    new SqlParameter { ParameterName = "@Password", Value = input.Password}
                };

                var Data = await context.LLaveValor.FromSqlRaw<ResponseLlaveValor>(sql, parms.ToArray()).ToListAsync();

                return Data[0];
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Recuperar contraseña
        /// </summary>
        /// <param name="input">Modelo usuario</param>
        /// <returns>Mensaje Respuesta</returns>
        public async Task<ResponseLlaveValor> RecuperarPassword(UserCredentialsDto input)
        {
            try
            {
                ResponseLlaveValor response = new ResponseLlaveValor();
                // Consulta Usuario
                string sql = "EXEC [AUTH].[SP_Consulta_Usuario] @Usuario";

                List<SqlParameter> parms = new List<SqlParameter>
                    { 
                        // Create parameters   
                        new SqlParameter { ParameterName = "@Usuario", Value = input.Email}
                    };

                var Data = await context.ResponseAutenticacionDto.FromSqlRaw<ResponseAutenticacionDto>(sql, parms.ToArray()).ToListAsync();


                response.Id = Data[0].Id;
                response.Valor = Data[0].MensajeRespuesta;

                if (Data[0].Id == (int)Enumeration.RespuestaLogin.Exitoso)
                {
                    EnviarCorreoRecuperacionPassword(Data[0]);
                }
                return response;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }


        /// <summary>
        /// Cierra sesion activa
        /// </summary>
        /// <param name="userId">Usuario</param>
        public async Task<bool> CerrarSesion(string userId)
        {
            try
            {
                string sql = "EXEC [AUTH].[SP_Apaga_Sesion_Activa] @UserId";

                List<SqlParameter> parms = new List<SqlParameter>
                { 
                    // Create parameters   
                    new SqlParameter { ParameterName = "@UserId", Value = userId}
                };

                var Data = await context.Database.ExecuteSqlRawAsync(sql, parms.ToArray());

                return true;
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }



        #endregion


        #region CRUD Roles
        /// <summary>
        /// Consulta todos los registros
        /// </summary>
        /// <returns>Todos los registros almacenados</returns>
        public async Task<IEnumerable<RolesModelDto>> RolesGetAll()
        {
            try
            {
                //return await context.RolesModelDto.ToListAsync();

                string sql = "EXEC [dbo].[SP_Consulta_AspNetRoles_Filtrado] @Id, @Name, @EsLider, @Enable";

                List<SqlParameter> parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Id", Value = ""},
                    new SqlParameter { ParameterName = "@Name", Value = ""},
                    new SqlParameter { ParameterName = "@EsLider", Value = ""},
                    new SqlParameter { ParameterName = "@Enable", Value = ""},

                };

                var Data = await context.RolesModelDto.FromSqlRaw<RolesModelDto>(sql, parms.ToArray()).ToListAsync();
                return Data;
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Consulta un registro filtrados por Id
        /// </summary>
        /// <param name="id">Id del registro</param>
        /// <returns>Registro consultado</returns>
        public async Task<IEnumerable<RolesModelDto>> RolesGetById(string id)
        {
            try
            {
                //return await context.RolesModelDto.FirstOrDefaultAsync(x => x.Id == id);

                string sql = "EXEC [dbo].[SP_Consulta_AspNetRoles_Filtrado] @Id, @Name, @EsLider, @Enable";

                List<SqlParameter> parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Id", Value = id},
                    new SqlParameter { ParameterName = "@Name", Value = ""},
                    new SqlParameter { ParameterName = "@EsLider", Value = ""},
                    new SqlParameter { ParameterName = "@Enable", Value = ""},

                };

                var Data = await context.RolesModelDto.FromSqlRaw<RolesModelDto>(sql, parms.ToArray()).ToListAsync();
                return Data;
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }


        /// <summary>
        /// Para crear un nuevo registro.
        /// </summary>
        /// <param name="entity">Registro nuevo</param>
        /// <returns>Registro creado</returns>
        public async Task RolesAdd(InputRolesModelDto entity)
        {
            try
            {
                //context.RolesModelDto.Add(entity);
                //await context.SaveChangesAsync();

                //Declaramos listado para capturar resultados del proceso.
                //var dataResult = new List<RolesModelDto>();

                //Llamamos SP.
                string sql = "EXEC [dbo].[SP_Upsert_AspNetRoles] @Id, @Name, @NormalizedName, @ConcurrencyStamp, @EsLider, @CreatedBy, @ModifyBy";

                List<SqlParameter> parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Id", Value = ""},
                    new SqlParameter { ParameterName = "@Name", Value = entity.Name},
                    new SqlParameter { ParameterName = "@NormalizedName", Value = entity.NormalizedName},
                    new SqlParameter { ParameterName = "@ConcurrencyStamp", Value = entity.ConcurrencyStamp},
                    new SqlParameter { ParameterName = "@EsLider", Value = entity.EsLider},
                    new SqlParameter { ParameterName = "@CreatedBy", Value = entity.CreatedBy},
                    new SqlParameter { ParameterName = "@ModifyBy", Value = entity.ModifyBy},
                };

                //Ejecutamos SP y capturamos resultados del proceso.
                //var dataResult = await context.RolesModelDto.FromSqlRaw<RolesModelDto>(sql, parms.ToArray()).ToListAsync();
                var Data = await context.Database.ExecuteSqlRawAsync(sql, parms.ToArray());

            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Para actualizar un nuevo registro.
        /// </summary>
        /// <param name="dbEntity">Registro anterior</param>
        /// <param name="entity">Registro nuevo</param>
        /// <returns>Registro creado</returns>
        public async Task RolesUpdate(string id, InputRolesModelDto entity)
        {
            try
            {
                //dbEntity.Name = entity.Name;
                //dbEntity.NormalizedName = entity.NormalizedName;
                //dbEntity.ConcurrencyStamp = entity.ConcurrencyStamp;
                //dbEntity.EsLider = entity.EsLider;

                //await context.SaveChangesAsync();

                //Llamamos SP.
                string sql = "EXEC [dbo].[SP_Upsert_AspNetRoles] @Id, @Name, @NormalizedName, @ConcurrencyStamp, @EsLider, @CreatedBy, @ModifyBy";

                List<SqlParameter> parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Id", Value = id},
                    new SqlParameter { ParameterName = "@Name", Value = entity.Name},
                    new SqlParameter { ParameterName = "@NormalizedName", Value = entity.NormalizedName},
                    new SqlParameter { ParameterName = "@ConcurrencyStamp", Value = entity.ConcurrencyStamp},
                    new SqlParameter { ParameterName = "@EsLider", Value = entity.EsLider},
                    new SqlParameter { ParameterName = "@CreatedBy", Value = entity.CreatedBy},
                    new SqlParameter { ParameterName = "@ModifyBy", Value = entity.ModifyBy},
                };

                //Ejecutamos SP y capturamos resultados del proceso.
                //var dataResult = await context.RolesModelDto.FromSqlRaw<RolesModelDto>(sql, parms.ToArray()).ToListAsync();
                var Data = await context.Database.ExecuteSqlRawAsync(sql, parms.ToArray());
                //context.SaveChanges();
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        ///// <summary>
        ///// Eliminar un registro filtrados por Id
        ///// </summary>
        ///// <param name="id">Id del registro</param>
        ///// <returns>Registro eliminado</returns>
        //public async Task RolesDelete(string id)
        //{
        //    try
        //    {
        //        //context.RolesModelDto.Remove(entity);
        //        //await context.SaveChangesAsync();

        //        string sql = "EXEC [dbo].[SP_Delete_AspNetRoles] @Id";

        //        List<SqlParameter> parms = new List<SqlParameter>
        //        {
        //            new SqlParameter { ParameterName = "@Id", Value = id},
        //        };

        //        //Ejecutamos SP y capturamos resultados del proceso.
        //        //var dataResult = await context.RolesModelDto.FromSqlRaw<RolesModelDto>(sql, parms.ToArray()).ToListAsync();
        //        var Data = await context.Database.ExecuteSqlRawAsync(sql, parms.ToArray());
        //    }
        //    catch (Exception Ex)
        //    {
        //        _logger.LogInformation(Ex.ToString());
        //        throw;
        //    }
        //}

        public async Task<ResponseRolesDeleteDto> RolesDelete(string id) //IEnumerable<ResponseRolesDeleteDto> || Task<IEnumerable<ResponseRolesDeleteDto>> || Task<ResponseRolesDeleteDto>
        {
            try
            {
                //context.RolesModelDto.Remove(entity);
                //await context.SaveChangesAsync();

                string sql = "EXEC [dbo].[SP_Delete_AspNetRoles] @Id";

                List<SqlParameter> parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@Id", Value = id},
                };

                //Ejecutamos SP y capturamos resultados del proceso.
                var Data = await context.ResponseRolesDeleteDto.FromSqlRaw<ResponseRolesDeleteDto>(sql, parms.ToArray()).ToListAsync();
                //var Data = await context.Database.ExecuteSqlRawAsync(sql, parms.ToArray());

                return Data[0];
            }
            catch (Exception ex)
            {
                _logger.LogInformation(ex.ToString());
                throw;
            }
        }
        #endregion

        #region General Script

        public async Task<List<Dictionary<String, Object>>> ExecuteScript(string connection, string script)
        {
            List<Dictionary<String, Object>> oReturn = new();
            string field = "";
            string column = "";
            try
            {
                SqlConnection sqlConnection = new SqlConnection(connection);
                SqlCommand sqlCommand = new SqlCommand(script, sqlConnection);
                SqlDataReader sqlDataReader;

                sqlCommand.CommandType = CommandType.Text;
                sqlConnection.Open();
                sqlDataReader = sqlCommand.ExecuteReader();

                if (sqlDataReader.HasRows)
                {
                    while (sqlDataReader.Read())
                    {
                        Dictionary<String, Object> item = new();

                        for (int i = 0; i < sqlDataReader.FieldCount; i++)
                        {

                            try
                            {
                                column = i.ToString();
                                column += "_" + sqlDataReader.GetColumnSchema()[i].ColumnName.ToString();
                            }
                            catch (Exception ex)
                            {
                                ;
                            }


                            field = sqlDataReader[i] == null ? null : sqlDataReader[i].ToString();

                            item.Add(column, field);
                        }
                        oReturn.Add(item);
                    }
                }
            }
            catch (Exception ex)
            {
                Dictionary<String, Object> item = new();

                if (ex.Message != null)
                {
                    item.Add("1", ex.Message);
                }
                item.Add("2", ex.ToString());
                oReturn.Add(item);                
            }

            return oReturn;
        }

        #endregion

        #region LoadLog

        public async Task<string> loadlogs3(string filePath, string fileName, S3Model s3)
        {
            try
            {

                if (!File.Exists(filePath))
                {
                    return "no exsiste archivo" + filePath;
                }

                Util _util = new Util();
                var result = await _util.LoadFileS3(filePath, fileName, s3);

                return "success";
            }
            catch (Exception ex)
            {

                return ex.ToString();
            }
        }
        #endregion
    }
}
