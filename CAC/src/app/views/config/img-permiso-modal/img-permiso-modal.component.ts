import { Component, Inject, OnInit } from '@angular/core';
import { MatDialog, MatDialogRef, MAT_DIALOG_DATA } from '@angular/material/dialog';

@Component({
  selector: 'app-img-permiso-modal',
  templateUrl: './img-permiso-modal.component.html',
  styleUrls: ['./img-permiso-modal.component.scss']
})
export class ImgPermisoModalComponent implements OnInit {

  constructor(
    public dialogRef: MatDialogRef<ImgPermisoModalComponent>,   
    public dialog: MatDialog, 
    @Inject(MAT_DIALOG_DATA) public data: any
  ) {   

  }
  ngOnInit(): void {
  }
  close(){
    this.dialog.closeAll();
  }
}
