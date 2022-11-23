import {Component, Inject} from '@angular/core';
import {MatDialog, MatDialogRef, MAT_DIALOG_DATA} from '@angular/material/dialog';


export interface DialogData {
  content: string;
  name: string;
  type: string;
}
export interface data {
  id: number;
  value: string; 
}


const variable: data[] = [

  {id: 2, value: 'Cancer Colon Antiguos'},
  {id: 3, value: 'Cancer Piel Nuevos'},
  {id: 4, value: 'Cancer Piel Antiguos'},
];

const variablesegmento: data[] = [ 
  {id: 2, value: 'Demografico'},
  {id: 3, value: 'Tratamiento'},

];


@Component({
  selector: 'app-dialog',
  templateUrl: './dialog.component.html',
  styleUrls: ['./dialog.component.scss']
})



export class DialogComponent {

  constructor(
    public dialogRef: MatDialogRef<DialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: DialogData) {}

    type_variable : data[] = variable;
    type_segmento : data[] = variablesegmento;
  onNoClick(): void {
    this.dialogRef.close();
  }

}