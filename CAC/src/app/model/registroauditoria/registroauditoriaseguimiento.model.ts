export class RegistroAuditoriaSeguimientoModel {

    constructor() {
    }

    registroAuditoriaId: number = 0;
    TipoObservacion:  number = 0;
    Observacion: string = "";
    Soporte:  number = 0;
    EstadoActual:  number = 0;
    EstadoNuevo: number = 0;
    CreatedBy:  string = "";
    CreatedDate: Date = new Date();
    ModifyBy:  string = "";
    ModifyDate: Date = new Date();
    Status:  boolean = true;
}
