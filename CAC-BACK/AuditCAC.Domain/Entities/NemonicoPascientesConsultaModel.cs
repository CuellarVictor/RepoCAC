using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AuditCAC.Domain.Entities
{
    public class NemonicoPascientesConsultaModel
    {
        [Key]
        public int id { get; set; }
        public string idEPS { get; set; }
        public bool estado { get; set; }
        public int idRegistro { get; set; }
        public int idPeriodo { get; set; }
        public string PrimerNombre { get; set; }
        public string SegundoNombre { get; set; }
        public string PrimerApellido { get; set; }
        public string SegundoApellido { get; set; }
        public string TipoIdentificacion { get; set; }
        public int Identificacion { get; set; }
        public DateTime FechaNacimiento { get; set; }
        public string Sexo { get; set; }
        public string Ocupacion { get; set; }
        public string Regimen { get; set; }
        public int idPertenenciaEtnica { get; set; }
        public int idGrupoPoblacional { get; set; }
        public string MunicipioDeResidencia { get; set; }
        public string TelefonoPaciente { get; set; }
        public DateTime FechaAfiliacion { get; set; }
        public int GestacionAlCorte { get; set; }
        public int EnPlanificacion { get; set; }
        public int EdadUsuarioMomentoDx { get; set; }
        public int MotivoPruebaDx { get; set; }
        public DateTime FechaDx { get; set; }
        public string IpsRealizaConfirmacionDx { get; set; }
        public int TipoDeficienciaDiagnosticada { get; set; }
        public int SeveridadSegunNivelFactor { get; set; }
        public decimal ActividadCoagulanteDelFactor { get; set; }
        public int AntecedentesFamilares { get; set; }
        public int FactorRecibidoTtoIni { get; set; }
        public int EsquemaTtoIni { get; set; }
        public DateTime FechaDeIniPrimerTto { get; set; }
        public int FactorRecibidoTtoAct { get; set; }
        public int EsquemaTtoAct { get; set; }
        public decimal Peso { get; set; }
        public decimal Dosis { get; set; }
        public int FrecuenciaPorSemana { get; set; }
        public int UnidadesTotalesEnElPeriodo { get; set; }
        public int AplicacionesDelFactorEnElPeriodo { get; set; }
        public int ModalidadAplicacionTratamiento { get; set; }
        public int ViaDeAdministracion { get; set; }
        public string CodigoCumFactorPosRecibido { get; set; }
        public string CodigoCumFactorNoPosRecibido { get; set; }
        public string CodigoCumDeOtrosTratamientosUtilizadosI { get; set; }
        public string CodigoCumDeOtrosTratamientosUtilizadosII { get; set; }
        public string IpsSeguimientoActual { get; set; }
        public int Hemartrosis { get; set; }
        public int CantHemartrosisEspontaneasUlt12Meses { get; set; }
        public int CantHemartrosisTraumaticasUlt12Meses { get; set; }
        public int HemorragiaIlioPsoas { get; set; }
        public int HemorragiaDeOtrosMusculosTejidos { get; set; }
        public int HemorragiaIntracraneal { get; set; }
        public int HemorragiaEnCuelloOGarganta { get; set; }
        public int HemorragiaOral { get; set; }
        public int OtrasHemorragias { get; set; }
        public int CantOtrasHemorragiasEspontaneasDiffHemartrosis { get; set; }
        public int CantOtrasHemorragiasTraumaticasDiffHemartrosis { get; set; }
        public int CantOtrasHemorragAsocProcedimientoDiffHemartrosis { get; set; }
        public int PresenciaDeInhibidor { get; set; }
        public DateTime FechaDeterminacionTitulosInhibidor { get; set; }
        public int HaRecibidoITI { get; set; }
        public int EstaRecibiendoITI { get; set; }
        public int DiasEnITI { get; set; }
        public int ArtropatiaHemofilicaCronica { get; set; }
        public int CantArticulacionesComprometidas { get; set; }
        public int UsuarioInfectadoPorVhc { get; set; }
        public int UsuarioInfectadoPorVhb { get; set; }
        public int UsuarioInfectadoPorVih { get; set; }
        public int Pseudotumores { get; set; }
        public int Fracturas { get; set; }
        public int Anafilaxis { get; set; }
        public string FactorAtribuyeReaccionAnafilactica { get; set; }
        public int ReemplazosArticulares { get; set; }
        public int ReemplazosArticularesEnPeriodoDeCorte { get; set; }
        public int LiderAtencion { get; set; }
        public int ConsultasConHematologo { get; set; }
        public int ConsultasConOrtopedista { get; set; }
        public int IntervencionProfesionalEnfermeria { get; set; }
        public int ConsultasOdontologo { get; set; }
        public int ConsultasNutricionista { get; set; }
        public int IntervencionTrabajoSocial { get; set; }
        public int ConsultasConFisiatria { get; set; }
        public int ConsultasConPsicologia { get; set; }
        public int IntervencionQuimicoFarmaceutico { get; set; }
        public int IntervencionFisioterapia { get; set; }
        public string PrimerNombreMedicoTratantePrincipal { get; set; }
        public string SegundoNombreMedicoTratantePrincipal { get; set; }
        public string PrimerApellidoMedicoTratantePrincipal { get; set; }
        public string SegundoApellidoMedicoTratantePrincipal { get; set; }
        public int CantAtencionesUrgencias { get; set; }
        public int CantEventosHospitalarios { get; set; }
        public decimal CostoFactoresPos { get; set; }
        public decimal CostoFactoresNoPos { get; set; }
        public decimal CostoTotalManejo { get; set; }
        public decimal CostoIncapacidadesLaborales { get; set; }
        public int? Novedades { get; set; }
        public int? CausaMuerte { get; set; }
        public DateTime? FechaMuerte { get; set; }
        public DateTime? FechaCreacionRegistro { get; set; }
        public int? SerialBDUA { get; set; }
        public int? CantidadReemplazosArticulares { get; set; }
        public DateTime? V66FechaCorte { get; set; }
    }
}
