import { Component, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CookieService } from 'ngx-cookie-service';
import { LayoutService } from '../service/layout.service';
import * as moment from 'moment';
import { Router } from '@angular/router';
import Swal from 'sweetalert2';
import { enumRespuestaLogin, enumRoles, enumRolesNombre, messageString } from 'src/app/model/Util/enumerations.enum';
import { MatDialog } from '@angular/material/dialog';
import { CloseSessionComponent } from './closeSession/close-session/close-session.component';

 
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.scss']
})
export class LoginComponent implements OnInit {
  credenciales!: FormGroup;
  cargando: boolean = false;
  show: any = true;
  MensajeError: any;
  userId: any;
  
  //Enumerations
  _messageString: messageString = new messageString();
  _rolNombre: any = enumRoles;
  constructor(public service: LayoutService,
              private coockie: CookieService,
              private router: Router,
              private formBuilder: FormBuilder,
              public dialog: MatDialog) { }

  ngOnInit(): void {
    console.log('Load Login: ' + (new Date));
    this.credenciales = this.formBuilder.group({
      email: ['', [Validators.required, Validators.pattern('^([a-zA-Z0-9._-]+@[a-zA-Z0-9-]+([.]+[a-zA-Z]{2,6}){1,3})')]],
      password: ['', [Validators.required]]
    });

    this.userId = localStorage.getItem("UserId")
    this.cerrarSesion();
    localStorage.setItem("OpenModalLogin", "false");

  }

  
  login(){
    this.MensajeError = '';
    if(this.credenciales.valid)
    {
      this.cargando = true
      this.service.postCredentials(this.credenciales.value).subscribe(Response => {
      if(Response){ 

        console.log(Response)

        if(Response.codigoRespuesta == enumRespuestaLogin.Exitoso)
        {
          var rol = Response.objUsuario.rol.userRolName;
          var encoded = btoa(JSON.stringify(Response.objUsuario));
          sessionStorage.setItem("sessionDead",Response.sessionDead);
          sessionStorage.setItem("inactivityTime",Response.inactivityTime);
          sessionStorage.setItem("rol",rol);
          sessionStorage.setItem("objUser", JSON.stringify(Response.objUsuario));
          sessionStorage.setItem("permisoRol",btoa(JSON.stringify(Response.permisoRol)));
          localStorage.setItem("UserId",Response.objUsuario.userId);
          sessionStorage.setItem('lider', Response.esLider)

          localStorage.setItem("Expiration",Response.expiration);

          this.coockie.set("objUser",encoded);
          let minutos = +Response.sessionDead;
          var date = new Date();
          date.setTime(date.getTime() + (minutos* 60 * 1000));
          console.log("date token", date)
          this.coockie.set("token",Response.token,date)
          this.cargando = false;
          console.log(Response.objUsuario)
          console.log(this._rolNombre.Auditor)
          if(Number(Response.objUsuario.rol.userRolId) == Number(this._rolNombre.Auditor))
            this.router.navigate(['/cronograma']); 
          else
            this.router.navigate(['/gestion-de-auditoria']);
        }
        else if(Response.codigoRespuesta == enumRespuestaLogin.ActualizarContrasena)
        {
            this.openMessage('warning', Response.mensajeRespuesta, 'Actualizar Contraseña');
            this.router.navigate(['/recuperarpassword/' + Response.objUsuario.userId]); 
        }
        else
        {
          this.MensajeError = Response.mensajeRespuesta;
          this.cargando = false;
        }
          
      }
    },error => {
      if(error.status == 400){
        this.cargando = false;
        this.MensajeError = error.error;
      }else{
        this.cargando = false;
      Swal.fire({
        text: error.error,
        icon: 'error'
      })
      }
    })
    }
    else
    {
      this.MensajeError = this._messageString.NecesaryUserMessage;
    }
    
  }

  contrase(){
    const value = document.getElementById('password')?.getAttribute("type");

    if(value == 'password'){
      document.getElementById('password')?.setAttribute("type", "text")
      this.show = false;
    }else{
      document.getElementById('password')?.setAttribute("type", "password")
      this.show = true;
    }
  }


  recuperarPassword()
  {
    this.MensajeError = '';
    if(this.credenciales.controls.email.value != undefined && this.credenciales.controls.email.value != '' &&this.credenciales.controls.email.value != null)
    {
      this.cargando = true;
      this.service.recuperarPassword(this.credenciales.value).subscribe(Response => {
        this.cargando = false;
        
        this.MensajeError = Response.valor;       
        
      },error => {
        this.MensajeError = this._messageString.ErrorMessage;
        this.cargando = false;
      })
    }
    else
    {
      this.MensajeError = this._messageString.NecesaryUserMessage;
    }
  }

  //Open message
  openMessage( type: any, message: string, title: string)
  {
    Swal.fire({
      title: title,
      text: message,
      icon: type
    }) 
  }

  cerrarSesion()
  {
    if(this.userId != undefined && this.userId != null)
    {
      this.service.cerrarSesion(this.userId).subscribe(
        (Response) => {   
          
          localStorage.clear();
          sessionStorage.clear();
  
        },
        (error) => { 
        }
      );
    }
    
  }

}
