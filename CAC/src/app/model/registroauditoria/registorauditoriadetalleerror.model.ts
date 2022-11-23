export class RegistroAuditoriaDetalleErrorModel {

    constructor() {
    }

    id: number = 0;
    registrosAuditoriaDetalleId: number = 0;
    idRegla: number = 0;
    descripcionRegla: string = "";
    idRestriccion: number = 0;
    reducido: string = "";
    variableId: number = 0;
    errorId: string = "";
    descripcion: string = "";
    sentencia: string = "";
    noCorregible: boolean = false;
    enable: boolean = false;
    createdBy: string = "";
    createdDate: any = null;
    modifyBy: string = "";
    modifyDate: any = null;
}