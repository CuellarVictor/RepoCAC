import { Component, Inject, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';
import { LoadingComponent } from 'src/app/layout/loading/loading.component';
import { ConfigService } from '../../service/config.service';

@Component({
  selector: 'app-lider-entidad-list',
  templateUrl: './lider-entidad-list.component.html',
  styleUrls: ['./lider-entidad-list.component.scss']
})
export class LiderEntidadListComponent implements OnInit {

  obj:any; 
  listAuditores: any;

  constructor(
    private services: ConfigService,
    public dialog: MatDialog, 
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {
    this.obj = data;
    this.getAuditoresEps();
   }

  ngOnInit(): void {
  }

  getAuditoresEps(){
      this.services.consultaAuditoresAsignacionLiderEntidad(this.obj).subscribe((Response) => {
        this.listAuditores = Response;
        console.log(this.listAuditores);

    // this.dataSource2 = new MatTableDataSource(this.dataTable);
    },error => {
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

}
