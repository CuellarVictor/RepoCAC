import { SelectionModel } from "@angular/cdk/collections";
import {
  AfterViewInit,
  Component,
  HostListener,
  OnInit,
  ViewChild,
} from "@angular/core";
import { MatSort } from "@angular/material/sort";
import { MatTableDataSource } from "@angular/material/table";
import { ActivatedRoute, Router } from "@angular/router";
import { VariableDetailsService } from "./services/variable-details.service";
import {
  InputsVariables,
  responseDataTable,
  updateVariables,
} from "src/utils/models/variable-details/Variables";
import { PagerService } from "../../cronograma/services/page.service";
import { MatCheckboxChange } from "@angular/material/checkbox";
import { MatDialog } from "@angular/material/dialog";
import { LoadingComponent } from "src/app/layout/loading/loading.component";
import Swal from "sweetalert2";
import { EstadosBolsa } from "src/app/model/Util/enumerations.enum";
import { VariableResponseModel } from "src/app/model/Variables/variableresponse.model";
import { GetPermisosServices } from "src/app/services/get-permisos.services";

export interface Detalle {
  orden: string; // Order
  subGroupId: string; // Grupo
  variable: string; // Variable
  nombre: string; // Nombre Variable
  descripcion: string; // Descripcion
  default: string; // Default
  calificable: Boolean; // Auditable
  visible: Boolean;
  hallazgo: Boolean;
  //idVariable: number;        // Visible
  variableId: number;
  MedicionId: number;
  hallazgoId: number;
}

export interface DetalleLite {
  variableId: number; // variableId
  MedicionId: number;
  subGrupoId: string; // subGrupoId
  default: string; // Default
  auditable: string; // Auditable, Boolean
  visible: string; // visible, Boolean
  idUsuario : string;  
  hallazgos: string;
}

export interface data {
  id: number;
  value: string;
}
const selector: data[] = [
  { id: 1, value: "Variable 1" },
  { id: 2, value: "Variable 2" },
  { id: 3, value: "Variable 3" },
  { id: 4, value: "Variable 4" },
  { id: 4, value: "Variable 5" },
];
// ---

// ---
@Component({
  selector: "app-variable-details",
  templateUrl: "./variable-details.component.html",
  styleUrls: ["./variable-details.component.scss"],
})
export class VariableDetailsComponent implements OnInit {
  type_select: data[] = selector;
  displayedColumns: string[] = [
    "more",
    "orden",
    "grupo",
    "tipo",
    "variable",
    "nombre_variable",
    "descripcion",
    "default",
    "esCalificable",
    "esVisible",
    "hallazgo"
  ];
  selection = new SelectionModel<Detalle>(true, []);
  selection2 = new SelectionModel<Detalle>(true, []);
  selection3 = new SelectionModel<Detalle>(true, []);
  InputsVariables = new InputsVariables();
  DataVariables: Detalle[] = [];
  dataSource = new MatTableDataSource(this.DataVariables);
  SourceDataVariables = new MatTableDataSource(this.DataVariables);
  updateVariables: updateVariables = new updateVariables();
  responseDataTable: any;
  catalogo: any;
  defaultValue: any;
  grupoValue: any;
  catalogoSelect: any;
  tipoVariableSelect: any;
  textBuscar: any;
  auditable: boolean = false;
  visible: boolean = false;
  listaAcutualizar: Detalle[] = [];
  listaAcutualizarLite: DetalleLite[] = []; //Para enviar datos a servicio "ActualizarVariablesLiderMasivo"
  grupoSelect: any;
  itemsPerpagina: number = 200;
  sizeList: number[] = [50, 100, 150, 200];
  pageNumber: number = 1;
  intialPosition: any;
  pager: any = {};
  totalRegister: number = 0;
  id: any;
  medicion: any;
  seleccionado: any;
  rol: any = "";
  title: any = "";
  isDisabled: boolean = true;
  tipoVariable: any;
  estado: any = "";
  resolucion: any = "";
  urlParams: any;
  detalle:any;
  idMedicion:any;
  errorOf:any;
  objUser;
  constructor(
    private router: Router,
    private route: ActivatedRoute,
    public dialog: MatDialog,
    private paginatorService: PagerService,
    private serviceVariableDetails: VariableDetailsService,
    public permisos: GetPermisosServices
  ) {
    const userEncoded = sessionStorage.getItem("objUser");
    this.objUser = JSON.parse(userEncoded || '');
    this.updateVariables.idUsuario=this.objUser.userId;
  }

