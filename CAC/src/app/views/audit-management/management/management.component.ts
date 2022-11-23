import { SelectionModel } from "@angular/cdk/collections";
import { AfterViewInit, Component, OnInit, ViewChild } from "@angular/core";
import { MatSort } from "@angular/material/sort";
import { MatTableDataSource } from "@angular/material/table";
import { Router } from "@angular/router";
import { DialogComponent } from "../dialog/dialog.component";
import {
  MatDialog,
  MatDialogRef,
  MAT_DIALOG_DATA,
} from "@angular/material/dialog";
import { ManagementService } from "../services/management.service";
import {
  listFiltros,
  objCobertura,
  objDuplicarMedicion,
  objFiltro,
  objPeriodo,
  objRegistroEstado,
  responsePeriodo,
} from "src/utils/models/manager/management";
import { detalleRegistro } from "src/utils/models/cronograma/information";
import { CookieService } from "ngx-cookie-service";
import { objuser } from "src/utils/models/cronograma/cronograma";
import { PaginationDto } from "src/utils/models/cronograma/pagination";
import { PagerService } from "../../cronograma/services/page.service";
import { LoadingComponent } from "src/app/layout/loading/loading.component";
import Swal from "sweetalert2";
import { ModalCarguePoblacionComponent } from "../modal-cargue-poblacion/modal-cargue-poblacion.component";
import { ProcessService } from "../services/process.service";
import { enumProcesos, enumValidationProcess } from "src/app/model/Util/enumerations.enum";
import { CurrentProcessModel } from "src/app/model/proceso/currentprocess.model";
import { ModalMoverBolsaComponent } from "../modal-mover-bolsa/modal-mover-bolsa.component";
import { GetPermisosServices } from "src/app/services/get-permisos.services";
import { ModalEliminarRegistroAuditoriaComponent } from "../modal-eliminar-registro-auditoria/modal-eliminar-registro-auditoria.component";
import { ModalCalificacionMasivaComponent } from "../modal-calificacion-masiva/modal-calificacion-masiva.component";

export interface PeriodicElement {
  registros: string;
  etiqueta: string;
  asignados: string;
  variables: string;
  auditados: string;
  creado: string;
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
];

@Component({
  selector: "app-management",
  templateUrl: "./management.component.html",
  styleUrls: ["./management.component.scss"],
  providers: [],
})
export class ManagementComponent implements AfterViewInit, OnInit {
  estadosFiltro: any;
  name: string = "";
  content: string = "";
  type: string = "";
  type_select: data[] = selector;
  estadosInput: any = null;
  objCobertura: objCobertura = new objCobertura();
  listCoberturas: any;
  objRegistroEstado: objRegistroEstado = new objRegistroEstado();

  detalleRegistro: detalleRegistro[] = [];
  
  
  //user
  userEncoded: any;
  objUser: objuser;
  
  objMedicion:any;
  objPeriodo: objPeriodo = new objPeriodo();
  listPerido: responsePeriodo[] = [];
  objPeriodoFiltro: any;
  objFiltro: objFiltro = new objFiltro();
  objDuplicarMedicion: objDuplicarMedicion = new objDuplicarMedicion();
  // objFiltro:any;
  //listFiltros: listFiltros[] = [];
  listFiltros: any;
  IdEnfMadre: any = "";
  IdPeriodo: any = "";
  IdAuditoria = [];
  enabledListPeriodo: boolean = false;

  dataTable: any[] = [];

  pageNumber: any;
  intialPosition: any;
  itemsPerpagina: number = 50;
  sizeList: number[] = [50, 100, 150, 200];
  page: number = 1;

  pagination!: PaginationDto;
  totalRegister: number = 0;
  totalLiderRegister: number = 0;
  pager: any = {};

  estados: any[] = [];
  enfMadres: any[] = [];

  idEnfMadre: any = "";
  idEstado: any[] = [];
  search = "";
  rol : any= "";
  title: any= "";
  show: boolean = false;
  estadoAuditoria:any;

  dropdownSelected:any;
  
  constructor(
    private router: Router,
    private coockie: CookieService,
    private services: ManagementService,
    private paginatorService: PagerService,
    public dialog: MatDialog,
    public processService: ProcessService,
    public permisos: GetPermisosServices
  ) {

    this.userEncoded = sessionStorage.getItem("objUser");
    this.objUser = JSON.parse(this.userEncoded);
  }

