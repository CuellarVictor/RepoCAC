import { ConstantPool } from '@angular/compiler';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatDialog } from '@angular/material/dialog'; 
import { MatPaginator } from '@angular/material/paginator';
import { MatTableDataSource } from '@angular/material/table';
import { Router, RouterLink } from '@angular/router';
import { LoadingComponent } from 'src/app/layout/loading/loading.component';
import { GetPermisosServices } from 'src/app/services/get-permisos.services';
import { UserModel } from 'src/app/model/auth/usermodel';
import { messageString } from 'src/app/model/Util/enumerations.enum';
import Swal from 'sweetalert2';
import { AuditorTraceabilityComponent } from '../../cronograma/traceability/auditor-traceability/auditor-traceability.component';
import { AdministradorService } from './services/administrador.service'; 


export interface usuario {
  Nombre: string; 
  Apellido: string; 
  Correo: string; 
  Rol: string; 
  Creacion: string; 
  Estado: string; 
}  

@Component({
  selector: 'app-administrador',
  templateUrl: './administrador.component.html',
  styleUrls: ['./administrador.component.scss']
})
export class AdministradorComponent implements OnInit {
   
  users : UserModel[] = []; 
  userSelected : UserModel = new UserModel();
  panelEnfermedadesMadre: any[] = [];

  displayedColumns!: string[];
  searchText: any = "";  

  enfermedadesMadre: any = [];
  mediciones: any = []
  usuariosXmedicion: any = [];

  panelFormated: any[] = []; 
  _messageString: messageString = new messageString();
  
  dataScource = new MatTableDataSource();
  @ViewChild(MatPaginator, { static: true })
  paginator!: MatPaginator;

  //user
  userEncoded: any;
  objUser: any;

  constructor(
    private adminService: AdministradorService,
    public dialog: MatDialog,
    private router: Router,
    public permisos: GetPermisosServices
    ) { }

  ngOnInit(): void {

    this.userEncoded = sessionStorage.getItem("objUser");
    this.objUser = JSON.parse(this.userEncoded);


    this.displayedColumns = [
      "Codigo",
      "Nombre",
      "Apellido",
      "Correo",
      "Rol",
      "Creación",
      "Estado"
    ]; 
    
    this.dataScource.paginator = this.paginator;

    // setTimeout(() => { 
    //   this.getGetPanelEnfermedadesMadre();
    // }, 1500);

    this.getUsers();
  }

  ngAfterViewInit(): void {}

  goToNewUser(){  
    this.userSelected = new UserModel();
    sessionStorage.setItem("userSelected", JSON.stringify(this.userSelected));
    this.router.navigateByUrl('/usuario');
  }    

  getGetPanelEnfermedadesMadre(){
    this.openDialogLoading(true);
    this.adminService.getPanelEnfermedadesMadre().subscribe(
      (Response) => {      
        this.panelEnfermedadesMadre = Response;  
        this.openDialogLoading(false);
      },
      (error) => {
        this.openDialogLoading(false);
        this.openMessage('error', this._messageString.ErrorMessage, 'Error');
      }
    );
  }     

  //obtenemos usuarios
  getUsers(){
    this.openDialogLoading(true);
    this.adminService.getUsers().subscribe(
      (Response) => {  
        console.log(Response);  
        this.users = Response; 
        //preparemos enfermedades de lider
        this.users.forEach((user: any) => {
          if(user.Enfermedades){
            user.Enfermedades = user.Enfermedades.replaceAll('<enfermedades>','').replaceAll('</enfermedades>', ',');  
            user.Enfermedades = user.Enfermedades.split(',')
          }
        });  
        this.dataScource.data = this.users; 
        this.openDialogLoading(false);
      },
      (error) => {
        this.openDialogLoading(false);
        this.openMessage('error', this._messageString.ErrorMessage, 'Error');
      }
    );
  }   

  applyFilter(filterValue: any) {   
    this.dataScource.filter = filterValue.trim().toLowerCase();;
  }  
  
  EditarUsuario(usuario: UserModel){ 
    this.userSelected = usuario;
    sessionStorage.setItem("userSelected", JSON.stringify(this.userSelected));
    this.router.navigateByUrl("/usuario"); 
  }  

  OpenLogUsuario(usuario: UserModel){
    this.userSelected = usuario;
    sessionStorage.setItem("userSelected", JSON.stringify(this.userSelected));
    this.router.navigateByUrl("/admin/log_usuario"); 
  }
    
  
  PopUpEliminar(user : UserModel){   
    this.userSelected = user;
    this.userSelected.modifyBy = this.objUser.userId;
    Swal.fire({
      title: "¿Está seguro que desea eliminar el usuario " + user.usuario +  "?",
      text: 'La eliminación no es reversible.',
      icon: "warning",
      showCancelButton: true,
      cancelButtonText: "CANCELAR",
      cancelButtonColor: 'lightgray',
      confirmButtonColor: '#a94785',  
      confirmButtonText: `OK`,
    }).then((res) => {

      if (res.value) {
        this.EliminarUsuario();
      }

    });
  }

  EliminarUsuario(){ 
    this.openDialogLoading(true); 
    
    this.userSelected.enable = false;
    this.adminService.upsertUser(this.userSelected).subscribe(
      (Response) => {  
        this.openDialogLoading(false);  
        this.openMessage('success', this._messageString.SuccessMessage, 'Correcto');
        
        this.getUsers();
      },
      (error) => { 
        this.openDialogLoading(false);
        this.openMessage('error', this._messageString.ErrorMessage, 'Error');
      }
    );
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

  openLog(idAuditor : string) { 
    const dialogRef = this.dialog.open(AuditorTraceabilityComponent, { 
      width: '1000px', 
      data: idAuditor, 
    }); 
    dialogRef.afterClosed().subscribe((res) => { 
    }); 
  }

}
  