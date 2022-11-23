import { RegistroAuditoriaDetalleErrorModel } from "./registorauditoriadetalleerror.model";

export class RegistroAuditoriaDetalleErrorGroupModel {

    constructor() {
    }

    idRegla: number = 0;
    descripcionError: string = "";
    registrosAuditoriaDetalleId: number = 0;        
    variableId: number = 0;
    restricciones: RegistroAuditoriaDetalleErrorModel[] = [];
}