using AuditCAC.Domain.Dto.Catalogo;
using AuditCAC.MainCore.Module.Catalogo.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace AuditCAC.Services.Controllers.Catalogo
{
    [Route("api/[controller]")]
    [ApiController]
    public class NewCatalogoController : ControllerBase
    {
        #region Dependency
        private readonly ICatalogoManager _catalogo;
        private readonly IItemCatalogoManager _item;
        private static readonly log4net.ILog _log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        #endregion

        #region Constructor
        public NewCatalogoController(ICatalogoManager catalogo, IItemCatalogoManager item)
        {
            _catalogo = catalogo;
            _item = item;
        }
        #endregion

        #region Methods
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                //Obtenemos todos los registros.
                var Data = await _catalogo.GetAll();

                //Retornamos datos.
                return Data != null ? Ok(Data) : (IActionResult)(NoContent());
            }
            catch (Exception ex)
            {
                _log.Fatal("Error", ex);
                throw new Exception("Error", ex);
            }
        }

        [HttpGet]
        [Route("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            try
            {
                //Obtenemos todos los registros.
                var Data = await _catalogo.GetById(id);

                //Retornamos datos.
                return Data != null ? Ok(Data) : (IActionResult)(NoContent());
            }
            catch (Exception ex)
            {
                _log.Fatal("Error", ex);
                throw new Exception("Error", ex);
            }
        }


        [HttpPost]
        [Route("Agregar")]
        public async Task<IActionResult> Add(CatalogoDto model)
        {
            try
            {
                //Obtenemos todos los registros.
                var Data = await _catalogo.AddOrUpdate(model);

                //Retornamos datos.
                return Data != false ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {
                _log.Fatal("Error", ex);
                throw new Exception("Error", ex);
            }
        }

        [HttpPost]
        [Route("Actualizar")]
        public async Task<IActionResult> Update(CatalogoDto model)
        {
            try
            {
                //Obtenemos todos los registros.
                var Data = await _catalogo.AddOrUpdate(model);

                //Retornamos datos.
                return Data != false ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {
                _log.Fatal("Error", ex);
                throw new Exception("Error", ex);
            }
        }

        [HttpGet]
        [Route("Eliminar/{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                //Obtenemos todos los registros.
                var Data = await _catalogo.Delete(id);

                //Retornamos datos.
                return Data != false ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {
                _log.Fatal("Error", ex);
                throw new Exception("Error", ex);
            }
        }

        [HttpGet]
        [Route("EliminarItem/{id}")]
        public async Task<IActionResult> DeleteItem(int id)
        {
            try
            {
                //Obtenemos todos los registros.
                var Data = await _item.Delete(id);

                //Retornamos datos.
                return Data != false ? Ok(Data) : NoContent();
            }
            catch (Exception ex)
            {
                _log.Fatal("Error", ex);
                throw new Exception("Error", ex);
            }
        }

        [HttpGet]
        [Route("GetItemById/{id}")]
        public async Task<IActionResult> GetItemById(int id)
        {
            try
            {
                //Obtenemos todos los registros.
                var Data = await _item.GetById(id);

                //Retornamos datos.
                return Data != null ? Ok(Data) : (IActionResult)(NoContent());
            }
            catch (Exception ex)
            {
                _log.Fatal("Error", ex);
                throw new Exception("Error", ex);
            }
        }
        #endregion
    }
}
