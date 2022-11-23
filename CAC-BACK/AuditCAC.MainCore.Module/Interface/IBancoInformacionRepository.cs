using AuditCAC.Domain.Dto;
using AuditCAC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.MainCore.Module.Interface
{
    public interface IBancoInformacionRepository<TEntity>
    {
        Task<IEnumerable<BancoInformacionModel>> GetAll();
        Task<BancoInformacionModel> Get(int id);
        Task Add(BancoInformacionModel entity);
        Task Update(BancoInformacionModel dbEntity, BancoInformacionModel entity);
        Task Delete(BancoInformacionModel entity);
        Task<List<ResponseBancoInformacionDto>> GetBancoInformacionByPalabraClave(InputsBancoInformacionDto inputsDto);

        /// <summary>
        /// Para consultar todos los registros de Banco de Informacion creados segun los filtrados ingresados.
        /// </summary>
        /// <param name="inputsDto">Entrada de datos</param>
        /// <returns>Modelo de datos</returns>
        Task<Tuple<List<ResponseBancoInformacionFiltradoDto>, int, int, int>> GetBancoInformacionFiltrado(InputsBancoInformacionFiltradoDto inputsDto);

        /// <summary>
        /// Para consultar info usada en cargar datos de Banco de Informacion.
        /// </summary>
        /// <param name="filePath">Ruta del archivo a cargar</param>
        /// <param name="input">Modelo Cargue</param>
        /// <returns>Modelo Current Process</returns>        
        Task<ResponseReasignacionesBolsaDetalladaDto> CargarBancoInformacionPlantilla(string filePath);

        /// <summary>
        /// Para cargar datos de Banco de Informacion.
        /// </summary>
        /// <param name="filePath">Ruta del archivo a cargar</param>
        /// <param name="input">Modelo Cargue</param>
        /// <returns>Modelo Current Process</returns>        
        Task<ResponseReasignacionesBolsaDetalladaDto> CargarBancoInformacion(string filePath, InputCargarBancoInformacionDto input, S3Model _s3);
    }
}
