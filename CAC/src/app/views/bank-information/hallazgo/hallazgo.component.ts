import { Component, Inject, OnInit, ViewChild } from '@angular/core';
import { MatTableDataSource } from "@angular/material/table";
import { MatPaginator } from "@angular/material/paginator";
import { MatDialogRef, MAT_DIALOG_DATA } from "@angular/material/dialog";


@Component({
  selector: 'app-hallazgo',
  templateUrl: './hallazgo.component.html',
  styleUrls: ['./hallazgo.component.scss']
})
export class HallazgoComponent implements OnInit {


  inputdata :any;

  displayedColumns: string[] = [
    "calificacion",
    "observacion",
    "estadohallazgo"
  ];

  dataSource = new MatTableDataSource();
  @ViewChild(MatPaginator, { static: true })

  ELEMENT_DATA!: any;

  constructor(@Inject(MAT_DIALOG_DATA) public data:any,
  public _dialogRef: MatDialogRef<HallazgoComponent>,) {
    this.inputdata = data;
    
   }

  ngOnInit(): void {
  }

  close() {
    this._dialogRef.close();
  }
  

}
