export class estado{
  nombre!: string;
  noRegistros!: string;
}

 export class tabla {
  id!: string;
  estado!: string;
  marcacion!: string;
  tooltip!: string;
  entidad!: string;
  medicion!: string;
  modificado!: string;
  btn!: string;
}

export class tabla2 {
  auditor!: string;
  registro!: string;
  entidades!: string;
  horas!: string;
  disponible!: string;
}

export class TodoItemNode {
  children!: TodoItemNode[];
  item!: string;
}

/** Flat to-do item node with expandable and level information */
export class TodoItemFlatNode {
  item!: string;
  level!: number;
  expandable!: boolean;
}

export class data{
id: string = "";
pageNumber: number = 0;
maxRows: number = 50000;
idRadicado: string = "";
idMedicion: string = "";
idPeriodo: string = "";
idLider: string = "";
idAuditor: string = "";
primerNombre: string = "";
segundoNombre: string = "";
primerApellido: string = "";
segundoApellido: string = "";
sexo: string = "";
tipoIdentificacion: string = "";
identificacion: string = "";
fechaNacimientoIni: string = "";
fechaNacimientoFin: string = "";
fechaCreacionIni: string = "";
fechaCreacionFin: string = "";
fechaAuditoriaIni: string = "";
fechaAuditoriaFin: string = "";
fechaMinConsultaAudit: string = "";
fechaMaxConsultaAudit: string = "";
fechaAsignacionIni: string = "";
fechaAsignacionFin: string = "";
fechaAsignacion: string = "";
activo: string = "";
conclusion: string = "";
urlSoportes: string = "";
reverse: string = "";
displayOrder: string = "";
ara: string = "";
eps: string = "";
fechaReversoIni: string = "";
fechaReversoFin: string = "";
araAtendido: string = "";
epsAtendido: string = "";
revisar: string = "";
estado: [] = [];
accionAuditor: string = "";
accionLider: string = "";
codigoEps: string = "";
}

export class objuser{
  rol!: objRol;
  userId!: string;
  userName!: string;
  name!: string;
  codigo !: string;
}

export class objRol{
    userRolName!: string;
    userRolId!: any;
}

export class filtro{
  nombreFiltro!: string;
  detalle!: detalleFiltro[];
}

export class detalleFiltro{
  id!: string;
  valor!: string;
}



export class objLogCronograma{
  pageNumber!: number;
  maxRows!: number;
  idAuditor!: string
}