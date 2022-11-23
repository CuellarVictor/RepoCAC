import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { LoadingComponent } from 'src/app/layout/loading/loading.component';
import { CatalogoRequest } from 'src/app/model/catalogo/catalogorequest.model';
import { CatalogoResponse } from 'src/app/model/catalogo/catalogoresponse.model';
import { PagerService } from '../../cronograma/services/page.service';
import { ConfigService } from '../service/config.service';
import { Output, EventEmitter } from '@angular/core';
import { NewCatalogoRequest } from 'src/app/model/catalogo/newCatalogoRequest.model';
import { CookieService } from 'ngx-cookie-service';

import * as moment from 'moment';
import Swal from 'sweetalert2';
import { GetPermisosServices } from 'src/app/services/get-permisos.services';
@Component({
  selector: 'app-lista-catalogo',
  templateUrl: './lista-catalogo.component.html',
  styleUrls: ['./lista-catalogo.component.scss']
})
export class ListaCatalogoComponent implements OnInit {


  @Output() editarCatalogo = new EventEmitter<any>();
  displayedColumns2: string[] = ['ncatalogo', 'creacion', 'modificacion'];

  crearCatalogo: boolean = false;
  estadoCatalogo='nuevo';
  formCatalogo!: FormGroup;
  elementoCatalogoActivo: any;
  dataSource2: CatalogoResponse[] = [];
  totalRegister: number = 0;

  pageNumber: any;
  intialPosition: any;
  itemsPerpagina: number = 50;
  sizeList: number[] = [50, 100, 150, 200];
  page: number = 1;
  pager: any = {};
  objFiltro !: CatalogoRequest ;
  form!: FormGroup;
  objUser: any;
  constructor( 
    private formBuilder: FormBuilder,
    public dialog: MatDialog, 
    private services: ConfigService,
    private paginatorService: PagerService,
    private coockie: CookieService,
    public permisos: GetPermisosServices) {
      this.objUser = JSON.parse(atob(this.coockie.get('objUser')));
     }

  ngOnInit(): void {
    console.log(this.objUser)
    this.objFiltro= new CatalogoRequest;
    this.form = this.formBuilder.group({
      buscar:[''],
     });
     this.formCatalogo = this.formBuilder.group({
      id:[''],
      nombre:['', Validators.required]
     });
     this.openDialogLoading(true);
     this.getFiltros(1,50)
  }

  getFiltros(page : any, itemsPerpagina : any) {
    // this.openDialogLoading(true);
    if(page && itemsPerpagina){      
      this.itemsPerpagina = itemsPerpagina;
      this.page = page;
    }
    
    this.dataSource2 = [];

    this.objFiltro.id = '';
    this.objFiltro.activo = 'true';
    this.objFiltro.nombreCatalogo = this.form.controls.buscar.value;
    this.objFiltro.maxRows = this.itemsPerpagina;
    this.objFiltro.pageNumber = this.page - 1;
    this.pageNumber = page;
    this.services.getFiltroCatalogo(this.objFiltro).subscribe((Response) => {
      this.dataSource2 = Response.data as CatalogoResponse[];
      this.totalRegister = Response.noRegistrosTotalesFiltrado;
      this.setPage(this.page);      
    
     // this.dataSource2 = new MatTableDataSource(this.dataTable);
      this.openDialogLoading(false);
    },error => {
      this.openDialogLoading(false);
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
      this.crearCatalogo=true;
      this.estadoCatalogo='nuevo';
    }

    cancelar(){
      this.formCatalogo.controls.nombre.setValue('');
      this.crearCatalogo=false;
    }

    btnEditarCatalogo(obj:any){
        this.elementoCatalogoActivo= obj;
        this.crearCatalogo=true;
        this.estadoCatalogo='editar';
        this.formCatalogo.controls.nombre.setValue(this.elementoCatalogoActivo.nombreCatalogo);
    }

    crearCatalogoAction(){
      let obj : NewCatalogoRequest;
      obj={
          id: 0,
          nombreCatalogo:  this.formCatalogo.controls.nombre.value,
          activo: true,
          createdBy: this.objUser.userId,
          creationDate: moment.utc().format('YYYY-MM-DD'),
          modifyBy:this.objUser.userId,
          modificationDate: moment.utc().format('YYYY-MM-DD')};
          if(this.formCatalogo.valid){
            this.services.postCatalogoCobertura(obj).subscribe(response => {
              if(response){
                console.log("Catalogo creado");
                this.cancelar();
                this.getFiltros(1,50);
              }
            });
          }
   
    }
    editarCatalogoAction(){
      console.log(this.elementoCatalogoActivo)
      let obj : NewCatalogoRequest;
      obj={
          id: this.elementoCatalogoActivo.id,
          nombreCatalogo:  this.formCatalogo.controls.nombre.value,
          activo: true,
          createdBy: this.elementoCatalogoActivo.createdBy,
          creationDate: this.elementoCatalogoActivo.creationDate,
          modifyBy: this.objUser.userId,
          modificationDate:  moment.utc().format('YYYY-MM-DD')};
      if(this.formCatalogo.valid){
        this.services.updateCatalogoCobertura(this.elementoCatalogoActivo.id, obj).
        subscribe(response => {
          
            console.log("Catalogo Editado");
            this.cancelar();
            this.getFiltros(1,50);
          
        });
      }
   
    }

    eliminarCatalogo(id:string){


      Swal.fire({
        title: "¿Está seguro que desea eliminar el catalogo seleccionado?",
        text: 'La eliminación no es reversible.',
        icon: "warning",
        showCancelButton: true,
        cancelButtonText: "CANCELAR",
        cancelButtonColor: 'lightgray',
        confirmButtonColor: '#a94785',  
        confirmButtonText: `OK`,
      }).then((res) => {
        if (!res.isDismissed) { 
          this.services.deleteCatalogoCobertura(id).
          subscribe(response => {
            
              console.log("Catalogo Eliminado");
              this.cancelar();
              this.getFiltros(1,50);
            
          });
        }
      });

     
    }

}
