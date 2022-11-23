import { Component, ElementRef, Inject, ViewChild } from "@angular/core";
import { FormBuilder, FormGroup } from "@angular/forms";
import {
  MatDialog,
  MatDialogRef,
  MAT_DIALOG_DATA,
} from "@angular/material/dialog";
import { LoadingComponent } from "src/app/layout/loading/loading.component";
import { dataTableAuditorBolsas, listaAuditores, objpostReasignarBolsa } from "src/utils/models/asignacionbolsa/asignacionbolsamodels";
import Swal from "sweetalert2";
import { ReporteMedicion } from 'src/app/model/Util/enumerations.enum';
import { ManagementService } from "../services/management.service";

export interface DialogData {
  content: string;
  name: string;
  type: string;
}

@Component({
  selector: "app-modal-reasignar-descargar",
  templateUrl: "./modal-reasignar-descargar.component.html",
  styleUrls: ["./modal-reasignar-descargar.component.scss"],
})
export class ModalReasignarDescargarComponent {

  seleccion=0;
  form!: FormGroup;
  selectlistaAuditores: listaAuditores[] = [];
  medicionSeleccionada : any;
  datos!:dataTableAuditorBolsas[];
  selectedFile: any = null;
  url:string = "";
  allowLoadFile:boolean = false;
  base64file: string = "";
  urlArchivoError='';
  objetoFiltro!:objpostReasignarBolsa;
  base64Filtrado: string = "";
  base64Total: string = "";
  tipo='';
  constructor(
    public dialogRef: MatDialogRef<ModalReasignarDescargarComponent>,
    private fb: FormBuilder,
    public dialog: MatDialog,   
    private services: ManagementService,
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {

    this.objetoFiltro=data.obj;
    this.tipo=data.tipo;

  }
  
  ngOnInit(): void {
    

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
  @ViewChild('total') total !: ElementRef<HTMLElement>;
  btnDescargarTotal(){    
    let objlimpio:objpostReasignarBolsa={
      codigoEps:[''],
      estado:[''],
      fechaAsignacionFin:'',
      fechaAsignacionIni:'',
      idRadicado:[''],
      medicionId:this.tipo=='total'?[]:this.objetoFiltro.medicionId,       
      keyWord:'',
      maxRows:9999999,
      auditorId: this.objetoFiltro.auditorId,
      pageNumber:0,
      finalizados: this.tipo=='total'?true:false
    }   

    this.services.DescargaReporteReasignar(objlimpio,this.tipo=='total'?ReporteMedicion.ReasignacionTotalData:ReporteMedicion.ReasignacionBolsaData).subscribe(Response => {
      this.base64Total=Response;
      setTimeout(() => {
        let el: HTMLElement = this.total.nativeElement;
        el.click();;
        }, 500);   
    
    });  

  }
  @ViewChild('filtrado') filtrado !: ElementRef<HTMLElement>;
  btnDescargarfiltrado(){
    this.objetoFiltro.maxRows=9999999;
    this.services.DescargaReporteReasignar(this.objetoFiltro,this.tipo=='total'?ReporteMedicion.ReasignacionTotalData:ReporteMedicion.ReasignacionBolsaData).subscribe(Response => {
      this.base64Filtrado=Response;
      setTimeout(() => {
        let el: HTMLElement = this.filtrado.nativeElement;
       el.click();
        }, 500);  
      
    });
   
  }

 
}
