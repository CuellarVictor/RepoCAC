import { Route } from "@angular/compiler/src/core";
import { isNull } from "@angular/compiler/src/output/output_ast";
import { Component, Input, OnInit } from "@angular/core";
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from "@angular/forms";
import { MatDialog } from "@angular/material/dialog";
import { ActivatedRoute, Router } from "@angular/router";
import { CookieService } from "ngx-cookie-service";
import { LoadingComponent } from "src/app/layout/loading/loading.component";
import { EstadosBolsa } from "src/app/model/Util/enumerations.enum";
import { GetPermisosServices } from "src/app/services/get-permisos.services";
import { objuser } from "src/utils/models/cronograma/cronograma";
import { InputCreateVarible } from "src/utils/models/variable-details/Variables";
import Swal from "sweetalert2";
import { ConfigService } from "../../config/service/config.service";
import { ModalVariableComponent } from "../modal-variable/modal-variable.component";
import { VariableDetailsService } from "../variable-details/services/variable-details.service";

export interface data {
  id: number;
  value: string;
}
@Component({
  selector: "app-variable",
  templateUrl: "./variable.component.html",
  styleUrls: ["./variable.component.scss"],
})
export class VariableComponent implements OnInit {
  condicionVariable: any;
  reglasVariable: any[] = [];
  name: string = "";
  checked: any[] = [];
  content: string = "";
  type: string = "";
  catalogo: any;
  defaultValue: any;
  auditable: boolean = false;
  modeloVariable: any;
  grupoValue: any;
  tipoVariable: any[] = [];
  calificaciones: any;
  tipo_regla: any;
  modelo: any;
  btn: any;
  activateDes: boolean = false;
  activateItemsCal: boolean = false;
  modelPost = new InputCreateVarible();
  idCobertura: any = "";
  objUser: objuser;
  disabledDefault: boolean = false;
  item: any[] = [];
  
  back: string = "";
  enfMadre: any;
  disableAuditable: boolean = false;
  disabledInh: boolean = false;
  checkedInh: boolean = false;
  condicion: number = 0;

  //variableForm!: FormGroup;
  //formulario!: FormGroup;
  disabledResolucion: boolean = false;
  disabledVisible: boolean = false;
  nombre: string = "";
  urlParams: any;
  selectedEstructure: any;
  detalleEncoded: any;
  detalleUncoded: any;

  userEncoded: any;

  estado:any;
  desabilitarInput=false;
  nombreModuloAnterior: any = "";

  variableSelected: any = "";

  constructor(
    private coockie: CookieService,
    private fb: FormBuilder,
    private serviceVariableDetails: VariableDetailsService,
    public dialog: MatDialog,
    private router: Router,
    private route: ActivatedRoute,
    private itemService: ConfigService,
    public permisos: GetPermisosServices
  ) {

    this.userEncoded = sessionStorage.getItem("objUser");
    this.objUser = JSON.parse(this.userEncoded);

    this.nombreModuloAnterior = JSON.parse(
      sessionStorage.getItem("returnModel")!
    );
  }

  ngOnInit(): void {
    this.openDialogLoading(true);
    setTimeout(() => {
      this.getDefaultValue();
      this.getCatalogo();
      this.getGrupo();
      this.getItemsVariableRegla();
      this.getTipoVariable();
      this.getCalificaciones();
      this.getCondicionVariable();
      this.detalleEncoded = sessionStorage.getItem("detalle");
      this.detalleUncoded = JSON.parse(this.detalleEncoded);
      this.enfMadre = sessionStorage.getItem("enfMadre");
      this.modeloVariable = sessionStorage.getItem("modelo"); 
      this.estado = sessionStorage.getItem("estado");
      sessionStorage.removeItem("estado");
      this.validarEstado();    
      this.idCobertura = this.detalleUncoded.idMedicion;

      this.variableSelected = JSON.parse(this.modeloVariable);  

      if (this.modeloVariable) {
        setTimeout(() => {
          this.setModel();
          this.btn = "update";
        }, 1000);
      }
      this.variableForm = this.fb.group({
        idCobertura: [{value:this.idCobertura,disabled:this.desabilitarInput}],
        tipoVariableItem: [Validators.required],
        orden: [{value:'',disabled:this.desabilitarInput},[Validators.required]],
        enableDC: false,
        enableNC: false,
        enableND: false,
        nombre: [{value:'',disabled:this.desabilitarInput},[Validators.required]],
        esCalificable: false,
        esVisible: false,
        calificacionXDefecto: 0,
        subGrupoId: [{value:'',disabled:this.desabilitarInput}],

        descripcion: [{value:'',disabled:this.desabilitarInput}],
        alerta: false,
        encuesta: false,
        calificacionIPSItem: [],
        alertaDescripcion: [{value:'',disabled:this.desabilitarInput}],
        idTipoVariable: [{value:'',disabled:this.desabilitarInput}],
        longitud: [{value:'',disabled:this.desabilitarInput}],
        decimales:  [{value:'',disabled:this.desabilitarInput}],
        formato:  [{value:'',disabled:this.desabilitarInput}],
      });
      this.formulario = this.fb.group({
        tipo_variable: [""],
        regla: 0,
        caracteres: ["" ? "" : ""],
        valorDesde: ["" ? "" : ""],
        valorHasta: ["" ? "" : ""],
        valorPermitido: ["" ? "" : ""],
        dominio: ["" ? "" : ""],
      });
      this.openDialogLoading(false);
    }, 800);
  }

