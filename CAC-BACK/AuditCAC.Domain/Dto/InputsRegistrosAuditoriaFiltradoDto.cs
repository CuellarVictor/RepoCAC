using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto
{
    public class InputsRegistrosAuditoriaFiltradoDto : IValidatableObject
    {
        //Paginado
        public int PageNumber { get; set; } = 0;
        public int MaxRows { get; set; } = 1000;
        [Key]
        public string Id { get; set; }
        public string IdRadicado { get; set; } //int?
        public string IdMedicion { get; set; } //int?
        public string IdPeriodo { get; set; } //int?
        public string IdLider { get; set; }  //public string IdAuditor { get; set; } //Guid
        public string IdAuditor { get; set; }
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public string Sexo { get; set; }
        public string TipoIdentificacion { get; set; } //Varchar de 2 caracteres
        public string Identificacion { get; set; } //int?
        //
        public string FechaNacimientoIni { get; set; } //DateTime?
        public string FechaNacimientoFin { get; set; } //DateTime?
        //
        public string FechaCreacionIni { get; set; } //DateTime?
        public string FechaCreacionFin { get; set; } //DateTime?
        //
        public string FechaAuditoriaIni { get; set; } //DateTime?
        public string FechaAuditoriaFin { get; set; } //DateTime?
        //
        public string FechaMinConsultaAudit { get; set; } //DateTime?
        public string FechaMaxConsultaAudit { get; set; } //DateTime?
        //
        public string FechaAsignacionIni { get; set; }
        public string FechaAsignacionFin { get; set; }
        //
        public string Activo { get; set; } //bool
        public string Conclusion { get; set; } //bool
        public string UrlSoportes { get; set; } //bool
        public string Reverse { get; set; } //bool
        public string DisplayOrder { get; set; } //int?
        public string Ara { get; set; } //bool
        public string Eps { get; set; } //bool
        //
        public string FechaReversoIni { get; set; } //DateTime?
        public string FechaReversoFin { get; set; } //DateTime?
        //
        public string AraAtendido { get; set; } //bool
        public string EpsAtendido { get; set; } //bool
        public string Revisar { get; set; } //bool
        public List<string> Estado { get; set; } //int?
        public string AccionLider { get; set; }
        public string AccionAuditor { get; set; }

        public string CodigoEps { get; set; }

        public string FechaApertura { get; set; }
        public string HoraApertura { get; set; }
        public string FechaCierre { get; set; }
        public string HoraCierre { get; set; }
        //Validamos datos de paginado.
        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (PageNumber < 0)
            {
                yield return new ValidationResult("El numero de pagina debe ser mayor o igual a 0");
            }

            if (MaxRows <= 0)
            {
                yield return new ValidationResult("El numero de registros maximos 'MaxRows' debe ser mayor a 0");
            }
        }


        //Version OLD, solo string.
        //[Key]
        //public string Id { get; set; }
        //public string IdRadicado { get; set; } //int?
        //public string IdMedicion { get; set; } //int?
        //public string IdPeriodo { get; set; } //int?
        //public string IdAuditor { get; set; }  //public string IdAuditor { get; set; } //Guid
        //public string PrimerNombre { get; set; }
        //public string SegundoNombre { get; set; }
        //public string PrimerApellido { get; set; }
        //public string SegundoApellido { get; set; }
        //public string Sexo { get; set; }
        //public string TipoIdentificacion { get; set; } //Varchar de 2 caracteres
        //public string Identificacion { get; set; } //int?
        ////
        //public string FechaNacimientoIni { get; set; } //DateTime?
        //public string FechaNacimientoFin { get; set; } //DateTime?
        ////
        //public string FechaCreacionIni { get; set; } //DateTime?
        //public string FechaCreacionFin { get; set; } //DateTime?
        ////
        //public string FechaAuditoriaIni { get; set; } //DateTime?
        //public string FechaAuditoriaFin { get; set; } //DateTime?
        ////
        //public string FechaMinConsultaAudit { get; set; } //DateTime?
        //public string FechaMaxConsultaAudit { get; set; } //DateTime?
        ////
        //public string FechaAsignacionIni { get; set; }
        //public string FechaAsignacionFin { get; set; }
        ////
        //public string Activo { get; set; } //bool
        //public string Conclusion { get; set; } //bool
        //public string UrlSoportes { get; set; } //bool
        //public string Reverse { get; set; } //bool
        //public string DisplayOrder { get; set; } //int?
        //public string Ara { get; set; } //bool
        //public string Eps { get; set; } //bool
        ////
        //public string FechaReversoIni { get; set; } //DateTime?
        //public string FechaReversoFin { get; set; } //DateTime?
        ////
        //public string AraAtendido { get; set; } //bool
        //public string EpsAtendido { get; set; } //bool
        //public string Revisar { get; set; } //bool
        //public string Estado { get; set; } //int?
    }
}
