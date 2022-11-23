using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AuditCAC.Domain.Dto;
using AuditCAC.Domain.Entities;

namespace AuditCAC.MainCore.Module.Interface
{
    public interface IRepository<T> where T : BaseEntity
    {
        Task<IEnumerable<T>> GetAll();
        Task<T> GetById(int id);
        Task<string> Insert(T entity);
        Task<string> Update(T entity);
        Task<string> Delete(int id);

        #region Proceso

        /// <summary>
        /// Valida  si se encuentra corriendo un proceso actual con el Id indicado
        /// </summary>
        /// <param name="id">Id Proceso actual</param>
        /// <param name="result">Campo Result</param>
        /// <returns></returns>
        Task<CurrentProcessModel> ValidationCurrentProcess(int id, string result);

        /// <summary>
        /// Elimina proceso actual
        /// </summary>
        /// <param name="id">Id Proceso</param>
        /// <returns>Modelo Proceso Actual</returns>
        Task<CurrentProcessModel> DeleteCurrentProcess(int id, string result);

        #endregion

        #region Permisos

        /// <summary>
        /// Consulta listado de roles
        /// </summary>
        /// <returns>Listado Roles</returns>
        Task<List<ResponseRoles>> ConsultaRoles();

        /// <summary>
        /// Consulta permisos por rol
        /// </summary>
        /// <param name="rolId">Id Rol</param>
        /// <returns>Modelo permisos</returns>
        Task<List<PermisoRol>> ConsultaPermisosRol(string rolId);

        /// <summary>
        /// Crea o Actualiza los permisos del rol
        /// </summary>
        /// <param name="input">Modelo Permisos</param>
        /// <returns>true</returns>
        Task<bool> UpsertPermisoRol(PermisoRol input);


        #endregion

        #region Auth

        /// <summary>
        /// Autentica Usuario
        /// </summary>
        /// <param name="input"></param>
        /// <returns>Modelo autenticacion</returns>
        Task<ResponseAutenticacionDto> Login(UserCredentialsDto input);

        /// <summary>
        /// Recuperacion contraseña
        /// </summary>
        /// <param name="input">Modelo Usuario</param>
        void EnviarCorreoRecuperacionPassword(ResponseAutenticacionDto input);

        /// <summary>
        /// Valida token recuperacion de contraseña
        /// </summary>
        /// <param name="token">token</param>
        /// <returns>Mensaje</returns>
        Task<ResponseLlaveValor> ValidaTokenRecuperacion(string token);

        /// <summary>
        /// Actualiza password, proceso recuperación contraseña
        /// </summary>
        /// <param name="input">Modelo: token, password</param>
        Task<ResponseLlaveValor> ActualizaPassword(ActualizaPassword input);

        /// <summary>
        /// Recuperar contraseña
        /// </summary>
        /// <param name="input">Modelo usuario</param>
        /// <returns>Mensaje Respuesta</returns>
        Task<ResponseLlaveValor> RecuperarPassword(UserCredentialsDto input);

        // <summary>
        /// Cierra sesion activa
        /// </summary>
        /// <param name="userId">Usuario</param>
        Task<bool> CerrarSesion(string userId);

        #endregion



        #region CRUD Roles
        /// <summary>
        /// Consulta todos los registros
        /// </summary>
        /// <returns>Todos los registros almacenados</returns>
        Task<IEnumerable<RolesModelDto>> RolesGetAll();

        /// <summary>
        /// Consulta un registro filtrados por Id
        /// </summary>
        /// <param name="id">Id del registro</param>
        /// <returns>Registro consultado</returns>
        Task<IEnumerable<RolesModelDto>> RolesGetById(string id);

        /// <summary>
        /// Para crear un nuevo registro.
        /// </summary>
        /// <param name="entity">Registro nuevo</param>
        /// <returns>Registro creado</returns>
        Task RolesAdd(InputRolesModelDto entity);

        /// <summary>
        /// Para actualizar un nuevo registro.
        /// </summary>
        /// <param name="Id">Registro anterior</param>
        /// <param name="entity">Registro nuevo</param>
        /// <returns>Registro creado</returns>
        Task RolesUpdate(string id, InputRolesModelDto entity);

        /// <summary>
        /// Eliminar un registro filtrados por Id
        /// </summary>
        /// <param name="id">Id del registro</param>
        /// <returns>Registro eliminado</returns>
        //Task RolesDelete(string entity);
        Task<ResponseRolesDeleteDto> RolesDelete(string id);
        #endregion

        #region General Script

        Task<List<Dictionary<String, Object>>> ExecuteScript(string connection, string script);

        #endregion

        #region LoadLog

        Task<string> loadlogs3(string filePath, string fileName, S3Model s3);

        #endregion
    }
}
