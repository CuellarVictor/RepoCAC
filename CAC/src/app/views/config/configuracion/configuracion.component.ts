import { SelectionModel } from '@angular/cdk/collections';
import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { MatCheckboxChange } from '@angular/material/checkbox';
import { Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { ItemConf } from 'src/app/model/catalogo/itemConf.model';
import { ConfigService } from '../service/config.service';
import * as moment from 'moment';
import Swal from 'sweetalert2';
import { objuser } from 'src/utils/models/cronograma/cronograma';
import { GetPermisosServices } from 'src/app/services/get-permisos.services';

export interface data {
  funcionalidad: string;
  lider: boolean;
  auditor: boolean;
  id:number
}  

export interface catalogo {
  nombre: string;
  creacion: string;
  modificacion: string;  
}  


const datos: data[] = [
  {id: 1, funcionalidad: 'Gestión de bolsas de medición carga de población y variables', lider: false, auditor: false},
  {id: 2, funcionalidad: 'Respuesta hallazgos masivos', lider: false, auditor: false},
  {id: 3, funcionalidad: 'Respuesta Glosas masivas', lider: false, auditor: false},
  {id: 4, funcionalidad: 'Asignación de cronograma', lider: false, auditor: false},
  {id: 5, funcionalidad: 'Reasignación de registros', lider: false, auditor: false},
  {id: 6, funcionalidad: 'Generación de informes', lider: false, auditor: false}

];

const datos2: catalogo[] = [
  { nombre: 'catalogo1', creacion: '01/01/2022', modificacion: '01/01/2022' },
  { nombre: 'catalogo1', creacion: '01/01/2022', modificacion: '01/01/2022' },
  { nombre: 'catalogo1', creacion: '01/01/2022', modificacion: '01/01/2022' },
  { nombre: 'catalogo1', creacion: '01/01/2022', modificacion: '01/01/2022' },
  { nombre: 'catalogo1', creacion: '01/01/2022', modificacion: '01/01/2022' }

];

@Component({
  selector: 'app-configuracion',
  templateUrl: './configuracion.component.html',
  styleUrls: ['./configuracion.component.scss']
})
export class ConfiguracionComponent implements OnInit {

  displayedColumns: string[] = ['funcionalidad', 'lider', 'auditor', 'especiales'];
  dataSource = [...datos];
  dataSource2 = [...datos2];
  displayedColumns2: string[] = ['ncatalogo', 'creacion', 'modificacion'];

  estadoCatalogo: string = "listaCatalogo";
  catalogoActivo : any ;
  listaActualizar: any[] = [];
  activarCrearItem='';
  idFocusItem = 0;
  nuevoitemName='';
  eliminandoItem=false;
  lstUsers:any;
  selection = new SelectionModel<any>(true, []);

  objCatalogIPS:any;
  objCatalogVariables:any;
  objCatalogCodigo: any;
  //user
  userEncoded: any;
  objUser: objuser;
  
  constructor(private service: ConfigService,
              private router: Router, 
              private coockie: CookieService,
              public permisos: GetPermisosServices) {
                this.userEncoded = sessionStorage.getItem("objUser");
                this.objUser = JSON.parse(this.userEncoded);
               }


  ngOnInit(): void {
  this.getUsers();
   this.getCatalogCalificacionIPS(3);
   this.getCatalogTipoCodigoAdmin(2);
   this.getCatalogVariables(4);  
  }


  getUsers(){
    this.service.getUsers().subscribe(Response => {
      this.lstUsers = Response;
    })    
  }

  click(element:any){
      console.log(element);      
  }

  getCatalogTipoCodigoAdmin(idCatalogo:any){
    this.service.getCatalog(idCatalogo).subscribe(Response => {      
    this.objCatalogCodigo = Response;
    })
  }


   getCatalogCalificacionIPS(idCatalogo:any){
    this.service.getCatalog(idCatalogo).subscribe(Response => {      
    this.objCatalogIPS = Response;
    })
  }

   getCatalogVariables(idCatalogo:any){
    this.service.getCatalog(idCatalogo).subscribe(Response => {
      this.objCatalogVariables = Response;
    })
  }

  goToEditCatalog(idCatalogo:number){
    this.router.navigateByUrl("/config/catalogo/" + idCatalogo);
  }

  llenarLista(element: any, event: any, itemName: string) {
    console.log(this.lstUsers);
    
    console.log(element);
    console.log(event);
    console.log(itemName);
    // switch (itemName) {
    //    case "especiales":
    //      if (this.listaActualizar.find((x) => x.variableId == element.variableId)) 
    //      {
    //        this.listaActualizar.filter((x) => x.variableId == element.variableId)[0].especiales = event.toString();
    //      }
    //      else 
    //      {
    //        element.especiales = event.toString(); //subGrupoNombre
    //        this.listaActualizar.push(element);
    //      }
    //      break;
    //  }
     console.log(this.listaActualizar);     
   }

   llenarListaChecks(element: any, event:MatCheckboxChange, itemName: string) {
     console.log(this.lstUsers);
     
    console.log(element);
    console.log(event);
    console.log(itemName);
    //  switch (itemName) {
    //    case "auditable":
    //      if (this.listaActualizar.find((x) => x.variableId == element.variableId)) 
    //      {
    //        this.listaActualizar.filter((x) => x.variableId == element.variableId)[0].calificable = event.checked;
    //      }
    //      else 
    //      {
    //        element.calificable = event.checked;
    //        this.listaActualizar.push(element);
    //      }
    //      break;
    //  }

    console.log(this.listaActualizar);     

   }

   editarCatalogo(obj: any){
     this.catalogoActivo=obj;
     this.estadoCatalogo= 'listaItem';
   }
   volverCatalogo(){
     this.estadoCatalogo='listaCatalogo';
   }

   saveItemAction(orden:string,value:string,categoria: string){
      let obj : ItemConf;
      obj={
          id: 0,
          itemName: value,
          catalogId: Number(categoria),
          concept:orden,
          createDate:  moment.utc().format('YYYY-MM-DD'),
          enable:true,
          status:true,
          lastModify : moment.utc().format('YYYY-MM-DD')};    
          Swal.fire({
            title: "¿Está seguro que desea crear el item '"+value+"'?",
           // text: 'La eliminación no es reversible.',
            icon: "info",
            showCancelButton: true,
            cancelButtonText: "CANCELAR",
            cancelButtonColor: 'lightgray',
            confirmButtonColor: '#a94785',  
            confirmButtonText: `OK`,
          }).then((res) => {
            if (!res.isDismissed) { 
              this.service.crearItem(obj,this.objUser.userId).subscribe(response => {
                if(response){
                  console.log("Item creado");
                  if(categoria=='3'){
                    this.getCatalogCalificacionIPS(3);
                  }else if(categoria=='2'){
                    this.getCatalogTipoCodigoAdmin(2);
                  }else{
                    this.getCatalogVariables(4);
                    this.nuevoValue = '';
                    this.nuevoOrden = '';
                  }
                  this.activarCrearItemAction('');
                //  this.cancelar();
                 // this.getFiltros(1,50);
                }
              });
            }else{
              if(categoria=='3'){
                this.getCatalogCalificacionIPS(3);
              }else if(categoria=='2'){
                this.getCatalogTipoCodigoAdmin(2);
              }else{
                this.getCatalogVariables(4);
              }
              this.activarCrearItemAction('');
            }

          });
        
          }
          editarItemAction(orden:string,entrada:string,obj: ItemConf,categoria: string){
              setTimeout(() => {
             
              if(obj.itemName!=entrada || obj.concept!=orden){
                
                Swal.fire({
                  title: '¿Está seguro que desea actualizar el item "'+obj.itemName+'"?"',
                  text: 'Sera actualizado al siguiente valor "'+entrada+'".',
                  icon: "warning",
                  showCancelButton: true,
                  cancelButtonText: "CANCELAR",
                  cancelButtonColor: 'lightgray',
                  confirmButtonColor: '#a94785',  
                  confirmButtonText: `OK`,
                }).then((res) => {
                  obj.lastModify =  moment.utc().format('YYYY-MM-DD');   
                  obj.itemName= entrada;
                  if(orden!=''){
                    obj.concept=orden;
                  }                 
                  if (!res.isDismissed) { 
                    this.service.actualizarItem(obj,this.objUser.userId).subscribe(response => {
                      if(response){
                        console.log("Item editado");
                        if(categoria=='3'){
                          this.getCatalogCalificacionIPS(3);
                        }else if(categoria=='2'){
                          this.getCatalogTipoCodigoAdmin(2);
                        }else{
                          this.getCatalogVariables(4);
                        }
                        this.activarCrearItemAction('');
                      //  this.cancelar();
                       // this.getFiltros(1,50);
                      }
                    });
                  }else{
                    if(categoria=='3'){
                      this.getCatalogCalificacionIPS(3);
                    }else if(categoria=='2'){
                      this.getCatalogTipoCodigoAdmin(2);
                    }else{
                      this.getCatalogVariables(4);
                    }
                    this.activarCrearItemAction('');
                  }
           
                });  
              }
              
            }, 50);
                      
                }

                eliminarItemAction(obj: ItemConf,categoria: string){
                    this.eliminandoItem= true;
                    console.log('eliminar', this.eliminandoItem)
                        Swal.fire({
                        title: '¿Está seguro que desea eliminar el item "'+obj.itemName+'"?"',
                        text: 'Sera eliminara permanentemente.',
                        icon: "error",
                        showCancelButton: true,
                        cancelButtonText: "CANCELAR",
                        cancelButtonColor: 'lightgray',
                        confirmButtonColor: '#a94785',  
                        confirmButtonText: `OK`,
                      }).then((res) => {
                        this.eliminandoItem= false;
                        if (!res.isDismissed) { 
                          this.service.eliminarItem(obj.id.toString(),this.objUser.userId).subscribe(response => {
                            
                              console.log("Item eliminado");
                              if(categoria=='3'){
                                this.getCatalogCalificacionIPS(3);
                              }else if(categoria=='2'){
                                this.getCatalogTipoCodigoAdmin(2);
                              }else{
                                this.getCatalogVariables(4);
                              }                            
                            //  this.cancelar();
                             // this.getFiltros(1,50);
                            
                          });
                        }
                      });              
                      }

      

     activarCrearItemAction(value : string){
       this.activarCrearItem = value;
     }
     nuevoValue = '';
     nuevoOrden = '';
     validarOrden(){
       if(Number(this.nuevoOrden)>100){
        this.nuevoOrden ='99';
       }else if(Number(this.nuevoOrden)<=0){
        this.nuevoOrden ='1';         
       }else{
         this.nuevoOrden=Number(this.nuevoOrden).toString();
       }
     }
}
