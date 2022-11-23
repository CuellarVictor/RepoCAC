export class RegistroAuditoriaDetalleModel {

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
    ipsId: number = 0;
    ipsNombre: string = "";
}
