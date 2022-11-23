import { ThisReceiver } from '@angular/compiler';
import { THIS_EXPR } from '@angular/compiler/src/output/output_ast';
import { REFERENCE_PREFIX } from '@angular/compiler/src/render3/view/util';
import { Component, Input, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { ActivatedRoute, Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { LoadingComponent } from 'src/app/layout/loading/loading.component';
import { UserModel } from 'src/app/model/auth/usermodel';
import { enumCatalog, enumRoles, messageString } from 'src/app/model/Util/enumerations.enum';
import { AdministradorService } from 'src/app/views/admin/administrador/services/administrador.service'; 
import { environment } from 'src/environments/environment';
import Swal from 'sweetalert2';
import { VariableDetailsService } from '../../audit-management/variable-details/services/variable-details.service';
import { ConfigService } from '../../config/service/config.service';

@Component({
  selector: 'app-usuario',
  templateUrl: './usuario.component.html',
  styleUrls: ['./usuario.component.scss']
})
export class UsuarioComponent implements OnInit {
  userForm!: FormGroup;
  
  cargando: boolean = false;
  show: any = true;
  MensajeError: any;
  selectedRol: any = ""; 

  action: string = "";
  nextUserCode: string = "";

  enfermedadLiderOk: boolean = false;
  editedForm: boolean = false;
  roles : any = [];  

  enfermedades : any = [];  
  itemsEstado: any = []; 

  userSelected : UserModel = new UserModel();
  userSelectedEncoded: any;

  _enumRoles: any = enumRoles;
  _messageString: messageString = new messageString();   
  
  //user
  userEncoded: any;
  objUser: any;
  esLider = false;
  constructor(
    private formBuilder: FormBuilder, 
    public activatedRoute: ActivatedRoute, 
    public dialog: MatDialog, 
    private adminService: AdministradorService, 
    private rolservice: ConfigService,
    private serviceVariableDetails: VariableDetailsService,
    private router: Router) {

      this.userSelectedEncoded = sessionStorage.getItem("userSelected");
      this.userSelected = JSON.parse(this.userSelectedEncoded);
      this.userEncoded = sessionStorage.getItem("objUser");
      this.objUser = JSON.parse(this.userEncoded);
 

    }

  ngOnInit(): void {  
    this.openDialogLoading(true); 
    this.getItemsByCatalogId();
    this.getEnfermedades();  
    this.getRoles();  

    var regex = /^[a-zA-Z\s]+$/;


    //Build Form
    this.userForm = this.formBuilder.group({
      nombres: [this.userSelected.nombre, [Validators.required, Validators.pattern(regex)]],
      apellidos: [this.userSelected.apellidos, [Validators.required,Validators.pattern(regex)]],
      telefono: [this.userSelected.telefono, [Validators.required]],
      usuario: [this.userSelected.usuario, [Validators.required, Validators.pattern('^([a-zA-Z0-9._-]+@[a-zA-Z0-9-]+([.]+[a-zA-Z]{2,6}){1,3})')]], 
      correo: [this.userSelected.email, [Validators.required, Validators.pattern('^([a-zA-Z0-9._-]+@[a-zA-Z0-9-]+([.]+[a-zA-Z]{2,6}){1,3})')]], 
      contrasena: [this.userSelected.password ],
      rol: [this.userSelected.rolId.toString(), [Validators.required]],
      codigo: [this.userSelected.codigo],
      estado: [this.userSelected.estado , [Validators.required]],
      enfermedades: [this.userSelected.usuariosxEnfermedad], 
    }); 
    this.validarLider();
  }


  //Request Items
  getItemsByCatalogId() {
    this.serviceVariableDetails.getItemsByCatalogId(enumCatalog.EstadoUsuario).subscribe((data) => {
      this.itemsEstado = data;

      if(this.userSelected.id == '')
      {
        this.userSelected.estado = this.itemsEstado[0].id; 
      }

    },
    (error) => {
     // this.openModalError();
    });

  }

  changeSelectUserRol(e : any){     
    if(e.value == "2"){   
      this.enfermedadLiderOk = true;
    }else{ 
      this.enfermedadLiderOk = false;
    }
  }   
    
  async getRoles(){ 
    await this.rolservice.getAdminRoles().subscribe(
      (Response) => {      
        this.roles = Response;
        this.validarLider();
      },
      (error) => { 
        this.openMessage('error', this._messageString.ErrorMessage, 'Error');
      }
    );
  } 
  
  validarLider(){
    this.userSelected.rolId = Number(this.userForm.controls.rol.value);
    this.esLider=this.roles.find((e : any) => e.id ==this.userSelected.rolId).esLider;
  }
  
  async getEnfermedades(){ 
    await this.adminService.getEnfermedades().subscribe(
      (Response) => {      
        this.enfermedades = Response; 
        this.openDialogLoading(false); 
      },
      (error) => { 
        this.openMessage('error', this._messageString.ErrorMessage, 'Error');
        this.openDialogLoading(false); 
      }
    );
  } 

  
  save(){      

    if(!this.userForm.valid)
    {
      this.openMessage('warning', this._messageString.ObligatoryForm, 'Alerta');
    }
    else if(this.userSelected.id == '' && (this.userSelected.password == '' || this.userSelected.password == null))
    {
      this.openMessage('warning', this._messageString.ObligatoryPassword, 'Alerta');
    }
    else if(this.userSelected.rolId == this._enumRoles.Lider && this.userSelected.usuariosxEnfermedad.length == 0)
    {
      this.openMessage('warning', this._messageString.ObligatoryAssingCobertura, 'Alerta');
    }
    else
    {
      this.openDialogLoading(true); 
      this.userSelected.modifyBy = this.objUser.userId;
      this.adminService.upsertUser(this.userSelected).subscribe(
        (Response) => {      
          this.openMessage('success', this._messageString.SuccessMessage, 'Correcto');
          this.router.navigate(['/admin']);
          this.openDialogLoading(false); 
        },
        (error) => { 
          this.openMessage('error', this._messageString.ErrorMessage, 'Error');
          this.openDialogLoading(false); 
        }
      );
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

  //Show Modal error
  openModalError()
  {
    Swal.fire({
      title: 'Error',
      text: this._messageString.ErrorMessage,
      icon: 'error'
    }) 
  }




  openDialogLoading(loading: boolean): void {
    if (loading) {
      this.dialog.open(LoadingComponent, { 
        disableClose: true,
        data: {},
      });
    } else {
      this.dialog.closeAll();
    }
  }

}
