import { AfterViewInit, Component, OnInit, ViewChild } from "@angular/core";
import { SelectionModel } from "@angular/cdk/collections";
import { FormGroup, FormBuilder } from "@angular/forms";
import { MatTableDataSource } from "@angular/material/table";
import {
  estado,
  tabla,
  data,
  objuser,
  filtro,
} from "src/utils/models/cronograma/cronograma";
import { Router } from "@angular/router";
import * as moment from "moment";
import { MatDialog } from "@angular/material/dialog";
import { LoadingComponent } from "src/app/layout/loading/loading.component";
import Swal from "sweetalert2";
import { MatPaginator } from "@angular/material/paginator";
import { PaginationDto } from "src/utils/models/cronograma/pagination";
import { environment } from "src/environments/environment";

//Services
import { GeneralServices } from "src/app/services/generalservices";
import { PagerService } from "../services/page.service";
import { CookieService } from "ngx-cookie-service";
import { CronogramaService } from "../services/cronograma.service";
import { ParametroModel } from "src/app/model/Util/parametro.model";
import {
  enumParametrosGenerales,
  requestBase,
} from "src/app/model/Util/enumerations.enum";
import { DetalleAsignacionModel } from "src/utils/models/cronograma/detalleasignacion.model";
import { THIS_EXPR } from "@angular/compiler/src/output/output_ast";
import { interval } from "rxjs";
import { AuditorTraceabilityComponent } from "../traceability/auditor-traceability/auditor-traceability.component";
import { UsuarioLiderModel } from "src/app/model/registroauditoria/usuariolider.model";
import { GetPermisosServices } from "src/app/services/get-permisos.services";

@Component({
  selector: "app-list-cronograma",
  templateUrl: "./list-cronograma.component.html",
  styleUrls: ["./list-cronograma.component.scss"],
  providers: [],
})
export class ListCronogramaComponent implements OnInit, AfterViewInit {
  @ViewChild(MatPaginator) paginator?: MatPaginator;

  ELEMENT_DATA: tabla[] = [];
  displayedColumns!: string[];
  dataSource2: any;
  estados_front!: estado[];
  selection: any;
  estados!: estado[];
  loading: boolean = true;
  progreso: any;
  progresoDia: number = 0;
  diasTotales: number = 0;
  filterData!: data;
  listDetails!: FormGroup;
  _requestBase: requestBase = new requestBase();

  listaAuditor: UsuarioLiderModel[] = [];
  selectedValue!: string;
  filtrados!: filtro[];
  Medicion: any = 0;
  Estado: any = 0;
  Entidad: any = 0;
  primerNombre: string = "";
  totalPage: any;
  datos = new data();
  idLider: any;
  idAuditor: string = '';
  auditorListId: string[] = [];
  searchValue: string = "";
  curSec: number = 0;
  pageNumber: any;
  intialPosition: any;
  prueba: [] = [];
  currentPage: any;
  pageSize: any;
  pagination!: PaginationDto;
  totalRegister: number = 0;
  totalLiderRegister: number = 0;
  pager: any = {};
  itemsPerpagina: number = 50;
  sizeList: number[] = [50, 100, 150, 200];
  paramterAssignationGroup: ParametroModel = new ParametroModel();
  detalleAsignacionList: DetalleAsignacionModel[] = [];
  spinnerCarga: number = 0;
  logActividades: boolean = false;
  generateInforme: boolean = false;
  informeGenerado: boolean = false;
  base64Data: string = "";
  fileName: string = "";
  listEstados:any[] = [];
  listEstadosTotal:any[] = [];
  perfilEstados:number = 0;
  perfiles: any[] = ["1", "2"];
  selectedUser: string ="";
  perfilAccionList: any[] = [];

  //user
  userEncoded: any;
  objUser: objuser;

