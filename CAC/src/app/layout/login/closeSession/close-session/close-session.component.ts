import { Component, Inject, OnDestroy, OnInit } from '@angular/core';
import { FormGroup, FormBuilder, Validators } from '@angular/forms';
import { MatDialogConfig, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { Router } from '@angular/router';
import * as moment from 'moment';
import { CookieService } from 'ngx-cookie-service';
import { Observable, Subject, timer } from 'rxjs';
import { startWith, switchMap } from 'rxjs/operators';
import { DialogData } from 'src/app/app.component';
import { LayoutService } from 'src/app/layout/service/layout.service';
import Swal from 'sweetalert2';
import { enumRespuestaLogin, enumRoles, enumRolesNombre, messageString } from 'src/app/model/Util/enumerations.enum';

@Component({
  selector: 'app-close-session',
  templateUrl: './close-session.component.html',
  styleUrls: ['./close-session.component.scss']
})
export class CloseSessionComponent implements OnInit{
  credenciales!: FormGroup;
  cargando: boolean = false;

  SessionDead:any;
  timeLeft!: number;
  interval: any;
  MensajeError: any;

  //Enumerations
  _messageString: messageString = new messageString();
  _rolNombre: any = enumRoles;

  constructor(public dialogRef: MatDialogRef<CloseSessionComponent>,
              private coockie: CookieService,
              private router: Router,
              public service: LayoutService,
              private formBuilder: FormBuilder,
              @Inject(MAT_DIALOG_DATA) public data: DialogData) {

              }

    ngOnInit(): void {
      this.SessionDead = sessionStorage.getItem("sessionDead");
      console.log('muerte',this.SessionDead)
      this.credenciales = this.formBuilder.group({
        email: ['', [Validators.required, Validators.pattern('^([a-zA-Z0-9._-]+@[a-zA-Z0-9-]+([.]+[a-zA-Z]{2,6}){1,3})')]],
        password: ['', [Validators.required]]
      });
      this.timeLeft = this.SessionDead;
      this.startTimer()
    }

    startTimer() {
      
      // this.interval = setInterval(() => {
      //   if(this.timeLeft > 0) {
      //     this.timeLeft--;
         
      //   } else {
      //     this.pauseTimer()
      //     // sessionStorage.clear();
      //     // localStorage.clear()
      //     // this.coockie.deleteAll();
      //     this.dialogRef.close();
      //     this.router.navigate(['/Login']);
      //   }
      // },1000)
    }

    pauseTimer() {
      clearInterval(this.interval);
    }

    // login(){
    //   this.cargando = true
    //   this.service.postCredentials(this.credenciales.value).subscribe(Response => {
    //     let minutos = moment(Response.expiration).daysInMonth();
    //     var date = new Date();
    //     date.setTime(date.getTime() + (minutos* 60 * 1000))
    //     this.coockie.set("token",Response.token,date)
    //     this.pauseTimer();
    //     this.cargando = false;
    //     this.dialogRef.close();
    //   },error => {
    //     this.cargando = false
    //     this.router.navigate(['Login']);
    //   })
    // }


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
            this.dialogRef.close();
            localStorage.setItem("OpenModalLogin", "false");
          }
          else if(Response.codigoRespuesta == enumRespuestaLogin.ActualizarContrasena)
          {
              this.dialogRef.close();
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


    //Open message
  openMessage( type: any, message: string, title: string)
  {
    Swal.fire({
      title: title,
      text: message,
      icon: type
    }) 
  }
}
