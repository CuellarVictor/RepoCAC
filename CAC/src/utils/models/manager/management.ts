export class objCobertura{
    pageNumber: number = 0;
    maxRows: number = 10000;
    idCobertura: string = "";
    nombre: string = "";
    nemonico: string = "";
    legislacion: string = "";
    definicion: string = "";
    tiempoEstimado: string = "";
    excluirNovedades: string = "";
    novedades: string = "";
    idResolutionSiame: string = "";
    novedadesCompartidosUnicos: string = "";
    cantidadVariables: string = "";
    idCoberturaPadre: string = "";
    idCoberturaAdicionales: string = "";
}


export class objRegistroEstado{
    pageNumber: number = 0;
    maxRows: number = 100;
    idAuditor: string = "f8a7a753-2c97-4ce9-bf42-2b3936606719";
    estado: string = ""
    }

export class objPeriodo{
    pageNumber: number = 0;
    maxRows: number = 100;
    idPeriodo: string = "";
    nombre: string = "";
    fechaCorteIni: string = "";
    fechaCorteFin: string = "";
    fechaFinalReporteIni: string = "";
    fechaFinalReporteFin: string = "";
    fechaMaximaCorreccionesIni: string = "";
    fechaMaximaCorreccionesFin: string = "";
    fechaMaximaSoportesIni: string = "";
    fechaMaximaSoportesFin: string = "";
    fechaMaximaConciliacionesIni: string = "";
    fechaMaximaConciliacionesFin: string = "";
    idCobertura: string = "";
    idPeriodoAnt: string = "";
    fechaMinConsultaAuditIni: string = "";
    fechaMinConsultaAuditFin: string = "";
    fechaMaxConsultaAuditIni: string = "";
    fechaMaxConsultaAuditFin: string = "";
}

export class responsePeriodo{
    fechaCorte: string = "";
    fechaFinalReporte: string = "";
    fechaMaxConsultaAudit: string = "";
    fechaMaximaConciliaciones: string = "";
    fechaMaximaCorrecciones: string = "";
    fechaMaximaSoportes: string = "";
    fechaMinConsultaAudit: string = "";
    idCobertura: string = "";
    idPeriodo: string = "";
    idPeriodoAnt: string = "";
    nombre: string = "";
}

export class objFiltro{
     PageNumber: number = 0;
     maxRows: number = 1000;
    // IdUsuario: string = "";
    idLider: string = "";
    // Id: string = "";
    idCobertura: string = "";
    idEstado: any;
    // idPeriodo: string = "";
    // Descripcion: string = "";
    // Activo: string = "";
    // CratedBy: string = "";
    // CreatedDate: string = "";
    // ModufyBy: string ="";
    // ModifyDate: string ="";
}

export class listFiltros{
    fechaCorte: string = "";
    idCobertura: number = 0;
    idPeriodo: number = 0;
    neomonico: string = "";
    nombre: string = "";

}

export class objDuplicarMedicion{
    medicionId: number = 0;
    userCreatedBy: string = "";
}

export  class medicionxemadre{
    id!:number;
    nombre!: string;
    total!: string;
}