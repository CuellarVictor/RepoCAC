import { Component, OnInit } from '@angular/core';
import { RequestRol } from 'src/app/model/roles/requestRol.model';
import { GetPermisosServices } from 'src/app/services/get-permisos.services';
import { objuser } from 'src/utils/models/cronograma/cronograma';
import { ConfigService } from '../service/config.service';
import * as moment from 'moment';
import Swal from 'sweetalert2';
import { ResponseRol } from 'src/app/model/roles/responseRol.model';

@Component({
  selector: 'app-admin-rol',
  templateUrl: './admin-rol.component.html',
  styleUrls: ['./admin-rol.component.scss']
})
export class AdminRolComponent implements OnInit {


  userEncoded: any;
  objUser: objuser;
  
  activarCrearItem=false;
  checkSaveLider = false;
  listaRol:ResponseRol[] = [];

  constructor(public permisos: GetPermisosServices,    
              private service: ConfigService) {
                this.userEncoded = sessionStorage.getItem("objUser");
                this.objUser = JSON.parse(this.userEncoded);
     }

  ngOnInit(): void {
    this.getAdminRoles();
  }

  getAdminRoles(){
    this.service.getAdminRoles().subscribe(Response => {      
    this.listaRol = Response as ResponseRol[];
    });
  }


  saveItemAction(value:string){
    let obj : RequestRol = new RequestRol;
    obj={
        id: '',
        concurrencyStamp: '',
        esLider: this.checkSaveLider,
      createdBy: this.objUser.userId,
      modifyBy:  this.objUser.userId,
      name: value,
      normalizedName: value
      };    
        Swal.fire({
          title: "¿Está seguro que desea crear el Rol '"+value+"'?",
         // text: 'La eliminación no es reversible.',
          icon: "info",
          showCancelButton: true,
          cancelButtonText: "CANCELAR",
          cancelButtonColor: 'lightgray',
          confirmButtonColor: '#a94785',  
          confirmButtonText: `OK`,
        }).then((res) => {
          if (!res.isDismissed) { 
            this.service.crearRol(obj).subscribe(response => {
              if(response){
                console.log("Item creado");
               this.getAdminRoles();
                this.activarCrearItemAction(false);
                this.checkSaveLider=false;

              }
            });
          }else{
            this.getAdminRoles();
            this.activarCrearItemAction(false);
          }

        });
      
        }
        editarItemAction(check:boolean,entrada:string,obj: ResponseRol){
            setTimeout(() => {
           
            if(obj.name!=entrada || obj.esLider!=!check){
              
              Swal.fire({
                title: '¿Está seguro que desea actualizar el item "'+obj.name+'"?"',
                text: 'Sera actualizado al siguiente valor "'+entrada+'", Es lider: '+(!check?'Si':'No')+' .',
                icon: "warning",
                showCancelButton: true,
                cancelButtonText: "CANCELAR",
                cancelButtonColor: 'lightgray',
                confirmButtonColor: '#a94785',  
                confirmButtonText: `OK`,
              }).then((res) => {

                let objSend= new RequestRol;
                objSend.id= obj.id;
                objSend.concurrencyStamp='';
                objSend.esLider=!check;
                objSend.name=entrada;
                objSend.normalizedName=entrada;
                objSend.createdBy= obj.createdBy;
                objSend.modifyBy= this.objUser.userId;
                               
                if (!res.isDismissed) { 
                  this.service.updateRol(obj.id,objSend).subscribe(response => {
                    if(response){
                      console.log("Rol editado");
                    }
                    this.getAdminRoles();
                    this.activarCrearItemAction(false);
                  });
                }else{
                  this.getAdminRoles();
                  this.activarCrearItemAction(false);
                }
         
              });  
            }
            
          }, 50);
                    
              }

              eliminarItemAction(obj: ResponseRol,categoria: string){
                      Swal.fire({
                      title: '¿Está seguro que desea eliminar el rol "'+obj.name+'"?"',
                      text: 'Sera eliminara permanentemente.',
                      icon: "error",
                      showCancelButton: true,
                      cancelButtonText: "CANCELAR",
                      cancelButtonColor: 'lightgray',
                      confirmButtonColor: '#a94785',  
                      confirmButtonText: `OK`,
                    }).then((res) => {
                      if (!res.isDismissed) { 
                        this.service.deleteRol(obj.id.toString()).subscribe(response => {
                          console.log(response)
                         
                            Swal.fire({
                              text: response.mensajeRespuesta,
                              confirmButtonColor: '#a94785',
                              confirmButtonText: 'ENTENDIDO'
                            })
                          
                         
                        this.getAdminRoles();
                          
                        });
                      }
                    });              
                    }


                    activarCrearItemAction(status: boolean){
                      this.activarCrearItem=status;
                    }


}