  constructor(
    private router: Router,
    private formBuilder: FormBuilder,
    private service: CronogramaService,
    private generalServices: GeneralServices,
    private paginatorService: PagerService,
    public dialog: MatDialog,
    private coockie: CookieService,
    public permisos: GetPermisosServices
  ) {
    
    this.userEncoded = sessionStorage.getItem("objUser");
    this.objUser = JSON.parse(this.userEncoded);

    this.primerNombre = this.objUser.name;
    this.GetConsultaPerfilAccion();
  }

  ngOnInit(): void {
    this.consultaUsuariosLider();
    this.listDetails = this.formBuilder.group({
      search: [""],
      medition: [""],
      state: ["1"],
      entity: [""],
      Enddate: ["", []],
      Startdate: ["", []],
      pAccion: [""],
    });
    this.searchValue = "";
    if (this.objUser.rol.userRolId !== "2") {
      this.idAuditor = this.objUser.userId;
      this.getProgresoDiario();
    } else if (this.objUser.rol.userRolId !== "3") {
      this.datos.idLider = this.objUser.userId;
      this.idLider = this.objUser.userId;
      this.idAuditor = this.objUser.userId;
      this.getProgresoDiario();

      this.selectedUser = "Todos";
      this.datos.idAuditor = "all";  //T
      this.logActividades = true;
      this.generateInforme = true;
      this.idAuditor = "all"; //T
      this.datos.idLider = "all"; //T

    }

    this.datos.fechaAsignacionIni = moment().utc().format("YYYY-MM-DD");
    this.datos.fechaAsignacionFin = moment().utc().format("YYYY-MM-DD");

    this.listDetails.reset();

   
    //consultamos los registros pendientes por acción del Lider
    if (this.objUser.rol.userRolId == "2") this.getLiderContador();

    //Consulta los estados de registros
    this.loadStates();

    this.idLider = this.objUser.userId;
    //this.setDataIntables();

    this.GetDetalleAsignacion();

    this.listDetails.reset();


     this.SetDefaultStatus();

    }
 

    SetDefaultStatus()
    {
      // Asigna estados default para consulta inicial
      let defaultStatus  = ["1","17"];
      this.listDetails.controls.state.setValue(defaultStatus); 
    }

  searchValueFunction(searchv: string) {
    this.searchValue = searchv;
    this.getFilters(1, this.itemsPerpagina);
  }

  //Carga los registros por cada uno de los grupos de detalle de asignacion
  filterByState(state: any) {
    this.listDetails.reset();
    this.datos.fechaAsignacionIni = "";
    this.datos.fechaAsignacionFin = "";
    this.datos.estado = this.detalleAsignacionList.filter(
      (x: any) => x.nombre == state
    )[0].estados as [];
    this.setDataIntables();
  }

  loadStates(){
    this.service.getEstadosRegistros(1).subscribe(
      (Response) => {   
        this.listEstadosTotal = Response;
      },
      (error) => {
        this.openDialogLoading(false);
        Swal.fire({
          title: "Error",
          text: "Lo sentimos, actualmente estamos presentando errores internos. Por favor intente nuevamente.",
          icon: "error",
        });
      }
    );
  }
  
  //Carga los registros por cada uno de los perfiles de accion
  filterByActionPerfil(estados: any) {  
    this.datos.estado = estados;
    this.setDataIntables(); 
  }
  
  setFilter() { 
    this.filtrados.forEach((element) => { 
      if (element.nombreFiltro == "Bolsa medicion") {
        this.Medicion = element.detalle;
      } //medeicion
      if (element.nombreFiltro == "Estado de registro") {     
        if(this.perfilEstados == 1 || this.perfilEstados == 2){  
          this.Estado = [];
          this.listEstados.forEach(estados => {
            this.Estado.push(element.detalle.find((x:any) => x.id == estados));            
            this.Estado.sort(function(a:any,b:any){return a.id - b.id;}); 
          });
        }else{
          this.Estado = element.detalle;
          this.Estado.sort(function(a:any,b:any){return a.id - b.id;}); 
        } 
      } //Estado
      if (element.nombreFiltro == "Entidad") {
        this.Entidad = element.detalle;
      } //Entidad
    });
  }

