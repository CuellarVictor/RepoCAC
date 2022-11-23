//angular
import { Component, OnInit } from '@angular/core';
import {MatDialog} from '@angular/material/dialog';
import { ObservationsComponent } from '../observations/observations.component';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { DialogComponent } from '../dialog/dialog.component';
import { CookieService } from 'ngx-cookie-service';
import Swal from 'sweetalert2';
import { Router } from '@angular/router';

import * as moment from 'moment';

//Components
import { LoadingComponent } from 'src/app/layout/loading/loading.component';

//Models
import { ResponseRegistroAuditoriaDetalleModel } from 'src/app/model/registroauditoria/responseregistroauditoriadetalle';
import { detalleRegistro, Registro } from 'src/utils/models/cronograma/information';
import { ResponseValidacionEstadoModel } from 'src/app/model/registroauditoria/responsevalidacionstado.model';
import { enumCatalog, messageString, enumEstadoRegistroAuditoria, enumActionRegistroAuditoriaDetalle, enumRoles, enumTipoVariable, enumCalificacion, enumEstadosMedicion } from 'src/app/model/Util/enumerations.enum';

//Services
import { VariableDetailsService } from '../../audit-management/variable-details/services/variable-details.service';
import { CronogramaService } from '../../cronograma/services/cronograma.service';
import { CatalogoCoberturaRequestModel } from 'src/app/model/registroauditoria/catalogocoberturarequest.model';
import { Observable } from 'rxjs';
import { VariableCondicional } from 'src/app/model/Variables/variablecondicionada.model';
import { GetPermisosServices } from 'src/app/services/get-permisos.services';
import { HallazgoModel } from 'src/app/model/hallazgos/hallazgos.model';
import { HallazgoComponent } from '../hallazgo/hallazgo.component';
import { ContextoBotComponent } from '../contexto-bot/contexto-bot.component';
import { DialogCalculadoraComponent } from '../dialog_calculadora/dialog_calculadora.component';
import { TablaReferenciaRegistroAuditoria } from 'src/app/model/registroauditoria/tablareferencialRegistroAuditoria.model';
import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';


export interface requestLista{
  itemDescripcion: string,
  itemId:  string,
  nombreCatalogo:  string,
}


@Component({
  selector: 'app-information',
  templateUrl: './information.component.html',
  styleUrls: ['./information.component.scss'],
  animations: [
    trigger('detailExpand', [
      state('collapsed', style({height: '0px', minHeight: '0'})),
      state('expanded', style({height: '*'})),
      transition('expanded <=> collapsed', animate('225ms cubic-bezier(0.4, 0.0, 0.2, 1)')),
    ]),
  ],
})
export class InformationComponent implements OnInit {

  //Variables
  objUser:any;
  loading: boolean = true;
  display_btn_footer : boolean = false;
  action: number = 0;

  //Objects
  objAuditar!:detalleRegistro;
  registerSelected: any;
  hallazgos: HallazgoModel[] = [];
  columnsToDisplay = ['name','DatoReportado', 'BOT','CT','H','E', 'C', 'select','Motivo'];
  validationObj: ResponseValidacionEstadoModel = new ResponseValidacionEstadoModel();
  itemsCalifications: any;
  _messageString: messageString = new messageString();
  objObservacion:any;
  expandirCategoria: string[] = new Array();
  validationError: any;
  rolAuidtor: number = enumRoles.Auditor;
  _enumEstadoRegistroAuditoria = enumEstadoRegistroAuditoria;
  _enumTipoVariable = enumTipoVariable;
  _enumCalificacion: any = enumCalificacion;
  _enumEstadosMedicion = enumEstadosMedicion;
  estado: number = 0;
  variableCondicionadaList: VariableCondicional[] = [];
  mensajeVariableCondicional: string = "";
  variableSelected: any;
  variableHija: any;
  dataTablaReferencial: TablaReferenciaRegistroAuditoria[] = [];
 

  listados: string [] = [];
  constructor(public dialog: MatDialog,
              private cronogramaService: CronogramaService,
              private router: Router,
              private serviceVariableDetails: VariableDetailsService,
              private coockie: CookieService,             
              public permisos: GetPermisosServices) {
                this.objAuditar = JSON.parse(atob(this.coockie.get('objAuditar')));
                this.objUser = JSON.parse(atob(this.coockie.get('objUser')));
              }
            

