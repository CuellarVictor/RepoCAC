import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { CookieService } from 'ngx-cookie-service';
import { ResponseRol } from 'src/app/model/roles/responseRol.model';
import { objuser } from 'src/utils/models/cronograma/cronograma';
import { PagerService } from '../../cronograma/services/page.service';
import { ImgPermisoModalComponent } from '../img-permiso-modal/img-permiso-modal.component';
import { ConfigService } from '../service/config.service';

@Component({
  selector: 'app-permisos',
  templateUrl: './permisos.component.html',
  styleUrls: ['./permisos.component.scss']
})
export class PermisosComponent implements OnInit {

  listaRoles!: any[];
  buscorol = false;

  dataSource2!: any[];
  displayedColumns2: string[] = ['tipo','nombre', 'visible','habilitado', 'guia'];


  form !: FormGroup;

  //user
  userEncoded: any;
  objUser: objuser;


  constructor( 
    private formBuilder: FormBuilder,
    public dialog: MatDialog, 
    private services: ConfigService,
    private paginatorService: PagerService,
    private coockie: CookieService) {
      this.userEncoded = sessionStorage.getItem("objUser");
      this.objUser = JSON.parse(this.userEncoded);

     }

  ngOnInit(): void {
    this.form = this.formBuilder.group({
      rol:['']
     });
    this.getAdminRoles();    
  }

  getAdminRoles(){
    this.services.getAdminRoles().subscribe(Response => {      
    this.listaRoles = Response as ResponseRol[];
    });
  }

  changeRol(){
    this.getPermisosRol(this.form.controls.rol.value);
  }

  getPermisosRol(idRol: string){
    this.services.consultarPermisosRol(idRol).subscribe(Response => {
      this.dataSource2=Response;
      this.buscorol=true;
    })
  }

  actualizarVisible(event:any,obj: any){
    obj.visible=event.checked;
    obj.user = this.objUser.userId;    
    this.actualizarPermiso(obj);
    }

    actualizarHabilitado(event:any,obj: any){
      obj.habilitado=event.checked;
      obj.user = this.objUser.userId;    
      this.actualizarPermiso(obj);
      }

    actualizarPermiso(obj: any){
      this.services.actualizarPermisos(obj).subscribe(Response => {
        console.log(Response);
        this.changeRol();
        });
    }

    openModalImg(img:string)
    { 
      
      const dialogRef = this.dialog.open(ImgPermisoModalComponent, {
        data: img,
        width: '1000px',
      }).afterClosed().subscribe((res) => {      
   
      }); 
      ;
    }
}
