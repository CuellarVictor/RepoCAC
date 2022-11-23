using AuditCAC.Dal.Data;
using AuditCAC.Domain.Entities;
using AuditCAC.MainCore.Module.Interface;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.MainCore.Module
{
    public class ItemManager : IItemRepository<ItemModel>
    {
        private readonly DBAuditCACContext _dBAuditCACContext;
        private readonly ILogger<ItemModel> _logger;

        //Constructor
        public ItemManager(DBAuditCACContext dBAuditCACContext, ILogger<ItemModel> logger)
        {
            this._dBAuditCACContext = dBAuditCACContext;
            _logger = logger;
        }

        //Metodos.
        public async Task<IEnumerable<ItemModel>> GetAll()
        {
            try
            {
                return await _dBAuditCACContext.ItemModel.Where(x => x.Status == true).ToListAsync();
                //return await this.dBAuditCACContext.ItemModel.ToListAsync();
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        public async Task<ItemModel> Get(int id)
        {
            try
            {
                return await _dBAuditCACContext.ItemModel.Where(x => x.Status == true).FirstOrDefaultAsync(x => x.Id == id);
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        public async Task<IEnumerable<ItemModel>> GetItemByCatalogId(int CatalogId)
        {
            try
            {
                return await _dBAuditCACContext.ItemModel.Where(x => x.Status == true).Where(x => x.CatalogId == CatalogId).ToListAsync();
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        public async Task Add(ItemModel entity, string UsuarioId)
        {
            try
            {
                _dBAuditCACContext.ItemModel.Add(entity);
                await _dBAuditCACContext.SaveChangesAsync();
                //return NoContent;

                // CatalogId = 4: Es Grupos de variables
                // CatalogId = : Código Administrativo                

                // Variables para Insertar Log
                int ProcessId = 0;
                string User = "";
                string Observation = "";

                // Validamos tipo de catalogo
                if (entity.CatalogId == 4)
                {
                    ProcessId = 29;
                    User = UsuarioId == null ? "": UsuarioId;
                    Observation = "Configuraciones - Catálogos (Grupo de variables): Nuevo";

                    //Llamamos SP para Insertar Logs de actividad.                
                    string sql = "EXEC [SP_Insertar_Process_Log] @ProcessId, @User, @Result, @Observation";
                    List<SqlParameter> parms = new List<SqlParameter>
                    {
                        new SqlParameter { ParameterName = "@ProcessId", Value = ProcessId},
                        new SqlParameter { ParameterName = "@User", Value = User},
                        new SqlParameter { ParameterName = "@Result", Value = "OK"},
                        new SqlParameter { ParameterName = "@Observation", Value = Observation}
                    };

                    var respuesta = _dBAuditCACContext.Database.ExecuteSqlRaw(sql, parms.ToArray());
                }
                //else if (entity.CatalogId == 2)
                //{
                //    ProcessId = 32;
                //    User = UsuarioId == null ? "" : UsuarioId;
                //    Observation = "Configuraciones - Catálogos (Catálogos): Nuevo";
                //}

            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        public async Task Update(ItemModel entity, string UsuarioId)
        {
            try
            {
                var item = _dBAuditCACContext.ItemModel.Where(x => x.Id == entity.Id).FirstOrDefault();
                item.CatalogId = entity.CatalogId;
                item.ItemName = entity.ItemName;
                item.Concept = entity.Concept;
                item.Enable = entity.Enable;
                item.LastModify = DateTime.Now;

                _dBAuditCACContext.SaveChanges();

                // Variables para Insertar Log
                int ProcessId = 0;
                string User = "";
                string Observation = "";

                // Validamos tipo de catalogo
                if (entity.CatalogId == 4)
                {
                    ProcessId = 29;
                    User = UsuarioId == null ? "" : UsuarioId;
                    Observation = "Configuraciones - Catálogos (Grupo de variables): Editar";
                }
                else if (entity.CatalogId == 2)
                {
                    ProcessId = 32;
                    User = UsuarioId == null ? "" : UsuarioId;
                    Observation = "Configuraciones - Catálogos (Catálogos): Editar";
                }

                //Llamamos SP para Insertar Logs de actividad.                
                string sql = "EXEC [SP_Insertar_Process_Log] @ProcessId, @User, @Result, @Observation";
                List<SqlParameter> parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@ProcessId", Value = ProcessId},
                    new SqlParameter { ParameterName = "@User", Value = User},
                    new SqlParameter { ParameterName = "@Result", Value = "OK"},
                    new SqlParameter { ParameterName = "@Observation", Value = Observation}
                };

                var respuesta = _dBAuditCACContext.Database.ExecuteSqlRaw(sql, parms.ToArray());
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }

        public async Task Delete(ItemModel entity, string UsuarioId)
        {
            try
            {
                //_dBAuditCACContext.ItemModel.Remove(entity);
                //await _dBAuditCACContext.SaveChangesAsync();

                entity.Status = false;

                await _dBAuditCACContext.SaveChangesAsync();

                // Variables para Insertar Log
                int ProcessId = 0;
                string User = "";
                string Observation = "";

                // Validamos tipo de catalogo
                if (entity.CatalogId == 4)
                {
                    ProcessId = 29;
                    User = UsuarioId == null ? "" : UsuarioId;
                    Observation = "Configuraciones - Catálogos (Grupo de variables): Eliminar";
                }
                else if (entity.CatalogId == 2)
                {
                    ProcessId = 32;
                    User = UsuarioId == null ? "" : UsuarioId;
                    Observation = "Configuraciones - Catálogos (Catálogos): Eliminar";
                }

                //Llamamos SP para Insertar Logs de actividad.                
                string sql = "EXEC [SP_Insertar_Process_Log] @ProcessId, @User, @Result, @Observation";
                List<SqlParameter> parms = new List<SqlParameter>
                {
                    new SqlParameter { ParameterName = "@ProcessId", Value = ProcessId},
                    new SqlParameter { ParameterName = "@User", Value = User},
                    new SqlParameter { ParameterName = "@Result", Value = "OK"},
                    new SqlParameter { ParameterName = "@Observation", Value = Observation}
                };

                var respuesta = _dBAuditCACContext.Database.ExecuteSqlRaw(sql, parms.ToArray());
            }
            catch (Exception Ex)
            {
                _logger.LogInformation(Ex.ToString());
                throw;
            }
        }
    }
}