  variableForm = this.fb.group({
    idCobertura: [this.idCobertura],
    tipoVariableItem: new FormControl(),
    orden: [""],
    enableDC: false,
    enableNC: false,
    enableND: false,
    nombre: [""],
    esCalificable: false,
    esVisible: false,
    calificacionXDefecto: 0,
    subGrupoId: "",

    descripcion: "",
    alerta: false,
    encuesta: false,
    calificacionIPSItem: [''],
    alertaDescripcion: "",
    idTipoVariable: "",
    longitud: "",
    decimales: "",
    formato: "",
  });
  formulario = this.fb.group({
    tipo_variable: [""],
    regla: 0,
    caracteres: ["" ? "" : ""],
    valorDesde: ["" ? "" : ""],
    valorHasta: ["" ? "" : ""],
    valorPermitido: ["" ? "" : ""],
    dominio: ["" ? "" : ""],
  });

  goPlaces(number: number) {
    if (number == 1) {
      this.router.navigateByUrl("/gestion-de-auditoria");
    } else {
      var encoded = btoa(JSON.stringify(this.detalleUncoded));
      this.router.navigateByUrl(
        "/gestion-de-auditoria/detalle-variable/" + encoded
      );
    }
  }

  setConcepto(element: any) {
    let res: any;
    if (element.regla == 58) {
      res = {
        valorDesde: element.valorDesde,
        valorHasta: element.valorHasta,
        valorPermitido: element.valorPermitido,
      };
    } else if (element.regla == 57) {
      res = {
        caracteres: element.caracteres,
      };
    } else if (element.regla == 61) {
      res = {
        caracteres: element.dominio,
      };
    }
    return res;
  }

  getCondicionVariable() {
    this.serviceVariableDetails.getCondicion().subscribe((Response) => {
      this.condicionVariable = Response;
    });
  }

  openDialogLoading(loading: boolean): void {
    if (loading) {
      this.dialog.open(LoadingComponent, {
        //width: '300px',
        disableClose: true,
        data: {},
      });
    } else {
      this.dialog.closeAll();
    }
  }

