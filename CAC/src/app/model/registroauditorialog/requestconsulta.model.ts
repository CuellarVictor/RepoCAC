export class RequestConsultaLog {

    constructor() {
    }

    pageNumber:number = 1;
    MaxRows:number = 3;
    ParametroBusqueda: string = '';
    FechaInicial: string = '';
    FechaFinal: string = '';
    IdAuditores:string[] = [];
    medicionId:number = 0;
}