  ngOnInit(): void {
    sessionStorage.removeItem("modelo");
    this.seleccionado = 25;
    this.urlParams = this.route.snapshot.paramMap.get('objVariable');
    this.detalle = JSON.parse(atob(this.urlParams));  
    this.id = this.detalle.idEnfMadre;
    this.idMedicion = this.detalle.idMedicion;    
    this.medicion = this.detalle.medicion;
    this.estado = this.detalle.estadoAuditoria;
    this.resolucion = this.detalle.resolucion;
    let arr = {
      id: this.id,
      medicion: this.medicion,
      estado: this.estado,
      resolucion: this.resolucion,
    };
          
    sessionStorage.setItem("returnModel", JSON.stringify(arr));
    this.rol = sessionStorage.getItem("rol");
    this.title =
      (this.rol == "Admin" ? "Administrador" : "Líder") +
      ", aquí puede gestionar sus auditorías";
    sessionStorage.setItem("idMedicion", this.idMedicion);
    sessionStorage.setItem("idCobertura", this.id);

    this.prepareTable();
    this.setDataIntables();
    this.getCatalogo();
    this.getDefaultValue();
    this.getGrupo();
    this.getTipoVariable();
    this.getErrorOf();
  }

  //Preparamos la ventana para el rol
  prepareTable() {
    if (this.rol == "Lider") {
      this.displayedColumns = [
        "more",
        "orden",
        "grupo",
        "tipo",
        "variable",
        "nombre_variable",
        "descripcion",
        "errorOff",
        "default",
        "esCalificable",
        "esVisible",
        "hallazgo"
      ];
    } else {
      //Administrador
      this.displayedColumns = [
        "more",
        "orden",
        "grupo",
        "tipo",
        "variable",
        "nombre_variable",
        "descripcion",
        "errorOff",
        "default",
        "esCalificable",
        "esVisible",
        "hallazgo"
      ];
    }
  }

  //Cargamos datos en Tabla.
  setDataIntables() {
    this.openDialogLoading(true);
    this.InputsVariables.medicionId = this.idMedicion.toString();
    this.InputsVariables.maxRows = this.itemsPerpagina;
    this.InputsVariables.alerta = "";
    this.serviceVariableDetails.GetVariablesFiltrado(this.InputsVariables).subscribe(
        (Response) => {         
          this.responseDataTable = Response;       
          this.DataVariables = this.responseDataTable.data;
          this.totalRegister = Response.noRegistrosTotalesFiltrado;
          this.SourceDataVariables = new MatTableDataSource(this.DataVariables);
          this.dataSource = this.SourceDataVariables;
          
          if(this.DataVariables.length > 0)
          {
            this.totalRegister = this.responseDataTable.noRegistrosTotalesFiltrado;
            this.setPage(this.pageNumber);        
          }

          this.openDialogLoading(false);
        },
        (error) => {
          console.log(error);
          this.openDialogLoading(false);
        }
      );
  }

  openUpdate(element: any) {
    let convert = JSON.stringify(element);
    sessionStorage.setItem("modelo", convert);
    sessionStorage.setItem("medicion", this.detalle.enfMadre);
    sessionStorage.setItem("estado", this.estado);
    sessionStorage.setItem("detalle", JSON.stringify(this.detalle));
    this.router.navigateByUrl("/gestion-de-auditoria/variable");
  }

  
  getErrorOf() {
    this.serviceVariableDetails.getErrorOf().subscribe((Response) => {
      this.errorOf = Response;      
    },
    (error) => {
      this.openDialogLoading(false);
      this.swalError();
    });
  }