  setModel() {
      
    if (this.variableSelected.concepto != null && this.variableSelected.concepto != "") {
      var reglas = JSON.parse(JSON.stringify(this.variableSelected.concepto));
    }
    this.nombre = this.variableSelected.nombre;
    const Items: any[] = []; 
    if (this.variableSelected.calificacionIPSItem.length != 0) { 
      this.variableSelected.calificacionIPSItem.forEach((e: any) => { 
        if (e.id != "") { 
          Items.push(parseInt(e.id)); 
        } 
      }); 
    }  
    const setModel = {
      orden: parseInt(this.variableSelected.vxM_Orden),
      idCobertura: parseInt(this.variableSelected.idCobertura),
      nombre: this.variableSelected.nombre,
      descripcion: this.variableSelected.descripcion,
      tipoVariableItem: parseInt(this.variableSelected.tipoVariableItem),

      esVisible: this.getBoolean(this.variableSelected.esVisible),
      esCalificable: this.getBoolean(this.variableSelected.esCalificable),
      enableDC: this.getBoolean(this.variableSelected.enableDC),
      enableNC: this.getBoolean(this.variableSelected.enableNC),
      enableND: this.getBoolean(this.variableSelected.enableND),
      calificacionXDefecto: parseInt(this.variableSelected.calificacionXDefecto),
      subGrupoId: this.variableSelected.subGrupoId,
      encuesta: this.getBoolean(this.variableSelected.encuesta),
      alerta: this.getBoolean(this.variableSelected.alerta),
      calificacionIPSItem: Items,
      alertaDescripcion: this.variableSelected.alertaDescripcion,
      idTipoVariable: this.variableSelected.idTipoVariable,
      longitud: this.variableSelected.longitud,
      decimales: this.variableSelected.decimales,
      formato: this.variableSelected.formato,
    };
    if (reglas) {
      if (reglas.caracteres) {
        let setModelRegla = {
          tipo_variable: parseInt(this.variableSelected.idRegla),
          regla: parseInt(this.variableSelected.idRegla),
          caracteres: reglas.caracteres,
          valorDesde: "",
          valorHasta: "",
          valorPermitido: "",
          dominio: "",
        };
        this.formulario.setValue(setModelRegla);
      } else if (reglas.valorDesde) {
        let setModelRegla = {
          tipo_variable: parseInt(this.variableSelected.idRegla),
          regla: parseInt(this.variableSelected.idRegla),
          caracteres: "",
          valorDesde: reglas.valorDesde,
          valorHasta: reglas.valorHasta,
          valorPermitido: reglas.valorPermitido,
          dominio: "",
        };
        this.formulario.setValue(setModelRegla);
      } else if (reglas.dominio) {
        let setModelRegla = {
          tipo_variable: parseInt(this.variableSelected.idRegla),
          regla: parseInt(this.variableSelected.idRegla),
          caracteres: "",
          valorDesde: "",
          valorHasta: "",
          valorPermitido: "",
          dominio: reglas.dominio,
        };
        this.formulario.setValue(setModelRegla);
      }
    }
    this.checkedInh = this.getBoolean(setModel.enableDC);
    this.activateItemsCal = this.getBoolean(setModel.enableDC);
    this.activateDes = this.getBoolean(this.variableSelected.alerta);
    this.changeReglaVariable(parseInt(this.variableSelected.idRegla));
    this.changeConditionVariable(this.variableSelected.idTipoVariable);
    this.variableForm.setValue(setModel);

    console.log(" --- model --- ");
    console.log(this.variableSelected);
    console.log(" ---- ");

    console.log(" --- orden --- ");
    console.log(this.variableSelected.orden);
    console.log(" ---- ");

    console.log(" --- VxM_Orden --- ");
    console.log(this.variableSelected.vxM_Orden);
    console.log(" ---- ");
  }

  getBoolean(value: any) {
    switch (value) {
      case true:
      case "true":
      case 1:
      case "1":
      case "on":
      case "yes":
        return true;
      default:
        return false;
    }
  }

  getItemsVariableRegla() {
    this.itemService.getItemById(10).subscribe(
      (Response) => {
        this.reglasVariable = Response;
      },
      (error) => {
        this.openDialogLoading(false);
        this.swalError();
      }
    );
  }

  swalError() {
    Swal.fire({
      title: "Error",
      text: "Lo sentimos, actualmente estamos presentando errores internos. Por favor intente nuevamente.",
      icon: "error",
    });
  }



  activateDescription(e: any) {
    if (e.checked == true) {
      this.activateDes = true;
    } else {
      this.activateDes = false;
    }
  }

  activateItemCal(e: any) {
    if (e.checked == true) {
      this.activateItemsCal = true;
    } else {
      this.activateItemsCal = false;
    }
  }

  update() {
    sessionStorage.setItem("boton", "ACTUALIZAR");
    if (this.variableForm.valid) {
      this.modelPost = this.variableForm.value;
      if (this.formulario.value.regla != 0) {
        var objRegla = {
          idRegla: this.formulario.value.regla,
          idVariable: 0,
          Concepto: JSON.stringify(this.setConcepto(this.formulario.value)),
        };
      } else {
        var objRegla = {
          idRegla: this.formulario.value.regla,
          idVariable: 0,
          Concepto: "",
        };
      }
      if(this.variableForm.value.calificacionIPSItem == "" 
          || this.variableForm.value.calificacionIPSItem == null
              || this.variableForm.value.calificacionIPSItem == 0){
        this.variableForm.value.calificacionIPSItem = [];
      }
      this.modelPost.variable = this.variableSelected.variableId;
      this.modelPost.reglaVariable = objRegla;
      this.modelPost.createdBy = this.objUser.userId;
      this.modelPost.modifyBy = "";
      this.modelPost.idCobertura = parseInt(this.idCobertura, 10);
      this.modelPost.tablaReferencial = "";
      this.modelPost.idUsuario= this.objUser.userId;
      const dialogRef = this.dialog.open(ModalVariableComponent, {
        data: this.modelPost,
      });
      dialogRef.afterClosed().subscribe((result) => {});
    }
  }

