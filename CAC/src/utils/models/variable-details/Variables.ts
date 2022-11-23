//Modelo para filtrar datos de Listado Variables Perfil Lider.
export class InputsVariables {
  pageNumber: number = 0;
  maxRows: number = 100;
  id: string = "";
  variable: string = "";
  activa: string = "";
  orden: string = "";
  idVariable: string = "";
  idCobertura: string = "";
  nombre: string = "";
  nemonico: string = "";
  descripcion: string = "";
  idTipoVariable: string = "";
  longitud: string = "";
  decimales: string = "";
  formato: string = "";
  tablaReferencial: string = "";
  campoReferencial: string = "";
  createdBy: string = "";
  createdDate: string = "";
  modifyBy: string = "";
  modifyDate: string = "";
  motivoVariable: string = "";
  bot: string = "";
  esResolucion: string = "";
  medicionId: string = "";
  esGlosa: string = "";
  esVisible: string = "";
  esCalificable: string = "";
  activo: string = "";
  enableDC: string = "";
  enableNC: string = "";
  enableND: string = "";
  calificacionXDefecto: string = "";
  subGrupoId: [""] = [""];
  encuesta: string = "";
  vxM_Orden: string = "";
  tipoVariableItem: [""] = [""];
  estructuraVariable: string = "";
  alerta: string = "";
  mensaje: string = "";
  alertaDescripcion: string = "";
  concepto: string ="";
  calificacionIPSItem : string = "";
  idRegla: string = "";
  hallazgos: boolean = true;
}

export class InputCreateVarible {
  variable: string = "";
  orden: number = 0;
  idCobertura: number = 0;
  nombre: string = "";
  descripcion: string = "";
  createdBy: string = "";
  modifyBy: string = "";
  tipoVariableItem: number = 0;
  estructuraVariable: number = 0;
  medicionId: number = 0;
  esVisible: boolean = false;
  esCalificable: boolean = false;
  enableDC: boolean = false;
  enableNC: boolean = false;
  enableND: boolean = false;
  calificacionXDefecto: number = 0;
  subGrupoId: number = 0;
  encuesta: boolean = false;
  alerta: boolean = false;
  CalificacionIPSItem: [] = [];
  descripcionAlerta: string = "";
  reglaVariable: {} = [];
  tablaReferencial: string = "";
  idTipoVariable: string = "";
  idUsuario: string = "";
}
//Modelo para Tabla de vista de Variables Perfil Lider.
export interface Detalle {
  orden: string; // Order
  subGrupoNombre: string; // Grupo
  variable: string; // Variable
  nombre: string; // Nombre Variable
  descripcion: string; // Descripcion
  dato_DC_NC_NDRAD: string; // Default
  auditable: Boolean; // Auditable
  visible: Boolean; // Visible
}

export class updateVariables {
  variableId: number = 0;
  MedicionId: number = 0;
  subGrupoId: number = 0;
  default: string = "";
  auditable: string = "";
  visible: number = 0;
  idUsuario: string = "";
  hallazgos: string = "false";
}

export class responseDataTable {
  data: [] = [];
  noRegistrosTotales: number = 0;
  noRegistrosTotalesFiltrado: number = 0;
  totalPages: number = 0;
}
