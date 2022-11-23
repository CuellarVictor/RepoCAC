import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { enumRespuestaLogin, enumValidacionToken, messageString } from 'src/app/model/Util/enumerations.enum';
import { environment } from 'src/environments/environment';
import Swal from 'sweetalert2';
import { LayoutService } from '../../service/layout.service';
import { ActualizaPassword } from 'src/app/model/auth/actualizarpassword';

@Component({
  selector: 'app-recuperar-password',
  templateUrl: './recuperar-password.component.html',
  styleUrls: ['./recuperar-password.component.scss']
})
export class RecuperarPasswordComponent implements OnInit {

  id:any;
  message:string = ""
  loading: boolean = true
  validation: number = 0;
  form!: FormGroup;
  validationPasswords: boolean = true;
  objActualizar: ActualizaPassword = new ActualizaPassword();
  action:boolean = false;

  //Enumerations
  _messageString: messageString = new messageString();
  _enumValidacionToken : any = enumValidacionToken;

  constructor(
    private activatedRoute: ActivatedRoute,
    public service: LayoutService,
    private formBuilder: FormBuilder) { 

    this.getRouterParam();
  }

  ngOnInit(): void {
    

    this.form = this.formBuilder.group({
      password1: ['', [Validators.required, Validators.pattern('^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$')]],
      password2: ['' ,[Validators.required, Validators.pattern('^(?=.*?[A-Z])(?=.*?[a-z])(?=.*?[0-9])(?=.*?[#?!@$%^&*-]).{8,}$')]]
    });

  }
  

  getRouterParam() {
    this.activatedRoute.params.subscribe((params) => {
      if (
        params["id"] == undefined ||
        params["id"] == null
      ) {
        this.id = null;
      } else {
        this.id = params["id"];
      }

      this.validaTokenRecuperacion(this.id);
    });
  }

  validaTokenRecuperacion(token:string)
  {
    this.service.validaTokenRecuperacion(token).subscribe(Response => {
      this.validation = Response.id;

      if(Response.id != this._enumValidacionToken.Correcto)
      {
          this.message = Response.valor
      }
      this.loading = false;
    },error => {
      this.loading = false;
    })
  }


  showPassword1(){
    const value = document.getElementById('password1')?.getAttribute("type");

    if(value == 'password'){
      document.getElementById('password1')?.setAttribute("type", "text")
    }else{
      document.getElementById('password1')?.setAttribute("type", "password")
    }

    console.log(this.form);
  }

  showPassword2(){
    const value = document.getElementById('password2')?.getAttribute("type");

    if(value == 'password'){
      document.getElementById('password2')?.setAttribute("type", "text")
    }else{
      document.getElementById('password2')?.setAttribute("type", "password")
    }
  }

  validationEqualPassword()
  {
    this.action = true;
    if(this.form.controls.password1.value != this.form.controls.password2.value)
    {
      this.validationPasswords = false;
    }
    else{
      this.validationPasswords = true;
    }
  }


  actualizaPassword()
  {
    this.loading = true;
    this.message = "";
    this.objActualizar.Password = this.form.controls.password1.value;
    this.objActualizar.Token = this.id;
    this.service.actualizaPassword(this.objActualizar).subscribe(Response => {
      this.loading = false;

      let message = Response.valor;
      if(Response.id == enumRespuestaLogin.Exitoso)
      {         
          Swal.fire({
            title: "Correcto",
            text: this._messageString.SuccessMessage,
            icon: "success",
            confirmButtonText: 'Aceptar',
            showCancelButton: false,
          }).then((result) => {          
            window.location.href = environment.rutaFront + "/#"
          })
      }
      else
      {
        this.message = message;
        this.loading = false;
      }

      
    },error => {
      this.message = this._messageString.ErrorMessage;
      this.loading = false;
    })
  }


  cancel()
  {
    window.location.href = environment.rutaFront + "/#"
  }

}
