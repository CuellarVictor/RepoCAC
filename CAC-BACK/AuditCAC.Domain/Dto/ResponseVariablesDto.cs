using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class ResponseVariablesDto
    {
        public string QueryNoRegistrosTotales { get; set; }
        [Key]
        public Guid Idk { get; set; } //List<string> NEWID() As 
        public string Id { get; set; } //List<string>
        public string Variable { get; set; }
        public bool Activa { get; set; }
        public string Orden { get; set; }
        public string idVariable { get; set; }
        public string idCobertura { get; set; }
        public string nombre { get; set; }
        public string nemonico { get; set; }
        public string descripcion { get; set; }
        public string idTipoVariable { get; set; }
        public string longitud { get; set; }
        public string decimales { get; set; }
        public string formato { get; set; }
        public string tablaReferencial { get; set; }
        public string campoReferencial { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string ModifyBy { get; set; }
        public string ModifyDate { get; set; }
        public string MotivoVariable { get; set; }
        public bool Bot { get; set; }
        public int TipoVariableItem { get; set; }
        public int EstructuraVariable { get; set; }
        //
        public string VariableId { get; set; }
        public string MedicionId { get; set; }
        public bool EsGlosa { get; set; }
        public bool EsVisible { get; set; }
        public bool EsCalificable { get; set; }
        public bool Hallazgos { get; set; }
        public bool Activo { get; set; }
        public bool EnableDC { get; set; }
        public bool EnableNC { get; set; }
        public bool EnableND { get; set; }
        public int CalificacionXDefecto { get; set; }
        public int? SubGrupoId { get; set; }
        public string SubGrupoNombre { get; set; }
        public bool Encuesta { get; set; }
        public string VxM_Orden { get; set; }

        public bool Alerta { get; set; }
        public string AlertaDescripcion { get; set; }
        public int? calificacionIPSItem { get; set; } //public List<Detalles> calificacionIPSItem { get; set; }
        public string calificacionIPSItemNombre { get; set; }
        public string IdRegla { get; set; }
        public string NombreRegla { get; set; }
        public string Concepto { get; set; }
        public string TipoVariableDesc { get; set; }
        public int TipoCampo { get; set; }
        public bool Promedio { get; set; }
        public bool ValidarEntreRangos { get; set; }
        public string Desde { get; set; }
        public string Hasta { get; set; }
        public bool Condicionada { get; set; }
        public string ValorConstante { get; set; }
        public bool Lista { get; set; }
        public bool? Calculadora { get; set; }
        public int? TipoCalculadora { get; set; }

    }

    public class ResponseVariablesDetails
    {
        public string QueryNoRegistrosTotales { get; set; }
        [Key]
        public string Id { get; set; } //List<string>
        public string Variable { get; set; }
        public bool Activa { get; set; }
        public string Orden { get; set; }
        public string idVariable { get; set; }
        public string idCobertura { get; set; }
        public string nombre { get; set; }
        public string nemonico { get; set; }
        public string descripcion { get; set; }
        public string idTipoVariable { get; set; }
        public string longitud { get; set; }
        public string decimales { get; set; }
        public string formato { get; set; }
        public string tablaReferencial { get; set; }
        public string campoReferencial { get; set; }
        public string CreatedBy { get; set; }
        public string CreatedDate { get; set; }
        public string ModifyBy { get; set; }
        public string ModifyDate { get; set; }
        public string MotivoVariable { get; set; }
        public bool Bot { get; set; }
        public int TipoVariableItem { get; set; }
        public int EstructuraVariable { get; set; }
        //
        public string VariableId { get; set; }
        public string MedicionId { get; set; }
        public bool EsGlosa { get; set; }
        public bool EsVisible { get; set; }
        public bool EsCalificable { get; set; }
        public bool Hallazgos { get; set; }
        public bool Activo { get; set; }
        public bool EnableDC { get; set; }
        public bool EnableNC { get; set; }
        public bool EnableND { get; set; }
        public int CalificacionXDefecto { get; set; }
        public int? SubGrupoId { get; set; }
        public string SubGrupoNombre { get; set; }
        public bool Encuesta { get; set; }
        public string VxM_Orden { get; set; }

        public bool Alerta { get; set; }
        public string AlertaDescripcion { get; set; }
        public List<int> calificacionIPSItem { get; set; }
        public string IdRegla { get; set; }
        public string NombreRegla { get; set; }
        public string Concepto { get; set; }
       public string TipoVariableDesc { get; set; }
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

    public class ResponseVariablesDtoDetalles
    {
        public string Id { get; set; }
        public string Valor { get; set; }
    }
}