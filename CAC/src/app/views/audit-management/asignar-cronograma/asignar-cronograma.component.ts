import { SelectionModel } from '@angular/cdk/collections';
import { Component, OnInit, ViewChild } from '@angular/core';
import { MatSort } from '@angular/material/sort';
import { MatTableDataSource } from '@angular/material/table';
import { tabla2 } from 'src/utils/models/cronograma/cronograma';



const ELEMENT_DATA: tabla2[] = [
  {auditor: 'Lina Casas', registro: '0', entidades: '0', horas: '0', disponible: '0'},
  {auditor: 'Lina Casas', registro: '0', entidades: '0', horas: '0', disponible: '0'},
  {auditor: 'Lina Casas', registro: '0', entidades: '0', horas: '0', disponible: '0'},
  {auditor: 'Lina Casas', registro: '0', entidades: '0', horas: '0', disponible: '0'},
  {auditor: 'Lina Casas', registro: '0', entidades: '0', horas: '0', disponible: '0'}
];

@Component({
  selector: 'app-asignar-cronograma',
  templateUrl: './asignar-cronograma.component.html',
  styleUrls: ['./asignar-cronograma.component.scss']
})
export class AsignarCronogramaComponent implements OnInit {

  constructor() { }
  displayedColumns!: string[];  
  ELEMENT_DATA!: tabla2[];
  ELEMENT_DATABack!: tabla2[];  
  dataSource2: any;
  selection: any;
  loading: boolean = true;
  progreso:any;
  progresoDia: number = 0;
  diasTotales: number = 0;

  
 

  ngOnInit(): void {
    this.displayedColumns = ['auditordesignado','registros','entidades', 'horas','disponible'];
    this.dataSource2 = new MatTableDataSource(ELEMENT_DATA);   
    this.selection = new SelectionModel<tabla2>(true, []);
  }

  isAllSelected() {
    const numSelected = this.selection.selected.length;
    const numRows = this.dataSource2.data.length;
    return numSelected === numRows;
  }


  /** Selects all rows if they are not all selected; otherwise clear selection. */
  masterToggle() {
    if (this.isAllSelected()) {
      this.selection.clear();
      return;
    }

    this.selection.select(...this.dataSource2.data);
  }

  /** The label for the checkbox on the passed row */
  checkboxLabel(row?: tabla2): string {
    if (!row) {
      return `${this.isAllSelected() ? 'deselect' : 'select'} all`;
    }
    return `${this.selection.isSelected(row) ? 'deselect' : 'select'} row ${row.registro + 1}`;
  }

  @ViewChild(MatSort) sort!: MatSort;

  ngAfterViewInit() {
    this.dataSource2.sort = this.sort;
  }

}
