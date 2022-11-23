using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class DetalleAsignacionOutputModel
    {
        public DetalleAsignacionOutputModel()
        {

        }

        public DetalleAsignacionOutputModel(ResponseRegistrosAuditoriaDetallesAsignacionDto contextModel)
        {
            this.Order = contextModel.Order;
            this.Nombre = contextModel.Nombre;
            this.NoRegistros = contextModel.NoRegistros;
            this.Extemporaneo = contextModel.Extemporaneo;
            this.Estados = new List<string>();

            if (!string.IsNullOrEmpty(contextModel.Estados))
            {
                contextModel.Estados.Split(",")?.All(x =>
                {
                    Estados.Add(x);
                    return true;
                });
            }
        }

        public int Order { get; set; }
        public string Nombre { get; set; }
        public string NoRegistros { get; set; }
        public int Extemporaneo { get; set; }
        public List<string> Estados { get; set; }
    }
}
