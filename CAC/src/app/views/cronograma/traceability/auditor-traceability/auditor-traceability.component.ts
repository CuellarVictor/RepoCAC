import { Component, Inject, OnInit, ViewChild } from "@angular/core";
import { MatTableDataSource } from "@angular/material/table";
import { MatPaginator } from "@angular/material/paginator";
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";
import { CronogramaService } from "../../services/cronograma.service";
import { objLogCronograma } from "src/utils/models/cronograma/cronograma";
import { PagerService } from "../../services/page.service";
import { UsuarioLiderModel } from "src/app/model/registroauditoria/usuariolider.model";
import { RequestConsultaLog } from "src/app/model/registroauditorialog/requestconsulta.model";
import Swal from 'sweetalert2';
import { messageString } from "src/app/model/Util/enumerations.enum";
import { ResponseConsultaLog } from "src/app/model/registroauditorialog/responseconsulta.model";

@Component({
  selector: "app-auditor-traceability",
  templateUrl: "./auditor-traceability.component.html",
  styleUrls: ["./auditor-traceability.component.scss"],
})
export class AuditorTraceabilityComponent implements OnInit {
  
  displayedColumns: string[] = [
    "id",
    "observacion",
    "auditor",
    "fecha",
    "hora"
  ];
  dataSource = new MatTableDataSource();
  @ViewChild(MatPaginator, { static: true })
  paginator!: MatPaginator;
  idAuditor = "";
  _messageString: messageString = new messageString();
  ELEMENT_DATA!: any;

  //Pagination
  pageNumber:number =  1;
  pageSize: number = 50;
  ObjectPagination: any = null;
  pager: any = {};
  totalRegister: number = 0;

  sizeList: number[] = [50, 100, 150, 200];
  listaAuditor: UsuarioLiderModel[] = [];
  auditorSelected: string = "";
  listMedicion : any [] = [];

  requestObject: RequestConsultaLog = new RequestConsultaLog();
  responseObject: ResponseConsultaLog[] = [];

  urlReporte= '';

  public loading: boolean = true;

  constructor(
    public _dialog: MatDialog,
    private cronogramaService: CronogramaService,
    private pagerService: PagerService,
    private _dialogRef: MatDialogRef<AuditorTraceabilityComponent>,

    @Inject(MAT_DIALOG_DATA) public model: any
  ) {
    
    //Get Date form father component
    if (this.model !== null) {
      this.listaAuditor = model.listaauditores;
      this.auditorSelected = model.selected;
      this.listMedicion= model.listMedicion;
      // Build Ids list
      if(this.listaAuditor.length > 0 && this.auditorSelected == "all")
      {
        this.listaAuditor.forEach((user: any) => {
          this.requestObject.IdAuditores.push(user.idAuditor);
        }); 
        this.consultaLogAccion(1, this.pageSize);
      }
      
      //Build List with only Id
      if(this.auditorSelected != "all")
      {
        this.requestObject.IdAuditores.push(this.auditorSelected);
        this.consultaLogAccion(1, this.pageSize);
      }
    }
  }

  ngOnInit(): void {

  }


  //Request for get actions log
  consultaLogAccion(pageNumber: number, pageSize: number)
  {
    if(this.requestObject.FechaFinal != null && this.requestObject.FechaInicial != null)
    {
      this.pageSize = pageSize;
    this.pageNumber = pageNumber;
    this.requestObject.MaxRows = this.pageSize;
    this.requestObject.pageNumber = this.pageNumber;
    this.loading = true;
    this.cronogramaService.consultaLogAccion(this.requestObject).subscribe(data => {
      this.responseObject = data;
      this.ELEMENT_DATA = data;
      this.dataSource.data = data; 
      
      if(this.responseObject.length > 0)
      {
        this.totalRegister = this.responseObject[0].countQuery;
        this.setPage(this.pageNumber);        
      }

      this.loading = false;
    }, error => {
      this.loading = true;
      this.openModalError();  
    })
    }
    
  }


  setPage(page: number) {
    // get pager object from service
    this.pager = this.pagerService.getPager(this.totalRegister, page, this.pageSize);
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

  close() {
    this._dialogRef.close();
  }

  cleanFilters()
  {
    this.requestObject.FechaFinal = '';
    this.requestObject.FechaInicial = '';
    this.requestObject.ParametroBusqueda = '';
    this.requestObject.medicionId = 0;
    this.consultaLogAccion(1, this.pageSize);
  }


  getReportLogAccion()
  {
    if(this.requestObject.FechaFinal != null && this.requestObject.FechaInicial != null)
    {
 
    this.requestObject.MaxRows = 500000;
    this.requestObject.pageNumber = 1;
    this.loading = true;
    this.cronogramaService.getUrlReporteLogAccion(this.requestObject).subscribe(data => {
     
      this.cronogramaService.getFileReporteLogAccion(data).subscribe(response => {
        if(response){
          this.downloadBase64File(response.fileContents);
        }
     
      })

      this.loading = false;
    }, error => {
      this.loading = true;
      this.openModalError();  
    })
    }
    
  }
  downloadBase64File(base64String:string)
  {
    const source = `data:application/pdf;base64,${base64String}`;
    const link = document.createElement("a");
    link.href = source;
    link.download = `reporte.log.csv`
    link.click();
  }

}
