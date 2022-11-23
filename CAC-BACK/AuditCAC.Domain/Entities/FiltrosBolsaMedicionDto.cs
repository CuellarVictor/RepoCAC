using AuditCAC.Domain.Dto;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Entities
{
    public class FiltrosBolsaMedicionDto
    {
        public List<CoberturaModel> coberturasAsignadas { get; set; }
        public List<CoberturaModel> coberturasAsociadas { get; set; }
        public List<PeriodoModel> periodosAsignados { get; set; }
        public List<ResponseMedicionCrudoDto> bolsasDeMedicion { get; set; } //MedicionModel
        public List<CantidadRegistrosModel> cantidadRegistros { get; set; }

    }

}
