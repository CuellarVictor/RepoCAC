import { Component, ElementRef, OnInit, ViewChild } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { CookieService } from 'ngx-cookie-service';
import { LoadingComponent } from 'src/app/layout/loading/loading.component';
import { NewCodigo } from 'src/app/model/codigo/newCodigo.model';
import Swal from 'sweetalert2';
import { PagerService } from '../../cronograma/services/page.service';
import { ConfigService } from '../service/config.service';
import * as moment from 'moment';
import { CodigoLista } from 'src/app/model/codigo/codigoLista.model';
import { CodigoListaRequest } from 'src/app/model/codigo/codigoListaRequest.model';
import { enumCatalogo, messageString } from 'src/app/model/Util/enumerations.enum';
import { GetPermisosServices } from 'src/app/services/get-permisos.services';
import { AdministradorService } from '../../admin/administrador/services/administrador.service';

@Component({
  selector: 'app-codigos',
  templateUrl: './codigos.component.html',
  styleUrls: ['./codigos.component.scss']
})
export class CodigosComponent implements OnInit {

  basePlantilla = '';

  dataSource2!: any[];

  displayedColumns2: string[] = ['menu','codigo', 'nombre', 'tipo','cobertura'];

  _enumCatalogo : any = enumCatalogo;

  pageNumber: any;
  intialPosition: any;
  itemsPerpagina: number = 50;
  sizeList: number[] = [50, 100, 150, 200];
  page: number = 1;
  pager: any = {};
  totalRegister: number = 0;
  estadoCodigo='';
  objFiltro !: any ;
  objUser: any;
  form!: FormGroup;
  formCodigo!: FormGroup;
  crearCodigo = false;
  elementoCodigoActivo!: NewCodigo;
  carga = false;
  objTipos: any;
  listCobertura: any;
  // archivo
  selectedFile: any = null;
  url:string = "";
  allowLoadFile:boolean = false;
  base64file: string = "";
  urlArchivoError='';  
  _messageString: messageString = new messageString();  
  constructor( 
    private formBuilder: FormBuilder,
    public dialog: MatDialog, 
    private services: ConfigService,
    private paginatorService: PagerService,
    private coockie: CookieService,
    public permisos: GetPermisosServices,    
    private adminService: AdministradorService ) {
      this.objUser = JSON.parse(atob(this.coockie.get('objUser')));
     }

  ngOnInit(): void {
    this.form = this.formBuilder.group({
      buscarcodigo:[''],
      idCobertura:[''],
     });
     this.formCodigo = this.formBuilder.group({
      code: ['',Validators.required],
      name: ['',Validators.required],
      type: ['',Validators.required],
      cobertura: ['',Validators.required],
     });
     this.getTipo(this._enumCatalogo.Tipo);
     this.getEnfermedades();
     this.openDialogLoading(true);
     this.getFiltros(1,50);
  }

  getFiltros(page : any, itemsPerpagina : any) {
   
    // this.openDialogLoading(true);
    if(page && itemsPerpagina){      
      this.itemsPerpagina = itemsPerpagina;
      this.page = page;
    }
    this.objFiltro= new CodigoListaRequest;
    this.dataSource2 = [];
   
    this.objFiltro.id = ''; 
    this.objFiltro.tipo = ''; 
    this.objFiltro.nombre = '';
    this.objFiltro.codigo = '';
    this.objFiltro.palabraClave =  this.form.controls.buscarcodigo.value;
    this.objFiltro.idCobertura =  this.form.controls.idCobertura.value.toString();
    this.objFiltro.maxRows = this.itemsPerpagina; 
    this.objFiltro.pageNumber = this.page - 1; 
    this.pageNumber = page;
    this.services.getFiltroCodigo(this.objFiltro).subscribe((Response) => {
      this.dataSource2 = Response.data as CodigoLista[];
      this.totalRegister = Response.noRegistrosTotalesFiltrado;
      this.setPage(this.page);      
    
     // this.dataSource2 = new MatTableDataSource(this.dataTable);
      this.openDialogLoading(false);
    },error => {
      this.openDialogLoading(false);
    });
  }

limpiar(){
  this.form.controls.buscarcodigo.setValue('');
  this.form.controls.idCobertura.setValue('');
  this.getFiltros(1,50);
}
  crear(){
    this.crearCodigo=true;
    this.carga=false;
    this.estadoCodigo='nuevo';
  }

  cancelar(){
    this.formCodigo.controls.name.setValue('');
    this.formCodigo.controls.code.setValue('');
    this.formCodigo.controls.type.setValue('');
    this.crearCodigo=false;
    this.carga=false;
  }

  btnEditarCodigo(obj:any){
      this.elementoCodigoActivo= obj;
      this.crearCodigo=true;
      this.estadoCodigo='editar';
      this.formCodigo.controls.name.setValue(this.elementoCodigoActivo.nombre);
      this.formCodigo.controls.code.setValue(this.elementoCodigoActivo.codigo);
      this.formCodigo.controls.type.setValue(this.elementoCodigoActivo.tipo);
  }

