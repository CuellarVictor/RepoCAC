import { Component, EventEmitter, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { FileUploader } from 'ng2-file-upload';
import { CookieService } from 'ngx-cookie-service';
import { objMedicion } from 'src/utils/models/medicion/medicion';
import Swal from 'sweetalert2';
import { ManagementService } from '../services/management.service';
import * as moment from 'moment';
import { LoadingComponent } from 'src/app/layout/loading/loading.component';
import { MatDialog } from '@angular/material/dialog';
import { EstadosBolsa, messageString } from 'src/app/model/Util/enumerations.enum';
import { objuser } from 'src/utils/models/cronograma/cronograma';
import { GetPermisosServices } from 'src/app/services/get-permisos.services';

export interface data {
  id: number;
  value: string;
}

const enfermedad: data[] = [
  {id: 1, value: 'Variable 1'},
  {id: 2, value: 'Variable 2'},
  {id: 3, value: 'Variable 3'},
  {id: 4, value: 'Variable 4'},
]

const URL = 'http://localhost:3000/fileupload/';

@Component({
  selector: 'app-medicion',
  templateUrl: './medicion.component.html',
  styleUrls: ['./medicion.component.scss']
})


export class MedicionComponent implements OnInit {

  date:Date = new Date()// moment.utc().format('full')
  fechaInicioAuditoria!:Date;
  fechaFinAuditoria!:Date;

  nombre: string = 'Nueva medición'

//user
  userEncoded: any;
  objUser: objuser;


  type_enfermedad:any;
  fileToUpload: File | null = null;
  primerNombre: string = "";
  medicionSeleccionada:any;
  objMedicion:objMedicion = new objMedicion();

  medicion!: FormGroup;

  variable:any = false;
  variable2:any='';

  textoBotonGuardar:string = 'CREAR MEDICIÓN';
  accion:number = 1;//pordefecto va a crear

  disabledButton: boolean = true;

  calendarioDisable:boolean = true;

  idsAsociados:any;
  disabledEnfMadre = false;
  estado='';

  //Enumerations
  _messageString: messageString = new messageString();

      
  customPatterns = {
    A: { pattern: new RegExp("[12]")},
    B: { pattern: new RegExp("[09]")},
    C: { pattern: new RegExp("[01]")},
    D: { pattern: new RegExp("[0123]")},
    E: { pattern: new RegExp("[0-9]")},
  };
  
  constructor(private coockie: CookieService,private router: Router,
              private route: ActivatedRoute,
              private services: ManagementService,
              public dialog: MatDialog,
              private formBuilder: FormBuilder,
              public permisos: GetPermisosServices) {  

                      this.userEncoded = sessionStorage.getItem("objUser");
                      this.objUser = JSON.parse(this.userEncoded);
                      this.primerNombre = this.objUser.name;
                      }

public uploader: FileUploader = new FileUploader({ url: URL, itemAlias: 'photo' });

  ngOnInit(): void {
    this.openDialogLoading(true);
    this.medicionSeleccionada = this.route.snapshot.paramMap.get('objMedicion');
    this.medicionSeleccionada = JSON.parse(atob(this.medicionSeleccionada));


    this.uploader.onAfterAddingFile = (file) => { file.withCredentials = false; };
    this.uploader.onCompleteItem = (item: any, response: any, status: any, headers: any) => {
         alert('File uploaded successfully');
    };  
    if(this.medicionSeleccionada.idMedicion){//controlo las acciones y valores de actualizacion
      this.obtenerDatosMedicion()
      this.textoBotonGuardar = 'ACTUALIZAR'
      this.accion = 2;  
      this.disabledEnfMadre = true;
      this.estado=this.medicionSeleccionada.estadoAuditoria;
      
    };

    setTimeout(() => {
      this.medicion = this.formBuilder.group({
        idCobertura:[this.objMedicion.idCobertura ? this.objMedicion.idCobertura : '', Validators.required],
        // lider:['', Validators.required],
        lider:[this.objMedicion.lider  ? this.objMedicion.lider : '', Validators.required],
        Resolucion:[this.objMedicion.resolucion  ? this.objMedicion.resolucion : '', Validators.required],
        nombre:[this.objMedicion.nombre ? this.objMedicion.nombre : '', Validators.required],
        fechaInicioAuditoria:[this.objMedicion.fechaInicioAuditoria ? this.objMedicion.fechaInicioAuditoria : ''],
        fechaFinAuditoria:[this.objMedicion.fechaFinAuditoria ? this.objMedicion.fechaFinAuditoria : ''],
        descripcion:[this.objMedicion.descripcion ? this.objMedicion.descripcion : '', Validators.required],   
        idPeriodo:[this.objMedicion.idPeriodo ? this.objMedicion.idPeriodo : 1, Validators.required], //Por defecto 1
        activo: [this.objMedicion.activo ? this.objMedicion.activo : true],//pordefecto TRUE
        createdBy: [this.objMedicion.createdBy ? this.objMedicion.createdBy : this.objUser.userId],//por defecto el usuario  que esta logueado
        createdDate: [this.objMedicion.createdDate ? this.objMedicion.createdDate : moment.utc().format('YYYY-MM-DD'),],
        modifyBy: [this.objMedicion.modifyBy ? this.objMedicion.modifyBy : this.objUser.userId],//por defecto el usuario que esta logueado
        modifyDate: [this.objMedicion.modifyDate ? this.objMedicion.modifyDate : moment.utc().format('YYYY-MM-DD'),],
        estado: [this.objMedicion.estado ? this.objMedicion.estado : 31],//por defecto 28
        fechaCorteAuditoria:[this.objMedicion.fechaCorteAuditoria ? this.objMedicion.fechaCorteAuditoria : '']
      });
      console.log(this.medicion.value);
      if(this.estado!=''){
      if(this.estado==EstadosBolsa.Finalizada){
        this.medicion.get('fechaInicioAuditoria')?.disable();      
        this.medicion.get('nombre')?.disable();
        this.medicion.get('Resolucion')?.disable();   
        this.medicion.get('descripcion')?.disable();  
        if(this.permisos.Modificar_Fecha_Final_Medicion.habilitado){
          this.medicion.get('fechaFinAuditoria')?.enable();  
        }else{
          this.medicion.get('fechaFinAuditoria')?.disable();   
        }
       
        
      }
      this.date=this.medicion.get('fechaInicioAuditoria')?.value;
      //this.medicion.get('fechaCorteAuditoria')?.disable();
      this.fechaInicioAuditoria = this.medicion.get("fechaInicioAuditoria")?.value;
      this.fechaFinAuditoria = this.medicion.get("fechaFinAuditoria")?.value;

      // if(this.medicion.get('fechaCorteAuditoria')?.value != null 
      //   && this.medicion.get('fechaCorteAuditoria')?.value != undefined 
      //   && this.medicion.get('fechaCorteAuditoria')?.value != "")
      //   {
      //     this.medicion.get('fechaCorteAuditoria')?.disable();
      //   }

      }
     
     //
      this.getEnfermedadMadre();
      this.validarBoton();
    }, 3500);    
    
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

  validarBoton(){ 
    if(this.medicion.status == 'VALID'){
      this.disabledButton = false
    }else{
      this.disabledButton = true
    }
    this.openDialogLoading(false);
  }

  obtenerDatosMedicion(){
    this.services.getMedicion(this.medicionSeleccionada.idMedicion).subscribe(Response => {
     this.objMedicion = Response; 
     this.nombre =  this.objMedicion.nombre;     
    })
  }

  getEnfermedadMadre(){    
    this.services.getEnfermedadMadreXUsuario(this.objUser.userId).subscribe(Response => { 
      this.type_enfermedad = Response;    
    },error => {
      console.log(error);
    })
  }

  accionBoton(accion:any){    
    this.medicion.get('lider')?.setValue("");        
    if(accion == 1){
      this.crearMedicion();
    }else{
      this.editarMedicion();
    }    
  }

  crearMedicion(){


    if(this.variable2 == ""  || this.variable2 == null || this.variable2 == undefined)
    {
      Swal.fire({
        title: "Error",
        text: this._messageString.CoberturaNotFoundLider,
        icon: "error",
      });
    }
    else
    {
      if(this.medicion.get("fechaInicioAuditoria")?.value){
        this.medicion.get("fechaInicioAuditoria")?.setValue(moment(this.medicion.get("fechaInicioAuditoria")?.value).format('YYYY-MM-DD'));
        this.medicion.get("fechaFinAuditoria")?.setValue(moment(this.medicion.get("fechaFinAuditoria")?.value).format('YYYY-MM-DD'));
        this.medicion.get("fechaCorteAuditoria")?.setValue(moment(this.medicion.get("fechaCorteAuditoria")?.value).format('YYYY-MM-DD'));
      }
  
      this.openDialogLoading(true);
      this.services.createMedicion(this.medicion.value).subscribe(Response => {
        this.openDialogLoading(false);
        Swal.fire({
          title: 'Creación correcta',
          text: 'La medición fue creada correctamente',
          icon: 'success',
          confirmButtonColor: '#a94785',
          confirmButtonText: 'ACEPTAR',
        }).then(resp => {
          this.router.navigateByUrl('/gestion-de-auditoria');
        })
      },error => {
        this.openDialogLoading(false);
        console.log('error',error);      
        Swal.fire({
          title: "Error",
          text: "Lo sentimos, actualmente estamos presentando errores internos. Por favor intente nuevamente.",
          icon: "error",
        });
      });    
    }
    
  }  

  editarMedicion(){    
    if(this.medicion.get("fechaInicioAuditoria")?.value){
      this.medicion.get("fechaInicioAuditoria")?.setValue(moment(this.medicion.get("fechaInicioAuditoria")?.value).format('YYYY-MM-DD'));
      this.medicion.get("fechaFinAuditoria")?.setValue(moment(this.medicion.get("fechaFinAuditoria")?.value).format('YYYY-MM-DD'));
      this.medicion.get("fechaCorteAuditoria")?.setValue(moment(this.medicion.get("fechaCorteAuditoria")?.value).format('YYYY-MM-DD'));
    }
    this.medicion.get("fechaCorteAuditoria")?.setValue(moment(this.medicion.get("fechaCorteAuditoria")?.value).format('YYYY-MM-DD'));

    this.openDialogLoading(true);
    this.services.editarMedicion(this.medicionSeleccionada.idMedicion, this.medicion.getRawValue()).subscribe(Response => { 
      this.openDialogLoading(false);
      Swal.fire({
        text: 'Actualizacion de la medición exitosa',
        icon: 'success',
        confirmButtonColor: '#a94785',
        confirmButtonText: 'ACEPTAR',
      }).then(resp => {
        this.router.navigateByUrl('/gestion-de-auditoria');
      })     
    },error => {
      this.openDialogLoading(false);
      console.log(error);
      Swal.fire({
        title: "Error",
        text: "Lo sentimos, actualmente estamos presentando errores internos. Por favor intente nuevamente.",
        icon: "error",
      });
    })
  }


  GetUsuariosConcatByRoleCoberturaId(element:any){
    var objUser = {
      roleId: this.objUser.rol.userRolId,
      coberturaId: element.idCobertura,
      userId: this.objUser.userId
    };
    this.services.GetUsuariosConcatByRoleCoberturaId(objUser).subscribe(Response => {   
      this.variable2 = Response.usuarios;
      this.idsAsociados = Response.id;      
      this.medicion.get('lider')?.setValue(this.variable2);
      document.getElementById('lider')!.setAttribute("disabled","disabled");
      this.variable = true;      
    })

  }

  validateFechaInicial(){    
    this.fechaInicioAuditoria = this.medicion.get("fechaInicioAuditoria")?.value
      var crt = this.medicion.get("fechaFinAuditoria");
      crt?.enable();       
  }

  validateFechaFinal(){
  /*  this.fechaFinAuditoria = this.medicion.get("fechaFinAuditoria")?.value
      var crt = this.medicion.get("fechaCorteAuditoria");
      crt?.enable();*/
  }
  convertDate(dateTime: any) {
    var date = new Date(dateTime),
      mnth = ("0" + (date.getMonth() + 1)).slice(-2),
      day = ("0" + date.getDate()).slice(-2);
    return [date.getFullYear(), mnth, day].join("-");
  }

  validarFecha(control: string){
    let valor=  this.medicion.get(control)?.value;
    let division= valor.split('-', 3);
    console.log(division);
    if(division.length==3){
      division[1]=Number(division[1])>12?'12':division[1];
      division[1]=Number(division[1])==0?'01':division[1];
      division[2]=Number(division[2])>31?'30':division[2];
      division[2]=Number(division[2])==0?'01':division[2];
      division[2]=Number(division[1])==2 && Number(division[2])>28?'28':division[2];
      this.medicion.get(control)?.setValue(division[0]+'-'+division[1]+'-'+division[2]);
    }else{
      this.medicion.get(control)?.setValue('');
    }
  }


}
