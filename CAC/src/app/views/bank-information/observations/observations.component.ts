import { Component, Inject, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import * as moment from 'moment';
import { CookieService } from 'ngx-cookie-service';
import { RegistroAuditoriaSeguimientoModel } from 'src/app/model/registroauditoria/registroauditoriaseguimiento.model';
import { messageString } from 'src/app/model/Util/enumerations.enum';
import { CronogramaService } from '../../cronograma/services/cronograma.service';
import Swal from 'sweetalert2';
import { Router } from '@angular/router';

interface Food {
  value: string;
  viewValue: string;
}

interface Car {
  value: string;
  viewValue: string;
}

@Component({
  selector: 'app-observations',
  templateUrl: './observations.component.html',
  styleUrls: ['./observations.component.scss']
})
export class ObservationsComponent implements OnInit {

  formObservation!: FormGroup;
  objUser:any;

  active:any;
  tipoObservacion:number = 0;
  selectedOption:number = 0;

  error = false;
  mensajePlaceholder = 'Escriba aquí su observación';
  prueba = 1;
  bandera : Number = 0;  

  option: any[] = [];
  tracingSelected: RegistroAuditoriaSeguimientoModel = new RegistroAuditoriaSeguimientoModel();
  _messageString: messageString = new messageString();
  
  constructor(@Inject(MAT_DIALOG_DATA) public data:any,
                                      public dialogRef: MatDialogRef<ObservationsComponent>,
                                      private coockie: CookieService,
                                      private router: Router,
                                      private cronogramaService:CronogramaService,
                                      private formBuilder: FormBuilder) {
                                        this.objUser = JSON.parse(atob(this.coockie.get('objUser')));                                       
                                      }


  ngOnInit(): void {
      this.bandera= this.data.bandera;
         

      this.formObservation = this.formBuilder.group({
        registroAuditoriaId:[this.data.idRegistroAuditoria],
        tipoObservacion: [this.data.tipificacionObservacionDefault==0?1:this.data.tipificacionObservacionDefault],// porfavor validar esta funcionalidad
        observacion: ['',Validators.required],
      })
      console.log('tipo observacion',this.data.tipificacionObservacionDefault)
      this.getOptions(); 
      this.getObservacionTemporal();
  } 


  getOptions(){
    console.log(this.data.tipificacionObservacionDefault)
    const catalogId = 1
    this.cronogramaService.getOptionsObservation(catalogId).subscribe(Response => {  
      this.option = Response
      this.formObservation.controls.tipoObservacion.setValue(this.option.find(e=> e.id == this.data.tipificacionObservacionDefault).id)
    });    
  }


  // this.service.postGuardarObservacion(this.formObservation.getRawValue()).subscribe(Response => {

  //submit action
  save()
  {
    if(this.data.tipificacionObservacionDefault == 133){
      this.reversar();
    }else{
      this.tracingSelected.registroAuditoriaId = this.data.idRegistroAuditoria;
      this.tracingSelected.TipoObservacion =  this.formObservation.controls["tipoObservacion"].value;
      this.tracingSelected.Observacion =  this.formObservation.controls["observacion"].value;
      this.tracingSelected.EstadoActual = this.data.estado;
      this.tracingSelected.EstadoNuevo = this.data.estado;
      this.tracingSelected.CreatedBy = this.data.usuario;
      this.tracingSelected.ModifyBy = this.data.usuario;
      this.tracingSelected.CreatedDate = new Date();
      this.tracingSelected.ModifyDate = new Date();
  
      //Request for save data
      this.cronogramaService.postGuardarObservacion(this.tracingSelected).subscribe(data => {
        this.openModalSuccess();
        this.dialogRef.close();
      }, error => {
        this.openModalError();    
      })
    }
   
  }


  //Validation for allow save
  validationSave()
  {
    if(
      this.formObservation.controls["observacion"].value == "" || this.formObservation.controls["observacion"].value == null ||
      this.formObservation.controls["tipoObservacion"].value == "" || this.formObservation.controls["tipoObservacion"].value == null ||
      this.formObservation.controls["tipoObservacion"].value == 0 
    )
    {
      return false;
    }
    else
    {
      return true;
    }
    
  }


  //Show Modal success
  openModalSuccess()
  {
    Swal.fire({
      title: 'Correcto',
      text: this._messageString.SuccessMessage,
      icon: 'success'
    }) 
  }

  //Show Modal error
  openModalError()
  {
    Swal.fire({
      title: 'Error',
      text: this._messageString.ErrorMessage,
      icon: 'error'
    }) 
  }


  reversar(){
    {
      let obj = {
        idRegistrosAuditoria: this.data.idRegistroAuditoria,
        estado: "",
        observacion:  this.formObservation.controls["observacion"].value,
        idUsuario: this.objUser.userId
      }
      this.cronogramaService.postReversarAuditoriaObservacion(obj).subscribe(data => {
        this.router.navigate(['/cronograma']) ;  
        this.openModalSuccess();
      }, error => {
        this.openModalError();    
      });
      
    }
  }


  getObservacionTemporal(){
  
  this.cronogramaService.getObservacionTemporal(this.data.idRegistroAuditoria).subscribe(data => {
    if(data.valor){
      this.formObservation.controls["observacion"].setValue(data.valor);
    }
  }, error => {
    this.openModalError();    
  });
  
  }

  setObservacionTemporal(){
  
    let obj= {
      id: this.data.idRegistroAuditoria,
      valor:  this.formObservation.controls["observacion"].value
    };
    
    this.cronogramaService.setObservacionTemporal(obj).subscribe(data => {
    }, error => {
      
    });
    
    }

}
