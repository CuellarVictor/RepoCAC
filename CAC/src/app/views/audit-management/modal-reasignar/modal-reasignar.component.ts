import { Component, ElementRef, Inject, ViewChild } from "@angular/core";
import { FormBuilder, FormGroup } from "@angular/forms";
import {
  MatDialog,
  MatDialogRef,
  MAT_DIALOG_DATA,
} from "@angular/material/dialog";
import { Router } from "@angular/router";
import { CookieService } from "ngx-cookie-service";
import { LoadingComponent } from "src/app/layout/loading/loading.component";
import { dataTableAuditorBolsas, listaAuditores, objpostReasignarBolsa } from "src/utils/models/asignacionbolsa/asignacionbolsamodels";
import Swal from "sweetalert2";
import { ManagementService } from "../services/management.service";
import { environment } from 'src/environments/environment';
import { messageString, ReporteMedicion } from "src/app/model/Util/enumerations.enum";
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
  selector: "app-modal-reasignar",
  templateUrl: "./modal-reasignar.component.html",
  styleUrls: ["./modal-reasignar.component.scss"],
})
export class ModalReasignarComponent {

  seleccion=0;
  errorAuditores=false;
  form!: FormGroup;
  selectlistaAuditores: listaAuditores[] = [];
  medicionSeleccionada : any;
  datos!:dataTableAuditorBolsas[];
  selectedFile: any = null;
  url:string = "";
  allowLoadFile:boolean = false;
  base64file: string = "";
  urlArchivoError: string = "";
  objUser;
  urlPlantilla: string = "";
  codAuditor='';
  objetoFiltro!:objpostReasignarBolsa;
  base64Plantilla: string = "";
  loading: boolean = false;
  _messageString: messageString = new messageString();

  constructor(
    public dialogRef: MatDialogRef<ModalReasignarComponent>,
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
    this.medicionSeleccionada= this.data.idMedicion;
    this.datos= this.data.datos;
    this.codAuditor=this.data.idAuditor;  
    this.objetoFiltro=this.data.obj;
    this.form = this.fb.group({
      auditor:['']    
     });
     const objpost={
      roleId: "3",
      userId: ""
      }
     this.services.GetUsuariosByRol(objpost).subscribe(Response => {
      this.selectlistaAuditores=Response as listaAuditores[];  
      this.selectlistaAuditores=this.selectlistaAuditores.filter(e=>e.id!=this.codAuditor);

    },
    (error) => {
      this.loading = false;
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

  btnEquitativa(){
    this.seleccion=1;
  }
  btnDetallado(){    
    this.seleccion=2;
  }

  btnEnviarDetallado(){
   this.loading = true;
    const obj= {user:this.objUser.userId ,
  idMedicion: [Number(this.medicionSeleccionada)],
              fileName: 'Plantilla Reasignacion Bolsa Detallada',
              fileBase64: this.base64file}

    this.services.ReasignacionesBolsaDetallada(obj).subscribe(Response => {
      if(Response.status){
        this.seleccion=4;
      }else{
        this.urlArchivoError=Response.file;
        this.seleccion=5;
      }
      this.loading = false;
    },
    (error) => {
      this.loading = false;
      this.OpenErrorodal();
    });   
  }

  btnEnviarEquitativa(){
    let radicado:number[]=[];
    this.datos.map(e => radicado.push(e.idRadicado));
    let auditores:string[]=[];
    auditores=this.form.controls.auditor.value==''?[]:this.form.controls.auditor.value
    const obj={auditoresId:auditores,idRadicado:radicado,idUsuario: this.objUser.userId}
    this.loading = true;
    if(radicado.length>=auditores.length && auditores.length>0){
      this.errorAuditores=false;
      this.services.ReasignacionesBolsaEquitativa(obj).subscribe(Response => {
        if(Response){
          this.seleccion=4;
        }
        this.loading = false;
      },
      (error) => {
        this.loading = false;
        this.OpenErrorodal();
      });      
    }else{
      this.loading = false;
      this.errorAuditores=true;
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
        
          // console.log(reader.result);
      };
  }

  downloadMyFile(url:string){
    const link = document.createElement('a');
    link.setAttribute('target', '_blank');
    link.setAttribute('href', url);
    link.setAttribute('download', `products.csv`);
    document.body.appendChild(link);
    link.click();
    link.remove();
}

@ViewChild('plantilla') plantilla !: ElementRef<HTMLElement>;
btnDescargarfiltrado(){
  this.loading = true;
  this.objetoFiltro.maxRows=9999999;
  this.services.DescargaReporteReasignar(this.objetoFiltro,ReporteMedicion.ReasignacionBolsaPlantilla).subscribe(Response => {
    this.base64Plantilla=Response;
    setTimeout(() => {
      let el: HTMLElement = this.plantilla.nativeElement;
     el.click();
     this.btnDetallado();
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
