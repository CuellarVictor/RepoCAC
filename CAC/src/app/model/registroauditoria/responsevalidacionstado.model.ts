export class ResponseValidacionEstadoModel {

    constructor() {
    }

    id: string = "";
    habilitarVariablesCalificar: boolean = false;
    visibleBotonGuardar: boolean = false;
    habilitadoBotonGuardar: boolean = false;
    visibleBotonMantenerCalificacion: boolean = false;
    visibleBotonEditarCalificacion: boolean = false;
    visibleBotonComiteExperto: boolean = false;
    visibleBotonComiteAdministrativo: boolean = false;
    visibleBotonLevantarGlosa: boolean = false;
    validarErroresLogica: boolean = false;
    observacionHabilitada: boolean = false;
    observacionObligatoria: boolean = false;
    observacionRegistrada: boolean = false;
    calificacionObligatoriaIPS: boolean = false;
    calificacionIPSRegistrada: boolean = false;
    idItemGlosa: number = 0;
    idItemDC: number = 0;
    idItemNC: number = 0;
    tipificacionObservacionDefault: number = 0;
    tipificacionObservasionHabilitada: boolean = false;
    idRegistroNuevo: number = 0;
    codigoRegistroPendiente: string = "";
    habilitarBotonMantenerCalificacion: boolean = false;
    habilitarBotonEditarCalificacion: boolean = false;
    habilitarBotonComiteExperto: boolean = false;
    habilitarBotonComiteAdministrativo: boolean = false;
    habilitarBotonLevantarGlosa: boolean = false;
    solicitarMotivo: boolean = false;
    guardarCadaCambioVariable: boolean = false;
    habilitarGlosa: boolean = false;
    erroresReportados: boolean = false;
    habilitadoBotonReversar: boolean = false;
    visibleBotonReversar: boolean = false;
}
