import { Component, OnInit } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { RegistroAuditoriaDetalleErrorModel } from 'src/app/model/registroauditoria/registorauditoriadetalleerror.model';
import { RegistroAuditoriaSeguimientoModel } from 'src/app/model/registroauditoria/registroauditoriaseguimiento.model';
import { enumRoles, enumTipoObservacion, messageString } from 'src/app/model/Util/enumerations.enum';
import { detalleRegistro } from 'src/utils/models/cronograma/information';
import { CronogramaService } from '../../cronograma/services/cronograma.service';
import Swal from 'sweetalert2';
import { ResponseValidacionEstadoModel } from 'src/app/model/registroauditoria/responsevalidacionstado.model';

@Component({
  selector: 'app-report-error',
  templateUrl: './report-error.component.html',
  styleUrls: ['./report-error.component.scss']
})
export class ReportErrorComponent implements OnInit {
  masivo= false;
  indeterminate= false;
  enviado=false;
  mensaje='';
  labelbtn='';
  rolOpuesto='';
  rol='';
  objAuditar!:detalleRegistro;
  objUser: any;
  errorsList: RegistroAuditoriaDetalleErrorModel[] = [];
  tracingSelected: RegistroAuditoriaSeguimientoModel = new RegistroAuditoriaSeguimientoModel();
  observacion:string = "";
  _messageString: messageString = new messageString();
  rolAuditor: number = enumRoles.Auditor;
  validationObj: ResponseValidacionEstadoModel = new ResponseValidacionEstadoModel();
  
  constructor(private cronogramaService: CronogramaService,
    private coockie: CookieService) 
  {
    this.objAuditar = JSON.parse(atob(this.coockie.get('objAuditar')));
    this.objUser = JSON.parse(atob(this.coockie.get('objUser')));
    console.log(this.objUser  )
    this.getValidations();
   }

  ngOnInit(): void {
    this.consultarErroresRegistrosAuditoria();
    this.rol = sessionStorage.getItem('rol') != null ? sessionStorage.getItem('rol') as string:'';
   
    if( Number(this.objUser.rol.userRolId) == Number(this.rolAuditor)){
      this.mensaje='Los errores han sido reportados al líder, guarde el registro para finalizar';
      this.labelbtn='Reportar';
      this.rolOpuesto='líder';
    }else{
      this.mensaje='Los errores no corregibles han sido reportados, guarde el registro para finalizar';
      this.labelbtn='Enviar';
      this.rolOpuesto='auditor';
    }
  }

  reportar(){
    this.enviado=true;
  }

  //Get Errors
  consultarErroresRegistrosAuditoria()
  {
    this.cronogramaService.ConsultarErroresRegistrosAuditoria(+this.objAuditar.id).subscribe(data => {
      this.errorsList = data;
    this.verificarCheckMasivo();
    }, error => {

    })
  }

  //Get Errors
  upsertErroresRegistrosAuditoria(value:any, error: RegistroAuditoriaDetalleErrorModel)
  {
    this.indeterminate=true;
    this.cronogramaService.UpsertErroresRegistrosAuditoria(error).subscribe(data => {
        this.verificarCheckMasivo();
    }, error => {

    })
  }

  UpsertErroresRegistrosAuditoriaMasivo(){

    this.errorsList.map(e=> e.noCorregible=this.masivo);

    this.cronogramaService.UpsertErroresRegistrosAuditoriaMasivo(this.objUser.userId ,this.errorsList).subscribe(data => {
     this.verificarCheckMasivo();
    }, error => {

    })
  }


  save()
  {
    this.enviado=true;
    this.tracingSelected.registroAuditoriaId = +this.objAuditar.id;
    this.tracingSelected.TipoObservacion =  enumTipoObservacion.Errorlogica;
    this.tracingSelected.Observacion =  this.observacion;
    this.tracingSelected.EstadoActual = +this.objAuditar.estado;
    this.tracingSelected.EstadoNuevo = +this.objAuditar.estado;
    this.tracingSelected.CreatedBy = this.objUser.userId;
    this.tracingSelected.ModifyBy = this.objUser.userId;
    this.tracingSelected.CreatedDate = new Date();
    this.tracingSelected.ModifyDate = new Date();

    //Request for save data
    this.cronogramaService.postGuardarObservacion(this.tracingSelected).subscribe(data => {
      this.openMessage(this._messageString.SuccessMessage, 'success');
    }, error => {
    })
  }


  //Open message
  openMessage(message: string, type: any)
  {
    Swal.fire({
      title: '',
      text: message,
      icon: type
    }) 
  }

  getValidations()
  {
    this.cronogramaService.GetValidacionesRegistroAuditoriaDetalle(+this.objAuditar.id, this.objUser.userId, 0).subscribe(data => {
      this.validationObj = data;
    }, error => {
    })
  }

  verificarCheckMasivo() {
    const cantidad=this.errorsList.filter(e => e.noCorregible==true).length;
    if(cantidad === this.errorsList.length){
      this.masivo=true;
      this.indeterminate=false;
    }else if(cantidad==0){
      this.masivo=false;
      this.indeterminate=false;
    }else{
      this.masivo=true;
      this.indeterminate=true;
    }
  }


}
