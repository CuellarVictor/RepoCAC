import { Component, OnInit } from '@angular/core';
import {MatDialog} from '@angular/material/dialog';
import { ObservationsComponent } from '../observations/observations.component';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { DialogComponent } from '../dialog/dialog.component';
import { CookieService } from 'ngx-cookie-service';
import { CronogramaService } from '../../cronograma/services/cronograma.service';
import Swal from 'sweetalert2';
import { detalleRegistro, Registro } from 'src/utils/models/cronograma/information';
import { Router } from '@angular/router';
import * as moment from 'moment';
import { LoadingComponent } from 'src/app/layout/loading/loading.component';

export interface data {
  id: number;
  value: string;
}


const medicion: data[] = [
  {id: 1, value: 'Variable 1'},
  {id: 2, value: 'Variable 2'},
  {id: 3, value: 'Variable 3'},
  {id: 4, value: 'Variable 4'},
]

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

  listEstados: any[] = [];

  panelOpenState = false;
  type_medicion : data[] = medicion;
  dataSource: any;
  columnsToDisplay = ['name','DatoReportado', 'BOT','H','E', 'select','Motivo'];
  expandedElement: Registro | null | undefined;
  expandirCategoria: string[] = new Array();
  objAuditar!:detalleRegistro;
  objUser:any;
  progreso:any;
  progresoDia: number = 0;
  diasTotales: number = 0;

  dataTable:any;

  calificacionTodos:boolean = true;
  tipoCalificacion:boolean = false;
  glosacalificada: number = 0;
  glosaMotivo: number = 0;
  calificacion: any[] = [];
  tipo: any[] = [];
  tipoMotivo: any[] = [];
  observacion:boolean = false;

  showMantCalifi: boolean = false;
  showEditCalifi: boolean = false;
  showCAdministrativo: boolean = false;
  showCExperto: boolean = false;
  showLevantarGlosa: boolean = false;
  showGuardar:boolean = false;
  enableButtonSave: boolean = true;
  enableRadioButtonCalifications: boolean = false;
  validarCalificacionDeTodasGlosas: boolean = false;
  editarCalificacion: boolean = false;
  status:number = 0;
  mantenerCalificacionAuditor:boolean = false;
  display_btn_footer : boolean = false;
  motivoNDNC: boolean = true;
  opcion:number = 0;
  levantarGlosaAccion:boolean = false;

  calificaTodoAuditor:number = 0;

  objMensaje:any;
  variableSinMotivo:any = false;
  validarCalificacionIPS: any = true;
  calificacionVariables:any = true;

  DatoConforme:number = 32;
  DatoNoConforme:number = 33; 
  DatoNoDisponible:number = 34; 

  
  constructor(public dialog: MatDialog,
              private service: CronogramaService,
              private router: Router,
              private coockie: CookieService) {
                this.objAuditar = JSON.parse(atob(this.coockie.get('objAuditar')));
                this.objUser = JSON.parse(atob(this.coockie.get('objUser')));
              }
            

  ngOnInit(): void {  
    this.openDialogLoading(true);
    setTimeout(() => {
      this.getStates(); 
      this.getAuditoriaDetalle();
      this.getProgresoDiario();    

      setTimeout(() => {
        this.validarInicialEstado();
        this.validarGRE();
        this.validarGO1();
        this.validarGORE();
        this.validarGo2();
        this.click();
        }, 1500);   
    }, 1800);
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

  validateAccions(){
    if(this.objAuditar.estado == '5' && this.objUser.rol.userRolId == 2){
      if(this.levantarGlosaAccion || this.mantenerCalificacionAuditor || this.objAuditar.comiteAdministrativo || this.objAuditar.comiteExperto){
        this.showMantCalifi = false;
        this.showEditCalifi = false;
        this.showGuardar = false;
        this.enableRadioButtonCalifications = true;
        this.glosacalificada = 0;
        this.display_btn_footer = true;
      }
    }
  }

  getStates(){
    this.service.GetListStates().subscribe((Response:any) => {
      this.listEstados = Response
      console.log(this.listEstados);
      
    })
  }

  validarInicialEstado(){    
    if(this.objAuditar.estado == '1'){ 
      this.showGuardar = true; 
      this.service.closeAuditoria(this.objAuditar.id,17,this.objUser.userId).subscribe(Response => {
        //this.observacionCambioEstado({tipoObservacion: 11, estadoActual: this.objAuditar.estado, estadoNuevo:17});
      },error => {
        console.log('Error del servicio', error);
      });
    }
    if(this.objAuditar.estado == '17'){
      this.showGuardar = true;      
    }
  }

  validarGRE(){
    if(this.objAuditar.estado == '2'){
      this.showMantCalifi = false;
      this.showEditCalifi = false;
      this.showGuardar = false;
      this.enableRadioButtonCalifications = true;
      this.glosacalificada = 0;
      this.display_btn_footer = true;
      this.click();
    }      
  }

  validarGO1(){
    if(this.objAuditar.estado == '3'){     
      this.showMantCalifi = true;
      this.showEditCalifi = true;
      this.showGuardar = true; 
      this.enableButtonSave = true;    
      this.enableRadioButtonCalifications = true;
      this.glosacalificada = 0;      
    }

    if(this.objAuditar.estado == '3' && this.objUser.rol.userRolId == 2){
      this.showEditCalifi = false;
      this.showMantCalifi = false;
      this.showGuardar = false;
      this.enableButtonSave = true; 
    }
  }

  validarGORE(){    
    if(this.objAuditar.estado == '4'){ // validaciones para GORE debe ser 4    
      this.showMantCalifi = false;
      this.showEditCalifi = false;
      this.showGuardar = false;
      this.enableRadioButtonCalifications = true;
      this.glosacalificada = 0;
      this.display_btn_footer = true;
    }

    if(this.objAuditar.estado == '5' && this.objUser.rol.userRolId == 3 && this.glosacalificada == 0){
      Swal.fire({
        text: 'Auditor, ahora puede calificar todos los grupos de variables',
        confirmButtonColor: '#a94785',
        confirmButtonText: 'ENTENDIDO'
      })
    }
  }

  validarGo2(){     
    if(this.objAuditar.estado == '4' && this.objUser.rol.userRolId == 3){//si el estado es GORE == 4 el auditor no puede generar ninguna accion sobre este registro
      this.enableRadioButtonCalifications = true;
    }
    if(this.objAuditar.estado == '5'){
      this.enableRadioButtonCalifications = true;
      this.showGuardar = true;

      if(this.objUser.rol.userRolId == 2 && this.objAuditar.estado == '5'){//validaciones para el lider debe ser 2
        this.showCAdministrativo = true;
        this.showCExperto = true;
        this.showMantCalifi = true;
        this.showLevantarGlosa = true;
      }

      if(this.objUser.rol.userRolId == 3){//esto solo es para el auditor
        if(this.objAuditar.estado == '5' && this.glosacalificada == 0){
          this.showGuardar = true;
          this.enableButtonSave = true;
          
          this.enableRadioButtonCalifications = false;//habilito todas las calificaciones
          this.click();
        }     

        if(this.objAuditar.estado == '5' && this.glosacalificada > 0){
          this.enableRadioButtonCalifications = true;
          // this.showCAdministrativo = true;
          // this.showCExperto = true;
          // this.showMantCalifi = true;
        }     
      }
    }
  }

  expand(element:any){   
    if(this.expandirCategoria.find(e => e==element) ){
      this.expandirCategoria= this.expandirCategoria.filter(e=> e!=element);
    }else{
      this.expandirCategoria.push(element);
    }     
  }

  verifyExpand(element:any):boolean{    
    if(this.expandirCategoria.find(e => e==element) ){ 
      return false;
    }else{ 
      return true;
    }
  }

  levantarGlosa(){
    this.validateAccions();
    if(this.objAuditar.levantarGlosa > 0){
      Swal.fire({
        html: 'A este registro ya se le realizo el levantamiento de glosa',
        confirmButtonColor: '#a94785',
        confirmButtonText: 'ENTENDIDO',
    })
  }else{
    if(this.objUser.rol.userRolId == 2){//aca debe ser el lider osea el rol es ==  2
        if(this.objAuditar.estado == '5'){//siempre y cuando el estado sea GO2 == 5
          Swal.fire({
            html: 'Lider, está a punto de levantar la glosa de este  registro.<br> ¿Desea confirmar esta acción?',
            confirmButtonColor: '#a94785',
            confirmButtonText: 'Si',
            showCancelButton: true,
              cancelButtonText:'No',
            }).then((result) => {
              console.log(this.dataSource);
              
                if(result.isConfirmed){
                  this.dataSource[0].variables.forEach((element:any) => {
                    element.dato_DC_NC_ND = this.DatoConforme;        
                  });

                  // this.dataSource[0].variables.forEach((element:any) => {
                            
                  //   this.changeStatus(element);       
                  // });

                  this.showCAdministrativo = false;
                  this.showCExperto = false;
                  this.showMantCalifi = false;
                  this.showLevantarGlosa = false;
                  this.enableButtonSave = false;

                //this.observacionCambioEstado({tipoObservacion: 13, estadoActual: 5, estadoNuevo:5});

                const objValue = {
                  registroAuditoriaId: this.objAuditar.id,
                  tipoObservacion: 13,//levantar glosa
                  observacion: 'El lider ha levantado la glosa',
                  soporte: 1,
                  estadoActual: 5,
                  estadoNuevo: 5,
                  createdDate: moment.utc().format('YYYY-MM-DD'),
                  createdBy: this.objUser.userId
                }            
                this.service.postGuardarObservacion(objValue).subscribe(Response => {
                },error => {
                });
                
                // Swal.fire({
                //   text: 'Ha guardado su observación con éxito y el auditor ahora puede continuar con la auditoría.',
                //   confirmButtonColor: '#a94785',
                //   confirmButtonText: 'ACEPTAR'
                // })

                this.levantarGlosaAccion = true;
                }
              })              
          }else{
            Swal.fire({
              text: 'Esta acción desde el lider solo es permitida cuando el estado es GO2 "Glosa Objetada 2"',
              confirmButtonColor: '#a94785',
              confirmButtonText: 'ENTENDIDO'
            })
          }
      }
    }
  }

  observacionCambioEstado(objOption:any) {

    if(this.validacionMensajeMotivo())
    {
        const objValue = {
          registroAuditoriaId:this.objAuditar.id,
          tipoObservacion: 11,//objOption.tipoObservacion,
          observacion: this.listEstados[objOption.estadoActual -1].descripción +' - '+ this.listEstados[objOption.estadoNuevo -1].descripción,
          soporte: 1,
          estadoActual: objOption.estadoActual,
          estadoNuevo: objOption.estadoNuevo,
          createdDate: moment.utc().format('YYYY-MM-DD'),
          createdBy: this.objUser.userId
        }
    
        this.service.postGuardarObservacion(objValue).subscribe(Response => {      
        },error => {
          console.log('Error de servicio',error);
        });  
      }
    else
    {
      this.mensajeMotivo();
    }
      

  }

  comites(opcion:number){
    this.validateAccions();
    //opcion == 1 C. ADMINISTRATIVO
    //opcion == 2 C. EXPERTO
    if((this.objAuditar.comiteAdministrativo || this.objAuditar.comiteExperto) > 0){//valido que solo allan entrado una vez sino sigue el proceso normal
      Swal.fire({
        text: 'Esta acción ya fue ejecutada.',
        confirmButtonColor: '#a94785',
        confirmButtonText: 'ENTENDIDO',     
      })
    }else{
      if(this.objUser.rol.userRolId == '2'){//solo el lider puede acceder a estas funciones de estos botones
        Swal.fire({
          text: 'Lider, confirmando esta acción el registro pasará a estado de Comité y deberá compartir el veredicto en las observaciones una vez lo tenga.',
          confirmButtonColor: '#a94785',
          confirmButtonText: 'ACEPTAR',
          showCancelButton: true,
          cancelButtonText:'CANCELAR',
          allowOutsideClick: false
        }).then((response) => {
          if(response.isConfirmed){
                if(opcion == 1 &&  this.objAuditar.estado == '5' && this.objUser.rol.userRolId == '2'){
                  this.service.closeAuditoria(this.objAuditar.id,9,this.objUser.userId).subscribe(Response => {    
                    this.observacionCambioEstado({tipoObservacion: 2, estadoActual: 5, estadoNuevo:9});                 
                      this.router.navigate(['/cronograma'])                   
                  }, error => {
                    Swal.fire({
                      text:'Estamos presentando problemas para procesar tu solicitud, porfavor intenta nuevamente',
                      confirmButtonColor: '#a94785',
                      confirmButtonText: 'ENTENDIDO',
                    })
                  })
                }
                if(opcion == 2 &&  this.objAuditar.estado == '5' && this.objUser.rol.userRolId == '2'){
                  this.service.closeAuditoria(this.objAuditar.id,10,this.objUser.userId).subscribe(Response => {   
                    this.observacionCambioEstado({tipoObservacion: 3, estadoActual: 5, estadoNuevo:10});
                    this.router.navigate(['/cronograma'])                   
                  },error => {
                    Swal.fire({
                      text:'Estamos presentando problemas para procesar tu solicitud, porfavor intenta nuevamente',
                      confirmButtonColor: '#a94785',
                      confirmButtonText: 'ENTENDIDO',
                    })
                 })
               }
            }
          })   
        } 
     }
  }

  accionCalificacion(opcion:number){  
    this.validateAccions();
    this.motivoNDNC = false;  
    //opcion == 1 EDITAR calificacion
    //opcion == 2 Mantener calificacion    
     
    if((opcion == 1 || opcion == 2) && this.objAuditar.estado == '3'){
      this.enableButtonSave = false;//habilitar el boton de cerrar auditoria      
    }

    if(opcion == 1 && this.objAuditar.estado == '3' && this.objUser.rol.userRolId == 3){
      Swal.fire({
        html: 'Recuerde hacer clic en cerrar auditoria<br>después de editar para guardar la nueva<br>calificación',
        confirmButtonColor: '#a94785',
        confirmButtonText: 'ENTENDIDO'
      }).then((result) => {        
        this.editarCalificacion = true;
        this.glosacalificada = 1;
        this.enableRadioButtonCalifications = false;   
        this.showMantCalifi = false;   
        this.showEditCalifi = false;
      })
    }
 
    if(opcion == 2 && this.objAuditar.estado == '3'){      
      this.opcion = 1
    }

    if(opcion == 2 && this.objAuditar.estado == '3' && this.objUser.rol.userRolId == 3){
      this.enableRadioButtonCalifications = true;
      this.showEditCalifi = false;
      this.showMantCalifi = false;
      this.display_btn_footer=true;
      this.glosacalificada = 0;
 
      const objObservation = {
        registroAuditoriaId:this.objAuditar.id,
        tipoObservacion: 3,//Glosa objetada 1
        observacion: 'El auditor ha mantenido la calificación',
        soporte: 1,
        estadoActual: 3,
        estadoNuevo: 3,
        createdDate: moment.utc(),
        createdBy: this.objUser.userId
      }                  
      this.service.postGuardarObservacion(objObservation).subscribe(Response => {  
        console.log(Response);        
      },error => {
        console.log('Error de servicio',error);
      });

      var objOption={
        value: 13,//mantener calificación
        state: 4
      }
      let bandera  = 0 ;
      if(this.glosacalificada > 0){bandera = 1;}
      const objValue = {
        dataSource: this.dataSource,
        objAuditar: this.objAuditar,
        objOption: objOption,
        bandera: bandera
      }
      const dialogRef = this.dialog.open(ObservationsComponent, {
        data:objValue,
        disableClose: true
      });

      dialogRef.afterClosed().subscribe(result => {
        this.display_btn_footer=false;         
    });

      this.mantenerCalificacionAuditor = true;         
    }

    if(opcion == 2 && this.objUser.rol.userRolId == 2 && this.objAuditar.estado == '4'){//mantener calificacion, rol lider == 2, estado GORE == 4
      var objOption={
        value: 14,
        state: 16
      } 
      this.openDialog(objOption);
    }

    if(opcion == 2 && this.objAuditar.estado == '5' && this.objUser.rol.userRolId == 2){
      if(this.objAuditar.mantenerCalificacion > 0){
        Swal.fire({
          text: 'A este registro ya se le mantuvo la calificación',
          confirmButtonColor: '#a94785',
          confirmButtonText: 'ENTENDIDO',
        })
      }else{
      Swal.fire({
        text: 'Líder, esta seguro que desea mantener esta calificación, de ser así el registro pasará al estado cerrado',
        confirmButtonColor: '#a94785',
        confirmButtonText: 'ESTOY SEGURO',
        showCancelButton: true,
        cancelButtonText:'NO ESTOY SEGURO',
      }).then(response  => {
        if(response.isConfirmed){
          this.showEditCalifi = false;
          this.showLevantarGlosa = false;
          this.showCAdministrativo = false;
          this.showCExperto = false;
          
              this.enableRadioButtonCalifications = true;
              this.display_btn_footer=true;
              this.glosacalificada = 0;

              var objOption={
                value: 13,
                state: 16
              }
              let bandera  = 0 ;
              if(this.glosacalificada > 0){bandera = 1;}
              const objValue = {
                dataSource: this.dataSource,
                objAuditar: this.objAuditar,
                objOption: objOption,
                bandera: bandera
              }
              const dialogRef = this.dialog.open(ObservationsComponent, {
                data:objValue
              }); 
              
              dialogRef.afterClosed().subscribe(result => {
                    if(result){
                      this.enableButtonSave = false;
                      console.log(this.enableButtonSave);
                      Swal.fire({
                        text: 'Ha guardado su observación con éxito y el registro ahora está en estado cerrado. Usted será redirigido al cronograma de auditoría.',
                        confirmButtonColor: '#a94785',
                        confirmButtonText: 'ACEPTAR'
                      })
                      this.mantenerCalificacionAuditor = true; 

                      // const objObservation = {
                      //   registroAuditoriaId:this.objAuditar.id,
                      //   tipoObservacion: 13,//Mantener calificacion
                      //   observacion: 'El lider ha mantenido la calificación',
                      //   soporte: 1,
                      //   estadoActual: 5,
                      //   estadoNuevo: 16,
                      //   createdDate: moment.utc(),
                      //   createdBy: this.objUser.userId
                      // }
                  
                      // this.service.postGuardarObservacion(objObservation).subscribe(Response => {  
                      //   console.log(Response);        
                      // },error => {
                      //   console.log('Error de servicio',error);
                      // });
                      
                      this.observacion = true
                      
                      return this.closeAuditoria();            
                    }else{
                      return null;
                    }     
                 });              
               }
           })
        }
     }

  }
 
  openDialog(objOption:any) {
    let bandera  = 0 ;    
     
    if(this.glosacalificada > 0){
      bandera = 1;
    }

    if(this.levantarGlosaAccion){//si levanto la glosa el man deberia mostrar el valor del 13
      this.objMensaje = {
        dataSource: this.dataSource,
        objAuditar: this.objAuditar,
        objOption: {value: 12, state: this.objAuditar.estado} ,
        bandera: bandera
      }     
    }
    
    if(this.objAuditar.estado == '3' && this.objUser.rol.userRolId == 3 && this.mantenerCalificacionAuditor){
      this.objMensaje = {
        dataSource: this.dataSource,
        objAuditar: this.objAuditar,
        objOption: {value: 3, state: this.objAuditar.estado} ,//mantener calificacion
        bandera: bandera
      }      
    }else if(this.objAuditar.estado == '3' && this.objUser.rol.userRolId == 3 && this.editarCalificacion){
      this.objMensaje = {
        dataSource: this.dataSource,
        objAuditar: this.objAuditar,
        objOption: {value: 1, state: this.objAuditar.estado} ,//general
        bandera: bandera
      }
    }else{
      this.objMensaje = {
        dataSource: this.dataSource,
        objAuditar: this.objAuditar,
        objOption: {value: 0, state: this.objAuditar.estado} ,
        bandera: bandera
      }
    }
    //if((this.objAuditar.estado == '3' || this.objAuditar.estado == '17') && (objOption != 1 || objOption != 'ok')){objValue.objOption = objOption }
     
    this.display_btn_footer=true;    
    if(this.objAuditar.estado == '4'){//el estado seria el 4
      const dialogRef = this.dialog.open(ObservationsComponent, {
        data:this.objMensaje,
        disableClose: true
      });
        dialogRef.afterClosed().subscribe(result => {
            this.display_btn_footer=false;
            if(result){
              this.observacion = true;
             return this.closeAuditoria();
            }else{
              return null;
            }     
        });
    }else{
      const dialogRef = this.dialog.open(ObservationsComponent, {
        data:this.objMensaje
      });
        dialogRef.afterClosed().subscribe(result => {
            this.display_btn_footer=false;
            if(result){
              Swal.fire({
                text: 'Su observación fue guardada con éxito en los detalles del registro',
                confirmButtonColor: '#a94785',
                confirmButtonText: 'ACEPTAR'
              })
              return this.observacion = true;
            }else{
              return null;
            }     
        });
      }
  }
  
  onUpdate(validador: string): void {
    if(validador=='Variables ocultas'){
      const name  = '¿Desea activar la calificación de esta variable?';
      const content = 'Al activar esta variable la calificación entrará dentro del aplicativo de hallazgos';
      const type = 'eliminar';
      const dialogRef = this.dialog.open(DialogComponent, {
        width: '400px',
        data: {name: name, content: content, type: type }
      });
    }
  }

  getDisabledValue(validador: string , check: string) {   

    
    
    if(validador == 'Glosas' && check == this.DatoNoDisponible.toString()){//deshabilito el combo de los motivos
      return true;
    }
  

    if(this.objAuditar.estado == '3' && this.opcion == 1){
      return true
    }
    
    if(this.objAuditar.estado == '2' ||
       this.objAuditar.estado == '4' || 
       this.objAuditar.estado == '8' || 
       this.objAuditar.estado == '13' || 
       this.objAuditar.estado == '16'){
      return true;
    }

  /*  if(this.objAuditar.estado == '17' && this.motivoNDNC == true){
      return false
    }*/

    if(validador=='Variables ocultas'){
      return true;
    }else{
      if(!check){
        return true;
      }else{
      if(check != ''){
        if(check == this.DatoConforme.toString()){
          return true;
        }else{
          return false;
        }
      }else{        
        return true;
      }      
    }
  }

  }

  getAuditoriaDetalle(){
    this.service.GetAuditoriaDetalle(this.objAuditar.id).subscribe(Response => {
      this.dataSource = Response;    
      var listado = JSON.stringify(this.dataSource)      
      localStorage.setItem("objListVariables",listado)
      this.glosacalificada = this.dataSource[0].variables.filter((x : any) => x.dato_DC_NC_ND == this.DatoNoConforme || x.dato_DC_NC_ND == null).length;
      for (let value of this.dataSource) {
        if(value.name.includes('Variables ocultas')){
          this.expandirCategoria.push(value.name);          
        }     
      }   
    }, error => {
      //console.log(error);  
      Swal.fire({
        title: 'Error',
        text: 'Lo sentimos, actualmente estamos presentando errores internos. Por favor intente nuevamente',
        icon: 'error'
      })  
    })
  }

  getProgresoDiario(){
    const idAuditor = {
      idAuditor: this.objAuditar.idAuditor
    };
    this.service.postProgresoDiario(idAuditor).subscribe(Response => {
      this.progreso = Response[0];
      this.progresoDia = this.progreso.progresoDia;
      this.diasTotales = this.progreso.totales;
    },error => {
      Swal.fire({
        title: 'Error',
        text: 'Lo sentimos, actualmente estamos presentando errores internos. Por favor intente nuevamente',
        icon: 'error'
      })
    })
  }

  click(){
    const DC:any = [];     
      this.calificacion = [];
      this.tipo = [];
      this.tipoMotivo = []; 
      setTimeout(() => {  
        this.glosacalificada = this.dataSource[0].variables.filter((x : any) => x.dato_DC_NC_ND == this.DatoNoConforme || x.dato_DC_NC_ND == null).length;
      }, 800);

      console.log('glosa calificada', this.glosacalificada);
      
      // this.glosaMotivo = this.dataSource[0].variables.filter((x : any) => (x.dato_DC_NC_ND == this.DatoNoConforme || x.dato_DC_NC_ND == this.DatoNoDisponible || x.dato_DC_NC_ND == null) && (x.motivo == null || x.motivo == 0 )).length;
      this.glosaMotivo = this.dataSource[0].variables.filter((x : any) => (x.dato_DC_NC_ND == this.DatoNoConforme || x.dato_DC_NC_ND == null) && (x.motivo == null || x.motivo == 0 )).length;
      
      this.dataSource[0].variables.forEach((variables:any) => {DC.push(variables.dato_DC_NC_ND)});
      DC.forEach((element:any) => {if(element){this.calificacion.push(element)}});

      console.log('calificacion', this.calificacion); 

      if(this.calificacion.length == this.dataSource[0].variables.length){this.calificacionTodos = false}else{this.calificacionTodos = true}//todos estan calificados
 
      DC.forEach((element:any) => {if(element != 1 && element){this.tipo.push(element);}});
 
      this.dataSource[0].variables.forEach((x:any) => {
        if((x.dato_DC_NC_ND == this.DatoNoConforme || x.dato_DC_NC_ND == this.DatoNoDisponible || x.dato_DC_NC_ND == null) && (x.motivo == null || x.motivo == 0 )){
          this.tipoMotivo.push(x.reducido);}
        });
     
      if(this.tipo){this.tipoCalificacion = true;
        if(this.glosacalificada!= 0){
          this.expandirCategoria= ['Variables ocultas'];
        }
      }else{this.tipoCalificacion = false;}//existe una calificacion no conforme      
       
      this.enableButtonSave = this.calificacionTodos; 
      
      console.log('todos calificados', this.calificacionTodos);
       
      if(this.objAuditar.estado == '3' && this.objUser.rol.userRolId == 3 && this.mantenerCalificacionAuditor){
        this.enableButtonSave = true;
      }

      if(this.objAuditar.estado == '1' && 
         this.glosacalificada == 0 && 
         this.objUser.rol.userRolId == 3 && 
         !this.validarCalificacionDeTodasGlosas){
        this.validarCalificacionDeTodasGlosas = true;  
      }
      
      if(this.objAuditar.estado == '3' && this.glosacalificada == 0 && this.objUser.rol.userRolId == 3 && this.calificaTodoAuditor == 0){
        Swal.fire({
          text: 'Auditor, recuerde que puede calificar todos los grupos de variables',
          confirmButtonColor: '#a94785',
          confirmButtonText: 'ENTENDIDO'
        }).then(response => {
          if(response.isConfirmed){
            this.calificaTodoAuditor = 1;            
          }
        })
      }  
    this.openDialogLoading(false);
  }

  changeStatus(value:any){      
    if((!value.motivo || value.dato_DC_NC_ND == this.DatoConforme) || (!value.motivo || value.dato_DC_NC_ND == this.DatoNoDisponible )){value.motivo = ""}
    if(this.objAuditar.estado == '1' || this.objAuditar.estado == '17' || this.objAuditar.estado == '5'){
      //   this.service.updateSatusRadioButton(value.motivo, value.dato_DC_NC_ND, value.registroAuditoriaDetalleId).subscribe(Response => {
      // });
    }    
  }

  validationDisabledGroup(value:string)
  {
    if(value == undefined || value == null || value == 'Glosas' )
    {
      return true;
    }
    else
    {
      return false;
    }
  }

  
  
  closeAuditoria(){      
    var mensajeMotivo = this.validacionMensajeMotivo(); 
    if(mensajeMotivo == false){
      Swal.fire({
        text: 'Debe seleccionar el motivo para la calificación de las variables',//'Seleccione los motivos de las siguiente variables: ('+vari+')',
        confirmButtonColor: '#a94785',
        confirmButtonText: 'ACEPTAR'
      }).then(response => {
        if(response.isConfirmed){
          this.variableSinMotivo = true; 
          if(this.validarCalificacionIPS && this.glosacalificada == 0){
              this.GetCalificacionEsCompletas(this.objAuditar.id)
          }     
        }
      });
    }    
    
    if(this.validarCalificacionIPS && mensajeMotivo == true && this.glosacalificada == 0){
     this.GetCalificacionEsCompletas(this.objAuditar.id)
    }

    setTimeout(() => {
      if(mensajeMotivo == true && this.calificacionVariables == true)
      {
        if(this.glosacalificada > 0 && !this.observacion)
        {     
          this.openDialog('ok');//no se esta envbiando la predefinida //GLOSA y no aparece el mensaje con exito de guardado
        }
    
        if(this.glosacalificada == 0 &&  (this.objAuditar.estado == '17' || this.objAuditar.estado == '1'))
        {
            this.status = 8;
            this.service.closeAuditoria(this.objAuditar.id,this.status,this.objUser.userId).subscribe(Response => {
              this.observacionCambioEstado({tipoObservacion: 10, estadoActual: this.objAuditar.estado, estadoNuevo:8});
              this.router.navigate(['/cronograma']);
            },error => {
              Swal.fire({
                html: 'Error interno al cerrar la auditoria, intente nuevamente, si el error persiste comuniquese con el administrador del sistema<br>Codigo Error: '+error,
                confirmButtonColor: '#a94785',
                confirmButtonText: 'ENTENDIDO'
              })
            });      
         
        }
    
        if(this.glosacalificada > 0 && (this.objAuditar.estado == '17' || this.objAuditar.estado == '1') && this.observacion)
        {   
          this.status = 2;
          this.service.closeAuditoria(this.objAuditar.id,this.status,this.objUser.userId).subscribe(Response => {
            this.observacionCambioEstado({tipoObservacion: 11, estadoActual: 1, estadoNuevo:2});
            this.router.navigate(['/cronograma']);
          },error => {
            Swal.fire({
              html: 'Error interno al cerrar la auditoria, intente nuevamente, si el error persiste comuniquese con el administrador del sistema<br>Codigo Error: '+error,
              confirmButtonColor: '#a94785',
              confirmButtonText: 'ENTENDIDO'
            })
          })
        }
        
        if(this.status == 2 && this.objAuditar.estado == '3' && this.observacion && !this.editarCalificacion)
        {
          this.status = 2;
          this.service.closeAuditoria(this.objAuditar.id,this.status,this.objUser.userId).subscribe(Response => {
            this.observacionCambioEstado({tipoObservacion: 11, estadoActual: 3, estadoNuevo:2});
            this.router.navigate(['/cronograma']);
          },error => {
            console.log('Error del servicio', error);
            Swal.fire({
              html: 'Error interno al cerrar la auditoria, intente nuevamente, si el error persiste comuniquese con el administrador del sistema<br>Codigo Error: '+error,
              confirmButtonColor: '#a94785',
              confirmButtonText: 'ENTENDIDO'
            })
          })
        }
        
        if(this.objAuditar.estado == '3' && this.glosacalificada > 0 && this.observacion)
        {
          this.status = 4;
          this.service.closeAuditoria(this.objAuditar.id,this.status,this.objUser.userId).subscribe(Response => {
            this.observacionCambioEstado({tipoObservacion: 10, estadoActual: 3, estadoNuevo:4});
            this.router.navigate(['/cronograma']);
          },error => {
            console.log('Error del servicio', error);
            Swal.fire({
              html: 'Error interno al cerrar la auditoria, intente nuevamente, si el error persiste comuniquese con el administrador del sistema<br>Codigo Error: '+error,
              confirmButtonColor: '#a94785',
              confirmButtonText: 'ENTENDIDO'
            })
          })
        }
    
        if(this.objAuditar.estado == '3' && this.glosacalificada == 0 && this.editarCalificacion){
          this.status = 8;      
            this.service.closeAuditoriaMasivo(this.objAuditar.id,this.status,this.dataSource,this.objUser.userId).subscribe(Response =>{
              this.observacionCambioEstado({tipoObservacion: 10, estadoActual: 3, estadoNuevo:8});
              console.log(Response);  
              this.router.navigate(['/cronograma']);
              // Swal.fire({
              //   text: 'Se ha realizado la edición satisfactoria de las calificaciones.',
              //   confirmButtonColor: '#a94785',
              //   confirmButtonText: 'ENTENDIDO'
              // })      
            }, error => {
              console.log('error insertando masivo' + error);        
            })      
        }
    
        if(this.objAuditar.estado == '3' && this.mantenerCalificacionAuditor && this.objUser.rol.userRolId == '3'){
          this.status = 4;        
          this.service.closeAuditoria(this.objAuditar.id,this.status,this.objUser.userId).subscribe(Response => {          
            this.observacionCambioEstado({tipoObservacion: 1, estadoActual: 3, estadoNuevo:4});
            this.router.navigate(['/cronograma']);
          },error => {
            console.log('Error del servicio', error);
            Swal.fire({
              html: 'Error interno al cerrar la auditoria, intente nuevamente, si el error persiste comuniquese con el administrador del sistema<br>Codigo Error: '+error,
              confirmButtonColor: '#a94785',
              confirmButtonText: 'ENTENDIDO'
            })
          })
        }
        
        if(this.objAuditar.estado == '4' && this.observacion){//debe estar en el estado 4
          this.status = 16;
          this.service.closeAuditoria(this.objAuditar.id,this.status,this.objUser.userId).subscribe(Response => {
            this.observacionCambioEstado({tipoObservacion: 9, estadoActual: 4, estadoNuevo:16});  
            Swal.fire({
              html: 'El registro ah cambiado de estado a Resgistro Cerrado RC #16 <br> ¿Deseas regresar al listado de los registros?',
              confirmButtonColor: '#a94785',
              confirmButtonText: 'Si',
              showCancelButton: true,
              cancelButtonText:'No',
            }).then((result) => {          
              if(result.isConfirmed){this.router.navigate(['/cronograma']);}      
            })
          },error => {
            console.log('Error del servicio', error);
            Swal.fire({
              html: 'Error interno al cerrar la auditoria, intente nuevamente, si el error persiste comuniquese con el administrador del sistema<br>Codigo Error: '+error,
              confirmButtonColor: '#a94785',
              confirmButtonText: 'ENTENDIDO'
            })
          })
        } 
        
        if(this.objAuditar.estado == '5' && this.mantenerCalificacionAuditor && this.observacion){//debe estar en el estado 5
          this.status = 16;
          this.service.closeAuditoria(this.objAuditar.id,this.status,this.objUser.userId).subscribe(Response => {
            this.observacionCambioEstado({tipoObservacion: 10, estadoActual: 5, estadoNuevo:16});  
            this.router.navigate(['/cronograma'])
          },error => {
            console.log('Error del servicio', error);
            Swal.fire({
              html: 'Error interno al cerrar la auditoria, intente nuevamente, si el error persiste comuniquese con el administrador del sistema<br>Codigo Error: '+error,
              confirmButtonColor: '#a94785',
              confirmButtonText: 'ENTENDIDO'
            })
          })
        } 
    
        if(this.objAuditar.estado == '5' && this.levantarGlosaAccion ){//debe estar en el estado 5
          this.status = 5;
          this.service.closeAuditoriaMasivo(this.objAuditar.id,this.status,this.dataSource,this.objUser.userId).subscribe(Response => {
            this.router.navigate(['/cronograma'])
          },error => {
            console.log('Error del servicio', error);
            Swal.fire({
              html: 'Error interno al cerrar la auditoria, intente nuevamente, si el error persiste comuniquese con el administrador del sistema<br>Codigo Error: '+error,
              confirmButtonColor: '#a94785',
              confirmButtonText: 'ENTENDIDO'
            })
          })
        } 
    
        if(this.objAuditar.estado == '5' && this.objUser.rol.userRolId == 3 ){//debe estar en el estado 5
          this.status = 8;
          this.service.closeAuditoria(this.objAuditar.id,this.status,this.objUser.userId).subscribe(Response => {
            this.observacionCambioEstado({tipoObservacion: 10, estadoActual: 5, estadoNuevo:8});  
            this.router.navigate(['/cronograma'])
          },error => {
            console.log('Error del servicio', error);
            Swal.fire({
              html: 'Error interno al cerrar la auditoria, intente nuevamente, si el error persiste comuniquese con el administrador del sistema<br>Codigo Error: '+error,
              confirmButtonColor: '#a94785',
              confirmButtonText: 'ENTENDIDO'
            })
          })
        }
        }
    }, 1500);

  }

  //Validacion para identificar si no se ha registrado un motivo y se debe mostrar modal
  validacionMensajeMotivo()
  {
    let validacionMotivoG1: number = 0;
    let validacionMotivoG2: number = 0;
    let validacionMotivoG3: number = 0;
    let validacionMotivoG4: number = 0;

    this.dataSource[0].variables.forEach((x:any) => {
    //  if((x.dato_DC_NC_ND == this.DatoNoConforme || x.dato_DC_NC_ND == this.DatoNoDisponible || x.dato_DC_NC_ND == null) && (x.motivo == null || x.motivo == 0 )){
      if((x.dato_DC_NC_ND == this.DatoNoConforme || x.dato_DC_NC_ND == null) && (x.motivo == null || x.motivo == 0 )){
       this.tipoMotivo.push(x.reducido);}        
     });


    //Validacion para saber si hay alguna variable de glosas que no sea conforme y se encuentra sin motivo
    validacionMotivoG1 = this.dataSource[0].variables.filter((x : any) =>      
            // ((x.dato_DC_NC_ND == this.DatoNoConforme || x.dato_DC_NC_ND == this.DatoNoDisponible || x.dato_DC_NC_ND == null)
            ((x.dato_DC_NC_ND == this.DatoNoConforme || x.dato_DC_NC_ND == null)
              && (x.motivo == null || x.motivo == 0 )
            )
            ).length;
    console.log("validacionMotivoG1", validacionMotivoG1);

    if(validacionMotivoG1 > 0)
    {
      return false;
    }
     

     //Validacion para saber si todos los datos son conformes y debe entrar a revisar que los demas grupos tengan registrado el motivo
     let glosasnoconforme = this.dataSource[0].variables.filter((x : any) =>      
     (x.dato_DC_NC_ND == this.DatoNoConforme || x.dato_DC_NC_ND == this.DatoNoDisponible || x.dato_DC_NC_ND == null)
     ).length
    if(glosasnoconforme == 0)
    {
        //Validacion para saber si hay alguna variable de demografica que no sea conforme y se encuentra sin motivo
        validacionMotivoG2 = this.dataSource[1].variables.filter((x : any) =>      
        ((x.dato_DC_NC_ND == this.DatoNoConforme || x.dato_DC_NC_ND == this.DatoNoDisponible || x.dato_DC_NC_ND == null)
          && (x.motivo == null || x.motivo == 0 )
        )
        ).length;
        console.log("validacionMotivoG2", validacionMotivoG2);

        if(validacionMotivoG2 > 0)
        {
          return false;
        } 

        //Validacion para saber si hay alguna variable de tratamiento que no sea conforme y se encuentra sin motivo
        validacionMotivoG3 = this.dataSource[2].variables.filter((x : any) =>      
        ((x.dato_DC_NC_ND == this.DatoNoConforme || x.dato_DC_NC_ND == this.DatoNoDisponible || x.dato_DC_NC_ND == null)
          && (x.motivo == null || x.motivo == 0 )
        )
        ).length;
        console.log("validacionMotivoG3", validacionMotivoG3);

        if(validacionMotivoG3 > 0)
        {
          return false;
        } 

        //Validacion para saber si hay alguna variable de grupo seguimiento que no sea conforme y se encuentra sin motivo
        validacionMotivoG4 = this.dataSource[3].variables.filter((x : any) =>      
        ((x.dato_DC_NC_ND == this.DatoNoConforme || x.dato_DC_NC_ND == this.DatoNoDisponible || x.dato_DC_NC_ND == null)
          && (x.motivo == null || x.motivo == 0 )
        )
        ).length;
        console.log("validacionMotivoG4", validacionMotivoG4);

        if(validacionMotivoG4 > 0)
        {
          return false;
        } 
    }

    return true;
  }

  mensajeMotivo()
  {
    Swal.fire({
      text: 'Debe seleccionar el motivo para la calificación de las variables',//'Seleccione los motivos de las siguiente variables: ('+vari+')',
      confirmButtonColor: '#a94785',
      confirmButtonText: 'ACEPTAR'
    }).then(response => {
      if(response.isConfirmed){
        this.variableSinMotivo = true;

        if(!this.validarCalificacionIPS){
          this.GetCalificacionEsCompletas(this.objAuditar.id)
        }
        //si la vartiable de las cvalifivaviones esta en fgakse pss que valla y consulte el metdo
      }
    });
  }

  validarCalificacionIpsMensaje(){
    if(this.validarCalificacionIPS){
      return true;
    }else{
      return false;
    }
  }


  GetCalificacionEsCompletas(registrosAuditoriaId:any){
    if(this.objAuditar.encuesta){
      this.service.GetCalificacionEsCompletas(registrosAuditoriaId).subscribe(Response => {      
        if(Response == true){
          this.calificacionVariables = true;
        }else{
          Swal.fire({
            text: 'Recuerde que para cerrar el registro debe calificar las IPS en el modulo lateral',
            confirmButtonColor: '#a94785',
            confirmButtonText: 'ACEPTAR'
          })
          this.calificacionVariables = false;
        }
      },error => {
        console.log(error);
      })
    }
  }
}

