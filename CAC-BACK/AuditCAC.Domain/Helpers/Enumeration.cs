using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Helpers
{
    public class Enumeration
    {

        public enum Procesos
        {
            CarguePoblacion = 1,
            CalificacionMasiva = 2,
        }

        public enum Parametros
        {
            URLAuditCACCoreCACServices = 1,
            UsuarioTokenAuditCACCoreCACServices = 2,
            PasswordTokenAuditCACCoreCACServices = 3,
            InactivityTime = 7,
            ExpirationTime = 8,
            SessionDead = 9,
            Autenticacionpordirectorioactivo =	10,
            CampoReferencialIps	=	12,
            CampoReferencialEps	=	13,
            SeparadorBase64 = 15,
            CantidadColumnasCarguePoblacion = 20,
            SeparadorArchivos = 21,
            SimulatorErrors = 22,
            HorasActualizarMongo = 23,
            ValidarErroesTipoA = 24,
            ValidarErroesTipoB = 25,
            IntentosFallidosLogin = 26,
            CantidadDiasExpiracionContrasena = 27,
            TimeOutCarguePoblacion = 28,
            EmailOrigen	=	29,
            EmailOrigenPassword	= 30,
            EmailCopia	=	31,
            AsuntoCorreoRecuperacion =	32,
            EmailPuerto	=	33,
            EmailHost	=	34,
            EmailRecuperacionBody = 35,
            UrlRecuperacionPassword = 36,
            IdsHEPATITIS = 37,
            ValidarErroresTipoSentencia = 38,
            LicenciaKeyPDF = 39
        }

        public enum EstadoRegistroAuditoria
        {
            Registronuevo = 1,
            Glosaenrevisionporlaentidad = 2,
            Glosaobjetada1	= 3,
            Glosaobjetadaenrevisionporlaentidad = 4,
            Glosaobjetada2	=	5,
            Errorlogicamarcaciónauditor = 6,
            Errorlogicamarcacionlíder = 7,
            Enviadoaentidad = 8,
            Comiteadministrativo = 9,
            Comiteexpertos = 10,
            Respuestaadministrativa = 11,
            Hallazgo1 =	12,
            Hallazgo1enviadoaentidad = 13,
            Hallazgo2lider = 14,
            Hallazgo2auditor = 15,
            Registrocerrado = 16,
            Registropendiente = 17,
        }

        public enum ReporteMedicion
        {
            ReasignacionBolsaPlantilla = 1,
            ReasignacionBolsaData = 2,
            ReasignacionTotalData = 3,
        }

        public enum RespuestaLogin
        {
            Exitoso = 1,
            UsuarioNoExiste = 2,
            ContrasenaIncorrecta = 3,
            UsuarioBloqueadoPorIntentosFallidos = 4,
            UsuarioInactivo = 5,
            ActualizarContrasena = 6,
            SesionActiva = 7
        }

        public enum Dato_DC_NC_ND
        {
            DC = 32,
            NC = 33,
            ND = 34
        }

        public enum SubGrupoVariable
        {
            Estructura2 = 106
        }

        public static class Actas
        {
            public enum Template : int
            {
                ActaApertura = 137,
                ActaCierre = 138
            }

            public enum TipoInput : int
            {
                Parametrizacion = 140,
                Usuario = 141,
                Consultado = 142
            }

            public enum TipoDato : int
            {
                String = 143,
                List = 144
            }

            public const string Listado = "$Listado$";

            public static class AperturaParamsConstantes
            {
                public const string TablaGrupoAuditor = "$Auditores$";
                public const string TablaGrupoAuditorNombres = "$GrupoAuditor$";
                public const string TablaGrupoAuditorFunciones = "$FuncionesAuditor$";

                public const string TablaMedicion = "$Medicion$";
                public const string TablaMedicionNombres = "$ListadoMediciones$";
                public const string TablaMedicionCasos = "$ListadoCasos$";

                public const string TablaAsistentesEntidad = "$AsistentesEntidad$";
                public const string TablaAsistentesEntidadNombres = "$AsistentesEntidadNombres$";
                public const string TablaAsistentesEntidadCargos = "$AsistentesEntidadCargos$";

                public const string TablaAsistentesCAC = "$AsistentesCAC$";
                public const string TablaAsistentesCACNombres = "$AsistentesCACNombres$";
                public const string TablaAsistentesCACCargos = "$AsistentesCACCargos$";
            }

            public static class CierreParamsConstantes
            {
                public const string TablaGrupoAuditor = "$Auditores$";
                public const string TablaGrupoAuditorNombres = "$GrupoAuditor$";
                public const string TablaGrupoAuditorFunciones = "$FuncionesAuditor$";

                public const string TablaDetallesEntidad = "$DetallesEntidad$";
                public const string TablaDetallesEntidadItem = "$DetallesEntidadItem$";
                public const string TablaDetallesEntidadSi = "$DetallesEntidadOpcionSi$";
                public const string TablaDetallesEntidadNo = "$DetallesEntidadOpcionNo$";
                public const string TablaDetallesEntidadObservacion = "$DetallesEntidadObservacion$";

                public const string TablaMedicionCalidad = "$MedicionCalidad$";
                public const string TablaMedicionCalidadItem = "$MedicionCalidadItem$";
                public const string TablaMedicionCalidadConformes = "$MedicionCalidadConforme$";
                public const string TablaMedicionCalidadNoConformes = "$MedicionCalidadNoConforme$";
                public const string TablaMedicionCalidadNoDisponible = "$MedicionCalidadNoDisponible$";
                public const string TablaMedicionCalidadCasosAuditados = "$MedicionCalidadCasosAuditados$";

                public const string TablaMedInconsistencias = "$MedicionInconsistencias$";
                public const string TablaMedInconsistenciasCasosAuditados = "$MedicionInconsistenciasCasosAuditados$";
                public const string TablaMedInconsistenciasIxDiagnostico = "$MedicionInconsistenciasPorDiagnostico$";
                public const string TablaMedInconsistenciasIxSoportes = "$MedicionInconsistenciasPorSoportes$";
                public const string TablaMedInconsistenciasGlosax100 = "$MedicionInconsistenciasGlosa$";

                public const string TablaAsistentesEntidad = "$AsistentesEntidad$";
                public const string TablaAsistentesEntidadNombres = "$AsistentesEntidadNombres$";
                public const string TablaAsistentesEntidadCargos = "$AsistentesEntidadCargos$";

                public const string TablaAsistentesCAC = "$AsistentesCAC$";
                public const string TablaAsistentesCACNombres = "$AsistentesCACNombres$";
                public const string TablaAsistentesCACCargos = "$AsistentesCACCargos$";
            }
        }
    }
}
