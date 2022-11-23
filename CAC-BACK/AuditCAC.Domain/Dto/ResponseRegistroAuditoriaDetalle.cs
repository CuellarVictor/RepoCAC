using AuditCAC.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class ResponseRegistroAuditoriaDetalle
    {
        public ResponseRegistroAuditoriaDetalle()
        {

        }

        public string name { get; set; }
        public int idgrupo { get; set; }
        public int SubGrupoOrden { get; set; }
        public string error { get; set; }
        public List<Variable> variables { get; set; }

        public class Variable
        {
            public string reducido { get; set; }
            public string estado { get; set; }
            public string detalle { get; set; }
            public string hallazgo { get; set; }
            public List<RegistroAuditoriaDetalleErrorGroupModel> error { get; set; }
            public string seleccion { get; set; }
            public string dato_reportado { get; set; }
            public string motivo { get; set; }
            public List<string> listaMotivos { get; set; }
            public int registroAuditoriaDetalleId { get; set; }
            public int? Dato_DC_NC_ND { get; set; }
            public bool? Visible { get; set; }
            public bool? Bot { get; set; }
            public int VariableId { get; set; }
            public bool? VariableEncuesta { get; set; }
            public bool? RegistrosAuditoriaEncuesta { get; set; }
            public string Nombre { get; set; } //public string IpsNombre { get; set; }
            public string TablaReferencial { get; set; }
            public string CampoReferencial { get; set; }
            public string IdTipoVariable { get; set; }
            public int? Longitud { get; set; }
            public int? Decimales { get; set; }
            public string Formato { get; set; }
            public string ValorDatoReportado { get; set; }
            public bool EnableError { get; set; }
            public int Orden { get; set; }
            public int TipoVariableId { get; set; }
            public string TipoVariableNombre { get; set; }
            public bool EnableDC { get; set; }
            public bool EnableNC { get; set; }
            public bool EnableND { get; set; }
            public bool Condicionada { get; set; }
            public string ValorConstante { get; set; }
            public string DescripcionVariable { get; set; }
            public bool Alerta { get; set; }
            public string AlertaDescripcion { get; set; }
            public int  MedicionId { get; set; }
            public bool EsVisible { get; set; }
            public bool EsCalificable { get; set; }
            public int CountHallazgos { get; set; }

            public ContextoDto Contexto { get; set; }
            public bool? Calculadora { get; set; }
            public int? TipoCalculadora { get; set; }
            public int EstadoMedicion { get; set; }
        }


    }
}

