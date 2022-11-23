import { Component, ElementRef, Inject, ViewChild } from "@angular/core";
import { FormBuilder, FormGroup } from "@angular/forms";
import {
  MatDialog,
  MatDialogRef,
  MAT_DIALOG_DATA,
} from "@angular/material/dialog";
import { CookieService } from "ngx-cookie-service";
import { LoadingComponent } from "src/app/layout/loading/loading.component";
import Swal from "sweetalert2";
import { ManagementService } from "../services/management.service";
import { environment } from 'src/environments/environment';
import { medicionxemadre } from "src/utils/models/manager/management";
import { messageString } from "src/app/model/Util/enumerations.enum";
export interface DialogData {
  content: string;
  name: string;
  type: string;
}

export interface dataTable {
  check: boolean;
  name: string;
  date: string;
}

@Component({
  selector: "app-modal-mover-bolsa",
  templateUrl: "./modal-mover-bolsa.component.html",
  styleUrls: ["./modal-mover-bolsa.component.scss"],
})
export class ModalMoverBolsaComponent {

  seleccion=0;
  form!: FormGroup;
  selectlistaMedicionxMadre :  medicionxemadre[]=[];
  selectedFile: any = null;
  url:string = "";
  allowLoadFile:boolean = false;
  allowBtnIntentar:boolean = false;
  todos=true;
  base64file: string = "";
  base64Respuesta: string = "";
  objUser;
  urlPlantilla: string = "";
  base64Plantilla: string = "";
  _messageString: messageString = new messageString();
  loading: boolean = false;
    
  constructor(
    public dialogRef: MatDialogRef<ModalMoverBolsaComponent>,
    private fb: FormBuilder,
    public dialog: MatDialog,   
    private services: ManagementService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.urlPlantilla=environment.UrlPlantillaReasignacion;
    const userEncoded = sessionStorage.getItem("objUser");
      this.objUser = JSON.parse(userEncoded || '');
  }
  
  ngOnInit(): void {
  
    this.form = this.fb.group({
      bolsa:['']    
     });
     const objpost={ idEnfermedadMadre: this.data.idEnfMadre.toString(), idMedicion: this.data.idMedicion.toString() }
   this.services.GetBolsasMedicionXEnfermedadMadre(objpost).subscribe(Response => {
     console.log(Response)
      this.selectlistaMedicionxMadre=Response as medicionxemadre[];  
    },
    (error) => {
      this.loading = false;
      this.OpenErrorodal();
    });
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

  btnTodos(){
    this.seleccion=1;
  }
  @ViewChild('subir') subir !: ElementRef<HTMLElement>;
  btnSubirArchivo(){   
    let el: HTMLElement = this.subir.nativeElement;
    el.click();
    this.seccionSubirArchivo();
  }
  @ViewChild('intentar') intentar !: ElementRef<HTMLElement>;
  DescargarIntentar(){   
    let el: HTMLElement = this.subir.nativeElement;
    el.click();
   this.allowBtnIntentar=true;
   this.allowLoadFile=false;
  }
  seccionSubirArchivo(){
    this.seleccion=2;
    this.allowBtnIntentar=false;

  }
  procesando=false;
  btnMoverRegistros(){
    const obj= {idUsuario: this.objUser.userId,
              fileName: 'Mover',
              fileBase64: this.base64file}
      this.procesando=true;
      this.loading = true;
    this.services.MoverAlgunosRegistrosAuditoriaBolsaMedicion(obj).subscribe(Response => {
      this.base64Respuesta=Response.file;
      if(Response.status){
        this.seleccion=4;
      }else{      
        this.seleccion=6;
      }
      this.procesando=false;

      this.OpenSuccesModal();
      this.loading = false;
    },
    (error) => {
      this.loading = false;
      this.OpenErrorodal();
    });   
  }

  btnEnviarTodos(){
    if(this.form.controls.bolsa.value!=''){
      const o = {
        medicionId: this.form.controls.bolsa.value.toString()
        };

        this.loading = true;
      this.services.GetValidacionEstadoBolsasMedicion(o).subscribe(Response => {
        const obj={
          medicionIdOriginal: this.data.idMedicion,
          idUsuario: this.objUser.userId,
          medicionIdDestino: Number(this.form.controls.bolsa.value)};
           this.services.MoverTodosRegistrosAuditoriaBolsaMedicion(obj).subscribe(Response => {
            
            this.base64Respuesta=Response.file;
            if(Response.status){
              this.seleccion=4;
            }else{            
              this.seleccion=5;
            } 
            this.OpenSuccesModal();
            this.loading = false;
          });
        },
        (error) => {
          this.loading = false;
          this.OpenErrorodal();
        });
      
    }        
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
        
        console.log(this.base64file);
      };
  }

  downloadMyFile(url:string){
    this.allowBtnIntentar = true;
    const link = document.createElement('a');
    link.setAttribute('target', '_blank');
    link.setAttribute('href', url);
    link.setAttribute('download', `products.csv`);
    document.body.appendChild(link);
    link.click();
    link.remove();
}

@ViewChild('plantilla') plantilla !: ElementRef<HTMLElement>;
btnDescargarPlantilla(){
  this.loading = true;
  this.todos=false;
  const obj={medicionIdOriginal: this.data.idMedicion,
             medicionIdDestino: 0}
  this.services.MoverAlgunosRegistrosAuditoriaBolsaMedicionPlantilla(obj).subscribe(Response => {
    
    this.base64Plantilla=Response.file;
    setTimeout(() => {
      let el: HTMLElement = this.plantilla.nativeElement;
     el.click();
          this.seccionSubirArchivo();          
      }, 500);  
      this.loading = false;    
  },
  (error) => {
    this.loading = false;
    this.OpenErrorodal();
  });
 
}

  OpenErrorodal()
  {
    Swal.fire({
      title: "Error",
      text: this._messageString.ErrorMessage,
      icon: "error",
    });
  }

  OpenSuccesModal()
  {
    Swal.fire({
      title: "Correcto",
      text: this._messageString.SuccessMessage,
      icon: "success",
    });
  }
  
}
