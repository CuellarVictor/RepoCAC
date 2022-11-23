export class VariableModel {

    constructor() {
    }

    reducido: string = "";
    estado?: any;
    detalle: string = "";
    hallazgo?: any;
    error?: any;
    seleccion: string = "";
    dato_reportado: string = "";
    motivo: string = "";
    listaMotivos: any[] = [];
    registroAuditoriaDetalleId: number = 0;
    dato_DC_NC_ND: number = 0;
    visible: boolean = false;
    bot: boolean = false;
    variableId: number = 0;
    variableEncuesta: boolean = false;
    registrosAuditoriaEncuesta: boolean = false;
    nombre: string = "";
    idTipoVariable: number = 0;
    longitud: number = 0;
    decimales: number = 0;
}
