import { SelectionModel } from '@angular/cdk/collections';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute, Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { LoadingComponent } from 'src/app/layout/loading/loading.component';
import { datatableAsignacionBolsa, listaAuditores, objMedicionAsignacion, objpostUsuariosMedicion } from 'src/utils/models/asignacionbolsa/asignacionbolsamodels';
import { PaginationDto } from 'src/utils/models/cronograma/pagination';
import { objMedicion } from 'src/utils/models/medicion/medicion';
import { ManagementService } from '../services/management.service';


@Component({
  selector: 'app-asignacion-bolsa',
  templateUrl: './asignacion-bolsa.component.html',
  styleUrls: ['./asignacion-bolsa.component.scss']
})
export class AsignacionBolsaComponent implements OnInit {


  displayedColumns!: string[];  
  dataTable: datatableAsignacionBolsa[]=[];
  ELEMENT_DATABack!: datatableAsignacionBolsa[];  
  dataSource: any;
  selection: any;
  loading: boolean = true;
  progreso:any;
  progresoDia: number = 0;
  diasTotales: number = 0;
  primerNombre;
  objUser;
  objMedicionQuery: any;
  objMedicion:any = new objMedicionAsignacion();
  medicionSeleccionada:any;
  selectlistaAuditores: listaAuditores[] = [];

  // paginador
  pageNumber: any;
  intialPosition: any;
  itemsPerpagina: number = 50;
  sizeList: number[] = [50, 100, 150, 200];
  page: number = 1;

  pagination!: PaginationDto;
  totalRegister: number = 0;
  totalLiderRegister: number = 0;
  pager: any = {};

  form!: FormGroup;

  constructor(private coockie: CookieService,private router: Router,
    private route: ActivatedRoute,
    private services: ManagementService,
    public dialog: MatDialog,
    private formBuilder: FormBuilder) {  
            this.objUser = JSON.parse(atob(this.coockie.get('objUser')));
            this.primerNombre = this.objUser.userName.split('.');
            }

  ngOnInit(): void {  
    this.displayedColumns = ['btns','auditor','usuario','codigo', 'asignados','estado'];
    this.dataSource = new MatTableDataSource(this.dataTable);   
    this.selection = new SelectionModel<datatableAsignacionBolsa>(true, []);
    this.openDialogLoading(true);
    this.objMedicionQuery = this.route.snapshot.paramMap.get('objMedicion');
    this.objMedicionQuery =  JSON.parse(atob(this.objMedicionQuery?this.objMedicionQuery:''));
    this.medicionSeleccionada= this.objMedicionQuery.idMedicion;
    this.services.getMedicion(this.medicionSeleccionada).subscribe(Response => {
      this.objMedicion=Response as objMedicionAsignacion;
      this.objMedicion.estado=this.objMedicionQuery.estadoAuditoria;
    }); 
    this.services.getSelectAuditoresAsignacion(this.medicionSeleccionada.toString()).subscribe(Response => {
      this.selectlistaAuditores=Response as listaAuditores[];  
    });
     this.form = this.formBuilder.group({
      auditor:[''],
      buscar:[''],
     });
     this.getFilters();
     this.openDialogLoading(false);
  }

  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.data.length;
    return numSelected === numRows;
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

  getFilters(){
    //this.medicionSeleccionada.idMedicion
    const objpostUM : objpostUsuariosMedicion= {      
      pageNumber:0,
      maxRows: 100,
      medicionId : this.medicionSeleccionada ,
      auditorId:this.form.controls.auditor.value==''?['']:this.form.controls.auditor.value,
      keyWord:this.form.controls.buscar.value,
      
    } 

    this.services.getUsuariosBolsaMedicion(objpostUM).subscribe(Response => {
        this.dataTable=Response.data;
        this.dataSource = new MatTableDataSource(this.dataTable);   
     });
  
  }
  limpiarForm(){
    this.form.controls.auditor.setValue('');
    this.form.controls.buscar.setValue('');
    this.getFilters();
  }
  goPlaces(number: number) {
    if (number == 1) {
      this.router.navigateByUrl("/gestion-de-auditoria");
    }
  }

  btnReasignarBolsa(auditor: any, tipo : string) {
    const obj={auditor: auditor, idbolsa: this.objMedicionQuery ,tipo:tipo};
    var encoded = btoa(JSON.stringify(obj));
    this.router.navigateByUrl("/gestion-de-auditoria/reasignar-bolsa/" + encoded);
  }

}