  openDialogEliminar(medicion: any): void {
    this.name = "¿Está seguro que desea eliminar la bolsa?";
    this.content = "Una vez elimine la bolsa no podrá recuperarla";
    this.type = "eliminar";
    const dialogRef = this.dialog.open(DialogComponent, {
      width: "400px",
      data: { name: this.name, content: this.content, type: this.type },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        console.log('result', result);
        console.log('meidiones', medicion);
        if(medicion.estadoAuditoria == 'Creada'){

          this.services.deleteMedicion(medicion.idMedicion).subscribe((Response) => {
            
              Swal.fire({
                title: 'Eliminado',
                text: "El registro ha sido eliminado con exito",
                confirmButtonColor: "#a94785",
                confirmButtonText: "ENTENDIDO"
              })

              this.router.navigateByUrl("/gestion-de-auditoria");
              this.show = false;
              setTimeout(() => {
                this.getFiltros('', '', '','',this.dropdownSelected);
                this.show = true;
              }, 1000);
            });

        }else{
          Swal.fire({
            title: 'Esta bolsa ya esta asignada a un usuario',
            text: "No se puede eliminar una bolsa en un estado diferente a 'Creada'",
            confirmButtonColor: "#a94785",
            confirmButtonText: "ENTENDIDO"
          })
        }

      }
    });
  }

  openDialogDuplicar(medicion: any): void {
    this.name = "¿Está seguro que desea duplicar la bolsa?";
    this.content = "Una vez duplique la bolsa, esta sera mostrada con el mismo nombre y un 'Copia -' al inicio";
    this.type = "duplicar";
    const dialogRef = this.dialog.open(DialogComponent, {
      width: "400px",
      data: { name: this.name, content: this.content, type: this.type },
    });
    
    this.objDuplicarMedicion.medicionId = medicion.idMedicion;
    this.objDuplicarMedicion.userCreatedBy = this.objUser.userId;
    dialogRef.afterClosed().subscribe((result) => {    
      //console.log(this.objDuplicarMedicion);
      if (result) {
          this.services.duplicarMedicion(this.objDuplicarMedicion).subscribe((Response) => {
              this.router.navigateByUrl("/gestion-de-auditoria");
              this.show = false;
              setTimeout(() => {
                this.getFiltros('', '', '','',this.dropdownSelected);
                this.show = true;
              }, 1000);
          });
      }
    });
  }

  openDialogMover(): void {
    this.name = "Seleccione la bolsa de destino de los registros";
    this.content = "La medición mantendrá el nombre de la bolsa de destino";
    this.type = "mover";
    const dialogRef = this.dialog.open(DialogComponent, {
      width: "400px",
      data: { name: this.name, content: this.content, type: this.type },
    });

    dialogRef.afterClosed().subscribe((result) => {});
  }

  openDialogEliminarMover(): void {
    this.name = "La bolsa a eliminar no debe contener registros";
    this.content = "Mueva los registros a otra bolsa antes de eliminarla";
    this.type = "eliminar";
    const dialogRef = this.dialog.open(DialogComponent, {
      width: "400px",
      data: { name: this.name, content: this.content, type: this.type },
    });

    dialogRef.afterClosed().subscribe((result) => {
      if (result) {
        this.openDialogMover();
      }
    });
  }

  displayedColumns2: string[] = [
    "medicion",
    "madre",
    "registros",
    "asignados",
    "auditados",
    "periodo",
    "estado",
  ]; // Lider
  displayedColumns: string[] = [
    "star",
    "select",
    "medicion",
    "bot",
    "registros",
    "asignados",
    "madre",
    "auditados",
    "estado",
    // "lider",
    "resolucion",
    "fechaCreacion",
    "ultimaModificacion",
  ]; // Admin
  dataSource = new MatTableDataSource(this.dataTable); 
  selection = new SelectionModel<PeriodicElement>(true, []);
  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.data.length;
    return numSelected === numRows;
  }

  /** Selects all rows if they are not all selected; otherwise clear selection. */
  masterToggle() {
    if (this.isAllSelected()) {
      this.selection.clear();
      return;
    }

    this.selection.select(...this.dataSource.data);
  }

  /** The label for the checkbox on the passed row */
  checkboxLabel(row?: PeriodicElement): string {
    if (!row) {
      return `${this.isAllSelected() ? "deselect" : "select"} all`;
    }
    return `${this.selection.isSelected(row) ? "deselect" : "select"} row ${
      row.etiqueta + 1
    }`;
  }

  @ViewChild(MatSort) sort!: MatSort;

  ngAfterViewInit() {
    this.dataSource.sort = this.sort;
    
  }

  btnAsignarCronograma() {
    this.router.navigateByUrl("/gestion-de-auditoria/asignar-cronograma");
  }

  btnMedicion(objMedicion: any) {
    console.log(objMedicion)
    var encoded = btoa(JSON.stringify(objMedicion));
    this.router.navigateByUrl("/gestion-de-auditoria/medicion/" + encoded);
  }

  btnAsignacion(objMedicion: any) {  
    
     var encoded = btoa(JSON.stringify(objMedicion));
    this.router.navigateByUrl("/gestion-de-auditoria/asignar-bolsa/" + encoded);
  }

  btnDetalleVariable(event: any) {
    var encoded = btoa(JSON.stringify(event));
    this.router.navigateByUrl(
      "/gestion-de-auditoria/detalle-variable/" +
        encoded       
    );
  }

  
 
  ngOnInit(): void {
    this.getFiltros('', '', 1,this.itemsPerpagina,'Todos');
    this.getState();
    this.rol = sessionStorage.getItem('rol');
    this.title= ( this.rol  =='Admin' ? this.objUser.name : this.objUser.name)+', aquí puede gestionar sus auditorías';      
   if( this.rol  != 'Admin' ){
    this.displayedColumns= [
      "star", 
      "idbolsa",          
      "medicion",
      "bot",
      "madre",
      "registros",
      "asignados",
     
      "auditados",
      "estado",
      // "lider",
      "resolucion",
      // "progreso",
      "fechaInicio",
      "fechaFin",
      "fechaCreacion",
      "ultimaModificacion"    
    ];
   }else{
    this.displayedColumns= [
      "star",
      "select",
      "idbolsa",
      "medicion",
      "bot",
      "madre",
      "registros",
      "asignados",      
      "auditados",
      "estado",
      // "lider",
      "resolucion",
      // "progreso",
      "fechaInicio",
      "fechaFin",
      "fechaCreacion",
      "ultimaModificacion",
    ];
   }
  



  }

  applyFilter(filterValue: any) {
    this.dataSource.filter = filterValue.target.value.trim().toLowerCase();
  }

  cleanFilters() {
    this.estadosInput = null;
    this.idEnfMadre = "";
    this.search = "";
    this.idEstado = [];
    this.estadosFiltro = [];
    this.listadoEnfermedades = [];
    this.estadoAuditoria = '';
    this.getFiltros('', '', 1,this.itemsPerpagina,'Todos');
    this.getState();
  }

  getSelectedOptions(selected: any) {
    this.idEstado = selected.value;
  }


  listadoEnfermedades:any[] = [];
  //monto la logica de los filtros
  getFiltros(IdEnfMadre : any,IdPeriodo : any,page : any, itemsPerpagina : any, nombreSeleccion:string) {
    this.openDialogLoading(true);
    if(page && itemsPerpagina){      
      this.itemsPerpagina = itemsPerpagina;
      this.page = page;
    }
    this.dataTable = [];
    this.objFiltro.idLider = this.objUser.userId;
    this.objFiltro.idCobertura = this.idEnfMadre.toString();
    this.objFiltro.idEstado = this.idEstado;
    this.objFiltro.maxRows = this.itemsPerpagina;
    this.objFiltro.PageNumber = this.page - 1;
    this.pageNumber = page;
    this.services.getFiltros(this.objFiltro).subscribe((Response) => {
      this.listFiltros = Response.data; //CAMBIOACTUAL
      this.totalRegister = Response.noRegistrosTotalesFiltrado;
      this.setPage(this.page);      
      sessionStorage.setItem("Bolsas", JSON.stringify(Response));
      this.dataTable = this.listFiltros;
      this.enfMadres = this.removeDuplicates(this.listFiltros, "enfMadre");
      if(this.listadoEnfermedades.length == 0){
        this.listadoEnfermedades = this.enfMadres;
      }
      this.dataSource = new MatTableDataSource(this.dataTable);
      this.openDialogLoading(false);
    },error => {
      this.openDialogLoading(false);
    });
  }

  removeDuplicates(originalArray: any, prop: any) {
    var newArray = [];
    var lookupObject: any = {};
    for (var i in originalArray) {
      lookupObject[originalArray[i][prop]] = originalArray[i];
    }
    for (i in lookupObject) {
      newArray.push(lookupObject[i]);
    }
    return newArray;
  }

  //seteo la lista del periodo
  setListPeriodo() {
    this.objPeriodoFiltro = this.listFiltros.data.periodosAsignados.filter(
      (x: listFiltros) => x.idCobertura == this.IdEnfMadre
    );
    if (this.objPeriodoFiltro) {
      this.enabledListPeriodo = true;
    }
  }

  //seteo el paginador
  setPage(page: number) {
    this.pager = this.paginatorService.getPager(
      this.totalRegister,
      page,
      this.itemsPerpagina
    );
  }

  getState() {
    this.services.getStates().subscribe((Response) => {
      this.estadosFiltro = Response;
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
  openModalEliminar_Registro_Auditoria(idMedicion: number, nombreMedicion: string)
  {
    let obj = { idMedicion: idMedicion , nombreMedicion: nombreMedicion};
    const dialogRef = this.dialog.open(ModalEliminarRegistroAuditoriaComponent, {
      data: obj,
      width: '500px',
      disableClose: false
    }).afterClosed().subscribe((res) => { 
      this.dataTable = [];
      this.objFiltro.idLider = this.objUser.userId;
      this.objFiltro.idCobertura = this.idEnfMadre.toString();
      this.objFiltro.idEstado = this.idEstado;
      this.services.getFiltros(this.objFiltro)
      this.getFiltros(this.IdEnfMadre,this.IdPeriodo,1, this.itemsPerpagina,this.dropdownSelected);
      this.getState();
    }); 
  }
  openModalCarguePoblacion(idMedicion: number, nombreMedicion: string, esConSubGrupo: boolean)
  {
    this.objMedicion = {
      idMedicion: idMedicion,
      nombreMedicion: nombreMedicion,
      esConSubGrupo: esConSubGrupo
    } 

    const dialogRef = this.dialog.open(ModalCarguePoblacionComponent, {
      data: this.objMedicion,
      width: '450px',
      disableClose: true
    }).afterClosed().subscribe((res) => { 
     
      this.dataTable = [];
      this.objFiltro.idLider = this.objUser.userId;
      this.objFiltro.idCobertura = this.idEnfMadre.toString();
      this.objFiltro.idEstado = this.idEstado;
      this.services.getFiltros(this.objFiltro)
        this.getFiltros(this.IdEnfMadre,this.IdPeriodo,1, this.itemsPerpagina,this.dropdownSelected);
    }); 
    
  }

  openModalCalificacionMasiva(idMedicion: number, nombreMedicion: string, esConSubGrupo: boolean)
  {
    this.objMedicion = {
      idMedicion: idMedicion,
      nombreMedicion: nombreMedicion,
      esConSubGrupo: esConSubGrupo
    } 

    const dialogRef = this.dialog.open(ModalCalificacionMasivaComponent, {
      data: this.objMedicion,
      width: '450px',
      disableClose: true
    }).afterClosed().subscribe((res) => { 
     
      this.dataTable = [];
      this.objFiltro.idLider = this.objUser.userId;
      this.objFiltro.idCobertura = this.idEnfMadre.toString();
      this.objFiltro.idEstado = this.idEstado;
      this.services.getFiltros(this.objFiltro)
        this.getFiltros(this.IdEnfMadre,this.IdPeriodo,1, this.itemsPerpagina,this.dropdownSelected);
    }); 
    
  }

  openModalBtnMover(obj:any)
  { 
   // const obj={obj: this.buildObjFilter(true),tipo:this.tipo};
    const dialogRef = this.dialog.open(ModalMoverBolsaComponent, {
      data: obj,
      width: '500px',
    }).afterClosed().subscribe((res) => {      
      this.dataTable = [];
      this.objFiltro.idLider = this.objUser.userId;
      this.objFiltro.idCobertura = this.idEnfMadre.toString();
      this.objFiltro.idEstado = this.idEstado;
      this.services.getFiltros(this.objFiltro)
        this.getFiltros(this.IdEnfMadre,this.IdPeriodo,1, this.itemsPerpagina,this.dropdownSelected);
    }); 
    ;
  }

 
}
