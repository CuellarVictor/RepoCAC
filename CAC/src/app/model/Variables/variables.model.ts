import { ReglasVariabeModel } from "./reglasVariable.model";

export interface VariablesModel {
     Variable : string;
     Orden : number;
     idCobertura : number;
     nombre :string;
     descripcion : string;
     idTipoVariable : string;
     tablaReferencial : string;
     CreatedBy : string;
     ModifyBy : string;
     TipoVariableItem : number;
     EstructuraVariable : number;
     Alerta :boolean;
     AlertaDescripcion :string;
     MedicionId : [];
     EsVisible : boolean;
     EsCalificable : boolean;
     EnableDC : boolean;
     EnableNC : boolean;
     EnableND : boolean;
     CalificacionXDefecto : number;
     SubGrupoId : number;
     Encuesta: boolean;
     CalificacionIPSItem : [];
     ReglaVariable : ReglasVariabeModel;
  }
  