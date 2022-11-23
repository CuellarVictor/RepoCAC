using AuditCAC.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.MainCore.Module.Interface
{
    public interface IHallazgosRepository<TEntity>
    {

        /// <summary>
        /// Consutar Hallazgos por id de radicado
        /// </summary>
        /// <param name="inputsDto">Id radicado</param>
        /// <returns>MOdelo Hallazgos</returns>
        Task<List<ResponseConsultarHallazgosDto>> ConsultarHallazgosByRadicadoId(InputsConsultarHallazgosDto inputsDto);

        /// <summary>
        /// Consulta de registros con hallazgos pendientes para enviar a la entidad por Id de Medicion (Suministro para implementacion de Modulo de hallazgos)
        /// </summary>
        /// <param name="inputsDto">(Medicion y Rango de Fechas)</param>
        /// <returns>Data para enviar a la entidad</returns>
        Task<List<ResponseConsultaHallazgosGeneradosDto>> ConsultaHallazgosGenerados(InputsConsultaHallazgosGeneradosDto inputsDto);

        /// <summary>
        /// Registra respuestas de las entidades sobre los hallazgos
        /// </summary>
        /// <param name="inputsDto">RegistroAuditoriaId, Estado, Observacion, Usuario</param>
        /// <returns>true</returns>
        Task<bool> RegistraRespuestaHallazgos(List<InputRegistraRespuestaHallazgosDto> inputsDto);
    }
}
