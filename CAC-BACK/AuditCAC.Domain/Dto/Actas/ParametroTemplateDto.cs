using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class ParametroTemplateDto
    {
        public ParametroTemplateDto()
        {

        }

        public int Id { get; set; }
        public int Template { get; set; }
        public int TipoInput { get; set; }
        public int TipoDato { get; set; }
        public string Parametro { get; set; }
        public string Valor { get; set; }
        public string Descripcion { get; set; }
        public bool Enable { get; set; }
        public DateTime CreateDate { get; set; }
        public string CreateBy { get; set; }
        public DateTime ModifyDate { get; set; }
        public string ModifyBy { get; set; }
        public bool Obligatorio { get; set; }
    }
}
