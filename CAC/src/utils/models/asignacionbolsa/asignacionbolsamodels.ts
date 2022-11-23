export class listaAuditores{         
    codigo!: string;
    id!: string;
    userName!: string;
    nombres!: string;
    apellidos!: string;
  }

  export class objpostUsuariosMedicion{
      pageNumber!: number;
      maxRows!: number;
      medicionId!: number;
      auditorId !: string[];
      keyWord!: string;  
  }

  export class datatableAsignacionBolsa{
    auditorCodigo!: string;    
    auditorEstado!: string;  
    auditorNombre!: string;  
    queryNoRegistrosTotales!: string;  
    registrosAsignados!: number;
    registrosAuditados!: number;
    auditorNombres!: string;
    auditorApellidos!: string;
    

  }

  export class auditor{
    auditorApellidos!: string;    
    auditorCodigo!: string;    
    auditorEstado!: string;    
    auditorNombres!: string;    
    auditorUsuario!: string;    
    queryNoRegistrosTotales!: string;    
    registrosAsignados!: number;
    registrosAuditados!: number;
  }

  export class objpostReasignarBolsa{
    pageNumber!: number;
    maxRows!: number;
    medicionId!: number[];
    fechaAsignacionIni!: string;  
    fechaAsignacionFin!: string;  
    estado!: string[];
    codigoEps!: string[];
    auditorId !: string[];
    keyWord!: string;  
    idRadicado!: string[];
    finalizados!: boolean;
}

export class dataTableAuditorBolsas{
  codigoUsuario!: string; 
  data_IdEPS!: string; 
  data_NombreEPS!: string; 
  estado!: number;
  estadoCodigo!: string; 
  estadoNombre!: string; 
  fechaAsignacion!: string; 
  idAuditor!: string; 
  idMedicion!: number;
  idRadicado!: number;
  ips!: string; 
  nombreAuditor!: string; 
  nombreMedicion!: string; 
  queryNoRegistrosTotales!: string; 
  idEnfermedadMadre!: number;
  nombreEnfermedadMadre!: string; 
}

export class objMedicionAsignacion{
  activo!: boolean;
createdBy!: string; 
createdDate!: string; 
descripcion!: string; 
estado!: number;
fechaCorteAuditoria!: string; 
fechaFinAuditoria!: string; 
fechaInicioAuditoria!: string; 
id!: number;
idCobertura!: number;
idPeriodo!: number;
lider!: string; 
modifyBy!: string; 
modifyDate!: string; 
nombre!: string; 
resolucion!: string; 
status!: boolean;
}
