import { Component, Inject, OnInit } from '@angular/core';
import { MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { CargueCalificacionMasiva } from 'src/app/model/proceso/cargueCalificacionMasiva.model';
import { CurrentProcessModel } from 'src/app/model/proceso/currentprocess.model';
import { enumProcesos, enumValidationProcess } from 'src/app/model/Util/enumerations.enum';
import { objuser } from 'src/utils/models/cronograma/cronograma';
import Swal from 'sweetalert2';
import { ManagementService } from '../services/management.service';
import { ProcessService } from '../services/process.service';

@Component({
  selector: 'app-modal-calificacion-masiva',
  templateUrl: './modal-calificacion-masiva.component.html',
  styleUrls: ['./modal-calificacion-masiva.component.scss']
})
export class ModalCalificacionMasivaComponent implements OnInit {


  userEncoded: any;
  objUser: objuser;

  loading : boolean = false;
  base64file: string = "";
  loadsTemplate: string = "";

  url:string = "";
  selectedFile: any = null;
  allowLoadFile:boolean = false;

  Idmedicion:number = 0;
  nombreMedicion: string = "";

  current: number = 0;
  max: number = 0;
  cargueObj :CargueCalificacionMasiva  = new CargueCalificacionMasiva();
  currentProcess: CurrentProcessModel = new CurrentProcessModel();
  fileprocessResult:string = "OK";
  filtetoDownload:string = "";

  enumProcesos = enumProcesos;  
  _enumValidationProcess: any = enumValidationProcess;

  constructor(private services: ManagementService,
    public dialog: MatDialog,
    public processService: ProcessService,    
    @Inject(MAT_DIALOG_DATA) public data:any
    ) {

      this.userEncoded = sessionStorage.getItem("objUser");
      this.objUser = JSON.parse(this.userEncoded);
     }

  ngOnInit(): void {
    this.Idmedicion = this.data.idMedicion;
    this.nombreMedicion = this.data.nombreMedicion;
    this.validationCurrentProcess();  
  }

   //Validar proceso actual
   validationCurrentProcess()
   {    
     this.loading = true;
     this.processService.validationCurrentProcess(this.enumProcesos.CalificacionMasiva, "Medicion" + this.Idmedicion).subscribe(data => {
           this.currentProcess = data;  
           if(this.currentProcess.id == 0)
           {
             this.loading = false;
           }
           else
           {
              this.asignarCurrentValue();
              this.GetProgress();
           }
             
         },
         error => {
           console.log(error);
           this.loading = false;
         });
   }
 
  
  generarTemplateCarguePoblacion()
  {    
  
      this.loading = true;
      this.services.generarTemplateCalificacionMasiva(this.Idmedicion).subscribe(data => {
        this.loadsTemplate = data;      
        this.downloadBase64File(this.loadsTemplate);
        this.loading = false;
        },
        error => {
          const swalCargar = Swal.mixin({
            customClass: {
              confirmButton: 'boton-swal-confirm btn-basic  btn-general ancho',
             },
            buttonsStyling: false
          });
          swalCargar.fire({
            text:'Error cargando el archivo, por favor intente nuevamente',
            confirmButtonColor: '#a94785',
            confirmButtonText: 'ACEPTAR'
          })
          this.loading = false;
        });
    
    
  }


  downloadBase64File(base64String:string)
  {
    const source = `data:application/pdf;base64,${base64String}`;
    const link = document.createElement("a");
    link.href = source;
    link.download = `plantilla_calificacion_masiva.csv`
    link.click();
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
    console.log(this.selectedFile);

    var splitname = this.selectedFile.name.split('.');
    var extesion = splitname[splitname.length - 1];

    if(extesion == "csv" || extesion == "txt")
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
      console.log(reader);
      reader.onload = () => {
        // console.log(Buffer.from(reader.result).toString('base64'));
        //   this.base64file = reader.result?.toString();
        //   console.log(atob(reader.result)); 
        var a = reader.result == null ? "" : reader.result.toString();
        this.base64file = a;
        console.log(this.base64file);
          // console.log(reader.result);
      };
  }
  

  ConfirmacionCargue()
  {
    const swalCargar = Swal.mixin({
      customClass: {
        confirmButton: 'boton-swal-confirm btn-basic  btn-general',
        cancelButton: 'btn-abrir btn-general subrayado fondo_subrayado'      
      },
      buttonsStyling: false
    });

    swalCargar.fire({
      html:'¿Está seguro de cargar el archivo <span style="color: #A03781">' + this.selectedFile.name + '</span> a la medición <span style="color: #A03781">' + this.nombreMedicion + '</span>?',
      confirmButtonColor: '#a94785',
      confirmButtonText: 'CONTINUAR',
      showCancelButton: true,
      cancelButtonText:'CANCELAR',    
      
    }).then((result) => {
      if(result.isConfirmed){
        this.CargarCalificacionMasiva();
      }
      
    });
  }
  
  CargarCalificacionMasiva()
  {    
    this.current = 0;
    this.max = 0;
    this.cargueObj.fileName = this.selectedFile.name;
    this.cargueObj.usuario = this.objUser.userId;
    this.cargueObj.medicion = "Medicion" + this.Idmedicion;
    this.cargueObj.fileBase64 = this.base64file;
    this.loading = true;
    this.services.CargarArchivoCalificacionMasiva(this.cargueObj).subscribe(data => { 
        this.currentProcess = data;      
        this.GetProgress();
        },
        error => {
          const swalCargar = Swal.mixin({
            customClass: {
              confirmButton: 'boton-swal-confirm btn-basic  btn-general ancho',
             },
            buttonsStyling: false
          });
          swalCargar.fire({
            text:'Error cargando el archivo, por favor intente nuevamente',
            confirmButtonColor: '#a94785',
            confirmButtonText: 'ACEPTAR'
          })
          this.loading = false;
        });
  }

  GetProgress()
   {
     
      const sleep = (milliseconds:any) => {
      return new Promise(resolve => setTimeout(resolve, milliseconds))
    }
    
    sleep(5000).then(() => {
      this.processService.validationCurrentProcess(this.enumProcesos.CalificacionMasiva, "Medicion" + this.Idmedicion).subscribe(
        data => {
              this.currentProcess = data;  
              this.asignarCurrentValue();             
              this.loading = true;

              if(data.progress == this._enumValidationProcess.Initializing)
              {
                this.GetProgress();
              }

              else if(data.progress == this._enumValidationProcess.Finish)
              {    
                this.loading = false;
                this.allowLoadFile = false;
                this.asignarCurrentValue();
              }

              else if(data.progress == this._enumValidationProcess.Error)
              {
                Swal.fire({
                  text:'Error procesando el archivo, por favor intente nuevamente',                  
                  confirmButtonColor: '#a94785',
                  confirmButtonText: 'ACEPTAR'
                });
                this.deleteCurrentProcess();
              }

              else if(data.progress >= this._enumValidationProcess.InProgress)
              {
                this.loading = true;
                this.GetProgress();
              }
        },
        error => {
          this.GetProgress();
        }
      )
      
    });

   }

       //Elimina proceso actual
  deleteCurrentProcess()
  {    
    this.filtetoDownload = '';
    this.loading = true;
    this.processService.deleteCurrentProcess(this.enumProcesos.CalificacionMasiva, "Medicion" + this.Idmedicion).subscribe(data => {
          this.currentProcess = data;  
          this.loading = false;            
        },
        error => {
          console.log(error);
          this.loading = false;
        });
  }

   asignarCurrentValue()
   {
     let result = this.currentProcess.result.split(",");
     this.current = +result[1];
     this.max = +result[2];
 
     if(result.length > 3)
     {
       this.fileprocessResult = result[3];
       this.filtetoDownload = result[4];
     }
   }

   downloadFile(){  
    let redirectWindow = window.open(this.filtetoDownload, '_blank');    
    }

}
