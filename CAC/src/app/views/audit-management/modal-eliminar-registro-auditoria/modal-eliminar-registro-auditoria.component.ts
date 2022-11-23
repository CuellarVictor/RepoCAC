import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MatDialog, MAT_DIALOG_DATA } from '@angular/material/dialog';
import Swal from 'sweetalert2';
import { ManagementService } from '../services/management.service';
import { ProcessService } from '../services/process.service';

@Component({
  selector: 'app-modal-eliminar-registro-auditoria',
  templateUrl: './modal-eliminar-registro-auditoria.component.html',
  styleUrls: ['./modal-eliminar-registro-auditoria.component.scss']
})
export class ModalEliminarRegistroAuditoriaComponent implements OnInit {
  allowLoadFile = false;
  formObservation!: FormGroup;
  url:string = "";
  base64file: string = "";
  selectedFile: any = null;
  loading: boolean = false;
  loadingProgress: boolean = false;
  loadsTemplate: string = '';

  urlResponse= "";
  //user
  userEncoded: any;
  objUser: any;

  Idmedicion:number = 0;
  nombreMedicion: string = "";

  estado=1;

  constructor(private services: ManagementService,
    public dialog: MatDialog,
    public processService: ProcessService, 
    private formBuilder: FormBuilder,   
    @Inject(MAT_DIALOG_DATA) public data:any
    ) {

      this.userEncoded = sessionStorage.getItem("objUser");
      this.objUser = JSON.parse(this.userEncoded);
      
     }

  ngOnInit(): void {


    this.formObservation = this.formBuilder.group({
      observacion: ['',Validators.required],
    })

    this.Idmedicion = this.data.idMedicion;
    this.nombreMedicion = this.data.nombreMedicion;

  }

  generarTemplate()
  {    
    if(this.loadsTemplate != "")
    {
      this.downloadBase64File(this.loadsTemplate);
    }
    else
    {
      this.loading = true;
      this.services.generarTemplateEliminarRAuditoria().subscribe(data => {
        if(data.status){
          this.loadsTemplate = data.file;      
          this.downloadBase64File(this.loadsTemplate);
        }else{
          const swalCargar = Swal.mixin({
            customClass: {
              confirmButton: 'boton-swal-confirm btn-basic  btn-general ancho',
             },
            buttonsStyling: false
          });
          swalCargar.fire({
            text:'Archivo no generado',
            confirmButtonColor: '#a94785',
            confirmButtonText: 'ACEPTAR'
          })
        }
      
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
    
  }

  downloadBase64File(base64String:string)
  {
    const source = `data:application/pdf;base64,${base64String}`;
    const link = document.createElement("a");
    link.href = source;
    link.download = `plantilla_eliminar_registros.csv`
    link.click();
  }

  ConfirmacionCargueEliminacion()
  {
    const swalCargar = Swal.mixin({
      customClass: {
        confirmButton: 'boton-swal-confirm btn-basic  btn-general',
        cancelButton: 'btn-abrir btn-general subrayado fondo_subrayado'      
      },
      buttonsStyling: false
    });

    swalCargar.fire({
      html:'¿Está seguro de cargar el archivo <span style="color: #A03781">' + this.selectedFile.name + '</span> a la medición <span style="color: #A03781">' + this.nombreMedicion + '</span> que eliminara registros de auditoria?',
      confirmButtonColor: '#a94785',
      confirmButtonText: 'CONTINUAR',
      showCancelButton: true,
      cancelButtonText:'CANCELAR',    
      
    }).then((result) => {
      if(result.isConfirmed){
       this.CargarArchivoEliminacion();
      }
      
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

  
  CargarArchivoEliminacion()
  {    

    let objCarga= {
      fileName: this.selectedFile.name,
      fileBase64: this.base64file,
      medicionId: this.Idmedicion.toString(),
      observacion: this.formObservation.controls.observacion.value,
      idUsuario: this.objUser.userId
    }
    const swalCargar = Swal.mixin({
      customClass: {
        confirmButton: 'boton-swal-confirm btn-basic  btn-general ancho',
       },
      buttonsStyling: false
    });

    this.loading = true;
    this.services.PostEliminarRegistrosAuditoria(objCarga).subscribe(data => { 
       
      
      let msn= '';
      if(data.status){
        msn= 'Archivo procesado correctamente.'
        this.estado=2;
      }else{
        msn= 'Error cargando el archivo, por favor intente nuevamente'
        this.estado=3;
      }
      this.urlResponse=data.file;
      swalCargar.fire({
        text:msn,
        confirmButtonColor: '#a94785',
        confirmButtonText: 'ACEPTAR'
      })

        this.loading = false;
        },
        error => {
         
          swalCargar.fire({
            text:'Error cargando el archivo, por favor intente nuevamente',
            confirmButtonColor: '#a94785',
            confirmButtonText: 'ACEPTAR'
          })
          this.loading = false;
        });
      }


      downloadMyFile(){
        const link = document.createElement('a');
        link.setAttribute('target', '_blank');
        link.setAttribute('href', this.urlResponse);
        link.setAttribute('download', `Resultado.csv`);
        document.body.appendChild(link);
        link.click();
        link.remove();
    }

    reintentar(){
      this.formObservation.controls.observacion.setValue('');
      this.estado=1;
      this.allowLoadFile = false;
      this.url= "";
      this.base64file = "";
      this.selectedFile = null;
      this.loadsTemplate = '';
    
      this.urlResponse= "";
    }
}
