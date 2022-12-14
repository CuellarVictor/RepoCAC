using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class InputsAtarListadoVariablesMedicionDto
    {
        public int? NaturalezaVariableId { get; set; }   //Foranea
        public int? MedicionId { get; set; } //Foranea
        public bool Activa { get; set; }
        public int? Orden { get; set; }
        public int? idVariable { get; set; }
        public int? idCobertura { get; set; }
        public string nombre { get; set; }
        public string nemonico { get; set; }
        public string descripcion { get; set; }
        public string idTipoVariable { get; set; }
        public int? longitud { get; set; }
        public int? decimales { get; set; }
        public string formato { get; set; }
        public string idErrorTipo { get; set; }
        public string tablaReferencial { get; set; }
        public string campoReferencial { get; set; }
        public string idErrorReferencial { get; set; }
        public string idTipoVariableAlterno { get; set; }
        public string formatoAlterno { get; set; }
        public bool permiteVacio { get; set; }
        public string idErrorPermiteVacio { get; set; }
        public bool identificadorRegistro { get; set; }
        public bool clavePrimaria { get; set; }
        public Byte? idTipoAnalisisEpidemiologico { get; set; }
        public bool sistema { get; set; }
        public bool exportable { get; set; }
        public bool enmascarado { get; set; }
    }
}