  ngOnInit(): void {  
    this.consultaDataTablasReferencialRegistroAuditoria();
    this.getItemsByCatalogId();
    this.openDialogLoading(true); //Init loading on
    this.getAuditRegisterDetail(); //Get audit register detail
    this.getValidations();

    this.estado = +this.objAuditar.estado; 
 

  }



  //Open loading modal
  openDialogLoading(loading: boolean): void {
    this.loading = loading;
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


  //Request for get audit register detail
  getAuditRegisterDetail()
  {
    this.cronogramaService.GetAuditoriaDetalle(this.objAuditar.id).subscribe(data => {
      var listado = JSON.stringify(data)      
      localStorage.setItem("objListVariables",listado);
      this.registerSelected = data;
      this.ConsultarHallazgos();
      this.openDialogLoading(false); //loading off
      this.asignarDataTablaReferencial();

    }, error => {
      this.openDialogLoading(false); //loading off
      this.openModalError();  
    })
  }

  //Request for get data reference's table
  consultaDataTablasReferencialRegistroAuditoria()
  {
    this.cronogramaService.consultaDataTablasReferencialRegistroAuditoria(this.objAuditar.id).subscribe(data => {
      this.dataTablaReferencial = data;

    }, error => {
      console.log(error);
    })
  }

  asignarDataTablaReferencial()
  {
    console.log('assing' + (new Date));
      const sleep = (milliseconds:any) => {
        return new Promise(resolve => setTimeout(resolve, milliseconds))
      }
    
    sleep(5000).then(() => {

      if(this.dataTablaReferencial.length == 0 || this.registerSelected.length == undefined ||  this.registerSelected.length == 0 )
      {
        console.log('attemp');
         this.asignarDataTablaReferencial();
      }
      else
      {
        console.log('loaded');
  
        this.registerSelected.forEach((gropuSelected: any) => { //Go througth  groups

          gropuSelected.variables.forEach((element:any) => { //Go througth  variables
            
            if(
               element.tablaReferencial != '' &&
               element.tablaReferencial != null &&  
               this.dataTablaReferencial.filter(tr => tr.itemId == element.dato_reportado && tr.nombreCatalogo == element.tablaReferencial).length > 0)
            {
              console.log("search: " + element.detalle);  
              element.valorDatoReportado =  this.dataTablaReferencial.filter(tr => tr.itemId == element.dato_reportado && tr.nombreCatalogo == element.tablaReferencial)[0].itemDescripcion;
              console.log("find: " + element.valorDatoReportado);
            }   
          });
          
        });  
        
        console.log('finish assing'  + (new Date));
      }
      
    });

  }

  //Request for get audit register detail
  getValidations()
  {
    this.cronogramaService.GetValidacionesRegistroAuditoriaDetalle(+this.objAuditar.id, this.objUser.userId, this.action).subscribe(data => {
      this.validationObj = data;
      if(+this.objAuditar.estado == this.validationObj.idRegistroNuevo)
      {
        this.objAuditar.estado = enumEstadoRegistroAuditoria.Registropendiente.toString()  
        this.ActualizaEstadoRegistroAuditoria(false);
        this.objAuditar.estadoCodigo = this.validationObj.codigoRegistroPendiente;
        this.objAuditar.estado = enumEstadoRegistroAuditoria.Registropendiente.toString();
        var encoded = btoa(JSON.stringify(this.objAuditar));
        this.coockie.set("objAuditar", encoded);
      }
    }, error => {
      this.openDialogLoading(false)
      this.openModalError();       
    })
  }

  //Request Items
  getItemsByCatalogId() {
    this.serviceVariableDetails.getItemsByCatalogId(enumCatalog.CalificacionDefecto).subscribe((data) => {
      this.itemsCalifications = data;
    },
    (error) => {
      this.openModalError();
    });
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

  //Update row
  updateAuditRegisterDetail(value:any)
  {


    if(value.alerta && value.condicionada)
    {
      this.openMessageAlert(value);
    }
    //Mensaje Alerta
    else if(value.alerta)
    {
      this.openMessage(value.alertaDescripcion, 'warning', '');
    }
    //Variable Condicionada
    else if(value.condicionada)
    {
      this.variableSelected = value;
      this.VariableCondicionada(value);
    }



    //Validation for clean reason
    if
     (
       (+value.dato_DC_NC_ND != this.validationObj.idItemNC && value.tipoVariableId != this._enumTipoVariable.Adicional)
       ||
       (+value.dato_DC_NC_ND != this.validationObj.idItemDC &&  +value.dato_DC_NC_ND != this.validationObj.idItemNC  && value.tipoVariableId == this._enumTipoVariable.Adicional) //ToDo: Confirmar
     )
      {
        value.motivo = "";
      }

    //Validation for save changes inemdiatly
    if(this.validationObj.guardarCadaCambioVariable)
    {      
      if(value.idTipoVariable==='datetime' && value.motivo != null){
        let division= value.motivo.split('-', 3);
        let mes31=[1,3,5,7,8,9,11];
        if(division.length==3){
          division[1]=Number(division[1])>12?'12':division[1];
          division[1]=Number(division[1])==0?'01':division[1];
          if(mes31.find(e => e == division[1])){
            division[2]=Number(division[2])>31?'31':division[2];
          }else{
            division[2]=Number(division[2])>30?'30':division[2];
          }
          division[2]=Number(division[2])==0?'01':division[2];
          division[2]=Number(division[1])==2 && Number(division[2])>28?'28':division[2];
          value.motivo=division[0]+'-'+division[1]+'-'+division[2];
        
          moment.utc().format('YYYY-MM-DD')
          let f1= new Date(value.motivo).getTime();
          // let f2= new Date(  moment.utc().format('YYYY-MM-DD')).getTime(); // Cambiar fecha maxima
          let f2= new Date(this.objAuditar.fechaCorteAuditoria).getTime(); // Cambiar fecha maxima

          if (f1 > f2){
            value.motivo='';
        }
        }else{
          value.motivo='';
        }
        
      }else if((value.idTipoVariable==='int' || value.idTipoVariable==='intEDT' ) && value.motivo == null){
        value.motivo=Number(value.motivo).toString();
        if(value.motivo=='0'){
          value.motivo=''
        }
      }
      this.cronogramaService.updateSatusRadioButton(+this.objAuditar.id, this.objUser.userId, value.motivo == null ? "" : value.motivo.toString(), value.dato_DC_NC_ND, value.registroAuditoriaDetalleId, this.action).subscribe(data => {
        this.validationObj = data; 
        this.listados= this.listados.filter(e => e!= value.variableId);     
      }, error => {
        this.openDialogLoading(false)
        this.openModalError();       
      })
    }
    

  }

  openMessageAlert(value:any)
  {
    Swal.fire({
      text: value.alertaDescripcion,
      icon: 'warning',
      confirmButtonColor: '#a94785',
      confirmButtonText: 'OK',
      showCancelButton: false
    }).then((result) => {

      if (value.condicionada) {
        this.variableSelected = value;
      this.VariableCondicionada(value);
      }
    });
  }

  VariableCondicionada(value:any)
  {
    this.mensajeVariableCondicional = "";
    this.serviceVariableDetails.consultaVariablesCondicionadas(+value.variableId, value.medicionId).subscribe(
      (Response) => {         
        this.variableCondicionadaList = Response;   
        this.MensajeVariableCondicionada();
          
      },
      (error) => {
        console.log(error);
      }
    );
  }

  MensajeVariableCondicionada()
  {
    this.mensajeVariableCondicional = this._messageString.ApplyConditionalVariable;

    let variablesCondicionadas: string = "";

    this.variableCondicionadaList.forEach(element => {
        variablesCondicionadas += element.nombre + ", "
    });

    this.mensajeVariableCondicional = this.mensajeVariableCondicional.replace("{{varPadre}}", this.variableSelected.detalle);
    this.mensajeVariableCondicional = this.mensajeVariableCondicional.replace("{{varHija}}", variablesCondicionadas);
    this.mensajeVariableCondicional = this.mensajeVariableCondicional.replace("{{valorCondicionado}}",this.variableSelected.valorConstante);

    Swal.fire({
      html: this.mensajeVariableCondicional,
      confirmButtonColor: '#a94785',
      confirmButtonText: 'Si',
      showCancelButton: true,
      cancelButtonText:'No',
    }).then((result) => {          
      if(result.isConfirmed)
      {
          this.ApplicarCambiosVariableCondicional();
      }      
    })
 
  }

  ApplicarCambiosVariableCondicional()
  {

    let listaValorConstante = this.variableSelected.valorConstante.split(' | ');

    this.openDialogLoading(true);
   
    this.variableCondicionadaList.forEach((element, index) => {


      this.registerSelected.forEach((detail :any) => {

        if(detail.variables.filter((x : any)  => x.variableId == element.variableHijaId).length > 0)
        {
          this.variableHija = detail.variables.filter((x : any)  => x.variableId == element.variableHijaId)[0];
        }
      });
      if(listaValorConstante[index].toString()==this.variableHija.dato_reportado.toString()){
        this.variableHija.dato_DC_NC_ND = this._enumCalificacion.DC;
      }else{
        this.variableHija.motivo= listaValorConstante[index];    
        this.variableHija.dato_DC_NC_ND = this._enumCalificacion.NC;
      }
     
      this.updateAuditRegisterDetail(this.variableHija);

    });

    this.openDialogLoading(false);
  }

 options!: any[] ;
  filteredOptions!: Observable<string[]>;


  private _filter(value: string): string[] {
    const filterValue = value.toLowerCase();

    return this.options.filter(option => option.toLowerCase().includes(filterValue));
  }

  getListaReferencial(e:any){
      let d = new CatalogoCoberturaRequestModel;
      d.tablaReferencial= e.tablaReferencial;
      d.valorBusqueda= e.motivo?e.motivo:'';
      this.options=[];
      this.cronogramaService.getAutocompleteCatalogoCobertura(d).subscribe(request => {
        this.options= request;
        this.options = this.options.filter(element => element.itemId != e.dato_reportado);
       
      });
  }

  //Show observation modal
  openModalReversar()
  {
    this.display_btn_footer=true; 
    this.objObservacion = {
      tipificacionObservacionDefault:133,
      tipificacionObservasionHabilitada: this.validationObj.tipificacionObservasionHabilitada,
      idRegistroAuditoria: +this.objAuditar.id,
      estado: this.objAuditar.estado,
      usuario: this.objUser.userId

    }

    const dialogRef = this.dialog.open(ObservationsComponent, {
      width: '550px',
      height: '305px',  
      panelClass: 'my-custom-dialog-class',
      data: this.objObservacion,
      disableClose: false
    });
      dialogRef.afterClosed().subscribe(result => {
        this.getValidations();
        this.display_btn_footer=false; 
      });
  }

  openModalObservation()
  {
    this.display_btn_footer=true; 
    this.objObservacion = {
      tipificacionObservacionDefault: this.validationObj.tipificacionObservacionDefault,
      tipificacionObservasionHabilitada: this.validationObj.tipificacionObservasionHabilitada,
      idRegistroAuditoria: +this.objAuditar.id,
      estado: this.objAuditar.estado,
      usuario: this.objUser.userId

    }

    const dialogRef = this.dialog.open(ObservationsComponent, {
      width: '550px',
      height: '305px', 
      panelClass: 'my-custom-dialog-class',
      data: this.objObservacion,
      disableClose: false
    });
      dialogRef.afterClosed().subscribe(result => {
        this.getValidations();
        this.display_btn_footer=false; 
      });
  }

  

  //Open message
  openMessage(message: string, type: any, title: string)
  {
    Swal.fire({
      title: title,
      text: message,
      icon: type
    }) 
  }
  

  //Submit audit
  save()
  {
    this.openDialogLoading(true);

    //Get validations
    this.cronogramaService.GetValidacionesRegistroAuditoriaDetalle(+this.objAuditar.id, this.objUser.userId, this.action).subscribe(async data => {
      this.validationObj = data;


      //Validation when dont saving changes inmediatly
      if(!this.validationObj.guardarCadaCambioVariable) 
      {        
        this.validationObj.solicitarMotivo = false;

        this.registerSelected.forEach((gropuSelected: any) => { //Go througth  groups

          gropuSelected.variables.forEach((element:any) => { //Go througth  variables

            if(element.dato_DC_NC_ND == this.validationObj.idItemNC && (element.motivo == "" || element.motivo == null))
            {
              this.validationObj.solicitarMotivo = true;
            }   
          });
          
        });       
         
      }


      var errosValdiationResult =  await this.validarErrores();

      //Validation for allow save
      if(this.validationObj.observacionObligatoria && !this.validationObj.observacionRegistrada)
      {
        let observatioMessage = this._messageString.NecesaryObservationMessage;

        if(+this.objAuditar.estado == enumEstadoRegistroAuditoria.Errorlogicamarcacionauditor && this.validationObj.habilitarVariablesCalificar)
        {
          observatioMessage = this._messageString.NecesaryObservationErrorLiderMessage;
        }
        this.openMessage(observatioMessage, 'warning', '');
        this.openDialogLoading(false);
      }
      //Validation for allow save
      else if(errosValdiationResult)
      {
        let observatioMessage = this._messageString.NecesaryObservationErrorMessage;
        let title = this._messageString.NecesaryObservationErrorTitle;
        
        if(+this.objAuditar.estado == enumEstadoRegistroAuditoria.Errorlogicamarcacionlíder)
        {
          observatioMessage = this._messageString.ErroresCorregiblesMessage;
          title = this._messageString.ErroresCorregiblesTitle;
        }
        this.openMessage(observatioMessage, 'warning', title);
        this.openDialogLoading(false);
      }
      //Validation for allow save
      else if(this.validationObj.calificacionObligatoriaIPS && !this.validationObj.calificacionIPSRegistrada)
      {
        this.openMessage(this._messageString.NecesaryIPSCalificationMessage, 'warning', '');
        this.openDialogLoading(false);
      }
      //Validation for allow save
      else if(this.validationObj.solicitarMotivo)
      {
        this.openMessage(this._messageString.MotiveEmpty, 'warning', '');
        this.openDialogLoading(false);
      }
      else
      {
        this.ActualizaEstadoRegistroAuditoria(true);
        this.openDialogLoading(false);
      }      

    }, error => {
      this.openDialogLoading(false)
      this.openModalError();       
    })
    
  }

  //Request update audit register
  ActualizaEstadoRegistroAuditoria(showMessage: boolean)
  {
    this.validationObj.habilitadoBotonGuardar = false;
    if(!this.validationObj.guardarCadaCambioVariable) //Validation for update massive
    {
      
      this.cronogramaService.ActualizarRegistroAuditoriaDetalleMultiple(+this.objAuditar.id, this.objUser.userId, this.action, this.registerSelected).subscribe(data => {
        this.action = 0;
        if(showMessage)
        {
          this.router.navigate(['/cronograma']) ;  
          this.openMessage(this._messageString.SuccessMessage, 'success', '');
        }
        
      }, error => {
        this.openDialogLoading(false)
        this.validationObj.habilitadoBotonGuardar = true;
        this.openMessage(this._messageString.ErrorMessage, 'error', '');     
      })
      
    }
    else{      
      this.cronogramaService.ActualizaEstadoRegistroAuditoria(+this.objAuditar.id, this.objUser.userId, this.action).subscribe(data => {
        this.validationObj = data;
        this.action = 0;
        if(showMessage)
        {
          this.router.navigate(['/cronograma']) ;  
          this.openMessage(this._messageString.SuccessMessage, 'success', '');
        }
        
      }, error => {
        this.openDialogLoading(false)
        this.validationObj.habilitadoBotonGuardar = true;
        this.openMessage(this._messageString.ErrorMessage, 'error', '');     
      })
    }    
  }


  //Function for expando groups 
  expand(element:any){   
    if(this.expandirCategoria.find(e => e==element) ){
      this.expandirCategoria= this.expandirCategoria.filter(e=> e!=element);
    }else{
      this.expandirCategoria.push(element);
    }     
  }
  
 //Function for expando groups 
  verifyExpand(element:any):boolean{    
    if(this.expandirCategoria.find(e => e==element) ){ 
      return false;
    }else{ 
      return true;
    }
  }

  //Open Modal Mantine qualification
  openModalMaintainQualification()
  {
    Swal.fire({
      text: this._messageString.MaintainQualification,
      icon: 'question',
      confirmButtonColor: '#a94785',
      confirmButtonText: 'SI',
      showCancelButton: true,
      cancelButtonText:'NO'
    }).then((result) => {
      if (result.value) {
        this.maintainQualification();
      }
    });
  }

  //Save count maintain qualification
  maintainQualification()
  {
    // this.validationObj.observacionHabilitada = true;
    // this.action = enumActionRegistroAuditoriaDetalle.MantenerCalificacion;
    // this.validationObj.habilitadoBotonGuardar = true;
    this.action = enumActionRegistroAuditoriaDetalle.MantenerCalificacion;
    this.getValidations();
  }

  //Edit button action
  editAction()
  {
    this.action = enumActionRegistroAuditoriaDetalle.Editar;
    this.getValidations();
  }
  

  //Open Modal up glosa
  openModalUpGlosa()
  {
    Swal.fire({
      text: this._messageString.UpGlosa,
      icon: 'question',
      confirmButtonColor: '#a94785',
      confirmButtonText: 'SI',
      showCancelButton: true,
      cancelButtonText:'NO'
    }).then((result) => {
      if (result.value) {
        this.upGlosa();
      }
    });
  }

  //up glosa
  upGlosa()
  {
    
    this.action = enumActionRegistroAuditoriaDetalle.LevantarGlosa;
    this.getValidations();

    this.validationObj.habilitadoBotonGuardar = true;
    this.validationObj.observacionHabilitada = true;
    
    this.validationObj.habilitarBotonComiteAdministrativo = false;
    this.validationObj.habilitarBotonComiteExperto = false;
    this.validationObj.habilitarBotonMantenerCalificacion = false;
    this.registerSelected[0].variables.forEach((element:any) => {
      element.dato_DC_NC_ND = this.validationObj.idItemDC;
      element.motivo = "";      
    });
    
  }

  //Open Modal comite management
  openComiteManagement()
  {
    Swal.fire({
      text: this._messageString.ComiteManagement,
      icon: 'question',
      confirmButtonColor: '#a94785',
      confirmButtonText: 'SI',
      showCancelButton: true,
      cancelButtonText:'NO'
    }).then((result) => {
      if (result.value) {
        this.ComiteManagement();
      }
    });
  }

  //pass to  comite management
  ComiteManagement()
  {
    this.action = enumActionRegistroAuditoriaDetalle.Comiteadministrativo;
    this.getValidations();
  }

  //Open Modal comite expert
  openComiteExpert()
  {
    Swal.fire({
      text: this._messageString.ComiteExpert,
      icon: 'question',
      confirmButtonColor: '#a94785',
      confirmButtonText: 'SI',
      showCancelButton: true,
      cancelButtonText:'NO'
    }).then((result) => {
      if (result.value) {
        this.ComiteExpert();
      }
    });
  }

  //pass to  comite expert
  ComiteExpert()
  {
    this.action = enumActionRegistroAuditoriaDetalle.Comiteexperto;
    this.getValidations();
  }

  // Funcion para buscar una variable y mostrarla en pantalla
  setFocus(){  
    document.getElementById('VAR73')?.scrollIntoView();
  }


  validarErrores() {
    let promise = new Promise((resolve, reject) => {
      
      let id = 0;
      if(this.validationObj.validarErroresLogica) //If Enable validation errors
      {
        let existerrors: boolean = false;
        this.cronogramaService.ValidarErrores(this.objUser.userId, +this.objAuditar.id, this.registerSelected).subscribe(data => {
          this.validationError = data;        
          this.registerSelected = data;
  
           //Validate if exist error
           this.validationError.forEach((gropuSelected: any) => { //Go througth  groups
            id = gropuSelected.variableId;

            gropuSelected.variables.forEach((element:any) => { //Go througth  variables

              if(element.error != null && element.error != undefined && element.error.length > 0)           
              {
                
                //Validate errors
                element.error.forEach((errord: any) => {

                  if(errord.restricciones != undefined  && errord.restricciones != null && errord.restricciones.length > 0)
                  {
                    errord.restricciones.forEach((rest: any) => {
                      if(rest.enable && !rest.noCorregible)
                      {
                        this.action = enumActionRegistroAuditoriaDetalle.ErroresEncontrados;
                        existerrors = true;
                      }
                    });
                  }
                });

              }   
            });
            
          }); 
          
          console.log("Id Variable: " + id);
          console.log("existerrors: " + existerrors);
          console.log("erroresReportados: " + this.validationObj.erroresReportados);
         
          if(existerrors && !this.validationObj.erroresReportados) //Validation errors
          {
            console.log("resolve true");
            resolve(true);
          }
          else if(existerrors && this.validationObj.erroresReportados) 
          { 
            this.action =  this.action = enumActionRegistroAuditoriaDetalle.ErroresEncontrados;
            console.log("resolve false");
            resolve(false);
          }
          else
          {
            this.action =  0;
            console.log("resolve false");
            resolve(false);
          } 
  
        }, error => {
          resolve(false);
        }) 
      }
      else{
        resolve(false);
      }

     

    });
    return promise;
  }
    
  customPatterns = {
    A: { pattern: new RegExp("[12]")},
    B: { pattern: new RegExp("[0789]")},
    C: { pattern: new RegExp("[01]")},
    D: { pattern: new RegExp("[0123]")},
    E: { pattern: new RegExp("[0-9]")},
  };

activoEnter = false;

  cambioCampo(evento: any ,obj:any){
   console.log(evento)
   if(evento.key=='Enter'){    
    this.activoEnter=true;
    this.updateAuditRegisterDetail(obj);
    
   }else{
    if(evento.key!='Tab'){
      this.activoEnter=false;
    }
   }
    if(!this.listados.find(e => e == obj.variableId)){
      this.listados.push(obj.variableId);
      console.log('cambio campo add')
    }
    console.log('listado',this.listados);
    
  }

  revisiondata(obj: any){
   if(this.listados.find(e => e == obj.variableId)){
    if(this.options.find(i => i.itemId == obj.motivo)){
      console.log('revision data update')
    this.listados= this.listados.filter(e => e!= obj.variableId);  
    this.updateAuditRegisterDetail(obj);
    } else{
      console.log('revision data borrar')
      if(!this.activoEnter){
        obj.motivo='';
        this.cronogramaService.updateSatusRadioButton(+this.objAuditar.id, this.objUser.userId, obj.motivo.toString(), obj.dato_DC_NC_ND, obj.registroAuditoriaDetalleId, this.action).subscribe(data => {
         this.validationObj = data; 
         this.listados= this.listados.filter(e => e!= obj.variableId);     
       }, error => {
         this.openDialogLoading(false)
         this.openModalError();       
       })
      }
      this.activoEnter=false;
    
    }  
   }
   this.activoEnter=false;
  }



  //Hallazgos

  ConsultarHallazgos()
  {
    this.cronogramaService.ConsultarHallazgos(this.objAuditar.idRadicado).subscribe(data => {
      this.hallazgos = data;
    }, error => {
      this.openModalError();  
    })
  }

  openModalHallazgos(id: number)
  {
    let info = this.hallazgos.filter(x => x.registrosAuditoriaDetalleId == id);

    const dialogRef = this.dialog.open(HallazgoComponent, {
      width: '800px',
      height: '530px',
      data: info,
      disableClose: false
    });
  }

  openModalCalculadora(obj: any)
  {

    let info = {id: obj.registroAuditoriaDetalleId , tipoCalculadora: obj.tipoCalculadora};

    const dialogRef = this.dialog.open(DialogCalculadoraComponent, {
      data: info,
      width: '500px',
    });
    dialogRef.afterClosed().subscribe((res) => { 
      
      if(res.status){
        if(Number(obj.dato_reportado) != Number(res.value)){
          obj.dato_DC_NC_ND  =33;
          obj.motivo= res.value;
        }
      }
       
    }); 
  }


  openModalContexto(contexto: any)
  {
    const dialogRef = this.dialog.open(ContextoBotComponent, {
      width: '800px',
      height: '800px',
      data: contexto,
      disableClose: false
    });
  }


 
}

