using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Dto.RegistroAuditoria
{
    public class RegistrosAuditoriaResponseModel
    {
        public RegistrosAuditoriaResponseModel()
        {

        }
        //public RegistrosAuditoriaResponseModel(ResponseRegistrosAuditoriaModelDto contextModel, string codigoEPS, string nombreEPS, string codigoIPS, string nombreIPS)
        public RegistrosAuditoriaResponseModel(ResponseRegistrosAuditoriaModelDto contextModel)
        {
            this.QueryNoRegistrosTotales = contextModel.QueryNoRegistrosTotales;
            this.Id = contextModel.Id;
            this.IdRadicado = contextModel.IdRadicado;
            this.IdMedicion = contextModel.IdMedicion;
            this.NombreMedicion = contextModel.NombreMedicion;
            this.FechaCorteAuditoria = contextModel.FechaCorteAuditoria;
            this.IdPeriodo = contextModel.IdPeriodo;
            //this.IdLider = contextModel.IdLider;
            this.IdAuditor = contextModel.IdAuditor;
            this.IdEntidad = contextModel.IdEntidad;
            this.Entidad = contextModel.Entidad;
            //this.CodigoEPS = codigoEPS;
            //this.NombreEPS = nombreEPS;
            this.CodigoEPS = contextModel.Data_IdEPS;
            this.NombreEPS = contextModel.Data_NombreEPS;
            this.CodigoIPS = contextModel.IPS; //"codigoIPS";
            this.NombreIPS = contextModel.IPS; //"nombreIPS";
            this.PrimerNombre = contextModel.PrimerNombre;
            this.SegundoNombre = contextModel.SegundoNombre;
            this.PrimerApellido = contextModel.PrimerApellido;
            this.SegundoApellido = contextModel.SegundoApellido;
            this.Sexo = contextModel.Sexo;
            this.TipoIdentificacion = contextModel.TipoIdentificacion;
            this.Identificacion = contextModel.Identificacion;
            this.FechaNacimiento = contextModel.FechaNacimiento;
            this.FechaCreacion = contextModel.FechaCreacion;
            this.FechaAuditoria = contextModel.FechaAuditoria;
            this.FechaMinConsultaAudit = contextModel.FechaMinConsultaAudit;
            this.FechaMaxConsultaAudit = contextModel.FechaMaxConsultaAudit;
            this.FechaAsignacion = contextModel.FechaAsignacion;
            this.Activo = contextModel.Activo;
            this.Conclusion = contextModel.Conclusion;
            this.UrlSoportes = contextModel.UrlSoportes;
            this.Reverse = contextModel.Reverse;
            this.DisplayOrder = contextModel.DisplayOrder;
            this.Ara = contextModel.Ara;
            this.Eps = contextModel.Eps;
            this.FechaReverso = contextModel.FechaReverso;
            this.AraAtendido = contextModel.AraAtendido;
            this.EpsAtendido = contextModel.EpsAtendido;
            this.Revisar = contextModel.Revisar;
            this.Extemporaneo = contextModel.Extemporaneo;
            this.Estado = contextModel.Estado;
            this.LevantarGlosa = contextModel.LevantarGlosa;
            this.MantenerCalificacion = contextModel.MantenerCalificacion;
            this.ComiteExperto = contextModel.ComiteExperto;
            this.ComiteAdministrativo = contextModel.ComiteAdministrativo;
            this.AccionLider = contextModel.AccionLider;
            this.AccionAuditor = contextModel.AccionAuditor;
            this.EstadoCodigo = contextModel.EstadoCodigo;
            this.EstadoNombre = contextModel.EstadoNombre;
            this.Encuesta = contextModel.Encuesta;
            this.CreatedBy = contextModel.CreatedBy;
            this.CreatedDate = contextModel.CreatedDate;
            this.ModifyBy = contextModel.ModifyBy;
            this.ModifyDate = contextModel.ModifyDate;
            this.ImagePath = contextModel.ImagePath;
            this.NombreAuditor = contextModel.NombreAuditor;
            this.CodigoUsuario = contextModel.CodigoUsuario;
            this.IdCobertura = contextModel.IdCobertura;
            this.EnfermedadMadre = contextModel.EnfermedadMadre;
            //this.Data_IdEPS = contextModel.Data_IdEPS;
            //this.Data_NombreEPS = contextModel.Data_NombreEPS;
            this.IPS = contextModel.IPS;
            this.FechaApertura = contextModel.FechaApertura;
            this.HoraApertura = contextModel.HoraApertura;
            this.FechaCierre = contextModel.FechaCierre;
            this.HoraCierre = contextModel.HoraCierre;
        }
        public string QueryNoRegistrosTotales { get; set; }
        public int Id { get; set; }
        public int? IdRadicado { get; set; }
        public int? IdMedicion { get; set; }
        public string NombreMedicion { get; set; }
        public string FechaCorteAuditoria { get; set; }
        public int? IdPeriodo { get; set; }
        public string IdAuditor { get; set; }
        public string IdLider { get; set; }  //public int? IdAuditor { get; set; } //Guid
        public int? IdEntidad { get; set; }
        public string Entidad { get; set; }
        public string CodigoEPS { get; set; }
        public string NombreEPS { get; set; }
        public string CodigoIPS { get; set; }
        public string NombreIPS { get; set; }
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public string Sexo { get; set; }
        public string TipoIdentificacion { get; set; } //Varchar de 2 caracteres
        public string Identificacion { get; set; }
        public DateTime? FechaNacimiento { get; set; }
        public DateTime? FechaCreacion { get; set; }
        public DateTime? FechaAuditoria { get; set; }

        public DateTime? FechaMinConsultaAudit { get; set; }
        public DateTime? FechaMaxConsultaAudit { get; set; }

        public DateTime? FechaAsignacion { get; set; }

        public bool Activo { get; set; }
        public string Conclusion { get; set; }
        public string UrlSoportes { get; set; }
        public bool Reverse { get; set; }
        public int? DisplayOrder { get; set; }
        public bool Ara { get; set; }
        public bool Eps { get; set; }
        public DateTime? FechaReverso { get; set; }
        public bool AraAtendido { get; set; }
        public bool EpsAtendido { get; set; }
        public bool Revisar { get; set; }
        public bool Extemporaneo { get; set; }
        public int? Estado { get; set; }
        public int? LevantarGlosa { get; set; }
        public int? MantenerCalificacion { get; set; }
        public int? ComiteExperto { get; set; }
        public int? ComiteAdministrativo { get; set; }
        public int? AccionLider { get; set; }
        public int? AccionAuditor { get; set; }
        public string EstadoCodigo { get; set; }
        public string EstadoNombre { get; set; }
        public bool Encuesta { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? CreatedDate { get; set; }
        public string ModifyBy { get; set; }
        public DateTime? ModifyDate { get; set; }

        public string ImagePath { get; set; }
        public int IdCobertura { get; set; }
        public string NombreAuditor { get; set; }
        public string CodigoUsuario { get; set; }
        public string EnfermedadMadre { get; set; }
        public string IPS { get; set; }
        public string FechaApertura { get; set; }
        public string HoraApertura { get; set; }
        public string FechaCierre { get; set; }
        public string HoraCierre { get; set; }
        //public string Data_IdEPS { get; set; }
        //public string Data_NombreEPS { get; set; }
    }
}