  openLog() {
    const dialogRef = this.dialog.open(AuditorTraceabilityComponent, {
      width: '1000px',
      data: { listaauditores: this.listaAuditor, selected: this.idAuditor, listMedicion:this.Medicion}

    });
    dialogRef.afterClosed().subscribe((res) => {
    });
  }

  setDataIntables() {
    console.log('sent Data');
    // this.BuildFilters();
    // this.datos.maxRows = this.itemsPerpagina;
    // this.service.postDataTable(this.datos).subscribe(
    //   (Response) => {  
    //     this.GetDetalleAsignacion();
    //     this.ELEMENT_DATA = Response.data;
    //     this.prueba = Response; 
    //   },
    //   (error) => {
    //     this.openDialogLoading(false);
    //     Swal.fire({
    //       title: "Error",
    //       text: "Lo sentimos, actualmente estamos presentando errores internos. Por favor intente nuevamente",
    //       icon: "error",
    //     });
    //   }
    // );
    this.datos.maxRows = this.itemsPerpagina;
    this.GetDetalleAsignacion();
  }

  //Request para consultar filtros
  BuildFilters() {    
    if(this.idAuditor == 'all'){
      this.service.postFiltros("all").subscribe((Response) => {
        this.filtrados = Response;      
        this.setFilter();
      });
    }else{
      this.service.postFiltros(this.idAuditor).subscribe((Response) => {
        this.filtrados = Response;       
        this.setFilter();
      });
    }
    
  }
   
  //Request para consultar detalle asignacion
  GetDetalleAsignacion() {
    this.openDialogLoading(true);
    this.auditorListId = [];

    if(this.idAuditor == 'all')
    {
      this.idAuditor = this.objUser.userId;
    }
    this.auditorListId.push(this.idAuditor);
    this.service
      .postDataDetalleAsignacion(this.auditorListId, this.objUser.userId)
      .subscribe(
        (Response) => {
          this.detalleAsignacionList = Response;
          this.BuildFilters();
          this.llenarTablaAuditor();
          this.openDialogLoading(false);
          this.getFilters(1, this.itemsPerpagina);
          this.dataSource2.paginator = this.paginator;
        },
        (error) => {
          this.openDialogLoading(false);
          Swal.fire({
            title: "Error",
            text: "Lo sentimos, actualmente estamos presentando errores internos. Por favor intente nuevamente",
            icon: "error",
          });
        }
      );
  }

  //Request para consultar registros pendientes para Líder
  getLiderContador(){
    this.service.GetLiderIssues(this.objUser.userId).subscribe(Response => {  
      this.totalLiderRegister = Response;
    },error => {
      this.openDialogLoading(false);
      Swal.fire({
        title: 'Error',
        html: 'Lo sentimos, actualmente estamos presentando errores internos. Por favor intente nuevamente',
        icon: 'error'
      })
    });
  }

  consultaUsuariosLider(){
    this.service.consultaUsuariosLider(this.objUser.userId).subscribe(Response => {
      this.listaAuditor = Response;     
    })
  }

  //Acciones al cambier el auditor del dropdown
  changeAuditor(value: any, userSelected: any) { 
    this.selectedUser = userSelected;
    if(value == "all"){
      this.datos.idAuditor = value;
    }
    this.logActividades = true;
    this.generateInforme = true;
    this.idAuditor = value;
    this.datos.idLider = value;
    this.GetDetalleAsignacion();
  } 

  getProgresoDiario() {
    const idAuditor = {
      idAuditor: this.idAuditor,
    };
    this.service.postProgresoDiario(idAuditor).subscribe(
      (Response) => {
        this.progreso = Response[0];
        this.progresoDia = this.progreso.progresoDia;
        this.diasTotales = this.progreso.totales;
      },
      (error) => {
        Swal.fire({
          title: "Error",
          text: "Lo sentimos, actualmente estamos presentando errores internos. Por favor intente nuevamente",
          icon: "error",
        });
      }
    );
  }

