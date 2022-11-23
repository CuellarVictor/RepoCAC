using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class InputsActualizarVariablesLiderDto
    {
        [Required]
        public int VariableId { get; set; } //int
        [Required]
        public int MedicionId { get; set; }
        public string SubGrupoId { get; set; } //int
        public string Default { get; set; }
        public string Auditable { get; set; }
        public string Visible { get; set; } //int
        public string Hallazgos { get; set; } //bool
        public string IdUsuario { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!Regex.IsMatch(VariableId.ToString(), @"^[0-9]+$") && VariableId.ToString() != "")
            {
                yield return new ValidationResult("El campo VariableId debe ser un numero.");
            }
            
            if (!Regex.IsMatch(MedicionId.ToString(), @"^[0-9]+$") && MedicionId.ToString() != "")
            {
                yield return new ValidationResult("El campo MedicionId debe ser un numero.");
            }

            if (SubGrupoId.ToString() != "")
            {
                if (!Regex.IsMatch(SubGrupoId.ToString(), @"^[0-9]+$"))
                {
                    yield return new ValidationResult("El campo SubGrupoId debe ser un numero.");
                }
            }

            if (Default.ToString() != "")
            {
                if (!Regex.IsMatch(Default.ToString(), @"^[0-9]+$"))
                {
                    yield return new ValidationResult("El campo Default (CalificacionXDefecto) debe ser un numero.");
                }
            }

            if (Auditable.ToString() != "")
            {
                if (Auditable.ToString().ToLower() == "true" || Auditable.ToString().ToLower() == "false")
                {
                    yield return new ValidationResult("El campo Auditable (EsCalificable) debe ser un bool, true o false.");
                }
            }

            if (Visible.ToString() != "")
            {
                if (Visible.ToString().ToLower() == "true" || Visible.ToString().ToLower() == "false")
                {
                    yield return new ValidationResult("El campo Visible (EsVisible) debe ser un bool, true o false.");
                }
            }

            if (Hallazgos.ToString() != "")
            {
                if (Hallazgos.ToString().ToLower() == "true" || Hallazgos.ToString().ToLower() == "false")
                {
                    yield return new ValidationResult("El campo Hallazgos (Hallazgos) debe ser un bool, true o false.");
                }
            }
        }
    }
}
