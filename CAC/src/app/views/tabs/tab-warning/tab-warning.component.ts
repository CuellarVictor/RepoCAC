import { Component, Input, OnInit } from '@angular/core';
import { MatDialogRef, MatDialog, MatDialogConfig } from '@angular/material/dialog';
import { CookieService } from 'ngx-cookie-service';
import { GetPermisosServices } from 'src/app/services/get-permisos.services';
import { objuser } from 'src/utils/models/cronograma/cronograma';
import { IndexComponent } from '../index/index.component';
@Component({
  selector: 'app-tab-warning',
  templateUrl: './tab-warning.component.html',
  styleUrls: ['./tab-warning.component.scss']
})
export class TabWarningComponent implements OnInit {

  @Input() Autorizacion: number = 0;
   

  Default: boolean = false;
  Error: boolean = false;
  Soportes: boolean = false;
  Detalles: boolean = false;
  Contacto: boolean = false;
  Califica: boolean = false;
  Banco: boolean = false;
  glosaM: boolean = false;
  HallazgoM: boolean = false;
  ExtemporaneidadM: boolean = false;
  user: number = 0;

  objUser: objuser;

  constructor(public dialog: MatDialog,
              private coockie: CookieService,
              public permisos: GetPermisosServices) {
    this.objUser = JSON.parse(atob(this.coockie.get('objUser')));
   }

  ngOnInit(): void {
    this.seeOptions();
    
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
          this.ExtemporaneidadM = true;
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


  openTab( opt: number, Autorizacion: number ) {
    const dialogRef = this.dialog.open(IndexComponent,{
      data:{
        tab: opt,       
        width: '400px',  
        Autorizacion: Autorizacion
      },
      panelClass: 'custom-modalbox'
    });

    dialogRef.afterClosed().subscribe(result => {
      console.log(`Dialog result: ${result}`);
    });
  }
}