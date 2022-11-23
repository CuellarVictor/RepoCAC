export class ResponseConsultaLog {

    constructor() {
    }

    countQuery:number = 0;
    paginas:number = 0;
    idRadicado:number = 0;
    registroAuditoriaId:number = 0;
    proceso:number = 0;
    observacion: string = '';
    estadoAnterioId:number = 0;
    estadoActual:number = 0;
    codigo: string = '';
    nombres: string = '';
    apellidos: string = '';
    nombreUsuario: string = '';
    createdDate:Date = new Date();
    modifyBy: string = '';
    modificationDate:Date = new Date();
    status: boolean  = false;
}
