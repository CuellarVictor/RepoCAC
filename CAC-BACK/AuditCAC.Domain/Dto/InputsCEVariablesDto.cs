using AuditCAC.Domain.Dto.ReglaVariable;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class InputsCEVariablesDto
    {
        public int Variable { get; set; }
        public int? Orden { get; set; }
        public int? idCobertura { get; set; }
        public string nombre { get; set; }
        public string descripcion { get; set; }
        public string idTipoVariable { get; set; }
        public int Decimales { get; set; }
        public int Longitud { get; set; }
        public string Formato { get; set; }
        public string tablaReferencial { get; set; }
        public string CreatedBy { get; set; }
        public string ModifyBy { get; set; }
        public int TipoVariableItem { get; set; }
        public int? EstructuraVariable { get; set; }
        public bool Alerta { get; set; }
        public string AlertaDescripcion { get; set; }
        //
        public List<int> MedicionId { get; set; } 
        public bool EsVisible { get; set; }
        public bool EsCalificable { get; set; }
        public bool Hallazgos { get; set; }        
        public bool EnableDC { get; set; }
        public bool EnableNC { get; set; }
        public bool EnableND { get; set; }
        public int CalificacionXDefecto { get; set; }
        public int SubGrupoId { get; set; }               
        public bool Encuesta { get; set; }
        public List<int> CalificacionIPSItem { get; set; }
        public ReglasVariablesDto ReglaVariable { get; set; }

        public int TipoCampo { get; set; }
        public bool Promedio { get; set; }
        public bool ValidarEntreRangos { get; set; }
        public string Desde { get; set; }
        public string Hasta { get; set; }
        public bool Condicionada { get; set; }
        public string ValorConstante { get; set; }
        public bool Lista { get; set; }
        public List<VariableCondicionalDto> VariableCondicional { get; set; }
        public bool? Calculadora { get; set; }
        public int? TipoCalculadora { get; set; }
    }
}