  crearCodigoAction(){
    let obj : NewCodigo;
    obj={
      id: 0,
      nombre: this.formCodigo.controls.name.value,
      tipo: this.formCodigo.controls.type.value,
      codigo: this.formCodigo.controls.code.value, 
      idCobertura: Number(this.formCodigo.controls.cobertura.value),
      createdBy: this.objUser.userId,
      createdDate: moment.utc().format('YYYY-MM-DD'), 
      modifyBy:this.objUser.userId,
      modifyDate: moment.utc().format('YYYY-MM-DD'),
      status: true };
        if(this.formCodigo.valid){
          this.services.postCodigo(obj).subscribe(response => {
            if(response){
              Swal.fire({
                text:'Codigo '+obj.codigo+' creado correctamente',
                confirmButtonText: 'ACEPTAR'
              });
              console.log("Codigo creado");              
              this.form.controls.buscarcodigo.setValue(obj.codigo);
              this.cancelar();
              this.getFiltros(1,50);
            }
          });
        }
 
  }
  editarCodigoAction(){
    let obj : NewCodigo;
    obj={
      id: this.elementoCodigoActivo.id,
      nombre: this.formCodigo.controls.name.value,
      tipo: this.formCodigo.controls.type.value,
      codigo: this.formCodigo.controls.code.value, 
      idCobertura: Number(this.formCodigo.controls.cobertura.value),
      createdBy: this.elementoCodigoActivo.createdBy,
      createdDate: this.elementoCodigoActivo.createdDate,
      modifyBy: this.objUser.userId,
      modifyDate: moment.utc().format('YYYY-MM-DD'),
      status: true };
    if(this.formCodigo.valid){
      this.services.updateCodigo(this.elementoCodigoActivo.id.toString(), obj).
      subscribe(response => {
        Swal.fire({
          text:'Codigo '+obj.codigo+' editado correctamente',
          confirmButtonText: 'ACEPTAR'
        });
          console.log("Codigo Editado");
          this.form.controls.buscarcodigo.setValue(obj.codigo);
          this.cancelar();
          this.getFiltros(1,50);
        
      });
    }
 
  }

  eliminarCodigo(id:string){


    Swal.fire({
      title: "¿Está seguro que desea eliminar el codigo seleccionado?",
      text: 'La eliminación no es reversible.',
      icon: "warning",
      showCancelButton: true,
      cancelButtonText: "CANCELAR",
      cancelButtonColor: 'lightgray',
      confirmButtonColor: '#a94785',  
      confirmButtonText: `OK`,
    }).then((res) => {
      if (!res.isDismissed) { 
        this.services.deleteCodigo(id).
        subscribe(response => {
          Swal.fire({
            text:'Codigo Eliminado',
            confirmButtonText: 'ACEPTAR'
          });
            console.log("Codigo Eliminado");
            this.cancelar();
            this.getFiltros(1,50);
          
        });
      }
    });

   
  }

  getEnfermedades(){ 
  this.adminService.getEnfermedades().subscribe(
       (Response) => {      
        this.listCobertura = Response; 
        this.openDialogLoading(false); 
      },
      (error) => { 
        this.openMessage('error', this._messageString.ErrorMessage, 'Error');
        this.openDialogLoading(false); 
      }
    );
  } 

  openMessage( type: any, message: string, title: string)
  {
    Swal.fire({
      title: title,
      text: message,
      icon: type
    }) 
  }



  @ViewChild('plantillaCodigo') total !: ElementRef<HTMLElement>;
  descargarPlantilla(){    
   
    this.services.descargarPlantillaBancoCodigo().
    subscribe(response => {
      if(response.status){
        this.basePlantilla = response.file;
        setTimeout(() => {
          let el: HTMLElement = this.total.nativeElement;
          el.click();;
          }, 500);   
      }
        console.log(response);
      
    });
  }


  fileSelected(event: any)
  {
    this.selectedFile = <File>event.target.files[0];

    var reader = new FileReader();
    reader.onload = (event:any) =>
    {
      this.url = event.target.result;
    }
    reader.readAsDataURL(this.selectedFile);

    var splitname = this.selectedFile.name.split('.');
    var extesion = splitname[splitname.length - 1];

    if(extesion == "csv")
    {
      this.allowLoadFile = true;
    }
    else{
      Swal.fire({
        text:'El archivo de cargue debe tener extension .csv',
        confirmButtonColor: '#a94785',
        confirmButtonText: 'ACEPTAR'
      })
      this.allowLoadFile = false;
    }
   
    this.handleUpload(event)
  }
    //Archivo a Base 64  
    handleUpload(event: any) {
      const file = event.target.files[0];
      const reader = new FileReader();
      reader.readAsDataURL(file);
      reader.onload = () => {
        // console.log(Buffer.from(reader.result).toString('base64'));
        //   this.base64file = reader.result?.toString();
        //   console.log(atob(reader.result)); 
        var a = reader.result == null ? "" : reader.result.toString();
        this.base64file = a;
        this.base64file= this.base64file.replace('data:application/vnd.ms-excel;base64,77u/','');
        
          // console.log(reader.result);
      };
  }

  btnEnviarMasivo(){
    this.openDialogLoading(true);
    
     const obj= {userId:this.objUser.userId ,
                fileName: "BancoInformacion",
                fileBase64: this.base64file}
 
     this.services.cargarArchivoBancoCodigo(obj).subscribe(Response => {
       if(Response.status){
        Swal.fire({
          text:'Archivo procesado correctamente',
          confirmButtonText: 'ACEPTAR'
        });
        this.urlArchivoError=Response.file;
          
       }else{
        Swal.fire({
          text:'El archivo no fue procesado',
          confirmButtonColor: '#a94785',
          confirmButtonText: 'ACEPTAR'
        });
        this.urlArchivoError=Response.file;
       }
       let redirectWindow = window.open(Response.file, '_blank');  
       this.openDialogLoading(false);
       this.cancelar();
     },
     (error) => {
      this.openDialogLoading(false);
      this.cancelar();
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

    cargamasiva(){
      this.crearCodigo= false;
      this.carga= true;
    }
  

    getTipo(idCatalogo:any){
      this.services.getCatalog(idCatalogo).subscribe(Response => {      
      this.objTipos = Response;
      })
    }


}
