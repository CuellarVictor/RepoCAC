import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { CookieService } from 'ngx-cookie-service';
import { TabsService } from '../services/tabs.service';
import { StarRatingComponent } from 'ng-starrating';
import * as moment from 'moment';
import Swal from 'sweetalert2';

//Models
import { CalificacionModel } from 'src/app/model/calificacion.model';
import { ItemModel } from 'src/app/model/Util/item.model';
import { RegistroAuditoriaDetalleModel } from 'src/app/model/registroauditoria/registroauditoriadetalle.model';

@Component({
  selector: 'app-qualify',
  templateUrl: './qualify.component.html',
  styleUrls: ['./qualify.component.scss']
})
export class QualifyComponent implements OnInit {
  rating3:any;

  ilegible!: string;
  modificable!: string;
  Soporte!: string;
  Supersalud!: string;
  objUser:any;

  disabledSave:boolean = true;


  public form!: FormGroup;
  objAuditar:any;
  objListCalificaciones:any;
  listCatalogCalifica:ItemModel[] = [];
  variableCalificar:number = 0;
  variableId:any;
  ipsId:any;
  disabledCalification: boolean = true;
  observation:any = "";

  objlistado: any = [];

  //Models
  listCalifications: CalificacionModel[] = [];
  objCalificacion: RegistroAuditoriaDetalleModel[] = [];
  registerDetailSelected: RegistroAuditoriaDetalleModel = new RegistroAuditoriaDetalleModel();
  
  hidestart: boolean = false;
  changecalification: number = 0;

  nombreBoton: string = "CALIFICAR";
  sinCalificado: boolean = true;

  accionCalificar: boolean = false;

  accionSelect: boolean = false;

  constructor(private fb: FormBuilder, 
              private service: TabsService, 
              private coockie: CookieService){ 
                this.objAuditar = JSON.parse(atob(this.coockie.get('objAuditar')));
                this.objUser = JSON.parse(atob(this.coockie.get('objUser')));
                this.objListCalificaciones = JSON.parse(localStorage.getItem('objListVariables')!);
              }

  ngOnInit(){  
    // this.accionSelect = true;
    this.setListadoDeVariablesCalificar();    
    //this.getCatalogCalifications();
  }

  setListadoDeVariablesCalificar(){
    this.objListCalificaciones.forEach((element:any = []) => {
      element.variables.forEach((variableC:any = []) => {       
        if(variableC.variableEncuesta){
          this.objlistado.push(variableC)
        }
      });
    });

    this.registerDetailSelected = this.objlistado[0];
    this.varibleSelected(this.registerDetailSelected);
  }

  getCatalogCalifications(){
    this.service.getCatalogCalifications(this.variableId).subscribe(Response => {
      this.listCatalogCalifica = Response;
      this.getCalificaciones(this.objAuditar.id.toString(),this.variableId.toString());


    },error => {
    
    })
  }

  //Aqui se cargar los items y las calificaciones (en caso de tener calificaciones).
  getCalificaciones(registrosAuditoriaId:any, variableId:any){ 
      this.service.getQualifications(registrosAuditoriaId,variableId).subscribe(Response => {
      this.listCalifications = Response;
      
      if(this.listCalifications.length > 0){
        this.observation = this.listCalifications[0].observacion        
        this.nombreBoton = 'ACTUALIZAR'
        this.accionCalificar = false;
      }

      //Si la variable no tiene calificaciones registradas, recorre los items a calificar y  arma listado de califiaciones 
      else{

        Swal.fire({
            title: "Error",
            text: "La variable no tiene parametrizado los items a calificar.",
            icon: "warning",
          });

        for(let i = 0; i <= this.listCatalogCalifica.length - 1; i++)
        {
          let calification: CalificacionModel = new CalificacionModel();
          calification.ipsId = this.ipsId
          calification.itemId = this.listCatalogCalifica[i].id;
          calification.calificacion = 0;
          calification.nombreItem = this.listCatalogCalifica[i].itemName;
          calification.registrosAuditoriaId = this.objAuditar.id;
          calification.registrosAuditoriaDetalleId = this.registerDetailSelected.registroAuditoriaDetalleId;
          calification.createdBy =  this.objUser.userId,
          calification.createdDate = moment.utc().format('YYYY-MM-DDTHH:mm:ss.SSS'),
          calification.modifyBy = this.objUser.userId,
          calification.modifyDate = moment.utc().format('YYYY-MM-DDTHH:mm:ss.SSS');
          calification.variableId = this.variableId;

          this.listCalifications.push(calification);
        }

        this.observation = ''
        this.disabledSave = true;        
        this.nombreBoton = 'CALIFICAR'
      }     
    },Error => {
     
    })
  }

  varibleSelected(e:RegistroAuditoriaDetalleModel){   
    this.registerDetailSelected = e;    
    this.hidestart = true;
    this.sinCalificado = false;
    const sleep = (milliseconds: any) => {
      return new Promise((resolve) => setTimeout(resolve, milliseconds));
    };

    sleep(200).then(() => {
      this.hidestart = false;
    });

    this.variableCalificar = e.registroAuditoriaDetalleId;
    this.ipsId = e.ipsId;
    this.disabledCalification = false;
    this.variableId = e.variableId;
    this.getCalificaciones(this.objAuditar.id.toString(),this.variableId.toString());
    console.log(e)
  }


  //Al hacer click en una estrella asigna calificacion al item corrrespondiente
  assignStartValue(e:any, itemToCalificate:number)  {   
    
    this.accionSelect = true; 
    //this.accionCalificar = true;
    this.sinCalificado = true;
    this.listCalifications.filter((x : any) => x.itemId == itemToCalificate)[0].calificacion = e;
  }

  //Valida que todos los items tengan calificacion y que exista una observacion para permitir guardar  this.sinCalificado
  validationSave()
  {
   if(this.accionSelect){
    if(this.listCalifications.filter((x : any) => x.calificacion == 0).length > 0 && !this.accionCalificar)
     {
       return false;
     }
     else if(this.accionCalificar)
       {      
         return false;
       }else
         {
           return true;
         }         
   }else{
     return false;
    }
         
    // if(this.listCalifications.filter((x : any) => x.calificacion == 0).length > 0 || this.observation == "" || this.observation ==  null)
    

  }
      
  //Request para guardar calificacion
  saveCalification(){    
    if(this.observation != ""){
      for (let index = 0; index < this.listCalifications.length; index++) {
        this.listCalifications[index].observacion = this.observation;
        this.listCalifications[index].modifyBy = this.objUser.userId;
        this.listCalifications[index].createdBy = this.objUser.userId;
        this.listCalifications[index].modifyDate = moment.utc().format('YYYY-MM-DDTHH:mm:ss.SSS');
        //this.listCalifications[index].registrosAuditoriaId = ;  

        if(this.listCalifications[index].createdBy == null)
        {
          this.listCalifications[index].createdBy = moment.utc().format('YYYY-MM-DDTHH:mm:ss.SSS');
        }
        if(this.listCalifications[index].createdBy == null)
        {
          this.listCalifications[index].createdBy = this.objUser.userId;
        }
       
      }
    }
    
    this.service.saveCalification(this.listCalifications).subscribe(Response => {
      Swal.fire({
        icon: 'success',
        text: 'Su calificación ha sido guardada con éxito',
        confirmButtonColor: '#a94785',
        confirmButtonText: 'ENTENDIDO',
      })     
    },error => {
     
    });
  }

}
