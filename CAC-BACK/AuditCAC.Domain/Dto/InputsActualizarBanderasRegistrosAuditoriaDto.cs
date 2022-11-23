using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class InputsActualizarBanderasRegistrosAuditoriaDto: IValidatableObject
    {
        [Required]
        public int IdUnico { get; set; }
        public string LevantarGlosa { get; set; }
        public string MantenerCalificacion { get; set; }
        public string ComiteExperto { get; set; }
        public string ComiteAdministrativo { get; set; }
        public string AccionLider { get; set; }
        public string AccionAuditor { get; set; }

        //Validamos datos.
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            //if (IdUnico == null) //!Regex.IsMatch(IdUnico, @"^[0-9]+$")
            //{
            //    yield return new ValidationResult("El IdUnico debe ser ingresado.");
            //}

            if (IdUnico <= 0) //!Regex.IsMatch(IdUnico, @"^[0-9]+$")
            {
                yield return new ValidationResult("El IdUnico no puede ser 0.");
            }

            if (!Regex.IsMatch(LevantarGlosa, @"^[0-9]+$") && LevantarGlosa != "")
            {
                yield return new ValidationResult("El campo LevantarGlosa debe ser un numero.");
            }

            if (!Regex.IsMatch(MantenerCalificacion, @"^[0-9]+$") && MantenerCalificacion != "")
            {
                yield return new ValidationResult("El campo MantenerCalificacion debe ser un numero.");
            }

            if (!Regex.IsMatch(ComiteExperto, @"^[0-9]+$") && ComiteExperto != "")
            {
                yield return new ValidationResult("El campo ComiteExperto debe ser un numero.");
            }

            if (!Regex.IsMatch(ComiteAdministrativo, @"^[0-9]+$") && ComiteAdministrativo != "")
            {
                yield return new ValidationResult("El campo ComiteAdministrativo debe ser un numero.");
            }

            if (!Regex.IsMatch(AccionLider, @"^[0-9]+$") && AccionLider != "")
            {
                yield return new ValidationResult("El campo AccionLider debe ser un numero.");
            }

            if (!Regex.IsMatch(AccionAuditor, @"^[0-9]+$") && AccionAuditor != "")
            {
                yield return new ValidationResult("El campo AccionAuditor debe ser un numero.");
            }
        }
    }
}
