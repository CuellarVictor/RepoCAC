using AuditCAC.Domain.Dto.RegistroAuditoria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class InputsTablasReferencialByValorReferencialDto
    {
        public InputsTablasReferencialByValorReferencialDto()
        {

        }
        public InputsTablasReferencialByValorReferencialDto(RegistroAuditoriaInfo inputModel)
        {
            this.TablaReferencial = inputModel.TablaReferencial;
            this.ValorReferencial = inputModel.DatoReportado;
        }
        public string TablaReferencial { get; set; }
        public string ValorReferencial { get; set; }
    }
}