  changeReglaVariable(e: any) {
    this.tipo_regla = e;
  }

  getCatalogo() {
    this.serviceVariableDetails.getCatalogo().subscribe((Response) => {
      this.catalogo = Response;
    },
    (error) => {
      this.openDialogLoading(false);
      this.swalError();
    });
  }

  itemSelect(e: any) {
    if (e.value == 36) {
      this.disabledDefault = true;
      this.auditable = false;
      this.disableAuditable = true;
      this.disabledInh = true;
      this.checkedInh = true;
    } else {
      this.disabledDefault = false;
      this.auditable = false;
      this.disableAuditable = false;
      this.disabledInh = false;
      this.checkedInh = false;
    }
  }

  changeConditionVariable(e: any) {
    this.condicion = 1;
    switch (e.value) {
      case "bit":
        this.condicion = 0;
        break;
    }
  }

  getTipoVariable() {
    this.serviceVariableDetails.GetTipo().subscribe((Response) => {
      this.tipoVariable = Response;
      this.validateTipo();
    },
    (error) => {
      this.openDialogLoading(false);
      this.swalError();
    });
  }

  validateTipo() {
    var model = JSON.parse(this.modeloVariable);
    this.tipoVariable.forEach((element: any) => {
      if (model) {
        if (model.tipoVariableItem == 35) {
          this.item.push(element);
          this.disabledResolucion = true;
          this.disableAuditable = true;
          this.auditable = true;
          this.disabledVisible = true;
        } else if (element.id != 35) {
          this.item.push(element);
        }
      } else {
        if (element.itemName != "Resolución") {
          this.item.push(element);
        }
      }
      this.tipoVariable = this.item;
    });
  }



  getDefaultValue() {
    this.serviceVariableDetails.GetDefaultValue().subscribe((Response) => {
      this.defaultValue = Response;
    },
    (error) => {
      this.openDialogLoading(false);
      this.swalError();
    });
  }

  getCalificaciones() {
    this.serviceVariableDetails.GetCalificaciones().subscribe((Response) => {
      this.calificaciones = Response;
    },
    (error) => {
      this.openDialogLoading(false);
      this.swalError();
    });
  }

  getGrupo() {
    this.serviceVariableDetails.GetGrupo().subscribe((Response) => {
      this.grupoValue = Response;
    },
    (error) => {
      this.openDialogLoading(false);
      this.swalError();
    });
  }

  openDialog(): void {
    this.name = "Está seguro que desea eliminar el grupo de variables";
    this.content = "Esta acción no podrá ser revertida";
    this.type = "eliminar";
    const dialogRef = this.dialog.open(ModalVariableComponent, {
      width: "600px",
      data: { name: this.name, content: this.content, type: this.type },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        //accion al cerrar
      }
    });
  }

  openDialogSave() {
    sessionStorage.setItem("boton", "GUARDAR");
    if (this.variableForm.valid) {
      this.modelPost = this.variableForm.value;
      var objRegla = {
        idRegla: this.formulario.value.regla,
        idVariable: 0,
        Concepto: JSON.stringify(this.setConcepto(this.formulario.value)),
      };
      this.modelPost.reglaVariable = objRegla;
      this.modelPost.createdBy = this.objUser.userId;
      this.modelPost.modifyBy = "";
      this.modelPost.idCobertura = parseInt(this.idCobertura, 10);
      this.modelPost.variable = "";
      this.modelPost.tablaReferencial = "";
      this.modelPost.idUsuario=  this.objUser.userId;
      if(this.variableForm.value.calificacionIPSItem == "" 
          || this.variableForm.value.calificacionIPSItem == null
              || this.variableForm.value.calificacionIPSItem == 0){
        this.variableForm.value.calificacionIPSItem = [];
      }
      const dialogRef = this.dialog.open(ModalVariableComponent, {
        data: this.modelPost,
      });
      dialogRef.afterClosed().subscribe((result) => {});
    }
  }

  changeInh(e: any) {
    if (e.checked == true) {
      this.auditable = true;
    } else {
      this.auditable = false;
    }
  }

  changeCaligicable(e: any) {
    if (e.checked) {
      this.checked.push(e.checked);
    } else {
      this.checked.splice(0, 1);
    }
    if (this.checked.length == 3) {
      this.auditable = false;
      this.disableAuditable = true;
    }
  }
  validarEstado(){
    if(this.estado)
    this.estado!=EstadosBolsa.Creada?this.desabilitarInput=true:this.desabilitarInput=false;
  }
}
