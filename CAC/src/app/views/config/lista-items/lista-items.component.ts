import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { LoadingComponent } from 'src/app/layout/loading/loading.component';
import { CatalogoRequest } from 'src/app/model/catalogo/catalogorequest.model';
import { CatalogoResponse } from 'src/app/model/catalogo/catalogoresponse.model';
import { ItemFiltroRequest } from 'src/app/model/catalogo/itemFiltroRequest.model';
import { ItemReponse } from 'src/app/model/catalogo/itemResponse.model';
import { PagerService } from '../../cronograma/services/page.service';
import { ConfigService } from '../service/config.service';
import { CookieService } from 'ngx-cookie-service';
import * as moment from 'moment';
import Swal from 'sweetalert2';
import { GetPermisosServices } from 'src/app/services/get-permisos.services';

@Component({
  selector: 'app-lista-items',
  templateUrl: './lista-items.component.html',
  styleUrls: ['./lista-items.component.scss']
})
export class ListaItemsComponent implements OnInit {

  @Input() data !:CatalogoResponse; // decorate the property with @Input()
  @Output() volverCatalogo = new EventEmitter<any>();
  
  displayedColumns2: string[] = ['options','nid','nitem', 'creacion', 'modificacion'];

  dataSource2: ItemReponse[] = [];
  totalRegister: number = 0;

  pageNumber: any;
  intialPosition: any;
  itemsPerpagina: number = 50;
  sizeList: number[] = [50, 100, 150, 200];
  page: number = 1;
  pager: any = {};
  objFiltro !: ItemFiltroRequest ;
  form!: FormGroup;
  formItem!: FormGroup;
  objUser: any;
  activarItem : boolean = false; 
  estadoItem='nuevo';
  itemActivo: any;
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
    this.objFiltro= new ItemFiltroRequest;
    this.form = this.formBuilder.group({
      buscar:[''],
     });
     this.formItem = this.formBuilder.group({
      iditem:['',Validators.required],
      nombre:['', Validators.required]
     });
  
     if(this.data.id==0){

     }
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
    this.objFiltro= new ItemFiltroRequest;  
    this.objFiltro.catalogoCoberturaId= this.data.id.toString();
    this.objFiltro.maxRows = this.itemsPerpagina;
    this.objFiltro.pageNumber = this.page - 1;
//PENDIENTE CAMPO BUSCAR

    this.pageNumber = page;
    this.services.getFiltroItem(this.objFiltro).subscribe((Response) => {
      this.dataSource2 = Response.data as ItemReponse[];
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

    activarCrearItemAction(estado: string){
      this.estadoItem=estado;
      this.activarItem=true;
      
      this.formItem.controls.iditem.setValue('');
      this.formItem.controls.nombre.setValue(''); //TEST
      }

    activarItemAction(estado: string, obj: ItemReponse){
      this.estadoItem=estado;
      this.activarItem=true;
      this.itemActivo=obj;
      if(obj){
        this.formItem.controls.iditem.setValue(obj.itemId);
        this.formItem.controls.nombre.setValue(obj.itemDescripcion); //TEST
      }
    }


  
    cancelar(){
      this.estadoItem='';
      this.activarItem=false;
    }
    volver(){
      this.volverCatalogo.emit();
    }

    crearItemAction(){
      let obj : ItemReponse;
      obj={     
        noRegistrosTotales :  0,
    id: 0,
    catalogoCoberturaId: this.data.id,
    itemId: this.formItem.controls.iditem.value,
    itemDescripcion: this.formItem.controls.nombre.value,
    itemActivo: true,
    itemOrden: 1,
    itemGlosa: true,
    createdBy: this.objUser.userId,
    creationDate: moment.utc().format('YYYY-MM-DD'),
    modifyBy: this.objUser.userId,
    modificationDate:moment.utc().format('YYYY-MM-DD')};
    if(this.formItem.valid){
      this.services.postItemCobertura(obj).subscribe(response => {
        if(response){
          console.log("item creado");
          this.cancelar();
          this.getFiltros(1,50);
        }
      });
    }
     
    }
    editarItemAction(){
      let obj : any;
      obj={        
        id: this.itemActivo.id,
        catalogoCoberturaId: this.data.id,
        itemId: this.formItem.controls.iditem.value,
        itemDescripcion: this.formItem.controls.nombre.value,
        itemActivo: true,
        itemOrden: 1,
        itemGlosa: true,
        createdBy: this.objUser.userId,
        creationDate: moment.utc().format('YYYY-MM-DD'),
        modifyBy: this.objUser.userId,
        modificationDate:moment.utc().format('YYYY-MM-DD')};
      this.services.updateItemCobertura(this.itemActivo.id.toString(), obj).subscribe(response => {
        
          console.log("item editado");
          this.cancelar();
          this.getFiltros(1,50);
        
      });
    }

    eliminarItem(id:string){


      Swal.fire({
        title: "¿Está seguro que desea eliminar el item seleccionado?",
        text: 'La eliminación no es reversible.',
        icon: "warning",
        showCancelButton: true,
        cancelButtonText: "CANCELAR",
        cancelButtonColor: 'lightgray',
        confirmButtonColor: '#a94785',  
        confirmButtonText: `OK`,
      }).then((res) => {
        if (!res.isDismissed) { 
          this.services.deleteItemCobertura(id).
          subscribe(response => {
            
              console.log("Item Eliminado");
              this.cancelar();
              this.getFiltros(1,50);
            
          });
        }
      });

     
    }
}