  swalError() {
    Swal.fire({
      title: "Error",
      text: "Lo sentimos, actualmente estamos presentando errores internos. Por favor intente nuevamente.",
      icon: "error",
    });
  }

  Delete(element: any) {
    //Enviamos DELETE al servicio "DELETE: api/Variables/5".    
    //console.log(element.id);
    this.serviceVariableDetails
      .deleteVariables(element.id)
      .subscribe((Response) => {
        window.location.reload();
        //objEvent = [];
      });
  }

  ConfirmacionDelete(element: any){
    Swal.fire({
      text:'¿Está seguro que desea eliminar el registro. Las tablas. Variables, VariableXMedicion, VariablesXItems, ReglasVariable se veran afectadas ?',
      confirmButtonColor: '#a94785',
      confirmButtonText: 'Continuar',
      showCancelButton: true,
      cancelButtonText:'Cancelar',
    }).then((result) => {

      if(result.isConfirmed){
        //Enviamos DELETE al servicio "DELETE: api/Variables/5".    
        //console.log(element.id);
        this.serviceVariableDetails
          .deleteVariables(element.id)
          .subscribe((Response) => {
            window.location.reload();
            //objEvent = [];
          });
      }
    });
  }

  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.data.length;
    return numSelected === numRows;
  }

  llenarLista(element: any, event: any, itemName: string) {
    this.isDisabled = false;
    switch (itemName) {
      //Capturamos valores de columna "group".
      case "group":
        if (
          this.listaAcutualizar.find((x) => x.variableId == element.variableId)
        ) {
          this.listaAcutualizar.filter(
            (x) => x.variableId == element.variableId
          )[0].subGroupId = event.toString();
        } else {
          element.subGroupId = event.toString(); //subGrupoNombre
          this.listaAcutualizar.push(element);
        }
        break;

      //Capturamos valores de columna "default".
      case "default":
        if (
          this.listaAcutualizar.find((x) => x.variableId == element.variableId)
        ) {
          this.listaAcutualizar.filter(
            (x) => x.variableId == element.variableId
          )[0].default = event.toString();
        } else {
          element.default = event.toString();
          this.listaAcutualizar.push(element);
        }
        break;
      default:
        break;
    }
  }

  llenarListaChecks(element: any, event: MatCheckboxChange, itemName: string) {
    this.isDisabled = false;
    switch (itemName) {
      //Capturamos valores de columna "calificable". esCalificable
      case "auditable":
        //   selected.target.name,
        // selected.target.value,
        // selected.target.checked
        if (
          this.listaAcutualizar.find((x) => x.variableId == element.variableId)
        ) {
          this.listaAcutualizar.filter(
            (x) => x.variableId == element.variableId
          )[0].calificable = event.checked;
        } else {
          element.calificable = event.checked;
          this.listaAcutualizar.push(element);
        }
        break;

      //Capturamos valores de columna "visible". esVisible
      case "visible":
        if (
          this.listaAcutualizar.find((x) => x.variableId == element.variableId)
        ) {
          this.listaAcutualizar.filter(
            (x) => x.variableId == element.variableId
          )[0].visible = event.checked;
        } else {
          element.visible = event.checked;
          this.listaAcutualizar.push(element);
        }
        break;
 //Capturamos valores de columna "hallazgo". hallazgos
 case "hallazgo":
  if (
    this.listaAcutualizar.find((x) => x.variableId == element.variableId)
  ) {
    this.listaAcutualizar.filter(
      (x) => x.variableId == element.variableId
    )[0].hallazgo = event.checked;
  } else {
    element.hallazgo = event.checked;
    this.listaAcutualizar.push(element);
  }
  break;
      //
      default:
        break;
    }
  }

  @HostListener("window:beforeunload", ["$event"])
  WindowBeforeUnoad($event: any) {
    if (!this.isDisabled) {
      $event.returnValue = "Your data will be lost!";
    }
  }

  btnGuardarCambios() {
    this.openDialogLoading(true);
    //Declaramos variables a usar.
    var variableId: number;
    var MedicionId: number;
    var subGrupoId: string;
    var calificacionDefault: string;
    var auditable: string;
    var visible: string;
    var hallazgo: string;

    //Declaramos listado
    var objEvent: DetalleLite[] = [];

    //Recorremos listado y asignamos valores que se van a enviar al servicio.
    this.listaAcutualizar.forEach((item, index, array) => {
      //Capturamos valores y validamos que no sean null.
      variableId = item.variableId;
      MedicionId = this.idMedicion;
      subGrupoId = item.subGroupId != null ? item.subGroupId : "";
      calificacionDefault = item.default != null ? item.default : "";
      auditable =
        item.calificable != null
          ? item.calificable == true
            ? "true"
            : "false"
          : "";
      visible =
        item.visible != null ? (item.visible == true ? "true" : "false") : "";
        hallazgo =
        item.hallazgo != null
          ? item.hallazgo == true
            ? "true"
            : "false"
          : "";
      objEvent.push({
        variableId: variableId,
        MedicionId: MedicionId,
        subGrupoId: subGrupoId,
        default: calificacionDefault,
        auditable: auditable,
        visible: visible,        
        hallazgos: hallazgo,
        idUsuario: this.objUser.userId
      });
    });
    //Enviamos POST con los datos al servicio "ActualizarVariablesLiderMasivo".
    this.serviceVariableDetails.updateVariables(objEvent)
      .subscribe((Response) => {
        this.openDialogLoading(false);
        this.setDataIntables();
        this.isDisabled = true;
        objEvent = [];
        Swal.fire({
          title: "Edición correcta",
          text: "La variable fue editada correctamente",
          icon: "success",
          confirmButtonColor: "#a94785",
          confirmButtonText: `ACEPTAR`,
        }).then(() => {
          
        });
      },
      (error) => {
        this.openDialogLoading(false);
        Swal.fire({
          title: "Error",
          text: "Error en edición de variable, por favor inténta nuevamente...",
          icon: "error",
        });
      })
  }

  /** Selects all rows if they are not all selected; otherwise clear selection. */
  masterToggle() {
    if (this.isAllSelected()) {
      this.selection.clear();
      return;
    }
    this.selection.select(...this.dataSource.data);
  }

  checkboxLabel(row?: Detalle): string {
    if (!row) {
      return `${this.isAllSelected() ? "deselect" : "select"} all`;
    }
    return `${this.selection.isSelected(row) ? "deselect" : "select"} row ${
      row.variable + 1
    }`;
  }

  checkboxLabel2(row?: Detalle): string {
    if (!row) {
      return `${this.isAllSelected() ? "deselect" : "select"} all`;
    }
    return `${this.selection2.isSelected(row) ? "deselect" : "select"} row ${
      row.variable + 1
    }`;
  }
  checkboxLabel3(row?: Detalle): string {
    if (!row) {
      return `${this.isAllSelected() ? "deselect" : "select"} all`;
    }
    return `${this.selection3.isSelected(row) ? "deselect" : "select"} row ${
      row.variable + 1
    }`;
  }

  @ViewChild(MatSort) sort!: MatSort;

  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
  }

  btnVariable() {
    this.openDialogLoading(false);

    let element = new VariableResponseModel();
    element.medicionId =  this.detalle.idMedicion;
    let convert = JSON.stringify(element);
    sessionStorage.setItem("modelo", convert);
    sessionStorage.setItem("medicion", this.detalle.medicion);
    sessionStorage.setItem("estado", this.estado);
    sessionStorage.setItem("enfMadre", this.detalle.enfMadre);
    sessionStorage.setItem("detalle", JSON.stringify(this.detalle));
    this.router.navigateByUrl("/gestion-de-auditoria/variable");
  }

  btnSegmentar() {
    this.router.navigateByUrl("/gestion-de-auditoria/segmentacion-variable");
  }

  actualizarVariableAuditable(
    variableId: any,
    MedicionId: any,
    subGrupoId: any,
    auditable: any
  ) {
    this.updateVariables.variableId = variableId;
    this.updateVariables.MedicionId = MedicionId;
    this.updateVariables.subGrupoId = subGrupoId;
    this.updateVariables.idUsuario  = this.objUser.userId;
    if (auditable) {
      this.updateVariables.auditable = "1";
    } else {
      this.updateVariables.auditable = "0";
    }

    this.serviceVariableDetails.updateVariables(this.updateVariables)
      .subscribe((Response) => {});
  }

  actualizarVariableVisible(variableId: any, MedicionId: any, subGrupoId: any, visible: any) {
    this.updateVariables.variableId = variableId;
    this.updateVariables.MedicionId - MedicionId;
    this.updateVariables.subGrupoId = subGrupoId;    
    this.updateVariables.idUsuario  = this.objUser.userId;
    this.updateVariables.hallazgos = "false";
    if (visible) {
      this.updateVariables.visible = 1;
    } else {
      this.updateVariables.visible = 0;
    }

    this.serviceVariableDetails.updateVariables(this.updateVariables)
      .subscribe((Response) => {});
  }

  getCatalogo() {
    this.serviceVariableDetails.getCatalogo().subscribe((Response) => {
      this.catalogo = Response;
    });
  }

  getTipoVariable() {
    this.serviceVariableDetails.GetTipo().subscribe((Response) => {
      this.tipoVariable = Response;
    });
  }

  getDefaultValue() {
    this.serviceVariableDetails.GetDefaultValue().subscribe((Response) => {
      this.defaultValue = Response;
    });
  }

  getGrupo() {
    this.serviceVariableDetails.GetGrupo().subscribe((Response) => {
      this.grupoValue = Response;
    });
  }

  setFilter(numeroPagina: number, size: number) {
    this.pageNumber = numeroPagina;
    this.InputsVariables.subGrupoId.splice(
      0,
      this.InputsVariables.subGrupoId.length
    );
    this.InputsVariables.tipoVariableItem.splice(
      0,
      this.InputsVariables.tipoVariableItem.length
    );
    this.InputsVariables.nombre = "";
    this.InputsVariables.descripcion = "";
    this.InputsVariables.variable = "";
    this.pageNumber = numeroPagina;
    this.itemsPerpagina = size;
    this.InputsVariables.maxRows = this.itemsPerpagina;
    this.InputsVariables.pageNumber = numeroPagina - 1;
    if (this.catalogoSelect) {
      this.catalogoSelect.forEach((catalogo: any) => {
        this.InputsVariables.subGrupoId.push(catalogo.toString());
      });
    }
    if (this.tipoVariableSelect) {
      this.tipoVariableSelect.forEach((tipoVariable: any) => {
        this.InputsVariables.tipoVariableItem.push(tipoVariable.toString());
      });
    }

    if (this.textBuscar) {
      this.InputsVariables.nombre = this.textBuscar;
      this.InputsVariables.descripcion = this.textBuscar;
      this.InputsVariables.variable = this.textBuscar;
      this.InputsVariables.orden = this.textBuscar;
    } else {
      this.InputsVariables.nombre = "";
      this.InputsVariables.descripcion = "";
      this.InputsVariables.variable = "";
      this.InputsVariables.orden = "";
    }

    this.setDataIntables();
  }

  cleanFilters() {
    this.InputsVariables.nombre = "";
    this.InputsVariables.descripcion = "";
    this.InputsVariables.variable = "";
    this.InputsVariables.subGrupoId = [""];
    this.InputsVariables.tipoVariableItem = [""];
    this.InputsVariables.orden = "";
    this.catalogoSelect = [];
    this.textBuscar = "";
    this.tipoVariableSelect = [];
    this.setDataIntables();
  }

  openDialogLoading(loading: boolean): void {
    if (loading) {
      this.dialog.open(LoadingComponent, {
        disableClose: true,
        data: {},
      });
    } else {
      this.dialog.closeAll();
    }
  }

  setPage(page: number) {
    this.pager = this.paginatorService.getPager(
      this.totalRegister,
      page,
      this.itemsPerpagina
    );
  }
  validarEstado():boolean{
    return this.estado!=EstadosBolsa.Creada?true:false;
  }
}
//---
