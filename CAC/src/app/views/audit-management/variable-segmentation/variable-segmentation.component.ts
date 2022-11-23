import { Component, OnInit } from '@angular/core';
import {CdkDragDrop, moveItemInArray, transferArrayItem} from '@angular/cdk/drag-drop';
import { Router } from '@angular/router';
import { DialogComponent } from '../dialog/dialog.component';
import { MatDialog } from '@angular/material/dialog';

@Component({
  selector: 'app-variable-segmentation',
  templateUrl: './variable-segmentation.component.html',
  styleUrls: ['./variable-segmentation.component.scss']
})
export class VariableSegmentationComponent implements OnInit {

  constructor(private router: Router,public dialog: MatDialog) { }

  listas : any = [];
  activeNumIndex: number = 0;
  activeAmtIndex: number = 0;

  name: string = '';
  content: string = '';
  type: string = '';
  openDialogEliminar(): void {
    this.name = 'Está seguro que desea eliminar el grupo de variables';
    this.content = 'Esta acción no podrá ser revertida';
    this.type = 'eliminar';
    const dialogRef = this.dialog.open(DialogComponent, {
      width: '400px',
      data: {name: this.name, content: this.content, type: this.type }
    });

    dialogRef.afterClosed().subscribe(result => {

      if(result){
        this.openDialogMover();
      }    
    });
  }

  openDialogMover(): void {
    this.name = 'Seleccione el grupo destino de las variables ';
    this.content = 'Las variables seleccionadas migrarán al grupo seleccionado a continuación';
    this.type = 'moverSegmento';
    const dialogRef = this.dialog.open(DialogComponent, {
      width: '400px',
      data: {name: this.name, content: this.content, type: this.type }
    });

    dialogRef.afterClosed().subscribe(result => {
      
    });
  }


  ngOnInit(): void {
    this.listas = [
      { number: 'Glosas', amount: ['VAR00', 'VAR01', 'VAR02', 'VAR03'] }, 
      { number: 'Demografico', amount: ['VAR04', 'VAR05', 'VAR06', 'VAR07'] }, 
      { number: 'Tratamiento', amount: ['VAR08', 'VAR09', 'VAR10', 'VAR11']}     
      ];

  }



  enter(i : number) {
    this.activeNumIndex = i;

  }

  drop(event: CdkDragDrop<string[]>) {
    if (event.previousContainer === event.container) {
      moveItemInArray(event.container.data, event.previousIndex, event.currentIndex);
    } else {
      transferArrayItem(event.previousContainer.data,
                        event.container.data,
                        event.previousIndex,
                        event.currentIndex);
    }
  }

  btnVariable () {
    this.router.navigateByUrl('/gestion-de-auditoria/variable');
  };

}
