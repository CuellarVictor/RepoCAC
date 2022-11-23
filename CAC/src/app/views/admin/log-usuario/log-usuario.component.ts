import { Component, OnInit } from '@angular/core';
import { MatDialog } from '@angular/material/dialog'; 
import { Router  } from '@angular/router';
import { LoadingComponent } from 'src/app/layout/loading/loading.component';
import { GetPermisosServices } from 'src/app/services/get-permisos.services';
import { UserModel } from 'src/app/model/auth/usermodel';
import { AdministradorService } from '../administrador/services/administrador.service'; 
import { FormBuilder, FormGroup } from '@angular/forms';
import { PagerService } from '../../cronograma/services/page.service';
import { CookieService } from 'ngx-cookie-service';
import { RequestLog } from 'src/app/model/conf/requestLog.model';
import { ResponseLog } from 'src/app/model/conf/responseLog.model';


export interface usuario {
  Nombre: string; 
  Apellido: string; 
  Correo: string; 
  Rol: string; 
  Creacion: string; 
  Estado: string; 
}  



@Component({
  selector: 'app-log-usuario',
  templateUrl: './log-usuario.component.html',
  styleUrls: ['./log-usuario.component.scss']
})
export class LogUsuarioComponent implements OnInit {
 

  displayedColumns: string[] = ['processLogId', 'nombreActividad', 'observation', 'date'];





  dataSource: ResponseLog[] = [];
  totalRegister: number = 0;

  pageNumber: any;
  intialPosition: any;
  itemsPerpagina: number = 50;
  sizeList: number[] = [50, 100, 150, 200];
  page: number = 1;
  pager: any = {};
  objFiltro !: RequestLog ;
  form!: FormGroup;
  objUser: any;
  userSelected = new UserModel();
  constructor( 
    private formBuilder: FormBuilder,
    public dialog: MatDialog, 
    private adminService: AdministradorService,
    private paginatorService: PagerService,
    public permisos: GetPermisosServices) {
     
      const userSelectedEncoded = sessionStorage.getItem("userSelected");
      this.userSelected = JSON.parse(userSelectedEncoded || '');

      const userEncoded = sessionStorage.getItem("objUser");
      this.objUser = JSON.parse(userEncoded || '');
     }

  ngOnInit(): void {
    console.log(this.objUser)
    this.form = this.formBuilder.group({
      buscar:[''],
     });
     this.openDialogLoading(true);
     this.objFiltro = new RequestLog;
     this.getFiltros(1,50);
  }

  getFiltros(page : any, itemsPerpagina : any) {
    // this.openDialogLoading(true);
    if(page && itemsPerpagina){      
      this.itemsPerpagina = itemsPerpagina;
      this.page = page;
    }
    
    this.dataSource = [];
    this.objFiltro.user= this.userSelected.id;
    //this.objFiltro.user = 'BDD04FEF-C614-467F-91BF-260FD2BDF617';
    this.objFiltro.palabraClave = this.form.controls.buscar.value;
    this.objFiltro.maxRows = this.itemsPerpagina;
    this.objFiltro.pageNumber = this.page - 1;
    this.pageNumber = page;
    this.adminService.getLog(this.objFiltro).subscribe((Response) => {
      this.dataSource = Response.data as ResponseLog[];
      this.totalRegister = Response.noRegistrosTotalesFiltrado;
      this.setPage(this.page);      
    
     // this.dataSource2 = new MatTableDataSource(this.dataTable);
      this.openDialogLoading(false);
    },error => {
      this.openDialogLoading(false);
    });
  }
  openDialogLoading(loading: boolean): void {
    if (loading) {
      this.dialog.open(LoadingComponent, {
        //width: '300px',
        disableClose: true,
        data: {},
      });
    } else {
      this.dialog.closeAll();
    }
  }

    //seteo el paginador
    setPage(page: number) {
      this.pager = this.paginatorService.getPager(
        this.totalRegister,
        page,
        this.itemsPerpagina
      );
    }

}
