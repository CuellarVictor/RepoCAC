

  import { Component, OnInit } from '@angular/core';
  import { FormBuilder, FormGroup, Validators } from '@angular/forms';
  import { MatDialog } from '@angular/material/dialog';
  import { LoadingComponent } from 'src/app/layout/loading/loading.component';
  import { PagerService } from '../../cronograma/services/page.service';
  import { ConfigService } from '../service/config.service';
  import { Output, EventEmitter } from '@angular/core';
  import { NewCatalogoRequest } from 'src/app/model/catalogo/newCatalogoRequest.model';
  import { CookieService } from 'ngx-cookie-service';
  
  import * as moment from 'moment';
  import Swal from 'sweetalert2';
  import { GetPermisosServices } from 'src/app/services/get-permisos.services';
import { LiderEntidadRequest } from 'src/app/model/lider_entidad/lider_entidad_request.model';
import { LiderEntidadResponse } from 'src/app/model/lider_entidad/lider_entidad_response.model';
import { AdministradorService } from '../../admin/administrador/services/administrador.service';
import { messageString } from 'src/app/model/Util/enumerations.enum';
import { LiderEntidadListComponent } from './lider-entidad-list/lider-entidad-list.component';

  @Component({
    selector: 'app-lider-entidad',
    templateUrl: './lider-entidad.component.html',
    styleUrls: ['./lider-entidad.component.scss']
  })
  export class LiderEntidadComponent implements OnInit {
  
  
    @Output() editarCatalogo = new EventEmitter<any>();
    displayedColumns2: string[] = ['ideps', 'nombre_eps', 'id_cobertura','id_periodo', 'usuario_Auditor', 'ver'];
  
    asignarLider: boolean = false;
    estadoCatalogo='nuevo';
    formAsignacionLider!: FormGroup;
    elementoEpsActivo!: LiderEntidadResponse;


    dataSource2: LiderEntidadResponse[] = [];
    totalRegister: number = 0;
  
    pageNumber: any;
    intialPosition: any;
    itemsPerpagina: number = 10;
    sizeList: number[] = [10, 50, 100, 150];
    page: number = 1;
    pager: any = {};
    objFiltro !: LiderEntidadRequest ;
    form!: FormGroup;
    objUser: any;

    listAuditores: any;
    listPeriodo: any[] = [];

    _messageString: messageString = new messageString();
    constructor( 
      private formBuilder: FormBuilder,
      public dialog: MatDialog, 
      private services: ConfigService,
      private paginatorService: PagerService,
      private coockie: CookieService,
      public permisos: GetPermisosServices,
      private adminService: AdministradorService) {
        this.objUser = JSON.parse(atob(this.coockie.get('objUser')));
       }
  
    ngOnInit(): void {
      console.log(this.objUser)
      this.objFiltro= new LiderEntidadRequest;
      this.form = this.formBuilder.group({
        periodo:[''],
        cobertura:[''],
       });
       this.formAsignacionLider = this.formBuilder.group({
        eps:['', Validators.required],
        cobertura:['', Validators.required],
        periodo:['', Validators.required],
        auditorLider:['', Validators.required]
       });
       this.openDialogLoading(true);
       this.getFiltros(1,10)
       this.getEnfermedades();
    }
  

    changeCobertura()
    {
      this.form.controls.periodo.setValue('');
      this.getFiltros(1,10);
    }

    changePeriodo()
    {
      this.getFiltros(1,10);
    }

    getFiltros(page : any, itemsPerpagina : any) {

      this.getPeriodos();

      // this.openDialogLoading(true);
      if(page && itemsPerpagina){      
        this.itemsPerpagina = itemsPerpagina;
        this.page = page;
      }
      
      this.dataSource2 = [];
  
      this.objFiltro.idCobertura = this.form.controls.cobertura.value.toString();
      this.objFiltro.idPeriodo =  this.form.controls.periodo.value.toString();
      //this.objFiltro.nombreCatalogo = this.form.controls.buscar.value;
      this.objFiltro.maxRows = this.itemsPerpagina;
      this.objFiltro.pageNumber = this.page - 1;
      this.pageNumber = page;
      this.services.consultaAsignacionLiderEntidad(this.objFiltro).subscribe((Response) => {
        this.dataSource2 = Response.data as LiderEntidadResponse[];
        this.totalRegister = Response.noRegistrosTotalesFiltrado;
        this.setPage(this.page);
      
       // this.dataSource2 = new MatTableDataSource(this.dataTable);
        this.openDialogLoading(false);
      },error => {
        this.openDialogLoading(false);
      });
    }

    getAuditoresEps(){
        let obj={
          idCobertura: this.elementoEpsActivo.idCobertura.toString() ,
          idPeriodo: this.elementoEpsActivo.idPeriodo.toString(),
          idEPS: this.elementoEpsActivo.data_IdEPS.toString()}
          this.openDialogLoading(true);
        this.services.consultaAuditoresAsignacionLiderEntidad(obj).subscribe((Response) => {
          this.listAuditores = Response;
          console.log(this.listAuditores);

       // this.dataSource2 = new MatTableDataSource(this.dataTable);
        this.openDialogLoading(false);
      },error => {
        this.openDialogLoading(false);
      });
    }
    listCobertura: any = [];
    getEnfermedades(){ 
      this.adminService.getEnfermedades().subscribe(
           (Response) => {      
            this.listCobertura = Response; 
            this.openDialogLoading(false); 
          },
          (error) => { 
            this.openDialogLoading(false); 
          }
        );
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
  
      //seteo el paginador
      setPage(page: number) {
        this.pager = this.paginatorService.getPager(
          this.totalRegister,
          page,
          this.itemsPerpagina
        );
      }
    
  
      verItemsAction(obj :any) {
        this.editarCatalogo.emit(obj);
      }
  
      crear(){
        this.asignarLider=true;
        this.estadoCatalogo='nuevo';
      }
  
      cancelar(){
        this.formAsignacionLider.controls.eps.setValue('');
        this.formAsignacionLider.controls.cobertura.setValue('');
        this.formAsignacionLider.controls.periodo.setValue('');
        this.formAsignacionLider.controls.auditorLider.setValue('');
        this.asignarLider=false;
      }
  
      btnEditarEps(obj:any){
          this.elementoEpsActivo= obj;
          this.asignarLider=true;
          this.estadoCatalogo='editar';
          this.formAsignacionLider.controls.eps.setValue(this.elementoEpsActivo.data_IdEPS);
          this.formAsignacionLider.controls.cobertura.setValue(this.elementoEpsActivo.idCobertura);
          this.formAsignacionLider.controls.periodo.setValue(this.elementoEpsActivo.idPeriodo);
          this.getAuditoresEps();
        }

      listAsig: any = [];
      getAuditores(){ //Revisar 
        this.adminService.getEnfermedades().subscribe(
             (Response) => {      
              this.listCobertura = Response; 
              this.openDialogLoading(false); 
            },
            (error) => { 
              this.openDialogLoading(false); 
            }
          );
        } 
  
      editarEpsAction(){
        this.openDialogLoading(false);
       
        if(this.formAsignacionLider.valid){
          let obj : any;
          obj={          
              idEPS: this.elementoEpsActivo.data_IdEPS.toString(),
              idAuditorLider: this.formAsignacionLider.controls.auditorLider.value.toString(),
              idCobertura: Number(this.elementoEpsActivo.idCobertura),
              idPeriodo: Number(this.elementoEpsActivo.idPeriodo),
              Usuario: this.objUser.userId
              }
          this.services.setLiderAuditorEps(obj).
          subscribe(response => {
              this.openDialogLoading(false);
              this.openMessage('success', this._messageString.SuccessMessage, 'Correcto');
              this.cancelar();
              this.getFiltros(1,10);
            
          },error => {
            this.openDialogLoading(false);
            this.openMessage('error', this._messageString.ErrorMessage, 'Error');
          });
        }
     
      }



      //Open message
      openMessage( type: any, message: string, title: string)
      {
        Swal.fire({
          title: title,
          text: message,
          icon: type
        }) 
      }



      getPeriodos(){
        
        let idCobertura: any;
        idCobertura = this.form.controls.cobertura.value.toString();
        
        if(idCobertura != null && idCobertura != "")
        {
          let c: number = +idCobertura;
          this.openDialogLoading(true);

          this.services.consultaPeriodosCobertura(c).subscribe((Response) => {
          this.listPeriodo = Response;  
          this.openDialogLoading(false);
        },error => {
          this.openDialogLoading(false);
        });
        }

        
        
    }



    openModalAuditores(idCobertura:string, idPeriodo: string, idEPS: string)
    { 
      
        let obj={
          idCobertura: idCobertura.toString() ,
          idPeriodo: idPeriodo.toString(),
          idEPS: idEPS.toString()
        }

        const dialogRef = this.dialog.open(LiderEntidadListComponent, {
          data: obj,
          width: '1000px',
        }).afterClosed().subscribe((res) => {      
    
        }); 
        ;
    }
  
  
  
  }
  