  llenarTablaAuditor() {
    if (this.objUser.rol.userRolId == 2) {
      //lider == 2
      this.displayedColumns = [
        "check",
        "id",
        "icon",
        "estado",
        "emadre",
        "nombreMedicion",
        "ultimaapertura",
        "fechaasignacion",
        "CodigoAuditor",
        "auditor",
        "eps",
        // "ips",
        "fechacierre",
      ];
    } else {
      //Auditor == 3
      this.displayedColumns = [
       
        
        "id",
        "icon",
        "estado",
        "emadre",
        "fechadeasignacion",
        "nombreMedicion",
        "eps",
        // "ips", 
        "fechadecierre",
        "displayOrder",
            
      ];
    }
    this.dataSource2 = new MatTableDataSource(this.ELEMENT_DATA);
    this.estados_front = this.estados;
    this.selection = new SelectionModel<tabla>(true, []);
  }

  ngAfterViewInit() {
    if (this.dataSource2) {
      this.dataSource2.paginator = this.paginator;
    }
  }

  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource2.data.length;
    return numSelected === numRows;
  }

  /** Selects all rows if they are not all selected; otherwise clear selection. */
  masterToggle() {
    if (this.isAllSelected()) {
      this.selection.clear();
      return;
    }

    this.selection.select(...this.dataSource2.data);
  }

  /** The label for the checkbox on the passed row */
  checkboxLabel(row?: tabla): string {
    if (!row) {
      return `${this.isAllSelected() ? "deselect" : "select"} all`;
    }
    return `${this.selection.isSelected(row) ? "deselect" : "select"} row ${
      row.id + 1
    }`;
  }

  redirect(element: any) {
    if(this.permisos.Auditoria.visible){
      var encoded = btoa(JSON.stringify(element));
      this.coockie.set("objAuditar", encoded);
      this.router.navigate(["/banco-de-informacion"]);
    }
    
  }


  //esta funcion sirve para obtener los filtros
  getFilters(numeroPagina: number, size: number) {
    console.log('sent getfilters');
    this.openDialogLoading(true);
    this.filterData = new data();
    this.pageNumber = numeroPagina;
    this.itemsPerpagina = size;
    this.filterData.maxRows = this.itemsPerpagina;
    this.filterData.pageNumber = numeroPagina - 1;

    if (this.listDetails.get("search")?.value) {
      this.filterData.idRadicado = this.listDetails.get("search")?.value;
      //this.filterData.id = this.listDetails.get("search")?.value;
    }
    // if(this.searchValue != "")
    // {
    //   this.filterData.id = this.searchValue;
    // }
    // else
    // {
    //   this.filterData.id = "";
    // }

    if (this.listDetails.get("medition")?.value) {
      const medition = this.listDetails.get("medition")?.value;
      this.filterData.idMedicion = medition;
    } 

    if(this.listDetails.get('pAccion')?.value){
      const pAccion = this.listDetails.get('pAccion')?.value;
      if(pAccion == 1){
        this.filterData.accionLider = '1'; 
      }else if(pAccion == 2){
        this.filterData.accionAuditor = '2';
      }
    }

    if (this.listDetails.get("state")?.value) {
      const state = this.listDetails.get("state")?.value;
      this.filterData.estado = state;
    } else {
      this.filterData.estado = this.datos.estado; //ToDo: validar doble request
    }

    if (this.listDetails.get("entity")?.value) {
      const entity = this.listDetails.get("entity")?.value;
      this.filterData.codigoEps = entity;
    }

    if (this.listDetails.get("Startdate")?.value) {
      const fechaInicio = this.listDetails.get("Startdate")?.value;
      const fechaFin = this.listDetails.get("Enddate")?.value;
      var fecha = this.convertDate(fechaInicio);
      this.filterData.fechaAsignacionIni = fecha;
      if (!fechaFin) this.filterData.fechaCreacionFin = fecha;
    }

    if (this.listDetails.get("Enddate")?.value) {
      const fechaFin = this.listDetails.get("Enddate")?.value;
      var fecha = this.convertDate(fechaFin);
      this.filterData.fechaAsignacionFin = fecha;
    }
    if(this.objUser.rol.userRolId == '2'){
      this.filterData.idLider = this.idLider;
      this.filterData.idAuditor = this.idAuditor;
    }else{
      this.filterData.idAuditor = this.idAuditor;
    } 
    this.datos.maxRows = this.itemsPerpagina;
    this.service.postDataTable(this.filterData).subscribe(
      (Response) => {
        this.openDialogLoading(false);
        this.ELEMENT_DATA = Response.data;
        this.llenarTablaAuditor();
        this.totalRegister = Response.noRegistrosTotalesFiltrado;
        this.setPage(this.pageNumber);
      },
      (error) => {
        this.openDialogLoading(false);
        Swal.fire({
          title: "Error",
          html:
            "Lo sentimos, actualmente estamos presentando errores internos. Por favor intente nuevamente",
          icon: "error",
        });
      }
    );
  }

  exportToExcel() {
    this.spinnerCarga = 0;
    this.informeGenerado = true;


    let filterExport = this.filterData;
    filterExport.maxRows = 1000000;
    filterExport.pageNumber = 0;

    this.service.exportToExcel(filterExport).subscribe(
      (Response) => {
        if(Response !== ""){
          this.download(Response);
          this.spinnerCarga = 100;
        }else{
          this.informeGenerado = false;
          Swal.fire({
            title: "Error",
            html:
              "No hay registros para descargar.",
            icon: "error",
          });
        }
       
      },
      (error) => {
        this.openDialogLoading(false);
        Swal.fire({
          title: "Error",
          html:
            "Lo sentimos, actualmente estamos presentando errores internos. Por favor intente nuevamente",
          icon: "error",
        });
      }
    );
  }

  download(fileName:string) {
    this.fileName = fileName;
    this.spinnerCarga = 0 + (50 * 95) / 50;
    this.service.download(fileName).subscribe(
      (Response) => {
        this.base64Data = Response.fileContents;
      },
      (error) => {
        this.openDialogLoading(false);
        Swal.fire({
          title: "Error",
          html:
            "Lo sentimos, actualmente estamos presentando errores internos. Por favor intente nuevamente",
          icon: "error",
        });
      }
    );
    this.service.postDataTable(this.filterData).subscribe(Response => {  
      console.log('sent dowload');
      this.openDialogLoading(false);
      this.ELEMENT_DATA = Response.data; 
      this.llenarTablaAuditor();
      this.totalRegister = Response.noRegistrosTotalesFiltrado; 
      this.setPage(this.pageNumber);  
    },error => {
      this.openDialogLoading(false);
      Swal.fire({
        title: 'Error',
        html: 'Lo sentimos, actualmente estamos presentando errores internos. Por favor intente nuevamente',
        icon: 'error'
      })
    });
  }

  setPage(page: number) {
    this.pager = this.paginatorService.getPager(
      this.totalRegister,
      page,
      this.itemsPerpagina
    );
  }

  cleanFilters() {
    this.listDetails.reset();
    this.datos.fechaAsignacionIni = moment().utc().format("YYYY-MM-DD");
    this.datos.fechaAsignacionFin = moment().utc().format("YYYY-MM-DD"); 
    this.listEstados = ["1","2","3","4","5","6","7","8","9","10","11","12","13","14","15","16","17"];
    this.datos.estado = [];
    this.SetDefaultStatus();
    this.setDataIntables();
  }

  convertDate(dateTime: any) {
    var date = new Date(dateTime),
      mnth = ("0" + (date.getMonth() + 1)).slice(-2),
      day = ("0" + date.getDate()).slice(-2);
    return [date.getFullYear(), mnth, day].join("-");
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

  //Consulta perfil accion
  GetConsultaPerfilAccion()
  {
    this.service.GetConsultaPerfilAccion().subscribe(Response => {  
      this.perfilAccionList = Response;
    },error => {
      Swal.fire({
        title: 'Error',
        html: 'Lo sentimos, actualmente estamos presentando errores internos. Por favor intente nuevamente',
        icon: 'error'
      })
    });
  }
}
