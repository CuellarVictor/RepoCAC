import { SelectionModel } from '@angular/cdk/collections';
import { Component, OnInit } from '@angular/core';
import { FormBuilder, FormGroup } from '@angular/forms';
import { MatDialog } from '@angular/material/dialog';
import { MatTableDataSource } from '@angular/material/table';
import { ActivatedRoute, Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { LoadingComponent } from 'src/app/layout/loading/loading.component';
import { auditor, dataTableAuditorBolsas, listaAuditores, objpostReasignarBolsa, objpostUsuariosMedicion } from 'src/utils/models/asignacionbolsa/asignacionbolsamodels';
import { filtro } from 'src/utils/models/cronograma/cronograma';
import { PaginationDto } from 'src/utils/models/cronograma/pagination';
import { objMedicion } from 'src/utils/models/medicion/medicion';
import { PagerService } from '../../cronograma/services/page.service';
import { ModalReasignarDescargarComponent } from '../modal-reasignar-descargar/modal-reasignar-descargar.component';
import { ModalReasignarComponent } from '../modal-reasignar/modal-reasignar.component';
import { ManagementService } from '../services/management.service';
@Component({
  selector: 'app-reasignar-bolsa',
  templateUrl: './reasignar-bolsa.component.html',
  styleUrls: ['./reasignar-bolsa.component.scss']
})
export class ReasignarBolsaComponent implements OnInit {


  displayedColumns!: string[];  
  dataTable: dataTableAuditorBolsas[]=[];
  ELEMENT_DATABack!: dataTableAuditorBolsas[];  
  dataSource: any;
  selection: any;
  loading: boolean = true;
  progreso:any;
  progresoDia: number = 0;
  diasTotales: number = 0;
  primerNombre;
  objUser;
  objMedicion:objMedicion = new objMedicion();
  medicionSeleccionada:any;
  selectlistaAuditores: listaAuditores[] = [];
  filtros!: filtro[];
  // paginador
  page: number = 0;
  pageNumber= 1;
  intialPosition: any;
  itemsPerpagina: number = 50;
  sizeList: number[] = [50, 100, 150, 200];
  backmedicion:any;

buscador='';

  pagination!: PaginationDto;
  totalRegister: number = 0;
  totalLiderRegister: number = 0;
  pager: any = {};
  dataAuditor!: auditor ;
  form!: FormGroup;
  codigoAuditor:any={codigo:''};
  nombreAuditor!:any;
  tipo='';
  constructor(private coockie: CookieService,private router: Router,
    private route: ActivatedRoute,
    private services: ManagementService,
    public dialog: MatDialog,
    private paginatorService: PagerService,
    private formBuilder: FormBuilder) {  
            this.objUser = JSON.parse(atob(this.coockie.get('objUser')));
            this.primerNombre = this.objUser.userName.split('.');
            }

  ngOnInit(): void {  
   
    this.dataSource = new MatTableDataSource(this.dataTable);   
    this.selection = new SelectionModel<dataTableAuditorBolsas>(true, []);
    this.openDialogLoading(true);
    let obj= this.route.snapshot.paramMap.get('obj');    
    let objDecode = JSON.parse(atob(obj?obj:'')) ;
    this.medicionSeleccionada= objDecode.idbolsa.idMedicion;
    this.backmedicion=objDecode.idbolsa;
    this.dataAuditor=objDecode.auditor;
    this.tipo= objDecode.tipo;
    if(this.tipo=='total'){
      this.displayedColumns = ['btns','idRegistro','estado','entidad', 'asignacion','enfermedadMadre','bolsaMedicion'];
   
      this.medicionSeleccionada='';
    }else{
      this.displayedColumns = ['btns','idRegistro','estado','entidad', 'asignacion','codigo','bolsaMedicion'];
      this.services.getMedicion(this.medicionSeleccionada).subscribe(Response => {
        this.objMedicion=Response as objMedicion;  
        this.objMedicion.estado=this.backmedicion.estadoAuditoria;      
      });  
    }
   
    
    this.services.getSelectAuditoresAsignacion(this.medicionSeleccionada.toString()).subscribe(Response => {
      this.selectlistaAuditores=Response as listaAuditores[]; 
      
      this.codigoAuditor= this.selectlistaAuditores.find(e=> e.codigo==this.dataAuditor.auditorCodigo); 
         
      let objfiltro={idAuditor:this.codigoAuditor?.id,medicionId:this.medicionSeleccionada.toString()}
      this.services.​getRegistrosAuditoriaXBolsaMedicionFiltro(objfiltro).subscribe(Response => {  
        this.filtros=Response as filtro[];  
        this.getFilters();
      });
    });
     this.form = this.formBuilder.group({
      estado:[''],
      codigoEps:[''],
      buscar:[''],
      Startdate:[''],
      Enddate:[''],
      bolsamedicion:[''],
     });
     
     this.openDialogLoading(false);
  }

  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource.data.length;
    return numSelected === numRows;
  }
  checkboxLabel(row?: dataTableAuditorBolsas): string {
    if (!row) {
      return `${this.isAllSelected() ? 'deselect' : 'select'} all`;
    }    
    return `${this.selection.isSelected(row) ? 'deselect' : 'select'} row ${row.idRadicado + 1}`;
  }
  masterToggle() {
    if (this.isAllSelected()) {
      this.selection.clear();
      return;
    }
    this.selection.select(...this.dataSource.data);
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

  setPage(page: number) {
    this.pager = this.paginatorService.getPager(
      this.totalRegister,
      page,
      this.itemsPerpagina
    );
  }


  filtroPaginador(page: number,itemsPerpagina: number){
    this.itemsPerpagina = itemsPerpagina;
    this.pageNumber = page;
    this.page = page-1;
    this.getFilters();
    this.setPage(this.page);    
  }

  getFilters(){
    this.selection.clear();
   
    //this.medicionSeleccionada.idMedicion  
    this.services.​getRegistrosAuditoriaXBolsaMedicion(this.buildObjFilter(false)).subscribe(Response => {
        this.dataTable=Response.data;
        this.dataSource = new MatTableDataSource(this.dataTable);  
        this.totalRegister = Response.noRegistrosTotalesFiltrado; 
     });
     
  }

  buildObjFilter(select:boolean){
    let bolsa;
    if(this.tipo=='bolsa'){
      bolsa=[this.medicionSeleccionada];
    }else if(this.tipo=='total'){
      if(this.form.controls.bolsamedicion.value==''){
        bolsa=[];
      }else{
        bolsa=[];
        const aux=this.form.controls.bolsamedicion.value as string[];
        aux.map(e  =>bolsa.push(Number(e)));        
      }
     // bolsa=this.form.controls.bolsamedicion.value==''?[]:this.form.controls.bolsamedicion.value;
    }  
    let dataSeleccion:string[]=[] ;
    if(select){
      if(this.selection.selected.length>0){
        const aux= this.selection.selected as dataTableAuditorBolsas[];
        aux.map(e =>  dataSeleccion.push(e.idRadicado.toString()));      
      }else{
        dataSeleccion=[''];
      }
    }else{
      dataSeleccion=[''];
    }
    let final=false;
    if(this.tipo=='total'){
      final=true;
    }
    const objpostUM : objpostReasignarBolsa= {      
      pageNumber:this.page,
      maxRows: this.itemsPerpagina,
      medicionId : bolsa as [] ,
      fechaAsignacionIni:this.form.controls.Startdate.value,
      fechaAsignacionFin:this.form.controls.Enddate.value,
      auditorId:this.codigoAuditor?.id,
      estado: this.form.controls.estado.value==''?['']:this.form.controls.estado.value,
      codigoEps: this.form.controls.codigoEps.value==''?['']:this.form.controls.codigoEps.value,
      keyWord:this.form.controls.buscar.value,     
      idRadicado: dataSeleccion,
      finalizados: final
    } 
    return objpostUM;
  }
  limpiarForm(){   
    this.form.controls.estado.setValue('');
    this.form.controls.codigoEps.setValue('');
    this.form.controls.buscar.setValue('');
    this.form.controls.Startdate.setValue('');
    this.form.controls.Enddate.setValue('');
    this.form.controls.Enddate.setValue('');
    this.form.controls.bolsamedicion.setValue('');
    this.itemsPerpagina=50;
    this.page=0;
    this.pageNumber=1;
    this.getFilters();
  }
  goPlaces(number: number) {
    if (number == 1) {
      this.router.navigateByUrl("/gestion-de-auditoria");
    }else if(number== 2){
      this.router.navigateByUrl("/gestion-de-auditoria/asignar-bolsa/"+btoa(JSON.stringify(this.backmedicion)));
    }
  }

  getSelect(id:string){
    if(this.filtros){
      return  this.filtros.find(e=>e.nombreFiltro===id)?.detalle;
    }else{
      return null;
    }
 
  }
  openModalReasignar()
  {
    const obj={obj: this.buildObjFilter(true),idAuditor: this.codigoAuditor.id ,idMedicion: this.medicionSeleccionada,datos:this.selection.selected};
    const dialogRef = this.dialog.open(ModalReasignarComponent, {
      data: obj,
      width: '500px',
    }).afterClosed().subscribe((res) => {      
          this.getFilters();
    }); 
    ;
  }

  openModalReasignarDescarga()
  { 

    const obj={obj: this.buildObjFilter(true),tipo:this.tipo};
    const dialogRef = this.dialog.open(ModalReasignarDescargarComponent, {
      data: obj,
      width: '500px',
    }).afterClosed().subscribe((res) => {      
 
    }); 
    ;
  }
 habilitarBtnReasignar() : boolean{
  
   if(this.selection.selected.length>0){
     if(this.tipo=='bolsa'){
       if(this.objMedicion.estado.toString()=='Finalizada'){
        return true;
       }else{
        return false;
     }    
   }
   return false;
  }
  return true;
}
}
