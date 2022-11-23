import { VariableCondicional } from "./variablecondicionada.model";

export class VariableResponseModel {

    constructor() {
    }
   
    queryNoRegistrosTotales : number = 0;
    id : number = 0;
    variable : number = 0;
    calculadora : boolean = false;    
    tipoCalculadora : number = 0;
    activa : boolean = false;
    orden : number = 0;
    idVariable : number = 0;
    idCobertura : number = 0;
    nombre : string = "";
    nemonico : string = "";
    descripcion : string = "";
    idTipoVariable : string = "";
    longitud : number = 0;
    decimales : number = 0;
    formato :  string = "";
    tablaReferencial : string = "";
    campoReferencial : string = "";
    createdBy : string = "";
    createdDate : Date = new Date();
    modifyBy :string = "";
    modifyDate : Date = new Date();
    motivoVariable : string = "";
    bot : boolean = false;
    tipoVariableItem : number = 0;    
    estructuraVariable : number = 0;
    variableId : number = 0;
    esGlosa : number = 0;
    medicionId : number[] = [];
    esVisible : boolean = true;
    esCalificable : boolean = true;
    activo : boolean = false;
    hallazgos : boolean = false;
    enableDC : boolean = false;
    enableNC : boolean = false;
    enableND : boolean = false;
    calificacionXDefecto : number = 0;
    subGrupoId : number = 0;
    subGrupoNombre : string = "";
    encuesta : boolean = false;
    vxM_Orden : number = 0;
    alerta : boolean = false;
    alertaDescripcion : string = "";
    calificacionIPSItem : number[] = [];
    idRegla: number = 0;
    nombreRegla :  string = "";
    concepto : string = "";
    tipoVariableDesc : string = "";
    tipoCampo :number = 0;
    promedio : boolean = false;
    validarEntreRangos : boolean = false;
    desde : string = "";
    hasta : string = "";
    condicionada : boolean = false;
    valorConstante : string = "";
    lista : boolean = false;
    variableCondicional: VariableCondicional[] = [];
 }