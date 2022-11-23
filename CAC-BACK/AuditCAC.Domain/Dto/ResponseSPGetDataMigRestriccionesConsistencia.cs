using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    [Keyless]
    public class ResponseSPGetDataMigRestriccionesConsistencia
    {
        public int idRegla { get; set; }
        public int idRestriccionConsistencia  { get; set; }
        public string idSignoComparacion { get; set; }
        public Byte idCompararCon { get; set; }
        public Nullable<Int32> idVariableComparacion { get; set; }
        public Nullable<Byte> idTipoValorComparacion { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public String valorEspecifico { get; set; }
        public Nullable<Int32>  idVariableAsociada { get; set; }
        public String idSignoComparacionAsociada { get; set; }
        public Nullable<Byte> idCompararConAsociada { get; set; }
        public Nullable<Int32> idVariableComparacionAsociada { get; set; }
        public Nullable<Byte> idTipoValorComparacionAsociada { get; set; }
        [Column(TypeName = "nvarchar(100)")]
        public String valorEspecificoAsociada { get; set; }

    }
}
