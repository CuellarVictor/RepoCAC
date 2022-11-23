using ApiAuditCAC.Domain.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ApiAuditCAC.Data.DataAccess
{
    public interface IProcedures
    {
        public String GetColums(int Id);
        public Dictionary<String, Object> GetData(int Id, String columna, String tabla);
        public List<SpRegistrosAudResponseDto> GetIdsAudByMedicion(int idMedicion);
        public List<int> GetMediciones();
        public String GetParametrosGeneralesByName(String nombre);
        public String GetObservaciones(int id);

        public List<CoberturaDto> ConsultarCoberturas();
        public List<RegistrosAuditoriaDto> ConsultaRegistrosAuditoriaPorMedicion(int id);

        public List<RegistrosAuditoriaSeguimientoDto> ConsultaRegistrosAuditoriaObservacionesPorMedicion(int id);

        public List<HallazgosDto> ConsultaHallazgosPorMedicion(int id);
        public List<SoportesDto> ConsultaSoportesPorMedicion(int id);

        public List<CalificacionDto> ConsultaCalificacionPorMedicion(int id);
        public List<RegistroAuditoriaDetalleErrores> ConsultaErroresRegistroAuditarMedicion(int id);
        public List<VariablesAsociadasDto> ConsultaVariablesAsociadaMedicion(int id);

        public List<ComodinVariableDto> ConsultaComodinesMedicion(int id);
        public List<UsuarioDto> ConsultaUsuarios();
    }
}
