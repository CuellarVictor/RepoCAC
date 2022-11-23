import { Component, Inject, OnInit } from '@angular/core';
import { MAT_DIALOG_DATA, MatDialogRef } from '@angular/material/dialog';
import { CookieService } from 'ngx-cookie-service';
import { GetPermisosServices } from 'src/app/services/get-permisos.services';
import { objuser } from 'src/utils/models/cronograma/cronograma';

@Component({
  selector: 'app-index',
  templateUrl: './index.component.html',
  styleUrls: ['./index.component.scss']
})
export class IndexComponent implements OnInit {
  active = 0;

  Default: boolean = false;
  Error: boolean = false;
  Soportes: boolean = false;
  Detalles: boolean = false;
  Contacto: boolean = false;
  Califica: boolean = false;
  Banco: boolean = false;
  glosaM: boolean = false;
  HallazgoM: boolean = false;
  user: number = 0;


  Autorizacion: number = 0;

  objUser: objuser;

  constructor(private coockie: CookieService,
              public permisos: GetPermisosServices,
              private dialogRef: MatDialogRef<IndexComponent>,
              @Inject(MAT_DIALOG_DATA) data:any) { 
              this.active = data.tab;
              this.Autorizacion = data.Autorizacion;
              this.objUser = JSON.parse(atob(this.coockie.get('objUser')));
    }

  ngOnInit(): void {
    this.seeOptions();    
  }

  

  selectTab( num : number){
    this.active = num;
  }

  seeOptions(){
    if(this.objUser.rol.userRolId == '3' && this.Autorizacion == 1){
      this.user = 1 //Auditor en dashboard
    }

    if(this.objUser.rol.userRolId == '2' && this.Autorizacion == 1){
      this.user = 2 //Lider en dashboard
    }

    if(this.objUser.rol.userRolId == '3' && this.Autorizacion == 2){
      this.user = 3 //Auditor en registro
    }

    if(this.objUser.rol.userRolId == '2' && this.Autorizacion == 2){
      this.user = 4 //Lider en registro
    }

   this.Default = true;
      this.Banco = true;
      switch(this.user){
        case 1:{//Auditor en dashboard
          this.Default = true;
          this.Error = false;
          this.Soportes = false;
          this.Detalles = false;
          this.Contacto = false;
          this.Califica = false;
          this.Banco = true;
          this.glosaM = false;
          this.HallazgoM = false;
          break
        } 
        case 2:{//Lider en dashboard
          this.Default = true;
          this.Error = false;
          this.Soportes = false;
          this.Detalles = false;
          this.Contacto = false;
          this.Califica = false;
          this.Banco = true;
          this.glosaM = true;
          this.HallazgoM = true;
          break
        }
        case 3:{//Auditor en registro
          this.Default = true;
          this.Error = true;
          this.Detalles = true;
          this.Soportes = true;
          this.Contacto = false;
          this.Califica = true;
          this.Banco = true;
          this.glosaM = false;
          this.HallazgoM = false;
          break
        } 
        case 4:{//Lider en registro
          this.Default = true;
          this.Error = true;
          this.Detalles = true;
          this.Soportes = true;
          this.Contacto = false;
          this.Califica = true;
          this.Banco = true;
          this.glosaM = false;
          this.HallazgoM = false;
          break
        }  
      }
    } 

}
